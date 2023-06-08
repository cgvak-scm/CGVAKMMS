using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MMSServices.Helper;
using MMSServices.Models;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/Login")]    
    public class LoginController : ApiController
    {
        private ReviewTaskDBEntities db;

        ApiResponse apiResponse = null;
        /// <summary>
        /// Login User
        /// </summary>
        /// <returns></returns>
        [Route("IsValidUser")]
        public HttpResponseMessage GetValidUser(string LoginUserName, string LoginPassword)
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {

                    var Username = (from EmpMaster in db.Employee_Master
                                      join MMSEmpMaster in db.MMS_Employee_Master on EmpMaster.EmployeeICode equals MMSEmpMaster.profile_employee_id
                                      where (EmpMaster.LoginUserName).ToLower() == LoginUserName.ToLower() && EmpMaster.IsActive == true
                                      select new LoginViewModel
                                      {
                                          EmployeeIcode = EmpMaster.EmployeeICode

                                      }).FirstOrDefault();

                    if (Username == null)
                    {
                        return Request.CreateResponse(
                                new ApiResponse(
                                    HttpStatusCode.NotFound, Username, "You have entered an invalid username.", 0));                       
                    }
                    else
                    {
                        var Loginres = (from EmpMaster in db.Employee_Master
                                        join MMSEmpMaster in db.MMS_Employee_Master on EmpMaster.EmployeeICode equals MMSEmpMaster.profile_employee_id
                                        where (EmpMaster.LoginUserName).ToLower() == LoginUserName.ToLower() && (EmpMaster.LoginPassword).ToLower() == LoginPassword.ToLower() && EmpMaster.IsActive == true
                                        select new LoginViewModel
                                        {
                                            EmployeeIcode = EmpMaster.EmployeeICode,
                                            EmployeeDisplayName = EmpMaster.EmployeeDisplayName                                            

                                        }).FirstOrDefault();

                        if (Loginres == null)
                        {
                            return Request.CreateResponse(
                                    new ApiResponse(
                                        HttpStatusCode.NotFound, Loginres, "You have entered an invalid password.", 0));
                        }
                        else
                        {
                            return Request.CreateResponse(
                                    new ApiResponse(
                                        HttpStatusCode.OK, Loginres, "Login Successfully.", 0));
                        }
                    }                                          
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
