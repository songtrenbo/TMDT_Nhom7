using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
using TMDT.Utility;

namespace TMDT.Controllers
{
    public class GioHangController : Controller
    {
        DBLaptopEntities database = new DBLaptopEntities();
        Cart cart = System.Web.HttpContext.Current.Session["Cart"] as Cart;
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HienThiGioHang()
        {
            return View();
        }
        public ActionResult GioHangRong()
        {
            return View();
        }
        public ActionResult CheckDangNhap()
        {
            return View();
        }

        public Cart GetCart()
        {
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public void AddToCart(int id, int quantity)
        {
            var _pro = database.SanPhams.SingleOrDefault(s => s.MaSanPham == id);
            if (_pro != null)
            {
                Cart cart = GetCart();
                var check = cart.Items.Where(s => s._sanPham.MaSanPham == id).FirstOrDefault();
                if (check == null)
                {
                    cart.Add_Product_Cart(_pro, quantity);
                }
            }
        }
        public ActionResult Checkout()
        {
            HoaDon hoaDon = new HoaDon();
            return View(hoaDon);
        }
        [HttpPost]
        public ActionResult Checkout(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NguoiDung nguoiDung = (NguoiDung)Session["Account"];
                    if (nguoiDung != null)
                        hoaDon.MaKhachHang = nguoiDung.MaNguoiDung;
                    hoaDon.NgayMua = DateTime.Now;
                    hoaDon.TinhTrang = 1;

                    database.HoaDons.Add(hoaDon);
                    foreach (var item in cart.Items)
                    {
                        CTHoaDon cTHoaDon = new CTHoaDon();
                        cTHoaDon.MaHoaDon = hoaDon.MaHoaDon;
                        cTHoaDon.MaSanPham = item._sanPham.MaSanPham;
                        cTHoaDon.DonGia = (int)item._sanPham.GiaBan;
                        cTHoaDon.SoLuong = item._quantity;
                        cTHoaDon.ThanhTien = (int)(item._quantity * item._sanPham.GiaBan);
                        database.CTHoaDons.Add(cTHoaDon);
                        foreach (var sp in database.SanPhams.Where(s => s.MaSanPham == cTHoaDon.MaSanPham))
                        {
                            sp.SoLuongTon -= item._quantity;
                            sp.SoLuongBan += item._quantity;
                        }
                    }

                    PhieuQuaTang phieuQuaTang = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == hoaDon.MaPhieuQuaTang).FirstOrDefault();
                    if (phieuQuaTang != null)
                    {
                        phieuQuaTang.SoLuong--;
                        database.Entry(phieuQuaTang).State = EntityState.Modified;

                        NguoiDung_PhieuQuaTang ngVoucher = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == hoaDon.MaKhachHang && s.MaPhieuQuaTang == hoaDon.MaPhieuQuaTang).FirstOrDefault();
                        database.NguoiDung_PhieuQuaTang.Remove(ngVoucher);
                    }
                    Session["hoaDon"] = hoaDon;

