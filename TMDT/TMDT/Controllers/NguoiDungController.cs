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
        public ActionResult DangNhap(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(NguoiDung _user, string returnUrl)
        {
            if (String.IsNullOrEmpty(_user.Username) || String.IsNullOrEmpty(_user.Password))
                return View("DangNhap");
            string username = _user.Username;
            string password = Utils.Crypto(_user.Password);

            var checkUser = database.NguoiDungs.Where(s => s.Username.Equals(username)).FirstOrDefault();

            if (checkUser == null)
            {
                ViewBag.ErrorInfo = "Sai tên tài khoản hoặc mật khẩu";
                return View("DangNhap");
            }
            if (checkUser.Password.Equals(password))
            {
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
                Session["TenAcc"] = checkUser.Ten;
                //ViewBag.Ten = check.Ten;
                if (returnUrl == null && checkUser.MaQuyen == 4 || checkUser.MaQuyen == 1 || checkUser.MaQuyen == 2 || checkUser.MaQuyen == 3)
                {
                    switch (checkUser.MaQuyen)
                    {
                        case 1:
                            return RedirectToAction("QLTaiKhoan", "Admin");
                        case 2:
                            return RedirectToAction("QuanLyThuongHieu", "QuanLy");
                        case 3:
                            return RedirectToAction("Index", "NhanVien");
                        default:
                            return RedirectToAction("Index", "TrangChu");
                    }
                }
                else
                    return Redirect(returnUrl);
            }
            return View("DangNhap");


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
        public ActionResult ThongTinCaNhan()
        {
            NguoiDung nguoiDung = (NguoiDung)Session["Account"];
            return View(database.NguoiDungs.Where(s => s.MaNguoiDung == nguoiDung.MaNguoiDung).FirstOrDefault());
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
                Session["Account"] = nguoiDung;
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
        public ActionResult DangKy(NguoiDung _user, string urlstr)
        {
            var check_ID = database.NguoiDungs.Where(s => s.Username == _user.Username).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (check_ID == null)
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    _user.Password = Utils.Crypto(_user.Password);
                    _user.ConfirmPass = _user.Password;
                    _user.MaQuyen = 4;
                    _user.NgayTao = DateTime.Now;
                    _user.Status = 2;
                    database.NguoiDungs.Add(_user);
                    database.SaveChanges();
                    SendEmail(_user.MaNguoiDung, urlstr, 0);
                    return RedirectToAction("SendEmailActive");
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
        public ActionResult ActiveAcc(int maNguoiDung)
        {
            var check_ID = database.NguoiDungs.Where(s => s.MaNguoiDung == maNguoiDung).FirstOrDefault();
            check_ID.ConfirmPass = check_ID.Password;
            if (check_ID != null)
            {
                check_ID.Status = 1;
                database.Entry(check_ID).State = EntityState.Modified;
                database.SaveChanges();

                return View();
            }
            return View();
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string email, string urlstr)
        {
            NguoiDung check = database.NguoiDungs.Where(s => s.Email == email).FirstOrDefault();

            if (check != null)
            {
                SendEmail(check.MaNguoiDung, urlstr, 1);
                return View("SendEmailDone");
            }
            else
            {
                ViewBag.Email = "Email không tồn tại!";
                return View();
            }
        }
        public ActionResult ResetPassword(int maNguoiDung)
        {
            var check = database.NguoiDungs.AsNoTracking().Where(u => u.MaNguoiDung == maNguoiDung).FirstOrDefault();
            ViewBag.maNguoiDung = maNguoiDung;
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(int maNguoiDung, string password)
        {
            var user = database.NguoiDungs.Where(s => s.MaNguoiDung == maNguoiDung).FirstOrDefault();
            user.Password = Utils.Crypto(password);
            user.ConfirmPass = Utils.Crypto(password);
            database.Entry(user).State = EntityState.Modified;
            database.SaveChanges();

            return RedirectToAction("DangNhap");
        }
        public ActionResult SendEmailDone()
        {
            return View();
        }
        public ActionResult SendEmailActive()
        {
            return View();
        }
        public void SendEmail(int maNguoiDung, string urlstr, int i = 0)
        {
            NguoiDung nguoiDung = database.NguoiDungs.Where(s => s.MaNguoiDung == maNguoiDung).FirstOrDefault();
            string content, subject;
            if (i == 0)
            {
                content = System.IO.File.ReadAllText(Server.MapPath("~/Template/Register.html"));
                urlstr = "https://" + urlstr + "/NguoiDung/ActiveAcc?maNguoiDung=" + nguoiDung.MaNguoiDung;
                subject = "Kích hoạt tài khoản";
            }
            else
            {
                content = System.IO.File.ReadAllText(Server.MapPath("~/Template/ForgotPassword.html"));
                urlstr = "https://" + urlstr + "/NguoiDung/ResetPassword?maNguoiDung=" + nguoiDung.MaNguoiDung;
                subject = "Khôi phục mật khẩu";
            }

            content = content.Replace("{{Link}}", urlstr);
            content = content.Replace("{{Name}}", nguoiDung.Ten);

            new MailHelper().SendMail(nguoiDung.Email, subject, content);
        }
    }
}