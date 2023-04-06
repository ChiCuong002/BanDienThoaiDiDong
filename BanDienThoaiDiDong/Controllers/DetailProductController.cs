using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoaiDiDong.Controllers
{
    public class DetailProductController : Controller
    {
        DB_DiDongEntities database = new DB_DiDongEntities();
        // GET: DetailProduct
        public ActionResult Index(int id)
        {
            var objProduct = database.SANPHAMs.Where(n => n.MaSP == id).FirstOrDefault();
            return View(objProduct);
        }
    }
}