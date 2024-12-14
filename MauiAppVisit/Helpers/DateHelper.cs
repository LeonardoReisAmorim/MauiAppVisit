using System.Globalization;

namespace MauiAppVisit.Helpers
{
    public static class DateHelper
    {
        public static bool AreEquals(string updateAt, string updatedAtDevice)
        {
            DateTime updateAtDate = DateTime.ParseExact(updateAt, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime updatedAtDeviceDate = DateTime.ParseExact(updatedAtDevice, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            return updateAtDate.Equals(updatedAtDeviceDate) && updateAtDate <= updatedAtDeviceDate;
        }
    }
}
