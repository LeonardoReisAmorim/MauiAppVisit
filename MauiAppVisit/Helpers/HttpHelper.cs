﻿using System.Text.Json;

namespace MauiAppVisit.Helpers
{
    public class HttpHelper
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions;
        readonly string baseUrl = "http://10.0.2.2:5241"; //5000 (docker)

        public HttpHelper()
        {
            #if DEBUG
                HttpsClientHandlerService handler = new HttpsClientHandlerService();
                _httpClient = new HttpClient(handler.GetPlatformMessageHandler());
            #else
                _httpClient = new HttpClient();
            #endif
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
        }

        public string GetBaseUrl()
        {
            return baseUrl;
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
