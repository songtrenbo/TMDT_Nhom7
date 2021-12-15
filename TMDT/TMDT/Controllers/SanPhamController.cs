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
        public ActionResult CT_SanPham(int MaSanPham)
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
        public PartialViewResult Rating(int maSP, int diem = 0)
        {
            var result = database.DanhGias.Where(s => s.MaSanPham == maSP && s.IsApproved == true && s.IsDeleted == false);
            if (diem != 0)
            {
                result = result.Where(s => s.Diem == diem);
            }
            return PartialView(result.ToList());
        }
    }
}