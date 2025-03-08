using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class obj_CT_HOADON
    {
        public string MA_CT_HD { get; set; } // Chuyển từ Guid sang string
        public string MAHD { get; set; }
        public string MAHH { get; set; }
        public string TENHH { get; set; }
        public Nullable<int> SOLUONGCT { get; set; }
        public Nullable<double> GIABAN { get; set; }
        public Nullable<double> THANHTIEN { get; set; }
        public string DVT { get; set; }
    }
}
