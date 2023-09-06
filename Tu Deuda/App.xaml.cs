using MarcTron.Plugin;
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
            MyAds.ShowRewardedVideo();

            getLanguage();

            var _dbCcontext = new Application_Context();

            _dbCcontext.Database.Migrate();

            DataApp.DefaultClient();
            DataApp.DefaultDataBase();
            DataApp.DefaultCode();

            InitializeComponent();

            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += (s, args) =>
            {
                CrossMTAdmob.Current.ShowRewardedVideo();
            };

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            // Push message received event
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Received");
            };
            //Push message received event
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            MainPage = new NavigationPage(new PageHome());
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