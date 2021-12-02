using System;
using System.Collections.Generic;
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
            List<Quyen> quyen = database.Quyens.ToList();

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
            List<Quyen> quyen = database.Quyens.Where(s=>s.MaQuyen!=1).ToList();

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
                nguoiDung.ConfirmPass = Utils.Crypto("123"); ;
                nguoiDung.DiemThuong = 0;
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

        #region TaiKhoan
        public ActionResult QLQuyen()
        {
            return View();
        }
        //public ActionResult QuyenCreate()
        //{
        //    return View(new Quyen());
        //}
        //[HttpPost]
        //public ActionResult QuyenCreate(Quyen quyen)
        //{
        //    var check = database.Quyens.Where(s => s.TenQuyen.ToLower().Equals(quyen.TenQuyen.ToLower())).FirstOrDefault();
        //    if (ModelState.IsValid)
        //    {
        //        if(check == null)
        //        {
        //            database.Configuration.ValidateOnSaveEnabled = false;
        //            quyen.NgayTao = DateTime.Now;
        //            quyen.IsDeleted = false;
        //            quyen.IsLockEdit = false;
        //            database.Quyens.Add(quyen);
        //            database.SaveChanges();
        //            return RedirectToAction("QLQuyen");
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Quyền này đã tồn tại";
        //            return View("QuyenCreate");
        //        }
        //    }
        //    if (check != null)
        //    {
        //        ViewBag.Error = "Quyền này đã tồn tại";
        //        return View("QuyenCreate");
        //    }
        //    return View("QuyenCreate");
        //}
        //public ActionResult QuyenDetail(int id)
        //{
        //    return View(database.Quyens.Where(s => s.MaQuyen == id).FirstOrDefault());
        //}
        //[HttpPost]
        //public ActionResult QuyenDetail(Quyen quyen, string submitButton)
        //{
        //    switch (submitButton)
        //    {
        //        case "Sửa":
        //            return (QuyenEdit(quyen.MaQuyen,quyen));
        //        case "Xóa":
        //            return (QuyenDelete(quyen.MaQuyen));
        //        default:
        //            return RedirectToAction("QLQuyen");
        //    }
        //}
        //[HttpPost]
        //public ActionResult QuyenEdit(int id, Quyen quyen)
        //{
        //    //quyen.NgayChinhSua = DateTime.Now;
        //    try
        //    {
        //        quyen.NgayChinhSua = DateTime.Now;
        //        database.Entry(quyen).State = System.Data.Entity.EntityState.Modified;
        //        database.SaveChanges();
        //        return RedirectToAction("QLQuyen");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //[HttpPost]
        //public ActionResult QuyenDelete(int id)
        //{
        //    var quyen = database.Quyens.Where(s => s.MaQuyen == id).FirstOrDefault();
        //    database.Quyens.Remove(quyen);
        //    database.SaveChanges();
        //    return RedirectToAction("QLQuyen");
        //}
        #endregion
    }
}