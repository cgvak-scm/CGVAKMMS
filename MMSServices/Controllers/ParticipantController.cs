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
    [RoutePrefix("api/Participant")]
    public class ParticipantController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;
        /// <summary>
        /// Getting Review Task Participants
        /// </summary>
        /// <param name="chairperson"></param>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll(int? chairperson, int pageno, int pagesize)
        {
            try
            {                
                using (db = new ReviewTaskDBEntities())
                {
                    // Determine the number of records to skip
                    int skip = (pageno - 1) * pagesize;

                    var resWithStatus = (from meetingDetails in db.MMS_Meeting_Details
                               join empMaster in db.Employee_Master on meetingDetails.Participant equals empMaster.EmployeeICode
                               where meetingDetails.Chairperson == chairperson && meetingDetails.Status == 3 // Status 3 is "IN PROGRESS
                               select new ReviewTaskParticipantViewModel
                               {
                                   employee_id = meetingDetails.Employee_Id,
                                   name = (empMaster.EmployeeFirstName + " " + empMaster.EmployeeLastName).Trim(),
                                   Participant = meetingDetails.Participant
                               }).Distinct().OrderBy(x => x.name).ToList();

                    var resWithOutStatus = (from meetingDetails in db.MMS_Meeting_Details
                               join empMaster in db.Employee_Master on meetingDetails.Participant equals empMaster.EmployeeICode
                               where meetingDetails.Chairperson == chairperson && meetingDetails.Status != 3
                                            select new ReviewTaskParticipantViewModel
                               {
                                   employee_id = meetingDetails.Employee_Id,
                                   name = (empMaster.EmployeeFirstName + " " + empMaster.EmployeeLastName).Trim(),
                                   Participant = meetingDetails.Participant
                               }).Distinct().OrderBy(x => x.name).ToList();
                   
                    var res = Enumerable.Union(resWithStatus, resWithOutStatus);
                    int count = res.Count();
                    ParticipantPagingViewModel result = new ParticipantPagingViewModel();
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
                                 HttpStatusCode.NotFound, result, "Participant doesn't exist", 0));
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
    }
}
