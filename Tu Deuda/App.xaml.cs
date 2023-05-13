using Microsoft.EntityFrameworkCore;
using Plugin.FirebasePushNotification;
using System.Linq;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Code;
using Tu_Deuda.Model;
using Tu_Deuda.View;
using Xamarin.Forms;

namespace Tu_Deuda
{
    public partial class App : Application
    {
        public App()
        {
            Xamarin.Essentials.SecureStorage.SetAsync("LANGUAGE", "EN");

            var _dbCcontext = new Application_Context();

            _dbCcontext.Database.Migrate();

            var id = 1;

            var searchData = _dbCcontext.Clients.Where(c => c.Id == id || c.Status == false).FirstOrDefault();

            if (searchData == null)
            {
                var inicialData = new MClient
                {
                    Id = 1,
                    Name = "Prueba",
                    Description = "Prueba",
                    Fecha = "10/25/2002",
                    Saldo_Inicial = 2.52f,
                    Status = true,
                };

                _dbCcontext.Add(inicialData);
                _dbCcontext.SaveChangesAsync();
            }

            var searchDatabase = _dbCcontext.DBApp.Where(c => c.Id == id).FirstOrDefault();

            if (searchDatabase == null)
            {
                var inicialDatabase = new Database
                {
                    Id = 1,
                    NameDatabase = "Sqlite",
                    UrlProyect = null,
                    KeyProyect = null,
                };
                _dbCcontext.Add(inicialDatabase);
                _dbCcontext.SaveChangesAsync();
            }

            var searchCode = _dbCcontext.Code_App.Where(c => c.Id == id).FirstOrDefault();

            if (searchCode == null)
            {
                var initialCode = new CodeApp
                {
                    Id = 1,
                    CodeAdmin = AdminCode.CodeAdmin(),
                };
                _dbCcontext.Add(initialCode);
                _dbCcontext.SaveChangesAsync();
            }

            InitializeComponent();
            MainPage = new NavigationPage(new PageHome());

            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
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