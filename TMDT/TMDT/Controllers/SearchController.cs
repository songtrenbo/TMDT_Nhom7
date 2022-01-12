using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        DBLaptopEntities database = new DBLaptopEntities();
        public ActionResult Index(string name, int? page, int danhMucID = 0, int cpuID = 0, int loaiGPUID = 0, int sizeManHinhID = 0, int thuongHieuID = 0)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);


            var danhmuc = database.DanhMucs.ToList();
            var cpu = database.CPUs.ToList();
            var loaiGPU = database.LoaiGPUs.ToList();
            var sizeManHinh = database.SizeManHinhs.ToList();
            var thuongHieu = database.ThuongHieux.ToList();
            ViewBag.danhmuc = new SelectList(danhmuc, "MaDanhMuc", "TenDanhMuc"); ;
            ViewBag.cpu = new SelectList(cpu, "MaCPU", "TenCPU"); ;
            ViewBag.loaiGPU = new SelectList(loaiGPU, "MaLoaiGPU", "TenLoaiGPU"); ;
            ViewBag.sizeManHinh = new SelectList(sizeManHinh, "MaSizeManHinh", "Size"); ;
            ViewBag.thuongHieu = new SelectList(thuongHieu, "MaThuongHieu", "TenThuongHieu", thuongHieuID); ;
            var listSP = database.SanPhams.Where(x => x.IsHide == false).OrderByDescending(x => x.NgayTao).ToList();
            var result = new List<SanPham>(listSP);
            if (!String.IsNullOrEmpty(name))
            {
                result = result.Where(s => s.TenSanPham.ToLower().Contains(name.ToLower())).ToList();
                ViewBag.name = name;
            }
            if (danhMucID != 0)
            {
                result = result.Where(s => s.DanhMuc.MaDanhMuc == danhMucID).ToList();
                ViewBag.danhMucID = danhMucID;
            }
            if (cpuID != 0)
            {
                result = result.Where(s => s.CPU.MaCPU == cpuID).ToList();
                ViewBag.cpuID = cpuID;
            }
            if (loaiGPUID != 0)
            {
                result = result.Where(s => s.LoaiGPU.MaLoaiGPU == loaiGPUID).ToList();
                ViewBag.loaiGPUID = loaiGPUID;
            }
            if (sizeManHinhID != 0)
            {
                result = result.Where(s => s.SizeManHinh1.MaSizeManHinh == sizeManHinhID).ToList();
                ViewBag.sizeManHinhID = sizeManHinhID;
            }
            if (thuongHieuID != 0)
            {
                result = result.Where(s => s.ThuongHieu.MaThuongHieu == thuongHieuID).ToList();
                ViewBag.thuongHieuID = thuongHieuID;
            }
            ViewBag.result = result.Count();
            return View(result.ToPagedList(pageNum, pageSize));
        }
    }
}