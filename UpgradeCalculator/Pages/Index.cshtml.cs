using Alcor;
using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;
using GenericClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UpgradeCalculator.Classes;
using Construction = UpgradeCalculator.Classes.Construction;

namespace UpgradeCalculator.Pages;

public class IndexModel : PageModel
{
    private readonly IAlcorClient _alcorClient;
    private readonly IAtomicClient<BattleMiners, Construction> _atomicClient;
    private readonly IGenericClient _genericClient;
    private readonly IConfiguration _config;
    private readonly ILogger<IndexModel> _logger;
    public Resources ResourceValues { get; set; } = new();
    public Resources AvailableResources { get; set; } = new();
    public Resources ResourcePerHour { get; set; } = new();
    public List<Schema> Schemas { get; set; } = new();
    public List<Template> Templates { get; set; } = new();
    public List<WalletItem>? Wallet { get; set; } = new();
    public List<MiningOperation>? Mining { get; set; } = new();
    public List<Asset<Construction>> Assets { get; set; } = new();
    public List<Construction> Construction { get; set; } = new();
    public List<Construction> Common { get; set; } = null!;
    public List<Construction> Rare { get; set; } = null!;
    public List<Construction> Epic { get; set; } = null!;
    public List<Construction> Legendary { get; set; } = null!;
    public List<Construction> Ultimate { get; set; } = null!;

    public IndexModel(IAlcorClient alcorClient, IAtomicClient<BattleMiners, Construction> atomicClient,
        IGenericClient genericClient, IConfiguration config, ILogger<IndexModel> logger)
    {
        _alcorClient = alcorClient;
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

        (Schemas, Templates, Assets) = await Infrastructure<Construction>.FetchData(userId, _atomicClient, _logger);
        ResourceValues = await Infrastructure<Lands>.GetCurrentPrices(_alcorClient, _config, _logger)
            .ConfigureAwait(false);

        var wallet = await _genericClient
            .GetData<List<WalletItem>>(new Uri($"https://sockdev.nftbattleminers.com:8890/user/{userId}/balance"),
                CancellationToken.None).ConfigureAwait(false);
        AvailableResources.Fusium = wallet.Find(w => w.resource_id == 1)!.amount;
        AvailableResources.Actium = wallet.Find(w => w.resource_id == 2)!.amount;
        AvailableResources.Minium = wallet.Find(w => w.resource_id == 3)!.amount;
        AvailableResources.Constructium = wallet.Find(w => w.resource_id == 4)!.amount;
        var mining = await _genericClient
            .GetData<List<MiningOperation>>(
                new Uri($"https://sockdev.nftbattleminers.com:8890/user/{userId}/mining_operations"),
                CancellationToken.None).ConfigureAwait(false);
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

        var allSales = await GetConstruction().ConfigureAwait(false);
        if (allSales.Count == 100)
        {
            Common = await GetConstruction("Common").ConfigureAwait(false);
            var ownedCommon = Assets.Where(a => a.data.rarity == "Common")
                .ToList();
            Common.AddRange(ownedCommon.Select(a => a.data));
            Construction.AddRange(Common);
            Common = Common.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            //if (Common.Count > 16)
            //    Common.RemoveRange(16, Common.Count - 16);

            Rare = await GetConstruction("Rare").ConfigureAwait(false);
            Rare.AddRange(Assets.Where(a => a.data.rarity == "Rare")
                .Select(a => a.data));
            Construction.AddRange(Rare);
            Rare = Rare.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            //if (Rare.Count > 8)
            //    Rare.RemoveRange(8, Rare.Count - 8);
            Epic = await GetConstruction("Epic").ConfigureAwait(false);
            Epic.AddRange(Assets.Where(a => a.data.rarity == "Epic")
                .Select(a => a.data));
            Construction.AddRange(Epic);
            Epic = Epic.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            //if (Epic.Count > 4)
            //    Epic.RemoveRange(4, Epic.Count - 4);
            Legendary = await GetConstruction("Legendary").ConfigureAwait(false);
            Legendary.AddRange(Assets.Where(a => a.data.rarity == "Legendary")
                .Select(a => a.data));
            Construction.AddRange(Legendary);
            Legendary = Legendary.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Ultimate = await GetConstruction("Ultimate").ConfigureAwait(false);
            Ultimate.AddRange(Assets.Where(a => a.data.rarity == "Ultimate")
                .Select(a => a.data));
            Construction.AddRange(Ultimate);
            Ultimate = Ultimate.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
        }
        else
        {
            Common = allSales.Where(a => a.rarity == "Common").ToList();
            var ownedCommon = Assets.Where(a => a.data.rarity == "Common").ToList();
            Common.AddRange(ownedCommon.Select(a => a.data));
            Construction.AddRange(Common);
            Common = Common.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Rare = allSales.Where(a => a.rarity == "Rare").ToList();
            Rare.AddRange(Assets.Where(a => a.data.rarity == "Rare").Select(a => a.data));
            Construction.AddRange(Rare);
            Rare = Rare.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Epic = allSales.Where(a => a.rarity == "Epic").ToList();
            Epic.AddRange(Assets.Where(a => a.data.rarity == "Epic").Select(a => a.data));
            Construction.AddRange(Epic);
            Epic = Epic.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Legendary = allSales.Where(a => a.rarity == "Legendary").ToList();
            Legendary.AddRange(Assets.Where(a => a.data.rarity == "Legendary").Select(a => a.data));
            Construction.AddRange(Legendary);
            Legendary = Legendary.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
            Ultimate = allSales.Where(a => a.rarity == "Ultimate").ToList();
            Ultimate.AddRange(Assets.Where(a => a.data.rarity == "Ultimate").Select(a => a.data));
            Construction.AddRange(Ultimate);
            Ultimate = Ultimate.OrderBy(c => c.PriceToLevel5(ResourceValues.Total)).ToList();
        }

        return new PageResult();
    }


    private async Task<List<Construction>> GetConstruction(string rarity = "")
    {
        try
        {
            var data = await _atomicClient.GetSales(rarity).ConfigureAwait(false);
            return Infrastructure<Construction>.AddNft(data, ResourceValues.Wax, _logger);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
