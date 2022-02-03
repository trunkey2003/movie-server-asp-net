using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrunkeyWebApp.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            Ghes = new HashSet<Ghe>();
        }

        public string TaiKhoan { get; set; } = null!;
        public string HoTen { get; set; } = "";
        public string Email { get; set; } = "";
        public string SoDt { get; set; } = "";
        public string MatKhau { get; set; } = null!;
        public string MaLoaiNguoiDung { get; set; } = null!;

        public virtual LoaiNguoiDung? MaLoaiNguoiDungNavigation { get; set; } = null!;
        public virtual ICollection<Ghe>? Ghes { get; set; } = new List<Ghe>();
    }
}
