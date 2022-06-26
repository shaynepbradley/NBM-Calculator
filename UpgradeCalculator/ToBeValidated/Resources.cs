namespace UpgradeCalculator.Classes;

public class Resources
{
    public float Wax { get; set; }
    public float Fusium { get; set; }
    public float Actium { get; set; }
    public float Minium { get; set; }
    public float Constructium { get; set; }
    public float Total => Fusium + Actium + Minium + Constructium;
    public DateTime? UpdatedAt { get; set; } = null;
}