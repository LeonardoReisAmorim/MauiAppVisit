namespace MauiAppVisit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B86F6"),
                BarTextColor = Color.FromRgb(255, 255, 255)
            };
        }
    }
}