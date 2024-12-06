namespace MauiAppVisit.Helpers
{
    public static class DateHelper
    {
        public static bool AreEquals(string updateAt, string updatedAtDevice)
        {
            DateTime updateAtDate = DateTime.Parse(updateAt);
            DateTime updatedAtDeviceDate = DateTime.Parse(updatedAtDevice);
            return updateAtDate.Equals(updatedAtDeviceDate) && updateAtDate <= updatedAtDeviceDate;
        }
    }
}
