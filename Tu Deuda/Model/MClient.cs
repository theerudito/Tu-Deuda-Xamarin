using Newtonsoft.Json;
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

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("ci")]
        public int CI { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("imagen")]
        public string Imagen { get; set; }
        [JsonProperty("saldo_Inicial")]
        public float Saldo_Inicial { get; set; }
        [JsonProperty("fecha")]
        public string Fecha { get; set; }
        [JsonProperty("status")]
        public bool Status { get; set; }
    }
}