using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MauiAppVisit.WebServiceHttpClient
{
    public class WebService : IWebService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public WebService(HttpClient client)
        {
            _client = client;
            _jsonSerializerOptions = JsonSerializeOptionHelper.Options;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await AuthorizationHelper.GetToken();

            if (!string.IsNullOrWhiteSpace(token) && AuthorizationHelper.IsTokenValid(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<string> GetInformationPlaceVRByIdPlace(int id)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _client.GetAsync($"/Place/utilizationPlaceVR/{id}");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<string>(responseStream);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<Lugar>> CarregaLugaresAsync()
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _client.GetAsync("/Place");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<ObservableCollection<Lugar>>(responseStream, _jsonSerializerOptions);
                }
                return [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<TypePlace>> CarregaTipoDeLugaresAsync()
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _client.GetAsync("/TypePlace");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<ObservableCollection<TypePlace>>(responseStream, _jsonSerializerOptions);
                }
                return [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Lugar>> GetLocationDetailsById(int id)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _client.GetAsync($"/Place/{id}");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<List<Lugar>>(responseStream, _jsonSerializerOptions);
                }
                return [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Stream> RequestDownloadFileVR(int id)
        {
            try
            {
                await AddAuthorizationHeader();
                var responseFile = await _client.GetAsync($"/FileVR/{id}");

                if (responseFile.IsSuccessStatusCode)
                {
                    return await responseFile.Content.ReadAsStreamAsync();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FileVrDetails> RequestFileVrDetails(int id)
        {
            try
            {
                await AddAuthorizationHeader();
                var responseDetailsFileVR = await _client.GetAsync($"/FileVR/dadosArquivos/{id}");

                if (responseDetailsFileVR.IsSuccessStatusCode)
                {
                    using var responseContentFileVrDetailsStream = await responseDetailsFileVR.Content.ReadAsStreamAsync();
                    var fileVrDetailsList = await JsonSerializer.DeserializeAsync<List<FileVrDetails>>(responseContentFileVrDetailsStream, _jsonSerializerOptions);
                    return fileVrDetailsList[0];
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserToken> RegisterAsync(Usuario usuario)
        {
            try
            {
                using HttpResponseMessage response = await _client.PostAsync("/User/register", new StringContent(JsonSerializer.Serialize(usuario), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    using var contentResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<UserToken>(contentResponse, _jsonSerializerOptions);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserToken> LogarAsync(Usuario usuario)
        {
            try
            {
                using HttpResponseMessage response = await _client.PostAsync("/User/login", new StringContent(JsonSerializer.Serialize(usuario), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    using var contentResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<UserToken>(contentResponse, _jsonSerializerOptions);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> GetUser(int id)
        {
            try
            {
                await AddAuthorizationHeader();
                var response = await _client.GetAsync($"/User/getUserById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    using var result = await response.Content.ReadAsStreamAsync();
                    return JsonSerializer.Deserialize<Usuario>(result, _jsonSerializerOptions);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
