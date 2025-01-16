using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class Register : ContentPage
{
    public Register()
	{
        InitializeComponent();
        BindingContext = App.Current.Handler.MauiContext.Services.GetRequiredService<UsuarioViewModel>();
        BackgroundColor = Color.FromRgb(36, 87, 255);
    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Login", true);
    }
}