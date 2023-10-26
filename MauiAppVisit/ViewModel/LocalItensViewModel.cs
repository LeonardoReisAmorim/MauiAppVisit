using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Model;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiAppVisit.ViewModel
{
    public partial class LocalItensViewModel : ObservableObject
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions;
        readonly string baseUrl = "http://10.0.2.2:5000";

        [ObservableProperty]
        public ObservableCollection<Lugar> _lugares; 

        public LocalItensViewModel()
        {
            #if DEBUG
                HttpsClientHandlerService handler = new HttpsClientHandlerService();
                _httpClient = new HttpClient(handler.GetPlatformMessageHandler());
            #else
                _httpClient = new HttpClient();
            #endif
            _lugares = new ObservableCollection<Lugar>();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            CarregaLugaresAsync();
        }

        private async void CarregaLugaresAsync()
        {
            var url = $"{baseUrl}/Lugar";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Lugar>>(responseStream);
                    Lugares = data;

                    foreach (var item in Lugares)
                    {
                        item.ImagemByte = Convert.FromBase64String(item.imagem);
                    }
                }
            }
        }
    }
}
