using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class QLThongTinController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        public ActionResult ThongTin()
        {
            var user = Session["kh"] as KHACHHANG;
            var objUser = database.KHACHHANGs.Where(c => c.MaKH == user.MaKH).FirstOrDefault();
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            var objUser = database.KHACHHANGs.Where(c => c.MaKH == id).FirstOrDefault();
            return View(objUser);
        }
        [HttpPost]
        public ActionResult Edit( KHACHHANG kh)
        {
            database.Entry(kh).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("TrangChu","Home");
        }
    }
}