using BanDienThoaiDiDong.DAO;
using BanDienThoaiDiDong.Models;
using Facebook;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong.Controllers
{
    public class AccountController : Controller
    {
        DB_DiDongEntities db = new DB_DiDongEntities();

        //Regiter

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                var check = db.KHACHHANGs.FirstOrDefault(s => s.Email == kh.Email);
                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.KHACHHANGs.Add(kh);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại!";
                    return View();
                }
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authen(KHACHHANG kh)
        {
            var check = db.KHACHHANGs.Where(s => s.Email.Equals(kh.Email) && s.MatKhau.Equals(kh.MatKhau)).FirstOrDefault();
            if (check == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không hợp lệ!");
                return View("Login", kh);
            }
            else
            {
                //Session["KH"] = check;
                Session["kh"] = check;
                //KHACHHANG khach = Session["KH"] as KHACHHANG;

                return RedirectToAction("TrangChu", "Home");
            }
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        //Login with Facebook
        private Uri RedirectUri 
        {  
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallBack(string code) 
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                //lấy thông tin của người dùng từ Facebook
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields = link, first_name, currency, middle_name, last_name, email, gender, locale, timezone, verified, picture, age_range, phoneNumber");
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                string email = me.email;
                string password = me.password;
                string locale = me.locale;
                string phone = me.phone;
                //gán vào đối tượng khách hàng
                var user = new KHACHHANG();
                user.Email= email;
                user.HoTen = firstName + " " + middleName + " " + lastName;
                user.DiaChi = locale;
                user.SoDienThoai = phone;
                user.MatKhau= password;
                //
                var resultInsert = new UserDAO().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    Session["kh"] = user;
                }
            }
            return Redirect("/");
        }
    }
}