using BanDienThoaiDiDong.Models;
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
        public ActionResult Index(string sortOrder, string SearchString)
        {

            DanhSachSPvaTH objSanPham = new DanhSachSPvaTH();

            objSanPham.lstHang = database.LOAISANPHAMs.ToList();
            //sap xep
            ViewBag.SortTheoTen = String.IsNullOrEmpty(sortOrder) ? "ma_desc" : "";
            if (!string.IsNullOrEmpty(SearchString))
            {
                objSanPham.lstSanPham = database.SANPHAMs.Where(n => n.TenSP.Contains(SearchString)).ToList();

            }
            else
            {
                //lay san pham trong SANPHAM
                objSanPham.lstSanPham = database.SANPHAMs.ToList();
            }
            switch (sortOrder)
            {
                case "ma_desc":
                    objSanPham.lstSanPham = objSanPham.lstSanPham.OrderByDescending(s => s.TenSP).ToList();
                    break;
                default:
                    objSanPham.lstSanPham = objSanPham.lstSanPham.OrderBy(s => s.TenSP).ToList();
                    break;
            }
            return View(objSanPham);
        }
        public ActionResult SanPhamTheoThuongHieu(int id, string SearchString, string sortOrder)
        {
            ViewBag.SortTheoTen = String.IsNullOrEmpty(sortOrder) ? "ma_desc" : "";
            ViewBag.TenSortParm = sortOrder == "ten" ? "ten_desc" : "ten";
            var lstSanPham = database.SANPHAMs.Where(n => n.MaSP == id).ToList();
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstSanPham = database.SANPHAMs.Where(n => n.TenSP.Contains(SearchString)).ToList();

            }
            else
            {
                //lay san pham trong SANPHAM
                lstSanPham = database.SANPHAMs.Where(n => n.MaLoaiSP == id).ToList();
            }
            switch (sortOrder)
            {
                case "ma_desc":
                    lstSanPham = lstSanPham.OrderByDescending(s => s.TenSP).ToList();
                    break;
                default:
                    lstSanPham = lstSanPham.OrderBy(s => s.TenSP).ToList();
                    break;
            }
            return View(lstSanPham);
        }
    }
}