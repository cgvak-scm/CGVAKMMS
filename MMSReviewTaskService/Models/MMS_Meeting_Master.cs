//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMSService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MMS_Meeting_Master
    {
        public MMS_Meeting_Master()
        {
            this.MMS_Meeting_Details = new HashSet<MMS_Meeting_Details>();
        }
    
        public int Meeting_Id { get; set; }
        public string Meeting_Objective { get; set; }
        public System.DateTime Meeting_Date { get; set; }
        public string Meeting_Time { get; set; }
        public string Meeting_Venue { get; set; }
        public string Meeting_Type { get; set; }
        public string Meeting_Duration { get; set; }
        public string Meeting_Department { get; set; }
        public Nullable<int> Meeting_Chairperson { get; set; }
        public string Minutes_Of_Meeting { get; set; }
        public int Meeting_No_Of_Participants { get; set; }
        public Nullable<bool> istemplate { get; set; }
        public Nullable<int> TemplateID { get; set; }
    
        public virtual ICollection<MMS_Meeting_Details> MMS_Meeting_Details { get; set; }
    }
}