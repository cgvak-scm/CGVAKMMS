using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MMSServices.Helper;
using MMSServices.Models;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        private ReviewTaskDBEntities db;

        /// <summary>
        /// Getting All Category
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        public HttpResponseMessage GetAll()
        {            
            try
            {
                using (db = new ReviewTaskDBEntities())
                {
                    var res = (from MMC in db.MMS_Meeting_Category
                               select new CategoryViewModel
                               {
                                   ID = MMC.ID,
                                   CategoryName = MMC.CategoryName

                               }).OrderBy(x => x.CategoryName).ToList();


                    if (res == null)
                        return Request.CreateResponse(
                            new ApiResponse(
                                HttpStatusCode.NotFound, res, "Category doesn't exist", 0)

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
