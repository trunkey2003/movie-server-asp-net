using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class LoaiNguoiDung
    {
        public LoaiNguoiDung()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        public string MaLoaiNguoiDung { get; set; } = null!;
        public string TenLoai { get; set; } = null!;

        public virtual ICollection<NguoiDung>? NguoiDungs { get; set; }
    }
}
