//------------------------------------------------------------------------------
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

    public partial class PhieuQuaTang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuQuaTang()
        {
            this.HoaDons = new HashSet<HoaDon>();
            this.NguoiDung_PhieuQuaTang = new HashSet<NguoiDung_PhieuQuaTang>();
        }
    
        public int MaPhieuQuaTang { get; set; }
        [Display(Name = "Tên phiếu quà tặng")]
        public string TenPhieuQuaTang { get; set; }
        [Display(Name = "Mã giảm giá")]
        public string MaGiamGia { get; set; }
        [Display(Name = "Loại phạm vi")]
        public int LoaiPhamVi { get; set; }
        [Display(Name = "Giá trị")]
        public Nullable<double> GiaTri { get; set; }
        [Display(Name = "Giá trị đơn hàng tối thiểu")]
        public Nullable<double> GiaTriDonHangToiThieu { get; set; }
        [Display(Name = "Số lượng")]
        public Nullable<int> SoLuong { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public System.DateTime NgayKichHoat { get; set; }
        public System.DateTime NgayKetThuc { get; set; }
        public Nullable<int> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NguoiDung_PhieuQuaTang> NguoiDung_PhieuQuaTang { get; set; }
    }
}
