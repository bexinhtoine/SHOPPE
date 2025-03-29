using Microsoft.AspNetCore.Mvc;

namespace ShoppeWebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
