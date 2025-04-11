using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Admin.Controllers.Accountmanager
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
