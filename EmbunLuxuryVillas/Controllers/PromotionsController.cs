using System.Linq;
using EmbunLuxuryVillas.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class PromotionsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            var liteDbHelper = new LiteDbHelper();

            var promotion = liteDbHelper.GetFullHotelViewModel().PromotionalEvents.FirstOrDefault(p => p.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }
    }
}