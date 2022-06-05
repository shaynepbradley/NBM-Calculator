namespace AtomicAssets.Models;

public class Schema
{
    public string contract { get; set; }
    public string schema_name { get; set; }
    public List<Property> format { get; set; }
    public string created_at_block { get; set; }
    public string created_at_time { get; set; }
    public Collection collection { get; set; }

}