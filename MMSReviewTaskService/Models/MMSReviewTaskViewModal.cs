using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMSService.Models;

namespace MMSService.Models
{    
    public class ReviewTaskViewModal
    {

        public int? Task_Id { get; set; }

        public int? Meeting_Id { get; set; }

        public int? Employee_Id { get; set; }

        public int? Chairperson { get; set; }

        public int? Participant { get; set; }

        public string Task { get; set; }

        public string Category { get; set; }

        public string Completion_Date { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public int? Review_Status { get; set; }

        public DateTime? Review_Date { get; set; }

        public DateTime? Assigned_Date { get; set; }

        public DateTime? Overdue_days { get; set; }


    }

    public class ReviewTaskParticipantViewModel
    {
        public int? employee_id { get; set; }
        public string name { get; set; }
        public int? Participant { get; set; }
    }

    public class ReviewTaskCategoryViewModel
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
    }
}