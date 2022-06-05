namespace AtomicAssets.Models;

public class Price
{
    public string amount { get; set; }
    public int token_precision { get; set; }
    public string token_contract { get; set; }
    public string token_symbol { get; set; }
}