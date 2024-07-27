namespace MauiAppVisit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromRgb(36, 87, 255),
                BarTextColor = Color.FromRgb(255, 255, 255)
            };
        }
    }
}