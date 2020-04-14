using EmbunLuxuryVillas.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Softinn.EntityModels.ViewModel;
using System.Collections.Generic;

namespace EmbunLuxuryVillas.Controllers
{
    public class MeetingEventsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var mice = liteDbHelper.GetFullHotelViewModel().Mices.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (mice == null)
            {
                return NotFound();
            }

            return View(mice);
        }

        public IActionResult Inquiry(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var mice = liteDbHelper.GetFullHotelViewModel().Mices.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (mice == null)
            {
                return NotFound();
            }

            return View(mice);
        }
    }
}