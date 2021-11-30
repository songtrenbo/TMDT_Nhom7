﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguoiDung()
        {
            this.DanhGias = new HashSet<DanhGia>();
            this.HoaDons = new HashSet<HoaDon>();
            this.HoaDons1 = new HashSet<HoaDon>();
        }
        public int MaNguoiDung { get; set; }
        [Display(Name = "Tên người dùng")]
        [Required(ErrorMessage = "Tên người dùng không được bỏ trống")]
        [StringLength(100, MinimumLength = 5)]
        public string Ten { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được bỏ trống")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không trùng khớp")]
        [DataType(DataType.Password)]
        public string ConfirmPass { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        public Nullable<int> DiemThuong { get; set; }
        public int MaQuyen { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> NgayChinhSua { get; set; }
        public Nullable<int> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons1 { get; set; }
        public virtual Quyen Quyen { get; set; }
    }
}
