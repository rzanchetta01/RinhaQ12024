using Api.Models;

namespace Api.ResponseStructs
{
    public record TransactionOutput
    {
        public int limite { get; init; }
        public int saldo { get; init; }
    }

    public record StatementOutput
    {
        public int total { get; init; }
        public DateTime data_extrato { get; init; }
        public int limite { get; init; }

        public ICollection<Transaction> ultimas_transacoes { get; init; } = null!;
    }

}
