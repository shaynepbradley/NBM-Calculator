using System.Diagnostics;
using Alcor;
using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;
using GenericClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UpgradeCalculator.Classes;
using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Pages;

public class IndexModel : PageModel
{
    private readonly IWaxId _id;
    private readonly IResourcePrices _resourcePrices;
    private readonly IAtomicClient<BattleMiners, Construction> _atomicClient;
    private readonly IMiningInfo _miningInfo;
    private readonly IConfiguration _config;
    private readonly ILogger<IndexModel> _logger;
    public Resources ResourceValues { get; set; } = new();
    public Resources AvailableResources { get; set; } = new();
    public Resources ResourcePerHour { get; set; } = new();
    public Resources RepairPerHour { get; set; } = new();
    public List<WalletItem>? Wallet { get; set; } = new();
    public List<MiningOperation>? Mining { get; set; } = new();
    public List<Asset<Construction>> Assets { get; set; } = new();
    public List<Construction> Construction { get; set; } = new();
    public List<Construction> Common { get; set; } = null!;
    public List<Construction> Rare { get; set; } = null!;
    public List<Construction> Epic { get; set; } = null!;
    public List<Construction> Legendary { get; set; } = null!;
    public List<Construction> Ultimate { get; set; } = null!;
    public string UserId { get; set; } = null!;

    public IndexModel(IWaxId id, IResourcePrices resourcePrices, IAtomicClient<BattleMiners, Construction> atomicClient,
        IMiningInfo miningInfo, IConfiguration config, ILogger<IndexModel> logger)
    {
        _id = id;
        _resourcePrices = resourcePrices;
        _atomicClient = atomicClient;
        _miningInfo = miningInfo;
        _config = config;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet([FromQuery] string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToPage("SetWaxId", new { ru = "/index" });
        }
        ViewData["userId"] = userId;
        _id.SetId(userId);

        // Assets = await Infrastructure<Construction>.FetchData(UserId, _atomicClient, _logger).ConfigureAwait(false);

        Stopwatch watch = new Stopwatch();
        /*
        _logger.LogInformation("Calling GetCurrentPrices");
        watch.Start();
        ResourceValues = await _resourcePrices.Current().ConfigureAwait(false);
        watch.Stop();
        _logger.LogInformation($"Calling GetCurrentPrices required {watch.ElapsedMilliseconds} ms");
        watch.Reset();

        AvailableResources = await _miningInfo.CurrentlyAvailable().ConfigureAwait(false);

        _logger.LogInformation("Retrieving asset sale information");
        watch.Start();
        var allSales = await GetConstruction().ConfigureAwait(false);
        watch.Stop();
        _logger.LogInformation($"Retrieving asset sale information required {watch.ElapsedMilliseconds} ms");
        watch.Reset();

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
        }*/

        return new PageResult();
    }

    public async Task<IActionResult> OnPostGetResourceValues()
    {
        return new JsonResult(await _resourcePrices.Current().ConfigureAwait(false));
    }

    public async Task<IActionResult> OnPostGetAvailableResource()
    {
        return new JsonResult(await _miningInfo.CurrentlyAvailable().ConfigureAwait(false));
    }

    public IActionResult OnPostSetUserId([FromForm] string userid)
    {
        if (!string.IsNullOrWhiteSpace(userid))
        {
            _id.SetId(userid);
            _id.Version += 5;
            UserId = userid;
        }

        return new JsonResult(new { status = "OK" });
    }

    public async Task<IActionResult> OnPostGetResourcesPerHour()
    {
        return new JsonResult(await _miningInfo.TotalPerHourProduction().ConfigureAwait(false));
    }
    /*
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
    */
}
