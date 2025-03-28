﻿using GadekHotspring.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GadekHotspring.Controllers
{
    public class TourPackagesController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var tourPackage = liteDbHelper.GetFullHotelViewModel().TourPackages.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (tourPackage == null)
            {
                return NotFound();
            }

            return View(tourPackage);
        }

        public IActionResult Inquiry(string name)
        {
            var liteDbHelper = new LiteDbHelper();

            var tourPackage = liteDbHelper.GetFullHotelViewModel().TourPackages.FirstOrDefault(p => p.Name.ToSeoFriendly().ToLower() == name.ToLower());
            if (tourPackage == null)
            {
                return NotFound();
            }

            return View(tourPackage);
        }
    }
}