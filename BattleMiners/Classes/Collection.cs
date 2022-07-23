namespace BattleMiners.Classes;

[Serializable]
public class Collection
{
    // https://sockdev.nftbattleminers.com:8890/collections
    public int id { get; set; }
    public string name { get; set; }
    public string image { get; set; }
    public int album_id { get; set; }
}