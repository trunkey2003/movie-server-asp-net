using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrunkeyWebApp.Models
{
    public partial class ViewNguoiDungDangNhap
    {
        public ViewNguoiDungDangNhap()
        {
            Ghes = new HashSet<Ghe>();
        }

        public string TaiKhoan { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SoDt { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public string MaLoaiNguoiDung { get; set; } = null!;

        public bool RememberMe { get; set; } = false;

        public virtual LoaiNguoiDung? MaLoaiNguoiDungNavigation { get; set; } = null!;
        public virtual ICollection<Ghe>? Ghes { get; set; }
    }
}
