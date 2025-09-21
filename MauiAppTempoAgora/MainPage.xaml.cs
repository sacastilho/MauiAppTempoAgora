using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Threading.Tasks;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

     

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null) 
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                            $"Longitude: {t.lon} \n" +
                            $"Descrição: {t.description} \n" +
                            $"Nacer do Sol: {t.sunrise} \n" +
                            $"Por do Sol: {t.sunset} \n" +
                            $"Temperatura máxima: {t.temp_max} \n" +
                            $"Temperatura mínima: {t.temp_min} \n" +
                            $"Valocidade do Vento: {t.speed} \n" +
                            $"Visibilidade: {t.visibility} \n";
                        
                        lbl_res.Text = dados_previsao;

                    } else
                    {
                        lbl_res.Text = "Cidade não encontrada";
                    }

                } else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }

            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Erro", "Sem conexão com a internet.", "OK");
            }

            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }

        }
    }

}
