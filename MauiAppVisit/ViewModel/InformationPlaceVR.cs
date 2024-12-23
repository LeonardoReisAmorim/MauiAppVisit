﻿using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using System.Text.Json;

namespace MauiAppVisit.ViewModel
{
    public partial class InformationPlaceVR : ObservableObject
    {
        private HttpHelper HttpHelper { get; set; }

        private readonly int IdLugar;

        [ObservableProperty]
        private string _utilizationPlaceVR;

        public InformationPlaceVR(string Id)
        {
            IdLugar = Convert.ToInt32(Id);
            HttpHelper = new HttpHelper();
        }

        public async Task GetInformationPlaceVRByIdPlace()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = await HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Place/utilizationPlaceVR/{IdLugar}";

            try
            {
                var response = await htppClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    UtilizationPlaceVR = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception)
            {
                throw;
                //Loading = "false";
                //Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }
    }
}
