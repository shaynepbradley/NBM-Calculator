using BattleMiners.Classes;

namespace BattleMiners.Interfaces;

public interface ICards
{
    public Task<Dictionary<int, Card>> GetCards();

}