using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Security;
using MMS.Models;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Data.Entity.Validation;


namespace MMS.Controllers
{

    [SessionExpire]
   
    public class MyTasksController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();

        [HttpGet]
        public ActionResult Index(int? meetingId, DateTime? selectDates)
     {
            var today = DateTime.Now;

            var endDate = today;
            //var startDate = new DateTime(today.Year, today.Month, 1);
            var startDate = new DateTime(2017, 3, 1);

            if (meetingId.HasValue)
            {
                MeetingDetailsViewModel.UpdateNotification(meetingId.Value);
            }
            //ViewData["ViewBag.StartDate"] = selectDates;
            if (selectDates != null)
            {
                Session["ViewBag.StartDate"] = Convert.ToDateTime(selectDates);
            }

            //if (selectDates == startDate)
            if (Session["ViewBag.StartDate"] != null)
            {
                //ViewBag.StartDate = Convert.ToDateTime(selectDates);
                ViewBag.StartDate = Session["ViewBag.StartDate"];
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate.ToString("dd/MMM/yyyy"));
            }

            //var date_element
            ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");
            ViewBag.CategoryTypeID = DepartmentCategoryModel.CategoryMasterList();
            ViewBag.ReviewFrequencyID = DepartmentCategoryModel.ReviewFrequency();

