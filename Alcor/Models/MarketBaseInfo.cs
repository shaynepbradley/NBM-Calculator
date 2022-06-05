using System.Text.Json.Serialization;

namespace Alcor.Models;

public class MarketBaseInfo
{
    public int pause { get; set; }
    public Token[] supported_tokens {get;set; }

    [JsonPropertyName("delegate")]
    public string Delegate { get; set; }
    public int init { get; set; }
    public int market_fee { get; set; }
    public int order_counter { get; set; }
}