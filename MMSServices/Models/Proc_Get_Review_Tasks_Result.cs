//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMSServices.Models
{
    using System;
    
    public partial class Proc_Get_Review_Tasks_Result
    {
        public int Task_Id { get; set; }
        public Nullable<int> Meeting_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
        public string Chairperson { get; set; }
        public string Participant { get; set; }
        public string Task { get; set; }
        public string Category { get; set; }
        public string Completion_Date { get; set; }
        public string Priority { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public string Comments { get; set; }
        public string CommenterName { get; set; }
        public int Review_Status { get; set; }
        public Nullable<System.DateTime> Review_Date { get; set; }
        public System.DateTime Assigned_Date { get; set; }
        public Nullable<int> Overdue_days { get; set; }
        public Nullable<int> Comments_count { get; set; }
    }
}