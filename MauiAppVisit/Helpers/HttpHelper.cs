using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MauiAppVisit.Helpers
{
    public class HttpHelper
    {
        readonly HttpClient _httpClient;
        readonly string baseUrl = "https://16ab-2804-2fb0-717-e800-5de9-dae1-ad74-67e3.ngrok-free.app"; //http://10.0.2.2:5241 https://apivisitvr.azurewebsites.net

        public HttpHelper()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public string GetBaseUrl()
        {
            return baseUrl;
        }

        public HttpClient GetHttpClient()
        {
            AddAuthorizationHeader();

            return _httpClient;
        }

        public StringContent GetJsonContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private void AddAuthorizationHeader()
        {
            var token = Preferences.Get("token", string.Empty);

            if (!string.IsNullOrWhiteSpace(token) && AuthorizationHelper.IsTokenValid(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
