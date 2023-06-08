using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using MMS.Models;
using System.Net.Mail;
using MMS.Security;
using System.Runtime.Serialization.Formatters.Binary;

namespace MMS.Models
{
    public class CreateMeetingViewModel
    {
        private static MMSCGVAKDBEntities db;
        public static string url = "http://202.129.196.131:8085/hurdles/webservices/get_users";
        
        #region Meeting and Task

        //createMeeting and createTask
        public static int? InsertMeeting(MeetingViewModel meetingObj, string meetingDept,
            createMeetingTask meetingTaskObj, int chairPerson, bool istemplate,
            HttpPostedFileBase[] UploadedFile = null)
        {
            int? res = 0;
            int? meetingId;
            int? taskId;
            int? templateID = 0;
            int mailresp = 0;

            List<string> listofParticipants = new List<string>();

            string Meeting_obj = meetingObj.mmsMeetingMaster.Meeting_Objective;
            string Meeting_Type = meetingObj.mmsMeetingMaster.Meeting_Type;

            if (Meeting_Type != "")
            {
                istemplate = false;
               
                List<string> meetingName = MeetingDetailsViewModel.TemplateDetails();

                for(int i=0; i< meetingName.Count; i++)
                {
                    if(meetingName[i] == Meeting_Type)
                    {
                        istemplate = true;
                        templateID = MeetingDetailsViewModel.MeetingTemplateID(meetingName[i]);
                            
                    }
                }   
                
                
            }

            DateTime Meeting_date = meetingObj.mmsMeetingMaster.Meeting_Date;


            string Meeting_time = meetingObj.mmsMeetingMaster.Meeting_Time;
            if (Meeting_time==null)
            {
                Meeting_time = "09:00 AM";
            }
            string Meeting_venue = meetingObj.mmsMeetingMaster.Meeting_Venue;
            
            string Meeting_Duration = meetingObj.mmsMeetingMaster.Meeting_Duration;
            if (Meeting_Duration == null)
            {
                Meeting_Duration = "01:00";
            }
            string Meeting_Chairperson = meetingObj.mmsMeetingMaster.Meeting_Chairperson.ToString();
            string Meeting_Department = meetingDept;
            int NoOfParticipants = meetingObj.mmsMeetingMaster.Meeting_No_Of_Participants;


            Regex regex = new Regex("<[^>]*>", RegexOptions.Compiled);

            string minutesofMeeting = string.Empty;

            if (!string.IsNullOrEmpty(meetingObj.mmsMeetingMaster.Minutes_Of_Meeting))
            {
                var meeting_txt = regex.Replace(meetingObj.mmsMeetingMaster.Minutes_Of_Meeting, string.Empty);
                minutesofMeeting = HttpUtility.HtmlDecode(meeting_txt);
            }


            using (db = new MMSCGVAKDBEntities())
            {
                if (meetingTaskObj.task != null)
                {
                    Meeting_Chairperson = db.Employee_Master.Where(x => x.EmployeeICode == meetingObj.mmsMeetingMaster.Meeting_Chairperson).Select(x=>x.EmployeeDisplayName).FirstOrDefault();                    
                    string meet_chairPerson = Meeting_Chairperson.ToString();
                    meet_chairPerson = db.Employee_Master.Where(x => x.EmployeeDisplayName == meet_chairPerson).Select(x => x.EmployeeICode).FirstOrDefault().ToString();

                    int meetperson = Convert.ToInt16(meetingObj.mmsMeetingMaster.Meeting_Chairperson);
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
                        respPerson = meetingTaskObj.TaskChairman[i].ToString();

                        //respPerson = db.sp_get_chairperson_username(meetperson).FirstOrDefault();
                        string a = string.Empty;
                        Regex r = new Regex("^[a-zA-Z ]+$");
                        if (r.IsMatch(respPerson))
                        {
                            respPerson = db.Employee_Master.Where(x => x.EmployeeDisplayName == respPerson).Select(x => x.EmployeeICode).FirstOrDefault().ToString();
                        }


                        DateTime compl_date;

                        try
                        {


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
                        
                        DateTime ReviewDate = DateTime.Now.AddDays(1);
                        listofParticipants.Clear();
                        listofParticipants.Add(employeeMail);
                        DateTime? tempDt = null;

                        if (meetingTaskObj.reviewdate[i] != "")
                        { tempDt = Convert.ToDateTime(meetingTaskObj.reviewdate[i]); }

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
                             ReviewDate,
                             "1",
                             templateID
                             );
                        taskId = db.MMS_Meeting_Details.OrderByDescending(x => x.Task_Id).Select(x => x.Task_Id).FirstOrDefault();
                        var meeting_status_update_details = db.sp_insert_status_update(meetingId, Convert.ToInt16(respPerson), meetingTaskObj.task[i], 5, taskId);
                        var meeting_status__details = db.sp_insert_Meeting_Status(Convert.ToInt32(respPerson), meetingTaskObj.task[i], Convert.ToInt32(respPerson), DateTime.Now, meetingTaskObj.comments[i], 5, "0.0", Convert.ToInt32(meetingId), Convert.ToInt32(chairPerson), meetperson, taskId);

                        int responsible = Convert.ToInt16(respPerson);
                        int schair_person = Convert.ToInt16(chairPerson);
                        string Participant_Person = db.Employee_Master.Where(x => x.EmployeeICode == responsible).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                        string Chair_Person = db.Employee_Master.Where(x => x.EmployeeICode == schair_person).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                        //mailresp return 1 the Mail Send Sucess or return -1 Not success
                        mailresp = MailFunctionModel.MailMeetingDetails(Participant_Person.ToString(), listofParticipants, meetingTaskObj.task[i], Chair_Person.ToString(), meetingTaskObj.comments[i], Convert.ToInt32(meetingId));
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
                            //string fileName = Meeting_obj + "_" + i + "_" + meetingTaskObj.files[i];
                            string fileName = meetingTaskObj.files[i];
                            res = db.sp_insert_Attachment(meetingId, fileName, extension, fileContents);
                        }

                    }
                }
            }

