using System.Text.Json.Serialization;
using AtomicAssets.Models;

namespace UpgradeCalculator.Classes;

public class Construction : IAsset
{
    public string name { get; set; }
    public string img { get; set; }
    public string album { get; set; }
    public string collection { get; set; }
    [JsonConverter(typeof(UlongJsonConverter))]
    public ulong level { get; set; }
    public string rarity { get; set; }
    public string variant { get; set; }

    [JsonPropertyName("resource mining")]
    [JsonConverter(typeof(UlongJsonConverter))]
    public ulong ResM { get; set; }
    [JsonPropertyName("nft mining ")]
    [JsonConverter(typeof(UlongJsonConverter))]
    public ulong NftM { get; set; }
    [JsonConverter(typeof(UlongJsonConverter))]
    public ulong size { get; set; }
    public string special { get; set; }
    public string description { get; set; }

    public float Price { get; set; }
    public string AssetId { get; set; }

    private static int[] baseResources = { 0, 70000, 60000, 45000, 25000, 0 };

    public int GetRarity() => rarity switch
    {
        "Common" => 1,
        "Rare" => 2,
        "Epic" => 3,
        "Legendary" => 4,
        "Ultimate" => 5,
        _ => 0
    };

    public float PriceToLevel5(float quadPrice)
    {
        try
        {
            return Price + quadPrice * (baseResources[level] * GetRarity());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }

    public float PriceToLevelFuse(float fusium) => MathF.Round(fusium * FusiumToFuse, 2);

    public int FusiumToFuse => 54000 + (GetRarity() - 1) * 27000;

    public int ResourcesToLevel => baseResources[level] * GetRarity();

    public float PriceToNextRarity5(float quadPrice) => quadPrice * ResourcesToLevel;

    public bool Ignore { get; set; }

    public override string ToString()
    {
        return $"{variant} {name}";
    }
}