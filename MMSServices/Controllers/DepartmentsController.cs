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
    [RoutePrefix("api/Departments")]
    public class DepartmentsController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;
        /// <summary>
        /// Getting All Employees by Department Name
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [Route("GetEmployeesByName")]
        public HttpResponseMessage GetEmployeesByName(string departmentName)
        {
            try
            {

                using (db = new ReviewTaskDBEntities())
                {

                    List<sp_get_EmployeeMaster_forJSON_Result> departments = db.sp_get_EmployeeMaster_forJSON(departmentName).ToList();

                    var employees =db.sp_get_Employee_Department();

                    var result = (from e in employees
                                  join dept in departments
                                  on e.Employee_Code equals dept.Employee_Id
                                  where e.Department_Id == dept.Department_ICode
                                  orderby e.User_name
                                  select new PseudoNames
                                  {
                                      Employee_Name = e.User_name,
                                      Employee_Id = e.Employee_Code
                                  }).ToList();

                    if (result == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, result, "Departments doesn't exist", 0));
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
        /// Getting All Departments 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("GetAllDepartments")]
        public HttpResponseMessage GetAllDepartments()
        {
            try
            {
                using (db = new ReviewTaskDBEntities())
                {

                    var dept = db.Sp_get_DepartmentsList().ToList();

                    if (dept == null)
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.NotFound, dept, "Departments Api Error", 0));
                    else
                        return Request.CreateResponse(
                             new ApiResponse(
                                 HttpStatusCode.OK, dept, "", 0));
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
