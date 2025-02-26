using Microsoft.AspNetCore.Mvc;

namespace GadekHotspring.Controllers
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