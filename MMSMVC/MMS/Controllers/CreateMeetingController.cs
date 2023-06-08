


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Security;
using MMS.Models;
using System.Net.Mail;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]

    public class CreateMeetingController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public ActionResult Index(int? MeetingID)
        {
            var Meetingname = "";
            var Meetingobjective = "";
            var today = DateTime.Now;
            var endDate = today;
            ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");


            ViewBag.Departments = DepartmentCategoryModel.DepartmentMasterList();
            ViewBag.Priority = DepartmentCategoryModel.TaskPriority();
            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();            

            if (MeetingID != 0 && MeetingID != null)
            {
                Meetingname = db.MMS_Meeting_Template.Where(x => x.tid == MeetingID).Select(x => x.meeting_name).FirstOrDefault().ToString();
                Meetingobjective = db.MMS_Meeting_Template.Where(x => x.tid == MeetingID).Select(x => x.objective).FirstOrDefault().ToString();

                ViewBag.selectedMeetingobjective = Meetingobjective;                
                ViewBag.selectedMeeting = Meetingname;
            }            
            ViewBag.Template = Template();
            
            return View();
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Index(MeetingViewModel meetingviewObj, FormCollection frm, HttpPostedFileBase[] uploadFile)
        {
            createMeetingTask meetingTaskObj = new createMeetingTask();
            string meetingDepartment = meetingviewObj.mmsMeetingMaster.Meeting_Department == null ?
                                        Request["Meeting_other_dept"] : meetingviewObj.mmsMeetingMaster.Meeting_Department;

            meetingTaskObj.task = Request.Form["task"].Split(',');
            meetingTaskObj.empId = Request.Form["Added_Participants"].Split(',');
            meetingTaskObj.TaskChairman= Request.Form["Participant"].Split(',');
            int count = frm["Added_Participants"].Split(',').Length;
            var chairpers = Request.Form["Meetmaster"];

            var chairpersonid = (from x in db.Employee_Master where x.EmployeeDisplayName == chairpers select x.EmployeeICode).FirstOrDefault();

            if (chairpersonid == 0)
            {
                chairpersonid = Convert.ToInt32(chairpers);
            }

            meetingTaskObj.priority = Request.Form["Priority"].Split(',');
            meetingTaskObj.comments = Request.Form["Comments"].Split(',');
            meetingTaskObj.completionDate = Request.Form["Completion_Date"].Split(',');
            meetingTaskObj.category = Request.Form["Category"].Split(',');
            meetingTaskObj.files = Request.Form["files"].Split(',');
            meetingTaskObj.reviewdate = Request.Form["reviewdate"].Split(',');
            meetingTaskObj.reviewFreq = Request.Form["reviewFreq"].Split(',');
            meetingviewObj.mmsMeetingMaster.Meeting_Chairperson = Convert.ToInt32(chairpersonid);
            DateTime formatDate = Convert.ToDateTime(Request.Form["mmsMeetingMaster.Meeting_Date"]);
            meetingviewObj.mmsMeetingMaster.Meeting_Date = DateTime.Parse(formatDate.ToString("dd/MMM/yyyy"), CultureInfo.InvariantCulture);
            meetingviewObj.mmsMeetingMaster.Meeting_No_Of_Participants = count;            

            try
            {
                int? res = CreateMeetingViewModel.InsertMeeting(meetingviewObj, meetingDepartment, meetingTaskObj, Convert.ToInt32(Session["loggedUserId"]), false, uploadFile);
                TempData["Success"] = "Inserted and Mailed Successfully";


            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return RedirectToAction("Index");
        }

        public List<MMS_Meeting_Template> Template()
        {
            using (MMSCGVAKDBEntities db = new MMSCGVAKDBEntities())
            {
                var template_list = db.MMS_Meeting_Template.OrderBy(x=>x.meeting_name).ToList();
                return template_list;
            }
        }

        [HttpGet]
        public ActionResult MeetingSummary(int? id, DateTime? selectD, string CategoryItem)
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

            //var NonTemplate = db.MMS_Meeting_Master.Where(x => x.istemplate == false).Select(x => x.Meeting_Objective).ToList();

            ViewBag.Template = db.MMS_Meeting_Template.OrderBy(x => x.meeting_name).ToList();
            //ViewBag.Template = NonTemplate;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ObjectiveSelected(string selectedObjective)
        {
          

            var objSelected = CreateMeetingViewModel.FetchTemplate(selectedObjective);
            return Json(objSelected, JsonRequestBehavior.AllowGet);
        }
        //For View/Edit Created Meeting--development in progress
        public ActionResult ViewMeeting(DateTime? select)
        {
            var today = DateTime.Now;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = today;
            ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");            

            // End Date 
            if (select != null)
            {
                Session["ViewBag.EndDate"] = Convert.ToDateTime(select);
            }
            if (Session["ViewBag.EndDate"] != null)
            {                
                ViewBag.EndDate = Session["ViewBag.EndDate"];
            }
            else
            {                
                ViewBag.EndDate = Convert.ToDateTime(endDate.ToString("dd/MMM/yyyy"));
            }


            ViewBag.chair = Session["LoggedEmployeeName"].ToString();
            ViewBag.Departments = DepartmentCategoryModel.DepartmentMasterList();
            return View();
        }


        /*task results passed to EditViewMeetingDetails page */
        [HttpGet]
        public PartialViewResult EditViewMeetingDetails(int? meetingID)

        {
            string tempSTR = string.Empty;
            string formatSTR = string.Empty;
            int colonPlace;

            DateTime StartTime = DateTime.ParseExact("09:00", "HH:mm", null);

            DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);

            TimeSpan Interval = new TimeSpan(00, 15, 0);

            List<string> timelist = new List<string>();
            while (StartTime <= EndTime)
            {
                tempSTR = StartTime.ToShortTimeString();
                colonPlace = tempSTR.IndexOf(':');
                if (colonPlace == 1)
                {
                    formatSTR = tempSTR.Insert(0, "0");
                    timelist.Add(formatSTR);
                }
                else
                {
                    timelist.Add(tempSTR);
                }

                StartTime = StartTime.Add(Interval);
            }

             List<SelectListItem> tmList = new List<SelectListItem>();          
            var timeget = (from x in db.MMS_Meeting_Master where x.Meeting_Id == meetingID select x.Meeting_Time).FirstOrDefault();
            foreach (var item in timelist)
            {
                tmList.Add(new SelectListItem()
                {
                    Text = item,
                    Value = item,
                    Selected = (item == timeget.ToString() ? true : false)
                });
            }

            ViewBag.newviewbag = timeget;
            ViewBag.timeLIST = tmList;
            
            DateTime StartHrs = DateTime.ParseExact("01:00", "HH:mm", null);
            DateTime EndHrs = DateTime.ParseExact("12:00", "HH:mm", null);
            TimeSpan IntervalHrs = new TimeSpan(00, 30, 0);
            List<string> hrslist = new List<string>();

            while (StartHrs <= EndHrs)
            {
                tempSTR = StartHrs.ToShortTimeString();

                if (tempSTR.Contains("AM"))
                {
                    formatSTR = tempSTR.Replace("AM", string.Empty);
                    formatSTR = formatSTR.TrimEnd();
                    colonPlace = formatSTR.IndexOf(':');
                    if (colonPlace == 1)
                    {
                        formatSTR = formatSTR.Insert(0, "0");
                    }
                    formatSTR += ":00";

                }
                else
                {
                    formatSTR = tempSTR.Replace("PM", string.Empty);
                    formatSTR = formatSTR.TrimEnd();

                    formatSTR += ":00";
                }

                hrslist.Add(formatSTR);

                StartHrs = StartHrs.Add(IntervalHrs);
            }

            ViewBag.hrslist = hrslist;
            
            ViewBag.Departments = DepartmentCategoryModel.DepartmentMasterList();
            ViewBag.Priority = DepartmentCategoryModel.TaskPriority();
            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            ViewUpdateMeetingDetails MeetingObj = new ViewUpdateMeetingDetails();            

            var UpdatedFile = (from files in db.MMS_Attachment
                                where files.Meeting_Id == meetingID
                                select new
                                {
                                    FileName = files.MMS_FileName
                                });
            ViewBag.AttachFiles = UpdatedFile;
            ViewBag.viewandsave = 1;
            var meetingItems = CreateMeetingViewModel.EditMeetingDetails(meetingID);                      
            var TaskResult = (from l in db.MMS_Meeting_Details
                              join c in db.Employee_Master on l.Participant equals c.EmployeeICode
                              join mc in db.MMS_Meeting_Category on l.Category equals mc.ID
                              join mp in db.MMS_Meeting_Participant on l.Meeting_Id equals mp.Meeting_Id
                              where l.Meeting_Id == meetingID
                              select  new 
                               {
                                   Task = l.Task,
                                   Taskid = l.Task_Id,
                                  EmpName= c.EmployeeDisplayName,
                                  CatName=mc.CategoryName,
                                 Priority = l.Priority,
                                 CompletionDate=l.Completion_Date,
                                 Comments=l.Comments,
                                 ResPerson=l.Employee_Id,
                                 Category=l.Category
                              }).ToList().Distinct();
            ViewBag.Task = TaskResult;

            var participantsItems = CreateMeetingViewModel.EditMeetingParticipants(meetingID);

            var searchmeetingDuration = "";
            foreach (var resItems in meetingItems)
            {
                MeetingObj.meetingId = meetingID;

                MeetingObj.Chairperson = resItems.Chairperson;
                int meetingchairPerson = Convert.ToInt16(MeetingObj.Chairperson);

                MeetingObj.Chairperson = db.Employee_Master.Where(x => x.EmployeeICode == meetingchairPerson).Select(x => x.EmployeeDisplayName).FirstOrDefault();

                MeetingObj.meetingTime = resItems.meetingTime;

                MeetingObj.meetingVenue = resItems.meetingVenue;

                MeetingObj.meetingDuration = resItems.meetingDuration;
                searchmeetingDuration = MeetingObj.meetingDuration;

                MeetingObj.meetingType = resItems.meetingType;

                MeetingObj.meetingObjective = resItems.meetingObjective;

                MeetingObj.minutesOfMeeting = resItems.minutesOfMeeting;

                MeetingObj.meetingNumParticipants = resItems.meetingNumParticipants;

                MeetingObj.meetingDepartment = resItems.meetingDepartment;


                MeetingObj.meetingDate = resItems.meetingDate.ToString("dd-MMM-yyyy");                
            }
            ViewBag.SelectedmeetingDuration = new SelectList(hrslist, searchmeetingDuration);

            int[] intArr = new int[participantsItems.Count];
            string[] strArr = new string[participantsItems.Count];

            for (int i = 0; i < participantsItems.Count; i++)
            {
                intArr[i] = participantsItems[i].ParticipantID;
                strArr[i] = participantsItems[i].Participant_Name;

            }

            MeetingObj.ParticipantID = intArr;
            MeetingObj.Participant_Name = strArr;


            int? TemplateID = db.MMS_Meeting_Template.Where(x => x.objective == MeetingObj.meetingType).Select(x => x.tid).FirstOrDefault();
            ViewBag.Templateid = TemplateID;
            Dictionary<int, string> dStudent = new Dictionary<int, string>();            
            ViewBag.participantList = participantsItems;//participantList;
           
            return PartialView(MeetingObj);
        }
    }
}