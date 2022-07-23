namespace BattleMiners.Classes; 

[Serializable]
public class Template
{
    // https://sockdev.nftbattleminers.com:8890/templates
    public int id { get; set; }
    public string name { get; set; }
    public int collection_id { get; set; }
    public int schema_id { get; set; }
    public string? description { get; set; }
    public int max_circulation { get; set; }
    public int released { get; set; }
}
