using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class Login : ContentPage
{
    public Login()
	{
		InitializeComponent();
        BindingContext = App.Current.Handler.MauiContext.Services.GetRequiredService<UsuarioViewModel>();
        BackgroundColor = Color.FromRgb(36, 87, 255);
    }

    private async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Register", true);
    }
}