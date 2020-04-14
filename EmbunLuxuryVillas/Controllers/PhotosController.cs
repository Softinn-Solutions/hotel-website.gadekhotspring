using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class PhotosController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}