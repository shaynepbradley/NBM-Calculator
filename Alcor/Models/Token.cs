using System.Text.Json.Serialization;

namespace Alcor.Models
{
    public class Token
    {
        [JsonPropertyName("symbol")]
        public Symbol TokenSymbol { get; set; } = null!;

        [JsonPropertyName("contract")]
        public string Contract { get; set; } = null!;

        [JsonPropertyName("str")]
        public string Contact { get; set; } = null!;
    }
}
