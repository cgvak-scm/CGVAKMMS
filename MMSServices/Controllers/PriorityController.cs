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
    [RoutePrefix("api/Priority")]
    public class PriorityController : ApiController
    {
        private ReviewTaskDBEntities db;

        /// <summary>
        /// Getting All Priority
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll()
        {            
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    var res = (from MP in db.MMS_Priority
                               select new PriorityViewModel
                               {
                                   Id = MP.Id,
                                   Name = MP.Name,
                                   Descriptions = MP.Descriptions,
                                   Active = MP.Active

                               }).OrderBy(x => x.Name).ToList();


                    if (res == null)
                        return Request.CreateResponse(
                            new ApiResponse(
                                HttpStatusCode.NotFound, res, "Priority doesn't exist", 0)

                      );
                    else
                        return Request.CreateResponse(
                           new ApiResponse(
                               HttpStatusCode.OK, res, "", 0)

                     );
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
