using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class TrangChuController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        public ActionResult Index()
        {
            var danhmuc = database.DanhMucs.Where(x => x.IsShowHome == true).ToList().ToArray();
            ViewBag.danhmuc = danhmuc;
            var sp = database.SanPhams.Where(x=>x.IsHide==false).OrderByDescending(x => x.NgayTao).ToList();
            List<List<SanPham>> result = new List<List<SanPham>>();
            for (int i = 0; i < danhmuc.Length; i++)
            {
            result.Add(new List<SanPham>());
            }
            foreach (var item in sp)
            {
                for (int i = 0; i < danhmuc.Length; i++)
                {
                    if (item.MaDanhMuc == danhmuc[i].MaDanhMuc)
                    {
                        result[i].Add(item);
                        break;
                    }
                }
            }
            ViewBag.result = result;
            return View();
        }

        public ActionResult XemTatCaSP(int madanhmuc)
        {
            var sanpham = database.SanPhams.Where(s => s.MaDanhMuc == madanhmuc).FirstOrDefault();
            ViewBag.danhmuc = sanpham.DanhMuc.TenDanhMuc;
            var sanphams = database.SanPhams.Where(s => s.MaDanhMuc == madanhmuc).ToList();
            return View(sanphams);
        }
    }
}