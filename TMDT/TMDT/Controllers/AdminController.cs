using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
using TMDT.Utility;

namespace TMDT.Controllers
{
    public class AdminController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        #region TakKhoan
        public ActionResult QLTaiKhoan(string searchString, int Quyen = 0)
        {
            List<Quyen> quyen = database.Quyens.Where(s => s.MaQuyen != 1).ToList();

            ViewBag.Quyen = new SelectList(quyen, "MaQuyen", "TenQuyen");

            var taiKhoan = database.NguoiDungs.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                taiKhoan = taiKhoan.Where(s => s.Username.ToLower().Contains(searchString.ToLower())).ToList();
            }
            if (Quyen != 0)
            {
                taiKhoan = taiKhoan.Where(x => x.MaQuyen == Quyen).ToList();
            }
            return View(taiKhoan);
        }
        public ActionResult TaiKhoanCreate()
        {
            List<Quyen> quyen = database.Quyens.Where(s => s.MaQuyen != 1).ToList();

            ViewBag.Quyen = new SelectList(quyen, "MaQuyen", "TenQuyen");
            NguoiDung nguoiDung = new NguoiDung();

            return View(nguoiDung);
        }
        [HttpPost]
        public ActionResult TaiKhoanCreate(NguoiDung nguoiDung)
        {
            var check_ID = database.NguoiDungs.Where(s => s.Username == nguoiDung.Username).FirstOrDefault();

            if (check_ID == null)
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                nguoiDung.Password = Utils.Crypto("123");
                nguoiDung.ConfirmPass = Utils.Crypto("123"); 
                nguoiDung.NgayTao = DateTime.Now;
                nguoiDung.Status = 1;
                database.NguoiDungs.Add(nguoiDung);
                database.SaveChanges();
                return RedirectToAction("QLTaiKhoan");
            }
            else
            {
                ViewBag.Error = "Tài khoản đã tồn tại";
                return View();
            }
        }
        public ActionResult TaiKhoanDetail(int id)
        {
            return View(database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault());
        }
        [HttpPost]
        public void ResetPassword(string username)
        {
            var account = database.NguoiDungs.Where(s => s.Username == username).FirstOrDefault();
            account.Password = Utils.Crypto("123");
            account.ConfirmPass = Utils.Crypto("123"); ;
            database.Entry(account).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
        }
        #endregion

        #region PhanQuyen
        public ActionResult QLPhanQuyen()
        {
            return View(database.NguoiDungs.Where(x=>x.Status==1));
        }
        public ActionResult PhanQuyenDetail(int id)
        {
            var dsQuyen = database.Quyens.ToList();
            ViewBag.DsQuyen = new SelectList(dsQuyen, "maQuyen", "tenQuyen");
            return View(database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult PhanQuyenUpdate(NguoiDung nguoiDung)
        {
            nguoiDung.NgayChinhSua = DateTime.Now;
            try
            {
                var dsQuyen = database.Quyens.ToList();
                ViewBag.DsQuyen = new SelectList(dsQuyen, "maQuyen", "tenQuyen");
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QLPhanQuyen");
            }
            catch
            {
                return View();
            }
        }
        
        #endregion
    }
}