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
    [RoutePrefix("api/TaskStatus")]
    public class TaskStatusController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;
        /// <summary>
        /// Getting Status from Review Task
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    var TaskStatus = (from ts in db.Task_Status
                                      select new ReviewTaskStatusViewModel
                                      {
                                          ID = ts.ID,
                                          Name = ts.Name,
                                          Descriptions = ts.Descriptions,
                                          Active = ts.Active,
                                      }).OrderBy(x => x.Name).ToList();

                    if (TaskStatus == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, TaskStatus, "Tasks doesn't exist", 0));
                    else
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.OK, TaskStatus, "", 0));
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
        /// Review Task For chairperson
        /// </summary>
        /// <param name="chairperson"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Route("Get")]
        public HttpResponseMessage Get(int chairperson, int taskId)
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    List<ReviewTaskStatusViewModel> reviewTask = new List<ReviewTaskStatusViewModel>();

                    var Task = (from mmd in db.MMS_Meeting_Details
                                where mmd.Chairperson == chairperson & mmd.Task_Id == taskId
                                select new TaskStatus
                                {
                                    taskName = mmd.Task
                                }).FirstOrDefault();

                    //var Task = db.MMS_Meeting_Details.Where(x => x.Chairperson == chairperson & x.Task_Id == taskId).Select(x => x.Task).FirstOrDefault().ToString();

                    if (Task != null)
                    {
                        reviewTask = (from ts in db.Task_Status
                                          select new ReviewTaskStatusViewModel
                                          {
                                              ID = ts.ID,
                                              Name = ts.Name,
                                              Descriptions = ts.Descriptions,
                                              Active = ts.Active,
                                          }).OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        reviewTask = (from ts in db.Task_Status where ts.Name != "CLOSED"
                                      select new ReviewTaskStatusViewModel
                                      {
                                          ID = ts.ID,
                                          Name = ts.Name,
                                          Descriptions = ts.Descriptions,
                                          Active = ts.Active,
                                      }).OrderBy(x => x.Name).ToList();
                    }

                    if (reviewTask == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, reviewTask, "Tasks doesn't exist", 0));
                    else
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.OK, reviewTask, "", 0));
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
