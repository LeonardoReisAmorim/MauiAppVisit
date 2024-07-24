using MauiAppVisit.View;
using MauiAppVisit.ViewModel;

namespace MauiAppVisit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgb(26, 26, 26);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Locations());
        }
    }
}