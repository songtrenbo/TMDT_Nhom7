using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class QuanLyController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        // GET: QuanLy
        #region ThuongHieu
        public ActionResult QuanLyThuongHieu()
        {
            var thuongHieu = database.ThuongHieux.ToList();
            thuongHieu.OrderBy(s => s.TenThuongHieu);
            return View(thuongHieu);
        }
        public ActionResult ThuongHieuCreate()
        {
            ThuongHieu thuongHieu = new ThuongHieu();
            return View(thuongHieu);
        }
        [HttpPost]
        public ActionResult ThuongHieuCreate(ThuongHieu thuongHieu)
        {
            var check = database.ThuongHieux.Where(s => s.TenThuongHieu == thuongHieu.TenThuongHieu).FirstOrDefault();
            if(check == null)
            {
                thuongHieu.SoLuong = 0;
                thuongHieu.NgayTao = DateTime.Now;
                thuongHieu.NgayChinhSua = null;
                thuongHieu.IsDeleted = false;
                thuongHieu.IsLockEdit = false;

                try
                {
                    if (thuongHieu.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(thuongHieu.UploadImage.FileName);
                        string extent = Path.GetExtension(thuongHieu.UploadImage.FileName);
                        filename = filename + extent;
                        thuongHieu.Hinh = "/Content/ThuongHieuImages/" + filename;
                        thuongHieu.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/ThuongHieuImages/"), filename));
                    }

                    database.ThuongHieux.Add(thuongHieu);
                    database.SaveChanges();
                    return RedirectToAction("QuanLyThuongHieu");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ViewBag.IsExist = "Thương hiệu đã tồn tại!";
                
                return View(thuongHieu);
            }
        }
        public ActionResult ThuongHieuDetails(int id)
        {
            return View(database.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault());
        }
        public ActionResult ThuongHieuEdit(int id)
        {
            return View(database.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult ThuongHieuEdit(int id, ThuongHieu thuongHieu)
        {
            thuongHieu.NgayChinhSua = DateTime.Now;
            thuongHieu.NgayTao = database.ThuongHieux.Where(s => s.MaThuongHieu == id).Select(s => s.NgayTao).FirstOrDefault();
            try
            {
                if (thuongHieu.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(thuongHieu.UploadImage.FileName);
                    string extent = Path.GetExtension(thuongHieu.UploadImage.FileName);
                    filename = filename + extent;
                    thuongHieu.Hinh = "/Content/ThuongHieuImages/" + filename;
                    thuongHieu.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/ThuongHieuImages/"), filename));
                }
                else
                {
                    string img = database.ThuongHieux.Where(s => s.MaThuongHieu == id).Select(s => s.Hinh).FirstOrDefault();
                    thuongHieu.Hinh = img;
                }
                database.Entry(thuongHieu).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyThuongHieu");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ThuongHieuDelete(int id)
        {
            var thuongHieu = database.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault();
            var LaptopHasThuongHieu = database.SanPhams.Where(s => s.MaThuongHieu == id).ToList();
            foreach (var item in LaptopHasThuongHieu)
            {
                database.SanPhams.Remove(item);
            }
            database.ThuongHieux.Remove(thuongHieu);
            database.SaveChanges();
            return RedirectToAction("QuanLyThuongHieu");
        }
        #endregion
        public ActionResult QuanLySanPham()
        {
            
            return View();
        }
        public ActionResult QuanLyNhapKho()
        {
            return View();
        }
        public ActionResult QuanLyNhanVien()
        {
            return View();
        }
        public ActionResult QuanLyVoucher()
        {
            return View();
        }
        public ActionResult QuanLyBaoCao()
        {
            return View();
        }
    }
}