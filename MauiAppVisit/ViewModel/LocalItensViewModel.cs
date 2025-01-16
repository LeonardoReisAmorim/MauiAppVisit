using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using MauiAppVisit.WebServiceHttpClient;
using System.Collections.ObjectModel;

namespace MauiAppVisit.ViewModel
{
    public partial class LocalItensViewModel : ObservableObject
    {
        private readonly IWebService _webService;

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

        public LocalItensViewModel(IWebService webService)
        {
            _webService = webService;
            Loading = "true";
            AvisoErro = "";
            UserName = $"Olá {PreferencesHelper.GetData("UserName")}";
        }

        public async Task CarregaLugaresAsync()
        {
            try
            {
                _originalPlaces = await _webService.CarregaLugaresAsync();
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
                Loading = "false";
            }
            catch (Exception)
            {
                Loading = "false";
                AvisoErro = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }

        public async Task CarregaTipoDeLugaresAsync()
        {
            try
            {
                var data = await _webService.CarregaTipoDeLugaresAsync();
                data.Add(new TypePlace
                {
                    Id = 0,
                    Type = "Todos",
                });
                TypePlaces = data;
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
