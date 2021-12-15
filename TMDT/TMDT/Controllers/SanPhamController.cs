using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class SanPhamController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: SanPham
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CT_SanPham(long MaSanPham)
        {
            var sanpham = database.SanPhams.Where(s => s.MaSanPham == MaSanPham).FirstOrDefault();
            return View(sanpham);
        }
        public JsonResult UpdateCart()
        {
            var sanPham = (from s in database.SanPhams.ToList()
                           select new SanPham
                           {
                               MaSanPham = s.MaSanPham,
                               GiaBan = s.GiaBan,
                               GiaGiam = s.GiaGiam
                           }).ToList();
            return Json(sanPham, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AllRating(int maSP)
        {
            var sanpham = database.DanhGias.Where(s => s.MaSanPham == maSP && s.IsApproved == true && s.IsDeleted == false).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult Rating_1(int maSP)
        {
            var sanpham = database.DanhGias.Where(s => s.MaSanPham == maSP && s.Diem == 1 && s.IsApproved == true && s.IsDeleted == false).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult Rating_2(int maSP)
        {
            var sanpham = database.DanhGias.Where(s => s.MaSanPham == maSP && s.Diem == 2 && s.IsApproved == true && s.IsDeleted == false).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult Rating_3(int maSP)
        {
            var sanpham = database.DanhGias.Where(s => s.MaSanPham == maSP && s.Diem == 3 && s.IsApproved == true && s.IsDeleted == false).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult Rating_4(int maSP)
        {
            var sanpham = database.DanhGias.Where(s => s.MaSanPham == maSP && s.Diem == 4 && s.IsApproved == true && s.IsDeleted == false).ToList();
            return PartialView(sanpham);
        }
        public PartialViewResult Rating_5(int maSP)
        {
            var sanpham = database.DanhGias.Where(s => s.MaSanPham == maSP && s.Diem == 5 && s.IsApproved == true && s.IsDeleted == false).ToList();
            return PartialView(sanpham);
        }
    }
}