using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        DBLaptopEntities db = new DBLaptopEntities();
        public ActionResult Index(int a)
        {
            ViewBag.abc = db.ThuongHieux.Where(x=>x.MaThuongHieu == a).FirstOrDefault().TenThuongHieu;
            return View();
        }
    }
}