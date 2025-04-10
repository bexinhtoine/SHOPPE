using Microsoft.AspNetCore.Mvc;
using ShoppeWebApp.Data;
using ShoppeWebApp.Models;

namespace ShoppeWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ShoppeWebAppContext _context;

        public AdminController(ShoppeWebAppContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            // Lấy dữ liệu từ CSDL
            var model = new HomeViewModel
            {
                TotalUsers = _context.NguoiDungs.Count(), 
                TotalSellers = _context.CuaHangs.Count(), 
                TotalOrders = _context.DonHangs.Count(), 
                TotalProducts = _context.SanPhams.Count() 
            };

            return View(model);
        }
    }
}