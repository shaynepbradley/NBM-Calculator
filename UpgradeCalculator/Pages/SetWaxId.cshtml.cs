using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UpgradeCalculator.Interfaces;

namespace UpgradeCalculator.Pages
{
    public class SetWaxIdModel : PageModel
    {
        private readonly IWaxId _id;

        public SetWaxIdModel(IWaxId id)
        {
            _id = id;
        }
        public string ReturnUrl { get; set; }
        public string? WaxId { get; set; }

        public void OnGet([FromQuery]string ru)
        {
            ReturnUrl = ru;
            WaxId = _id.HasId() ?  _id.GetId() : string.Empty;
        }
    }
}
