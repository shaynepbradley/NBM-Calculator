namespace PinupWarlords.Models;

public class Act<T>
{
    public string account { get; set; }
    public string name { get; set; }
    public T data { get; set; }
}