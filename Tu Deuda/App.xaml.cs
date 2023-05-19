using Microsoft.EntityFrameworkCore;
using Plugin.FirebasePushNotification;
using Plugin.Multilingual;
using System;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Helpers;
using Tu_Deuda.View;
using Xamarin.Forms;

namespace Tu_Deuda
{
    public partial class App : Application
    {
        public App()
        {
            getLanguage();

            var _dbCcontext = new Application_Context();

            _dbCcontext.Database.Migrate();

            DataApp.DefaultClient();
            DataApp.DefaultDataBase();
            DataApp.DefaultCode();

            InitializeComponent();
            MainPage = new NavigationPage(new PageHome());

            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
        }

        public void getLanguage()
        {
            var currentLanguage = CrossMultilingual.Current.CurrentCultureInfo;

            Console.WriteLine("Idioma es " + currentLanguage.Name);

            if (currentLanguage.Name == "en-US")
            {
                LocalStorange.SetStorange("language", "EN");
            }
            else if (currentLanguage.Name == "es-ES")
            {
                LocalStorange.SetStorange("language", "ES");
            }
            else
            {
                LocalStorange.SetStorange("language", "EN");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}