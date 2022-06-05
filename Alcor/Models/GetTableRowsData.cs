namespace Alcor.Models
{
    public class GetTableRowsData
    {
        public bool json { get; set; } = true;
        public string code { get; set; }
        public string scope { get; set; }
        public string table { get; set; }
        public object lower_bound { get; set; }
        public object upper_bound { get; set; }
        public object index_position { get; set; } = 1;
        public string key_type { get; set; } = "i64";
        public int limit { get; set; }
        public bool reverse { get; set; } = false;
        public bool show_payer { get; set; } = false;
    }
}
