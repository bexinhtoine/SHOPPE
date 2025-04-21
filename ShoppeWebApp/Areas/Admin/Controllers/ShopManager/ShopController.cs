using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Areas.Admin.Controllers.ShopManager
{
    [Authorize("Admin")]
    [Area("Admin")]
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