                    if (hoaDon.HinhThucThanhToan == 2)
                        return PaymentWithMomo(hoaDon.TongThanhToan ?? 0);
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
                Tuple<int, string> result = ConfirmPaymentClient(0);
                switch (result.Item1)
                {
                    case 0:
                        database.SaveChanges();
                        Session["hoaDon"] = null;
                        Session["Cart"] = null;
                        //cart.ClearCart();
                        return RedirectToAction("CheckOutResult", "GioHang", new { result = result.Item2 });
                    case 49:
                        return RedirectToAction("CheckOut", "GioHang");
                    default:
                        return RedirectToAction("CheckOutResult", "GioHang", new { result = result.Item2 });
                }
            }
            else
                return View();

        }
        public ActionResult PaymentWithMomo(double totalPrice)
        {
            try
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                //request params need to request to MoMo system
                string endpoint = MomoConfig.EndPoint;
                string partnerCode = MomoConfig.PartnerCode;
                string accessKey = MomoConfig.AccessKey;
                string serectkey = MomoConfig.Serectkey;
                string orderInfo = "test";
                string returnUrl = baseUrl + "GioHang/CheckoutResult";
                string notifyurl = baseUrl + "GioHang/SavePayment";

                string amount = totalPrice.ToString();
                string orderid = DateTime.Now.Ticks.ToString();
                string requestId = DateTime.Now.Ticks.ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);

                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
            catch
            {
                return View("HienThiGioHang");
            }
        }
        public Tuple<int, string> ConfirmPaymentClient(int? resultCode = null)
        {
            if (resultCode == null)
                resultCode = Convert.ToInt32(Request.QueryString["errorCode"]);

            string result = "";
            switch (resultCode)
            {
                case 0:
                    result = "Đặt hàng và thanh toán thành công";
                    break;
                case 36:
                    result = "Quá thời gian thanh toán";
                    break;
                case 49:
                    result = "Quay lại";
                    break;
                //return RedirectToAction("CheckOut", "GioHang");
                default:
                    result = "Lỗi " + resultCode.ToString();
                    break;
            }
            ViewBag.Result = result;
            return new Tuple<int, string>(resultCode.Value, result);
        }
        public ActionResult CheckoutResult()
         {
            int resultCode = Convert.ToInt32(Request.QueryString["errorCode"]);

            string result = "";
            switch (resultCode)
            {
                case 0:
                    result = "Đặt hàng và thanh toán thành công";
                    HoaDon hoaDon = Session["hoaDon"] as HoaDon;
                    if (hoaDon != null)
                        SavePayment();
                        //cart.ClearCart();
                        break;
                case 36:
                    result = "Quá thời gian thanh toán";
                    break;
                case 49:
                    result = "Quay lại";
                    return RedirectToAction("CheckOut", "GioHang");
                default:
                    result = "Lỗi " + resultCode.ToString();
                    break;
            }
            ViewBag.ResultCode = resultCode;
            ViewBag.Result = result;
            return View();
        }
        public void SavePayment()
        {
            HoaDon hoaDon = Session["hoaDon"] as HoaDon;
            if (hoaDon!=null)
            {
                database.HoaDons.Add(hoaDon);
                foreach (var item in cart.Items)
                {
                    CTHoaDon cTHoaDon = new CTHoaDon();
                    cTHoaDon.MaHoaDon = hoaDon.MaHoaDon;
                    cTHoaDon.MaSanPham = item._sanPham.MaSanPham;
                    cTHoaDon.DonGia = (int)item._sanPham.GiaBan;
                    cTHoaDon.SoLuong = item._quantity;
                    cTHoaDon.ThanhTien = (int)(item._quantity * item._sanPham.GiaBan);
                    database.CTHoaDons.Add(cTHoaDon);
                    foreach (var sp in database.SanPhams.Where(s => s.MaSanPham == cTHoaDon.MaSanPham))
                    {
                        sp.SoLuongTon -= item._quantity;
                        sp.SoLuongBan += item._quantity;
                    }
                }

                PhieuQuaTang phieuQuaTang = database.PhieuQuaTangs.Where(s => s.MaPhieuQuaTang == hoaDon.MaPhieuQuaTang).FirstOrDefault();
                if (phieuQuaTang != null)
                {
                    phieuQuaTang.SoLuong--;
                    database.Entry(phieuQuaTang).State = EntityState.Modified;

                    NguoiDung_PhieuQuaTang ngVoucher = database.NguoiDung_PhieuQuaTang.Where(s => s.MaNguoiDung == hoaDon.MaKhachHang && s.MaPhieuQuaTang == hoaDon.MaPhieuQuaTang).FirstOrDefault();
                    database.NguoiDung_PhieuQuaTang.Remove(ngVoucher);
                }
                else hoaDon.SoTienGiam = 0;
                database.SaveChanges();
                Session["hoaDon"] = null;
                Session["Cart"] = null;
            }
            
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            //try
            //{
            //A resource representing a Payer that funds a payment Payment Method as paypal  
            //Payer Id will be returned when payment proceeds or click to pay  
            string payerId = Request.Params["PayerID"];
            if (string.IsNullOrEmpty(payerId))
            {
                //this section will be executed first because PayerID doesn't exist  
                //it is returned by the create function call of the payment class  
                // Creating a payment  
                // baseURL is the url on which paypal sendsback the data.  
                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                //here we are generating guid for storing the paymentID received in session  
                //which will be used in the payment execution  
                var guid = Convert.ToString((new Random()).Next(100000));
                //CreatePayment function gives us the payment approval url  
                //on which payer is redirected for paypal account payment  
                var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                //get links returned from paypal in response to Create function call  
                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    Links lnk = links.Current;
                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.href;
                    }
                }
                // saving the paymentID in the key guid  
                Session.Add(guid, createdPayment.id);
                return Redirect(paypalRedirectUrl);
            }
            else
            {
                // This function exectues after receving all parameters for the payment  
                var guid = Request.Params["guid"];
                var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                //If executed payment failed then we will show payment failure message to user  
                if (executedPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    return View("FailureView" +ex);
            //}
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            Cart cart = Session["Cart"] as Cart;
            foreach (var item in cart.Items)
            {
                //Adding Item Details like name, currency, price etc  
                itemList.items.Add(new Item()
                {
                    name = item._sanPham.TenSanPham,
                    currency = "USD",
                    price = (item._sanPham.GiaBan / Utils.EXCHANGE_RATE).ToString(),
                    quantity = item._quantity.ToString(),
                    sku = "sku"
                });
            }

            var totalCart = cart.Total_money();
            var payment = new Payment()
            {
                intent = "sale",
                transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        amount = new Amount()
                        {
                            total = totalCart.ToString(),
                            currency = "USD",
                            details = new Details
                            {
                                tax = "0",
                                shipping = "0",
                                subtotal = totalCart.ToString()
                            }
                        },
                        item_list = itemList,
                        description = "Invoice #"+DateTime.Now.Ticks,
                        invoice_number = "INV"+DateTime.Now.Ticks
                    }
                },
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = "/Paypal/CheckoutFail",
                    return_url = "/Paypal/CheckoutSuccess"
                },
                payer = new Payer()
                {
                    payment_method = "paypal"
                }
            };
            // Create a payment using a APIContext  
            return payment.Create(apiContext);
        }
        public ActionResult CheckoutFail()
        {
            return View();
        }
        public ActionResult CheckoutSuccess()
        {
            return View();
        }
        public JsonResult ApplyVoucher(string maVoucher)
        {
            NguoiDung nguoiDung = (NguoiDung)Session["Account"];
            var voucher = (from t in database.PhieuQuaTangs.ToList()
                           where t.MaGiamGia == maVoucher
                           select new PhieuQuaTang
                           {
                               MaPhieuQuaTang = t.MaPhieuQuaTang,
                               MaGiamGia = t.MaGiamGia,
                               SoLuong = t.SoLuong,
                               GiaTri = t.GiaTri,
                               GiaTriDonHangToiThieu = t.GiaTriDonHangToiThieu,
                               TenPhieuQuaTang = t.TenPhieuQuaTang,
                               LoaiPhamVi = t.LoaiPhamVi,
                               NgayKetThuc = t.NgayKetThuc,
                               NgayKichHoat = t.NgayKichHoat,
                               NgayTao = t.NgayTao,
                               Status = t.Status,
                           }).FirstOrDefault();

            return Json(voucher, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckVoucher(string maVoucher)
        {
            NguoiDung nguoiDung = (NguoiDung)Session["Account"];
            var tmp = database.PhieuQuaTangs.Where(s => s.MaGiamGia == maVoucher).FirstOrDefault();
            var ngdungvuocher = database.NguoiDung_PhieuQuaTang.Where(s => s.MaPhieuQuaTang == tmp.MaPhieuQuaTang).ToList();
            if (ngdungvuocher.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}