﻿using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Areas.Admin.Controllers
{
    public class DanhSachSanPhamController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        public ActionResult Index()
        {
            //tao 1 cai bien chua danh sach san pham
            var lstSanPham = database.SANPHAMs.ToList();
            //phai tra ve 1 gia tri 
            return View(lstSanPham);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(database.LOAISANPHAMs, "MaLoai", "TenLoai");
            ViewBag.MaColor = new SelectList(database.Colors.ToList(), "MaColor", "TenMau");
            ViewBag.MaCapacity = new SelectList(database.Capacities.ToList(), "MaCapacity", "DungLuong");
            ViewBag.SeriMay = new SelectList(database.Seris.ToList(), "SeriMay", "TenSeriMay");
            return View();
        }
        [HttpPost]
        public ActionResult Create(SANPHAM objSanPham, HttpPostedFileBase ImageUpload, string MaColor, string MaCapacity, string SeriMay)
        {
            if (ImageUpload == null)
            {
                ViewBag.ThongBao = " Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(ImageUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hinh đã tồn tại";
                    }
                    else
                    {
                        ImageUpload.SaveAs(path);

                    }
                    objSanPham.MaSP = MaSP(MaColor, MaCapacity, SeriMay);
                    objSanPham.Hinh = filename;
                    database.SANPHAMs.Add(objSanPham);
                    database.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
        public string MaSP(string MaColor, string MaCapacity, string SeriMay)
        {
            string masp = MaColor + MaCapacity + SeriMay;
            do
            {
                Random rnd = new Random();
                int num = rnd.Next(100, 2000);
                masp += num;
            } while (database.SANPHAMs.Where(s => s.MaSP == masp).FirstOrDefault() != null);
            return masp;
        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            
            var objSanPham = database.SANPHAMs.Where(n => n.MaSP == id).FirstOrDefault();
            
            return View(objSanPham);
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var Maloai = database.SANPHAMs.Where(c => c.MaSP.ToString() == id).FirstOrDefault();
            if (Maloai == null)
            {
                return HttpNotFound();
            }
            return View(Maloai);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {

                var Maloai = database.SANPHAMs.Where(c => c.MaSP.ToString() == id).FirstOrDefault();
                database.SANPHAMs.Remove(Maloai);
                database.SaveChanges();
                return RedirectToAction("Index", "DanhSachSanPham", new { area = "Admin" });
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var mau = id.Substring(0, id.IndexOf("m") + 1);
            var dungluong = id.Substring(id.IndexOf("m") + 1, id.IndexOf("d") - id.IndexOf("m"));
            var seri = id.Substring(id.IndexOf("d") + 1, id.IndexOf("s") - id.IndexOf("d"));
            var sp = database.SANPHAMs.Where(s => s.MaSP == id).FirstOrDefault();
            ViewBag.MaLoai = new SelectList(database.LOAISANPHAMs, "MaLoai", "TenLoai", sp.MaLoai);
            ViewBag.MaColor = new SelectList(database.Colors.ToList(), "MaColor", "TenMau", mau);
            ViewBag.MaCapacity = new SelectList(database.Capacities.ToList(), "MaCapacity", "DungLuong", dungluong);
            ViewBag.SeriMay = new SelectList(database.Seris.ToList(), "SeriMay", "TenSeriMay", seri);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Edit(SANPHAM sanpham, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                var productDB = database.SANPHAMs.FirstOrDefault(p => p.MaSP == sanpham.MaSP);
                if (productDB != null)
                {
                    productDB.TenSP = sanpham.TenSP;
                    productDB.MoTa = sanpham.MoTa;
                    productDB.Gia = sanpham.Gia;
                    if (ImageUpload != null)
                    {
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/image"), fileName);
                        productDB.Hinh = fileName;
                        ImageUpload.SaveAs(path);
                    }
                    productDB.MaLoai = sanpham.MaLoai;
                }
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LOAISANPHAM = new SelectList(database.LOAISANPHAMs, "MaLoai", "Tenloai", sanpham.LOAISANPHAM);
            return View(sanpham);
           
        }
        
    }
}