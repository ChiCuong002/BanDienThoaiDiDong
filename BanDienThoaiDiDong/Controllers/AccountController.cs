using BanDienThoaiDiDong.DAO;
using BanDienThoaiDiDong.Models;
using DocumentFormat.OpenXml.EMMA;
using Facebook;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;


namespace BanDienThoaiDiDong.Controllers
{
    public class AccountController : Controller
    {
        DB_DiDongEntities db = new DB_DiDongEntities();

        //Register

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
                if (string.IsNullOrEmpty(kh.HoTen))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.DiaChi))
                    ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống");
                if (string.IsNullOrEmpty(kh.SoDienThoai))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống");
                if (string.IsNullOrEmpty(kh.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                // kiểm tra email đã tồn tại chưa
                var check = db.KHACHHANGs.FirstOrDefault(s => s.Email == kh.Email);
                if (check != null)
                {
                    ModelState.AddModelError(string.Empty, "Email đã tồn tại!");
                }

                if (kh.MatKhau != kh.confirmMatKhau)
                {
                    ModelState.AddModelError("", "Mật khẩu không khớp!");
                }

                if (ModelState.IsValid)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.KHACHHANGs.Add(kh);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return View(kh);
                }
            }
            return View(kh);
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authen(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    var check = db.KHACHHANGs.FirstOrDefault(s => s.Email.Equals(kh.Email) && s.MatKhau.Equals(kh.MatKhau));
                    if (check == null)
                    {
                        ModelState.AddModelError("", "Email hoặc mật khẩu không hợp lệ!");  
                        return View("Login", kh);
                    }
                    else
                    {
                        //Session["KH"] = check;
                        Session["kh"] = check;
                        return RedirectToAction("Index", "Home");
                        //KHACHHANG khach = Session["KH"] as KHACHHANG;
                    }
                }
            }
            return View(kh);
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        //Change password
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(changePassword.OldPassword))
                    ModelState.AddModelError("OldPassword", "Vui lòng điền mật khẩu cũ");
                if (string.IsNullOrEmpty(changePassword.MatKhauMoi))
                    ModelState.AddModelError("MatKhauMoi", "Vui lòng điền mật khẩu mới");
                if (string.IsNullOrEmpty(changePassword.ConfirmMatKhauMoi))
                    ModelState.AddModelError("ConfirmMatKhauMoi", "Vui lòng xác nhận mật khẩu mới");
                if (changePassword.MatKhauMoi != changePassword.ConfirmMatKhauMoi)
                    ModelState.AddModelError("ConfirmMatKhauMoi", "Mật khẩu không khớp");
                if (ModelState.IsValid)
                {
                    var email = ((KHACHHANG)Session["kh"])?.Email;
                    if (email != null)
                    {
                        var customer = db.KHACHHANGs.FirstOrDefault(c => c.Email == email);
                        if (customer != null && customer.MatKhau == changePassword.OldPassword)
                        {
                            customer.MatKhau = changePassword.MatKhauMoi;
                            db.Entry(customer).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            ModelState.AddModelError("OldPassword", "Mật khẩu cũ không hợp lệ!");
                            return View(changePassword);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Session email bị null hoặc rỗng");
                        return View(changePassword);
                    }
                }
            }
            return View(changePassword);
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
                dynamic me = fb.Get("me?fields=id, name, email");
                string name = me.name;
                string email = me.email;
                FormsAuthentication.SetAuthCookie(email, false);
                //gán vào đối tượng khách hàng
                var user = new KHACHHANG();
                user.Email= email;
                user.HoTen = name;
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