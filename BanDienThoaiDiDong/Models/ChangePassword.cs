using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới!")]
        [DataType(DataType.Password)]
        public string MatKhauMoi { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới!")]
        [DataType(DataType.Password)]
        [Compare("MatKhauMoi")]
        public string ConfirmMatKhauMoi { get; set; }
    }
}