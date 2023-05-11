using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanDienThoaiDiDong.Models;
using System.Net;
using PagedList;

namespace BanDienThoaiDiDong.Areas.NVQLDonHang.Controllers
{
    public class QLDonHanggController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: NVQLDonHang/QLDonHangg
        public ActionResult DSDonHang(string sortOrder, int? page)
        {
            List<HDBAN> list;
            switch (sortOrder)
            {
                case "danger":
                    list = database.HDBANs.Where(s => s.TrangThaiDH.Equals("Chờ xác nhận")).ToList();
                    break;
                case "info":
                    list = database.HDBANs.Where(s => s.TrangThaiDH.Equals("Đang giao hàng")).ToList();
                    break;
                case "success":
                    list = database.HDBANs.Where(s => s.TrangThaiDH.Equals("Đã giao hàng")).ToList();
                    break;
                default:
                    list = database.HDBANs.ToList();
                    break;
            }

            int pageSize = 10;
            int pageNum = (page ?? 1);
            return View(list.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(string id)
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
        public ActionResult Edit(string id, HDBAN ttdonhang, int ttdh)
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
                database.HDBANs.Remove(order);
                //var chitiet = database.CHITIETHDBANs.Where(c => c.ID_HDBAN == id).ToList();

                //foreach (var remove in chitiet)
                //{
                //    database.CHITIETHDBANs.Remove(remove);
                //}
                //database.Entry(order).State = System.Data.Entity.EntityState.Modified;
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