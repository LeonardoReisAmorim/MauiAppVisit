using CommunityToolkit.Maui.Views;
using MauiAppVisit.ViewModel;

namespace MauiAppVisit.View;

public partial class PopupInfo : Popup
{
    private readonly InformationPlaceVR _informationPlaceVR;
    public PopupInfo(string id)
	{
		InitializeComponent();
        _informationPlaceVR = new InformationPlaceVR(id);
        BindingContext = _informationPlaceVR;
        Task.Run(async () => await _informationPlaceVR.GetInformationPlaceVRByIdPlace());
    }
}