using BanDienThoaiDiDong.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Collections.Specialized.BitVector32;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class PayPal_PaymentController : Controller
    {
        DB_DiDongEntities db = new DB_DiDongEntities();
        private decimal TyGiaUsd = 23890;
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                    "/PayPal_Payment/PaymentConfirm?";
                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                //return View("FailureView");
            }
            //on successful payment, show success page to user.
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            //Adding Item Details like name, currency, price etc
            Cart cart = Session["Cart"] as Cart;
            foreach (var item in cart.Items)
            {
                itemList.items.Add(new Item()
                {
                    name = item.product.TenSP,
                    currency = "USD",
                    price = Math.Round(item.Gia / TyGiaUsd, 2, MidpointRounding.ToEven).ToString(),
                    quantity = item.quantity.ToString(),
                    sku = "sku"
                });
            }
            var payer = new Payer() { payment_method = "paypal" };
            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details

            var tax = 1;
            var shipping = 1;
            var sum = Math.Round(cart.Total_money() / TyGiaUsd, 2, MidpointRounding.ToEven) + tax + shipping;
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = Math.Round(cart.Total_money() / TyGiaUsd, 2).ToString(),
            };
            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = sum.ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No
                amount = amount,
                item_list = itemList
            });
            //string thanhcongURL = Request.Url.Scheme + "://" + Request.Url.Authority +
            //        "/PayPal_Payment/ThanhCong?";
            //string thatbaiURL = Request.Url.Scheme + "://" + Request.Url.Authority +
            //        "/PayPal_Payment/ThatBai?";
            //var redirUrls = new RedirectUrls()
            //{
            //    cancel_url = thatbaiURL + "&Cancel=true",
            //    return_url = thanhcongURL
            //};
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
        public void PaymentConfirm()
        {
            string url = Request.Url.AbsoluteUri;
            string payment_ID = Request.QueryString["paymentId"];
            if (url.Contains("&Cancel=true"))
            {
                ThatBai();
            }
            else
            {
                ThanhCong();
            }
        }
        public ActionResult ThatBai()
        {
            TempData["message"] = "Có lỗi xảy ra trong quá trình xử lý hóa đơn ";
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult ThanhCong()
        {
            HDBAN hd = new HDBAN();
            var kh = Session["KH"] as KHACHHANG;
            Cart cart = Session["Cart"] as Cart;
            hd.MaHD = Guid.NewGuid().ToString();
            hd.NgayDatHang = DateTime.Now;
            hd.MaKH = kh.MaKH;
            hd.DiaChiGiaoHang = kh.DiaChi;
            hd.TongGiaTri = cart.Total_money();
            hd.TrangThaiTT = "Đã thanh toán";
            hd.TrangThaiDH = "Chờ xác nhận";
            hd.New = true;
            hd.HienThi = true;
            db.HDBANs.Add(hd);
            foreach (var item in cart.Items)
            {
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
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
    }
}