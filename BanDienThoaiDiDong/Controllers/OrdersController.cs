using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        DB_DiDongEntities db = new DB_DiDongEntities();
        public ActionResult Index(string sortOrder)
        {
            KHACHHANG kh = Session["kh"] as KHACHHANG;
            List<HDBAN> list;
            switch (sortOrder)
            {
                case "danger":
                    list = db.HDBANs.Where(s => s.MaKH == kh.MaKH && s.TrangThaiDH.Equals("Chờ xác nhận")).ToList();
                    break;
                case "info":
                    list = db.HDBANs.Where(s => s.MaKH == kh.MaKH && s.TrangThaiDH.Equals("Đang giao hàng")).ToList();
                    break;
                case "success":
                    list = db.HDBANs.Where(s => s.MaKH == kh.MaKH && s.TrangThaiDH.Equals("Đã giao")).ToList();
                    break;
                default:
                    list = db.HDBANs.Where(s => s.MaKH == kh.MaKH).ToList();
                    break;
            }
            foreach(var item in list)
            {
                if(item.TrangThaiDH.Equals("Chờ xác nhận"))
                {
                    item.statusDH = "text-danger";
                } else if (item.TrangThaiDH.Equals("Đang giao hàng"))
                {
                    item.statusDH = "text-info";
                } else if (item.TrangThaiDH.Equals("Đã giao"))
                {
                    item.statusDH = "text-success";
                }

                if (item.TrangThaiTT.Equals("Chờ thanh toán"))
                {
                    item.statusTT = "text-info";
                }
                else if (item.TrangThaiTT.Equals("Đã thanh toán"))
                {
                    item.statusTT = "text-success";
                }
            }
            return View(list);
        }
        public ActionResult DetailOrder(string id)
        {
            KHACHHANG kh = Session["kh"] as KHACHHANG;
            ViewBag.Ten = kh.HoTen;
            ViewBag.HD = db.HDBANs.Where(s => s.MaHD == id).FirstOrDefault();
            return View(db.CHITIETHDBANs.Where(s => s.ID_HDBAN == id).ToList());
        }
    }
}