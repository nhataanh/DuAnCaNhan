using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.App_Start;
using DemoWebNC.Models;

namespace DemoWebNC.Controllers
{
    [AdminAuthorize]
    public class BaoCaoThongKeController : Controller
    {
        private DemoWebNCEntities db = new DemoWebNCEntities();

        // GET: ChiTietDonHangs
        [HttpGet]
        public ActionResult Index()
        {
            var chiTietDonHangs = db.ChiTietDonHangs
                .Include(c => c.SanPham)
                .Where(c => c.SoLuong>0)
                .ToList();

            return View(chiTietDonHangs);
        }
        public ActionResult SanPhamTon()
        {
            var sanPhamTon = db.SanPhams
                            .Where(c => c.SoLuongTon > 0)
                            .ToList();

            return View(sanPhamTon);
        }
    }
}
