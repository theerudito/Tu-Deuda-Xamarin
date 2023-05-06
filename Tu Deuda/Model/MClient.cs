using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tu_Deuda.Model
{
    public class MClient
    {
        [Key]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [NotMapped]
        public string ClientId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("CI")]
        public int CI { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Imagen")]
        public string Imagen { get; set; }

        [JsonProperty("Saldo_Inicial")]
        public float Saldo_Inicial { get; set; }

        [JsonProperty("Fecha")]
        public string Fecha { get; set; }

        [JsonProperty("Status")]
        public bool Status { get; set; }
    }
}