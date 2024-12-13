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

        private ObservableCollection<Lugar> _originalPlaces;

        [ObservableProperty]
        public ObservableCollection<Lugar> _lugares;

        [ObservableProperty]
        public ObservableCollection<TypePlace> _typePlaces;

        [ObservableProperty]
        private TypePlace _selectedItem;

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
                        _originalPlaces = await JsonSerializer.DeserializeAsync<ObservableCollection<Lugar>>(responseStream, _jsonSerializerOptions);
                        Lugares = _originalPlaces;

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

        public async Task CarregaTipoDeLugaresAsync()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = await HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/TypePlace";

            try
            {
                var response = await htppClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Loading = "false";
                    AvisoErro = "Usuario nao autorizado!";
                }

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<ObservableCollection<TypePlace>>(responseStream, _jsonSerializerOptions);
                        data.Add(new TypePlace
                        {
                            Id = 0,
                            Type = "Todos"
                        });
                        TypePlaces = data;
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

        partial void OnSelectedItemChanged(TypePlace value)
        {
            if(value == null)
            {
                return;
            }

            if (value.Id == 0)
            {
                Lugares = _originalPlaces;
                SelectedItem = null;
                return;
            }

            Lugares = new ObservableCollection<Lugar>(_originalPlaces.Where(p => p.TypePlaceId == value.Id));
            SelectedItem = null;
        }

        
    }
}
