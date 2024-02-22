using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.Models;

namespace DemoWebNC.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home

        DemoWebNCEntities db = new DemoWebNCEntities();

        public ActionResult Index()
        {
            var lstSanPham = db.SanPhams.ToList();
            return View(lstSanPham);
        }
        public ViewResult XemChiTiet(int masp,int malsp)
        {
            List<Models.SanPham> relatedProduct = db.SanPhams.Where(n => n.LoaiSanPham.MaLoaiSP == malsp).Take(3).ToList();
            ViewBag.relatedProduct = relatedProduct;
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == masp);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
    }
}