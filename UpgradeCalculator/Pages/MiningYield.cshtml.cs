using Alcor;
using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using GenericClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Pages;

public class MiningYieldModel : PageModel
{
    private readonly IAlcorClient _alcorClient;
    private readonly IGenericClient _genericClient;
    private readonly IConfiguration _config;
    private readonly ILogger<IndexModel> _logger;
    public Resources ResourceValues { get; set; } = new();
    public Resources ResourcePerHour { get; set; } = new();
    
    public MiningYieldModel(IAlcorClient alcorClient, IGenericClient genericClient, IConfiguration config, ILogger<IndexModel> logger)
    {
        _alcorClient = alcorClient;
        _genericClient = genericClient;
        _config = config;
        _logger = logger;
    }

    public async Task OnGet(string id)
    {
        ResourceValues = await Infrastructure<Lands>.GetCurrentPrices(_alcorClient, _config, _logger).ConfigureAwait(false);
        var mining = await _genericClient.GetData<List<MiningOperation>>(new Uri($"https://sockdev.nftbattleminers.com:8890/user/{id}/mining_operations"), CancellationToken.None).ConfigureAwait(false);
        foreach (var item in mining)
        {
            switch (item.resource_type)
            {
                case WalletItem.ResIds.Fusium:
                    ResourcePerHour.Fusium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Actium:
                    ResourcePerHour.Actium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Minium:
                    ResourcePerHour.Minium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Constructium:
                    ResourcePerHour.Constructium += item.amount_ph;
                    break;
            }
        }
    }
}
