using BattleMiners.Classes;

namespace BattleMiners.Interfaces;

public interface ICollections
{
    public  Task<Dictionary<int, Collection>> GetCollections();
}