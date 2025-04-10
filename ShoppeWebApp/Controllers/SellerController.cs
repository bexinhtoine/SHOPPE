using Microsoft.AspNetCore.Mvc;
using ShoppeWebApp.Models.Seller;
using ShoppeWebApp.Data;
using ShoppeWebApp.Models.Database;

namespace ShoppeWebApp.Controllers
{
    public class SellerController : Controller
    {
        private readonly ShoppeWebAppContext _context;

        public SellerController(ShoppeWebAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // Kiểm tra tất cả các trường không được để trống
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra mật khẩu xác nhận
            if (model.MatKhau != model.XacNhanMatKhau)
            {
                ModelState.AddModelError(nameof(model.XacNhanMatKhau), "Mật khẩu xác nhận không khớp.");
                return View(model);
            }

            // Kiểm tra email đã tồn tại
            var existingEmail = _context.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng.");
                return View(model);
            }

            // Kiểm tra số điện thoại đã tồn tại
            var existingPhone = _context.NguoiDungs.FirstOrDefault(u => u.Sdt == model.Sdt);
            if (existingPhone != null)
            {
                ModelState.AddModelError(nameof(model.Sdt), "Số điện thoại đã được sử dụng.");
                return View(model);
            }

            // Kiểm tra CCCD phải đủ 12 số
            if (model.Cccd.Length != 12 || !model.Cccd.All(char.IsDigit))
            {
                ModelState.AddModelError(nameof(model.Cccd), "CCCD phải là 12 chữ số.");
                return View(model);
            }

            // Tạo ID cho người bán
            var maxId = _context.NguoiDungs
                .OrderByDescending(u => u.IdNguoiDung)
                .Select(u => u.IdNguoiDung)
                .FirstOrDefault();

            string newId = string.IsNullOrEmpty(maxId) ? "0000000001" : (long.Parse(maxId) + 1).ToString("D10");

            var nguoiDung = new NguoiDung
            {
                IdNguoiDung = newId,
                HoVaTen = model.HoVaTen,
                Email = model.Email,
                Sdt = model.Sdt,
                Cccd = model.Cccd,
                DiaChi = "Chưa cập nhật",
                VaiTro = 1, // Vai trò người bán
                TrangThai = 1,
                ThoiGianTao = DateTime.Now
            };

            var taiKhoan = new TaiKhoan
            {
                Username = model.TenDangNhap,
                Password = model.MatKhau,
                IdNguoiDung = newId
            };

            try
            {
                _context.NguoiDungs.Add(nguoiDung);
                _context.TaiKhoans.Add(taiKhoan);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(model);
            }
        }
    }
}