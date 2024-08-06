using System.Linq;
using EmbunLuxuryVillas.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class EventsController : BaseController
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var selectedEvent = liteDbHelper.GetFullHotelViewModel().Events.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (selectedEvent == null)
            {
                return NotFound();
            }

            return View(selectedEvent);
        }

        public IActionResult Inquiry(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var selectedEvent = liteDbHelper.GetFullHotelViewModel().Events.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (selectedEvent == null)
            {
                return NotFound();
            }

            return View(selectedEvent);
        }
    }
}