using BattleMiners.Classes;

namespace BattleMiners.Interfaces;

public interface IAlbums
{
    public Task<Dictionary<int, Album>> GetAlbums();

}