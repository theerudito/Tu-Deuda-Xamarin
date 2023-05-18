namespace Tu_Deuda.Helpers
{
    public static class LocalStorange
    {
        public static void SetStorange(string key, string value)
        {
            Xamarin.Essentials.Preferences.Set(key, value);
        }

        public static string GetStorange(string key)
        {
            return Xamarin.Essentials.Preferences.Get(key, string.Empty);
        }

        public static void DeleteStorange(string key)
        {
            Xamarin.Essentials.Preferences.Remove(key);
        }
    }
}