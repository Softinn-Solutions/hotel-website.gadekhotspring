using EmbunLuxuryVillas.Helpers;
using Microsoft.AspNetCore.Mvc;
using Softinn.EntityModels.ViewModel;
using System;

namespace EmbunLuxuryVillas.Controllers
{
    public class ReservationController : BaseController
    {
        public IActionResult Index(DateTime startDate, DateTime endDate, string promotionCode)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            string url = "https://booking.mysoftinn.com/bookHotelRoom/web?hotelId=" + hotelViewModel.Id;

            if (startDate != null && endDate != null && startDate.ToString() != "01-Jan-01 12:00:00 AM" && endDate.ToString() != "01-Jan-01 12:00:00 AM")
            {
                ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.ToString("yyyy-MM-dd");
                url = url + "&startDate=" + ViewBag.StartDate + "&endDate=" + ViewBag.EndDate;
            }

            url = url + "&themeColor=8DC53E&darkenColor=8DC53E";

            if (promotionCode != null)
            {
                url = url + "&promotionCode=" + promotionCode;
            }

            return Redirect(url);
        }
    }
}