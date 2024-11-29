using MauiAppVisit.Platforms.Android;

namespace MauiAppVisit;


public partial class Teste : ContentPage
{
	public Teste()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        AndroidUtils.GrantedPermission();
        AndroidUtils.ListFilesInDownloadFolder();

        DisplayAlert("teste", "teste", "OK");
    }
}