using System.Diagnostics;
using GenericClient;
using UpgradeCalculator.Interfaces;
using UpgradeCalculator.Pages;

namespace UpgradeCalculator.Classes;

public class MiningInfo : IMiningInfo
{
    private readonly IWaxId _id;
    private readonly IGenericClient _genericClient;
    private readonly IConfiguration _config;
    private readonly ILogger<MiningInfo> _logger;
    private Resources _perHour = new();
    private Resources _available = new();
    private DateTime _lastUpdated = DateTime.MinValue;

    public MiningInfo(IWaxId id, IGenericClient genericClient, IConfiguration config, ILogger<MiningInfo> logger)
    {
        _id = id;
        _genericClient = genericClient;
        _config = config;
        _logger = logger;
    }

    public async Task<Resources> CurrentlyAvailable()
    {
        await GetAssetInfo().ConfigureAwait(false);
        return _available;
    }

    public async Task<Resources> TotalPerHourProduction()
    {
        await GetAssetInfo().ConfigureAwait(false);
        return _perHour;
    }

    private async Task GetAssetInfo()
    {
        if (_lastUpdated.AddMinutes(65 - _lastUpdated.Minute) > DateTime.UtcNow)
            return;

        var wallet = await _genericClient
            .GetData<List<WalletItem>>(new Uri($"https://sockdev.nftbattleminers.com:8890/user/{_id.GetId()}/balance"),
                CancellationToken.None).ConfigureAwait(false);
        if (wallet == null)
            return;

        _available.Fusium = wallet.Find(w => w.resource_id == 1)!.amount;
        _available.Actium = wallet.Find(w => w.resource_id == 2)!.amount;
        _available.Minium = wallet.Find(w => w.resource_id == 3)!.amount;
        _available.Constructium = wallet.Find(w => w.resource_id == 4)!.amount;

        var mining = await _genericClient
            .GetData<List<MiningOperation>>(
                new Uri($"https://sockdev.nftbattleminers.com:8890/user/{_id.GetId()}/mining_operations"),
                CancellationToken.None).ConfigureAwait(false);
        if (mining == null)
            return;

        foreach (var item in mining)
        {
            switch (item.resource_type)
            {
                case WalletItem.ResIds.Fusium:
                    _available.Fusium += item.tokens_mined;
                    _perHour.Fusium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Actium:
                    _available.Actium += item.tokens_mined;
                    _perHour.Actium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Minium:
                    _available.Minium += item.tokens_mined;
                    _perHour.Minium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Constructium:
                    _available.Constructium += item.tokens_mined;
                    _perHour.Constructium += item.amount_ph;
                    break;
            }
        }
        _lastUpdated = DateTime.UtcNow;
    }
}