using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class TrangChuController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        public ActionResult Index()
        {
            ViewBag.thuonghieu = database.ThuongHieux.ToList();
            var sanpham = database.SanPhams.ToList();
            return View(sanpham);
        }

    }
}