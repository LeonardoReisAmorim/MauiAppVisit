using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class Locations : ContentPage
{
    private readonly LocalItensViewModel _localItensViewModel;

    public Locations()
	{
		InitializeComponent();
        _localItensViewModel = new LocalItensViewModel();
        BindingContext = _localItensViewModel;  
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _localItensViewModel.CarregaLugaresAsync();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        Navigation.PushAsync(new LocationDetailsView(imageButton.CommandParameter.ToString()));
    }
}