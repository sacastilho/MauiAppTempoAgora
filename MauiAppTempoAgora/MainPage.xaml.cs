using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System;
using System.Threading.Tasks;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

     

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
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

                        string mapa = $"https://embed.windy.com/embed.html?" +
                                      $"type=map&location=coordinates&metricRain=default&metricTemp=default" +
                                      $"&metricWind=default&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                                      $"&lat={t.lat.ToString().Replace(",", ".")}&lon={t.lon.ToString().Replace(",", ".")}";

                        wv_mapa.Source = mapa;


                        Debug.WriteLine(mapa);


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

        private async void Button_Clicked_localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = 
                    new GeolocationRequest(
                        GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10)
                    );

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null) 
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude} \n";

                    lbl_coords.Text = local_disp;

                    // pega o nome da cidade que está nas cordenadas
                    Getcidade(local.Latitude, local.Longitude);
                }

            } catch(FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não suporta", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: Localização Desabilitada", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão de Localização", pEx.Message, "OK");
            }
            catch (Exception ex) 
            {
                await DisplayAlert("Errro", ex.Message, "OK");
            }
        }

        private async void Getcidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                if (place != null)
                {
                    txt_cidade.Text = place.Locality;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: Obtenção do nome da cidade", ex.Message, "OK");
            }
        }
    }

}
