using GadekHotspring.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace GadekHotspring.Controllers
{
    public class BaseController : Controller
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await base.OnActionExecutionAsync(context, next);

            var controller = context.Controller as Controller;
            if (controller == null) return;

            var liteDbHelper = new LiteDbHelper();

            controller.ViewBag.ViewModel = liteDbHelper.GetFullHotelViewModel();
        }
    }
}