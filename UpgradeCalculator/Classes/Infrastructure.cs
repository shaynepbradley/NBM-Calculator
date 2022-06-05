using System.Diagnostics;
using Alcor;
using Alcor.Models;
using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;

namespace UpgradeCalculator.Classes;

public static class Infrastructure<T> where T : class, IAsset
{
    public static async Task<(List<Schema> schema, List<Template> templates, List<Asset<T>> assets)> FetchData(string id, IAtomicClient<BattleMiners, T> client, ILogger log)
    {
        Stopwatch watch = new Stopwatch();

        log.LogInformation("Retrieving Schemas");
        watch.Start();
        var schemas = await client.GetSchemas().ConfigureAwait(false);
        watch.Stop();
        log.LogInformation($"Retrieving schemas required {watch.ElapsedMilliseconds} ms, Count {schemas.Count}");
        watch.Reset();

        log.LogInformation("Retrieving Templates");
        watch.Start();
        var templates = await client.GetTemplates(string.Empty).ConfigureAwait(false);
        watch.Stop();
        log.LogInformation($"Retrieving Templates required {watch.ElapsedMilliseconds} ms, Count {templates.Count}");
        watch.Reset();

        log.LogInformation("Retrieving user assets");
        watch.Start();
        var assets = await client.GetAssets(id).ConfigureAwait(false);
        watch.Stop();
        log.LogInformation($"Retrieving user assets required {watch.ElapsedMilliseconds} ms, Count {assets.Count}");
        watch.Reset();

        assets.ForEach(a => a.data.AssetId = a.asset_id);

        return(schemas, templates, assets);
    }

    public static async Task<Resources> GetCurrentPrices(IAlcorClient alcorClient, IConfiguration config, ILogger log)
    {
        var results = new Resources();
        var defiData = await alcorClient.GetDefiData<DefiResponse>(51).ConfigureAwait(false);
        if (defiData.code == 0 && defiData.message == "success")
        {
            results.Wax = float.Parse(defiData.waxUsdtPrice);
            results.Minium = defiData.data[0].price;
        }
        defiData = await alcorClient.GetDefiData<DefiResponse>(29).ConfigureAwait(false);
        if (defiData.code == 0)
            results.Constructium = defiData.data[0].price;
        defiData = await alcorClient.GetDefiData<DefiResponse>(53).ConfigureAwait(false);
        if (defiData.code == 0)
            results.Actium = defiData.data[0].price;
        defiData = await alcorClient.GetDefiData<DefiResponse>(52).ConfigureAwait(false);
        if (defiData.code == 0)
            results.Fusium = defiData.data[0].price;

        return results;
    }

    public static List<T> AddNft(List<Sale<T>> resp, float waxPrice, ILogger log)
    {
        List<T> results = new();
        var schema = typeof(T).Name.ToLowerInvariant();

        foreach (var item in resp)
        {
            if (item.assets.Count < 1 || item.assets[0].schema.schema_name != schema)
                continue;

            var asset = item.assets[0].data;
            asset.Price = (float)(item.listing_symbol == "USD" ? long.Parse(item.listing_price) * (1 / waxPrice) / 100.0 : float.Parse(item.listing_price) / MathF.Pow(10, 8));
            if (asset.Price < 1)
                Debugger.Break();
            asset.AssetId = item.assets[0].asset_id;
            log.LogInformation($"{item.assets[0].asset_id} - {asset.name} - {asset.variant} (Original Price {item.listing_price} {item.listing_symbol}, adjusted price of {asset.Price})");
            results.Add(asset);
        }

        return results;
    }

}