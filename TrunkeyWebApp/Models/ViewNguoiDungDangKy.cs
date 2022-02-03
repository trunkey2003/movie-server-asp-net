using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrunkeyWebApp.Models
{
    public partial class ViewNguoiDungDangKy
    {
        public ViewNguoiDungDangKy()
        {
            Ghes = new HashSet<Ghe>();
        }

        public string TaiKhoan { get; set; } = null!;
        public string HoTen { get; set; } = "";
        public string Email { get; set; } = "";
        public string SoDt { get; set; } = "";
        public string MatKhau { get; set; } = null!;
        [Compare("MatKhau", ErrorMessage = "Mật Khẩu Nhập Lại Không Khớp Vui Lòng Nhập Lại !")]
        public string NhapLaiMatKhau { get; set; } = null!;
        public string MaLoaiNguoiDung { get; set; } = "user";

        public virtual LoaiNguoiDung MaLoaiNguoiDungNavigation { get; set; } = new()
        {
            MaLoaiNguoiDung="user",
            TenLoai="Khách Hàng"
        };

        public virtual ICollection<Ghe>? Ghes { get; set; }
    }
}
