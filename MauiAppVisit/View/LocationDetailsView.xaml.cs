using CommunityToolkit.Maui.Views;
using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class LocationDetailsView : ContentPage
{
    private readonly LocationDetailsViewModel _locationDetailsViewModel;
    private string _Id = string.Empty;

    public LocationDetailsView(string id)
	{
        _Id = id;
		InitializeComponent();
        _locationDetailsViewModel = App.Current.Handler.MauiContext.Services.GetRequiredService<LocationDetailsViewModel>();
        BindingContext = _locationDetailsViewModel;
        BackgroundColor = Color.FromRgb(36, 87, 255);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _locationDetailsViewModel.GetLocationDetailsById(Convert.ToInt32(_Id));
    }

    void OnButtonClicked(object sender, EventArgs e)
    {
        var Button = (Button)sender;
        this.ShowPopup(new PopupInfo(Button.CommandParameter.ToString()));
    }
}