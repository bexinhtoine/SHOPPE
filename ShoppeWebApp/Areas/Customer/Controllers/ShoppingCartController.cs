using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Customer.Controllers
{
    public class ShoppingCartController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
