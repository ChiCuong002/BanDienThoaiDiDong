using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanDienThoaiDiDong.Models;

namespace BanDienThoaiDiDong.Controllers
{
    public class HomeController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        public List<SANPHAM> SoLuongSP(int soluong)
        {
            return database.SANPHAMs.Take(soluong).ToList();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangChu(string SearchString)
        {
            var lstSanPham = SoLuongSP(9);

            if (!string.IsNullOrEmpty(SearchString))
            {
                lstSanPham = database.SANPHAMs.Where(n => n.TenSP.Contains(SearchString)).ToList();

            }
             
            ViewBag.dsHang = database.LOAISANPHAMs.ToList();
            return View(lstSanPham);
        }
    }
}