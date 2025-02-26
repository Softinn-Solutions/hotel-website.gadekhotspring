using Microsoft.AspNetCore.Mvc;

namespace GadekHotspring.Controllers
{
    public class ContactUsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}