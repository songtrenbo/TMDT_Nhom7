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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBLaptopEntities1 : DbContext
    {
        public DBLaptopEntities1()
            : base("name=DBLaptopEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CPU> CPUs { get; set; }
        public virtual DbSet<CTHoaDon> CTHoaDons { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<GPU> GPUs { get; set; }
        public virtual DbSet<Hinh> Hinhs { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<PhieuQuaTang> PhieuQuaTangs { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<ThuongHieu> ThuongHieux { get; set; }
    }
}
