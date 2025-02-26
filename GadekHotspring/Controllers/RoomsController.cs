using GadekHotspring.Helpers;
using Microsoft.AspNetCore.Mvc;
using Softinn.EntityModels.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace GadekHotspring.Controllers
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