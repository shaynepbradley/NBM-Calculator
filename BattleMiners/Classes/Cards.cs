using BattleMiners.Interfaces;
using GenericClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BattleMiners.Classes;

public class Cards : ICards
{
    private readonly IGenericClient _nbmClient;
    private readonly ITemplates _templates;
    private readonly IConfiguration _config;
    private readonly ILogger<Collections> _logger;
    private readonly Dictionary<int, Card> _cards = new();

    public Cards(IGenericClient nbmClient, ITemplates templates, IConfiguration config, ILogger<Collections> logger)
    {
        _nbmClient = nbmClient;
        _templates = templates;
        _config = config;
        _logger = logger;
    }

    public async Task<Dictionary<int, Card>> GetCards()
    {
        if (_cards.Count > 0)
            return _cards;
        
        foreach (var template in _templates.GetTemplates())
        {
            var url = new Uri($"{_config["NbmUrl"]}/card_type/template/{template.Key}");
            var list = await _nbmClient.GetData<List<Card>>(url, CancellationToken.None).ConfigureAwait(false);
            list?.ForEach(c => { _cards.TryAdd(c.id, c); });
        }

        return _cards;
    }
}