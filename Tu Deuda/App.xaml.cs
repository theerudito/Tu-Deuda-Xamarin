using Tu_Deuda.View;
using Tu_Deuda.ApplicationDB;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Tu_Deuda.Model;
using System.Linq;

namespace Tu_Deuda
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Xamarin.Essentials.SecureStorage.SetAsync("LANGUAGE", "EN");

            MainPage = new NavigationPage(new PageHome());

            var _dbCcontext = new Application_Context();

            _dbCcontext.Database.Migrate();

            var id = 1;

            var searchData = _dbCcontext.Clients.Where(c => c.Id == id || c.Status == false).FirstOrDefault();

            if (searchData == null )
            {
                var inicialData = new Client
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
