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
using PagedList;
using PagedList.Mvc;

namespace DemoWebNC.Controllers
{
    [AdminAuthorize]
    public class sSanPhamController : Controller
    {
        private DemoWebNCEntities db = new DemoWebNCEntities();

        // GET: SanPhams
        public ActionResult Index(int? page)
        {
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            // Sắp xếp danh sách sản phẩm theo ngày cập nhật mới nhất
            var lstSanPham = db.SanPhams.OrderByDescending(n => n.NgayCapNhat).ToList().ToPagedList(pageNumber, pageSize);

            return View(lstSanPham);
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,GiaBan,MoTa,Anh,Anh1,Anh2,Anh3,Anh4,NgayCapNhat,SoLuongTon,MaLoaiSP")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật trường NgàyCapNhat về ngày hiện tại
                sanPham.NgayCapNhat = DateTime.Now;

                var f = Request.Files["Anh"];
                if (f != null && f.ContentLength > 0)
                {
                    string fName = f.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f.SaveAs(foder);
                    sanPham.Anh = "/assets/images/" + fName;
                }
                var f1 = Request.Files["Anh1"];
                if (f1 != null && f1.ContentLength > 0)
                {
                    string fName = f1.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f1.SaveAs(foder);
                    sanPham.Anh1 = "/assets/images/" + fName;
                }
                var f2 = Request.Files["Anh2"];
                if (f2 != null && f2.ContentLength > 0)
                {
                    string fName = f2.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f2.SaveAs(foder);
                    sanPham.Anh2 = "/assets/images/" + fName;
                }
                var f3 = Request.Files["Anh3"];
                if (f3 != null && f3.ContentLength > 0)
                {
                    string fName = f3.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f3.SaveAs(foder);
                    sanPham.Anh3 = "/assets/images/" + fName;
                }
                var f4 = Request.Files["Anh4"];
                if (f4 != null && f4.ContentLength > 0)
                {
                    string fName = f4.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f4.SaveAs(foder);
                    sanPham.Anh4 = "/assets/images/" + fName;
                }
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,GiaBan,MoTa,Anh,Anh1,Anh2,Anh3,Anh4,NgayCapNhat,SoLuongTon,MaLoaiSP")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật trường NgàyCapNhat về ngày hiện tại
                sanPham.NgayCapNhat = DateTime.Now;

                var f = Request.Files["Anh"];
                if (f != null && f.ContentLength > 0)
                {
                    string fName = f.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f.SaveAs(foder);
                    sanPham.Anh = "/assets/images/" + fName;
                }
                var f1 = Request.Files["Anh1"];
                if (f1 != null && f1.ContentLength > 0)
                {
                    string fName = f1.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f1.SaveAs(foder);
                    sanPham.Anh1 = "/assets/images/" + fName;
                }
                var f2 = Request.Files["Anh2"];
                if (f2 != null && f2.ContentLength > 0)
                {
                    string fName = f2.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f2.SaveAs(foder);
                    sanPham.Anh2 = "/assets/images/" + fName;
                }
                var f3 = Request.Files["Anh3"];
                if (f3 != null && f3.ContentLength > 0)
                {
                    string fName = f3.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f3.SaveAs(foder);
                    sanPham.Anh3 = "/assets/images/" + fName;
                }
                var f4 = Request.Files["Anh4"];
                if (f4 != null && f4.ContentLength > 0)
                {
                    string fName = f4.FileName;
                    string foder = Server.MapPath("/assets/images/") + fName;
                    f4.SaveAs(foder);
                    sanPham.Anh4 = "/assets/images/" + fName;
                }
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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
