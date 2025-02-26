using GadekHotspring.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GadekHotspring.Controllers
{
    public class MeetingsController : BaseController
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var meeting = liteDbHelper.GetFullHotelViewModel().Meetings.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        public IActionResult Inquiry(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var meeting = liteDbHelper.GetFullHotelViewModel().Meetings.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }
    }
}