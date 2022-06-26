using GenericClient;
using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Classes;

public class Schemas : ISchemas
{
    private readonly IGenericClient _nbmClient;
    private readonly IConfiguration _config;
    private readonly ILogger<Schemas> _logger;
    private readonly Dictionary<int, Schema> _schemas = new ();

    public Schemas(IGenericClient nbmClient, IConfiguration config, ILogger<Schemas> logger)
    {
        _nbmClient = nbmClient;
        _config = config;
        _logger = logger;
    }
    public async Task<Dictionary<int, Schema>> GetSchemas()
    {
        if (_schemas.Count > 0)
            return _schemas;

        var url = new Uri($"{_config["NbmUrl"]}/schemas");
        var list = await _nbmClient.GetData<List<Schema>>(url, CancellationToken.None).ConfigureAwait(false);
        list?.ForEach(c => { _schemas.TryAdd(c.id, c); });
        return _schemas;
    }
}