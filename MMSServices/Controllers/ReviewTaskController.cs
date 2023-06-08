using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MMSServices.Helper;
using MMSServices.Models;
using System.Linq;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/ReviewTask")]
    public class ReviewTaskController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;

        /// <summary>
        /// Getting All Review Task
        /// </summary>
        /// <param name="chairperson"></param>
        /// <param name="categoryID"></param>
        /// <param name="participantID"></param>
        /// <param name="priorityType"></param>
        /// <param name="frequencyId"></param>
        /// <param name="pageno"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll(int chairperson, int categoryID, int participantID, string priorityType, string frequencyId, int pageno, int pagesize)
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    // Determine the number of records to skip
                    int skip = (pageno - 1) * pagesize;

                    var res = (from rt in db.Proc_Get_Review_Tasks(chairperson, categoryID, participantID, priorityType, frequencyId)
                               select new ReviewTaskViewModel
                               {
                                   Task_Id = rt.Task_Id,
                                   Meeting_Id = rt.Meeting_Id,
                                   Employee_Id = rt.Employee_Id,
                                   Chairperson = rt.Chairperson,
                                   Participant = rt.Participant,
                                   Task = rt.Task,
                                   Category = rt.Category,
                                   Completion_Date = rt.Completion_Date,
                                   Priority = rt.Priority,
                                   Status = rt.Status,
                                   StatusName = rt.StatusName,
                                   Comments = rt.Comments,
                                   CommenterName = rt.CommenterName,
                                   Review_Status = rt.Review_Status,
                                   Review_Date = rt.Review_Date,
                                   Assigned_Date = rt.Assigned_Date,
                                   Overdue_days = rt.Overdue_days,
                                   Comments_count = rt.Comments_count
                               }).ToList();


                    ReviewTaskPagingViewModel result = new ReviewTaskPagingViewModel();
                    if (res.Count() > 0)
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
                                 HttpStatusCode.NotFound, result, "Review Tasks doesn't exist", 0));
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
        /// Add Review
        /// </summary>
        /// <param name="taskID">11396</param>
        /// <returns></returns>
        [Route("Post")]
        public HttpResponseMessage Post(int taskID)
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    MMS_Track_ReviewTasks reviewTasks = new MMS_Track_ReviewTasks();
                    reviewTasks.TaskId = taskID;
                    reviewTasks.ActualReviewedDate = DateTime.Now;
                    reviewTasks.Review_Status = true;

                    reviewTasks.NextReviewDate = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskID).Select(x => x.NextReviewDate).FirstOrDefault();
                    db.MMS_Track_ReviewTasks.Add(reviewTasks);
                    db.SaveChanges();


                    return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.OK, null, "Review Details Has Been Inserted Successfully", 0));
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
