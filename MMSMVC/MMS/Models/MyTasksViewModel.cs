using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class MyTasksViewModel
    {



        #region props
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string categoryId { get; set; }
        #endregion


        #region dbcontext
        private static MMSCGVAKDBEntities db;
        
        #endregion

        #region Fetching total tasks
        public static List<Proc_MyTask_Result> FetchMyTasks(DateTime? startDate, DateTime? endDate, int? categoryId, int? FrequencyId)
        {
            ////ViewBag.Participants = MeetingDetailsViewModel.SelectParticipants(Session["LoggedUserId"].ToString());
            using (db = new MMSCGVAKDBEntities())
            {
                var result = (from res in db.Proc_MyTask(Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"].ToString()), startDate, endDate, categoryId, FrequencyId)
                                  //group res by res.meeting_id into y
                                  //select y.First())                        //Commented. Gokul Thangavel 18-Aug-2017
                              select res
                              ).OrderBy(x => x.Allocated_Date).ToList();

                return result;
            }
        }
        #endregion

        #region Meeting status update
        public static dynamic MeetingStatusUpdate(int meetingId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                var res = (from meetingDetails in db.MMS_Meeting_Details
                           join
                           meetingStatus in db.MMS_Meeting_Status_Update
                           on meetingDetails.Meeting_Id equals meetingStatus.MeetingId
                           where meetingDetails.Meeting_Id == meetingId
                           select new
                           {
                               meetingId = meetingDetails.Meeting_Id,
                               task = meetingDetails.Task,
                               participant = meetingDetails.Participant,
                               employee_id = meetingDetails.Employee_Id,
                               completion_date = meetingDetails.Completion_Date,
                               priority = meetingDetails.Priority,
                               category = meetingDetails.Category,
                               status = meetingStatus.status,
                               comments = meetingDetails.Comments,
                               task_id = meetingDetails.Task_Id,

                           }).ToList();


                return res;
            }
        }
        #endregion

        #region GetTaskByID
        /// <summary>
        /// Get the task by TaskID. Added 17Aug2017 Gokul Thangavel. Dito of MeetingStatusUpdate
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static dynamic GetTaskByID(int? taskId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                var res = (from meetingDetails in db.MMS_Meeting_Details
                               //join meetingStatus in db.MMS_Meeting_Status_Update on meetingDetails.Meeting_Id equals meetingStatus.MeetingId
                           where meetingDetails.Task_Id == taskId
                           select new
                           {
                               meetingId = meetingDetails.Meeting_Id,
                               task = meetingDetails.Task,
                               participant = meetingDetails.Participant,
                               employee_id = meetingDetails.Employee_Id,
                               completion_date = meetingDetails.Completion_Date,
                               priority = meetingDetails.Priority,
                               category = meetingDetails.Category,
                               status = meetingDetails.Status,
                               comments = meetingDetails.Comments,
                               task_id = meetingDetails.Task_Id,
                               chairPerson = meetingDetails.Chairperson,
                               //meetingstatusauto_id = meetingStatus.MeetingStatusAutoId

                           }).FirstOrDefault();


                return res;
            }
        }
        #endregion


        #region Update meeting details
        public static string UpdateMeetingDetails(int empId, string task, int status, string comments, DateTime completionDate, int respPerson, string commentedBy,
               int commentedById, int meetingId, string hours)
        {

            int taskId = Convert.ToInt16(HttpContext.Current.Session["taskId"]);

            using (db = new MMSCGVAKDBEntities())
            {
                var res = db.sp_insert_Meeting_Status(empId, task, respPerson, completionDate, comments, status, hours, meetingId, Convert.ToInt32(commentedBy), commentedById, taskId);


            }

            return "Updated Successfully";

        }
        #endregion

        #region UpdateMeetingTaskDetail
        /// <summary>
        /// Update the TaskDetails. Added 17Aug2017. Gokul Thangavel. Dito of UpdateMeetingDetails. UpdateMeetingDetails updated based on MeetingID, instead of MeetingID added TaskID 
        /// </summary>

        /// <returns></returns>

        public static string UpdateMeetingTaskDetails(int empId, string task, int status, string completionDate,
             int taskId, string priority, string comments)
        {
            var completionDateTime = Convert.ToDateTime(completionDate);
            using (db = new MMSCGVAKDBEntities())
            {
                var meetingDetails = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).FirstOrDefault();

                meetingDetails.Completion_Date = completionDateTime;
                meetingDetails.Priority = priority;
                meetingDetails.Task = task;
                meetingDetails.Status = Convert.ToInt16(status);
                meetingDetails.Comments = comments;
                if (status == 2)
                {
                    meetingDetails.Percentage_Completed = 100;
                }
                else if (status == 4)
                {
                    meetingDetails.Percentage_Completed = 0;
                }
                db.Entry(meetingDetails).State = System.Data.Entity.EntityState.Modified;

                MMS_Meeting_Status_Update meetingStatusUpdate = new MMS_Meeting_Status_Update();
                meetingStatusUpdate.MeetingId = meetingDetails.Meeting_Id;
                meetingStatusUpdate.EmployeeId = empId;
                meetingStatusUpdate.Task = task;
                meetingStatusUpdate.status = status;
                meetingStatusUpdate.TaskId = taskId;
                db.MMS_Meeting_Status_Update.Add(meetingStatusUpdate);
                MMS_Meeting_Status meetingStatus = new MMS_Meeting_Status();

                meetingStatus.Meeting_Status_Comments = comments;
                meetingStatus.Meeting_Id = meetingDetails.Meeting_Id; ;
                meetingStatus.Meeting_Status = status;
                meetingStatus.Task_Id = taskId;
                meetingStatus.Meeting_Status_Employee_Id = empId;
                meetingStatus.Meeting_Status_Date = DateTime.Now;
                meetingStatus.Meeting_Status_Task = task;
                meetingStatus.Commented_By_ID = empId;                
                meetingStatus.Meeting_Status_ResponsiblePerson = (db.Employee_Master.Where(x => x.EmployeeICode == empId).Select(x => x.EmployeeICode).Distinct().FirstOrDefault());
                db.SaveChanges();
            }

            return "Updated Successfully";
        }

        #endregion


        #region ShowComments 
        #region Not Used
        public static dynamic ShowComments(int meetingId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.Proc_Get_meeting_status(Convert.ToInt32(HttpContext.Current.Session["LoggedUserId"].ToString()), meetingId).ToList();
            }
        }
        #endregion
        public static dynamic GetCommentsByTaskId(int TaskID)
        {
            List<GetTaskComments_Result> TaskCommentlist = new List<GetTaskComments_Result>();
            using (db = new MMSCGVAKDBEntities())
            {
                var taskComments = (from meetingstatus in db.MMS_Meeting_Status
                                    where meetingstatus.Task_Id == TaskID
                                    orderby meetingstatus.Meeting_Status_Date descending
                                    select new
                                    {
                                        Comments = meetingstatus.Meeting_Status_Comments,
                                        Commented_by = meetingstatus.Commented_By,
                                        StatusDate = meetingstatus.Meeting_Status_Date
                                    }).ToList();
                if (taskComments.Count > 1)
                {
                    foreach (var a in taskComments)
                    {
                        GetTaskComments_Result taskComment = new GetTaskComments_Result();
                        taskComment.Comments = a.Comments;
                        taskComment.Commented_by = a.Commented_by;
                        taskComment.StatusDate = a.StatusDate.ToString();
                        TaskCommentlist.Add(taskComment);
                    }
                }

            }
            return TaskCommentlist;

        }
        #endregion

        #region PercentageChanges
        public static string PercentageUpdateChanges(decimal percent, int meetingId, int taskID)
        {
            using (db = new MMSCGVAKDBEntities())
            {

                db.sp_update_meeting_Percent(Convert.ToInt32(HttpContext.Current.Session["Task_Meeting_id"]), Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), percent, taskID).ToString();
                if (percent.Equals(100))
                {
                    (from mmsDet in db.MMS_Meeting_Details
                     where mmsDet.Task_Id == taskID
                     select mmsDet).ToList().ForEach(x => x.Status = 2);

                    (from mmsstatus in db.MMS_Meeting_Status
                     where mmsstatus.Task_Id == taskID
                     select mmsstatus).ToList().ForEach(x => x.Meeting_Status = 2);

                    (from mmsstatusUpdate in db.MMS_Meeting_Status_Update
                     where mmsstatusUpdate.TaskId == taskID
                     select mmsstatusUpdate).ToList().ForEach(x => x.status = 2);


                    db.SaveChanges();
                }
                else
                    db.sp_update_meeting_Percent(Convert.ToInt32(HttpContext.Current.Session["Task_Meeting_id"]), Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), percent, taskID).ToString();

            }
            return "Percentage has been modified";

        }

        #region Get Meeting Total Counts
        public static int GetCounts(int meetingId, int taskid, string Status)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                MMS_Meeting_Details meetdetails = new MMS_Meeting_Details();

                if (meetdetails.Status == 2)

                {
                    meetdetails.Percentage_Completed = 100;
                    db.Entry(meetdetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }



                var result = (from val in db.MMS_Meeting_Status
                                  //where val.Meeting_Id == meetingId && val.Meeting_Status_Task == task
                              where
                              //  val.Meeting_Id == meetingId &&
                              //val.Task_Id==taskid &&
                              val.Task_Id == taskid && val.Meeting_Status_Comments != null && val.Meeting_Status_Comments != " "
                              select val).Count();
                return result;
                //db.sp_select_meeting_status(meetingId, task, participant).Count();                
            }
        }
        #endregion


        #endregion

        #region GetMeetingStatus

        public static string GetMeetingTaskStatus(int taskId)
        {
            //string status = string.Empty;
            int status = 0;
            using (db = new MMSCGVAKDBEntities())
            {
                var meeting = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).OrderByDescending(x => x.Comments).FirstOrDefault();

                if (meeting != null)
                {
                    status = Convert.ToInt16(meeting.Status);
                }
            }
            return status.ToString();
        }

        #endregion

        #region AddComments

        /// <summary>
        /// Add new comment
        /// </summary>
        public static string AddComment(int meetingId, int status, string comment, int hours, int minutes, int taskId, string reviewsId,int percentage)
        {
            try
            {
                MMS_Meeting_Status_Update meetingStatusUpdate;

                using (db = new MMSCGVAKDBEntities())
                {
                    meetingStatusUpdate = db.MMS_Meeting_Status_Update.Where(x => x.TaskId == taskId).FirstOrDefault();                    
                    int? loggedInUser = Convert.ToInt32(HttpContext.Current.Session["LoggedUserId"]);
                    loggedInUser = db.MMS_Meeting_Details.Where(x => x.Participant == loggedInUser && x.Task_Id == taskId).Select(x => x.Chairperson).FirstOrDefault();                                       
                    var loggedInUserID = Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]);                    
                    IQueryable<int?> Chairperson;
                    Chairperson = (from meeting in db.MMS_Meeting_Details where meeting.Participant == loggedInUser && meeting.Task_Id == taskId select meeting.Chairperson);

                    foreach(var data in Chairperson)
                    {
                        Console.WriteLine("Contact ID: {0}");
                    }
                    string totalHours = hours + ":" + minutes;
                    string Task = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.Task).FirstOrDefault();
                    db.sp_insert_Meeting_Status(loggedInUserID, Task, loggedInUserID, DateTime.Now, comment, status, totalHours, meetingId, loggedInUserID, loggedInUserID, taskId);

                    MMS_Track_ReviewTasks reviewTasks = new MMS_Track_ReviewTasks();
                    reviewTasks.TaskId = taskId;
                    reviewTasks.ActualReviewedDate = DateTime.Now;
                    var meetDetails = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).FirstOrDefault();
                    meetDetails.Percentage_Completed = Convert.ToDecimal(percentage);

                    if (reviewsId == "yes")
                    {
                        reviewTasks.Review_Status = true;
                    }

                    else
                        reviewTasks.Review_Status = false;

                    reviewTasks.NextReviewDate = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.NextReviewDate).FirstOrDefault();
                    db.MMS_Track_ReviewTasks.Add(reviewTasks);                    
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //TempData["Success"] = ex.ToString();
            }

            return string.Empty;
        }

        #endregion


        #region UserCheck

        public static bool CheckExistingUser(int meetingId)
        {
            bool _success = false;

            using (db = new MMSCGVAKDBEntities())
            {
                int empId = Convert.ToInt16(db.MMS_Meeting_Details.Where(x => x.Meeting_Id == meetingId).Select(x => x.Employee_Id).FirstOrDefault());

                if (string.Equals(empId.ToString(), HttpContext.Current.Session["LoggedUserId"].ToString()))
                {
                    _success = true;
                }
            }

            return _success;
        }

        #endregion
    }
}