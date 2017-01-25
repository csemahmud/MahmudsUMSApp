using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MahmudsUMSApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Uri uri = Request.Url;
            ViewBag.AbsoluteUrlBase = String.Format(
                "{0}://{1}{2}",
                uri.Scheme,
                uri.Host,
                (uri.IsDefaultPort ? "" : String.Format(
                                        ":{0}", uri.Port
                                        ))
                );

            ViewBag.Message = "M@hmud's UMS is a Software for complete University Management System";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
