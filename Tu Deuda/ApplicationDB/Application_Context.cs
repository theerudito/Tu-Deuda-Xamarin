using Microsoft.EntityFrameworkCore;
using Tu_Deuda.Data;
using Tu_Deuda.Model;

namespace Tu_Deuda.ApplicationDB
{
    public class Application_Context : DbContext
    {
        public Application_Context()
        {
            SQLitePCL.Batteries_V2.Init();

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabaseConfig.ConnectionString()}");
        }

        public DbSet<MClient> Clients { get; set; }
        public DbSet<Database> DBApp { get; set; }
        public DbSet<CodeApp> Code_App { get; set; }
    }
}