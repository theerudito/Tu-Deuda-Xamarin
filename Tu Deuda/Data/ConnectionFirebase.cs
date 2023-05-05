using Tu_Deuda.ApplicationDB;

namespace Tu_Deuda.Data
{
    public class ConnectionFirebase
    {
        public static string GetFirebaseFireStore()
        {
            var _dbContext = new Application_Context();

            var db = _dbContext.DBApp.Find(1);

            return db.UrlProyect;
        }
    }
}