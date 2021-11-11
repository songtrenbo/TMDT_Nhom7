using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class HomeController : Controller
    {
        DBLaptopEntities1 database = new DBLaptopEntities1();
        public ActionResult Index()
        {
            ViewBag.thuonghieu = database.ThuongHieux.ToList();
            var sanpham = database.SanPhams.ToList();
            return View(sanpham);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}