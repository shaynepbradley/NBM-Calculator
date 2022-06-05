using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UpgradeCalculator.Pages
{
    public class SetUserModel : PageModel
    {
        public void OnGet([FromQuery]string userId, [FromQuery] string ru)
        {
            ViewData["userId"] = userId;
            ViewData["ru"] = ru;
        }
    }
}
