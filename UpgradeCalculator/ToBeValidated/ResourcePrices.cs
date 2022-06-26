using System.Diagnostics;
using Alcor;
using Alcor.Models;
using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Classes
{
    public class ResourcePrices : Resources, IResourcePrices
    {
        private readonly IAlcorClient _client;
        private readonly ILogger<ResourcePrices> _logger;

        public ResourcePrices(IAlcorClient client, ILogger<ResourcePrices> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Resources> Current()
        {

            var watch = new Stopwatch();
            _logger.LogInformation("Calling GetCurrentPrices");
            watch.Start();
            var values = await GetCurrentPrices(_client).ConfigureAwait(false);
            watch.Stop();
            _logger.LogInformation($"Calling GetCurrentPrices required {watch.ElapsedMilliseconds} ms");
            watch.Reset();

            return values;
        }

        private static async Task<Resources> GetCurrentPrices(IAlcorClient alcorClient)
        {
            var results = new Resources();
            var json = await alcorClient.GetDefiData(51, CancellationToken.None).ConfigureAwait(false);
            var defiData = await alcorClient.GetDefiData<DefiResponse>(51).ConfigureAwait(false);
            if (defiData.code == 0 && defiData.message == "success")
            {
                results.Wax = float.Parse(defiData.waxUsdtPrice);
                results.Minium = defiData.data[0].price;
            }
            defiData = await alcorClient.GetDefiData<DefiResponse>(29).ConfigureAwait(false);
            if (defiData.code == 0)
                results.Constructium = defiData.data.FirstOrDefault(d => d.symbol1 == "NBMCON").price;
            defiData = await alcorClient.GetDefiData<DefiResponse>(53).ConfigureAwait(false);
            if (defiData.code == 0)
                results.Actium = defiData.data[0].price;
            defiData = await alcorClient.GetDefiData<DefiResponse>(52).ConfigureAwait(false);
            if (defiData.code == 0)
                results.Fusium = defiData.data[0].price;

            return results;
        }

    }
}
