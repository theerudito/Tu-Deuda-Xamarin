using System.Linq;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Code;
using Tu_Deuda.Model;

namespace Tu_Deuda.Helpers
{
    public static class DataApp
    {
        private static Application_Context _dbCcontext = new Application_Context();

        private static int iD = 1;

        public static void DefaultClient()
        {
            var searchData = _dbCcontext.Clients.Where(c => c.Id == iD || c.Status == false).FirstOrDefault();

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
        }

        public static void DefaultDataBase()
        {
            var searchDatabase = _dbCcontext.DBApp.Where(c => c.Id == iD).FirstOrDefault();

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
        }

        public static void DefaultCode()
        {
            var searchCode = _dbCcontext.Code_App.Where(c => c.Id == iD).FirstOrDefault();

            if (searchCode == null)
            {
                var initialCode = new CodeApp
                {
                    Id = 1,
                    CodeAdmin = BcryManager.HashPassword(AppCode.codeAdmin),
                };

                _dbCcontext.Add(initialCode);
                _dbCcontext.SaveChangesAsync();
            }
        }
    }
}