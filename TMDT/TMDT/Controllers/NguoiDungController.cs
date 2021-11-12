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
        DBLaptopEntities1 database = new DBLaptopEntities1();
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
        public ActionResult DangXuat()
        {
            Session.Abandon();
            return RedirectToAction("Index", "TrangChu");
        }

        public ActionResult DangKy()
        {
            NguoiDung nguoidung = new NguoiDung();
            return View(nguoidung);
        }

        [HttpPost]
        public ActionResult DangKy(NguoiDung _user)
        {
            var check_ID = database.NguoiDungs.Where(s => s.Username == _user.Username).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (check_ID == null)
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    _user.MaQuyen = 4;
                    _user.NgayTao = DateTime.Now;
                    database.NguoiDungs.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Tài khoản đã tồn tại";
                    return View();
                }
            }
            if (check_ID != null)
            {
                ViewBag.Error = "Tài khoản đã tồn tại";
                return View();
            }
            return View();
        }
    }
}