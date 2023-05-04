using System.ComponentModel.DataAnnotations;

namespace Tu_Deuda.Model
{
    public class Database
    {
        [Key]
        public int Id { get; set; }

        public string NameDatabase { get; set; }
        public string UrlProyect { get; set; }
        public string KeyProyect { get; set; }
    }
}