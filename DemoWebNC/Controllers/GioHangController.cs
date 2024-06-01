using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.Models;
using PayPal.Api;
using static DemoWebNC.RouteConfig;

namespace DemoWebNC.Controllers
{
    public class GioHangController : Controller
    {
        // GET: Cart
        DemoWebNCEntities db = new DemoWebNCEntities();
        #region Giỏ hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;

            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int masp, string strURL)
        {
            
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
                    
            }

            List<GioHang> lstGioHang = LayGioHang();
            GioHang gh = lstGioHang.Find(n => n.masp == masp);

            if (gh == null)
            {
                gh = new GioHang(masp);

                // Kiểm tra nếu số lượng yêu cầu vượt quá số lượng tồn
                if (gh.soluong > sp.SoLuongTon)
                {
                    gh.soluong = (int)sp.SoLuongTon; // Chỉ cập nhật số lượng sản phẩm bằng số lượng tồn
                }
                lstGioHang.Add(gh);
                return Json(new { success = true, itemCount = TongSoLuong() });
            }
            else
            {
                gh.soluong++;

                // Kiểm tra nếu số lượng yêu cầu vượt quá số lượng tồn
                if (gh.soluong > sp.SoLuongTon)
                {
                    gh.soluong = (int)sp.SoLuongTon; // Chỉ cập nhật số lượng sản phẩm bằng số lượng tồn
                }
                return Json(new { success = true, itemCount = TongSoLuong() });
            }
        }

        public ActionResult CapNhatGH(int masp, FormCollection f)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.masp == masp);
            if (sanpham != null)
            {
                int requestedQuantity = int.Parse(f["txtSoLuong"].ToString());

                // Kiểm tra nếu số lượng yêu cầu vượt quá số lượng tồn kho
                if (requestedQuantity > sp.SoLuongTon)
                {
                    // Cập nhật số lượng sản phẩm trong giỏ hàng bằng số lượng tồn kho tối đa
                    sanpham.soluong = (int)sp.SoLuongTon;
                    
                    TempData["CapNhatGioHang_ThongBao"] = "Số lượng sản phẩm trong kho không đủ, chỉ cập nhật số lượng sản phẩm đủ để có sẵn.";
                }
                else
                {
                    // Cập nhật số lượng sản phẩm trong giỏ hàng bằng số lượng yêu cầu
                    sanpham.soluong = requestedQuantity;
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int masp)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.masp == masp);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.masp == sanpham.masp);

            }
            if(lstGioHang.Count == 0)
            {
                Session["GioHang"] = null;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                TempData["KiemTraGH"] = "Trong giỏ hàng không có sản phẩm nào!";
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int TongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                //TongSoLuong = lstGioHang.Sum(n => n.soluong);
                TongSoLuong = lstGioHang.Count;
            }
            else
            {
                TongSoLuong = 0;
            }
            return TongSoLuong;
        }
        private double TongTien()
        {
            double TongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                TongTien = lstGioHang.Sum(n => n.thanhtien);
            }
            return TongTien;
        }

        public ActionResult GioHangPartial()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;

            if (lstGioHang == null || lstGioHang.Count == 0)
            {
                ViewBag.TongSoLuong = 0;
                TempData["KiemTraGH"] = "Trong giỏ hàng không có sản phẩm nào!";
            }
            else
            {
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
            }

            return PartialView(lstGioHang);
        }

        #endregion Giỏ hàng
        #region Đặt hàng
        [HttpPost]
        public ActionResult DatHang()
        {
            if(Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Account");
            }
            else
            {
                if (Session["GioHang"] == null)
                {
                    TempData["KiemTraDH"] = false;
                    return RedirectToAction("FailureView", "GioHang");
                }
                //Thêm đơn hàng
                else
                {
                    DonHang dh = new DonHang();
                    NguoiDung kh = (NguoiDung)Session["TaiKhoan"];
                    List<GioHang> gh = LayGioHang();
                    dh.MaKH = kh.MaKH;
                    dh.NgayDat = DateTime.Now;
                    db.DonHangs.Add(dh);
                    db.SaveChanges();
                    //Thêm chi tiết đơn hàng
                    foreach (var item in gh)
                    {
                        SanPham sp = db.SanPhams.Find(item.masp);
                        sp.SoLuongTon -= item.soluong;
                        db.Entry(sp).State = EntityState.Modified;

                        ChiTietDonHang ctdh = new ChiTietDonHang();
                        ctdh.MaDonHang = dh.MaDonHang;
                        ctdh.MaSP = item.masp;
                        ctdh.SoLuong = item.soluong;
                        ctdh.DonGia = (decimal)item.dongia;
                        db.ChiTietDonHangs.Add(ctdh);
                    }
                    List<GioHang> ghCopy = new List<GioHang>(gh);
                    int check = db.SaveChanges();
                    if (check > 0)
                    {
                        TempData["KiemTraDH"] = true;
                        foreach (var item in ghCopy)
                        {
                            XoaGioHang(item.masp);

                        }
                        Session["GioHang"] = null;
                    }
                    return RedirectToAction("SuccessView", "GioHang");
                }
            }

        }
        public ActionResult FailureView()
        {
            return View();
        }

        public ActionResult SuccessView()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Account");
            }
            else
            {
                //getting the apiContext  
                APIContext apiContext = PaypalConfiguration.GetAPIContext();
                try
                {
                    //A resource representing a Payer that funds a payment Payment Method as paypal  
                    //Payer Id will be returned when payment proceeds or click to pay  
                    string payerId = Request.Params["PayerID"];
                    if (string.IsNullOrEmpty(payerId))
                    {
                        //this section will be executed first because PayerID doesn't exist  
                        //it is returned by the create function call of the payment class  
                        // Creating a payment  
                        // baseURL is the url on which paypal sendsback the data.  
                        string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                        //here we are generating guid for storing the paymentID received in session  
                        //which will be used in the payment execution  
                        var guid = Convert.ToString((new Random()).Next(100000));
                        //CreatePayment function gives us the payment approval url  
                        //on which payer is redirected for paypal account payment  
                        var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
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
                }
                catch (Exception ex)
                {
                    return View("FailureView");
                }
                DonHang dh = new DonHang();
                NguoiDung kh = (NguoiDung)Session["TaiKhoan"];
                List<GioHang> gh = LayGioHang();
                dh.MaKH = kh.MaKH;
                dh.NgayDat = DateTime.Now;
                dh.DaThanhToan = 1;
                db.DonHangs.Add(dh);
                db.SaveChanges();
                //Thêm chi tiết đơn hàng
                foreach (var item in gh)
                {
                    SanPham sp = db.SanPhams.Find(item.masp);
                    sp.SoLuongTon -= item.soluong;
                    db.Entry(sp).State = EntityState.Modified;

                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.MaDonHang = dh.MaDonHang;
                    ctdh.MaSP = item.masp;
                    ctdh.SoLuong = item.soluong;
                    ctdh.DonGia = (decimal)item.dongia;
                    db.ChiTietDonHangs.Add(ctdh);
                }
                List<GioHang> ghCopy = new List<GioHang>(gh);
                int check = db.SaveChanges();
                if (check > 0)
                {
                    TempData["KiemTraDH"] = true;
                    foreach (var item in ghCopy)
                    {
                        XoaGioHang(item.masp);

                    }
                    Session["GioHang"] = null;
                }
                //on successful payment, show success page to user.  
                return View("SuccessView");
            }
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
            List<GioHang> lstGioHang = LayGioHang();
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            foreach(var item in lstGioHang)
            {
                itemList.items.Add(new Item()
                {
                    name = item.tensp,
                    currency = "USD",
                    price = Math.Round(item.dongia / 24965,2).ToString(),
                    quantity = item.soluong.ToString(),
                    sku = item.masp.ToString()
                });
            }
            
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            double subtotal = lstGioHang.Sum(item => item.thanhtienUSD);
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = subtotal.ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = subtotal.ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }


        #endregion Đặt hàng
    }
}