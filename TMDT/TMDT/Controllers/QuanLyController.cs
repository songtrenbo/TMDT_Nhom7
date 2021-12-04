﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
using TMDT.Utility;

namespace TMDT.Controllers
{
    public class QuanLyController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: QuanLy
        #region ThuongHieu
        public ActionResult QuanLyThuongHieu()
        {
            var thuongHieu = database.ThuongHieux.ToList();
            thuongHieu.OrderBy(s => s.TenThuongHieu);
            return View(thuongHieu);
        }
        public ActionResult ThuongHieuCreate()
        {
            ThuongHieu thuongHieu = new ThuongHieu();
            return View(thuongHieu);
        }
        [HttpPost]
        public ActionResult ThuongHieuCreate(ThuongHieu thuongHieu)
        {
            var check = database.ThuongHieux.Where(s => s.TenThuongHieu == thuongHieu.TenThuongHieu).FirstOrDefault();
            if (check == null)
            {
                thuongHieu.NgayTao = DateTime.Now;
                thuongHieu.NgayChinhSua = null;
                thuongHieu.IsDeleted = false;

                try
                {
                    if (thuongHieu.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(thuongHieu.UploadImage.FileName);
                        string extent = Path.GetExtension(thuongHieu.UploadImage.FileName);
                        filename = filename + extent;
                        thuongHieu.Hinh = "/Content/ThuongHieuImages/" + filename;
                        thuongHieu.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/ThuongHieuImages/"), filename));
                    }

                    database.ThuongHieux.Add(thuongHieu);
                    database.SaveChanges();
                    return RedirectToAction("QuanLyThuongHieu");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ViewBag.IsExist = "Thương hiệu đã tồn tại!";

                return View(thuongHieu);
            }
        }
        public ActionResult ThuongHieuDetails(int id)
        {
            return View(database.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault());
        }
        public ActionResult ThuongHieuEdit(int id)
        {
            return View(database.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult ThuongHieuEdit(int id, ThuongHieu thuongHieu)
        {
            thuongHieu.NgayChinhSua = DateTime.Now;
            thuongHieu.NgayTao = database.ThuongHieux.Where(s => s.MaThuongHieu == id).Select(s => s.NgayTao).FirstOrDefault();
            try
            {
                if (thuongHieu.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(thuongHieu.UploadImage.FileName);
                    string extent = Path.GetExtension(thuongHieu.UploadImage.FileName);
                    filename = filename + extent;
                    thuongHieu.Hinh = "/Content/ThuongHieuImages/" + filename;
                    thuongHieu.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/ThuongHieuImages/"), filename));
                }
                else
                {
                    string img = database.ThuongHieux.Where(s => s.MaThuongHieu == id).Select(s => s.Hinh).FirstOrDefault();
                    thuongHieu.Hinh = img;
                }
                database.Entry(thuongHieu).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyThuongHieu");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ThuongHieuDelete(int id)
        {
            var thuongHieu = database.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault();
            var LaptopHasThuongHieu = database.SanPhams.Where(s => s.MaThuongHieu == id).ToList();
            foreach (var item in LaptopHasThuongHieu)
            {
                database.SanPhams.Remove(item);
            }
            database.ThuongHieux.Remove(thuongHieu);
            database.SaveChanges();
            return RedirectToAction("QuanLyThuongHieu");
        }
        #endregion
        public ActionResult QuanLySanPham()
        {

            return View();
        }
        public ActionResult QuanLyNhapKho()
        {
            return View();
        }


        #region QLNhanVien
        public ActionResult QuanLyNhanVien()
        {
            var nhanviens = database.NguoiDungs.Where(s => s.MaQuyen == 3 && s.Status != 3).ToList().OrderBy(s => s.MaQuyen);
            return View(nhanviens);
        }
        public ActionResult NhanVienCreate()
        {
            return View(new NguoiDung());
        }
        [HttpPost]
        public ActionResult NhanVienCreate(NguoiDung nguoiDung)
        {
            var check = database.NguoiDungs.Where(s => s.Username == nguoiDung.Username).FirstOrDefault();
            if (check == null)
            {
                nguoiDung.NgayTao = DateTime.Now;
                var password = Utils.Crypto(nguoiDung.Username);
                nguoiDung.Password = password;
                nguoiDung.ConfirmPass = password;
                nguoiDung.Status = 1;
                nguoiDung.MaQuyen = 3;

                try
                {
                    database.NguoiDungs.Add(nguoiDung);
                    database.SaveChanges();
                    return RedirectToAction("QuanLyNhanVien");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ViewBag.IsExist = "Username đã tồn tại!";

                return View(nguoiDung);
            }
        }
      
        public ActionResult NhanVienLock(int id)
        {
            var nguoiDung = database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            nguoiDung.ConfirmPass = nguoiDung.Password;
            nguoiDung.NgayChinhSua = DateTime.Now;
            nguoiDung.Status = 2;
            database.Entry(nguoiDung).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyNhanVien","QuanLy");
        }
        [HttpPost]
        public ActionResult ResetPassword(int id)
        {
            var nguoiDung = database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            nguoiDung.NgayChinhSua = DateTime.Now;
            var password = Utils.Crypto(nguoiDung.Username);
            nguoiDung.Password = password;
            nguoiDung.ConfirmPass = password;
            try
            {
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult NhanVienEdit(int id)
        {
            return View(database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult NhanVienEdit(int id, NguoiDung nguoiDung)
        {
            nguoiDung.NgayChinhSua = DateTime.Now;
            try
            {
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult NhanVienDelete(int id)
        {
            try
            {
                var nguoiDung = database.NguoiDungs.Find(id);
                nguoiDung.Status = 3;
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        public ActionResult QuanLyVoucher()
        {
            var voucher = database.PhieuQuaTangs.ToList();
            return View(voucher);
        }
        public ActionResult VoucherCreate()
        {
            List<ThuongHieu> thuonghieu = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuonghieu, "MaThuongHieu", "TenThuongHieu");
            PhieuQuaTang phieuquatang = new PhieuQuaTang();
            return View(phieuquatang);
        }
        [HttpPost]
        public ActionResult VoucherCreate(PhieuQuaTang phieuquatang, string ngayBatDau, string ngayKetThuc)
        {
            var check_ID = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == phieuquatang.MaPhieuQuaTang).FirstOrDefault();

            if (check_ID == null)
            {
                if (phieuquatang.NgayKichHoat > phieuquatang.NgayKetThuc)
                {
                    return View();
                }
                database.Configuration.ValidateOnSaveEnabled = false;
                phieuquatang.NgayTao = DateTime.Now;
                phieuquatang.Status = 2;
                phieuquatang.NgayKichHoat = DateTime.Parse(ngayBatDau);
                phieuquatang.NgayKetThuc = DateTime.Parse(ngayKetThuc);
                database.PhieuQuaTangs.Add(phieuquatang);
                database.SaveChanges();
                return RedirectToAction("QuanLyVoucher");
            }
            else
            {
                ViewBag.Error = "voucher đã tồn tại";
                return View();
            }
        }
        public ActionResult EditVoucher(int id)
        {
            List<ThuongHieu> thuonghieu = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuonghieu, "MaThuongHieu", "TenThuongHieu");
            var voucher = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == id).FirstOrDefault();
            return View(voucher);
        }
        [HttpPost]
        public ActionResult EditVoucher(int id, PhieuQuaTang phieuquatang, string ngayBatDau, string ngayKetThuc)
        {
         
            List<ThuongHieu> thuonghieu = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuonghieu, "MaThuongHieu", "TenThuongHieu");           
            try
            {
                if(phieuquatang.Status==3 && phieuquatang.SoLuong > 0)
                {
                    phieuquatang.Status = 2;
                }
                else if(phieuquatang.Status!=3 && phieuquatang.SoLuong == 0)
                {
                    phieuquatang.Status = 3;
                }
                phieuquatang.NgayKichHoat = DateTime.Parse(ngayBatDau);
                phieuquatang.NgayKetThuc = DateTime.Parse(ngayKetThuc);
                database.Entry(phieuquatang).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyVoucher");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ThayDoiTrangThai(int id)
        {
            var phieuquatang = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == id).FirstOrDefault();
            switch (phieuquatang.Status)
            {
                case 1:
                    phieuquatang.Status = 2;
                   
                    break;
                case 2:
                    phieuquatang.Status = 1;
                    break;
            }
            database.Entry(phieuquatang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyVoucher");
        }

            #region QLBaoCao
        public ActionResult QuanLyBaoCao(string searching, string Year, string Month)
        {
            var dates = database.HoaDons.Where(s => (s.NgayMua.Year + "-" + s.NgayMua.Month + "-" + s.NgayMua.Day) == searching).ToList();

            var ngaymua = dates.ToList();


            int soluong = 0;
            foreach (var item in ngaymua)
            {
                soluong += item.CTHoaDons.Count;
                ViewBag.ngay = item.NgayMua;
            }
            ViewBag.sl = soluong;

            //List<int> thangs = new List<int>(12);
            //List<int> gianhap = new List<int>();
            //List<int> doanhthu = new List<int>();
            //List<int> giagiam = new List<int>();
            


            if (Month != "" && Year != null)
            {
                int ngaytrongthang = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(Month));
                double[] doanhthutuans = new double[5];
                double[] giamgiatuans = new double[5];
                double[] giavontuans = new double[5];
                double[] loinhuantuans = new double[5];
                int ngaytrongtuan1 = 1; 
                int ngaytrongtuan2 = 7;
                for (int i = 0; i < 5;i++)
                {
                    if (ngaytrongthang >= ngaytrongtuan2)
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongtuan2 && s.NgayMua.Day>=ngaytrongtuan1).ToList();
                        ngaytrongtuan1 += 7;
                        ngaytrongtuan2 += 7;
                        foreach(var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                    else
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongthang && s.NgayMua.Day >= ngaytrongtuan1).ToList();
                        
                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                }
                ViewBag.doanhthutuans = doanhthutuans;
                ViewBag.giamgiatuans = giamgiatuans;
                ViewBag.giavontuans = giavontuans;
                ViewBag.loinhuantuans = loinhuantuans;

                double tongdoanhthutuan = 0, tonggiamgiatuan = 0, tonggiavongtuan = 0, tongloinhuantuan = 0;



                foreach (var i in doanhthutuans)
                {
                    tongdoanhthutuan += i;

                }

                foreach (var i in giamgiatuans)
                {
                    tonggiamgiatuan += i;
                }
                foreach (var i in giavontuans)
                {
                    tonggiavongtuan += i;
                }
                foreach (var i in loinhuantuans)
                {
                    tongloinhuantuan += i;
                }
                ViewBag.tongdoanhthutuan = tongdoanhthutuan;
                ViewBag.tonggiamgiatuan = tonggiamgiatuan;
                ViewBag.tonggiavongtuan = tonggiavongtuan;
                ViewBag.tongloinhuantuan = tongloinhuantuan;
                ViewBag.ngay = System.DateTime.DaysInMonth(Int32.Parse(Year), Int32.Parse(Month));
            }
            if (Month == "" && Year != null)
            {

                double[] doanhthuthangs = new double[15];
                double[] giamgiathangs = new double[15];
                double[] giavonthangs = new double[15];
                double[] loinhuanthangs = new double[15];
                var hoadonNam = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year).ToList();
                foreach (var itemhd in hoadonNam)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (itemhd.NgayMua.Month == i)
                        {
                            double temp1 = itemhd.TongThanhToan ?? 0;
                            double temp2 = itemhd.SoTienGiam ?? 0;
                            doanhthuthangs[i] = doanhthuthangs[i] + temp1;
                            giamgiathangs[i] = giamgiathangs[i] + temp2;

                            foreach (var itemcthd in itemhd.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavonthangs[i] = giavonthangs[i] + temp3;
                            }

                            loinhuanthangs[i] = doanhthuthangs[i] - giavonthangs[i];
                        }
                    }
                }
                ViewBag.doanhthuthangs = doanhthuthangs.Skip(1).ToArray();
                ViewBag.giamgiathangs = giamgiathangs;
                ViewBag.giavonthangs = giavonthangs;
                ViewBag.loinhuanthangs = loinhuanthangs;

                double tongdoanhthuthang = 0, tonggiamgiathang = 0, tonggiavongthang = 0, tongloinhuanthang = 0;



                foreach (var i in doanhthuthangs)
                {
                    tongdoanhthuthang += i;

                }

                foreach (var i in giamgiathangs)
                {
                    tonggiamgiathang += i;
                }
                foreach (var i in giavonthangs)
                {
                    tonggiavongthang += i;
                }
                foreach (var i in loinhuanthangs)
                {
                    tongloinhuanthang += i;
                }
                ViewBag.tongdoanhthuthang = tongdoanhthuthang;
                ViewBag.tonggiamgiathang = tonggiamgiathang;
                ViewBag.tonggiavongthang = tonggiavongthang;
                ViewBag.tongloinhuanthang = tongloinhuanthang;

            }



            ViewBag.thang = Month;
            ViewBag.nam = Year;

            return View(dates);
        }
        public ActionResult QuanLyBaoCaoXuatPDF(string Acc, string Year, string Month)
        {

            double[] doanhthuthangs = new double[15];
            double[] giamgiathangs = new double[15];
            double[] giavonthangs = new double[15];
            double[] loinhuanthangs = new double[15];


            if (Month != null && Year != null)
            {
                int ngaytrongthang = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(Month));
                double[] doanhthutuans = new double[5];
                double[] giamgiatuans = new double[5];
                double[] giavontuans = new double[5];
                double[] loinhuantuans = new double[5];
                int ngaytrongtuan1 = 1;
                int ngaytrongtuan2 = 7;
                for (int i = 0; i < 5; i++)
                {
                    if (ngaytrongthang >= ngaytrongtuan2)
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongtuan2 && s.NgayMua.Day >= ngaytrongtuan1).ToList();
                        ngaytrongtuan1 += 7;
                        ngaytrongtuan2 += 7;
                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                    else
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongthang && s.NgayMua.Day >= ngaytrongtuan1).ToList();

                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                }
                ViewBag.doanhthutuans = doanhthutuans;
                ViewBag.giamgiatuans = giamgiatuans;
                ViewBag.giavontuans = giavontuans;
                ViewBag.loinhuantuans = loinhuantuans;

                double tongdoanhthutuan = 0, tonggiamgiatuan = 0, tonggiavongtuan = 0, tongloinhuantuan = 0;



                foreach (var i in doanhthutuans)
                {
                    tongdoanhthutuan += i;

                }

                foreach (var i in giamgiatuans)
                {
                    tonggiamgiatuan += i;
                }
                foreach (var i in giavontuans)
                {
                    tonggiavongtuan += i;
                }
                foreach (var i in loinhuantuans)
                {
                    tongloinhuantuan += i;
                }
                ViewBag.tongdoanhthutuan = tongdoanhthutuan;
                ViewBag.tonggiamgiatuan = tonggiamgiatuan;
                ViewBag.tonggiavongtuan = tonggiavongtuan;
                ViewBag.tongloinhuantuan = tongloinhuantuan;
                ViewBag.ngay = System.DateTime.DaysInMonth(Int32.Parse(Year), Int32.Parse(Month));
            }
            if (Month == null && Year != null)
            {
                var hoadonNam = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year).ToList();
                foreach (var itemhd in hoadonNam)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (itemhd.NgayMua.Month == i)
                        {
                            double temp1 = itemhd.TongThanhToan ?? 0;
                            double temp2 = itemhd.SoTienGiam ?? 0;
                            doanhthuthangs[i] = doanhthuthangs[i] + temp1;
                            giamgiathangs[i] = giamgiathangs[i] + temp2;

                            foreach (var itemcthd in itemhd.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap??0) * itemcthd.SoLuong;
                                giavonthangs[i] = giavonthangs[i] + temp3;
                            }

                            loinhuanthangs[i] = doanhthuthangs[i] - giavonthangs[i];
                        }
                    }
                }
            }
            ViewBag.doanhthuthangs = doanhthuthangs.Skip(1).ToArray(); ;
            ViewBag.giamgiathangs = giamgiathangs;
            ViewBag.giavonthangs = giavonthangs;
            ViewBag.loinhuanthangs = loinhuanthangs;

