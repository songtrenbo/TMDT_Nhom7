using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class NguoiDungController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: DangNhap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(NguoiDung _user)
        {
            var check = database.NguoiDungs.Where(s => s.Username == _user.Username && s.Password == _user.Password).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("Index");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["NguoiDung"] = check.Ten;
                //Session["ten"] = check.Ten;
                Session["maQuyen"] = check.MaQuyen;
                Session["id"] = check.MaNguoiDung;
                ViewBag.Ten = check.Ten;
                if (check.MaQuyen == 2)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "TrangChu");
                }
            }
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "TrangChu");
        }
    }
}