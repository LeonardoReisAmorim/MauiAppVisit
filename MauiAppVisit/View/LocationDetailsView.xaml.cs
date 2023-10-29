using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class LocationDetailsView : ContentPage
{
    public LocationDetailsView(string id)
	{
		InitializeComponent();
        BindingContext = new LocationDetailsViewModel(id);
    }
}