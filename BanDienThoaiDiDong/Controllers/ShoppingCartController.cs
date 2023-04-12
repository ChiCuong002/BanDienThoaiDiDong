using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            var kh = db.KHACHHANGs.Where(s => s.MaKH == 1).FirstOrDefault();
            Session["KH"] = kh;
            return View(db.SANPHAMs.ToList());
        }
        public ActionResult ShowCart()
        {
            string msg = "";
            if (TempData["message"] != null)
            {
                msg = TempData["message"].ToString();
                ViewBag.Message = msg;
            }
            if (Session["Cart"] == null)
            {
                ViewBag.Message = "Giỏ hàng của bạn hiện đang trống.";
                return View();
            }
            Cart cart = Session["Cart"] as Cart;
            ViewBag.TongTien = cart.Total_money();
            return View(cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int MaSP, int color, int dungluong, string gia)
        {
            var pro = db.SANPHAMs.SingleOrDefault(s => s.MaSP == MaSP);
            var mau = db.Colors.SingleOrDefault(s => s.ColorID == color);
            var capacity = db.Capacities.SingleOrDefault(s => s.CapacityID == dungluong);
            decimal donGia = decimal.Parse(gia, CultureInfo.InvariantCulture);
            if (pro != null && mau != null && capacity != null)
            {
                GetCart().Add_Product_Cart(pro, capacity, mau, donGia, 1);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        [HttpPost]
        public JsonResult Update_Cart_Quantity(int idPro, int quant)//FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int productID = idPro;//int.Parse(form["idPro"]);
            int quantity = quant;//int.Parse(form["CartQuantity"]);
            cart.Update_quantity(productID, quantity);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public JsonResult Update_Gia(int id, int slted)
        {
            var gia = db.ChiTietSPs.Where(s => s.MaSP == id && s.MaCapacity == slted).FirstOrDefault();
            decimal result = 0;
            if (gia != null)
                result = (decimal)gia.Gia;
            return Json(new { success = true, message = "" + result.ToString("N0") }, JsonRequestBehavior.AllowGet);
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
            Cart cart = Session["Cart"] as Cart;
            foreach (var item in cart.Items)
            {
                var sp = db.ChiTietSPs.Where(s => s.MaSP == item.product.MaSP && s.MaCapacity == item.capacity.CapacityID && s.MaColor == item.color.ColorID).FirstOrDefault();
                int soluongsp = (int)sp.SoLuong;
                if (item.quantity > soluongsp)
                {
                    TempData["message"] = "Số lượng sản phẩm " + item.product.TenSP + " vượt quá số lượng trong kho.";
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
            Cart cart = Session["Cart"] as Cart;
            hd.MaHD = GenerateID(10);
            hd.NgayDatHang = DateTime.Now;
            hd.MaKH = kh.MaKH;
            hd.DiaChiGiaoHang = kh.DiaChi;
            hd.TongGiaTri = cart.Total_money();
            hd.TrangThaiTT = "Chờ thanh toán";
            hd.TrangThaiDH = "Chờ xác nhận";
            hd.New = true;
            hd.HienThi = true;
            db.HDBANs.Add(hd);
            foreach (var item in cart.Items)
            {
                var sp = db.ChiTietSPs.Where(s => s.MaSP == item.product.MaSP && s.MaCapacity == item.capacity.CapacityID && s.MaColor == item.color.ColorID).FirstOrDefault();
                sp.SoLuong -= item.quantity;
                CHITIETHDBAN cthd = new CHITIETHDBAN();
                cthd.ID_HDBAN = hd.MaHD;
                cthd.ID_SanPham = item.product.MaSP;
                cthd.Mau = item.color.TenColor;
                cthd.DungLuong = item.capacity.DungLuong;
                cthd.SoLuongDatHang = item.quantity;
                cthd.DonGia = item.Gia;
                db.CHITIETHDBANs.Add(cthd);
            }
            db.SaveChanges();
            cart.ClearCart();
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