            return View();
        }

        [HttpPost]       
        public JsonResult FetchMyTasks(DateTime? startDate, DateTime? endDate, int? categoryId, int? frequencyId)
        {

            var RetMsg = string.Empty;

            try
            {
                //Fetch meeting details of own tasks
                var Taskgrid = MyTasksViewModel.FetchMyTasks(startDate, endDate.Value.AddDays(1), Convert.ToInt16(categoryId), Convert.ToInt16(frequencyId));

                return Json(Taskgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(RetMsg, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult MeetingStatusUpdate(int? meetingId, int taskId)
        {
            MeetingEditDetails MeetingObj = null;

            //update read notification
            //MeetingDetailsViewModel.UpdateNotification(meetingId);

            //var getTaskDetails = MyTasksViewModel.MeetingStatusUpdate(meetingId);
            var getTaskDetail = MyTasksViewModel.GetTaskByID(taskId);          //Change to Task on 17Aug2017

            //ToInt16 changes as ToInt32 in meetingId & empId - Charles S / 03-Aug-2017

            //foreach (var res in getTaskDetails)
            //    {

            if (getTaskDetail != null)
            {
                int chairPerson_getTask = getTaskDetail.chairPerson;
                int Participant_getTask = getTaskDetail.participant;
                MeetingObj = new MeetingEditDetails
                {
                    meetingId = Convert.ToInt32(meetingId),
                    Task = getTaskDetail.task.Replace("&nbsp;", ""),
                    RespPerson = Convert.ToInt32(getTaskDetail.participant).ToString(),
                    //RespPerson = db.Employee_Master.Where(x => x.EmployeeICode == Participant_getTask).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                    empId = Convert.ToInt32(getTaskDetail.employee_id),
                    CompletionDate = Convert.ToDateTime(getTaskDetail.completion_date).ToString("dd/MMM/yyyy"),
                    Priority = getTaskDetail.priority,
                    selectedCategory = getTaskDetail.category.ToString(),

                    Status = getTaskDetail.status, //Commented By Thangavel

                    Comments = getTaskDetail.comments,
                    TaskID = getTaskDetail.task_id,
                    ChairPerson = Convert.ToString(getTaskDetail.chairPerson),
                    //ChairPerson = db.Employee_Master.Where(x => x.EmployeeICode == chairPerson_getTask).Select(x => x.EmployeeDisplayName).FirstOrDefault()
                };


                var statusList = db.Task_Status.ToList();
                
                statusList = statusList.Where(x => x.Name != "CLOSED").ToList();
                string q = string.Empty;
                //if (Session["LoggedEmployeeName"].ToString().Trim() != MeetingObj.ChairPerson.Trim())
                if (Session["LoggedUserId"].ToString().Trim() != MeetingObj.ChairPerson.Trim())
                {
                    //statusList = statusList.Where(x => !x.Name.ToLower().Equals("closed")).ToList();
                    statusList = statusList.Where(x => x.Name != "CLOSED").ToList();
                }


                //ViewBag.Name = new SelectList(context.Roles.Where(u => u.Name != "Admin" && u.Name != "Customer")).ToList(), "Name", "Name");

                ViewBag.SelectedStatus = new SelectList(statusList, "ID", "Name");

                ViewBag.Priorities = GetPriorities();
                ViewBag.Status = getTaskDetail.status;
                Session["taskId"] = getTaskDetail.task_id;
                MMS_Meeting_Status mms_meeting_status = new MMS_Meeting_Status();
                mms_meeting_status.Commented_By = getTaskDetail.chairPerson;
                //Obtaining the name of the chairPerson to display it...
                int chairpersonName = getTaskDetail.chairPerson;
                //mms_meeting_status

                db.Entry(mms_meeting_status).State = System.Data.Entity.EntityState.Modified;
                //}
            }

            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            if (MeetingObj.Comments != null)
                //keep changing this
                // ViewBag.CommentsList = MyTasksViewModel.ShowComments(meetingId);   //Commented. Gokul Thangavel 18-Aug-2017
                ViewBag.CommentsList = MyTasksViewModel.GetCommentsByTaskId(taskId);
            if (Session["LoggedUserId"] == null)
                return RedirectToAction("Login", "Account");
            return View(MeetingObj);
        }


        private List<SelectListItem> GetPriorities()
        {
            List<SelectListItem> priorities = new List<SelectListItem> {
             new SelectListItem { Text="Select", Value=""},
             new SelectListItem { Text="High", Value="High"},
             new SelectListItem { Text="Medium", Value="Medium"},
             new SelectListItem { Text="Low", Value="Low"}
         };
            return priorities;
        }

        [HttpGet]
        public ActionResult NotificationChanges(int meetingId)
        {
            var today = DateTime.Now;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = today;
            ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");
            ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");

            bool Success = MyTasksViewModel.CheckExistingUser(meetingId);

            if (Success)
            {
                if (Session["LoggedUserId"] != null)
                {
                    MeetingEditDetails MeetingObj = null;

                    //update read notification
                    MeetingDetailsViewModel.UpdateNotification(meetingId);

                    var getTaskDetails = MyTasksViewModel.MeetingStatusUpdate(meetingId);

                    List<string> sb = new List<string>();

                    if (getTaskDetails.Count > 0)
                    {
                        foreach (var res in getTaskDetails)
                        {
                            MeetingObj = new MeetingEditDetails
                            {
                                meetingId = Convert.ToInt32(meetingId),
                                Task = res.task,
                                RespPerson = res.participant,
                                empId = Convert.ToInt32(res.employee_id),
                                CompletionDate = Convert.ToDateTime(res.completion_date).ToShortDateString(),
                                Priority = res.priority,
                                selectedCategory = res.category,
                                Comments = res.comments

                            };
                            Session["taskId"] = res.task_id;
                            if (MeetingObj.Comments != null)
                            {
                                sb.Add(MeetingObj.Comments);
                            }

                            ViewBag.Status = new SelectList(db.Task_Status, "Name", "Name");

                        }

                    }

                    ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
                    ViewBag.Comments = sb.ToList();

                    return View(MeetingObj);

                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }

            }
            else
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public JsonResult UpdateMeetingDetails(int empId, string task, int status, string comments, string completionDate, string respPerson, string commentedBy,
                int commentedById, int meetingId, string hours, int taskId, string priority)
        {

            var ResMeetingMsg = string.Empty;

            try
            {
                //ResMeetingMsg = MyTasksViewModel.UpdateMeetingTaskDetails(empId, task, status, comments, completionDate, respPerson, commentedBy,
                //                                                     commentedById, meetingId, hours, taskId, priority);

                ResMeetingMsg = MyTasksViewModel.UpdateMeetingTaskDetails(empId, task, status, completionDate, taskId, priority, comments);


                ResMeetingMsg = "Updated Successfully";
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

        public ActionResult PercentageUpdate(int meetingId)
        {
            Session["Task_Meeting_id"] = meetingId;
            return View();
        }

        [HttpGet]
        public ActionResult ShowComments(int meetingId, int taskId)
        {            
            var result = MyTasksViewModel.GetCommentsByTaskId(taskId);
            List<GetTaskComments_Result> CommentsRes = new List<GetTaskComments_Result>();            

            foreach (var res in result)
            {
                if (res.Comments != null && res.Comments != "")
                {
                    CommentsRes.Add(new GetTaskComments_Result { Comments = res.Comments, Commented_by = res.Commented_by, StatusDate = res.StatusDate });
                }
                CommentsRes.OrderByDescending(x => x.StatusDate).ToList();
            }

            var statusList = db.Task_Status.ToList();
           // if (Session["LoggedUserId"].ToString().Trim() != MeetingObj.ChairPerson.Trim())
            //{
                //statusList = statusList.Where(x => !x.Name.ToLower().Equals("closed")).ToList();
                statusList = statusList.Where(x => x.Name != "CLOSED").ToList();
            //}
            ViewBag.SelectedStatus = new SelectList(statusList, "ID", "Name");


           // ViewBag.SelectedStatus = MyTasksViewModel.GetMeetingTaskStatus(taskId);


            ViewBag.Status_Selected = MyTasksViewModel.GetMeetingTaskStatus(taskId);


            ViewBag.Status = new SelectList(db.Task_Status.Where(x=>x.Name!="CLOSED").ToList(), "ID", "Name");
            ViewBag.MeetingID = meetingId;
            ViewBag.TaskID = taskId;
            return View(CommentsRes.OrderByDescending(x => x.StatusDate).ToList());
        }

        [HttpPost]
        public JsonResult PercentageUpdateChanges(decimal percent, int meetingId, int taskID)
        {
            var ResJson = string.Empty;
            try
            {
                ResJson = MyTasksViewModel.PercentageUpdateChanges(percent, meetingId, taskID);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();
            }

            return Json(ResJson, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCounts(int meetingId, int taskid)
        {
           //int taskid = (from x in db.MMS_Meeting_Details  where x.Task == task && x.Meeting_Id==meetingId select x.Task_Id) .FirstOrDefault();
           

            string Status = "Completed";
            var selectCount = MyTasksViewModel.GetCounts(meetingId, taskid, Status);

            return Json(selectCount, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void AddComment(string comment, int status, int meetingId, int hours, int minutes, int taskId,string reviewsId,int percentage)
        {
            //if (status.ToUpper() == "TASK ASSIGNED" || status.ToUpper() == "COMPLETED" || status.ToUpper() == "RE-OPENED" || status.ToUpper() == "CLOSED")
            //{
            //    status = "IN PROGRESS";
            //}

            MyTasksViewModel.AddComment(meetingId, status, comment, hours, minutes, taskId, reviewsId, percentage);

        }


    }
}