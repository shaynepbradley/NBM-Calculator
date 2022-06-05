using System.Runtime.CompilerServices;
using AtomicAssets.Models;

namespace AtomicAssets.Interfaces;

public interface IAtomicClient<T, T2> where T : class where T2 : class
{
    public Task<List<Schema>> GetSchemas();
    public Task<List<Template>> GetTemplates(string schema);
    public Task<List<Asset<T2>>> GetAssets(string user);
    public Task<List<Sale<T2>>> GetSales(string rarity);
}

