using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using MauiAppVisit.WebServiceHttpClient;
using System.Text.Json;
using System.Windows.Input;

namespace MauiAppVisit.ViewModel
{
    public partial class UsuarioViewModel : ObservableObject
    {
        private readonly IWebService _webService;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _nome;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _loading;

        public UsuarioViewModel(IWebService webService)
        {
            _webService = webService;
            Loading = "false";
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());
        public ICommand LoginCommand => new Command(async () => await LogarAsync());

        private async Task RegisterAsync()
        {
            try
            {
                Loading = "true";

                var usuario = new Usuario(Nome, Email, Password);
                var userToken = await _webService.RegisterAsync(usuario);

                await AuthorizationHelper.SetDataUser(userToken.Token, userToken.UsuarioId);
                await GetUser(userToken.UsuarioId);

                Loading = "false";

                await Shell.Current.GoToAsync("//Locations");
            }
            catch (Exception)
            {
                Loading = "false";
                await Shell.Current.DisplayAlert("teste", "Ocorreu um erro, por favor tente novamente mais tarde!", "OK");
            }
        }

        private async Task LogarAsync()
        {
            try
            {
                Loading = "true";

                var usuario = new Usuario(null, Email, Password);

                var userToken = await _webService.LogarAsync(usuario);
                await AuthorizationHelper.SetDataUser(userToken.Token, userToken.UsuarioId);
                await GetUser(userToken.UsuarioId);

                Loading = "false";

                await Shell.Current.GoToAsync("//Locations", true);
            }
            catch (Exception ex)
            {
                Loading = "false";
                await Shell.Current.DisplayAlert("Erro", "Ocorreu um erro, por favor tente novamente mais tarde!", "OK");
            }
        }

        private async Task GetUser(int id)
        {
            try
            {
                Usuario usuario = await _webService.GetUser(id);
                PreferencesHelper.SetData("UserName", usuario.Name);
                PreferencesHelper.SetData("userIsAdmin", usuario.IsAdmin.ToString());
                PreferencesHelper.SetData("userEmail", usuario.Email);
            }
            catch (Exception)
            {
                Loading = "false";
            }
        }
    }
}
