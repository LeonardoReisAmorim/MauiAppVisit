using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiAppVisit.ViewModel
{
    public partial class LocalItensViewModel : ObservableObject
    {
        HttpHelper HttpHelper { get; set; }

        [ObservableProperty]
        public ObservableCollection<Lugar> _lugares; 

        public LocalItensViewModel()
        {
            HttpHelper = new HttpHelper();
            CarregaLugaresAsync();
        }

        private async void CarregaLugaresAsync()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Lugar";
            var response = await htppClient.GetAsync(url);

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
