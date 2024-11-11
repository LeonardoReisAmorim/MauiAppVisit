﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MauiAppVisit.Helpers
{
    public class HttpHelper
    {
        readonly HttpClient _httpClient;
        readonly string baseUrl = "https://233a-2804-2fb0-717-e800-7d25-3ada-3a41-c9bc.ngrok-free.app"; //http://10.0.2.2:5241 https://apivisitvr.azurewebsites.net

        public HttpHelper()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        public string GetBaseUrl()
        {
            return baseUrl;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            await AddAuthorizationHeader();

            return _httpClient;
        }

        public StringContent GetJsonContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await AuthorizationHelper.GetToken();

            if (!string.IsNullOrWhiteSpace(token) && AuthorizationHelper.IsTokenValid(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
