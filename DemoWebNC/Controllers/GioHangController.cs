using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.Models;
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
        public ActionResult ThemGioHang(int masp,string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang gh = lstGioHang.Find(n => n.masp == masp);
            if(gh == null)
            {
                gh = new GioHang(masp);
                lstGioHang.Add(gh);
                return Redirect(strURL);
            }
            else
            {
                gh.soluong++;
                return Redirect(strURL);
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
            if(sanpham != null)
            {
                sanpham.soluong = int.Parse( f["txtSoLuong"].ToString() );

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
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
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
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
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
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Thêm đơn hàng
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
                foreach (var item in ghCopy)
                {
                    XoaGioHang(item.masp);
                    
                }
                ViewBag.ThongBao = "Bạn đã đặt hàng thành công!";
            }
            return RedirectToAction("Index", "Home");
            //return View();

        }
        #endregion Đặt hàng
    }
}