using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Client
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Limite {  get; set; }
        public int Saldo { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; } = [];
    }
}
