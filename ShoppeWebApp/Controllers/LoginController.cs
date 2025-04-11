using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppeWebApp.Models;
using ShoppeWebApp.Models.Login;
using ShoppeWebApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppeWebApp.Models.Database;

namespace ShoppeWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShoppeWebAppContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ShoppeWebAppContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Customer/UserLogin.cshtml");
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = _context.TaiKhoans
                    .FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);

                if (taiKhoan != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nhập sai mật khẩu hoặc tài khoản.");
                }
            }

            return View("~/Views/Customer/UserLogin.cshtml", model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("~/Views/Customer/ForgotPassword.cshtml");
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var taiKhoan = _context.TaiKhoans
                        .Include(a => a.IdNguoiDungNavigation)
                        .FirstOrDefault(a => a.Username == model.Username && a.IdNguoiDungNavigation.Sdt == model.PhoneNumber);

                    if (taiKhoan != null)
                    {
                        return RedirectToAction("ResetPassword", new { username = model.Username });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc số điện thoại không đúng.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Đã xảy ra lỗi khi tìm kiếm tài khoản.");
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đặt lại mật khẩu.");
                }
            }

            return View("~/Views/Customer/ForgotPassword.cshtml", model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string username)
        {
            var model = new ResetPasswordViewModel { Username = username };
            return View("~/Views/Customer/ResetPassword.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword == model.ConfirmPassword)
                {
                    var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(a => a.Username == model.Username);
                    if (taiKhoan != null)
                    {
                        _logger.LogInformation("Tài khoản tìm thấy: {Username}", model.Username);
                        taiKhoan.Password = model.NewPassword;

                        try
                        {
                            _context.TaiKhoans.Update(taiKhoan);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Cập nhật mật khẩu thành công cho: {Username}", model.Username);
                            ViewBag.Message = "Mật khẩu đã được đặt lại thành công.";
                            return RedirectToAction("Index", "Login");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Lỗi khi lưu mật khẩu mới cho: {Username}", model.Username);
                            ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi lưu mật khẩu mới.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Không tìm thấy tài khoản: {Username}", model.Username);
                        ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu và xác nhận mật khẩu không khớp.");
                }
            }

            return View("~/Views/Customer/ResetPassword.cshtml", model);
        }
    }
}
