using GenericClient;
using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Classes;

public class Templates : ITemplates
{
    private readonly IGenericClient _nbmClient;
    private readonly IConfiguration _config;
    private readonly ILogger<Templates> _logger;
    private readonly Dictionary<int, Template> _templates = new();
    private readonly object _lock = new();
    private Task[]? _loading;

    public Templates(IGenericClient nbmClient, IConfiguration config, ILogger<Templates> logger)
    {
        _nbmClient = nbmClient;
        _config = config;
        _logger = logger;
    }

    private async Task LoadTemplates()
    {
        _logger.LogInformation($"Loading templates at {DateTime.Now}");
        var url = new Uri($"{_config["NbmUrl"]}/templates");
        var list = await _nbmClient.GetData<List<Template>?>(url, CancellationToken.None).ConfigureAwait(false);
        list?.ForEach(t => { _templates.TryAdd(t.id, t); });
    }

    public Dictionary<int, Template> GetTemplates()
    {
        if (_loading != null)
        {
            Task.WaitAll(_loading);
        }

        if (_templates.Count > 0)
            return _templates;
        
        lock (_lock)
        {
            if (_templates.Count > 0)
                return _templates;

            _loading = new Task[1];
            _loading[0] = LoadTemplates();
            Task.WaitAll(_loading);
            _loading = null;
        }

        return _templates;
    }

    public Template? GetTemplateById(int templateId)
    {
        if (_templates.Count < 1)
            GetTemplates();
        return _templates.ContainsKey(templateId) ? _templates[templateId] : null;
    }
}