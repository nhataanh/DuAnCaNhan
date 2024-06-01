using DemoWebNC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebNC.Controllers
{
    [AdminAuthorize]
    public class AdminPQController : Controller
    {
        // GET: AdminPQ
        public ActionResult Index()
        {
            return View();
        }
    }
}