//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_HOADON
    {
        public string MAHD { get; set; }
        public Nullable<System.DateTime> NGAYHD { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<double> TONGTIEN { get; set; }
        public string GHICHU { get; set; }
        public string MANV { get; set; }
        public string MAKH { get; set; }
    
        public virtual tb_KHACHHANG tb_KHACHHANG { get; set; }
        public virtual tb_NHANVIEN tb_NHANVIEN { get; set; }
    }
}
