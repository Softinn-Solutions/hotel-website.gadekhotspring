﻿using GadekHotspring.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GadekHotspring.Controllers
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

            url = url + "&themeColor=125FA9&darkenColor=125FA9";

            if (promotionCode != null)
            {
                url = url + "&promotionCode=" + promotionCode;
            }

            return Redirect(url);
        }
    }
}