using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Areas.QLDonHang.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: QLDonHang/ThongKe
        DB_DiDongEntities database = new DB_DiDongEntities();
        public ActionResult ThongKeNgay()
        {
            DateTime currentDate = DateTime.Now.Date;
            List<HDBAN> danhSachHDBan = database.HDBANs.Where(hd => hd.NgayDatHang == currentDate).ToList();
            return PartialView("ThongKeNgay", danhSachHDBan);
        }

    }
}