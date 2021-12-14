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
        DBLaptopEntities database = new DBLaptopEntities();
        public ActionResult Index(string name)
        {
            //ViewBag.abc = database.ThuongHieux.Where(x=>x.MaThuongHieu == a).FirstOrDefault().TenThuongHieu;
            //return View();
            var result = database.SanPhams.Where(x => x.IsHide == false);
            if (!String.IsNullOrEmpty(name))
            {
            result = result.Where(x => x.TenSanPham.Contains(name));
            }
            return View(result.ToList());
        }
    }
}