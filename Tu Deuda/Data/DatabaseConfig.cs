using System;
using System.IO;
using Xamarin.Forms;

namespace Tu_Deuda.Data
{
    public static class DatabaseConfig
    {
        public static readonly string nameDatabase = "TuDeuda.db";

        public static string ConnectionString()
        {
            string databasePath = string.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", nameDatabase);
                    break;

                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nameDatabase);
                    break;

                default:
                    throw new NotImplementedException("Platform not supported");
            }
            return databasePath;
        }
    }
}
