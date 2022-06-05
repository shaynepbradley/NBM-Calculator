namespace AtomicAssets.Models;

public class Sale<T>
{
    public string market_contract { get; set; }
    public string assets_contract { get; set; }
    public string sale_id { get; set; }
    public string seller { get; set; }
    public string buyer { get; set; }
    public string offer_id { get; set; }

    public Price price { get; set; }
    public string listing_price { get; set; }
    public string listing_symbol { get; set; }
    public List<Asset<T>> assets { get; set; }
    public string? maker_marketplace { get; set; }
    public string? taker_marketplace { get; set; }
    public Collection collection { get; set; }
    public uint state { get; set; }
    public string updated_at_block { get; set; }
    public string updated_at_time { get; set; }
    public string created_at_block { get; set; }
    public string created_at_time { get; set; }
}