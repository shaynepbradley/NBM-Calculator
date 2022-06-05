using GenericClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;

namespace AtomicAssets.Classes;

public class AtomicClient<T, T2> : IAtomicClient<T, T2> where T : class where T2 : class
{
    private readonly IGenericClient _client;
    private readonly ILogger<AtomicClient<T, T2>> _logger;
    private readonly IConfiguration _config;
    private static object _locker = new object();
    private static bool _updating = false;
    private readonly string _collection;
    private readonly List<Schema> _schemas = new();
    private readonly List<Template> _templates = new();

    public AtomicClient(IGenericClient client, ILogger<AtomicClient<T, T2>> logger, IConfiguration config)
    {
        _client = client;
        _logger = logger;
        _config = config;
        _collection = typeof(T).Name.ToLowerInvariant();
    }

    private async Task LoadSchemasAndTemplates()
    {
        try
        {
            lock (_locker)
            {
                _updating = true;
            }

            if (_schemas.Count == 0)
            {
                var api = $"/schemas?collection_name={_collection}&page=1&limit=100&order=desc&sort=created";
                var s = await _client.GetData<EosioResponse<Schema>>(api, CancellationToken.None).ConfigureAwait(false);
                if (s is { success: true })
                    _schemas.AddRange(s.data);
                else
                {
                    _logger.LogCritical($"Request failed with message {s?.message}");
                }
            }
            else
            {
                while (_updating)
                    Thread.Sleep(500);
            }

            if (_templates.Count == 0)
            {
                var more = true;
                var page = 1;
                while (more)
                {
                    var api =
                        $"/templates?collection_name={_collection}&has_assets=true&page={page}&limit=100&order=desc&sort=created";
                    var t = await _client.GetData<EosioResponse<Template>>(api, CancellationToken.None)
                        .ConfigureAwait(false);
                    if (t is { success: true })
                    {
                        if (t.data.Count != 0)
                        {
                            _templates.AddRange(t.data); 
                            page++;
                        }
                        else
                        {
                            more = false;
                        }
                    }
                    else
                    {
                        _logger.LogCritical($"Request failed with message {t?.message}");
                        more = false;
                    }
                }
            }
            else
            {
                while (_updating)
                    Thread.Sleep(500);
            }
        }
        finally
        {
            lock (_locker)
                _updating = false;
        }
    }

    public async Task<List<Schema>> GetSchemas()
    {
        if (_schemas.Count == 0)
            await LoadSchemasAndTemplates().ConfigureAwait(false);

        return _schemas;
    }

    public async Task<List<Template>> GetTemplates(string schema)
    {
        if (_templates.Count == 0)
            await LoadSchemasAndTemplates().ConfigureAwait(false);

        return string.IsNullOrWhiteSpace(schema) ? _templates : _templates.Where(t => t.schema.schema_name == schema).ToList();
    }

    public async Task<List<Asset<T2>>> GetAssets(string user)
    {
        string schema = typeof(T2).Name.ToLowerInvariant();
        List<Asset<T2>> results = new();
        var more = true;
        var page = 1;
        while (more)
        {
            var api =
                $"/assets?collection_name={_collection}&schema_name={schema}&owner={user}&page={page}&limit=100&order=desc&sort=created";
            var resp = await _client.GetData<EosioResponse<Asset<T2>>>(api, CancellationToken.None).ConfigureAwait(false);
            if (resp is { success: true })
            {
                if (resp.data.Count != 0)
                {
                    results.AddRange(resp.data);
                    page++;
                }
                else
                {
                    more = false;
                }
            }
            else
            {
                _logger.LogCritical($"Request failed with message {resp?.message}");
                more = false;
            }
        }

        return results;
    }

    public async Task<List<Sale<T2>>> GetSales(string rarity)
    {
        var count = rarity switch
        {
            "Common" => 16,
            "Rare" => 8,
            "Epic" => 4,
            "Legendary" => 2,
            _ => 1
        };

        //        var api = $"https://wax.api.atomicassets.io/atomicmarket/v1/sales/templates?symbol=WAX&collection_name=battleminers&schema_name={typeof(T2).Name.ToLowerInvariant()}&page=1&limit={count}&order=asc&sort=price&immutable_data:text.rarity={rarity}";
        var api = $"https://wax.api.atomicassets.io/atomicmarket/v2/sales?state=1&collection_name=battleminers&schema_name={typeof(T2).Name.ToLowerInvariant()}&page=1&limit=100&order=asc&sort=price";
        if (!string.IsNullOrEmpty(rarity))
            api += $"&immutable_data:text.rarity={rarity}";
        var a = await _client.GetData<EosioResponse<Sale<T2>>>(new Uri(api), CancellationToken.None).ConfigureAwait(false);
        switch (a)
        {
            case { success: true }:
                return a.data;
            default:
                _logger.LogCritical($"Request failed with message {a?.message}");
                break;
        }

        return new();
    }
}
