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

        [ObservableProperty]
        private string _loading;

        [ObservableProperty]
        private string _avisoErro;

        public LocalItensViewModel()
        {
            HttpHelper = new HttpHelper();
            Loading = "true";
            AvisoErro = "";
            CarregaLugaresAsync();
        }

        private async void CarregaLugaresAsync()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Lugar";

            try
            {
                var response = await htppClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Lugar>>(responseStream);
                        Lugares = data;

                        if (!Lugares.Any())
                        {
                            AvisoErro = "No momento não temos ambientes virtuais disponiveis";
                        }
                        else
                        {
                            foreach (var item in Lugares)
                            {
                                item.ImagemByte = Convert.FromBase64String(item.imagem);
                            }
                        }
                    }
                    Loading = "false";
                }
            }
            catch (Exception ex)
            {
                Loading = "false";
                AvisoErro = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
            
        }
    }
}
