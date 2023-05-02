using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong.Models
{
    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string MatKhauMoi { get; set; }
        public string ConfirmMatKhauMoi { get; set; }
    }
}