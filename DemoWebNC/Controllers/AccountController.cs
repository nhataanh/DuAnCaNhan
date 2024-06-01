using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DemoWebNC.Models;
namespace DemoWebNC.Controllers
{
    public class AccountController : Controller
    {

        DemoWebNCEntities db = new DemoWebNCEntities();
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("DangNhap");
        }
        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NguoiDung kh)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var crrcheck = db.NguoiDungs.FirstOrDefault(m => m.TaiKhoan == kh.TaiKhoan);
                    if(crrcheck != null)
                    {
                        ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại");
                    }
                    else
                    {
                        db.NguoiDungs.Add(kh);
                        db.SaveChanges();
                        TempData["ThongBaoDK"] = "Bạn đã đăng ký thành công!";
                    }
                }
                catch
                {

                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]

        public ActionResult DangNhap(FormCollection f, string ReturnUrl)
        {
            string TaiKhoans = f["txtTaiKhoan"].ToString();
            string MatKhaus = f.Get("txtMatKhau").ToString();
            NguoiDung kh = db.NguoiDungs.SingleOrDefault(n => n.TaiKhoan == TaiKhoans && n.MatKhau == MatKhaus);

            if (kh != null)
            {
                if (TaiKhoans == "Admin")
                {
                    FormsAuthentication.SetAuthCookie(kh.TaiKhoan, false);
                    Session["TaiKhoan"] = kh;
                    Session["HoTen"] = kh.HoTen;
                    Session["MaKH"] = kh.MaKH;
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "AdminPQ");
                    }
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(kh.TaiKhoan, false);
                    Session["TaiKhoan"] = kh;
                    Session["HoTen"] = kh.HoTen;
                    Session["MaKH"] = kh.MaKH;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ThongBaoTK = "Tên tài khoản hoặc mật khẩu không đúng !";
                return View(); // Trả về view để hiển thị thông báo lỗi
            }
        }

        [HttpGet]
        public ActionResult DangXuat()
        {
            // Xóa thông tin đăng nhập (ví dụ: session hoặc cookie)
            HttpContext.Session.Clear(); // Xóa toàn bộ dữ liệu phiên

            // Hủy phiên làm việc (session)
            HttpContext.Session.Abandon();

            // Xóa cookie (nếu có)
            if (Request.Cookies.Count > 0)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}