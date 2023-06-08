using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class ReviewTasksViewModel
    {
        private static MMSCGVAKDBEntities db;
        public static string url = "http://202.129.196.131:8085/hurdles/webservices/get_users";

        public static List<Proc_Get_Review_Tasks_Result> FetchMyTasks(int chairperson , int Category_id,int Participant, string PriorityType, string FrequencyId)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                var result = (from res in db.Proc_Get_Review_Tasks(chairperson, Category_id, Participant, PriorityType,FrequencyId) 

                              select res
                              ).OrderBy(x => x.Task).ToList();
                return result;
            }
        }

        public static string AddComment(int meetingId, int status, string comment, int hours, int minutes, int taskId, string reviewsId)
        {
            try
            {
                MMS_Meeting_Status_Update meetingStatusUpdate;

                using (db = new MMSCGVAKDBEntities())
                {
                    meetingStatusUpdate = db.MMS_Meeting_Status_Update.Where(x => x.TaskId == taskId).FirstOrDefault();

                    //  }
                    int? loggedInUser = Convert.ToInt32(HttpContext.Current.Session["LoggedUserId"]);

                    loggedInUser = db.MMS_Meeting_Details.Where(x => x.Participant == loggedInUser && x.Task_Id == taskId).Select(x => x.Chairperson).FirstOrDefault();
                    //int? part = loggedInUser;
                    var loggedInUserID = Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]);
                    //loggedInUserID = Convert.ToInt16(part);

                    string totalHours = hours + ":" + minutes;
                    //using (db = new MMSCGVAKDBEntities())
                    //{
                    string Task = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.Task).FirstOrDefault();
                    db.sp_insert_Meeting_Status(loggedInUserID, Task, loggedInUserID, DateTime.Now, comment, status, totalHours, meetingId, loggedInUserID, loggedInUserID, taskId);


                    //Insert into MMS_track_reviewtasks for Review Status

                    MMS_Track_ReviewTasks reviewTasks = new MMS_Track_ReviewTasks();
                    reviewTasks.TaskId = taskId;
                    reviewTasks.ActualReviewedDate = DateTime.Now;

                    

                    var taskpercent = (from x in db.MMS_Meeting_Details where x.Task_Id == taskId select x).ToList();

                    if (status == 2)
                    {

                        foreach (var item in taskpercent)
                        {
                            item.Percentage_Completed = Convert.ToDecimal(100);
                        }
                    }

                    else if (status == 5 || status == 4)
                    {
                        foreach (var item in taskpercent)
                        {
                            item.Percentage_Completed = Convert.ToDecimal(0);
                        }

                    }
                    else
                    {
                        foreach (var item in taskpercent)
                        {
                            item.Percentage_Completed = item.Percentage_Completed;
                        }

                    }

                    if (reviewsId == "yes")
                    {
                        reviewTasks.Review_Status = true;
                    }

                    else
                        reviewTasks.Review_Status = false;

                    reviewTasks.NextReviewDate = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.NextReviewDate).FirstOrDefault();

                    db.MMS_Track_ReviewTasks.Add(reviewTasks);
                    //  db.Entry(meetingDetail).State = EntityState.Deleted;
                    db.SaveChanges();
                    // string message = "Review Details Has Been Inserted Successfully";

                }
            }
            catch (Exception ex)
            {
                //TempData["Success"] = ex.ToString();
            }


            return string.Empty;
        }

        public class Requency
        {                       
            public string[] FrequencyId { get; set; }
        }

    }

}