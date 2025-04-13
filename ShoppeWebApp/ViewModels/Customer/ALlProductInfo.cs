using Microsoft.EntityFrameworkCore;
using ShoppeWebApp.Data;
using ShoppeWebApp.Models;

namespace ShoppeWebApp.ViewModels.Customer
{
    public class AllProductInfo
    {
        public List<ProductInfo> productInfos;
        public List<DanhMuc> categories;
        public AllProductInfo(ShoppeWebAppContext context)
        {
            productInfos = new List<ProductInfo>();
            categories = context.DanhMucs.ToList();
            var products = context.SanPhams.ToList();
            foreach(var i in products)
            {
                productInfos.Add(new ProductInfo
                {
                    IdSanPham = i.IdSanPham,
                    TenSanPham = i.TenSanPham,
                    UrlAnh = i.UrlAnh,
                    GiaBan = i.GiaBan,
                    DiemDanhGia = i.TongDiemDg / i.SoLuotDg,
                    SoLuongBan = i.SoLuongBan,
                });
            }

        }
    }
    public class ProductInfo
    {
        public string IdSanPham { get; set; } = null!;
        public string TenSanPham { get; set; } = null!;
        public string UrlAnh { get; set; } = null!;
        public decimal GiaBan { get; set; }
        public decimal DiemDanhGia { get; set; }
        public int SoLuongBan { get; set; }
    }
}
