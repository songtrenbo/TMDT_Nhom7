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
    
    public partial class OCung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OCung()
        {
            this.SanPhams = new HashSet<SanPham>();
        }
    
        public int MaOCung { get; set; }
        public Nullable<int> LoaiOCung { get; set; }
        public Nullable<int> DungLuong { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> NgayChinhSua { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}