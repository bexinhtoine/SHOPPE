using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Controllers
{
    public class AllProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
