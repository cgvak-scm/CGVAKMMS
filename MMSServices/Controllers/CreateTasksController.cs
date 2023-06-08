using MMSServices.Helper;
using MMSServices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MMSServices.Controllers
{
    [MMSApiAuthorize]
    [RoutePrefix("api/CreateTasks")]
    public class CreateTasksController : ApiController
    {
        private ReviewTaskDBEntities db;
        ApiResponse apiResponse = null;

        /// <summary>
        /// Posting Create tasks
        /// </summary>
        /// <returns></returns>
        [Route("PostAll")]
        [HttpPost]
        public HttpResponseMessage PostAll(MeetingViewModel meetingViewObj, FormDataCollection frm, System.Web.HttpPostedFileBase[] uploadFile)
        {
           
                createMeetingTask meetingTaskObj = new createMeetingTask();

                string meetingDepartment = meetingViewObj.mmsMeetingMaster.Meeting_Department == null ?
                                       frm["Meeting_other_dept"] : meetingViewObj.mmsMeetingMaster.Meeting_Department;
                meetingTaskObj.empId = frm["Participant"].Split(',');
                meetingTaskObj.priority = frm["Priority"].Split(',');
                var taskval = frm["task"];
                var trimtask = taskval.Trim();
                meetingTaskObj.comments = frm["Comments"].Split(',');
                meetingTaskObj.task = frm["task"].Split(',');

                // meetingTaskObj.reviewFreq = Request.Form["frequency_1"].Split(',');
                // meetingTaskObj.reviewdate = Request.Form["revDt"].Split(',');

                meetingTaskObj.completionDate = frm["Completion_Date"].Split(',');
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


                meetingTaskObj.category = frm["Category"].Split(',');
                meetingTaskObj.files = frm["files"].Split(',');
                int reviewF = Convert.ToInt32(frm["frequency_"]);
                meetingTaskObj.reviewFreq = frm["frequency"].Split(',');

                meetingTaskObj.reviewdate = frm["ReviewDate"].Split(',');

                //Modified to correct Date Format Issue
                DateTime dt = DateTime.Now;
                String strDate = "";
                strDate = dt.ToString("MM/dd/yyyy");
                String formatDate = strDate.Replace('-', '/');

                if (meetingDepartment != null)
                {
                    var TaskTemplate = (from resMTemplate in db.MMS_Meeting_Template
                                        where resMTemplate.department == meetingDepartment
                                        select new
                                        {
                                            MeetingTime = resMTemplate.meeting_time,
                                            MeetingDuration = resMTemplate.meeting_duration,
                                            MeetingObjective = resMTemplate.objective,
                                            MeetingVenue = resMTemplate.meeting_venue
                                        }).FirstOrDefault();

                    meetingViewObj.mmsMeetingMaster.Meeting_Time = TaskTemplate.MeetingTime;
                    meetingViewObj.mmsMeetingMaster.Meeting_Duration = TaskTemplate.MeetingDuration;
                    meetingViewObj.mmsMeetingMaster.Meeting_Objective = TaskTemplate.MeetingObjective;
                    meetingViewObj.mmsMeetingMaster.Meeting_Venue = TaskTemplate.MeetingVenue;
                }
                else
                {
                    meetingViewObj.mmsMeetingMaster.Meeting_Time = "0.00";
                    meetingViewObj.mmsMeetingMaster.Meeting_Duration = "0.00";
                    meetingViewObj.mmsMeetingMaster.Meeting_Objective = "Test Task";
                    meetingViewObj.mmsMeetingMaster.Meeting_Venue = string.Empty;
                }
                
                meetingViewObj.mmsMeetingMaster.Meeting_Date = DateTime.ParseExact(formatDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                meetingViewObj.mmsMeetingMaster.Meeting_Chairperson = Convert.ToInt16(3);

                meetingViewObj.mmsMeetingMaster.Meeting_Department = frm["DepartmentTypeID"];
                meetingViewObj.mmsMeetingMaster.Meeting_Type = string.Empty;
                meetingViewObj.mmsMeetingMaster.Minutes_Of_Meeting = string.Empty;
            
                try
                {
                    int? res = CreateMeetingViewModel.InsertMeeting1(meetingViewObj, meetingDepartment, meetingTaskObj, Convert.ToInt16(3), false, uploadFile);
                
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

    internal class CreateMeetingViewModel
    {

        public static int? InsertMeeting1(MeetingViewModel meetingObj, string meetingDept,
           createMeetingTask meetingTaskObj, int chairPerson, bool istemplate,
           System.Web.HttpPostedFileBase[] UploadedFile = null)
        {
            int? res = 0;
            int? meetingId;
            int? taskId;
            int? templateID = 0;
           // int mailresp = 0;

            List<string> listofParticipants = new List<string>();

            string Meeting_obj = meetingObj.mmsMeetingMaster.Meeting_Objective;
            string Meeting_Type = meetingObj.mmsMeetingMaster.Meeting_Type;
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

            DateTime Meeting_date = meetingObj.mmsMeetingMaster.Meeting_Date;


            string Meeting_time = meetingObj.mmsMeetingMaster.Meeting_Time;
            string Meeting_venue = meetingObj.mmsMeetingMaster.Meeting_Venue;
            string Meeting_Duration = meetingObj.mmsMeetingMaster.Meeting_Duration;
            string Meeting_Chairperson = meetingObj.mmsMeetingMaster.Meeting_Chairperson.ToString();
            string Meeting_Department = meetingDept;
            int NoOfParticipants = meetingObj.mmsMeetingMaster.Meeting_No_Of_Participants;


            Regex regex = new Regex("<[^>]*>", RegexOptions.Compiled);

            string minutesofMeeting = string.Empty;

            if (!string.IsNullOrEmpty(meetingObj.mmsMeetingMaster.Minutes_Of_Meeting))
            {
                var meeting_txt = regex.Replace(meetingObj.mmsMeetingMaster.Minutes_Of_Meeting, string.Empty);
                minutesofMeeting = System.Web.HttpUtility.HtmlDecode(meeting_txt);
            }


            using (ReviewTaskDBEntities db = new ReviewTaskDBEntities())
            {
                if (meetingTaskObj.task != null)
                {
                    int? ChairpersonId = Convert.ToInt16(meetingObj.mmsMeetingMaster.Meeting_Chairperson);
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


                    for (int i = 0; i < meetingTaskObj.task.Count(); i++)
                    {
                        //Get chairperson name by passing employeeId
                        string respPerson = string.Empty;
#pragma warning disable CS0219 // The variable 'userDepartment' is assigned but its value is never used
                        //sp_Get_Employee_Department_By_Id_Result userDepartment = null;
#pragma warning restore CS0219 // The variable 'userDepartment' is assigned but its value is never used

                        //respPerson = db.sp_get_chairperson_username(Convert.ToInt32(meetingTaskObj.empId[i])).FirstOrDefault();//Commented. Instead of binding EmployeeID now binding UserDepartmentID in Task create page
                        respPerson = meetingTaskObj.empId[i].ToString();

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

                        try
                        {
                            //Changed For Date Format Issue
                            compl_date = DateTime.ParseExact(meetingTaskObj.completionDate[i], "MM/dd/yyyy",
                                CultureInfo.InvariantCulture);
                        }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                        catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                        {
                            compl_date = Convert.ToDateTime(meetingTaskObj.completionDate[i]);
                        }

                        string employeeMail = db.sp_get_EmailId(Convert.ToInt32(respPerson)).FirstOrDefault();
                        //string employeeMail = db.sp_get_EmailId(meetperson).FirstOrDefault();

                        listofParticipants.Clear();
                        listofParticipants.Add(employeeMail);
                        DateTime? tempDt = null;
                        if (meetingTaskObj.reviewdate[i] != "")
                        { tempDt = Convert.ToDateTime(meetingTaskObj.reviewdate[i]); }
                        //inserting into meeting details  table
                        db.sp_insert_MeetingDetails1(
                             Convert.ToInt32(meetingId),
                             Convert.ToInt32(respPerson),
                             Convert.ToInt32(chairPerson),
                             Convert.ToInt16(respPerson),
                             meetingTaskObj.task[i],
                             compl_date,
                             meetingTaskObj.priority[i],
                             5,
                             meetingTaskObj.comments[i],
                             meetingTaskObj.category[i],
                             "false",
                             false,
                             istemplate,
                             tempDt,
                             meetingTaskObj.reviewFreq[i],
                             templateID
                             );
                        taskId = db.MMS_Meeting_Details.OrderByDescending(x => x.Task_Id).Select(x => x.Task_Id).FirstOrDefault();

                        //inserting into meeting status update table                        
                        var meeting_status_update_details = db.sp_insert_status_update(meetingId, Convert.ToInt16(respPerson), meetingTaskObj.task[i], 5, taskId);

                        var meeting_status__details = db.sp_insert_Meeting_Status(Convert.ToInt32(respPerson), meetingTaskObj.task[i], Convert.ToInt32(respPerson), DateTime.Now, meetingTaskObj.comments[i], 5, "0.0", Convert.ToInt32(meetingId), Convert.ToInt32(chairPerson), meetperson, taskId);


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
                    for (int k = 0; k < meetingTaskObj.empId.Count(); k++)
                    {
                        MeetParticipants.Meeting_Id = meetid1;
                        MeetParticipants.Participant_Id = Convert.ToInt32(meetingTaskObj.empId[k]);
                        MeetParticipants.Participant_Name = meetingTaskObj.empId[k];// respPerson;
                        db.MMS_Meeting_Participant.Add(MeetParticipants);
                        db.SaveChanges();
                    }


                    if (meetingTaskObj.files[0] != "")
                    {
                        for (int i = 0; i < meetingTaskObj.files.Count(); i++)
                        {
                            byte[] fileContents = null;

                            BinaryFormatter bf = new BinaryFormatter();
                            MemoryStream ms = new MemoryStream();
                            bf.Serialize(ms, meetingTaskObj.files);
                            fileContents = ms.ToArray();

                            string extension = Path.GetExtension(meetingTaskObj.files[i]);
                            string fileName = meetingTaskObj.files[i];
                            res = db.sp_insert_Attachment(meetingId, fileName, extension, fileContents);
                        }

                    }
                }

            }

            return res;
        }
    }
}
