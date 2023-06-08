using MMS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMS.Controllers
{
    public class EmployeeDetailsController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();

        // GET: EmployeeDetails
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ViewEmployee()
        {
            var EmployeeLocation = db.MMS_Employee_Location.Select(x => x.Description).ToList();
            List<EmployeeSummary> EmpList = db.Employee_Master.Select(x => new EmployeeSummary
            {
                EmployeeICode = x.EmployeeICode,
                LoginUserName = x.LoginUserName,
                Location = x.Location,
                EmployeeCorporateEmailId = x.EmployeeCorporateEmailId,
                EmployeeDateOfJoining = x.EmployeeDateOfJoining,
                EmployeePrimarySkill = x.EmployeePrimarySkill,
                EmployeePreviousExperienceYears = x.EmployeePreviousExperienceYears,
                EmployeePreviousExperienceMonths = x.EmployeePreviousExperienceMonths,                
                EmployeeNumber = x.EmployeeNumber,
                SwipeICode = x.SwipeICode
            }).ToList();

            var Location = "";
            var EmpLocation = "";
            for (int i = 0; i < EmpList.Count; i++)
            {
                Location = "";
                EmpLocation = "";
                if (EmpList[i].Location != null)
                {
                    Location = Convert.ToString(EmpList[i].Location).ToUpper();
                }

                EmpLocation = db.MMS_Employee_Location.Where(x => x.Location.ToUpper() == Location.ToUpper()).Select(x => x.Description).FirstOrDefault();
                EmpList[i].Location = EmpLocation;
            }
            return Json(EmpList, JsonRequestBehavior.AllowGet);
        }
    }
}