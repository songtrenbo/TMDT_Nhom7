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