using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Classes;

public class WaxId : IWaxId
{
    private string? _id = null;
    public bool HasId() => !string.IsNullOrEmpty(_id);
    public void SetId(string id)
    {
        _id = id;
    }

    public string? GetId() => _id;
    public int Version { get; set; } = -1;
}