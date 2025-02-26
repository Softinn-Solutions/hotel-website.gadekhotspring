using GadekHotspring.Helpers;
using Microsoft.AspNetCore.Mvc;
using Softinn.EntityModels.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace GadekHotspring.Controllers
{
    public class BlogsController : BaseController
    {
        // GET: Blog
        public IActionResult Index()
        {
            return View();
        }

        // GET: Blog
        public IActionResult Detail(string titleSlug)
        {
            var liteDbHelper = new LiteDbHelper();
            List<BlogViewModel> blogs = liteDbHelper.GetFullHotelViewModel().Blogs;

            var blog = blogs.FirstOrDefault(b => b.TitleSlug == titleSlug);

            if (blog == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Blog = blog;

            if (!blog.IsPublished)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}