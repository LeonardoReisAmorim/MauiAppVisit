using CommunityToolkit.Mvvm.ComponentModel;
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
            GetInformationPlaceVRByIdPlace();
        }

        private async void GetInformationPlaceVRByIdPlace()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Lugar/utilizationPlaceVR/{IdLugar}";

            try
            {
                var response = await htppClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    UtilizationPlaceVR = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                //Loading = "false";
                //Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }
    }
}
