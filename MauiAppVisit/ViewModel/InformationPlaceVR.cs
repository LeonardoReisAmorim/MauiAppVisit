using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.WebServiceHttpClient;

namespace MauiAppVisit.ViewModel
{
    public partial class InformationPlaceVR : ObservableObject
    {
        private readonly IWebService _webService;

        [ObservableProperty]
        private string _utilizationPlaceVR;

        public InformationPlaceVR(IWebService webService)
        {
            _webService = webService;
        }

        public async Task GetInformationPlaceVRByIdPlace(int id)
        {
            try
            {
                UtilizationPlaceVR = await _webService.GetInformationPlaceVRByIdPlace(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
