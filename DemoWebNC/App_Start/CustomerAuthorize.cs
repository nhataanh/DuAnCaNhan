using DemoWebNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebNC.App_Start
{
    public class CustomerAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.Session["TaiKhoan"] as NguoiDung;

            // Kiểm tra nếu người dùng đang đăng nhập và có quyền là "Admin" thì được phép truy cập
            if (user != null && user.TaiKhoan != "Admin")
            {
                return true;
            }

            return false;
        }
    }
}