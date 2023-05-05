using Tu_Deuda.ApplicationDB;

namespace Tu_Deuda.Data
{
    public class ConnectionSupabase
    {

        public static string URLSupabase()
        {
            var _dbContext = new Application_Context();
            var db = _dbContext.DBApp.Find(1);
            return db.UrlProyect;
        }
        public static string KeySupabase()
        {
            var _dbContext = new Application_Context();
            var db = _dbContext.DBApp.Find(1);
            return db.KeyProyect;
        }
    }
}
