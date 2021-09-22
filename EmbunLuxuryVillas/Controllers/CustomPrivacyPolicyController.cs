using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class CustomPrivacyPolicyController : BaseController
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}