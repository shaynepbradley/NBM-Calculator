namespace UpgradeCalculator.Classes;

public class WalletItem
{
    public enum ResIds
    {
        Fusium = 1,
        Actium,
        Minium,
        Constructium,
        Unknownium
    }
    public int resource_id { get; set; }
    public string name { get; set; }
    public float amount { get; set; }
}

