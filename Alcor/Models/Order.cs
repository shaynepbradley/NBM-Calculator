namespace Alcor.Models;

public class Order
{
    public int order_id { get; set; }
    public string create_time { get; set; }
    public string resource_quantity { get; set; }
    public string xps_quantity { get; set; }
    public string rate { get; set; }
    public string creator { get; set; }

    public float ResourceQuantity => float.Parse(resource_quantity.Split(" ")[0]);
    public float XpsQuantity => float.Parse(xps_quantity.Split(" ")[0]);
    public float PricePreUnit => XpsQuantity / ResourceQuantity;
}