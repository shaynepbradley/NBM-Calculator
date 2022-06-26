using GenericClient;
using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Classes;

public class Albums : IAlbums
{
    private readonly IGenericClient _nbmClient;
    private readonly IConfiguration _config;
    private readonly ILogger<Albums> _logger;
    private readonly Dictionary<int, Album> _albums = new();

    public Albums(IGenericClient nbmClient, IConfiguration config, ILogger<Albums> logger)
    {
        _nbmClient = nbmClient;
        _config = config;
        _logger = logger;
    }

    public async Task<Dictionary<int, Album>> GetAlbums()
    {
        if (_albums.Count > 0)
            return _albums;

        var url = new Uri($"{_config["NbmUrl"]}/schemas");
        var list = await _nbmClient.GetData<List<Album>>(url, CancellationToken.None).ConfigureAwait(false);
        list?.ForEach(c => { _albums.TryAdd(c.id, c); });
        return _albums;
    }

}
