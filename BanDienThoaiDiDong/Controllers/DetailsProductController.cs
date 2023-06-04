using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class DetailsProductController : Controller
    {
        // GET: DetailsProduct
        DB_DiDongEntities db = new DB_DiDongEntities();
        public ActionResult Index()
        {
            var kh = db.KHACHHANGs.Where(s => s.MaKH == 2).FirstOrDefault();
            Session["KH"] = kh;
            return View(db.SANPHAMs.ToList());
        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            List<Color> listmau = new List<Color>();
            List<Capacity> listcapa = new List<Capacity>();
            var seri = id.Substring(id.IndexOf("d") + 1, id.IndexOf("s") - id.IndexOf("d"));
            var dongmay = db.SANPHAMs.Where(s => s.MaSP.Contains(seri)).ToList();
            List<SANPHAM> temp = new List<SANPHAM>();
            foreach (var s in dongmay)
            {
                var dungluong = s.MaSP.Substring(s.MaSP.IndexOf("m") + 1, s.MaSP.IndexOf("d") - s.MaSP.IndexOf("m"));
                var capa = db.Capacities.Where(a => a.MaCapacity == dungluong).FirstOrDefault();
                if (!listcapa.Contains(capa))
                    listcapa.Add(capa);
            }
            foreach (var s in dongmay)
            {
                var maMau = s.MaSP.Substring(0, s.MaSP.IndexOf("m") + 1);
                var color = db.Colors.Where(a => a.MaColor == maMau).FirstOrDefault();
                if (!listmau.Contains(color))
                    listmau.Add(color);
            }
            ViewBag.mau = listmau;
            ViewBag.capa = listcapa;
            return View("Details", db.SANPHAMs.Where(s => s.MaSP == id).FirstOrDefault());
        }
        [HttpPost]
        public JsonResult GetMaSP(string masp)
        {
            var sanpham = db.SANPHAMs.Where(s => s.MaSP.Contains(masp)).FirstOrDefault();
            string id = sanpham.MaSP;
            //return RedirectToAction("Details", "DetailsProduct", new { id = id });
            return Json(new { id = id });
        }
       
    }
}