using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
using TMDT.Utility;

namespace TMDT.Controllers
{
    public class NguoiDungController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(NguoiDung _user)
        {
            if (String.IsNullOrEmpty(_user.Username) || String.IsNullOrEmpty(_user.Password))
                return View("DangNhap");
            string username = _user.Username;
            string password = Utils.Crypto(_user.Password);

            var checkUser = database.NguoiDungs.Where(s => s.Username.Equals(username) && s.Password.Equals(password)).FirstOrDefault();

            if (checkUser == null)
            {
                ViewBag.ErrorInfo = "Sai tên tài khoản hoặc mật khẩu";
                return View("DangNhap");
            }
            switch (checkUser.Status)
            {
                case 2:
                    ViewBag.ErrorInfo = "Tài khoản bị khóa";
                    return View("DangNhap");
                case 3:
                    ViewBag.ErrorInfo = "Tài khoản đã bị xóa";
                    return View("DangNhap");
                default:
                    break;
            }
            database.Configuration.ValidateOnSaveEnabled = false;
            Session["Account"] = checkUser;
            //ViewBag.Ten = check.Ten;
            switch (checkUser.MaQuyen)
            {
                case 1:
                    return RedirectToAction("QLQuyen", "Admin");
                case 2:
                    return RedirectToAction("QuanLyThuongHieu", "QuanLy");
                case 3:
                    return RedirectToAction("Index", "NhanVien");
                default:
                    return RedirectToAction("Index", "TrangChu");
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
                nguoiDung.NgayChinhSua = DateTime.Now;
                //nguoiDung.ConfirmPass = nguoiDung.Password;
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("ThongTinCaNhan", new { id = nguoiDung.MaNguoiDung });
            }
            catch (DbEntityValidationException e)
            {
                string a = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    a += ("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State) + "\n";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        a += ("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage) + "\n";
                    }
                }
                //throw;
                return Content(a);
            }
            //return RedirectToAction("ThongTinCaNhan", new { id = nguoiDung.MaNguoiDung });
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
                    _user.Password = Utils.Crypto(_user.Password);
                    _user.ConfirmPass = _user.Password;
                    _user.DiemThuong = 0;
                    _user.MaQuyen = 4;
                    _user.NgayTao = DateTime.Now;
                    _user.Status = 1;
                    database.NguoiDungs.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("DangNhap");
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