using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
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
            string username = _user.Username;
            string password = _user.Password;
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                ViewBag.ErrorInfo = "Sai tên tài khoản hoặc mật khẩu";
                return View("Index");
            }
            var check = database.NguoiDungs.Where(s => s.Username.Equals(_user.Username) && s.Password.Equals(_user.Password) &&s.Status==true).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai tên tài khoản hoặc mật khẩu";
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
                switch (check.MaQuyen)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        return RedirectToAction("Index", "QuanLy");
                    case 3:
                        return RedirectToAction("Index", "NhanVien");
                    default:
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
        public ActionResult ThongTinCaNhan(int id)
        {
            return View(database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult CapNhatThongTinCaNhan(NguoiDung nguoiDung)
        {
            //if(ModelState.IsValid)
            try
            {
                //var nguoiDung1 = database.NguoiDungs.Where(s => s.MaNguoiDung == nguoiDung.MaNguoiDung).FirstOrDefault();
                //nguoiDung1 = nguoiDung;
                nguoiDung.NgayChinhSua = DateTime.Now;
                nguoiDung.ConfirmPass = nguoiDung.Password;
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("ThongTinCaNhan", new { id = nguoiDung.MaNguoiDung });
            }
            catch (DbEntityValidationException e)
            {
                string a = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    a+=("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State)+"\n";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        a+=("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage) + "\n";
                    }
                }
                //throw;
                return Content(a);
            }
            return RedirectToAction("ThongTinCaNhan", new { id = nguoiDung.MaNguoiDung });
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
                    _user.DiemThuong = 0;
                    _user.MaQuyen = 4;
                    _user.NgayTao = DateTime.Now;
                    _user.Status = true;
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