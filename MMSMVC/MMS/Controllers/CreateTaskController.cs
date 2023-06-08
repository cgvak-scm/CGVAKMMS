using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Security;
using System.IO;
using MMS.Models;
using System.Net.Mail;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using static MMS.Models.MeetingViewModel;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]

    public class CreateTaskController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();

        // GET: CreateTask
        public ActionResult Index()
        {
            ViewBag.Departments = DepartmentCategoryModel.DepartmentMasterList();
            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            ViewBag.Frequencies = DepartmentCategoryModel.ReviewFrequency();
            return View();
        }

        [HttpGet]
        public JsonResult Designation(string selectedDepartment)
        {
            var departmentICode = db.Department_Master.Where(x => x.DepartmentName == selectedDepartment).Select(x => x.DepartmentICode).FirstOrDefault();

            var designationlist = db.Designation_Master.Where(x => x.DepartmentICode == departmentICode).ToList();
            List<Designation_Master> implst = new List<Designation_Master>();

            foreach (var list in designationlist)
            {
                implst.Add(new Designation_Master
                {
                    DesignationICode = list.DesignationICode,
                    Designation = list.Designation
                });
            }
            ViewBag.Dist = implst;

            return Json(new SelectList(ViewBag.Dist, "DesignationICode", "Designation"), JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TaskAdd(MeetingViewModel meetingViewObj, FormCollection frm, HttpPostedFileBase[] uploadFile)
        {
            createMeetingTask meetingTaskObj = new createMeetingTask();

            string meetingDepartment = meetingViewObj.mmsMeetingMaster.Meeting_Department == null ?
                                       Request["Meeting_other_dept"] : meetingViewObj.mmsMeetingMaster.Meeting_Department;
            meetingTaskObj.empId = Request.Form["Participant"].Split(',');
            meetingTaskObj.priority = Request.Form["Priority"].Split(',');
            var taskval = Request.Form["task"];
            var trimtask = taskval.Trim();
            meetingTaskObj.comments = Request.Form["Comments"].Split(',');
            meetingTaskObj.task = Request.Form["task"].Split(',');

            // meetingTaskObj.reviewFreq = Request.Form["frequency_1"].Split(',');
            // meetingTaskObj.reviewdate = Request.Form["revDt"].Split(',');

            meetingTaskObj.completionDate = Request.Form["Completion_Date"].Split(',');
            string[] k = Regex.Split(trimtask, "~,");
            string[] task = new string[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                if (i == k.Length - 1 && k[i].Contains("~"))
                {
                    task[i] = Regex.Replace(k[i], "~", "");
                    task[i] = task[i].Trim();
                }
                else
                {

                    task[i] = k[i].Trim();
                }
            }

            // meetingTaskObj.task = task;

            meetingTaskObj.category = Request.Form["Category"].Split(',');
            meetingTaskObj.files = Request.Form["files"].Split(',');
            int reviewF = Convert.ToInt32(Request.Form["frequency_"]);
            meetingTaskObj.reviewFreq = Request.Form["frequency"].Split(',');

            meetingTaskObj.reviewdate = Request.Form["ReviewDate"].Split(',');

            //Modified to correct Date Format Issue
            DateTime dt = DateTime.Now;
            String strDate = "";
            strDate = dt.ToString("MM/dd/yyyy");
            String formatDate = strDate.Replace('-', '/');

            if (meetingDepartment != null)
            {
                var TaskTemplate = (from resMTemplate in db.MMS_Meeting_Template
                                    where resMTemplate.department == meetingDepartment
                                    select new
                                    {
                                        MeetingTime = resMTemplate.meeting_time,
                                        MeetingDuration = resMTemplate.meeting_duration,
                                        MeetingObjective = resMTemplate.objective,
                                        MeetingVenue = resMTemplate.meeting_venue
                                    }).FirstOrDefault();

                meetingViewObj.mmsMeetingMaster.Meeting_Time = TaskTemplate.MeetingTime;
                meetingViewObj.mmsMeetingMaster.Meeting_Duration = TaskTemplate.MeetingDuration;
                meetingViewObj.mmsMeetingMaster.Meeting_Objective = TaskTemplate.MeetingObjective;
                meetingViewObj.mmsMeetingMaster.Meeting_Venue = TaskTemplate.MeetingVenue;
            }
            else
            {
                meetingViewObj.mmsMeetingMaster.Meeting_Time = "0.00";
                meetingViewObj.mmsMeetingMaster.Meeting_Duration = "0.00";
                meetingViewObj.mmsMeetingMaster.Meeting_Objective = "Test Task";
                meetingViewObj.mmsMeetingMaster.Meeting_Venue = string.Empty;
            }


            meetingViewObj.mmsMeetingMaster.Meeting_Date = DateTime.ParseExact(formatDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            meetingViewObj.mmsMeetingMaster.Meeting_Chairperson = Convert.ToInt16(Session["LoggedUserId"]);
            meetingViewObj.mmsMeetingMaster.Meeting_Department = Request["DepartmentTypeID"];
            meetingViewObj.mmsMeetingMaster.Meeting_Type = string.Empty;
            meetingViewObj.mmsMeetingMaster.Minutes_Of_Meeting = string.Empty;


            try
            {
                int? res = CreateMeetingViewModel.InsertMeeting1(meetingViewObj, meetingDepartment, meetingTaskObj, Convert.ToInt16(Session["LoggedUserId"]), false, uploadFile);
                //TempData["Success"] = "Inserted and Mailed Successfully";
                TempData["Message"] = "Task Created Successfully";
                //ViewBag.Message = "Inserted and Mailed Successfully";
            }

            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTaskParticipants(string deptName)
        {
            var deptId = CreateMeetingViewModel.GetParticipants(deptName);
            return Json(deptId, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TaskSummary(int? id, DateTime? selectD, string CategoryItem)
        {
            var today = DateTime.Now;
            var startDate = new DateTime(2017, 3, 1);
            var endDate = today;
            ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");
            // ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");


            ViewBag.Participants = MeetingDetailsViewModel.SelectParticipants(Convert.ToInt32(Session["LoggedUserId"]));

            // End Date 
            if (selectD != null)
            {
                Session["ViewBag.EndDate"] = Convert.ToDateTime(selectD);
            }
            if (Session["ViewBag.EndDate"] != null)
            {
                //ViewBag.StartDate = Convert.ToDateTime(selectDates);
                ViewBag.EndDate = Session["ViewBag.EndDate"];
            }
            else
            {
                //Convert.ToDateTime(startDate.ToString("dd/MMM/yyyy"));
                ViewBag.EndDate = Convert.ToDateTime(endDate.ToString("dd/MMM/yyyy"));
            }
            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            ViewBag.Status = DepartmentCategoryModel.TaskStatus();
            return View();
        }


        //First Report
        /// <summary>
        /// TIMETAKENFORCOMPLETIONREPORT
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeTakenForCompletion_Report(DateTime? endD)
        {
            var today = DateTime.Now;
            var start_Date = new DateTime(2017, 3, 1);
            var end_Date = today;
            ViewBag.StartDate = start_Date.ToString("dd/MMM/yyyy");
            //ViewBag.EndDate = end_Date.ToString("dd/MMM/yyyy");


            if (endD != null)
            {
                Session["ViewBag.EndDate"] = Convert.ToDateTime(endD);
            }
            if (Session["ViewBag.EndDate"] != null)
            {
                //ViewBag.StartDate = Convert.ToDateTime(selectDates);
                ViewBag.EndDate = Session["ViewBag.EndDate"];
            }
            else
            {
                //Convert.ToDateTime(startDate.ToString("dd/MMM/yyyy"));
                ViewBag.EndDate = Convert.ToDateTime(end_Date.ToString("dd/MMM/yyyy"));
            }

            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            ViewBag.Participants = MeetingDetailsViewModel.SelectParticipants(Convert.ToInt32(Session["LoggedUserId"]));

            return View();
        }

        //Second Report
        /// <summary>
        /// COMPLETIONEFFECTIVENESSREPORT
        /// </summary>
        /// <returns></returns>
        public ActionResult Completion_Effectiveness_Report(DateTime? end)
        {
            var today = DateTime.Now;
            var start_Date = new DateTime(2017, 3, 1);
            var end_Date = today;
            ViewBag.StartDate = start_Date.ToString("dd/MMM/yyyy");
            //ViewBag.EndDate = end_Date.ToString("dd/MMM/yyyy");


            if (end != null)
            {
                Session["ViewBag.EndDate"] = Convert.ToDateTime(end);
            }
            if (Session["ViewBag.EndDate"] != null)
            {
                //ViewBag.StartDate = Convert.ToDateTime(selectDates);
                ViewBag.EndDate = Session["ViewBag.EndDate"];
            }
            else
            {
                //Convert.ToDateTime(startDate.ToString("dd/MMM/yyyy"));
                ViewBag.EndDate = Convert.ToDateTime(end_Date.ToString("dd/MMM/yyyy"));
            }


            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            ViewBag.Participants = MeetingDetailsViewModel.SelectParticipants(Convert.ToInt32(Session["LoggedUserId"]));
            return View();
        }

        [HttpPost]
        public JsonResult AddUser(string departmentName, string userName, string emailID, string FirstName, string LastName, string Password, int DesignationICode)
        {
            //int Department_Code = 0; //
            //int companyICode = 1; //
            var departmentICode = db.Department_Master.Where(x => x.DepartmentName == departmentName).Select(x => x.DepartmentICode).FirstOrDefault();

            string employeeFirstName = FirstName;
            string employeeLastName = LastName;
            string employeeDisplayName = FirstName + ' ' + LastName;
            string employeeCorporateEmailId = emailID;
            string loginUserName = userName;
            string loginPassword = Password;
            //Boolean isActive = true;
            DateTime createdDate = DateTime.Now;
            int designationICode = DesignationICode; //
            DateTime employee_DateofJoin = DateTime.Now;

            dynamic Employee_AddRes = null;
            using (db = new MMSCGVAKDBEntities())
            {
                //try
                //{
                //    //Employee_AddRes = db.AddEmployee_MMS(companyICode, departmentICode, employeeFirstName, employeeLastName, employeeDisplayName, employeeCorporateEmailId, loginUserName, loginPassword, isActive, createdDate, designationICode, employee_DateofJoin, departmentName);
                //}
                //catch (Exception ex)
                //{

                //}
                MailFunctionModel.EmailEmployee_Added(emailID, FirstName, LastName, Password, userName, departmentName);
                return Json(Employee_AddRes, JsonRequestBehavior.AllowGet);

                // MailFunctionModel.Employee_Mail(employeeMail, meetingTask, meetingComments, MailPerson, commentedBy);

                //return Json(new { Result = "Success", EmployeeCode = employeeMaster.employee_icode, UserName = userName, Message = employeeMaster != null ? "Saved Successfully" : "Please try after some time" }, JsonRequestBehavior.AllowGet);

            }

        }

        #region Sync Users

        /// <summary>
        /// Sync users from Service to our DB.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StartUserSync()
        {
            var employeesToSync = CreateMeetingViewModel.GetEmployees();
            var synchedEmployees = CreateMeetingViewModel.GetSynchedEmployees();
            bool result = true;
            var synchedUsersIDList = synchedEmployees.Select(x => x.employee_icode).ToList();

            var usersToSync = employeesToSync.Where(x => !synchedUsersIDList.Contains(x.employee_icode)).ToList();
            foreach (var item in usersToSync)
            {
                var employeeMail = CreateMeetingViewModel.GetEmployeeEmailByCode(item.employee_icode);
                var employeeDepartment = CreateMeetingViewModel.AddEmployeeDepartment(item.username, item.Department_iCode, employeeMail, item.employee_icode);
                if (employeeDepartment == null)
                    result = false;
            }

            return Json(new { Result = result ? "Success" : "Failure", Message = result ? "Employees Synched successfully" : "Employees not synched. Please try again" }, JsonRequestBehavior.AllowGet);
            //var synchedEmployees=
        }

        #endregion

        #region CheckEmailExists

        public JsonResult CheckEmailExits(string emailID, string username)
        {
            var userexistCheck = db.Employee_Master.Where(u => u.LoginUserName.ToLower() == username.ToLower()).FirstOrDefault();
            if (userexistCheck == null)
            {
                var employeeMaster = CreateMeetingViewModel.GetEmployeeByEmail(emailID, username);
                if (employeeMaster == null)
                    return Json(new { EmployeeCode = "0" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { EmployeeCode = employeeMaster.employee_icode }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { EmployeeCode = "-1" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
