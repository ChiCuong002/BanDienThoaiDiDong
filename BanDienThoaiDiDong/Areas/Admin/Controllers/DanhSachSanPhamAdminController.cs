using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanDienThoaiDiDong.Models;
 

namespace BanDienThoaiDiDong.Areas.Admin.Controllers
{
    public class DanhSachSanPhamAdminController : Controller
    {
        //ket noi csdl
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: Admin/DanhSachSanPhamAdmin
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
            ViewBag.MaLoaiSP = new SelectList(database.LOAISANPHAMs, "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        public ActionResult Create(SANPHAM objSanPham)
        {
            try
            {
                if(objSanPham.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objSanPham.ImageUpload.FileName);
                    string extension = Path.GetExtension(objSanPham.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objSanPham.Hinh = fileName;
                    objSanPham.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                }
                database.SANPHAMs.Add(objSanPham);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objSanPham = database.SANPHAMs.Where(n => n.MaSP == id).FirstOrDefault(); 
            return View(objSanPham);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            ViewBag.MaLoaiSP = new SelectList(database.LOAISANPHAMs, "MaLoai", "TenLoai");
            var objSanPham = database.SANPHAMs.Where(n => n.MaSP == id).FirstOrDefault();
            return View(objSanPham);
        }
        [HttpPost]
        public ActionResult Delete(SANPHAM objSanPham)
        {
            if(objSanPham.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objSanPham.ImageUpload.FileName);
                string extension = Path.GetExtension(objSanPham.ImageUpload.FileName);
                fileName = fileName + extension;
                objSanPham.Hinh = fileName;
                objSanPham.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
            }
            database.Entry(objSanPham).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.MaLoaiSP = new SelectList(database.LOAISANPHAMs, "MaLoai", "TenLoai");
            var objSanPham = database.SANPHAMs.Where(n => n.MaSP == id).FirstOrDefault();
            return View(objSanPham);
        }
        [HttpPost]
        public ActionResult Edit(SANPHAM objSanPham)
        {
            if (objSanPham.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objSanPham.ImageUpload.FileName);
                string extension = Path.GetExtension(objSanPham.ImageUpload.FileName);
                fileName = fileName + extension;
                objSanPham.Hinh = fileName;
                objSanPham.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
            }
            database.Entry(objSanPham).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}