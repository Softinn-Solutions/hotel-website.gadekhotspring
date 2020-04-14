using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class ContactUsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}