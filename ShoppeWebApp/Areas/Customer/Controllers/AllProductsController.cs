using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Customer.Controllers
{
    public class AllProductsController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

