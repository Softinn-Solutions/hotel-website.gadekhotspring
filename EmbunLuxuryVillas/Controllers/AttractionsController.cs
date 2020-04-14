using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class AttractionsController : BaseController
    {
        public IActionResult Index(string attractionType)
        {
            ViewBag.AttractionType = attractionType;
            return View();
        }
    }
}