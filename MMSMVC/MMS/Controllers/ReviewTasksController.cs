using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Models;
using MMS.Security;
using System.Data.Entity.Validation;

namespace MMS.Controllers
{
    [SessionExpire]
    
    public class ReviewTasksController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        // GET: ReviewTasks
        public ActionResult Index(DateTime? ReviewEndDate,DateTime? ReviewStartDate)
        {
            var today = DateTime.Now;
            var startDate = new DateTime(today.Year, today.Month, 1);
            // var startDate = new DateTime(2017, 3, 1);
            var endDate = today;
            ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");
            // ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");

            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();
            ViewBag.Participants = MeetingDetailsViewModel.SelectParticipants(Convert.ToInt32(Session["LoggedUserId"]));
            ViewBag.ReviewFrequencyID = DepartmentCategoryModel.ReviewFrequency();
            //End Date

            if (ReviewEndDate != null)
            {
                Session["ViewBag.EndDate"] = Convert.ToDateTime(ReviewEndDate);
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

            //End Of End Date block


            // Start Date Block

            if (ReviewStartDate != null)
            {
                Session["ViewBag.StartDate"] = Convert.ToDateTime(ReviewStartDate);
            }
            if (Session["ViewBag.StartDate"] != null)
            {
                //ViewBag.StartDate = Convert.ToDateTime(selectDates);
                ViewBag.StartDate = Session["ViewBag.StartDate"];
            }
            else
            {
                //Convert.ToDateTime(startDate.ToString("dd/MMM/yyyy"));
                ViewBag.StartDate = Convert.ToDateTime(endDate.ToString("dd/MMM/yyyy"));
            }


            //


            return View();
        }
        public JsonResult FetchMyTasks( int Category_id, int Participant, string PriorityType, string[] FrequencyId)
        //public JsonResult FetchMyTasks(DateTime? startDate, DateTime? endDate )
        {
            string FrequencyID = string.Empty;
            if (FrequencyId != null)
            {
                if (FrequencyId[0] == "All")
                    FrequencyID = "All";
                else
                {
                    for (int i = 0; i < FrequencyId.Length; i++)
                    {
                        FrequencyID = FrequencyID.Trim() + FrequencyId[i] + ",";
                    }
                }
            }
            
            var RetMsg = string.Empty;
            int chairperson = Convert.ToInt16(Session["LoggedUserId"]);
            try
            {
                //Fetch meeting details of own tasks               
                var Taskgrid = ReviewTasksViewModel.FetchMyTasks(chairperson, Category_id, Participant, PriorityType, FrequencyID);                


                return Json(Taskgrid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Success"] = ex.ToString();

            }


            return Json(RetMsg, JsonRequestBehavior.AllowGet);
        }

        #region

        [HttpPost]
        public ActionResult Review(int taskId, int? meetingId)
        {

            string ResMeetingMsg = string.Empty;
            try
            {
                ResMeetingMsg = MeetingDetailsViewModel.Review(taskId, meetingId);
                

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


        #endregion

        #region Show Comment Popup for Review Task

        [HttpGet]
        public ActionResult ShowComments(int meetingId, int taskId)
        {           
            var result = MyTasksViewModel.GetCommentsByTaskId(taskId);
            List<GetTaskComments_Result> CommentsRes = new List<GetTaskComments_Result>();
           
            var taskname = db.MMS_Meeting_Details.Where(c=>c.Task_Id==taskId).FirstOrDefault();
            ViewBag.TaskName = taskname.Task;
            foreach (var res in result)
            {
                if (res.Comments != null && res.Comments != "")
                {
                    CommentsRes.Add(new GetTaskComments_Result { Comments = res.Comments, Commented_by = res.Commented_by, StatusDate = res.StatusDate });
                }
                CommentsRes.OrderByDescending(x => x.StatusDate).ToList();
            }

            var statusList = db.Task_Status.ToList();
            
            statusList = statusList.Where(x => x.Name != "CLOSED").ToList();          
            ViewBag.SelectedStatus = new SelectList(statusList, "ID", "Name");
            ViewBag.Status_Selected = MyTasksViewModel.GetMeetingTaskStatus(taskId);
            ViewBag.Status = new SelectList(db.Task_Status.Where(x => x.Name != "CLOSED").ToList(), "ID", "Name");
            ViewBag.MeetingID = meetingId;
            ViewBag.TaskID = taskId;
            return View(CommentsRes.OrderByDescending(x => x.StatusDate).ToList());
        }

        [HttpPost]
        public void AddComment(string comment, int status, int meetingId, int hours, int minutes, int taskId, string reviewsId)
        {            
            ReviewTasksViewModel.AddComment(meetingId, status, comment, hours, minutes, taskId, reviewsId);
        }

        #endregion


    }
}