using MMS.Models;
using MMS.Models.ApiModel;
using MMS.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MMS.Controllers
{

    [SessionExpire]
    [HandleError]
   
    public class MeetingDetailsController : Controller
    {
        private AttachmentViewModel attachmentOb = new AttachmentViewModel();

        public MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public ActionResult Index(int? id)
        {
            LoginViewModel model = new LoginViewModel();
            var userId = LoginViewModel.Login(model);


            Session["LoggedUserId"] = userId;

            //categories list 
            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();            
            ViewBag.Participants = MeetingDetailsViewModel.SelectParticipants(Convert.ToInt32(Session["LoggedUserId"]));
            return View();
        }

        //added newly for VIEW Meetings
        [HttpGet]
        /*fetching meeting name for appending into dropdown */
        public JsonResult ViewMeeting(DateTime startDate, DateTime endDate, string meetingDepartment)
        {
            int schairPerson = Convert.ToInt32(Session["LoggedUserId"]);
            var Ret_meeting = MeetingDetailsViewModel.ViewMeeting(schairPerson, startDate, endDate.AddDays(1), meetingDepartment);
            return Json(Ret_meeting, JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpGet]
        public JsonResult MeetingFetchRes(string taskPeriod, DateTime startDate, DateTime endDate, int? category,
                                        int participants, int status, string priority) //Fetching meetingDetails  
        {            
            dynamic ReturnJsonResult = null;

            try
            {                
                int schairPerson = Convert.ToInt16(Session["LoggedUserId"]);
                ReturnJsonResult = MeetingDetailsViewModel.MeetingFetchRes(schairPerson, taskPeriod, startDate, endDate, Convert.ToInt32(category), participants, status, priority);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MeetingSummaryRes(string taskPeriod, DateTime startDate, DateTime endDate, int? template,
                                        int participants, int status, string priority) //Fetching meetingDetails  
        {

            dynamic ReturnJsonResult = null;

            try
            {
                
                int schairPerson = Convert.ToInt16(Session["LoggedUserId"]);

                ReturnJsonResult = MeetingDetailsViewModel.MeetingSummaryRes(schairPerson, taskPeriod, startDate, endDate, Convert.ToInt32(template), participants, status, priority);

            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]        
        public JsonResult ReportFetchRes(int? ChairPerson, DateTime? SDate, DateTime? EDate, int? Participant)
        {
            dynamic ReturnJsonResult = null;
            //AVERAGEDays = AVERAGEDays.ToString("dd/MMM/yyyy");
            try
            {
                if (Participant == 0)
                {
                    Participant = 0;
                }
                else
                {
                    int user = Convert.ToInt32(Participant);
                    Participant = Convert.ToInt32(db.MMS_Meeting_Details.Where(x => x.Employee_Id == user).Select(x => x.Participant.ToString()).FirstOrDefault());
                }
                ChairPerson = Convert.ToInt32(Session["LoggedUserId"]);                
                ReturnJsonResult = MeetingDetailsViewModel.ReportFetchRes(ChairPerson, SDate, EDate, Participant);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }

        #region TaskFetchRes

        public JsonResult TaskFetchRes(string taskPeriod, DateTime startDate, DateTime endDate, string category,
                                        string participants, string status, string priority) //Fetching meetingDetails  
        {
            dynamic ReturnJsonResult = null;

            try
            {
                var schairPerson = Session["LoggedEmployeeName"].ToString();
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }


        #endregion

        [HttpGet]
        public JsonResult MeetingFetchIndividualRes(string taskPeriod, DateTime startDate, DateTime endDate, int category,
                                       int participants, int status, string priority) //Fetching meetingDetails  
        {
            dynamic ReturnJsonResult = null;

            try
            {
                int schairPerson = Convert.ToInt32(Session["LoggedUserId"]);
                //Meetingsearch results will be added as object result through stored procedure call
                ReturnJsonResult = MeetingDetailsViewModel.MeetingFetchIndividualRes(schairPerson, taskPeriod, startDate, endDate, category, participants, status, priority);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MeetingSummaryIndividualRes(string taskPeriod, DateTime startDate, DateTime endDate, int template,
                                       int participants, int status, string priority) //Fetching meetingDetails  
        {
            dynamic ReturnJsonResult = null;

            try
            {
                int schairPerson = Convert.ToInt32(Session["LoggedUserId"]);
                //Meetingsearch results will be added as object result through stored procedure call
                ReturnJsonResult = MeetingDetailsViewModel.MeetingSummaryIndividualRes(schairPerson, taskPeriod, startDate, endDate, template, participants, status, priority);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }

        #region TaskFetchIndividualRes
        [HttpGet]
        public JsonResult TaskFetchIndividualRes(string taskPeriod, DateTime startDate, DateTime endDate, string category,
                                       string participants, string status, string priority) //Fetching meetingDetails  
        {
            dynamic ReturnJsonResult = null;

            try
            {
                var schairPerson = Session["LoggedUserId"].ToString();
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return Json(ReturnJsonResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpGet]
        public PartialViewResult EditMeetingDetails(int taskId, int? meetingId)
        {
            MeetingEditDetails MeetingObj = new MeetingEditDetails();
            try
            {
                var today = DateTime.Now;
                var startDate = new DateTime(today.Year, today.Month, 1);
                var endDate = today;
                ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");
                ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");
              
                var TaskItems = MeetingDetailsViewModel.editMeetingDetails(taskId, meetingId);              
                MeetingObj.PriorityTypesSelected = TaskItems.priority;
                ViewBag.Priority = TaskItems.priority;
                var searchFor = "";
                MeetingObj.meetingId = Convert.ToInt32(TaskItems.meetingId);
                MeetingObj.TaskID = taskId;
                MeetingObj.Task= (from e in db.MMS_Meeting_Details
                                  where e.Task_Id == taskId
                                  select e.Task).FirstOrDefault();
                MeetingObj.RespPerson = Convert.ToString(TaskItems.participant);

                MeetingObj.empId = Convert.ToInt16(TaskItems.employee_id);
                var ListMeetingdetails = db.sp_select_meeting_status(MeetingObj.TaskID).OrderByDescending(x => x.Meeting_Status_Date).ToList();
                DateTime tempDate = Convert.ToDateTime(TaskItems.completion_date);
                DateTime reviewDate = Convert.ToDateTime(TaskItems.reviewDate);
                MeetingObj.Comments = ListMeetingdetails.OrderByDescending(x => x.Meeting_Status_Date).Select(x => x.Meeting_Status_Comments).FirstOrDefault();
                MeetingObj.ReviewDate = reviewDate.ToString("dd/MMM/yyyy").Replace('-', '/');
                var ReviewDate = new DateTime(today.Year, today.Month, 1);
                ViewBag.ReviewDate = ReviewDate.ToString("dd/MMM/yyyy");

                MeetingObj.CompletionDate = tempDate.ToString("dd/MMM/yyyy").Replace('-', '/');
                var CompletionDate = new DateTime(today.Year, today.Month, 1);
                ViewBag.CompletionDate = CompletionDate.ToString("dd/MMM/yyyy");
                MeetingObj.selectedCategory = Convert.ToString(TaskItems.category);
                MeetingObj.selectedCategoryID = TaskItems.category;
                MeetingObj.TaskID = taskId;              
                ViewBag.SelectedStatus = DepartmentCategoryModel.TaskStatus();
                ViewBag.Rev_Frequency = new SelectList(db.MMS_Review_Frequency, "Freq_In_Days", "Freq_Name", TaskItems.reviewfreq);               
                MeetingObj.Status = TaskItems.status;
                searchFor = MeetingObj.selectedCategory;
                int categoryID = Convert.ToInt32(searchFor);
               
                string selected = (from sub in db.MMS_Meeting_Category
                                   where sub.ID == categoryID
                                   select sub.CategoryName).FirstOrDefault();

                ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();//new SelectList(db.MMS_Meeting_Category, "ID", "CategoryName", selected);
                ViewBag.Attachments = attachmentOb.GetAttachmentFile(meetingId);              
            }
            catch(Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return PartialView(MeetingObj);
        }
            
        [HttpPost]
        public ActionResult DeleteMeetingDetails(int taskId, int? meetingId)
        {

            string ResMeetingMsg = string.Empty;
            try
            {
                ResMeetingMsg = MeetingDetailsViewModel.deleteMeetingDetails(taskId, meetingId);

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            return Json(ResMeetingMsg, JsonRequestBehavior.AllowGet);
        }

        //added   [HttpGet]
        [HttpGet]
        //update meeting details will be called from meeting details & edit task page
        public JsonResult UpdateMeetingDetails1(int respPerson, DateTime? date, int taskStatus, int category, string comments, int meetingId, int empId, string task, int taskId, int percentage, int commented_by, int ReviewFrequency, string Priority)
        {
            string resMeetingMsg = string.Empty;
            try
            {
                resMeetingMsg = MeetingDetailsViewModel.UpdateMeetingDetails1(respPerson, date, taskStatus, category, comments, meetingId, empId, task, taskId, percentage, commented_by, ReviewFrequency, Priority);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(resMeetingMsg, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //update meeting details will be called from meeting details & edit task page        
        public JsonResult UpdateMeetingDetails(int? respPerson, DateTime? date, int taskStatus, string category, string comments, DateTime? revdate, int meetingId, int empId, string task, int taskId, int? ReviewFrequency, string Priority)
        {
           string resMeetingMsg = string.Empty;            
           if(Priority== "AllHigh" || Priority == "All" || Priority == "undefined")
            {
                Priority = "High";
            }
            if (Priority == "AllLow")
            {
                Priority = "Low";
            }
            if (Priority == "AllMedium")
            {
                Priority = "Medium";
            }
            
            try
            {
                resMeetingMsg = MeetingDetailsViewModel.UpdateMeetingDetails(Convert.ToInt16(respPerson), date, taskStatus, Convert.ToInt16(category), comments, revdate, meetingId, empId, task, taskId, Convert.ToInt32(Session["LoggedUserId"]), Convert.ToInt16(ReviewFrequency), Priority);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return Json(resMeetingMsg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMainMeetingDetails(MeetingDetailModel meetingdetails, List<TaskDetailModel> taskdetails, List<UpdateMeetingTask> updateddetails) 
        {
            string resMeetingMsg = string.Empty;
            try
            {
                resMeetingMsg = MeetingDetailsViewModel.UpdateMainMeetingDetails(meetingdetails, taskdetails, updateddetails);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return Json(resMeetingMsg, JsonRequestBehavior.AllowGet);
        }

        //comments will be appended with stringbuilder class
        [AcceptVerbs(HttpVerbs.Post)]
        public StringBuilder GetCommentsListByMail(int meetingId, int taskId, int respPerson, int employeeID, string meetingStatus)
        {
            StringBuilder strHtml = new StringBuilder();
            strHtml = MeetingDetailsViewModel.GetCommentsListByMail(meetingId, taskId, respPerson, employeeID, meetingStatus);
            return strHtml;
        }

        //getCommentsList called through json
        public string GetCommentsList(int meetingId,  string respPerson, int empId, string meetingStatus, int taskId)
        {
            StringBuilder strHtml = new StringBuilder();
            try
            {
                int resultMeetingStatus =  db.Task_Status.Where(x => x.Name == meetingStatus).Select(x => x.ID).FirstOrDefault();
                strHtml = MeetingDetailsViewModel.GetCommentsList(meetingId,  respPerson, empId, resultMeetingStatus.ToString(), taskId);                
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return strHtml.ToString();
        }

        //Adding comments for individual comment
        public JsonResult AddComments(int? meetingStatusEmpID, string meetingTask, string meetingComments, int? respPerson, int? meetingId, int? meetingStatus, string commentedBy, int? commentedByID, string currentMail, int? taskId)
        {
            string retMsg = string.Empty;
            int cnt = 0;        

            try
            {
                cnt = MeetingDetailsViewModel.AddComments(meetingStatusEmpID, meetingTask, meetingComments, respPerson, meetingId, meetingStatus, commentedBy, commentedByID, currentMail, taskId ?? 0);

                if (cnt > 0)
                    retMsg = "Updated and Mail has been sent";
                else
                    retMsg = "Updation Error";
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(retMsg, JsonRequestBehavior.AllowGet);
        }

        //Emailcomments called through ajax call 
        public JsonResult EmailComments(string emailAddress, string cc, string subject, string bodyContent, int? respPerson, string taskStatus, string commentedBy, string currentMail)
        {
            var retMsg = string.Empty;
            int mailresp = 0;

            try
            {
                var resPerson = db.Employee_Master.Where(c => c.EmployeeICode == respPerson).Select(c=>c.EmployeeDisplayName).FirstOrDefault();
                //mailresp return 1 the Mail Send Sucess or return -1 Not success
                mailresp = MailFunctionModel.CommentsMail(emailAddress, subject, bodyContent, resPerson, commentedBy, cc);

                retMsg = "Updated and Mail has been sent";

            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(retMsg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //for closing the comments ajax call
        public ActionResult CloseComments(int? taskId, int? meetingId)
        {
            var resultComments = string.Empty;

            try
            {
                resultComments = MeetingDetailsViewModel.CloseComments(Convert.ToInt32(taskId), meetingId);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(resultComments, JsonRequestBehavior.AllowGet);
        }

        //Meeting counts will be displayed through ajax call
        public JsonResult GetCounts(int meetingId, int employeeId, int participant, int task_id, string comments)
        {
            var selectCount = MeetingDetailsViewModel.GetCounts(meetingId, employeeId, participant, task_id);
            return Json(selectCount, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<DDLViewModel> PriorityTypes()
        {
            List<DDLViewModel> lstPriorityTypes = new List<DDLViewModel>();
            lstPriorityTypes.Add(new DDLViewModel() { ItemId = 0, ItemName = "Low" });
            lstPriorityTypes.Add(new DDLViewModel() { ItemId = 1, ItemName = "Medium" });
            lstPriorityTypes.Add(new DDLViewModel() { ItemId = 2, ItemName = "High" });
            return lstPriorityTypes;
        }

        [HttpGet]
        public JsonResult EffectivenessFetchRes(string ChairPerson, DateTime? SDate, DateTime? EDate, string Participant)
        {
            dynamic ReturnJsonResults = null;
            try
            {
                if (Participant == "0")
                {
                    Participant = "0";
                }
               
                ChairPerson = Session["LoggedUserId"].ToString();
                ReturnJsonResults = MeetingDetailsViewModel.EffectivenessFetchRes(Convert.ToInt16(ChairPerson), SDate, EDate, Participant);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }
            return Json(ReturnJsonResults, JsonRequestBehavior.AllowGet);
        }
    }
}