using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.Models;
using PagedList.Mvc;
using PagedList;

namespace DemoWebNC.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        DemoWebNCEntities db = new DemoWebNCEntities();
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f,int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<SanPham> lstKQTK = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào! ";
                return View(db.SanPhams.OrderBy(n=>n.TenSP).ToPagedList(pageNumber,pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả.";
            return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult KetQuaTimKiem(int? page, string sTuKhoa)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<SanPham> lstKQTK = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào! ";
                return View(db.SanPhams.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả.";
            return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
        public ViewResult XemChiTiet(int masp, int malsp)
        {
            List<Models.SanPham> relatedProduct = db.SanPhams.Where(n => n.LoaiSanPham.MaLoaiSP == malsp).Take(3).ToList();
            ViewBag.relatedProduct = relatedProduct;
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
    }
}