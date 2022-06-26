namespace UpgradeCalculator.Interfaces;

public interface IWaxId
{
    public bool HasId();
    public void SetId(string id);
    public string? GetId();

    public int Version { get; set; }
}