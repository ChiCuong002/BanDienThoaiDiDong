using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Areas.Admin.Controllers
{
    public class QLUserController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: Admin/QuanLyUser
        public ActionResult Index()
        {
            var lstNguoiDung = database.KHACHHANGs.ToList();
            return View(lstNguoiDung);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objNguoiDung = database.KHACHHANGs.Where(n => n.MaKH == id).FirstOrDefault();
            return View(objNguoiDung);
        }
        [HttpPost]
        public ActionResult Delete(KHACHHANG objKhachHang)
        {
            database.Entry(objKhachHang).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult DangKy(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                var check = database.AdminUsers.FirstOrDefault(s => s.Username == _user.Username);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.AdminUsers.Add(_user);
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
        public ActionResult DangNhap(string username, string password)
        {
            if (ModelState.IsValid)
            {

                var f_password = GetMD5(password);
                var data = database.AdminUsers.Where(s => s.Username.Equals(username) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session

                    Session["Email"] = data.FirstOrDefault().Username;
                    Session["idUser"] = data.FirstOrDefault().ID;
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
        public ActionResult Profile(string username)
        {
            var objAdmin = database.AdminUsers.Where(n => n.Username == username).FirstOrDefault();
            return View(objAdmin);
        }

    }
}