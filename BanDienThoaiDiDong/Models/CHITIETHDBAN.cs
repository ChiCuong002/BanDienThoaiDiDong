//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanDienThoaiDiDong.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHITIETHDBAN
    {
        public int MaCTHD { get; set; }
        public Nullable<int> ID_SanPham { get; set; }
        public string ID_HDBAN { get; set; }
        public string Mau { get; set; }
        public string DungLuong { get; set; }
        public Nullable<int> SoLuongDatHang { get; set; }
        public Nullable<decimal> DonGia { get; set; }
    
        public virtual HDBAN HDBAN { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }
    }
}
