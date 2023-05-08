using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tu_Deuda.Model
{
    public class MClient
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string ClientId { get; set; }

        public string Name { get; set; }
        public int CI { get; set; }
        public string Description { get; set; }
        public string Imagen { get; set; }
        public float Saldo_Inicial { get; set; }
        public string Fecha { get; set; }
        public bool Status { get; set; }
    }
}