using System;
using System.Collections.Generic;
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
        public ActionResult QuanLyThuongHieu()
        {
            return View();
        }
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
        public ActionResult QuanLyBaoCao()
        {
            return View();
        }
    }
}