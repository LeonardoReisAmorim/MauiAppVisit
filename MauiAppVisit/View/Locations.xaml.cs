using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class Locations : ContentPage
{
	public Locations()
	{
		InitializeComponent();
        BindingContext = new LocalItensViewModel();
        BackgroundColor = Color.FromRgb(36, 87, 255);
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        Navigation.PushAsync(new LocationDetailsView(imageButton.CommandParameter.ToString()));
    }
}