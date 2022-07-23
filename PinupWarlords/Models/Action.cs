using System.Text.Json.Serialization;

namespace PinupWarlords.Models
{
    public class Action<T>
    {
        [JsonPropertyName("@timestamp")]
        public string Timestamp { get; set; }
        public string timestamp { get; set; }
        public ulong block_num { get; set; }
        public string trx_id { get; set; }
        public Act<T> act { get; set; }
        public List<string> notified { get; }
        public ulong global_sequence { get; set; }
        public string producer { get; set; }
        public int action_ordinal { get; set; }
        public int creator_action_ordinal { get; set; }
    }
}
