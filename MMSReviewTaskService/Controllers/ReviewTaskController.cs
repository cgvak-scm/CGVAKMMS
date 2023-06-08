using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MMSService.Models;
using Newtonsoft.Json;

namespace MMSService.Controllers
{    
    public class ReviewTaskController : ApiController
    {
        private ReviewTaskEntities db;

        //[HttpGet]
        // GET: api/ReviewTask
        //public List<ReviewTaskViewModal> GetMyTasks(int chairperson, int Category_id, int Participant, string PriorityType)
        //{

        //    var RetMsg = string.Empty;            

        //    using (db = new ReviewTaskEntities())
        //    {
        //        var result = (from res in db.Proc_Get_Review_Tasks(chairperson, Category_id, Participant, PriorityType)

        //                        select new ReviewTaskViewModal
        //                        {
        //                            Task_Id = res.Task_Id,
        //                            Meeting_Id = res.Meeting_Id,
        //                            Employee_Id = res.Meeting_Id,
        //                            Chairperson = res.Meeting_Id,
        //                            Participant = res.Meeting_Id,
        //                            Task = res.Task,
        //                            Category = res.Task,
        //                            Completion_Date = res.Completion_Date,
        //                            Priority = res.Completion_Date,
        //                            Status = res.Completion_Date,
        //                            Comments = res.Completion_Date,
        //                            Review_Status = res.Review_Status,
        //                            Review_Date = res.Review_Date,
        //                            Assigned_Date = res.Review_Date,
        //                            Overdue_days = res.Review_Date

        //                        }
        //                        ).OrderBy(x => x.Task).ToList();

        //        return result;
        //    }                

        //}

        //[HttpGet]
        //// GET: api/ReviewTask
        //public List<ReviewTaskParticipantViewModel> GetAllParticipants(int? chairperson)
        //{
        //    using (db = new ReviewTaskEntities())
        //    {
        //        var res= (from meetingDetails in db.MMS_Meeting_Details
        //                join empMaster in db.Employee_Master on meetingDetails.Participant equals empMaster.EmployeeICode
        //                where meetingDetails.Chairperson == chairperson
        //                  select new ReviewTaskParticipantViewModel
        //                {
        //                    employee_id = meetingDetails.Employee_Id,
        //                    name = empMaster.EmployeeFirstName + " " + empMaster.EmployeeLastName,
        //                    Participant = meetingDetails.Participant
        //                }).Distinct().OrderBy(x => x.name).ToList();

        //        return res;
        //    }
        //}


        [HttpGet]
        // GET: api/ReviewTask
        public HttpResponseMessage GetAllParticipants(int? chairperson)
        {
            using (db = new ReviewTaskEntities())
            {
                var res = (from meetingDetails in db.MMS_Meeting_Details
                           join empMaster in db.Employee_Master on meetingDetails.Participant equals empMaster.EmployeeICode
                           where meetingDetails.Chairperson == chairperson
                           select new ReviewTaskParticipantViewModel
                           {
                               employee_id = meetingDetails.Employee_Id,
                               name = empMaster.EmployeeFirstName + " " + empMaster.EmployeeLastName,
                               Participant = meetingDetails.Participant
                           }).Distinct().OrderBy(x => x.name).ToList();
                
                if (res == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Invalid ID");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, res);
            }
        }

        //[HttpGet]
        // GET: api/ReviewTask
        //public List<ReviewTaskCategoryViewModel> GetAllCategory()
        //{                        
        //    using (db = new ReviewTaskEntities())
        //    {
        //        return (from MMC in db.MMS_Meeting_Category
        //                select new ReviewTaskCategoryViewModel
        //                {
        //                    ID = MMC.ID,
        //                    CategoryName = MMC.CategoryName

        //                }).OrderBy(x => x.CategoryName).ToList();                
        //    }            
        //}               
    }
}
