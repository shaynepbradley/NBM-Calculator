using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;
using GenericClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UpgradeCalculator.Classes;
using UpgradeCalculator.Interfaces;
using UpgradeCalculator.ToBeValidated;
using Schema = UpgradeCalculator.Classes.Schema;
using Template = UpgradeCalculator.Classes.Template;

namespace UpgradeCalculator.Pages;

public class LandsModel : PageModel
{
    private readonly IResourcePrices _resourcePrices;
    private readonly IAtomicClient<BattleMiners, object> _atomicClient;
    private readonly IGenericClient _genericClient;
    private readonly IConfiguration _config;
    private readonly ILogger<IndexModel> _logger;
    /*
    public Resources ResourceValues { get; set; } = new();
    public Resources AvailableResources { get; set; } = new();
    public Resources ResourcePerHour { get; set; } = new();
    public List<Asset<Lands>> Assets { get; set; } = new();
    public List<Lands> Lands { get; set; } = new();
    public List<Lands> Common { get; set; } = null!;
    public List<Lands> Rare { get; set; } = null!;
    public List<Lands> Epic { get; set; } = null!;
    public List<Lands> Legendary { get; set; } = null!;
    public List<Lands> Ultimate { get; set; } = null!;

    public LandsModel(IResourcePrices resourcePrices, IAtomicClient<BattleMiners, Lands> atomicClient, IGenericClient genericClient, IConfiguration config, ILogger<IndexModel> logger)
    {
        _resourcePrices = resourcePrices;
        _atomicClient = atomicClient;
        _genericClient = genericClient;
        _config = config;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet([FromQuery] string userId)
    {
        ViewData["userId"] = userId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            userId = "cnrzi.wam";
        }
        /*
        Assets = await Infrastructure<Lands>.FetchData(userId, _atomicClient, _logger);
        ResourceValues = await _resourcePrices.Current().ConfigureAwait(false);

        var wallet = await _genericClient.GetData<List<WalletItem>>(new Uri($"https://sockdev.nftbattleminers.com:8890/user/{userId}/balance"), CancellationToken.None).ConfigureAwait(false);
        AvailableResources.Fusium = wallet.Find(w => w.resource_id == 1)!.amount;
        AvailableResources.Actium = wallet.Find(w => w.resource_id == 2)!.amount;
        AvailableResources.Minium = wallet.Find(w => w.resource_id == 3)!.amount;
        AvailableResources.Constructium = wallet.Find(w => w.resource_id == 4)!.amount;
        var mining = await _genericClient.GetData<List<MiningOperation>>(new Uri($"https://sockdev.nftbattleminers.com:8890/user/{userId}/mining_operations"), CancellationToken.None).ConfigureAwait(false);
        foreach (var item in mining)
        {
            switch (item.resource_type)
            {
                case WalletItem.ResIds.Fusium:
                    AvailableResources.Fusium += item.tokens_mined;
                    ResourcePerHour.Fusium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Actium:
                    AvailableResources.Actium += item.tokens_mined;
                    ResourcePerHour.Actium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Minium:
                    AvailableResources.Minium += item.tokens_mined;
                    ResourcePerHour.Minium += item.amount_ph;
                    break;
                case WalletItem.ResIds.Constructium:
                    AvailableResources.Constructium += item.tokens_mined;
                    ResourcePerHour.Constructium += item.amount_ph;
                    break;
            }
        }

        var allSales = await GetLands().ConfigureAwait(false);
        if (allSales.Count == 100)
        {
            Common = await GetLands("Common").ConfigureAwait(false);
            var ownedCommon = Assets.Where(a => a.data.rarity == "Common")
                .ToList();
            Common.AddRange(ownedCommon.Select(a => a.data));
            Lands.AddRange(Common);
            Common = Common.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();

            Rare = await GetLands("Rare").ConfigureAwait(false);
            Rare.AddRange(Assets.Where(a => a.data.rarity == "Rare")
                .Select(a => a.data));
            Lands.AddRange(Rare);
            Rare = Rare.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();

            Epic = await GetLands("Epic").ConfigureAwait(false);
            Epic.AddRange(Assets.Where(a => a.data.rarity == "Epic")
                .Select(a => a.data));
            Lands.AddRange(Epic);
            Epic = Epic.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();

            Legendary = await GetLands("Legendary").ConfigureAwait(false);
            Legendary.AddRange(Assets.Where(a => a.data.rarity == "Legendary")
                .Select(a => a.data));
            Lands.AddRange(Legendary);
            Legendary = Legendary.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Ultimate = await GetLands("Ultimate").ConfigureAwait(false);
            Ultimate.AddRange(Assets.Where(a => a.data.rarity == "Ultimate")
                .Select(a => a.data));
            Lands.AddRange(Ultimate);
            Ultimate = Ultimate.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
        }
        else
        {
            Common = allSales.Where(a => a.rarity == "Common").ToList();
            var ownedCommon = Assets.Where(a => a.data.rarity == "Common").ToList();
            Common.AddRange(ownedCommon.Select(a => a.data));
            Lands.AddRange(Common);
            Common = Common.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Rare = allSales.Where(a => a.rarity == "Rare").ToList();
            Rare.AddRange(Assets.Where(a => a.data.rarity == "Rare").Select(a => a.data));
            Lands.AddRange(Rare);
            Rare = Rare.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Epic = allSales.Where(a => a.rarity == "Epic").ToList();
            Epic.AddRange(Assets.Where(a => a.data.rarity == "Epic").Select(a => a.data));
            Lands.AddRange(Epic);
            Epic = Epic.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Legendary = allSales.Where(a => a.rarity == "Legendary").ToList();
            Legendary.AddRange(Assets.Where(a => a.data.rarity == "Legendary").Select(a => a.data));
            Lands.AddRange(Legendary);
            Legendary = Legendary.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Ultimate = allSales.Where(a => a.rarity == "Ultimate").ToList();
            Ultimate.AddRange(Assets.Where(a => a.data.rarity == "Ultimate").Select(a => a.data));
            Lands.AddRange(Ultimate);
            Ultimate = Ultimate.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
        }
        
        return await Task.FromResult(new PageResult()).ConfigureAwait(false);
    }

    private async Task<List<Lands>> GetLands(string rarity = "")
    {
        try
        {
            var data = await _atomicClient.GetSales(rarity).ConfigureAwait(false);
            return Infrastructure<Lands>.AddNft(data, ResourceValues.Wax, _logger);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    */
}