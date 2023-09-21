namespace MauiAppVisit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromRgb(16, 181, 191),
                BarTextColor = Color.FromRgb(255, 255, 255),
            };
        }
    }
}