using MauiAppVisit.Helpers;

namespace MauiAppVisit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            RedirectUserBasedOnToken();
        }

        private async void RedirectUserBasedOnToken()
        {
            if (!AuthorizationHelper.HasToken())
            {
                await Shell.Current.GoToAsync("//Login");
                return;
            }
            
            await Shell.Current.GoToAsync("//Locations");
        }
    }
}