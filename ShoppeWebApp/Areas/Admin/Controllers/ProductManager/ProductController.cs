using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Admin.Controllers.ProductManager
{
    public class ProductController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
