using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class HomeController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
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
        public List<SANPHAM> SoLuongSP(int soluong)
        {
            return database.SANPHAMs.Take(soluong).ToList();
        }
        public ActionResult TrangChu(string SearchString)
        {
            var lstSanPham = SoLuongSP(9);

            if (!string.IsNullOrEmpty(SearchString))
            {
                lstSanPham = database.SANPHAMs.Where(n => n.TenSP.Contains(SearchString)).ToList();

            }
            return View(lstSanPham);
        }
    }
}