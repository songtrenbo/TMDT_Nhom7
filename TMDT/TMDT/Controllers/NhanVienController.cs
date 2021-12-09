using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class NhanVienController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: NhanVien
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLyDonHang()
        {
            var donHang = database.HoaDons.ToList();
            donHang = donHang.OrderByDescending(s => s.NgayMua).ToList();
            return View(donHang);
        }
        public ActionResult DonHangChiTiet(int maDonHang)
        {
            var donHangCT = database.CTHoaDons.Where(s => s.MaHoaDon == maDonHang).ToList();
            ViewData["DonHang"] = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            double tongTien = 0;
            foreach (var item in donHangCT)
            {
                tongTien += item.DonGia * item.SoLuong;
            }
            ViewBag.TongTien = tongTien;
            return View(donHangCT);
        }
        public ActionResult XacNhanDonHang(int maDonHang)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 2;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyDonHang", "NhanVien");
        }
        public ActionResult HuyDonHang(int maDonHang)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 5;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyDonHang", "NhanVien");

        }
    }
}