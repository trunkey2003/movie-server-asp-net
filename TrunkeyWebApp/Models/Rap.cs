using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class Rap
    {
        public Rap()
        {
            LichChieus = new HashSet<LichChieu>();
        }

        public int MaRap { get; set; }
        public string TenRap { get; set; } = null!;
        public string MaCumRap { get; set; } = null!;

        public virtual CumRap? MaCumRapNavigation { get; set; } = null!;
        public virtual ICollection<LichChieu>? LichChieus { get; set; }
    }
}
