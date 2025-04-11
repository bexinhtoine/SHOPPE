using Microsoft.EntityFrameworkCore;
using ShoppeWebApp.Models.Database;

namespace ShoppeWebApp.Models.Login
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> Accounts { get; set; }
    }
}