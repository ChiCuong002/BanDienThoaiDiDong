using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong.Models
{
    public class DanhSachSPvaTH
    {
        public List<SANPHAM> lstSanPham { get; set; }
        public List<LOAISANPHAM> lstHang { get; set; }
    }
}