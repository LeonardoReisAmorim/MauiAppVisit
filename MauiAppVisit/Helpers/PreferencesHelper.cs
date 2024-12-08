namespace MauiAppVisit.Helpers
{
    public static class PreferencesHelper
    {
        public static void SetData(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public static void RemoveData(string key)
        {
            Preferences.Remove(key);
        }

        public static string GetData(string key)
        {
            return Preferences.Get(key, "");
        }
    }
}