            return res;
        }

        public static int? InsertMeeting1(MeetingViewModel meetingObj, string meetingDept,
            createMeetingTask meetingTaskObj, int chairPerson, bool istemplate,
            HttpPostedFileBase[] UploadedFile = null)
        {
            int? res = 0;
            int? meetingId;
            int? taskId;
            int? templateID = 0;
            int mailresp = 0;

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
                        istemplate = true;
                        templateID = db.MMS_Meeting_Template.Where(x => x.objective == Meeting_Type).Select(x => x.tid).FirstOrDefault();
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
                minutesofMeeting = HttpUtility.HtmlDecode(meeting_txt);
            }


            using (db = new MMSCGVAKDBEntities())
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
                        mailresp = MailFunctionModel.MailMeetingDetails(Participant_Person.ToString(), listofParticipants, meetingTaskObj.task[i], Chair_Person.ToString(), meetingTaskObj.comments[i], Convert.ToInt32(meetingId));
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



        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;

            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }



        private static void DeleteFile(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPCredentials.FTPRequestUrl + fileName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(FTPCredentials.FTPUserName, FTPCredentials.FTPPassword);
            string responseStr = string.Empty;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                responseStr = response.StatusDescription;
            }
        }

        #endregion

        #region Meeting Template

        //createMeeting and createTask
        public static int InsertMeetingTemplate(MMS_Meeting_Template meetingObj, string meetingDept, string chairPerson, string[] added_participants, int ChairPerson)
        {
            var res = 0;
            List<string> listofParticipants = new List<string>();


            string minutesofMeeting = string.Empty;

            using (db = new MMSCGVAKDBEntities())
            {                
                //meetingObj.meeting_chairperson = db.Employee_Master.Where(x => x.EmployeeDisplayName == ChairPersonJoin).Select(x => x.EmployeeICode).FirstOrDefault();
                meetingObj.meeting_chairperson = ChairPerson;
                db.MMS_Meeting_Template.Add(meetingObj);
                db.SaveChanges();

                MMS_Meeting_Template_Users mmstemp = new MMS_Meeting_Template_Users();

                int lastempId = db.MMS_Meeting_Template.Max(item => item.tid);                
                for (int i = 0; i < added_participants.Length; i++)
                {
                    mmstemp.tid = lastempId;
                    //mmstemp.participants = respPerson;
                    mmstemp.participants = Convert.ToInt32(added_participants[i]);

                    db.MMS_Meeting_Template_Users.Add(mmstemp);
                    db.SaveChanges();
                }                
                res = 1;
            }
            return res;
        }

        #endregion

        #region participants

        // Added JsonData (11/may/2016)
        /// <summary>
        /// Getting participants from Service
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public static dynamic GetParticipants(string deptName)
        {
            List<PseudoName> resultItem = new List<PseudoName>();

            using (db = new MMSCGVAKDBEntities())
            {
                var http = new WebClient();
                string res = http.DownloadString(url);
                List<EmployeeMaster> emp = JsonConvert.DeserializeObject<List<EmployeeMaster>>(res);

                List<sp_get_EmployeeMaster_forJSON_Result> deptId = db.sp_get_EmployeeMaster_forJSON(deptName).ToList();

                if (deptName.Contains("Marketing"))
                {
                    resultItem = (from empList in emp.AsEnumerable()
                                  join deptList in deptId.AsEnumerable()
                                  on empList.employee_icode equals deptList.Employee_Id
                                  where empList.Department_iCode == deptList.Department_ICode
                                  select new PseudoName
                                  {
                                      Employee_Name = string.IsNullOrEmpty(empList.pseudo_name) ? empList.username : empList.pseudo_name,
                                      Employee_Id = empList.employee_icode

                                  }).ToList();
                }
                else
                {
                    resultItem = (from empList in emp.AsEnumerable()
                                  join deptList in deptId.AsEnumerable()
                                  on empList.employee_icode equals deptList.Employee_Id
                                  where empList.Department_iCode == deptList.Department_ICode
                                  select new PseudoName
                                  {
                                      Employee_Name = empList.username,
                                      Employee_Id = empList.employee_icode

                                  }).ToList();
                }
                return resultItem;
            }

        }


        /// <summary>
        /// Get participants from DB
        /// </summary>
        /// <returns></returns>
        public static dynamic GetParticipantsByDepartment(string departmentName)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                List<sp_get_EmployeeMaster_forJSON_Result> departments = db.sp_get_EmployeeMaster_forJSON(departmentName).ToList();

                var employees = db.sp_get_Employee_Department();
                
                return (from e in employees
                        join dept in departments
                        on e.Employee_Code equals dept.Employee_Id
                        where e.Department_Id == dept.Department_ICode
                        orderby e.User_name
                        select new PseudoName
                        {
                            Employee_Name = e.User_name,
                            Employee_Id = e.Employee_Code


                        }).ToList();
            }
        }
        #endregion
       
        #region GetMeetingId

        public static int? GetMeetingId()
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.sp_get_meeting_id().FirstOrDefault();
            }
        }

        #endregion

        //modified this method to bind EmployeeID with Participants of Meeting
        //public static List<MMS_Meeting_Template_Users> GetParticipantId(int tid)

        public static List<TemplateUsersID> GetParticipantId(int tid)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                List<TemplateUsersID> tempemployees = (from post in db.MMS_Meeting_Template_Users
                                                           // join meta in db.Employee_Master on post.participants equals meta.EmployeeFirstName + " " + meta.EmployeeLastName
                                                       join meta in db.Employee_Master on post.participants equals meta.EmployeeICode
                                                       where post.tid == tid
                                                       select new TemplateUsersID
                                                       {
                                                           tid = tid,
                                                           //EmployeeICode = meta.EmployeeICode,

                                                           EmployeeICode = meta.EmployeeICode,
                                                           participants = meta.EmployeeDisplayName

                                                           //EmployeeICode = meta.EmployeeFirstName + " " + meta.EmployeeLastName,
                                                           //participants = meta.EmployeeFirstName + " " + meta.EmployeeLastName
                                                       }).ToList();

                return tempemployees;
            }
        }

        #region Add UserDepartments

        //public static EmployeeMaster AddEmployeeDepartment(string userName, int departmentID, string emailID, int? empCode = null)
        public static EmployeeMaster AddEmployeeDepartment(string userName, int departmentID, string emailID, int Department_Code)
        {
#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
            var result = true;
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used
            EmployeeMaster employeeMaster = null;
            using (db = new MMSCGVAKDBEntities())
            {

                try
                {
                    var employeeDepartment = db.sp_add_Employee_Department(Department_Code, userName, departmentID, emailID).FirstOrDefault();

                    if (employeeDepartment != null)
                        employeeMaster = new EmployeeMaster() { employee_icode = employeeDepartment.Employee_Code ?? 0, username = employeeDepartment.User_name, Department_iCode = employeeDepartment.Department_Id };
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    result = false;
                }
            }
            return employeeMaster;
        }

        #endregion

        #region Get Dept code by Name
        public static int GetDepartmentCodeByName(string departmentName)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                //var lastName = selectedChairperson.Substring(selectedChairperson.IndexOf(' ') + 1);
                //var id = db.sp_get_DepartmentCode_By_Name(departmentName).FirstOrDefault();

                //return id ?? 0;
                return 0;
            }
        }

        #endregion

        #region User Sync

        /// <summary>
        /// Get Employees From synergy using service
        /// </summary>
        /// <returns></returns>

        public static List<EmployeeMaster> GetEmployees()
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["GetUsers"]; //"http://202.129.196.131:8085/hurdles/webservices/get_users";
            var http = new WebClient();
            string res = http.DownloadString(url);
            List<EmployeeMaster> employees = JsonConvert.DeserializeObject<List<EmployeeMaster>>(res);
            return employees;
        }

        public static EmployeeMaster GetEmployeeByEmail(string email,string username)
        {
            EmployeeMaster employeeMaster = null;
            
            using (db = new MMSCGVAKDBEntities())
            {
                //            var CheckUserName = db.Employee_Master
                //.Where(x => x.LoginUserName == username)
                //.Select(x => new { x.EmployeeICode, x.DepartmentICode,x.LoginUserName }).ToList();
                
                var employeeDepartment = db.sp_Get_Employee_By_Email(email).FirstOrDefault();
                if (employeeDepartment != null)
                    employeeMaster = new EmployeeMaster() { employee_icode = employeeDepartment.Employee_Code ?? 0, Department_iCode = employeeDepartment.Department_Id, username = employeeDepartment.User_name };
                //if (CheckUserName != null)
                //    employeeMaster = new EmployeeMaster() { employee_icode = CheckUserName. ?? 0, Department_iCode = employeeDepartment.Department_Id, username = employeeDepartment.User_name };
            }
            return employeeMaster;
        }

        /// <summary>
        /// Get the synched Employees. Employees from MMSDB
        /// </summary>
        /// <returns></returns>
        public static List<EmployeeMaster> GetSynchedEmployees()
        {
            List<sp_get_Employee_Department_Result> employeeDepartments = null;
            List<EmployeeMaster> employeeMasterList = null;
            using (db = new MMSCGVAKDBEntities())
            {
                employeeDepartments = db.sp_get_Employee_Department().ToList();
            }
            if (employeeDepartments != null)

                employeeMasterList = (from e in employeeDepartments
                                      select new EmployeeMaster
                                      {
                                          employee_icode = e.Employee_Code,
                                          Department_iCode = e.Department_Id,
                                          username = e.User_name
                                      }).ToList();
            return employeeMasterList;
        }

        #endregion

        #region  Get Employee Email by empid

        public static string GetEmployeeEmailByCode(int id)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.sp_get_EmailId(id).FirstOrDefault();
            }
        }
        #endregion

        //newly created FOr edit MeetingView--1
        public static dynamic EditMeetingDetails(int? meetingID)
        {
         
            using (db = new MMSCGVAKDBEntities())
            {
                var res = (from meetingMaster in db.MMS_Meeting_Master
                           where meetingMaster.Meeting_Id == meetingID
                           select new
                           {
                               meetingId = meetingMaster.Meeting_Id,
                               meetingObjective = meetingMaster.Meeting_Objective,
                               meetingDate = meetingMaster.Meeting_Date,
                               meetingTime = meetingMaster.Meeting_Time,
                               meetingVenue = meetingMaster.Meeting_Venue,
                               meetingType = meetingMaster.Meeting_Type,
                               meetingDuration = meetingMaster.Meeting_Duration,
                               meetingDepartment = meetingMaster.Meeting_Department,
                               Chairperson = meetingMaster.Meeting_Chairperson.ToString(),
                               minutesOfMeeting = meetingMaster.Minutes_Of_Meeting.ToString(),
                               meetingNumParticipants = meetingMaster.Meeting_No_Of_Participants,
                           }).ToList();

                

                return res;
            }
        }
        //newly created FOr edit MeetingView--2
        public static dynamic EditMeetingParticipants(int? meetingID)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                var res = (from meetingParticipants in db.MMS_Meeting_Participant
                           where meetingParticipants.Meeting_Id == meetingID
                           select new
                           {
                               meetingId = meetingParticipants.Meeting_Id,
                               ParticipantID = meetingParticipants.Participant_Id,
                               Participant_Name = db.Employee_Master.Where(x => x.EmployeeICode == meetingParticipants.Participant_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault()
                           }).ToList();


                return res;
            }
        }

        #region
        public static dynamic FetchTemplate(string selectedObjective)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                // return db.sp_select_Meeting_Template(Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), selectedObjective).ToList().FirstOrDefault();
                //int? meetperson=0;
                MMS_Meeting_Template mmstem = new MMS_Meeting_Template();




                var res = (from mmsmeetingtemplate in db.MMS_Meeting_Template
                           where mmsmeetingtemplate.meeting_name == selectedObjective
                           select new
                           {
                               meetperson = mmsmeetingtemplate.meeting_chairperson,

                               department = mmsmeetingtemplate.department,
                                meeting_chairperson = db.Employee_Master.Where(x => x.EmployeeICode == mmsmeetingtemplate.meeting_chairperson).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                              // meeting_chairperson = db.Employee_Master.Where(x => x.EmployeeICode == mmsmeetingtemplate.meeting_chairperson).Select(x => x.EmployeeFirstName + " " + x.EmployeeLastName).FirstOrDefault(),
                               meeting_duration = mmsmeetingtemplate.meeting_duration,
                               meeting_name = mmsmeetingtemplate.meeting_name,
                               meeting_time = mmsmeetingtemplate.meeting_time,
                               meeting_venue = mmsmeetingtemplate.meeting_venue,
                               objective = mmsmeetingtemplate.objective,
                               tid = mmsmeetingtemplate.tid
                           }).ToList();


                string a = string.Empty;
                return res;
            }
        }
        #endregion
    }

    public class createMeetingTask
    {
        public string[] task { get; set; }
        public string[] empId { get; set; }
        public string[] priority { get; set; }
        public string[] comments { get; set; }
        public string[] completionDate { get; set; }
        public string[] category { get; set; }
        public string[] files { get; set; }
        public string[] reviewFreq { get; set; }
        public string[] reviewdate { get; set; }
        public string[] TaskChairman { get; set; }        
        public bool? Review_Status { get; set; }
    }
    /// <summary>
    /// New changes for inserting task without split functionality update by Simon
    /// </summary>
    public class createMeetingViewModal
    {
        public string task { get; set; }
        public string empId { get; set; }
        public string priority { get; set; }
        public string comments { get; set; }
        public string completionDate { get; set; }
        public string category { get; set; }
        public string files { get; set; }
        public string reviewFreq { get; set; }
        public string reviewdate { get; set; }
        public string TaskChairman { get; set; }
        public bool Review_Status { get; set; }
    }

    public class EmployeeMaster
    {
        [JsonProperty("employee_icode")]
        public int employee_icode { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("pseudo_name")]
        public string pseudo_name { get; set; }

        [JsonProperty("department_id")]
        public int Department_iCode { get; set; }
    }

    public class PseudoName
    {
        public string Employee_Name { get; set; }

        public int Employee_Id { get; set; }
    }

    //create new class for 'MMS_Meeting_Template_Users' & 'Employee_Master' JOIN
    public class TemplateUsersID
    {
        MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public Nullable<int> tid { get; set; }

        public string participants { get; set; }
        
        public int EmployeeICode { get; set; }
    }
}