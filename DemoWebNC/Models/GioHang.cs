using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebNC.Models
{
    public class GioHang
    {
        DemoWebNCEntities db = new DemoWebNCEntities();
        public int masp { get; set; }
        public string tensp { get; set; }
        public int maloaisp { get; set; }
        public string tenloaisp { get; set; }
        public string hinhanh { get; set; }
        public double dongia { get; set; }
        public int soluong { get; set; }
        public double thanhtien 
        {
            get { return soluong * dongia; }
        }
        public double TongTien
        {
            get
            {
                double TongTien = 0;
                List<GioHang> lstGioHang = HttpContext.Current.Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)
                {
                    TongTien = lstGioHang.Sum(n => n.thanhtien);
                }
                return TongTien;
            }
        }
        public GioHang(int MASP)
        {
            masp = MASP;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == masp);
            tensp = sp.TenSP;
            maloaisp = sp.LoaiSanPham.MaLoaiSP;
            tenloaisp = sp.LoaiSanPham.TenLoaiSP;
            hinhanh = sp.Anh;
            dongia = double.Parse(sp.GiaBan.ToString());
            soluong = 1;

        }

    }
}