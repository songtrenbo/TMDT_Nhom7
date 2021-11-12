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
            var check = Login(_user.Username, _user.Password);
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
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "TrangChu");
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