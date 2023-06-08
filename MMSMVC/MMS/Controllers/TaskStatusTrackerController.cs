using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Security;

namespace MMS.Controllers
{
    [SessionExpire]
   
    public class TaskStatusTrackerController : Controller
    {
        // GET: TaskStatusTracker
        public ActionResult Index()
        {
            return View();
        }
    }
}