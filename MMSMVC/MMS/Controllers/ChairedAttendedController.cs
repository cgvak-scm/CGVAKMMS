using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Security;
using MMS.Models;
using System.Globalization;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]
  
    public class ChairedAttendedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMeetingDetails(int? meetingId)   //meeting details will be fetched by this meetingId
        {            
            var retResult = ChairedAttendedViewModel.GetMeetingDetails(Convert.ToInt32(meetingId));

            DateTime dt = DateTime.Now;
            String strDate = "";
            strDate = dt.ToString("MM/dd/yyyy");
            String formatDate = strDate.Replace('-', '/');
          

            return Json(retResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MeetingTask(DateTime startDate, DateTime endDate, int selectedVal)
        {
            dynamic meetingTaskRes = null;

            try
            {
                meetingTaskRes = ChairedAttendedViewModel.MeetingTask(startDate, endDate, selectedVal);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(meetingTaskRes, JsonRequestBehavior.AllowGet);
        }

    }
}