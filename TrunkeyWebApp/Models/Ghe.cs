using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class Ghe
    {
        public int MaGhe { get; set; }
        public int MaLichChieu { get; set; }
        public int SttGhe { get; set; }
        public string LoaiGhe { get; set; } = null!;
        public string DaDat { get; set; } = null!;
        public string? TaiKhoanNguoiDat { get; set; }

        
        public virtual LichChieu MaLichChieuNavigation { get; set; } = null!;
        public virtual NguoiDung? TaiKhoanNguoiDatNavigation { get; set; }
    }
}
