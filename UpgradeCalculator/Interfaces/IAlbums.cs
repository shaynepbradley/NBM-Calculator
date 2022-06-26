using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Interfaces;

public interface IAlbums
{
    public Task<Dictionary<int, Album>> GetAlbums();

}