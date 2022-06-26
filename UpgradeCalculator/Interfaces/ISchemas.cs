using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Interfaces;

public interface ISchemas
{
    public Task<Dictionary<int, Schema>> GetSchemas();
}