using MMSServices.Helper;
using MMSServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/TaskAssigned")]
    public class AssignedTasksController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;

        /// <summary>
        /// Getting All Track / Close Assigned Tasks
        /// </summary>
        /// <param name="chairperson"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="priority"></param>
        /// <param name="status"></param>
        /// <param name="pageno"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll(int? chairperson, DateTime startDate, DateTime endDate, string priority, int? status, int pageno, int pagesize)
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    // Determine the number of records to skip
                    int skip = (pageno - 1) * pagesize;

                    var res = (from rt in db.sp_select_Meeting_Name(chairperson, startDate, endDate.AddDays(1), priority, status)

                               select new AssignedTaskViewModel
                               {
                                   MeetingId = rt.meeting_id,
                                   EmployeeId = rt.employee_id,
                                   TaskId = rt.Task_Id,
                                   Task = rt.Task,
                                   CategoryId = rt.Category,
                                   Comments = rt.Comments,
                                   Participant = rt.Participant,
                                   meeting_date = rt.meeting_date,
                                   completion_date = rt.completion_date,
                                   priority = rt.priority,
                                   status = rt.status,
                                   Overdue_Days = rt.Overdue_Days

                               }).ToList();

                    AssignedTaskPagingViewModel result = new AssignedTaskPagingViewModel();
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

                    if (res == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, result, "Tasks doesn't exist", 0));
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
        /// Add Track / Close Assigned Tasks
        /// </summary>
        /// <param name="AssignedTask"></param>        
        /// <returns></returns>
        [Route("Post")]
        public HttpResponseMessage Post(PostAssignedTaskViewModel AssignedTask)
        {
            try
            {                
                int? employeeId = 0;
                int? taskId = 0;
                int? commented_by = 0;
                

                using (db = new ReviewTaskDBEntities())
                {
                   if(AssignedTask != null)
                    {
                        var meetingdetails = db.MMS_Meeting_Details.Where(x => x.Meeting_Id == AssignedTask.meetingId && x.Task_Id == AssignedTask.task).FirstOrDefault();

                        employeeId = meetingdetails.Employee_Id;
                        taskId = meetingdetails.Task_Id;
                        commented_by = meetingdetails.Chairperson;
                        DateTime ReviewDate = DateTime.Now.AddDays(1);
                                                
                        var updateCount = db.Sp_Update_MMS_Meeting_Details_Change(employeeId, taskId, AssignedTask.completionDate, AssignedTask.status, AssignedTask.category, AssignedTask.meetingId, AssignedTask.comments, ReviewDate, 1, commented_by, Convert.ToString(AssignedTask.task), AssignedTask.priority);                                               
                    }

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
