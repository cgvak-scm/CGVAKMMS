using MMS.Models.ApiModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;


namespace MMS.Models
{

    public class MeetingViewModel
    {
        public MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public MMS_Meeting_Master mmsMeetingMaster { get; set; }                 

        public string Meetmaster
        {
            get
            {
                string meet_Chairperson = string.Empty;
                return meet_Chairperson = db.Employee_Master.Where(x => x.EmployeeICode == mmsMeetingMaster.Meeting_Chairperson).Select(x => x.EmployeeDisplayName).ToString();
            }
        }

        public Designation_Master Designationmaster { get; set; }

        public MMS_Meeting_Details mmsMeetingDetails { get; set; }

        public List<MMS_Meeting_Participant> Participant { get; set; }

    }

    //public class MeetingViewModel
    //{
    //    public List<MMS_Meeting_Details> mmsMeetingDetails { get; set; }
    //}

        public class comment
    {
        MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public string commented_By { get; set; }

        public string commented_person
        {
            get
            {
                int responsible_forcomments = Convert.ToInt16(commented_By);
                return (db.Employee_Master.Where(x => x.EmployeeICode == responsible_forcomments)).Select(x => x.EmployeeDisplayName).FirstOrDefault();
            }
        }
        public string commented_date { get; set; }

        public string comments { get; set; }
    }

    public class MeetingEditDetails
    {
        MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        public int meetingId { get; set; }
        public int TaskID { get; set; }
        public string Task { get; set; }
        public string RespPerson { get; set; }

        public string ResponsiblePerson
        {
            get
            {
                int res = Convert.ToInt16(RespPerson);
                return (db.Employee_Master.Where(x => x.EmployeeICode == res)).Select(x => x.EmployeeDisplayName).FirstOrDefault();
            }
        }
        public int empId { get; set; }

        //public string RespPerson
        //{
        //    get
        //    {
        //        return (RespPerson != null ? db.Employee_Master.Where(x => x.EmployeeICode == empId).Select(x => x.EmployeeDisplayName) : null).ToString();

        //    }
        //}


        public string Comments { get; set; }

        public string CompletionDate { get; set; }

        // public List<SelectListItem> Priority { get; set; }
        public string Priority { get; set; }

        public string Prio
        {
            get
            {
                return (Priority != null ? Priority : null).ToString();
            }
        }
        //public string Status { get; set; }
        public int Status { get; set; }

        public string selectedCategory { get; set; }
        public int selectedCategoryID { get; set; }
        public string ChairPerson { get; set; }
        public List<comment> Comment { get; set; }
        public IEnumerable<SelectListItemHelper> GetPriorityList { get; set; }
        public List<DDLViewModel> priorityTypes { get; set; }

        public string PriorityTypesSelected { get; set; }
        //
        public string ReviewDate { get; set; }

        //


        public static IEnumerable<SelectListItem> GetPriority()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "High", Value = "High"},
                new SelectListItem{Text = "Medium", Value = "Medium"},
                new SelectListItem{Text = "Low", Value = "Low"},
            };
            return items;
        }

    }

    public class SelectListItemHelper
    {

    }

    public class DDLViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
    ///for Editing Meeting Details--venu,time,..
    ///
    public class ViewUpdateMeetingDetails
    {

        public int? meetingId { get; set; }

        public string meetingObjective { get; set; }

        public string meetingDate { get; set; }

        public string meetingTime { get; set; }
        public string meetingVenue { get; set; }

        public string meetingType { get; set; }

        public string meetingDuration { get; set; }

        public string meetingDepartment { get; set; }

        public string Chairperson { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string minutesOfMeeting { get; set; }

        public int meetingNumParticipants { get; set; }

        public string[] Participant_Name { get; set; }

        public int[] ParticipantID { get; set; }

        //public string participantsName { get; set; }

        //public int ParticipantID { get; set; }

        //public List<UpdateMeetingTask> UploadedFiles { get; set; }
        
    }

    //public class TimeTakenReportsViewDetails
    //{
    //    public int? EmployeeID { get; set; }
    //    public int? Completed { get; set; }
    //    public int? INPROGRESS { get; set; }
    //    public int? TotalTasks { get; set; }
    //    public int? AssignedTasks { get; set; }
    //    public int? AVERAGEDays { get; set; }
    //    public int? MAXIMUMDays { get; set; }
    //    public int? MINIMUMDays { get; set; }
    //}



    public class EditMeetingPage
    {
        MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();

        public int meetingId { get; set; }

        public string meetingObjective { get; set; }

        public string meetingDate { get; set; }

        public string meetingTime { get; set; }
        public string meetingVenue { get; set; }

        public string meetingType { get; set; }


        public string meetingDuration { get; set; }

        public string meetingDepartment { get; set; }

        public string Chairperson { get; set; }

        public string minutesOfMeeting { get; set; }

        public int meetingNumParticipants { get; set; }

        public string[] Participant_Name { get; set; }

        public int[] ParticipantID { get; set; }

        public int TaskID { get; set; }
        public string Task { get; set; }
        public string RespPerson { get; set; }

        public string ResponsiblePerson
        {
            get
            {
                int res = Convert.ToInt16(RespPerson);
                return (db.Employee_Master.Where(x => x.EmployeeICode == res)).Select(x => x.EmployeeDisplayName).FirstOrDefault();
            }
        }
        public int empId { get; set; }

        //public string RespPerson
        //{
        //    get
        //    {
        //        return (RespPerson != null ? db.Employee_Master.Where(x => x.EmployeeICode == empId).Select(x => x.EmployeeDisplayName) : null).ToString();

        //    }
        //}


        public string Comments { get; set; }

        public string CompletionDate { get; set; }

        // public List<SelectListItem> Priority { get; set; }
        public string Priority { get; set; }

        public string Prio
        {
            get
            {
                return (Priority != null ? Priority : null).ToString();
            }
        }
        //public string Status { get; set; }
        public int Status { get; set; }

        public string selectedCategory { get; set; }
        public string ChairPerson { get; set; }
        public List<comment> Comment { get; set; }
        public IEnumerable<SelectListItemHelper> GetPriorityList { get; set; }
        public List<DDLViewModel> priorityTypes { get; set; }

        public string PriorityTypesSelected { get; set; }
        //
        public string ReviewDate { get; set; }

        //


        public static IEnumerable<SelectListItem> GetPriority()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "High", Value = "High"},
                new SelectListItem{Text = "Medium", Value = "Medium"},
                new SelectListItem{Text = "Low", Value = "Low"},
            };
            return items;
        }

    }

}


