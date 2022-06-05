namespace Alcor.Models;

public class DefiResponse
{
    public int code { get; set; }
    public string message { get; set; }
    public List<DefiData> data { get; set; }
    public string waxUsdtPrice { get; set; }
}