using MauiAppVisit.Helpers;

namespace MauiAppVisit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new Teste();
        }

        //protected override async void OnStart()
        //{
        //    base.OnStart();
        //    await RedirectUserBasedOnToken();
        //}

        //private async Task RedirectUserBasedOnToken()
        //{
        //    if (!await AuthorizationHelper.HasToken())
        //    {
        //        MainPage = new AppShell();
        //        await Shell.Current.GoToAsync("//Login");
        //    }
        //    else
        //    {
        //        MainPage = new AppShell();
        //        await Shell.Current.GoToAsync("//Locations");
        //    }
        //}
    }
}