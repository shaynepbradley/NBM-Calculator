namespace AtomicAssets.Models;

[Serializable]
public class Collection
{
    public string collection_name { get; set; }
    public string name { get; set; }
    public string author { get; set; }
    public bool allow_notify { get; set; }
    public List<string> authorized_accounts { get; set; }
    public List<string> notify_accounts { get; set; }
    public float market_fee { get; set; }
    public string created_at_block { get; set; }
    public string created_at_time { get; set; }
}