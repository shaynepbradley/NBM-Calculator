using Alcor.Models;

namespace AtomicAssets.Models;

public class Asset<T>
{
    public string contract { get; set; }
    public string asset_id { get; set; }
    public string owner { get; set; }
    public string name { get; set; }
    public bool is_transferable { get; set; }
    public bool is_burnable { get; set; }
    public string template_mint { get; set; }
    public Collection collection { get; set; }
    public Schema schema { get; set; }
    public Template template { get; set; }
    public List<Token> backed_tokens { get; set; }
    public Dictionary<string, dynamic> immutable_data { get; set; }
    public Dictionary<string, dynamic> mutable_data { get; set; }
    public T data { get; set; }
    public string burned_by_account { get; set; }
    public string burned_at_block { get; set; }
    public string burned_at_time { get; set; }
    public string updated_at_block { get; set; }
    public string updated_at_time { get; set; }
    public string transferred_at_block { get; set; }
    public string transferred_at_time { get; set; }
    public string minted_at_block { get; set; }
    public string minted_at_time { get; set; }
}