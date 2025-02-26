using Microsoft.AspNetCore.Mvc;

namespace GadekHotspring.Controllers
{
    public class PhotosController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}