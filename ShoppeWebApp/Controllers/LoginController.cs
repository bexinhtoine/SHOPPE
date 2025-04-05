using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using ShoppeWebApp.Data;
using ShoppeWebApp.Models;

namespace ShoppeWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShoppeWebAppContext _context = null!;
        public LoginController(ShoppeWebAppContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginHandle(string username, string password)
        {
            var taikhoans = _context.TaiKhoans.Where(
                tk => tk.Username == username && tk.Password == password);
            foreach(var i in taikhoans)
            {
                return RedirectToAction("LoginSuccess");
            }
            return RedirectToAction("LoginFailed");
        }
        public string LoginFailed()
        {
            return "Bạn đã đăng nhập không thành công!";
        }
        public string LoginSuccess()
        {
            return "Bạn đã đăng nhập thành công!";
        }
    }
}
