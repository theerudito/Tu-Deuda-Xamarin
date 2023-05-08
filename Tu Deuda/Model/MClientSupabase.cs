using Postgrest.Attributes;
using Postgrest.Models;

namespace Tu_Deuda.Model
{
    [Table("MClientSupabase")]
    public class MClientSupabase : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("CI")]
        public int CI { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Saldo_Inicial")]
        public float Saldo_Inicial { get; set; }

        [Column("Fecha")]
        public string Fecha { get; set; }

        [Column("Status")]
        public bool Status { get; set; }
    }
}