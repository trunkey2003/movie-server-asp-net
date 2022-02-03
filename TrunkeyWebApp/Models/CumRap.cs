using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class CumRap
    {
        public CumRap()
        {
            Raps = new HashSet<Rap>();
        }

        public string MaCumRap { get; set; } = null!;
        public string TenCumRap { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string MaHeThongRap { get; set; } = null!;

        public virtual HeThongRap? MaHeThongRapNavigation { get; set; } = null!;
        public virtual ICollection<Rap>? Raps { get; set; }
    }
}
