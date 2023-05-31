using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        DB_DiDongEntities db = new DB_DiDongEntities();
        public ActionResult ShowCart()
        {
            string msg = "";
            var cart = GetCart();
            if (TempData["message"] != null)
            {
                msg = TempData["message"].ToString();
                ViewBag.Message = msg;
            }
            if (cart == null)
            {
                ViewBag.Message = "Giỏ hàng của bạn hiện đang trống.";
                return View();
            }
            return View(cart);
        }
        public UserCart GetCart()
        {
            int giohang = GetCartID();
            List<ChiTietGH> chitietGH = db.ChiTietGHs.Where(s => s.MaGH == giohang).ToList();
            UserCart cart = new UserCart(chitietGH);
            return cart;
        }
        public int GetCartID()
        {
            KHACHHANG kh = (KHACHHANG)Session["kh"];
            var giohang = db.GIOHANGs.Where(s => s.MaKH == kh.MaKH).FirstOrDefault();
            if (giohang == null)
            {
                giohang = new GIOHANG();
                giohang.MaKH = kh.MaKH;
                giohang.TrangThai = true;
                db.GIOHANGs.Add(giohang);
                db.SaveChanges();
            }
            return giohang.id;
        }
        public ActionResult AddToCart(string masp)
        {
            if (Session["kh"] == null)
                return RedirectToAction("Login", "Account");
            else { 
            int giohang_id = GetCartID();
            bool flag = GetCart().Add_To_Cart( masp, giohang_id);
            if(flag == true)
            {
                return RedirectToAction("ShowCart");
            }
            return View();
            }
        }
        [HttpPost]
        public JsonResult Update_Cart_Quantity(int idPro, int quant)
        {
            int giohang = GetCartID();
            var sanpham = db.ChiTietGHs.Where(s => s.id == idPro && s.MaGH == giohang).FirstOrDefault();
            sanpham.SoLuong = quant;
            db.Entry(sanpham).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveCart(int id)
        {
            var sanpham = db.ChiTietGHs.Where(s => s.id == id).FirstOrDefault();
            db.ChiTietGHs.Remove(sanpham);
            db.SaveChanges();
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        [HttpGet]
        public ActionResult DiaChiGiaoHang()
        {
            var kh = db.KHACHHANGs.Where(s => s.MaKH == 1).FirstOrDefault();
            Session["KH"] = kh;
            return View(kh);
        }
        [HttpPost]
        public ActionResult PayMethod(string PaymentMethod)
        {
            UserCart cart = GetCart();
            foreach (var item in cart.Items)
            {
                var sp = db.SANPHAMs.Where(s => s.MaSP == item.MaSP).FirstOrDefault();
                int? soluongsp = item.SoLuong;
                if (sp.SoLuong < soluongsp)
                {
                    TempData["message"] = "Số lượng sản phẩm " + item.SANPHAM.TenSP + " vượt quá số lượng trong kho.";
                    return RedirectToAction("ShowCart");
                }
            }
            if (PaymentMethod.Equals("1"))
            {
                return RedirectToAction("ShipCOD");
            }
            else if (PaymentMethod.Equals("2"))
            {
                return RedirectToAction("Payment", "VNPAY");
            }
            else
            {
                return RedirectToAction("PaymentWithPaypal", "PayPal_Payment", "");
            }

        }
        public ActionResult ShipCOD()
        {

            HDBAN hd = new HDBAN();
            var kh = Session["KH"] as KHACHHANG;
            UserCart cart = GetCart();
            hd.MaHD = GenerateID(10);
            hd.NgayDatHang = DateTime.Now;
            hd.MaKH = kh.MaKH;
            hd.DiaChiGiaoHang = kh.DiaChi;
            hd.TongGiaTri = cart.Total_money();
            hd.TrangThaiTT = "Chờ thanh toán";
            hd.TrangThaiDH = "Chờ xác nhận";
            hd.New = true;
            //hd.HienThi = true;
            db.HDBANs.Add(hd);
            foreach (var item in cart.Items)
            {
                var sp = db.SANPHAMs.Where(s => s.MaSP == item.MaSP).FirstOrDefault();
                sp.SoLuong -= item.SoLuong;
                CHITIETHDBAN cthd = new CHITIETHDBAN();
                cthd.ID_HDBAN = hd.MaHD;
                cthd.ID_SanPham = item.SANPHAM.MaSP;
                cthd.SoLuongDatHang = item.SoLuong;
                cthd.DonGia = item.SANPHAM.Gia;
                db.CHITIETHDBANs.Add(cthd);
            }     
            cart.ClearCart();
            db.SaveChanges();
            TempData["message"] = "Đặt hàng thành công.";
            return RedirectToAction("ShowCart");
        }
        private string GenerateID(int length)
        {
            HDBAN check;
            string mahd;
            do
            {
                const string chars = "0123456789";
                var random = new Random();
                var id = new string(Enumerable.Repeat(chars, length - 1)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                var firstDigit = random.Next(1, 9);
                mahd = $"{firstDigit}{id}";
                check = db.HDBANs.Where(s => s.MaHD == mahd).FirstOrDefault();
            } while (check != null);


            return mahd;
        }

    }
}