            double tongdoanhthuthang = 0, tonggiamgiathang = 0, tonggiavongthang = 0, tongloinhuanthang = 0;
            foreach (var i in doanhthuthangs)
            {
                tongdoanhthuthang += i;
            }
            foreach (var i in giamgiathangs)
            {
                tonggiamgiathang += i;
            }
            foreach (var i in giavonthangs)
            {
                tonggiavongthang += i;
            }
            foreach (var i in loinhuanthangs)
            {
                tongloinhuanthang += i;
            }
            ViewBag.tongdoanhthuthang = tongdoanhthuthang;
            ViewBag.tonggiamgiathang = tonggiamgiathang;
            ViewBag.tonggiavongthang = tonggiavongthang;
            ViewBag.tongloinhuanthang = tongloinhuanthang;

            ViewBag.thang = Month;
            ViewBag.nam = Year;


            //var acc = (NguoiDung)Session["Account"];
            ViewBag.ten = Acc;
            return View();
        }

        public ActionResult GeneratePDF(string Year, string Month)
        {
            var acc = (NguoiDung)Session["Account"];
            string Acc = acc.Ten;
            return new Rotativa.ActionAsPdf("QuanLyBaoCaoXuatPDF", new { Acc, Year = Year, Month = Month });
        }


        public ActionResult BaoCaoDetail(int id)
        {
            var hoadon = database.HoaDons.Where(s => s.MaHoaDon == id).FirstOrDefault();

            return View(hoadon);
        }
        #endregion
    }
}