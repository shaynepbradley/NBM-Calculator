using System.Text.Json.Serialization;

namespace UpgradeCalculator.Classes;

public interface IAsset
{
    public string name { get; set; }
    public string img { get; set; }
    public string album { get; set; }
    public string collection { get; set; }
    public ulong level { get; set; }
    public string rarity { get; set; }
    public string variant { get; set; }
    public float Price { get; set; }
    public string AssetId { get; set; }


}