namespace Alcor.Models;

public class TableRowResponse<T>
{
    public List<T> rows { get; set; }
    public bool more { get; set; }
    public string next_key { get; set; }
}