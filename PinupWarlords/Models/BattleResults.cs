namespace PinupWarlords.Models
{
    public class BattleResults
	{
    public string winner {get;set;}
    public string user1 {get;set;}
    public string user2 {get;set;}
    public short firepower_1 {get;set;}
    public short firepower_2 {get;set;}
    public short initiative_1 {get;set;}
    public short initiative_2 {get;set;}
    public short total_hitpoints_1 {get;set;}
    public short total_hitpoints_2 {get;set;}
    public List<short> hitpoints_2 {get;set;}
    public short initiative_roll_1 {get;set;}
    public short initiative_roll_2 {get;set;}
    public bool sniper_hit_1 {get;set;}
    public bool sniper_hit_2 {get;set;}
    public bool paratrooper_defended_1 {get;set;}
    public bool paratrooper_defended_2 {get;set;}

    }
}
