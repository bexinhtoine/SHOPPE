using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Admin.Controllers.ProductManager
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
