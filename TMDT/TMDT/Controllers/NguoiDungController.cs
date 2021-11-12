using System;
using System.Collections.Generic;
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
            var check = Login(username, password);
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
                    object[] sqlParams =
                    {
                        new SqlParameter("@ten",_user.Ten),
                        new SqlParameter("@username",_user.Username),
                        new SqlParameter("@password",_user.Password),
                        new SqlParameter("@diaChi",_user.DiaChi??""),
                        new SqlParameter("@sdt",_user.SDT??""),
                        new SqlParameter("@email",_user.Email??"")
                    };
                    var result = database.Database.ExecuteSqlCommand("USP_Register @ten, @username, @password, @diaChi, @sdt, @email", sqlParams);
                    if (result>0)
                    {
                        database.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "loi";
                        return View();
                    }
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

        public NguoiDung Login(string username, string password)
        {
            object[] sqlParams =
            {
                new SqlParameter("@username",username),
                new SqlParameter("@password",password),
            };
            var result = database.Database.SqlQuery<NguoiDung>("USP_Login @username, @password", sqlParams).FirstOrDefault();
            return result;
        }
    }
}