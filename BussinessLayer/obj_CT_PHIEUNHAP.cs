using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class obj_CT_PHIEUNHAP
    {
        public string MA_CT_PN { get; set; } // Chuyển từ Guid sang string
        public string MAPN { get; set; }
        public string MAHH { get; set; }
        public string TENHH { get; set; }
        public Nullable<int> SOLUONGCT { get; set; }
        public Nullable<double> GIANHAP { get; set; }
        public Nullable<double> THANHTIEN { get; set; }
        public string DVT { get; set; }
    }
}
