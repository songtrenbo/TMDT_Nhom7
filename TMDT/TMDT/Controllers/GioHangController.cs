using PayPal.Api;
using System;
using System.Collections.Generic;
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
            Cart cart = Session["Cart"] as Cart;
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
                GetCart().Add_Product_Cart(_pro, quantity);
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
                NguoiDung nguoiDung = (NguoiDung)Session["Account"];
                Cart cart = Session["Cart"] as Cart;
                hoaDon.MaKhachHang = nguoiDung.MaNguoiDung;
                hoaDon.NgayMua = DateTime.Now;
                hoaDon.NgayTao = DateTime.Now;
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

                if (hoaDon.HinhThucThanhToan == 2)
                {
                    PaymentWithPaypal();
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                return View();
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
    }
}