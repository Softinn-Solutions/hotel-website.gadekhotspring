using Microsoft.AspNetCore.Mvc;

namespace GadekHotspring.Controllers
{
    public class DiningController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}