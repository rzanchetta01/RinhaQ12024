using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Transaction
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int Valor { get; set; }

        [MaxLength(1)]
        public char Tipo { get; set; }

        [MaxLength(10)]
        public string Descricao { get; set; } = null!;

        [JsonIgnore]
        public int? IdCliente { get; set; }
        public DateTime Realizada_em { get; set; }

        [JsonIgnore]
        public virtual Client Cliente { get; set; } = null!;
    }
}
