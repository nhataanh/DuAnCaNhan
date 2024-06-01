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
    public class sDonHangController : Controller
    {
        private DemoWebNCEntities db = new DemoWebNCEntities();

        // GET: DonHangs
        public ActionResult Index()
        {
            var donHangs = db.DonHangs.Include(d => d.NguoiDung);
            return View(donHangs.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<ChiTietDonHang> chiTietDonHang = db.ChiTietDonHangs.Where(c => c.MaDonHang == id).ToList();
            if (chiTietDonHang == null || chiTietDonHang.Count == 0)
            {
                return HttpNotFound();
            }
            ViewBag.HoTenNguoiDung = chiTietDonHang.FirstOrDefault()?.DonHang?.NguoiDung?.HoTen;
            return View(chiTietDonHang);
        }
        // GET: DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKH", "HoTen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDonHang,DaThanhToan,TinhTrangGiaoHang,NgayDat,NgayGiao,MaKH")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKH", "HoTen", donHang.MaKH);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKH", "HoTen", donHang.MaKH);
            return View(donHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,DaThanhToan,TinhTrangGiaoHang,NgayDat,NgayGiao,MaKH")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKH", "HoTen", donHang.MaKH);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.FirstOrDefault(c => c.MaDonHang == id);
            db.ChiTietDonHangs.Remove(chiTietDonHang);
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
