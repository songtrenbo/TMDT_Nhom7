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
    
    public partial class Banner
    {
        public int MaBanner { get; set; }
        public string TenBanner { get; set; }
        public string Path { get; set; }
        public long MaSanPham { get; set; }
        public Nullable<bool> IsHide { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
    
        public virtual SanPham SanPham { get; set; }
    }
}
