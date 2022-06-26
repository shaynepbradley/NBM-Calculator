using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Interfaces;

public interface IMiningInfo
{
    public Task<Resources> CurrentlyAvailable();
    public Task<Resources> TotalPerHourProduction();
}