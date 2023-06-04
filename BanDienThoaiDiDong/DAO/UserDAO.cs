using BanDienThoaiDiDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong.DAO
{
    public class UserDAO
    {
        DB_DiDongEntities db = new DB_DiDongEntities();

        public long InsertForFacebook(KHACHHANG kh)
        {
            var user = db.KHACHHANGs.FirstOrDefault(x => x.Email == kh.Email);
            if (user == null)
            {
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return kh.MaKH;
            }
            return user.MaKH;
        }
    }
}