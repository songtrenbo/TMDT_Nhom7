using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
        #region TinTuc
        public ActionResult QLTinTuc()
        {
            var tintuc = database.TinTucs.ToList();
            return View(tintuc);
        }
        public ActionResult TinTucCreate()
        {
            List<SanPham> sanphams = database.SanPhams.ToList();
            ViewBag.SanPham = new SelectList(sanphams, "MaSanPham", "TenSanPham");
            TinTuc tintuc = new TinTuc();
            return View(tintuc);
        }
        [HttpPost]
        public ActionResult TinTucCreate(TinTuc tintuc)
        {
            var acc = (NguoiDung)Session["Account"];
            var check_ID = database.TinTucs.Where(s => s.MaTinTuc == tintuc.MaTinTuc).FirstOrDefault();
            try
            {
                if (tintuc.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(tintuc.UploadImage.FileName);
                    string extent = Path.GetExtension(tintuc.UploadImage.FileName);
                    filename = filename + extent;
                    tintuc.Thumbnail = "/Content/images/" + filename;
                    tintuc.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/images/"), filename));
                }
                tintuc.NgayTao = DateTime.Now;
                tintuc.NgayChinhSua = DateTime.Now;
                tintuc.MaNguoiSua = acc.MaNguoiDung;
                tintuc.MaNguoiTao = acc.MaNguoiDung;
                tintuc.IsDeleted = false;
                tintuc.IsHide = false;
                database.Configuration.ValidateOnSaveEnabled = false;
                database.TinTucs.Add(tintuc);
                database.SaveChanges();
                return RedirectToAction("QLTinTuc"); 
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
                return Content(a);
            }
        }

        public ActionResult TinTucDetail(int id)
        {
            var tintuc = database.TinTucs.Where(s => s.MaTinTuc == id).FirstOrDefault();
            return View(tintuc);
        }
        public ActionResult TinTucEdit(int id)
        {
            List<SanPham> sanphams = database.SanPhams.ToList();
            ViewBag.SanPham = new SelectList(sanphams, "MaSanPham", "TenSanPham");
            return View(database.TinTucs.Where(s=>s.MaTinTuc==id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult TinTucEdit(int id, TinTuc tintuc)
        {
            var acc = (NguoiDung)Session["Account"];
            //var check_ID = database.TinTucs.Where(s => s.MaTinTuc == tintuc.MaTinTuc).FirstOrDefault();
            try
            {
                if (tintuc.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(tintuc.UploadImage.FileName);
                    string extent = Path.GetExtension(tintuc.UploadImage.FileName);
                    filename = filename + extent;
                    tintuc.Thumbnail = "/Content/images/" + filename;
                    tintuc.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/images/"), filename));
                }
                else
                {
                    string img = database.TinTucs.Where(s => s.MaTinTuc == id).Select(s => s.Thumbnail).FirstOrDefault();
                    tintuc.Thumbnail = img;
                }
                tintuc.NgayChinhSua = DateTime.Now;
                tintuc.MaNguoiSua = acc.MaNguoiDung;
                //tintuc.IsDeleted = false;
                //tintuc.IsHide = false;
                database.Entry(tintuc).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QLTinTuc");
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
                return Content(a);
            }
            catch (InvalidOperationException e)
            {
                return Content(e.Data.Values.ToString());
            }
        }
        public ActionResult TinTucDelete(int id)
        {
            var tintuc = database.TinTucs.Where(s => s.MaTinTuc == id).FirstOrDefault();
            database.TinTucs.Remove(tintuc);
            database.SaveChanges();
            return RedirectToAction("QLTinTuc");
        }
        #endregion
    }
}