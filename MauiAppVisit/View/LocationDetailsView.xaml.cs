using CommunityToolkit.Maui.Views;
using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class LocationDetailsView : ContentPage
{
    private readonly LocationDetailsViewModel _locationDetailsViewModel;
    public LocationDetailsView(string id)
	{
		InitializeComponent();
        _locationDetailsViewModel = new LocationDetailsViewModel(id);
        BindingContext = _locationDetailsViewModel;
        BackgroundColor = Color.FromRgb(36, 87, 255);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _locationDetailsViewModel.GetLocationDetailsById();
    }

    void OnButtonClicked(object sender, EventArgs e)
    {
        var Button = (Button)sender;
        this.ShowPopup(new PopupInfo(Button.CommandParameter.ToString()));
    }
}