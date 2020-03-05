using System.Collections.Generic;
using System.Linq;
using EmbunLuxuryVillas.Helpers;
using EmbunLuxuryVillas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Softinn.EntityModels.ViewModel;

namespace EmbunLuxuryVillas.Controllers
{
    public class RoomsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string name)
        {
            var liteDbHelper = new LiteDbHelper();
            List<RoomTypeViewModel> rooms = liteDbHelper.GetFullHotelViewModel().RoomTypes;

            if (!rooms.Any(r => r.Name.ToSeoFriendly().ToLower() == name.ToLower()))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Room = rooms.FirstOrDefault(r => r.Name.ToSeoFriendly().ToLower() == name.ToLower());
            ViewBag.OtherRooms = rooms.Where(r => r.Name.ToSeoFriendly().ToLower() != name.ToLower()).ToList();
            ViewBag.AdditionalRoomInfo = BvHelper.GetAdditionalRoomInfo(ViewBag.Room.Name);

            return View();
        }
    }
}