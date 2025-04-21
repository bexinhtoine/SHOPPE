using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Areas.Customer.Controllers
{
    [Authorize("Customer")]
    [Area("Customer")]
    public class AllProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

