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
            if (check == null)
            {
                thuongHieu.NgayTao = DateTime.Now;
                thuongHieu.NgayChinhSua = null;
                thuongHieu.IsDeleted = false;

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

        #region ViewBag
        public void SetViewBagDanhMuc(int? selectedID = null)
        {
            List<DanhMuc> danhMucs = database.DanhMucs.ToList();
            ViewBag.DanhMuc = new SelectList(danhMucs, "MaDanhMuc", "TenDanhMuc", selectedID);
        }
        public void SetViewBagThuongHieu(int? selectedID = null)
        {
            List<ThuongHieu> thuongHieus = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuongHieus, "MaThuongHieu", "TenThuongHieu", selectedID);
        }
        public void SetViewBagCPU(int? selectedID = null)
        {
            List<CPU> cPUs = database.CPUs.ToList();
            ViewBag.CPU = new SelectList(cPUs, "MaCPU", "TenCPU", selectedID);
        }
        public void SetViewBagLoaiGPU(int? selectedID = null)
        {
            List<LoaiGPU> loaiGPUs = database.LoaiGPUs.ToList();
            ViewBag.LoaiGPU = new SelectList(loaiGPUs, "MaLoaiGPU", "TenLoaiGPU", selectedID);
        }
        public void SetViewBagSizeManHinh(int? selectedID = null)
        {
            List<SizeManHinh> sizeManHinhs = database.SizeManHinhs.ToList();
            ViewBag.SizeManHinh = new SelectList(sizeManHinhs, "MaSizeManHinh", "Size", selectedID);
        }
        #endregion

        #region QLSanPham
        public ActionResult QuanLySanPham()
        {
            var result = database.SanPhams.OrderByDescending(x => x.IsHide).ToList();
            result = result.OrderByDescending(x => x.NgayTao).ToList();
            return View(database.SanPhams.ToList());
        }

        public ActionResult SanPhamCreate()
        {
            SetViewBagDanhMuc();
            SetViewBagThuongHieu();
            SetViewBagCPU();
            SetViewBagLoaiGPU();
            SetViewBagSizeManHinh();
            return View(new SanPham());
        }
        [HttpPost]
        public ActionResult SanPhamCreate(SanPham sanPham)
        {
            SetViewBagDanhMuc(sanPham.MaDanhMuc);
            SetViewBagThuongHieu(sanPham.MaThuongHieu);
            SetViewBagCPU(sanPham.MaLoaiCPU);
            SetViewBagLoaiGPU(sanPham.MaLoaiGPU);
            SetViewBagSizeManHinh(sanPham.SizeManHinh);
            sanPham.SeoTitle = Utils.ToUnsignString(sanPham.TenSanPham);
            var check = database.SanPhams.Where(s => s.TenSanPham == sanPham.TenSanPham).FirstOrDefault();
            if (check == null)
            {
                sanPham.SoLuongTon = 0;
                sanPham.SoLuongBan = 0;
                sanPham.DiemRate = 0;
                sanPham.SoLuotRate = 0;
                sanPham.LuotXem = 0;
                sanPham.NgayTao = DateTime.Now;
                sanPham.IsHide = false;

                try
                {
                    if (sanPham.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(sanPham.UploadImage.FileName);
                        string extent = Path.GetExtension(sanPham.UploadImage.FileName);
                        filename = filename + extent;
                        sanPham.Hinh = "/Content/images/" + filename;
                        sanPham.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/images/"), filename));
                    }

                    database.SanPhams.Add(sanPham);
                    database.SaveChanges();
                    return RedirectToAction("QuanLySanPham");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ViewBag.IsExist = "Sản phẩm đã tồn tại!";

                return View(sanPham);
            }
        }
        public ActionResult SanPhamDetails(int id)
        {
            return View(database.SanPhams.Where(s => s.MaSanPham == id).FirstOrDefault());
        }
        public ActionResult SanPhamEdit(int id)
        {
            SetViewBagDanhMuc();
            SetViewBagThuongHieu();
            SetViewBagCPU();
            SetViewBagLoaiGPU();
            SetViewBagSizeManHinh();
            return View(database.SanPhams.Where(s => s.MaSanPham == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult SanPhamEdit(int id, SanPham sanPham)
        {
            SetViewBagDanhMuc(sanPham.MaDanhMuc);
            SetViewBagThuongHieu(sanPham.MaThuongHieu);
            SetViewBagCPU(sanPham.MaLoaiCPU);
            SetViewBagLoaiGPU(sanPham.MaLoaiGPU);
            SetViewBagSizeManHinh(sanPham.SizeManHinh);
            sanPham.NgayChinhSua = DateTime.Now;
            sanPham.SeoTitle = Utils.ToUnsignString(sanPham.TenSanPham);
            var check = database.SanPhams.Where(s => s.TenSanPham == sanPham.TenSanPham).FirstOrDefault();
            if (check != null)
            {
                ViewBag.IsExist = "Sản phẩm đã tồn tại!";
                return View();
            }
            try
            {
                sanPham.NgayChinhSua = DateTime.Now;
                if (sanPham.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(sanPham.UploadImage.FileName);
                    string extent = Path.GetExtension(sanPham.UploadImage.FileName);
                    filename = filename + extent;
                    sanPham.Hinh = "/Content/images/" + filename;
                    sanPham.UploadImage.SaveAs(Path.Combine(Server.MapPath("/Content/images/"), filename));
                }
                else
                {
                    string img = database.SanPhams.Where(s => s.MaSanPham == id).Select(s => s.Hinh).FirstOrDefault();
                    sanPham.Hinh = img;
                }
                database.Entry(sanPham).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLySanPham");
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
        }
        public ActionResult SanPhamHide(int id)
        {
            var sanPham = database.SanPhams.Where(x => x.MaSanPham == id).FirstOrDefault();
            sanPham.NgayChinhSua = DateTime.Now;
            sanPham.IsHide = !sanPham.IsHide;
            try
            {
                database.Entry(sanPham).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLySanPham");
            }
            catch
            {
                return View();
            }
        }
        #endregion



        #region QLNhanVien
        public ActionResult QuanLyNhanVien()
        {
            var nhanviens = database.NguoiDungs.Where(s => s.MaQuyen == 3 && s.Status != 3).ToList().OrderBy(s => s.MaQuyen);
            return View(nhanviens);
        }
        public ActionResult NhanVienCreate()
        {
            return View(new NguoiDung());
        }
        [HttpPost]
        public ActionResult NhanVienCreate(NguoiDung nguoiDung)
        {
            var check = database.NguoiDungs.Where(s => s.Username == nguoiDung.Username).FirstOrDefault();
            if (check == null)
            {
                nguoiDung.NgayTao = DateTime.Now;
                var password = Utils.Crypto(nguoiDung.Username);
                nguoiDung.Password = password;
                nguoiDung.ConfirmPass = password;
                nguoiDung.Status = 1;
                nguoiDung.MaQuyen = 3;

                try
                {
                    database.NguoiDungs.Add(nguoiDung);
                    database.SaveChanges();
                    return RedirectToAction("QuanLyNhanVien");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ViewBag.IsExist = "Username đã tồn tại!";

                return View(nguoiDung);
            }
        }

        public ActionResult NhanVienLock(int id)
        {
            var nguoiDung = database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            nguoiDung.ConfirmPass = nguoiDung.Password;
            nguoiDung.NgayChinhSua = DateTime.Now;
            nguoiDung.Status = 2;
            database.Entry(nguoiDung).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyNhanVien", "QuanLy");
        }
        public ActionResult ResetPassword(int id)
        {
            var nguoiDung = database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            nguoiDung.NgayChinhSua = DateTime.Now;
            var password = Utils.Crypto(nguoiDung.Username);
            nguoiDung.Password = password;
            nguoiDung.ConfirmPass = password;
            try
            {
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult NhanVienEdit(int id)
        {
            return View(database.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult NhanVienEdit(int id, NguoiDung nguoiDung)
        {
            nguoiDung.NgayChinhSua = DateTime.Now;
            try
            {
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult NhanVienDelete(int id)
        {
            try
            {
                var nguoiDung = database.NguoiDungs.Find(id);
                nguoiDung.Status = 3;
                database.Entry(nguoiDung).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        public ActionResult QuanLyVoucher()
        {
            var voucher = database.PhieuQuaTangs.ToList();
            return View(voucher);
        }
        public ActionResult VoucherCreate()
        {
            List<ThuongHieu> thuonghieu = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuonghieu, "MaThuongHieu", "TenThuongHieu");
            PhieuQuaTang phieuquatang = new PhieuQuaTang();
            return View(phieuquatang);
        }
        [HttpPost]
        public ActionResult VoucherCreate(PhieuQuaTang phieuquatang, string ngayKetThuc)
        {
            var check_ID = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == phieuquatang.MaPhieuQuaTang).FirstOrDefault();

            if (check_ID == null)
            {
                if (phieuquatang.NgayKichHoat > phieuquatang.NgayKetThuc)
                {
                    return View();
                }
                database.Configuration.ValidateOnSaveEnabled = false;
                phieuquatang.NgayTao = DateTime.Now;
                phieuquatang.Status = 2;
                phieuquatang.NgayKichHoat = DateTime.Now;
                phieuquatang.NgayKetThuc = DateTime.Parse(ngayKetThuc);
                database.PhieuQuaTangs.Add(phieuquatang);
                database.SaveChanges();
                return RedirectToAction("QuanLyVoucher");
            }
            else
            {
                ViewBag.Error = "voucher đã tồn tại";
                return View();
            }
        }
        public ActionResult EditVoucher(int id)
        {
            List<ThuongHieu> thuonghieu = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuonghieu, "MaThuongHieu", "TenThuongHieu");
            var voucher = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == id).FirstOrDefault();
            return View(voucher);
        }
        [HttpPost]
        public ActionResult EditVoucher(int id, PhieuQuaTang phieuquatang, string ngayKetThuc)
        {

            List<ThuongHieu> thuonghieu = database.ThuongHieux.ToList();
            ViewBag.ThuongHieu = new SelectList(thuonghieu, "MaThuongHieu", "TenThuongHieu");
            try
            {
                if (phieuquatang.Status == 3 && phieuquatang.SoLuong > 0)
                {
                    phieuquatang.Status = 2;
                }
                else if (phieuquatang.Status != 3 && phieuquatang.SoLuong == 0)
                {
                    phieuquatang.Status = 3;
                }
                phieuquatang.NgayKetThuc = DateTime.Parse(ngayKetThuc);
                database.Entry(phieuquatang).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("QuanLyVoucher");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ThayDoiTrangThai(int id)
        {
            var phieuquatang = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == id).FirstOrDefault();
            switch (phieuquatang.Status)
            {
                case 1:
                    phieuquatang.Status = 2;

                    break;
                case 2:
                    phieuquatang.Status = 1;
                    break;
            }
            database.Entry(phieuquatang).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyVoucher");
        }

        #region QLBaoCao
        public ActionResult QuanLyBaoCao(string searching, string Year, string Month)
        {
            if (Year == "")
            {
                Year = (DateTime.Now.Year-3).ToString();
            }
            var dates = database.HoaDons.Where(s => (s.NgayMua.Year + "-" + s.NgayMua.Month + "-" + s.NgayMua.Day) == searching).ToList();

            var ngaymua = dates.ToList();


            int soluong = 0;
            foreach (var item in ngaymua)
            {
                soluong += item.CTHoaDons.Count;
                ViewBag.ngay = item.NgayMua;
            }
            ViewBag.sl = soluong;


            if (Month != "" && Year != null)
            {
                int ngaytrongthang = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(Month));
                double[] doanhthutuans = new double[5];
                double[] giamgiatuans = new double[5];
                double[] giavontuans = new double[5];
                double[] loinhuantuans = new double[5];
                int ngaytrongtuan1 = 1;
                int ngaytrongtuan2 = 7;
                for (int i = 0; i < 5; i++)
                {
                    if (ngaytrongthang >= ngaytrongtuan2)
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongtuan2 && s.NgayMua.Day >= ngaytrongtuan1).ToList();
                        ngaytrongtuan1 += 7;
                        ngaytrongtuan2 += 7;
                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                    else
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongthang && s.NgayMua.Day >= ngaytrongtuan1).ToList();

                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                }
                ViewBag.doanhthutuans = doanhthutuans;
                ViewBag.giamgiatuans = giamgiatuans;
                ViewBag.giavontuans = giavontuans;
                ViewBag.loinhuantuans = loinhuantuans;

                double tongdoanhthutuan = 0, tonggiamgiatuan = 0, tonggiavongtuan = 0, tongloinhuantuan = 0;



                foreach (var i in doanhthutuans)
                {
                    tongdoanhthutuan += i;

                }

                foreach (var i in giamgiatuans)
                {
                    tonggiamgiatuan += i;
                }
                foreach (var i in giavontuans)
                {
                    tonggiavongtuan += i;
                }
                foreach (var i in loinhuantuans)
                {
                    tongloinhuantuan += i;
                }
                ViewBag.tongdoanhthutuan = tongdoanhthutuan;
                ViewBag.tonggiamgiatuan = tonggiamgiatuan;
                ViewBag.tonggiavongtuan = tonggiavongtuan;
                ViewBag.tongloinhuantuan = tongloinhuantuan;
                ViewBag.ngay = System.DateTime.DaysInMonth(Int32.Parse(Year), Int32.Parse(Month));
            }
            if (Month == "" && Year != null)
            {

                double[] doanhthuthangs = new double[15];
                double[] giamgiathangs = new double[15];
                double[] giavonthangs = new double[15];
                double[] loinhuanthangs = new double[15];
                var hoadonNam = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year).ToList();
                foreach (var itemhd in hoadonNam)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (itemhd.NgayMua.Month == i)
                        {
                            double temp1 = itemhd.TongThanhToan ?? 0;
                            double temp2 = itemhd.SoTienGiam ?? 0;
                            doanhthuthangs[i] = doanhthuthangs[i] + temp1;
                            giamgiathangs[i] = giamgiathangs[i] + temp2;

                            foreach (var itemcthd in itemhd.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavonthangs[i] = giavonthangs[i] + temp3;
                            }

                            loinhuanthangs[i] = doanhthuthangs[i] - giavonthangs[i];
                        }
                    }
                }
                ViewBag.doanhthuthangs = doanhthuthangs.Skip(1).ToArray();
                ViewBag.giamgiathangs = giamgiathangs;
                ViewBag.giavonthangs = giavonthangs;
                ViewBag.loinhuanthangs = loinhuanthangs;

                double tongdoanhthuthang = 0, tonggiamgiathang = 0, tonggiavongthang = 0, tongloinhuanthang = 0;



                foreach (var i in doanhthuthangs)
                {
                    tongdoanhthuthang += i;

                }

                foreach (var i in giamgiathangs)
                {
                    tonggiamgiathang += i;
                }
                foreach (var i in giavonthangs)
                {
                    tonggiavongthang += i;
                }
                foreach (var i in loinhuanthangs)
                {
                    tongloinhuanthang += i;
                }
                ViewBag.tongdoanhthuthang = tongdoanhthuthang;
                ViewBag.tonggiamgiathang = tonggiamgiathang;
                ViewBag.tonggiavongthang = tonggiavongthang;
                ViewBag.tongloinhuanthang = tongloinhuanthang;

            }



            ViewBag.thang = Month;
            ViewBag.nam = Year;

            return View(dates);
        }
        public ActionResult QuanLyBaoCaoXuatPDF(string Acc, string Year, string Month)
        {

            double[] doanhthuthangs = new double[15];
            double[] giamgiathangs = new double[15];
            double[] giavonthangs = new double[15];
            double[] loinhuanthangs = new double[15];


            if (Month != null && Year != null)
            {
                int ngaytrongthang = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(Month));
                double[] doanhthutuans = new double[5];
                double[] giamgiatuans = new double[5];
                double[] giavontuans = new double[5];
                double[] loinhuantuans = new double[5];
                int ngaytrongtuan1 = 1;
                int ngaytrongtuan2 = 7;
                for (int i = 0; i < 5; i++)
                {
                    if (ngaytrongthang >= ngaytrongtuan2)
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongtuan2 && s.NgayMua.Day >= ngaytrongtuan1).ToList();
                        ngaytrongtuan1 += 7;
                        ngaytrongtuan2 += 7;
                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                    else
                    {
                        var hoadonNgay = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year && s.NgayMua.Month.ToString() == Month && s.NgayMua.Day <= ngaytrongthang && s.NgayMua.Day >= ngaytrongtuan1).ToList();

                        foreach (var item in hoadonNgay)
                        {
                            double temp1 = item.TongThanhToan ?? 0;
                            double temp2 = item.SoTienGiam ?? 0;
                            doanhthutuans[i] = doanhthutuans[i] + temp1;
                            giamgiatuans[i] = giamgiatuans[i] + temp2;
                            foreach (var itemcthd in item.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavontuans[i] = giavontuans[i] + temp3;
                            }
                        }
                        loinhuantuans[i] = doanhthutuans[i] - giavontuans[i];
                    }
                }
                ViewBag.doanhthutuans = doanhthutuans;
                ViewBag.giamgiatuans = giamgiatuans;
                ViewBag.giavontuans = giavontuans;
                ViewBag.loinhuantuans = loinhuantuans;

                double tongdoanhthutuan = 0, tonggiamgiatuan = 0, tonggiavongtuan = 0, tongloinhuantuan = 0;



                foreach (var i in doanhthutuans)
                {
                    tongdoanhthutuan += i;

                }

                foreach (var i in giamgiatuans)
                {
                    tonggiamgiatuan += i;
                }
                foreach (var i in giavontuans)
                {
                    tonggiavongtuan += i;
                }
                foreach (var i in loinhuantuans)
                {
                    tongloinhuantuan += i;
                }
                ViewBag.tongdoanhthutuan = tongdoanhthutuan;
                ViewBag.tonggiamgiatuan = tonggiamgiatuan;
                ViewBag.tonggiavongtuan = tonggiavongtuan;
                ViewBag.tongloinhuantuan = tongloinhuantuan;
                ViewBag.ngay = System.DateTime.DaysInMonth(Int32.Parse(Year), Int32.Parse(Month));
            }
            if (Month == null && Year != null)
            {
                var hoadonNam = database.HoaDons.Where(s => s.NgayMua.Year.ToString() == Year).ToList();
                foreach (var itemhd in hoadonNam)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (itemhd.NgayMua.Month == i)
                        {
                            double temp1 = itemhd.TongThanhToan ?? 0;
                            double temp2 = itemhd.SoTienGiam ?? 0;
                            doanhthuthangs[i] = doanhthuthangs[i] + temp1;
                            giamgiathangs[i] = giamgiathangs[i] + temp2;

                            foreach (var itemcthd in itemhd.CTHoaDons)
                            {
                                var giavon = database.SanPhamNhapKhoes.Where(s => s.MaSanPham == itemcthd.MaSanPham).FirstOrDefault();
                                double temp3 = (giavon.GiaNhap ?? 0) * itemcthd.SoLuong;
                                giavonthangs[i] = giavonthangs[i] + temp3;
                            }

                            loinhuanthangs[i] = doanhthuthangs[i] - giavonthangs[i];
                        }
                    }
                }
            }
            ViewBag.doanhthuthangs = doanhthuthangs.Skip(1).ToArray(); ;
            ViewBag.giamgiathangs = giamgiathangs;
            ViewBag.giavonthangs = giavonthangs;
            ViewBag.loinhuanthangs = loinhuanthangs;

            double tongdoanhthuthang = 0, tonggiamgiathang = 0, tonggiavongthang = 0, tongloinhuanthang = 0;
            foreach (var i in doanhthuthangs)
            {
                tongdoanhthuthang += i;
            }
            foreach (var i in giamgiathangs)
            {
                tonggiamgiathang += i;
            }
            foreach (var i in giavonthangs)
            {
                tonggiavongthang += i;
            }
            foreach (var i in loinhuanthangs)
            {
                tongloinhuanthang += i;
            }
            ViewBag.tongdoanhthuthang = tongdoanhthuthang;
            ViewBag.tonggiamgiathang = tonggiamgiathang;
            ViewBag.tonggiavongthang = tonggiavongthang;
            ViewBag.tongloinhuanthang = tongloinhuanthang;

            ViewBag.thang = Month;
            ViewBag.nam = Year;


            //var acc = (NguoiDung)Session["Account"];
            ViewBag.ten = Acc;
            return View();
        }

        public ActionResult GeneratePDF(string Year, string Month)
        {
            var acc = (NguoiDung)Session["Account"];
            string Acc = acc.Ten;
            return new Rotativa.ActionAsPdf("QuanLyBaoCaoXuatPDF", new { Acc, Year = Year, Month = Month });
        }


        public ActionResult BaoCaoDetail(int id)
        {
            var hoadon = database.HoaDons.Where(s => s.MaHoaDon == id).FirstOrDefault();

            return View(hoadon);
        }
        #endregion

        #region QLNhapKho
        public ActionResult QuanLyNhapKho()
        {
            var nhapkho = database.SanPhamNhapKhoes.ToList();
            return View(nhapkho);
        }
        public ActionResult NhapKho()
        {
            List<SanPham> sanphams = database.SanPhams.ToList();
            ViewBag.SanPham = new SelectList(sanphams, "MaSanPham", "TenSanPham");
            SanPhamNhapKho sp = new SanPhamNhapKho();
            return View(sp);
        }
        [HttpPost]
        public ActionResult NhapKho(SanPhamNhapKho sp)
        {
            sp.NgayTao = DateTime.Now;
            
            database.Configuration.ValidateOnSaveEnabled = false;
            database.SanPhamNhapKhoes.Add(sp);
            database.SaveChanges();
            return RedirectToAction("QuanLyNhapKho");
        }
        #endregion




        public ActionResult QuanLyFeedback()
        {
            var feedback = database.DanhGias.Where(s => s.IsApproved == false && s.IsDeleted == false).ToList();
            return View(feedback);
        }
        public ActionResult ApproveFB(int maDanhGia)
        {
            var feedback = database.DanhGias.Where(s => s.MaDanhGia == maDanhGia).FirstOrDefault();
            feedback.IsApproved = true;

            database.Entry(feedback).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyFeedBack");
        }
        public ActionResult DeletedFB(int maDanhGia)
        {
            var feedback = database.DanhGias.Where(s => s.MaDanhGia == maDanhGia).FirstOrDefault();
            feedback.IsDeleted = true;

            database.Entry(feedback).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("QuanLyFeedBack");
        }
    }
}