using CommunityToolkit.Maui.Views;
using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class LocationDetailsView : ContentPage
{
    public LocationDetailsView(string id)
	{
		InitializeComponent();
        BindingContext = new LocationDetailsViewModel(id);
        BackgroundColor = Color.FromRgb(36, 87, 255);
    }

    void OnButtonClicked(object sender, EventArgs e)
    {
        var Button = (Button)sender;
        this.ShowPopup(new PopupInfo(Button.CommandParameter.ToString()));
    }
}