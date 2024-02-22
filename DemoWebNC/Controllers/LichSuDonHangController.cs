using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoWebNC.Models;

namespace DemoWebNC.Controllers
{
    public class LichSuDonHangController : Controller
    {
        private DemoWebNCEntities db = new DemoWebNCEntities();
        // GET: LichSuDonHang
        public ActionResult Index(int? id)
        {
            // Kiểm tra xem id có được truyền vào hay không
            if (id == null)
            {
                // Trả về một trang lỗi hoặc thực hiện xử lý phù hợp
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy chi tiết đơn hàng theo ID
            var lichsudonhangs = db.ChiTietDonHangs
                                    .Include(c => c.DonHang)
                                    .Include(c => c.SanPham)
                                    .Where(c => c.DonHang.MaKH == id); // Lọc theo DonHangId

            // Trả về danh sách chi tiết đơn hàng cho view
            return View(lichsudonhangs.ToList());
        }
    }
}