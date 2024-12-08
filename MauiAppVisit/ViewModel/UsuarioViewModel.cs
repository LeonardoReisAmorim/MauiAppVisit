using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using System.Text.Json;
using System.Windows.Input;

namespace MauiAppVisit.ViewModel
{
    public partial class UsuarioViewModel : ObservableObject
    {
        private HttpHelper HttpHelper { get; set; }
        private readonly JsonSerializerOptions _jsonSerializerOptions;

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

        public UsuarioViewModel()
        {
            HttpHelper = new HttpHelper();
            Loading = "false";
            _jsonSerializerOptions = JsonSerializeOptionHelper.Options;
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());
        public ICommand LoginCommand => new Command(async () => await LogarAsync());

        private async Task RegisterAsync()
        {
            Loading = "true";

            var baseUrl = HttpHelper.GetBaseUrl();
            var httpClient = await HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/User/register";
            var usuario = new Usuario(Nome, Email, Password);
            
            try
            {
                using HttpResponseMessage response = await httpClient.PostAsync(url, HttpHelper.GetJsonContent(usuario));

                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Loading = "false";
                    await Shell.Current.DisplayAlert("Erro", content, "OK");
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();

                    var userToken = JsonSerializer.Deserialize<UserToken>(contentResponse, _jsonSerializerOptions);

                    await AuthorizationHelper.SetDataUser(userToken.Token, userToken.UsuarioId);
                    await GetUser(userToken.UsuarioId);

                    Loading = "false";

                    await Shell.Current.GoToAsync("//Locations");
                }
            }
            catch (Exception)
            {
                Loading = "false";
                await Shell.Current.DisplayAlert("teste", "Ocorreu um erro, por favor tente novamente mais tarde!", "OK");
            }
        }

        private async Task LogarAsync()
        {
            Loading = "true";

            var baseUrl = HttpHelper.GetBaseUrl();
            var httpClient = await HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/User/login";
            var usuario = new Usuario(null, Email, Password);

            try
            {
                using HttpResponseMessage response = await httpClient.PostAsync(url, HttpHelper.GetJsonContent(usuario));

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Loading = "false";
                    await Shell.Current.DisplayAlert("Erro", content, "OK");
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    var userToken = JsonSerializer.Deserialize<UserToken>(contentResponse, _jsonSerializerOptions);

                    await AuthorizationHelper.SetDataUser(userToken.Token, userToken.UsuarioId);
                    await GetUser(userToken.UsuarioId);

                    Loading = "false";

                    await Shell.Current.GoToAsync("//Locations", true);
                }
            }
            catch (Exception ex)
            {
                Loading = "false";
                await Shell.Current.DisplayAlert("Erro", "Ocorreu um erro, por favor tente novamente mais tarde!", "OK");
            }
        }

        private async Task GetUser(int id)
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var httpClient = await HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/User/getUserById/{id}";

            try
            {
                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Loading = "false";
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Usuario usuario = JsonSerializer.Deserialize<Usuario>(result, _jsonSerializerOptions);

                    PreferencesHelper.SetData("UserName", usuario.Name);
                    PreferencesHelper.SetData("userIsAdmin", usuario.IsAdmin.ToString());
                    PreferencesHelper.SetData("userEmail", usuario.Email);
                }
            }
            catch (Exception ex)
            {
                Loading = "false";
            }
        }
    }
}
