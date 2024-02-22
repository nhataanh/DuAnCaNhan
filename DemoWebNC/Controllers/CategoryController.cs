using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.Models;
using PagedList;
using PagedList.Mvc;

namespace DemoWebNC.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        DemoWebNCEntities db = new DemoWebNCEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var lstSanPham = db.SanPhams.ToList().OrderBy(n=>n.GiaBan).ToPagedList(pageNumber,pageSize);
            return View(lstSanPham);
        }
        public ViewResult XemChiTiet(int masp,int malsp)
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