namespace Alcor.Models;

public class Balance
{
    public string balance { get; set; }
    public string Name => balance.Split(" ")[1];
    public float Amount => float.Parse(balance.Split(" ")[0]);
}