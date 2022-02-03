using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class Phim
    {
        public Phim()
        {
            Banners = new HashSet<Banner>();
            LichChieus = new HashSet<LichChieu>();
        }

        public int MaPhim { get; set; }
        public string TenPhim { get; set; } = null!;
        public string BiDanh { get; set; } = null!;
        public string Trailer { get; set; } = null!;
        public string HinhAnh { get; set; } = null!;
        public string MoTa { get; set; } = null!;
        public string MaNhom { get; set; } = null!;
        public string NgayKhoiChieu { get; set; } = null!;
        public long DanhGia { get; set; }
        public string Hot { get; set; } = null!;
        public string DangChieu { get; set; } = null!;
        public string SapChieu { get; set; } = null!;

        public virtual ICollection<Banner>? Banners { get; set; }
        public virtual ICollection<LichChieu>? LichChieus { get; set; }
    }
}
