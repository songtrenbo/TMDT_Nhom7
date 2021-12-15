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
        public PartialViewResult QuanLyDonHang(int tinhTrang = 1)
        {
            var donHang = database.HoaDons.Where(s => s.TinhTrang == tinhTrang).ToList();
            donHang = donHang.OrderByDescending(s => s.NgayMua).ToList();
            return PartialView(donHang);
        }
        public ActionResult DonHangChiTiet(int maDonHang)
        {
            var donHangCT = database.CTHoaDons.Where(s => s.MaHoaDon == maDonHang).ToList();
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            ViewData["DonHang"] = donHang;
            ViewData["NguoiDung"] = database.NguoiDungs.Where(s => s.MaNguoiDung == donHang.MaKhachHang).FirstOrDefault();
            double tongTien = 0;
            foreach (var item in donHangCT)
            {
                tongTien += item.DonGia * item.SoLuong;
            }
            ViewBag.TongTien = tongTien;
            return View(donHangCT);
        }
        public ActionResult Bill(int maDonHang)
        {
            var donHangCT = database.CTHoaDons.Where(s => s.MaHoaDon == maDonHang).ToList();
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            ViewData["DonHang"] = donHang;
            ViewData["NguoiDung"] = database.NguoiDungs.Where(s => s.MaNguoiDung == donHang.MaKhachHang).FirstOrDefault();
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
            NguoiDung nguoiDung = (NguoiDung)Session["Account"];
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 2;
            donHang.MaNVDuyet = nguoiDung.MaNguoiDung;
            database.Entry(donHang).State = EntityState.Modified;

            database.SaveChanges();
            return RedirectToAction("Index", "NhanVien");
        }
        public ActionResult HuyDonHang(int maDonHang)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 5;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index", "NhanVien");
        }
        public ActionResult SanSangGiaoHang(int maDonHang)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 3;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index", "NhanVien");
        }
        public ActionResult XacNhanDaGiao(int maDonHang)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 4;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index", "NhanVien");
        }
    }
}