using MMSServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MMSServices.Controllers
{
    [RoutePrefix("api/IsValidURL")]
    public class IsValidURLController : ApiController
    {
        /// <summary>
        /// IsValidURL
        /// </summary>
        /// <returns></returns>
        [Route("Get")]
        public HttpResponseMessage Get()
        {
            try
            {                                    
                return Request.CreateResponse(
                        new ApiResponse(
                            HttpStatusCode.OK, "True", "Successfully.", 0));                                    
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
