//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMS.Models
{
    using System;
    
    public partial class sp_select_Meeting_Name_Result
    {
        public Nullable<int> meeting_id { get; set; }
        public Nullable<int> employee_id { get; set; }
        public Nullable<int> Task_Id { get; set; }
        public string Task { get; set; }
        public string Participant { get; set; }
        public Nullable<System.DateTime> meeting_date { get; set; }
        public Nullable<System.DateTime> completion_date { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public Nullable<int> Overdue_Days { get; set; }
    }
}