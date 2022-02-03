using System;
using System.Collections.Generic;

namespace TrunkeyWebApp.Models
{
    public partial class HeThongRap
    {
        public HeThongRap()
        {
            CumRaps = new HashSet<CumRap>();
        }

        public string MaHeThongRap { get; set; } = null!;
        public string TenHeThongRap { get; set; } = null!;
        public string BiDanh { get; set; } = null!;
        public string Logo { get; set; } = null!;

        public virtual ICollection<CumRap>? CumRaps { get; set; }
    }
}
