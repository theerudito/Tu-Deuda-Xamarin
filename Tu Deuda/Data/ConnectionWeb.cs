using Tu_Deuda.ApplicationDB;

namespace Tu_Deuda.Data
{
    public class ConnectionWeb
    {
        public static string UrlWeb()
        {
            var _context = new Application_Context();

            var url = _context.DBApp.Find(1);

            return url.UrlProyect;
        }
    }
}