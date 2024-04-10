using CommunityToolkit.Maui.Views;
using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class PopupInfo : Popup
{
	public PopupInfo(string id)
	{
		InitializeComponent();
        BindingContext = new InformationPlaceVR(id);
    }
}