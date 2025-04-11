using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ShoppeWebApp.Data; 

namespace ShoppeWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<ShoppeWebAppContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppeWebApp")));
            builder.Services.AddAuthentication("CustomerSchema")
            .AddCookie("CustomerSchema", options =>
            {
                options.LoginPath = "/Customer/Account/Login";
                options.AccessDeniedPath = "/Authentication/AccessDenied";
                options.Cookie.Name = "CustomerCookie";
            })
            .AddCookie("SellerSchema", options =>
             {
                 options.LoginPath = "/Seller/Account/Login";
                 options.AccessDeniedPath = "/Authentication/AccessDenied";
                 options.Cookie.Name = "SellerCookie";
             });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("Customer", policy => policy.RequireClaim(ClaimTypes.Role, "Customer"));
                options.AddPolicy("Seller", policy => policy.RequireClaim(ClaimTypes.Role, "Seller"));
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
