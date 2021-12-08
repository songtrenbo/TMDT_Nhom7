﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class KhachHangController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }
        #region Voucher
        public ActionResult Voucher()
        {
            //Kiểm tra ngày hết hạng
            var checkNgay = database.PhieuQuaTangs.ToList();
            foreach (var item in checkNgay)
            {
                if (item.NgayKetThuc < DateTime.Now)
                {
                    item.Status = 3;
                    database.Entry(item).State = EntityState.Modified;
                    database.SaveChanges();
                }
            }

            var acc = (NguoiDung)Session["Account"];
            var voucher = database.PhieuQuaTangs.Where(s => s.Status == 2).ToList();
  
            ViewBag.ThuongHieu = database.ThuongHieux.ToList();

            //kiểm tra account đã có kích hoạt voucher chưa
            var checkVoucher = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == acc.MaNguoiDung).ToList();
            ViewBag.checkVoucher = checkVoucher;

            
            return View(voucher);
        }
        public ActionResult KichHoatVoucher(int id)
        {
            var acc = (NguoiDung)Session["Account"];
            NguoiDung_PhieuQuaTang ngdung_phieuqt = new NguoiDung_PhieuQuaTang();
            ngdung_phieuqt.MaNguoiDung = acc.MaNguoiDung;
            ngdung_phieuqt.MaPhieuQuaTang = id; 
            database.NguoiDung_PhieuQuaTang.Add(ngdung_phieuqt);

            var phieuqt = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == id).FirstOrDefault();
            phieuqt.SoLuong -= 1;
            database.Entry(phieuqt).State = EntityState.Modified;

            database.SaveChanges();
            return RedirectToAction("Voucher");
        }

        public ActionResult VoucherCuaToi()
        {
            var acc = (NguoiDung)Session["Account"];
            //Kiểm tra ngày hết hạng
            var phieuQT = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == acc.MaNguoiDung && s.PhieuQuaTang.Status==2).ToList();
            foreach (var item in phieuQT)
            {
                if (item.PhieuQuaTang.NgayKetThuc < DateTime.Now)
                {
                    item.PhieuQuaTang.Status = 3;
                    database.Entry(item).State = EntityState.Modified;
                    database.SaveChanges();
                }
            }

           

            ViewBag.ThuongHieu = database.ThuongHieux.ToList();
            //kiểm tra account đã có kích hoạt voucher chưa
            var checkVoucher = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == acc.MaNguoiDung).ToList();
            ViewBag.checkVoucher = checkVoucher;
            return View(phieuQT);
        }
        public ActionResult VoucherHetHan()
        {
            var acc = (NguoiDung)Session["Account"];
            //Kiểm tra ngày hết hạng
            var phieuQT = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == acc.MaNguoiDung && s.PhieuQuaTang.Status == 3).ToList();
            foreach (var item in phieuQT)
            {
                if (item.PhieuQuaTang.NgayKetThuc < DateTime.Now)
                {
                    item.PhieuQuaTang.Status = 3;
                    database.Entry(item).State = EntityState.Modified;
                    database.SaveChanges();
                }
            }


            ViewBag.ThuongHieu = database.ThuongHieux.ToList();

            //kiểm tra account đã có kích hoạt voucher chưa
            var checkVoucher = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == acc.MaNguoiDung).ToList();
            ViewBag.checkVoucher = checkVoucher;
            return View(phieuQT);
        }
        #endregion

        public ActionResult DonMua()
        {
            return View();
        }
    }
}