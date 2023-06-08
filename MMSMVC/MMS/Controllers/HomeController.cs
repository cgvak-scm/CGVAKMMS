using MMS.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMS.Controllers
{
    public class HomeController : Controller
    {
        [SessionExpire]
     
        public ActionResult VersionInfo()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            ViewBag.Version = fvi.ProductVersion;
            ViewBag.Year = DateTime.Now.Year.ToString();
            //lblVersion.InnerText = fvi.ProductVersion;
            //lblYear.InnerText = DateTime.Now.Year.ToString();
            return View();
        }       
    }
}