using System.Text.Json.Serialization;

namespace Alcor.Models;

public class Transaction
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("type")]
    public string TrxType { get; set; } = null!;

    [JsonPropertyName("trx_id")]
    public string TrxId { get; set; } = null!;

    [JsonPropertyName("unit_price")]
    public float UnitPrice { get; set; }

    [JsonPropertyName("ask")]
    public float Ask { get; set; }

    [JsonPropertyName("bid")]
    public float Bid { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; } = null!;
}