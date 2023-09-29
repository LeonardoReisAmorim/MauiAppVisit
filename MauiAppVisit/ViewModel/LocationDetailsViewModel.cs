using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Model;
using System.Text.Json;

namespace MauiAppVisit.ViewModel
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions;
        readonly string baseUrl = "http://10.0.2.2:5241";

        private readonly int id;

        [ObservableProperty]
        private string _descriptionPlace;

        [ObservableProperty]
        private byte[] _imagePlaceByte;

        public LocationDetailsViewModel(string Id)
        {
            id = Convert.ToInt32(Id);
            #if DEBUG
                HttpsClientHandlerService handler = new HttpsClientHandlerService();
                _httpClient = new HttpClient(handler.GetPlatformMessageHandler());
            #else
                _httpClient = new HttpClient();
            #endif
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            GetLocationDetailsById();
        }

        private async void GetLocationDetailsById()
        {
            var url = $"{baseUrl}/Lugar/{id}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<List<Lugar>>(responseStream);

                    DescriptionPlace = data[0].descricao;
                    ImagePlaceByte = Convert.FromBase64String(data[0].imagem);
                }
            }
        }
    }
}
