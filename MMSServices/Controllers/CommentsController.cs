using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MMSServices.Helper;
using MMSServices.Models;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/Comments")]    
    public class CommentsController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;
        /// <summary>
        /// Getting Comments from Review Task
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        [Route("Get")]
        public HttpResponseMessage Get(int taskID)
        {
            try
            {
                List<ReviewTaskCommentViewModel> TaskCommentlist = new List<ReviewTaskCommentViewModel>();

                using (db = new ReviewTaskDBEntities())
                {
                    var taskComments = (from meetingstatus in db.MMS_Meeting_Status
                                        where meetingstatus.Task_Id == taskID
                                        orderby meetingstatus.Meeting_Status_Date descending
                                        select new ReviewTaskCommentViewModel
                                        {
                                            Comments = meetingstatus.Meeting_Status_Comments,
                                            Commented_by = meetingstatus.Commented_By,
                                            StatusDate = meetingstatus.Meeting_Status_Date
                                        }).ToList();


                    if (taskComments == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, taskComments, "Comments doesn't exist", 0));
                    else
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.OK, taskComments, "", 0));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                              new ApiResponse(
                                  HttpStatusCode.InternalServerError, null, ex.ToString(), 1));
            }
        }

        /// <summary>
        /// Getting All Comments from Review Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Route("GetAll")]
        [HttpGet]
        public HttpResponseMessage GetAll(int taskId, int pageno, int pagesize)
        {
            int count = 0;

            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    // Determine the number of records to skip
                    int skip = (pageno - 1) * pagesize;

                    var res = (from val in db.sp_select_meeting_status(taskId)
                               select new meetingstatusViewModel
                               {
                                   Meeting_Status_Employee_Id = val.Meeting_Status_Employee_Id,
                                   Meeting_Status_Task = val.Meeting_Status_Task,
                                   Meeting_Status_ResponsiblePerson = val.Meeting_Status_ResponsiblePerson,
                                   Meeting_Status_Date = val.Meeting_Status_Date,
                                   Meeting_Status_Comments = val.Meeting_Status_Comments,
                                   Meeting_Status = val.Meeting_Status,
                                   Meeting_Id = val.Meeting_Id,
                                   Meeting_Status_Hours = val.Meeting_Status_Hours,
                                   Comments_Count = count,
                                   Commented_By = val.Commented_By,
                                   Commented_By_ID = val.Commented_By_ID,
                                   MeetingStatusAutoId = val.MeetingStatusAutoId,
                                   Task_Id = val.Task_Id
                               }).OrderByDescending(x => x.Meeting_Status_Date).ToList();


                    CommentsPagingViewModel result = new CommentsPagingViewModel();
                    if (res.Count > 0)
                    {
                        // Get total number of records
                        result.totCount = res.Count();
                        result.pageNo = pageno;
                        result.pageSize = pagesize;
                        if (result.pageNo == 0)
                            result.data = res.ToList();
                        else
                            result.data = res.Skip(skip).Take(pagesize).ToList();
                    }

                    if (result == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, result, "Comments doesn't exist", 0));
                    else
                        return Request.CreateResponse(
                            new ApiResponse(
                                HttpStatusCode.OK, result, "", 0));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.InternalServerError, null, ex.ToString(), 1));
            }
        }

        /// <summary>
        /// Add Comments to Review Task
        /// </summary>
        /// <param name="reviewViewModel"></param>
        /// <returns></returns>
        [Route("Post")]        
        public HttpResponseMessage Post(PostCommentReviewViewModel reviewViewModel)
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {

                    string totalHours = reviewViewModel.hours + ":" + reviewViewModel.minutes;
                    int? loggedInUserID = db.MMS_Meeting_Details.Where(x => x.Task_Id == reviewViewModel.taskId).Select(x => x.Chairperson).FirstOrDefault();

                    string Task = db.MMS_Meeting_Details.Where(x => x.Task_Id == reviewViewModel.taskId).Select(x => x.Task).FirstOrDefault();

                    db.sp_insert_Meeting_Status(loggedInUserID, Task, loggedInUserID, DateTime.Now, reviewViewModel.comment, reviewViewModel.status, totalHours, reviewViewModel.meetingId, loggedInUserID, loggedInUserID, reviewViewModel.taskId);

                    MMS_Track_ReviewTasks reviewTasks = new MMS_Track_ReviewTasks();
                    reviewTasks.TaskId = reviewViewModel.taskId;
                    reviewTasks.ActualReviewedDate = DateTime.Now;
                    var taskpercent = (from x in db.MMS_Meeting_Details where x.Task_Id == reviewViewModel.taskId select x).ToList();

                    if (reviewViewModel.status == 2)
                    {
                        foreach (var item in taskpercent)
                        {
                            item.Percentage_Completed = Convert.ToDecimal(100);
                        }
                    }

                    else if (reviewViewModel.status == 5 || reviewViewModel.status == 4)
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

                    if (reviewViewModel.reviewsId == "yes")
                    {
                        reviewTasks.Review_Status = true;
                    }
                    else
                        reviewTasks.Review_Status = false;

                    reviewTasks.NextReviewDate = db.MMS_Meeting_Details.Where(x => x.Task_Id == reviewViewModel.taskId).Select(x => x.NextReviewDate).FirstOrDefault();
                    db.MMS_Track_ReviewTasks.Add(reviewTasks);
                    db.SaveChanges();

                    return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.OK, null, "Updated Successfully", 0));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.InternalServerError, null, ex.ToString(), 1));
            }
        }
    }
}
