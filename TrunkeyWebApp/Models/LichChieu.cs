using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class LichChieu
    {
        public LichChieu()
        {
            Ghes = new HashSet<Ghe>();
        }

        public int MaLichChieu { get; set; }
        public int MaRap { get; set; }
        public int MaPhim { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public int GiaVe { get; set; }

        public virtual Phim? MaPhimNavigation { get; set; } = null!;
        public virtual Rap? MaRapNavigation { get; set; } = null!;
        public virtual ICollection<Ghe>? Ghes { get; set; }
    }
}
