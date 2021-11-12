using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class GioHangController : Controller
    {
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
    }
}