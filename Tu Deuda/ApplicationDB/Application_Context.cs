using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Tu_Deuda.Model;
using Xamarin.Forms;

namespace Tu_Deuda.ApplicationDB
{
    public class Application_Context : DbContext
    {
        public Application_Context()
        {
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
        }

        private const string DatabaseName = "YourDeuda.db3";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
                    break;

                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                    break;

                default:
                    throw new NotImplementedException("Platform not supported");
            }
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Database> DBApp { get; set; }
        public DbSet<CodeApp> Code_App { get; set; }
    }
}