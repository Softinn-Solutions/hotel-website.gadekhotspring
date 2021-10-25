using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EmbunLuxuryVillas.Controllers
{
    public class DiningController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}