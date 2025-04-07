using Microsoft.AspNetCore.Mvc;
using ShoppeWebApp.Models;
using ShoppeWebApp.Data;
using System;

namespace ShoppeWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShoppeWebAppContext _context;

        public LoginController(ShoppeWebAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Trang đăng nhập (nếu có)
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Trả về trang đăng ký
        }

        [HttpPost]
        public IActionResult Register(NguoiDung model, string MatKhau, string XacNhanMatKhau)
        {
            // ✅ Bước 1: Kiểm tra xác nhận mật khẩu
            if (MatKhau != XacNhanMatKhau)
            {
                ModelState.AddModelError("", "Mật khẩu xác nhận không khớp.");
                return View(model);
            }

            // ✅ Bước 2: Kiểm tra email đã tồn tại chưa
            var existingUser = _context.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email đã được sử dụng.");
                return View(model);
            }

            // ✅ Bước 3: Tạo ID ngẫu nhiên cho người dùng
            var newId = Guid.NewGuid().ToString("N")[..10].ToUpper();

            // ✅ Bước 4: Tạo đối tượng NguoiDung
            var nguoiDung = new NguoiDung
            {
                IdNguoiDung = newId,
                HoVaTen = model.HoVaTen,
                Email = model.Email,
                Sdt = model.Sdt,
                Cccd = "000000000000", // Tạm thời
                DiaChi = "Chưa cập nhật",
                VaiTro = 0,
                TrangThai = 1,
                ThoiGianTao = DateTime.Now
            };

            // ✅ Bước 5: Tạo tài khoản tương ứng
            var taiKhoan = new TaiKhoan
            {
                Username = model.Email,
                Password = MatKhau,
                IdNguoiDung = newId
            };

            try
            {
                // ✅ Bước 6: Lưu vào database
                _context.NguoiDungs.Add(nguoiDung);
                _context.TaiKhoans.Add(taiKhoan);
                _context.SaveChanges();

                // ✅ Bước 7: Chuyển về trang đăng nhập
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // ❌ Trường hợp lỗi database
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(model);
            }
        }
    }
}
