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
    
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            this.CTHoaDons = new HashSet<CTHoaDon>();
        }
    
        public int MaHoaDon { get; set; }
        public System.DateTime NgayMua { get; set; }
        public int MaKhachHang { get; set; }
        public Nullable<int> MaPhieuQuaTang { get; set; }
        public Nullable<int> SoTienGiam { get; set; }
        public Nullable<int> PhiGiaoHang { get; set; }
        public Nullable<int> TinhTrang { get; set; }
        public Nullable<int> TongThanhToan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHoaDon> CTHoaDons { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual PhieuQuaTang PhieuQuaTang { get; set; }
    }
}
