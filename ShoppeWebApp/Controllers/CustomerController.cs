using Microsoft.AspNetCore.Mvc;
using ShoppeWebApp.Models.Login;
using ShoppeWebApp.Models.Database;
using ShoppeWebApp.Models.ViewModels;
using ShoppeWebApp.Data;


namespace ShoppeWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ShoppeWebAppContext _context; 

        public CustomerController(ShoppeWebAppContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~Views/Customer/Register.cshtml");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // Kiểm tra mật khẩu xác nhận
            if (model.MatKhau != model.XacNhanMatKhau)
            {
                ModelState.AddModelError(nameof(model.XacNhanMatKhau), "Mật khẩu xác nhận không khớp.");
                return View("Register", model);
            }
        
            // Kiểm tra email đã tồn tại
            var existingEmail = _context.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng.");
                return View("Register", model);
            }
        
            // Kiểm tra số điện thoại đã tồn tại
            var existingPhone = _context.NguoiDungs.FirstOrDefault(u => u.Sdt == model.Sdt);
            if (existingPhone != null)
            {
                ModelState.AddModelError(nameof(model.Sdt), "Số điện thoại đã được sử dụng.");
                return View("Register", model);
            }
        
            // Kiểm tra số điện thoại phải đủ 10 số
            if (model.Sdt.Length != 10 || !model.Sdt.All(char.IsDigit))
            {
                ModelState.AddModelError(nameof(model.Sdt), "Số điện thoại phải là 10 chữ số.");
                return View("Register", model);
            }
        
            // Tạo ID bằng cách tìm ID lớn nhất hiện tại và tăng lên
            var maxId = _context.NguoiDungs
                .OrderByDescending(u => u.IdNguoiDung)
                .Select(u => u.IdNguoiDung)
                .FirstOrDefault();

            string newId;
            if (string.IsNullOrEmpty(maxId))
            {
                newId = "0000000001"; // Nếu chưa có ID nào, bắt đầu từ 0000000001
            }
            else
            {
                newId = (long.Parse(maxId) + 1).ToString("D10"); // Tăng ID lên 1 và định dạng thành 10 chữ số
            }
        
            var nguoiDung = new NguoiDung
            {
                IdNguoiDung = newId,
                HoVaTen = model.HoVaTen,
                Email = model.Email,
                Sdt = model.Sdt,
                Cccd = "000000000000",
                DiaChi = "Chưa cập nhật",
                VaiTro = 0,
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký: " + ex.Message);
                return View(model);
            }
        }
    }
}
