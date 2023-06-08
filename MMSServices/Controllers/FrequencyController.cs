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
    [RoutePrefix("api/Frequency")]
    public class FrequencyController : ApiController
    {
        private ReviewTaskDBEntities db;

        /// <summary>
        /// Getting Review Task Frequency
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll()
        {
            ApiResponse apiResponse = null;
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    var res = (from MRF in db.MMS_Review_Frequency
                               select new ReviewTaskFrequencyViewModel
                               {
                                   ID = MRF.Freq_Id,
                                   FrequencyName = MRF.Freq_Name,
                                   FrequencyInDay = MRF.Freq_In_Days

                               }).OrderBy(x => x.ID).ToList();


                    if (res == null)
                        return Request.CreateResponse(
                            new ApiResponse(
                                HttpStatusCode.NotFound, res, "Frequency doesn't exist", 0)

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
