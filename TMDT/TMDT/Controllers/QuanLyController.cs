﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

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
            if(check == null)
            {
                thuongHieu.SoLuong = 0;
                thuongHieu.NgayTao = DateTime.Now;
                thuongHieu.NgayChinhSua = null;
                thuongHieu.IsDeleted = false;
                thuongHieu.IsLockEdit = false;

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
        public ActionResult QuanLyNhanVien()
        {
            var nhanviens = database.NguoiDungs.Where(s => s.MaNguoiDung == 3).ToList();
            return View(nhanviens);
        }
        public ActionResult QuanLyVoucher()
        {
            return View();
        }
        public ActionResult QuanLyBaoCao(string searching, string Year, string Month)
        {
            var dates = database.HoaDons.Where(s => (s.NgayMua.Year+"-"+s.NgayMua.Month+"-"+s.NgayMua.Day) == searching).ToList();
         
            var ngaymua = dates.ToList();
            

            int soluong = 0;
            foreach(var item in ngaymua)
            {
                soluong += item.CTHoaDons.Count;
                ViewBag.ngay = item.NgayMua;
            }
            ViewBag.sl = soluong;

            //List<int> thangs = new List<int>(12);
            //List<int> gianhap = new List<int>();
            //List<int> doanhthu = new List<int>();
            //List<int> giagiam = new List<int>();
            int[] doanhthuthangs = new int[15];
            int[] giamgiathangs = new int[15];
            int[] giavonthangs = new int[15];
            int[] loinhuanthangs = new int[15];


            if (Month!="" && Year != null)
            {
                var a = System.DateTime.DaysInMonth(2001, 1);
            }
            if(Month=="" && Year != null)
            {
                var hoadonNam = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year).ToList();
                foreach(var itemhd in hoadonNam)
                {
                    for(int i = 1; i <= 12; i++)
                    {
                        if (itemhd.NgayMua.Month == i)
                        {
                            int temp1 = itemhd.TongThanhToan ?? 0;
                            int temp2 = itemhd.SoTienGiam ?? 0;
                            doanhthuthangs[i] = doanhthuthangs[i] + temp1;
                            giamgiathangs[i] = giamgiathangs[i] + temp2;
                            
                            foreach(var itemcthd in itemhd.CTHoaDons)
                            {
                                int temp3 = (itemcthd.SanPham.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavonthangs[i] = giavonthangs[i]+temp3;
                            }

                            loinhuanthangs[i] = doanhthuthangs[i] - giavonthangs[i];
                        }
                    }
                }
            }


            ViewBag.doanhthuthangs = doanhthuthangs;
            ViewBag.giamgiathangs = giamgiathangs;
            ViewBag.giavonthangs = giavonthangs;
            ViewBag.loinhuanthangs = loinhuanthangs;

            int tongdoanhthuthang =0,tonggiamgiathang=0, tonggiavongthang=0, tongloinhuanthang = 0;
            foreach(var i in doanhthuthangs)
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

            //ViewBag.ngay= System.DateTime.DaysInMonth(Int32.Parse(Year), Int32.Parse(Month));
            ViewBag.thang = Month;
            ViewBag.nam = Year;


            return View(dates);
        }
        public ActionResult QuanLyBaoCaoXuatPDF(string Acc, string Year, string Month)
        {

            int[] doanhthuthangs = new int[15];
            int[] giamgiathangs = new int[15];
            int[] giavonthangs = new int[15];
            int[] loinhuanthangs = new int[15];


            if (Month != null && Year != null)
            {
                var a = System.DateTime.DaysInMonth(2001, 1);
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
                            int temp1 = itemhd.TongThanhToan ?? 0;
                            int temp2 = itemhd.SoTienGiam ?? 0;
                            doanhthuthangs[i] = doanhthuthangs[i] + temp1;
                            giamgiathangs[i] = giamgiathangs[i] + temp2;

                            foreach (var itemcthd in itemhd.CTHoaDons)
                            {
                                int temp3 = (itemcthd.SanPham.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavonthangs[i] = giavonthangs[i] + temp3;
                            }

                            loinhuanthangs[i] = doanhthuthangs[i] - giavonthangs[i];
                        }
                    }
                }
            }
            ViewBag.doanhthuthangs = doanhthuthangs;
            ViewBag.giamgiathangs = giamgiathangs;
            ViewBag.giavonthangs = giavonthangs;
            ViewBag.loinhuanthangs = loinhuanthangs;

            int tongdoanhthuthang = 0, tonggiamgiathang = 0, tonggiavongthang = 0, tongloinhuanthang = 0;
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
    }
}