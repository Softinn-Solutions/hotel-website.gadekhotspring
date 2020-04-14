using Microsoft.AspNetCore.Mvc;
using System;

namespace EmbunLuxuryVillas.Controllers
{
    public class ReservationController : BaseController
    {
        public IActionResult Index(DateTime startDate, DateTime endDate)
        {
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-dd");

            return View();
        }
    }
}