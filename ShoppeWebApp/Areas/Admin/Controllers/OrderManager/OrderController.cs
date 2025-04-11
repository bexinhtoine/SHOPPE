using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Admin.Controllers.OrderManager
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
