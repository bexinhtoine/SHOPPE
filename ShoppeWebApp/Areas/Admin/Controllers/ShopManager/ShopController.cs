using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Admin.Controllers.ShopManager
{
    [Area("Admin")]
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
