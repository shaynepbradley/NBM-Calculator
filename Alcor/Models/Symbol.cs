using System.Text.Json.Serialization;

namespace Alcor.Models;

public class Symbol
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("precision")]
    public float Precision { get; set; }
}