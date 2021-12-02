using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class GioHangController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HienThiGioHang()
        {
            return View();
        }
        public ActionResult GioHangRong()
        {
            return View();
        }
        public ActionResult CheckDangNhap()
        {
            return View();
        }

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public void AddToCart(int id, int quantity)
        {
            var _pro = database.SanPhams.SingleOrDefault(s => s.MaSanPham == id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro, quantity);
            }
        }
        public ActionResult Checkout()
        {
            HoaDon hoaDon = new HoaDon();
            return View(hoaDon);
        }
        [HttpPost]
        public ActionResult Checkout(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                NguoiDung nguoiDung = (NguoiDung)Session["Account"];
                Cart cart = Session["Cart"] as Cart;
                hoaDon.MaKhachHang = nguoiDung.MaNguoiDung;
                hoaDon.NgayMua = DateTime.Now;
                hoaDon.NgayTao = DateTime.Now;
                hoaDon.TinhTrang = 1;

                database.HoaDons.Add(hoaDon);

                foreach (var item in cart.Items)
                {
                    CTHoaDon cTHoaDon = new CTHoaDon();
                    cTHoaDon.MaHoaDon = hoaDon.MaHoaDon;
                    cTHoaDon.MaSanPham = item._sanPham.MaSanPham;
                    cTHoaDon.DonGia = (int)item._sanPham.GiaBan;
                    cTHoaDon.SoLuong = item._quantity;
                    cTHoaDon.ThanhTien = (int)(item._quantity * item._sanPham.GiaBan);
                    database.CTHoaDons.Add(cTHoaDon);

                    foreach (var sp in database.SanPhams.Where(s => s.MaSanPham == cTHoaDon.MaSanPham))
                    {
                        sp.SoLuongTon -= item._quantity;
                        sp.SoLuongBan += item._quantity;
                    }
                }

                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                return View();
            }

        }
    }
}