namespace AtomicAssets.Models; 

public class Template
{
    public string name { get; set; }
    public string contract { get; set; }
    public string template_id { get; set; }
    public string max_supply { get; set; }
    public string issued_supply { get; set; }
    public bool is_transferable { get; set; }
    public bool is_burnable { get; set; }
    public string created_at_time { get; set; }
    public string created_at_block { get; set; }
    public Schema schema { get; set; }
    public Collection collection { get; set; }
    public Dictionary<string, dynamic> immutable_data { get; set; }
}