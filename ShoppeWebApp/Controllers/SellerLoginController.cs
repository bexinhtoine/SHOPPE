using Microsoft.AspNetCore.Mvc;
using ShoppeWebApp.Models.Login;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShoppeWebApp.Data;

namespace ShoppeWebApp.Controllers
{
    public class SellerLoginController : Controller
    {
        private readonly ShoppeWebAppContext _context;
        private readonly ILogger<SellerLoginController> _logger;

        public SellerLoginController(ShoppeWebAppContext context, ILogger<SellerLoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //return View("SellerLogin");
            return View("~/Views/Seller/SellerLogin.cshtml");
        }

        [HttpPost]
        public IActionResult Index(SellerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check the username and password against the database
                var account = _context.TaiKhoans
                    .FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);

                if (account != null)
                {
                    // Authentication successful
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Authentication failed
                    ModelState.AddModelError(string.Empty, "Nhập sai mật khẩu hoặc tài khoản.");
                }
            }

            // return View("SellerLogin", model);
            return View("~/Views/Seller/SellerLogin.cshtml", model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            //return View("SellerForgotPassword");
            return View("~/Views/Seller/SellerForgotPassword.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _context.TaiKhoans
                        .FirstOrDefault(a => a.Username == model.Username && a.IdNguoiDungNavigation.Sdt == model.PhoneNumber);

                    if (account != null)
                    {
                        //account.Password = "12345678"; // Reset password logic
                        //await _context.SaveChangesAsync();
                        //ViewBag.Message = "Mật khẩu đã được đặt lại thành công.";
                        return RedirectToAction("ResetPassword", new { username = model.Username });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc số điện thoại không đúng.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while resetting the password.");
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đặt lại mật khẩu.");
                }
            }

            return View("~/Views/Seller/SellerForgotPassword.cshtml", model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string username)
        {
            var model = new ResetPasswordViewModel { Username = username };
            return View("~/Views/Seller/SellerResetPassword.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the new password and confirm password fields match
                if (model.NewPassword == model.ConfirmPassword)
                {
                    var account = await _context.TaiKhoans.FirstOrDefaultAsync(a => a.Username == model.Username);
                    if (account != null)
                    {
                        account.Password = model.NewPassword;
                        try
                        {
                            await _context.SaveChangesAsync();
                            ViewBag.Message = "Mật khẩu đã được đặt lại thành công.";
                            return RedirectToAction("Index", "SellerLogin");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred while saving the new password.");
                            ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi lưu mật khẩu mới.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu và xác nhận mật khẩu không khớp.");
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("ModelState error in {Key}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
            }

            return View("~/Views/Seller/SellerResetPassword.cshtml", model);
        }




    }
}



