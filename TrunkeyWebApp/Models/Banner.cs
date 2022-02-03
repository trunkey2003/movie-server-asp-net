using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class Banner
    {
        public int MaBanner { get; set; }
        public int MaPhim { get; set; }
        public string HinhAnh { get; set; } = null!;

        public virtual Phim? MaPhimNavigation { get; set; } = null;
    }
}
