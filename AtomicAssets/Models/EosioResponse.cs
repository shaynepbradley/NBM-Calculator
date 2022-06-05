namespace AtomicAssets.Models;

[Serializable]
    public class EosioResponse<T>
    {
        public bool success { get; set; }
        public List<T> data { get; set; }
        public string message { get; set; }
        public ulong? query_time { get; set; }
    }
