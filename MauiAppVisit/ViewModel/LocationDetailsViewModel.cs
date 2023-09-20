using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppVisit.ViewModel
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        private string id;

        [ObservableProperty]
        private string _descriptionPlace;

        [ObservableProperty]
        private string _imagePlace;

        public LocationDetailsViewModel(string id)
        {
            this.id = id;
            CriarBaseParaTeste();
            GetLocationDetailsById();
        }

        private void CriarBaseParaTeste()
        {
            throw new NotImplementedException();
        }

        private void GetLocationDetailsById()
        {
            throw new NotImplementedException();
        }
    }
}
