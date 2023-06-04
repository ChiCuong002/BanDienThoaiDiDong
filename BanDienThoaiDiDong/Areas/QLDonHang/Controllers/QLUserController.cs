using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BanDienThoaiDiDong.Areas.QLDonHang.Controllers
{
    public class QLUserController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: Admin/QuanLyUse
        public ActionResult Index(string searchString, int? page)
        {
            var lstNguoiDung = database.KHACHHANGs.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                lstNguoiDung = database.KHACHHANGs.Where(n => n.HoTen.Contains(searchString)).ToList();

            }
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(lstNguoiDung.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objNguoiDung = database.KHACHHANGs.Where(n => n.MaKH == id).FirstOrDefault();
            return View(objNguoiDung);
        }
        [HttpPost]
        public ActionResult DeleteKH(int MaKH)
        {
            var kh = database.KHACHHANGs.Where(s => s.MaKH == MaKH).FirstOrDefault();
            //database.Entry(objKhachHang).State = System.Data.Entity.EntityState.Modified;
            kh.HienThi = false;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NHANVIEN _user)
        {
            if (ModelState.IsValid)
            {
                var check = database.NHANVIENs.FirstOrDefault(s => s.UserName == _user.UserName);
                if (check == null)
                {
                    _user.MatKhau = GetMD5(_user.MatKhau);
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.NHANVIENs.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Username already exists";
                    return View();
                }
            }
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string bye25string = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                bye25string = targetData[i].ToString("x2");
            }
            return bye25string;
        }
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string username, string matkhau)
        {
            if (ModelState.IsValid)
            {

                var f_password = GetMD5(matkhau);
                var data = database.NHANVIENs.Where(s => s.UserName.Equals(username) && s.MatKhau.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session

                    Session["Email"] = data.FirstOrDefault().UserName;
                    Session["Hoten"] = data.FirstOrDefault().HoTen;
                    Session["emaill"] = data.FirstOrDefault().Email;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("DangNhap");
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Profile()
        {
            NHANVIEN user = Session["User"] as NHANVIEN;
            var objAdmin = database.NHANVIENs.Where(n => n.MaNV == user.MaNV).FirstOrDefault();
            return View(objAdmin);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objUser = database.NHANVIENs.Where(c => c.MaNV == id).FirstOrDefault();
            return View(objUser);
        }
        [HttpPost]
        public ActionResult Edit(int id, string HoTen, string Email, string DiaChi, string SoDienThoai )
        {
            var nHANVIEN = database.NHANVIENs.Where(s => s.MaNV == id).FirstOrDefault();
            //database.Entry(nHANVIEN).State = System.Data.Entity.EntityState.Modified;
            nHANVIEN.HoTen = HoTen;
            nHANVIEN.Email = Email;
            nHANVIEN.DiaChi = DiaChi;
            nHANVIEN.SoDienThoai = SoDienThoai;
            database.SaveChanges();
            return RedirectToAction("Profile");
        }
    }
}
