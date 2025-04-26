using System.Collections.Generic;

namespace ShoppeWebApp.ViewModels.Admin
{
    public class ProductViewModel
    {
        // Danh sách sản phẩm
        public List<ProductInfo> productInfos { get; set; } = new List<ProductInfo>();

        // Danh sách danh mục
        public List<CategoryInfo> categories { get; set; } = new List<CategoryInfo>();

        // ID danh mục hiện tại 
        public string? danhMuc { get; set; }

        // Thông tin cửa hàng
        public string? TenCuaHang { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? UrlAnhCuaHang { get; set; }
    }

    // Thông tin sản phẩm
    public class ProductInfo
    {
        public string? IdSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public decimal GiaGoc { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongBan { get; set; }
        public string? UrlAnh { get; set; }
        public decimal TyLeGiamGia { get; set; }
    }

    // Thông tin danh mục
    public class CategoryInfo
    {
        public string? IdDanhMuc { get; set; }
        public string? TenDanhMuc { get; set; }
    }
}