using System.Text.Json.Serialization;

namespace UpgradeCalculator.Classes;

// Populated by calling either 
// https://sockdev.nftbattleminers.com:8890/card_type/[cardid] - Card
// https://sockdev.nftbattleminers.com:8890/card_type/template/[templateId] - list of Cards
public class Card
{
    public int id { get; set; }
    public int template_id { get; set; }
    public string atomic_img { get; set; }
    public ulong level { get; set; }
    public string rarity { get; set; }
    public string variant { get; set; }
    public string special { get; set; }
    public int? skill { get; set; }
    public int? skill_value { get; set; }
    public string type { get; set; }

    // Specific to active cards
    public int energy { get; set; }
    public int power { get; set; }

    // Not used by land cards
    public int res_mining { get; set; }
    public int nft_mining { get; set; }

    // used by land cards
    public int size { get; set; }

    // asset information
    public float Price { get; set; }
    public string Name { get; set; }
    public string AssetId { get; set; }

    public int GetRarity() => rarity switch
    {
        "Common" => 1,
        "Rare" => 2,
        "Epic" => 3,
        "Legendary" => 4,
        "Ultimate" => 5,
        _ => 0
    };

}
