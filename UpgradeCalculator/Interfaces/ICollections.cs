using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Interfaces;

public interface ICollections
{
    public  Task<Dictionary<int, Collection>> GetCollections();
}