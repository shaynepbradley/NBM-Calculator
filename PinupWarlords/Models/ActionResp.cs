using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SignalR;

namespace PinupWarlords.Models
{
    public class ActionResp<T>
    {
        public double query_time_ms { get; set; }
        public bool cached { get; set; }
        public int lib { get; set; }
        public Value total { get; set; }

        public PinupWarlords.Models.Action<Task>[] actions { get; }
    }
}
