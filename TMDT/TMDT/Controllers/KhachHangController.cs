using System;
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
            var voucher = database.PhieuQuaTangs.Where(s => s.Status == 2 && s.GiaTriDonHangToiThieu <= acc.HoaDons.Count).ToList();
            List<object> thuonghieus = new List<object>();
            foreach(var item in voucher)
            {
                //lấy thương hiệu trùng với loại phạm vi của voucher
                var thuonghieu = database.ThuongHieux.Where(s => s.MaThuongHieu == item.LoaiPhamVi).FirstOrDefault();
                if (thuonghieu != null)
                {
                    thuonghieus.Add(thuonghieu);
                }
            }
            ViewBag.ThuongHieu = thuonghieus.ToList();


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
    }
}