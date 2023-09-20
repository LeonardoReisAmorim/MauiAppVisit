using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppVisit.Model
{
    public class LocationDetails
    {
        public int Id { get; set; }
        public string DescriptionPlace { get; set; }
        public string ImagePlace { get; set; }
        public byte[] ImagePlaceByte { get; set; }
    }
}
