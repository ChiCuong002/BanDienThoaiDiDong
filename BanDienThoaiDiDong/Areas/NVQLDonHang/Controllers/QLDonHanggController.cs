using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanDienThoaiDiDong.Models;
using System.Net;

namespace BanDienThoaiDiDong.Areas.NVQLDonHang.Controllers
{
    public class QLDonHanggController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: NVQLDonHang/QLDonHangg
        public ActionResult DSDonHang(string Searching)
        {
            var order = database.HDBANs.ToList();
            if (!string.IsNullOrEmpty(Searching))
            {
                order = database.HDBANs.Where(n => n.KHACHHANG.SoDienThoai.Contains(Searching)).ToList();
            }
            else
            {
                order = database.HDBANs.ToList();
            }
            return View(order);
        }

        public ActionResult Details(int id)
        {
            var order = database.HDBANs.Where(c => c.MaHD == id.ToString()).FirstOrDefault();
            var chitiet = database.CHITIETHDBANs.Where(c => c.ID_HDBAN == id.ToString()).ToList();
            //ViewBag.list = chitiet.ToList();
            ViewBag.hoten = order.KHACHHANG.HoTen;
            ViewBag.mahd = order.MaHD;
            ViewBag.dienthoai = order.KHACHHANG.SoDienThoai;
            ViewBag.email = order.KHACHHANG.Email;
            ViewBag.diachi = order.KHACHHANG.DiaChi;
            ViewBag.tien = order.TongGiaTri;
            ViewBag.ngaytao = order.NgayDatHang;
            ViewBag.trangthaidh = order.TrangThaiDH;
            ViewBag.trangthaitt = order.TrangThaiTT;
            return View(chitiet);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var ttdonhang = database.HDBANs.Where(c => c.MaHD == id).FirstOrDefault();
            return View(ttdonhang);
        }

        [HttpPost]
        public ActionResult Edit(int id, HDBAN ttdonhang, int ttdh)
        {
            string[] chon = { "Đang giao hàng", "Đã giao hàng" };
            string trangthai = chon[ttdh];
            ttdonhang.TrangThaiDH = trangthai;
            database.Entry(ttdonhang).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("DSDonHang");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = database.HDBANs.Where(c => c.MaHD == id).FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var order = database.HDBANs.Where(c => c.MaHD == id).FirstOrDefault();
                order.HienThi = false;
                database.Entry(order).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("DSDonHang");
            }
            catch
            {
                return Content("Không xóa được");
            }
        }
    }
}