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
        DBLaptopEntities1 database = new DBLaptopEntities1();
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
    }
}