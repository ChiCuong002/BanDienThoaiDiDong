using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BanDienThoaiDiDong.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: Admin/AdminLogin
        DB_DiDongEntities db = new DB_DiDongEntities();
        public ActionResult Index()
        {
            ViewBag.user = User.Identity.Name;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoLogin(string userName, string passWord)
        {
            var nv = db.NHANVIENs.Where(s => s.UserName == userName && s.MatKhau == passWord).FirstOrDefault();
            if (nv != null)
            {
                Session["NV"] = nv.HoTen;
                Session["User"] = nv;
                if (nv.ROLE.MaRole == 3)
                {
                    FormsAuthentication.SetAuthCookie(nv.HoTen, true);
                    return RedirectToAction("DSDonHang", "QLDonHang", new { area = "QLDonHang" });

                }
                else
                {
                    FormsAuthentication.SetAuthCookie(nv.HoTen, true);
                    return RedirectToAction("Index", "DanhSachSanPham", new { area = "Admin" });

                }
            }
            else
            {
                ModelState.AddModelError("invalid", "Tài khoản hoặc mật khẩu không chính xác.");
                return View("Login");
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}