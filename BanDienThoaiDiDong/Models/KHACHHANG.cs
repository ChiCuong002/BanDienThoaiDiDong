﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanDienThoaiDiDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.ModelBinding;

    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            this.HDBANs = new HashSet<HDBAN>();
        }
        public int MaKH { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên!")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [RegularExpression(@"^\d{3}\d{3}\d{4}$", ErrorMessage = "Hãy nhập số điện hợp lệ (XXXXXXXXXX).")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu!")]
        [DataType(DataType.Password)]
        [Compare("MatKhau")]
        [NotMapped]
        public string ConfirmMatKhau { get; set; }
        public Nullable<bool> HienThi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HDBAN> HDBANs { get; set; }
    }
}
