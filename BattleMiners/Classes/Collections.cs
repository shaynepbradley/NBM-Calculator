using BattleMiners.Interfaces;
using GenericClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BattleMiners.Classes;

public class Collections : ICollections
{
    private readonly IGenericClient _nbmClient;
    private readonly IConfiguration _config;
    private readonly ILogger<Collections> _logger;
    private readonly Dictionary<int, Collection> _collections = new Dictionary<int, Collection>();

    public Collections(IGenericClient nbmClient, IConfiguration config, ILogger<Collections> logger)
    {
        _nbmClient = nbmClient;
        _config = config;
        _logger = logger;
    }

    public async Task<Dictionary<int, Collection>> GetCollections()
    {
        if (_collections.Count > 0) 
            return _collections;

        var url = new Uri($"{_config["NbmUrl"]}/collections");
        var list = await _nbmClient.GetData<List<Collection>>(url, CancellationToken.None).ConfigureAwait(false);
        list?.ForEach(c => { _collections.TryAdd(c.id, c); });
        return _collections;
    }
}