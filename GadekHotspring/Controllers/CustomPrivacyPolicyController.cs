using Microsoft.AspNetCore.Mvc;

namespace GadekHotspring.Controllers
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