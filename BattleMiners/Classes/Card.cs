namespace BattleMiners.Classes;

public class Card
{
    public int id { get; set; }
    public int template_id { get; set; }
    public byte level { get; set; }
    public string rarity { get; set; }
    public string variant { get; set; }
    public short? energy { get; set; }
    public short? power { get; set; }
    public short? res_mining { get; set; }
    public short? nft_mining { get; set; }
    public string? special { get; set; }
    public int? circulation { get; set; }
    public string type { get; set; }
    public string atomic_img { get; set; }
    public int? size { get; set; }
    public int? skill { get; set; }
    public int? skill_value { get; set; }
}