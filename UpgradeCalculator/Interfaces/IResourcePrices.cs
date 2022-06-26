using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Interfaces;

public interface IResourcePrices
{
    public Task<Resources> Current();

}