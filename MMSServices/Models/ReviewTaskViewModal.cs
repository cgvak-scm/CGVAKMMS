using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMSServices.Models
{

    public class ReviewTaskViewModel
    {
        public int? Task_Id { get; set; }

        public int? Meeting_Id { get; set; }

        public int? Employee_Id { get; set; }

        public string Chairperson { get; set; }

        public string Participant { get; set; }

        public string Task { get; set; }

        public string Category { get; set; }

        public string Completion_Date { get; set; }

        public string Priority { get; set; }

        public int? Status { get; set; }

        public string StatusName { get; set; }

        public string Comments { get; set; }

        public string CommenterName { get; set; }

        public int? Review_Status { get; set; }

        public DateTime? Review_Date { get; set; }

        public DateTime? Assigned_Date { get; set; }

        public int? Overdue_days { get; set; }

        public int? Comments_count { get; set; }
    }

    public class ReviewTaskParticipantViewModel
    {
        public int? employee_id { get; set; }
        public string name { get; set; }
        public int? Participant { get; set; }
    }

    public class CategoryViewModel
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
    }

    public class ReviewTaskFrequencyViewModel
    {
        public int ID { get; set; }
        public string FrequencyName { get; set; }
        public int? FrequencyInDay { get; set; }
    }

    public class ReviewTaskCommentViewModel
    {
        public string Comments { get; set; }
        public DateTime? Meeting_Status_Date { get; set; }
        public int? Commented_by { get; set; }
        public DateTime? StatusDate { get; set; }
    }

    public class ReviewTaskUpdateViewModel
    {
        public Nullable<int> MeetingId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string Task { get; set; }
        public Nullable<int> status { get; set; }
        public int MeetingStatusAutoId { get; set; }
        public Nullable<int> TaskId { get; set; }
    }

    public class TrackReviewTasksViewModel
    {
        public int ID { get; set; }
        public Nullable<int> TaskId { get; set; }
        public Nullable<System.DateTime> NextReviewDate { get; set; }
        public Nullable<System.DateTime> ActualReviewedDate { get; set; }
        public Nullable<bool> Review_Status { get; set; }
    }

    public class TasksReviewViewModel
    {
        public int ID { get; set; }
        public Nullable<int> TaskId { get; set; }
        public Nullable<System.DateTime> NextReviewDate { get; set; }
        public Nullable<System.DateTime> ActualReviewedDate { get; set; }
        public Nullable<bool> Review_Status { get; set; }
    }

    public class meetingstatusViewModel
    {
        public int Meeting_Status_Employee_Id { get; set; }
        public string Meeting_Status_Task { get; set; }
        public string Meeting_Status_ResponsiblePerson { get; set; }
        public string Meeting_Status_Date { get; set; }
        public string Meeting_Status_Comments { get; set; }
        public string Meeting_Status { get; set; }
        public Nullable<int> Meeting_Id { get; set; }
        public string Meeting_Status_Hours { get; set; }
        public string Commented_By { get; set; }
        public Nullable<int> Commented_By_ID { get; set; }
        public int MeetingStatusAutoId { get; set; }
        public Nullable<int> Task_Id { get; set; }
        public int Comments_Count { get; set; }
    }

    public class PostCommentReviewViewModel
    {
        public int meetingId { get; set; }

        public int status { get; set; }

        public string comment { get; set; }

        public int hours { get; set; }

        public int minutes { get; set; }

        public int taskId { get; set; }

        public string reviewsId { get; set; }

    }

    public class ReviewTaskStatusViewModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class LoginViewModel
    {
        public int EmployeeIcode { get; set; }
        public string EmployeeDisplayName { get; set; }

    }

    public class ReviewTaskPagingViewModel
    {
        public int totCount { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public List<ReviewTaskViewModel> data { get; set; }

    }

    public class AssignedTaskPagingViewModel
    {
        public int totCount { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public List<AssignedTaskViewModel> data { get; set; }

    }

    public class CommentsPagingViewModel
    {
        public int totCount { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public List<meetingstatusViewModel> data { get; set; }

    }

    public class ParticipantPagingViewModel
    {
        public int totCount { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public List<ReviewTaskParticipantViewModel> data { get; set; }

    }

    public class Requency
    {
        public List<SelectListItem> Requencys { get; set; }
        public int[] RequencyIds { get; set; }
    }

    public class TaskStatus
    {
        public string taskName { get; set; }

    }

    public class AssignedTaskViewModel
    {
        public int? MeetingId { get; set; }

        public int? EmployeeId { get; set; }

        public int? TaskId { get; set; }

        public string Task { get; set; }

        public int? CategoryId { get; set; }

        public string Comments { get; set; }

        public string Participant { get; set; }

        public Nullable<System.DateTime> meeting_date { get; set; }

        public Nullable<System.DateTime> completion_date { get; set; }

        public string priority { get; set; }

        public string status { get; set; }

        public int? Overdue_Days { get; set; }

    }


    public class PostAssignedTaskViewModel
    {
        public int? meetingId { get; set; }
        //public int? employeeId { get; set; }
        //public int? taskId { get; set; }
        public int task { get; set; }
        public string comments { get; set; }
        public int? category { get; set; }
        //public string Participant { get; set; }
        //public Nullable<System.DateTime> meetingDate { get; set; }
        public Nullable<System.DateTime> completionDate { get; set; }
        public string priority { get; set; }
        public int? status { get; set; }
        //public int? overdueDays { get; set; }
    }
    

    public class PriorityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Descriptions { get; set; }

        public bool? Active { get; set; }        
    }


    public class PseudoNames
    {
        public object Employee_Name { get; set; }
        public object Employee_Id { get; set; }
    }

    public class MeetingViewModel
    {
        public ReviewTaskDBEntities db = new ReviewTaskDBEntities();
        public MMS_Meeting_Master mmsMeetingMaster { get; set; }

        public string Meetmaster
        {
            get
            {
                string meet_Chairperson = string.Empty;
                return meet_Chairperson = db.Employee_Master.Where(x => x.EmployeeICode == mmsMeetingMaster.Meeting_Chairperson).Select(x => x.EmployeeDisplayName).ToString();
            }
        }

       // public Designation_Master Designationmaster { get; set; }

        public MMS_Meeting_Details mmsMeetingDetails { get; set; }

        public List<MMS_Meeting_Participant> Participant { get; set; }

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

    public class MeetingDetailsViewModel
    {
        public static List<string> TemplateDetails()
        {
            using (ReviewTaskDBEntities db = new ReviewTaskDBEntities())
            {
                var template_list = db.MMS_Meeting_Template.Select(x => x.objective).ToList();
                return template_list;
            }
        }
    }

    public class TaskDetailsForApp
    {
        public string Department { get; set; }
        public string[] Employees { get; set; }
        public List<Tasks> TaskLIst { get; set; }

    }

    public class Tasks
    {
        public int Task_Id { get; set; }
        public Nullable<int> Meeting_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
        public Nullable<int> Chairperson { get; set; }
        public Nullable<int> Participant { get; set; }
        public string Task { get; set; }
        public Nullable<System.DateTime> Completion_Date { get; set; }
        public string Priority { get; set; }
        public Nullable<int> Status { get; set; }
        public string Comments { get; set; }
        public Nullable<decimal> Percentage_Completed { get; set; }
        public Nullable<int> Category { get; set; }
        public string is_read_notification { get; set; }
        public Nullable<bool> seen { get; set; }
        public Nullable<bool> is_template { get; set; }
        public Nullable<System.DateTime> NextReviewDate { get; set; }
        public Nullable<int> ReviewFrequencyID { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> ChairPersonID { get; set; }
        public Nullable<int> ParticipantID { get; set; }
        public Nullable<int> TemplateID { get; set; }
        public bool baz { get; set; }

        //public virtual MMS_Meeting_Master MMS_Meeting_Master { get; set; }
    }

}