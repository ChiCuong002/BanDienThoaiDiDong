using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong.Models
{
    public class UserCart
    {
        DB_DiDongEntities db = new DB_DiDongEntities();
        List<ChiTietGH> items = new List<ChiTietGH>();
        public UserCart(List<ChiTietGH> items)
        {
            this.items = items;
        }

        public IEnumerable<ChiTietGH> Items
        {
            get { return items; }
        }
        public int? Total_quantity()
        {
            return Items.Sum(s => s.SoLuong);
        }
        public decimal Total_money()
        {
            return (decimal)Items.Sum(s => s.SANPHAM.Gia * s.SoLuong);
        }
        public decimal Total_money_each(int id)
        {
            
            return (decimal)Items.Where(s => s.id == id).Sum(s => s.SANPHAM.Gia * s.SoLuong);
        }
        public string GetDungLuong(string masp)
        {
            var id = masp.Substring(masp.IndexOf("m") + 1,masp.IndexOf("d") - masp.IndexOf("m"));
            var dungluong = db.Capacities.Where(s => s.MaCapacity == id).FirstOrDefault();
            return dungluong.DungLuong;
        }
        public string GetMau(string masp)
        {
            var maMau = masp.Substring(0, masp.IndexOf("m") + 1);
            var mau = db.Colors.Where(s => s.MaColor == maMau).FirstOrDefault();
            return mau.TenMau;
        }
        public bool Add_To_Cart(string masp, int giohang_id)
        {
            try
            {
                var sanpham = Items.FirstOrDefault(s => s.MaSP == masp && s.MaGH == giohang_id);
                if (sanpham == null)
                {
                    ChiTietGH chiTietGH = new ChiTietGH();
                    chiTietGH.MaGH = giohang_id;
                    chiTietGH.MaSP = masp;
                    chiTietGH.SoLuong = 1;
                    db.ChiTietGHs.Add(chiTietGH);
                    db.SaveChanges();
                }
                else
                {
                    var sp = db.ChiTietGHs.Where(s => s.MaSP == masp && s.MaGH == giohang_id).FirstOrDefault();
                    sp.SoLuong += 1;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void ClearCart()
        {
            foreach (var item in Items)
            {
                var chitiet = db.ChiTietGHs.Where(s => s.id == item.id).FirstOrDefault();
                db.ChiTietGHs.Remove(chitiet);
            }
            db.SaveChanges();
        }
    }
}