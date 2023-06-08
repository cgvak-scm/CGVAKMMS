using MMSServices.Helper;
using MMSServices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/TaskCreation")]
    public class TaskCreationController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;

        /// <summary>
        /// Posting Create tasks
        /// </summary>
        /// <returns></returns>
        [Route("PostAll")]
        [HttpPost]
        public HttpResponseMessage PostAll(TaskDetailsForApp meetingTaskObj)
        {
            createMeetingTask meetingTaskObjs = new createMeetingTask();

            //MeetingViewModel meetingViewObject = new MeetingViewModel();

            MeetingViewModel meetingViewObject = new MeetingViewModel();

            string meetingDepartment = meetingTaskObj.Department;
            meetingTaskObjs.empId = meetingTaskObj.Employees;
            var taskval = meetingTaskObj.TaskLIst[0].Task;
            var trimtask = taskval.Trim();
          
            // meetingTaskObj.reviewFreq = Request.Form["frequency_1"].Split(',');
            // meetingTaskObj.reviewdate = Request.Form["revDt"].Split(',');

            string[] k = Regex.Split(trimtask, "~,");
            string[] task = new string[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                if (i == k.Length - 1 && k[i].Contains("~"))
                {
                    task[i] = Regex.Replace(k[i], "~", "");
                    task[i] = task[i].Trim();
                }
                else
                {

                    task[i] = k[i].Trim();
                }
            }

            // meetingTaskObj.task = task;



            //Modified to correct Date Format Issue
            DateTime dt = DateTime.Now;
            String strDate = "";
            strDate = dt.ToString("MM/dd/yyyy");
            String formatDate = strDate.Replace('-', '/');

            //if (meetingDepartment != null)
            //{
            //    var TaskTemplate = (from resMTemplate in db.MMS_Meeting_Template
            //                        where resMTemplate.department == meetingDepartment
            //                        select new
            //                        {
            //                            MeetingTime = resMTemplate.meeting_time,
            //                            MeetingDuration = resMTemplate.meeting_duration,
            //                            MeetingObjective = resMTemplate.objective,
            //                            MeetingVenue = resMTemplate.meeting_venue
            //                        }).FirstOrDefault();

            //    meetingViewObject.mmsMeetingMaster.Meeting_Time = TaskTemplate.MeetingTime;
            //    meetingViewObject.mmsMeetingMaster.Meeting_Duration = TaskTemplate.MeetingDuration;
            //    meetingViewObject.mmsMeetingMaster.Meeting_Objective = TaskTemplate.MeetingObjective;
            //    meetingViewObject.mmsMeetingMaster.Meeting_Venue = TaskTemplate.MeetingVenue;
            //}
            //else
            //{
            //    meetingViewObject.mmsMeetingMaster.Meeting_Time = "0.00";
            //    meetingViewObject.mmsMeetingMaster.Meeting_Duration = "0.00";
            //    meetingViewObject.mmsMeetingMaster.Meeting_Objective = "Test Task";
            //    meetingViewObject.mmsMeetingMaster.Meeting_Venue = string.Empty;
            //}
            //meetingViewObject.mmsMeetingMaster.Meeting_Date = DateTime.ParseExact(formatDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //meetingViewObject.mmsMeetingMaster.Meeting_Chairperson = Convert.ToInt16(3);

            //meetingViewObject.mmsMeetingMaster.Meeting_Department = meetingDepartment;
            //meetingViewObject.mmsMeetingMaster.Meeting_Type = string.Empty;
            //meetingViewObject.mmsMeetingMaster.Minutes_Of_Meeting = string.Empty;

            //Added meeting details
            MMS_Meeting_Master mMS_Meeting_Master = new MMS_Meeting_Master();

            mMS_Meeting_Master.Meeting_Time = "0.00";
            mMS_Meeting_Master.Meeting_Duration = "0.00";
            mMS_Meeting_Master.Meeting_Objective = "Test Task";
            mMS_Meeting_Master.Meeting_Venue = string.Empty;
            mMS_Meeting_Master.Meeting_Date = DateTime.ParseExact(formatDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            mMS_Meeting_Master.Meeting_Chairperson = Convert.ToInt16(3);
            mMS_Meeting_Master.Meeting_Department = meetingDepartment;
            mMS_Meeting_Master.Meeting_Type = string.Empty;
            mMS_Meeting_Master.Minutes_Of_Meeting = string.Empty;

            try
            {
                int? res = CreateMeetingViewModelNew.InsertMeetingTask(mMS_Meeting_Master, meetingDepartment, meetingTaskObj, Convert.ToInt16(3), false);

                using (db = new ReviewTaskDBEntities())
                {

                    if (res == null)
                        return Request.CreateResponse(
                            new ApiResponse(
                                HttpStatusCode.NotFound, res, "Create task cannot be done", 0)
                      );
                    else
                        return Request.CreateResponse(
                           new ApiResponse(
                               HttpStatusCode.OK, res, "Success", 0)
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


        public void CreateTaskForAPP(TaskDetailsForApp taskDetailsForApp)
        {
            taskDetailsForApp.Department = "Marketing";
            //taskDetailsForApp.Employees = "538";
            Tasks tasks = new Tasks();
            tasks.Task = "tesst";
            tasks.Category = 17;
            tasks.ReviewFrequencyID = 1;
            tasks.Completion_Date = DateTime.Now;
            tasks.Comments = "test";
            tasks.Priority = "";
            tasks.NextReviewDate = DateTime.Now.AddDays(5);
            taskDetailsForApp.TaskLIst.Add(tasks);
        }

    }

    internal class CreateMeetingViewModelNew
    {
        //public static int? InsertMeetingTask(MeetingViewModel meetingObj, string meetingDept,
        // createMeetingTask meetingTaskObj, int chairPerson, bool istemplate)
        public static int? InsertMeetingTask(MMS_Meeting_Master meetingObj, string meetingDept,
           TaskDetailsForApp meetingTaskObj, int chairPerson, bool istemplate)
        {
            int? res = 0;
            int? meetingId;
            int? taskId;
            int? templateID = 0;
            // int mailresp = 0;

            List<string> listofParticipants = new List<string>();

            string Meeting_obj = meetingObj.Meeting_Objective;
            string Meeting_Type = meetingObj.Meeting_Type;
            if (Meeting_Type != "")
            {
                istemplate = false;
                List<string> meetingObjective = MeetingDetailsViewModel.TemplateDetails();

                for (int i = 0; i < meetingObjective.Count; i++)
                {
                    if (meetingObjective[i] == Meeting_Type)
                    {
                        using (ReviewTaskDBEntities db = new ReviewTaskDBEntities())
                        {
                            istemplate = true;
                            templateID = db.MMS_Meeting_Template.Where(x => x.objective == Meeting_Type).Select(x => x.tid).FirstOrDefault();
                        }
                    }
                }
            }
            else
            {
                templateID = -1;
            }

            //DateTime Meeting_date = meetingObj.mmsMeetingMaster.Meeting_Date;
            //string Meeting_time = meetingObj.mmsMeetingMaster.Meeting_Time;
            //string Meeting_venue = meetingObj.mmsMeetingMaster.Meeting_Venue;
            //string Meeting_Duration = meetingObj.mmsMeetingMaster.Meeting_Duration;
            //string Meeting_Chairperson = meetingObj.mmsMeetingMaster.Meeting_Chairperson.ToString();
            //string Meeting_Department = meetingDept;
            //int NoOfParticipants = meetingObj.mmsMeetingMaster.Meeting_No_Of_Participants;

            DateTime Meeting_date = meetingObj.Meeting_Date;
            string Meeting_time = meetingObj.Meeting_Time;
            string Meeting_venue = meetingObj.Meeting_Venue;
            string Meeting_Duration = meetingObj.Meeting_Duration;
            string Meeting_Chairperson = meetingObj.Meeting_Chairperson.ToString();
            string Meeting_Department = meetingDept;
            int NoOfParticipants = meetingObj.Meeting_No_Of_Participants;


            Regex regex = new Regex("<[^>]*>", RegexOptions.Compiled);

            string minutesofMeeting = string.Empty;

            if (!string.IsNullOrEmpty(meetingObj.Minutes_Of_Meeting))
            {
                var meeting_txt = regex.Replace(meetingObj.Minutes_Of_Meeting, string.Empty);
                minutesofMeeting = System.Web.HttpUtility.HtmlDecode(meeting_txt);
            }


            using (ReviewTaskDBEntities db = new ReviewTaskDBEntities())
            {
                if (meetingTaskObj.TaskLIst[0].Task != null)
                {
                    int? ChairpersonId = Convert.ToInt16(meetingObj.Meeting_Chairperson);
                    //Remove the Stored Procedure
                    Meeting_Chairperson = db.Employee_Master.Where(x => x.EmployeeICode == ChairpersonId).Select(x => x.EmployeeDisplayName).FirstOrDefault().ToString();

                    string meet_chairPerson = Meeting_Chairperson;
                    meet_chairPerson = db.Employee_Master.Where(x => x.EmployeeDisplayName == meet_chairPerson).Select(x => x.EmployeeICode).FirstOrDefault().ToString();

                    int meetperson = Convert.ToInt16(meet_chairPerson);
                    //inserting into meeting master table
                    res = db.sp_insert_MeetingMaster(Meeting_obj, Meeting_date, Meeting_time, Meeting_venue, Meeting_Type,
                        Meeting_Duration,
                        Meeting_Department, Convert.ToInt16(meet_chairPerson), minutesofMeeting, NoOfParticipants, istemplate, templateID).FirstOrDefault();

                    //Get Max Meeting Id
                    meetingId = res;

                    //Inserting for notification
                    db.sp_add_mms_notifications(meetingId, chairPerson);


                    for (int i = 0; i < meetingTaskObj.TaskLIst.Count(); i++)
                    {
                        //Get chairperson name by passing employeeId
                        string respPerson = string.Empty;
#pragma warning disable CS0219 // The variable 'userDepartment' is assigned but its value is never used
                        //sp_Get_Employee_Department_By_Id_Result userDepartment = null;
#pragma warning restore CS0219 // The variable 'userDepartment' is assigned but its value is never used

                        //respPerson = db.sp_get_chairperson_username(Convert.ToInt32(meetingTaskObj.empId[i])).FirstOrDefault();//Commented. Instead of binding EmployeeID now binding UserDepartmentID in Task create page
                        respPerson = meetingTaskObj.TaskLIst[i].Employee_Id.ToString();

                        //respPerson = db.sp_get_chairperson_username(meetperson).FirstOrDefault();
                        string a = string.Empty;


                        //Regex objAlphaPattern = new Regex(@"^\d+$");

                        Regex r = new Regex("^[a-zA-Z ]+$");
                        if (r.IsMatch(respPerson))
                        {
                            respPerson = db.Employee_Master.Where(x => x.EmployeeDisplayName == respPerson).Select(x => x.EmployeeICode).FirstOrDefault().ToString();
                        }

                        //Parsing datetime
                        DateTime compl_date;

//                        try
//                        {
//                            //Changed For Date Format Issue
//                            compl_date = DateTime.ParseExact(meetingTaskObj.TaskLIst[i].Completion_Date, "MM/dd/yyyy",
//                                CultureInfo.InvariantCulture);
//                            compl_date = meetingTaskObj.TaskLIst[i].Completion_Date;
//                        }
//#pragma warning disable CS0168 // The variable 'e' is declared but never used
//                        catch (Exception e)
//#pragma warning restore CS0168 // The variable 'e' is declared but never used
//                        {
//                            compl_date = Convert.ToDateTime(meetingTaskObj.completionDate[i]);
//                        }

                        compl_date = Convert.ToDateTime(meetingTaskObj.TaskLIst[i].Completion_Date);

                        string employeeMail = db.sp_get_EmailId(Convert.ToInt32(respPerson)).FirstOrDefault();
                        //string employeeMail = db.sp_get_EmailId(meetperson).FirstOrDefault();

                        listofParticipants.Clear();
                        listofParticipants.Add(employeeMail);
                        DateTime? tempDt = null;
                        if (Convert.ToString(meetingTaskObj.TaskLIst[i].NextReviewDate)!= "")
                        { tempDt = Convert.ToDateTime(meetingTaskObj.TaskLIst[i].NextReviewDate); }
                        //inserting into meeting details  table
                        db.sp_insert_MeetingDetails1(
                             Convert.ToInt32(meetingId),
                             Convert.ToInt32(respPerson),
                             Convert.ToInt32(chairPerson),
                             Convert.ToInt16(respPerson),
                             meetingTaskObj.TaskLIst[i].Task,
                             compl_date,
                             meetingTaskObj.TaskLIst[i].Priority,
                             5,
                             meetingTaskObj.TaskLIst[i].Comments,
                             Convert.ToString(meetingTaskObj.TaskLIst[i].Category),
                             "false",
                             false,
                             istemplate,
                             tempDt,
                             Convert.ToString(meetingTaskObj.TaskLIst[i].ReviewFrequencyID),
                             templateID
                             );
                        taskId = db.MMS_Meeting_Details.OrderByDescending(x => x.Task_Id).Select(x => x.Task_Id).FirstOrDefault();

                        //inserting into meeting status update table                        
                        var meeting_status_update_details = db.sp_insert_status_update(meetingId, Convert.ToInt16(respPerson),meetingTaskObj.TaskLIst[i].Task, 5, taskId);

                        var meeting_status__details = db.sp_insert_Meeting_Status(Convert.ToInt32(respPerson), meetingTaskObj.TaskLIst[i].Task, Convert.ToInt32(respPerson), DateTime.Now, meetingTaskObj.TaskLIst[i].Comments, 5, "0.0", Convert.ToInt32(meetingId), Convert.ToInt32(chairPerson), meetperson, taskId);


                        //mail function to be called         
                        //Because in mail, the Id of the respective person gets passed and is displayed. So to change it, the name is got and passed.
                        int responsible = Convert.ToInt16(respPerson);
                        int schair_person = Convert.ToInt16(chairPerson);
                        string Participant_Person = db.Employee_Master.Where(x => x.EmployeeICode == responsible).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                        string Chair_Person = db.Employee_Master.Where(x => x.EmployeeICode == schair_person).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                        //mailresp return 1 the Mail Send Sucess or return -1 Not success
                        // mailresp = MailFunctionModel.MailMeetingDetails(Participant_Person.ToString(), listofParticipants, meetingTaskObj.task[i], Chair_Person.ToString(), meetingTaskObj.comments[i], Convert.ToInt32(meetingId));
                    }
                    MMS_Meeting_Participant MeetParticipants = new MMS_Meeting_Participant();
                    var meetid1 = db.MMS_Meeting_Master.Max(item => item.Meeting_Id);
                    for (int k = 0; k < meetingTaskObj.Employees.Count(); k++)
                    {
                        MeetParticipants.Meeting_Id = meetid1;
                        MeetParticipants.Participant_Id = Convert.ToInt32(meetingTaskObj.Employees[k]);
                        MeetParticipants.Participant_Name = meetingTaskObj.Employees[k];// respPerson;
                        db.MMS_Meeting_Participant.Add(MeetParticipants);
                        db.SaveChanges();
                    }


                    //if (meetingTaskObj.files[0] != "")
                    //{
                    //    for (int i = 0; i < meetingTaskObj.files.Count(); i++)
                    //    {
                    //        byte[] fileContents = null;

                    //        BinaryFormatter bf = new BinaryFormatter();
                    //        MemoryStream ms = new MemoryStream();
                    //        bf.Serialize(ms, meetingTaskObj.files);
                    //        fileContents = ms.ToArray();

                    //        string extension = Path.GetExtension(meetingTaskObj.files[i]);
                    //        string fileName = meetingTaskObj.files[i];
                    //        res = db.sp_insert_Attachment(meetingId, fileName, extension, fileContents);
                    //    }

                    //}
                }

            }

            return res;
        }
    }

}
