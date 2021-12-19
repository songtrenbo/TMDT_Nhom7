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
        public PartialViewResult QuanLyDonHang(int tinhTrang = 0)
        {
            List<HoaDon> donHang = null;
            if (tinhTrang == 0)
            {
                donHang = database.HoaDons.ToList();
            }
            else
            {
                donHang = database.HoaDons.Where(s => s.TinhTrang == tinhTrang).ToList();
            }
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
        public ActionResult XacNhanDonHang(int maDonHang, string urlstr)
        {
            NguoiDung nguoiDung = (NguoiDung)Session["Account"];
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 2;
            donHang.MaNVDuyet = nguoiDung.MaNguoiDung;
            database.Entry(donHang).State = EntityState.Modified;

            database.SaveChanges();
            SendEmail(maDonHang, urlstr,1);
            return RedirectToAction("Index", "NhanVien");
        }
        public ActionResult SanSangGiaoHang(int maDonHang,string urlstr)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 3;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            SendEmail(maDonHang, urlstr, 2);
            return RedirectToAction("Index", "NhanVien");
        }
        public ActionResult XacNhanDaGiao(int maDonHang, string urlstr)
        {
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 4;
            donHang.NgayNhan = DateTime.Now;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            SendEmail(maDonHang, urlstr, 3);
            return RedirectToAction("Index", "NhanVien");
        }
        public ActionResult HuyDonHang(int maDonHang, string reason, string urlstr)
        {
            NguoiDung nguoiDung = Session["Account"] as NguoiDung;
            var donHang = database.HoaDons.Where(s => s.MaHoaDon == maDonHang).FirstOrDefault();
            donHang.TinhTrang = 5;
            donHang.LyDoHuy = reason;
            donHang.NguoiHuyDon = nguoiDung.MaNguoiDung;
            donHang.NgayHuyDon = DateTime.Now;
            database.Entry(donHang).State = EntityState.Modified;
            database.SaveChanges();
            SendEmail(maDonHang, urlstr, 4);
            return RedirectToAction("Index", "NhanVien");
        }
        public JsonResult GetReason(int maDonHang)
        {
            var donHang = (from d in database.HoaDons.ToList()
                           where d.MaHoaDon == maDonHang
                           select new HoaDon
                           {
                               NgayHuyDon = d.NgayHuyDon,
                               NguoiHuyDon = d.NguoiHuyDon,
                               LyDoHuy = d.LyDoHuy
                           }).FirstOrDefault();
            return Json(donHang, JsonRequestBehavior.AllowGet);
        }
        public void SendEmail(int maHoaDon, string urlstr, int tinhTrang)
        {
            string content = "", subject = "";
            HoaDon hoaDon = database.HoaDons.Where(s => s.MaHoaDon == maHoaDon).FirstOrDefault();
            var donHangCT = database.CTHoaDons.Where(s => s.MaHoaDon == maHoaDon).ToList();
            string body = "";

            foreach (var item in donHangCT)
            {
                body += $@"<tr>
                        <td>
                            <img src=""{item.SanPham.Hinh}"" width=""80"" />
                        </td>
                        <td>
                            <a href=""/SanPham/CT_SanPham?MaSanPham={item.MaSanPham}"">{item.SanPham.TenSanPham}</a>
                        </td>
                        <td>
                            {item.DonGia}
                        </td>
                        <td style=""text-align:center;"">
                            {item.SoLuong}
                        </td>
                        <td>
                            {item.ThanhTien}
                        </td>
                    </tr>";
            }
            switch (tinhTrang)
            {
                case 1:
                    content = System.IO.File.ReadAllText(Server.MapPath("~/Template/Accept.html"));
                    subject = "Xác nhận đơn hàng";
                    break;
                case 2:
                    content = System.IO.File.ReadAllText(Server.MapPath("~/Template/Ready.html"));
                    subject = "Đơn hàng đã bắt đầu giao";
                    break;
                case 3:
                    content = System.IO.File.ReadAllText(Server.MapPath("~/Template/Delivered.html"));
                    subject = "Đơn hàng đã giao";
                    break;
                case 4:
                    content = System.IO.File.ReadAllText(Server.MapPath("~/Template/Cancel.html"));
                    subject = "Đơn hàng đã bị hủy";
                    content = content.Replace("{{Reason}}", hoaDon.LyDoHuy);
                    break;
                default: break;
            }
            urlstr = "https://" + urlstr + "/KhachHang/DonHangChiTiet?maDonHang=" + hoaDon.MaHoaDon;
            content = content.Replace("{{Link}}", urlstr);
            content = content.Replace("{{MaHoaDon}}", maHoaDon.ToString());
            content = content.Replace("{{Name}}", hoaDon.TenKhach);
            content = content.Replace("{{NgayMua}}", hoaDon.NgayMua.ToString());
            content = content.Replace("{{TongTien}}", (hoaDon.TongThanhToan - hoaDon.SoTienGiam - hoaDon.PhiGiaoHang).ToString());
            content = content.Replace("{{PhiVanChuyen}}", hoaDon.PhiGiaoHang.ToString());
            content = content.Replace("{{GiamGia}}", hoaDon.SoTienGiam.ToString());
            content = content.Replace("{{ThanhTien}}", hoaDon.TongThanhToan.ToString());
            content = content.Replace("{{Tbody}}", body);
            if (hoaDon.HinhThucThanhToan == 1)
            {
                content = content.Replace("{{GiaoHang}}", "Thanh toán khi nhận hàng");
            }
            else
            {
                content = content.Replace("{{GiaoHang}}", "Thanh toán Online");
            }
            new MailHelper().SendMail(hoaDon.NguoiDung.Email, subject, content);
        }
    }
}