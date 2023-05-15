using Tu_Deuda.ApplicationDB;

namespace Tu_Deuda.Data
{
    public class Connections
    {
        public static string urlSupabase()
        {
            var _dbContext = new Application_Context();
            var db = _dbContext.DBApp.Find(1);
            return db.UrlProyect;
        }

        public static string keySupabase()
        {
            var _dbContext = new Application_Context();
            var db = _dbContext.DBApp.Find(1);
            return db.KeyProyect;
        }

        public static string urlFirebase()
        {
            var _dbContext = new Application_Context();

            var db = _dbContext.DBApp.Find(1);

            return db.UrlProyect;
        }

        public static string urlWebApi()
        {
            var _dbContext = new Application_Context();
            var db = _dbContext.DBApp.Find(1);

            return db.UrlProyect;
        }
    }
}