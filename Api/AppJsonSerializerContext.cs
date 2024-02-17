using Api.Models;
using Api.ResponseStructs;
using System.Text.Json.Serialization;

namespace Api
{
    [JsonSerializable(typeof(Client[]))]
    [JsonSerializable(typeof(TransactionOutput))]
    [JsonSerializable(typeof(StatementOutput))]
    [JsonSerializable(typeof(Transaction[]))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}
