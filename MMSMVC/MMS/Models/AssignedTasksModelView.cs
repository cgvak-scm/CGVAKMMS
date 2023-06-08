using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class AssignedTasksModelView
    {

        private static MMSCGVAKDBEntities db;

        #region methods

        public static dynamic MeetingTask(DateTime startDate, DateTime endDate, string priority, int status)
        {

            using (db = new MMSCGVAKDBEntities())
            {
                return db.sp_select_Meeting_Name(Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), startDate, endDate, priority, status).ToList();

            }
        }


        public static dynamic GetMeetingDetails(int meetingName, string priority, int status)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.sp_get_meeting_details_Advanced(meetingName, priority, status).ToList();
            }
        }

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
        public static dynamic EditTaskDetails(int taskID)
        {
            using (db = new MMSCGVAKDBEntities())
            {

                var ststusres = (from meetingdetailstemp in db.MMS_Meeting_Details
                                 where meetingdetailstemp.Task_Id == taskID
                                 select new
                                 {
                                     statustemp = meetingdetailstemp.Status
                                 }).FirstOrDefault();
                if (ststusres.statustemp != null)
                {
                    var res = (from meetingDetails in db.MMS_Meeting_Details
                               join meetingStatus in db.MMS_Meeting_Status_Update
                               on meetingDetails.Meeting_Id equals meetingStatus.MeetingId
                               where meetingDetails.Task_Id == taskID                               
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
                                   task_id = meetingDetails.Task_Id

                               }).ToList();


                    return res;
                }
                else
                {
                    var res = (from meetingDetails in db.MMS_Meeting_Details                              
                               where meetingDetails.Task_Id == taskID                               
                               select new
                               {
                                   meetingId = meetingDetails.Meeting_Id,
                                   task = meetingDetails.Task,
                                   participant = meetingDetails.Participant,
                                   employee_id = meetingDetails.Employee_Id,
                                   completion_date = meetingDetails.Completion_Date,
                                   priority = meetingDetails.Priority,
                                   category = meetingDetails.Category,
                                   status = 5,
                                   comments = meetingDetails.Comments,
                                   task_id = meetingDetails.Task_Id,

                               }).ToList();


                    return res;
                }
            }
        }
        #endregion
    }
}