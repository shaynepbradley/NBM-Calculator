using Alcor.Models;

namespace Alcor
{
    public interface IAlcorClient
    {
        public Task<T?> GetData<T>(string market, bool deals, CancellationToken cancelToken);

        public Task<bool> IsValidMarketName(string market);

        public Task<MarketInfo?> GetMarketInfo(string market, CancellationToken cancelToken);
        public Task<MarketInfo?> GetMarketInfo(int marketId, CancellationToken cancelToken);

        public Task<List<Transaction>?> GetTransactions(string market, CancellationToken cancelToken);

        public Task<T> GetDefiData<T>(int id) where T : new();

        public Task<string> GetDefiData(int id, CancellationToken cancelToken);
    }
}
