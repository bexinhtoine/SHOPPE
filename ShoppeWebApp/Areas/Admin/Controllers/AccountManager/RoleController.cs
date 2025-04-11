using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Area.Admin.Controllers.AccountManager
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
