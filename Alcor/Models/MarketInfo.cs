using System.Text.Json.Serialization;

namespace Alcor.Models
{
    public class MarketInfo
    {
        [JsonPropertyName("base_token")] 
        public Token BaseToken { get; set; } = null!;
            
        [JsonPropertyName("quote_token")]
        public Token QuoteToken { get; set; } = null!;

        [JsonPropertyName("_id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("chain")]
        public string Chain { get; set; } = null!;

        [JsonPropertyName("id")]
        public int MarketId { get; set; }

        [JsonPropertyName("min_buy")]
        public string MinBuy { get; set; } = null!;

        [JsonPropertyName("min_sell")]
        public string MinSell { get; set; } = null!;

        [JsonPropertyName("frozen")]
        public bool Frozen { get; set; }

        [JsonPropertyName("fee")]
        public float Fee { get; set; }

        [JsonPropertyName("last_price")]
        public float last_price { get; set; }

        [JsonPropertyName("volume24")]
        public float Volume24 { get; set; }

        [JsonPropertyName("volumeWeek")]
        public float VolumeWeek { get; set; }

        [JsonPropertyName("volumeMonth")]
        public float VolumeMonth { get; set; }

        [JsonPropertyName("change24")]
        public float Change24 { get; set; }

        [JsonPropertyName("changeWeek")]
        public float ChangeWeek { get; set; }

        public string SymbolName => QuoteToken.TokenSymbol.Name;
        public float LastPrice => last_price;  // * QuoteToken.TokenSymbol.Precision;
    }
}
 