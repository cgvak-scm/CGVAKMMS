using System;
using System.Linq;
using System.Web.Mvc;
using MMS.Security;
using MMS.Models;
using System.Web.Helpers;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]
  
    public class AssignedTasksController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public ActionResult Index(DateTime? selectEndD)
        {
            var today = DateTime.Now;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = today;
            ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");
            // ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");

            if (selectEndD != null)
            {
                Session["ViewBag.EndDate"] = Convert.ToDateTime(selectEndD);
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

            ViewBag.Status = DepartmentCategoryModel.TaskStatus();

            return View();
        }

        //changed from HttpPost to HttpGet
        [HttpGet]
        /*fetching meeting name for appending into dropdown */
        public JsonResult MeetingTask(DateTime startDate, DateTime endDate, string priority, int status)
        {

            var Ret_meeting = AssignedTasksModelView.MeetingTask(startDate, endDate.AddDays(1), priority, status);
            return Json(Ret_meeting, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        //changed the return type into "PartialViewResult" from 'ActionResult' to Solve DatePicker stops working  in edit modal of 'Track/Close Assigned Tasks' section 
        public PartialViewResult EditTaskDetails(int meetingID, int empId, int taskID)
        /*task results passed to edittaskDetails page */
        {
            var today = DateTime.Now;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = today;
            ViewBag.StartDate = startDate.ToString("dd/MMM/yyyy");
            ViewBag.EndDate = endDate.ToString("dd/MMM/yyyy");


            MeetingEditDetails MeetingObj = new MeetingEditDetails();
            
            var TaskItems = AssignedTasksModelView.EditTaskDetails(taskID);

            int? searchFor = 0;
            var searchForStatus = "";
            var searchForPriority = "";
            foreach (var resItems in TaskItems)
            {
                MeetingObj.meetingId = meetingID;
                //MeetingObj.Task = task.ToString().Replace("%20", " ");
                MeetingObj.Task = resItems.task;
                MeetingObj.RespPerson = resItems.participant.ToString();
                MeetingObj.empId = empId;
                //original---->
                //MeetingObj.CompletionDate = Convert.ToDateTime(resItems.completion_date).ToShortDateString();

                //Modified to solve Date Format issue
                DateTime tempDate = Convert.ToDateTime(resItems.completion_date);


                MeetingObj.CompletionDate = tempDate.ToString("dd/MMM/yyyy").Replace('-', '/');


                MeetingObj.Priority = resItems.priority;                
                searchForPriority = Convert.ToString(resItems.priority);

                MeetingObj.selectedCategory = Convert.ToString(resItems.category);
                Session["taskid"] = resItems.task_id;

                //added newly to bind 'TaskID'
                MeetingObj.TaskID = resItems.task_id;
                //added newly to bind 'Comments'
                MeetingObj.Comments = resItems.comments;                               
                MeetingObj.Status = Convert.ToInt32(resItems.status);
                searchForStatus = Convert.ToString(resItems.status);

                ViewBag.SelectedCategory = resItems.category;
                searchFor = Convert.ToInt16(resItems.category);
            }
            //original
            //ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();

            ViewBag.AlteredPriority = DepartmentCategoryModel.TaskPriority();
            ViewBag.SelectedPriority = new SelectList(db.MMS_Priority, "Name", "Name", searchForPriority);

            ViewBag.AlteredStatus = DepartmentCategoryModel.TaskStatus();
            ViewBag.SelectedStatus = new SelectList(db.Task_Status, "Name", "Name", searchForStatus);
            //edited

            ViewBag.AlternateCategories = DepartmentCategoryModel.CategoryMasterList();

            //original-->

            string selected = (from sub in db.MMS_Meeting_Category
                               where sub.ID == searchFor
                               select sub.CategoryName).FirstOrDefault();

            ViewBag.Categories = new SelectList(db.MMS_Meeting_Category, "CategoryName", "CategoryName", selected);

            ViewBag.CommentsList = AssignedTasksModelView.GetCommentsByTaskId(taskID);
            return PartialView(MeetingObj);

        }

    }
}