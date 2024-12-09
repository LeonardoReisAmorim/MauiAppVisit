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
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        [ObservableProperty]
        public ObservableCollection<Lugar> _lugares;

        [ObservableProperty]
        public ObservableCollection<TypePlace> _typePlaces;

        [ObservableProperty]
        private string _loading;

        [ObservableProperty]
        private string _avisoErro;

        [ObservableProperty]
        private string _userName;

        public LocalItensViewModel()
        {
            HttpHelper = new HttpHelper();
            Loading = "true";
            AvisoErro = "";
            UserName = $"Olá {PreferencesHelper.GetData("UserName")}";
            _jsonSerializerOptions = JsonSerializeOptionHelper.Options;
            TypePlaces = new ObservableCollection<TypePlace>
            {
                new TypePlace { Id = 1, Type = "Museus", ImageUrl = "museu.png" },
                new TypePlace { Id = 2, Type = "Eventos", ImageUrl = "evento.png" },
                new TypePlace { Id = 3, Type = "Faculdades", ImageUrl = "faculdade.png" },
                new TypePlace { Id = 4, Type = "Faculdades", ImageUrl = "faculdade.png" },
                new TypePlace { Id = 5, Type = "Faculdades", ImageUrl = "faculdade.png" }
            };
        }

        public async Task CarregaLugaresAsync()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = await HttpHelper.GetHttpClient();
            
            var url = $"{baseUrl}/Place";

            try
            {
                var response = await htppClient.GetAsync(url);

                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Loading = "false";
                    AvisoErro = "Usuario nao autorizado!";
                }

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Lugar>>(responseStream, _jsonSerializerOptions);
                        Lugares = data;

                        if (!Lugares.Any())
                        {
                            AvisoErro = "No momento não temos ambientes virtuais disponiveis";
                        }
                        else
                        {
                            foreach (var item in Lugares)
                            {
                                item.ImagemByte = Convert.FromBase64String(item.Image);
                            }
                        }
                    }
                    Loading = "false";
                }
            }
            catch (Exception)
            {
                Loading = "false";
                AvisoErro = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
            
        }
    }
}
