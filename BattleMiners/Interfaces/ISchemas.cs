using BattleMiners.Classes;

namespace BattleMiners.Interfaces;

public interface ISchemas
{
    public Task<Dictionary<int, Schema>> GetSchemas();
}