using BanDienThoaiDiDong.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BanDienThoaiDiDong.Controllers
{
    public class DanhSachSanPhamController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: DanhSachSanPham
        public ActionResult Index(string sortOrder, string SearchString, int? page, string currentFilter)
        {
            var lstSanPham = database.SANPHAMs.ToList();
            ViewBag.CurrentSort = sortOrder;
             
            //sap xep 
            ViewBag.SortTheoTen = String.IsNullOrEmpty(sortOrder) ? "ma_desc" : "";
            ViewBag.SortTheoGia = sortOrder == "price" ? "price_desc" : "price";

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            ViewBag.CurrentFilter = SearchString;
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstSanPham = database.SANPHAMs.Where(n => n.TenSP.Contains(SearchString)).ToList();
            }
            else
            {
                //lay san pham trong SANPHAM
                lstSanPham = database.SANPHAMs.ToList();
            }
            switch (sortOrder)
            {
                case "ma_desc":
                    lstSanPham = lstSanPham.OrderByDescending(s => s.TenSP).ToList();
                    break;
                case "price":
                    lstSanPham = lstSanPham.OrderBy(s => s.Gia).ToList();
                    break;
                case "price_desc":
                    lstSanPham = lstSanPham.OrderByDescending(s => s.Gia).ToList();
                    break;
                default:
                    lstSanPham = lstSanPham.OrderBy(s => s.TenSP).ToList();
                    break;
            }
            ViewBag.dsHang = database.LOAISANPHAMs.ToList();
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return PartialView("Index",lstSanPham.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SanPhamTheoThuongHieu(int id, string SearchString, string sortOrder, int? page, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortTheoTen = String.IsNullOrEmpty(sortOrder) ? "ma_desc" : "";
            ViewBag.SortTheoGia = sortOrder == "price" ? "price_desc" : "price";
            var lstSanPham = database.SANPHAMs.Where(n => n.MaLoai == id).ToList();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            ViewBag.CurrentFilter = SearchString;
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstSanPham = database.SANPHAMs.Where(n => n.TenSP.Contains(SearchString)).ToList();

            }
            else
            {
                //lay san pham trong SANPHAM
                lstSanPham = database.SANPHAMs.Where(n => n.MaLoai == id).ToList();
            }
            switch (sortOrder)
            {
                case "ma_desc":
                    lstSanPham = lstSanPham.OrderByDescending(s => s.TenSP).ToList();
                    break;
                case "price":
                    lstSanPham = lstSanPham.OrderBy(s => s.Gia).ToList();
                    break;
                case "price_desc":
                    lstSanPham = lstSanPham.OrderByDescending(s => s.Gia).ToList();
                    break;
                default:
                    lstSanPham = lstSanPham.OrderBy(s => s.TenSP).ToList();
                    break;
            }
            int pageSize = 9;
            int pageNum = (page ?? 1);
            return View(lstSanPham.ToPagedList(pageNum,pageSize));
        }
    }
}