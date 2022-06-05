namespace UpgradeCalculator.Classes;

public class MiningOperation
{
    public ulong id { get; set; }
    public ulong user_id { get; set; }
    public short active { get; set; }
    public ulong land_asset_id { get; set; }
    public WalletItem.ResIds resource_type { get; set; }
    public float amount_ph { get; set; }
    public string created_at { get; set; }
    public string closed_at { get; set; }
    public string close_reason { get; set; }
    public string arrival_time { get; set; }
    public string return_time { get; set; }
    public string status { get; set; }
    public float tokens_mined { get; set; }
    public float global_break_chance { get; set; }
    public int? round { get; set; }
    public int? disband_on_arrival { get; set; }
    public int? auto_repair { get; set; }
}