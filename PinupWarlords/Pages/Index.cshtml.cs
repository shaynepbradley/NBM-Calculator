using Microsoft.AspNetCore.Mvc.RazorPages;
using PinupWarlords.Models;
using System.Text;
using System.Text.Json;

namespace PinupWarlords.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Models.Action<FindOpponent>>? FindOpponent { get; set; }
        public List<Models.Action<PrepBattle?>>? PrepBattle { get; set; }
        public List<Models.Action<BattleResults>>? BattleResults { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            var client = new EosrioClient(_logger);
            string? json = await client.GetData<string>("v2/history/get_actions?account=pinupwarlord&filter=pinupwarlord%3Afindopponent&skip=0&limit=5&sort=desc", CancellationToken.None).ConfigureAwait(false);
            var obj = JsonDocument.Parse(json) ?? throw new ArgumentNullException("JsonNode.Parse(json)");
            var success = obj.RootElement.TryGetProperty("actions", out var actions);
            FindOpponent = JsonSerializer.Deserialize<List<Models.Action<FindOpponent>>>(actions.GetRawText());

            var resp2 = await client.GetData<string>("v2/history/get_actions?account=pinupwarlord&filter=pinupwarlord%3Aprepbattle&skip=0&limit=5&sort=desc", CancellationToken.None).ConfigureAwait(false);
            obj = JsonDocument.Parse(resp2) ?? throw new ArgumentNullException("JsonNode.Parse(json)");
            success = obj.RootElement.TryGetProperty("actions", out actions);
            PrepBattle = JsonSerializer.Deserialize<List<Models.Action<PrepBattle>>>(actions.GetRawText());

            var resp3 = await client.GetData<string>("v2/history/get_actions?account=pinupwarlord&filter=pinupwarlord%3Abattleresult&skip=0&limit=10&sort=desc", CancellationToken.None).ConfigureAwait(false);
            obj = JsonDocument.Parse(resp3) ?? throw new ArgumentNullException("JsonNode.Parse(json)");
            success = obj.RootElement.TryGetProperty("actions", out actions);
            BattleResults = JsonSerializer.Deserialize<List<Models.Action<BattleResults>>>(actions.GetRawText());
        }
    }
}