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
    using System.Collections.Generic;
    
    public partial class MMS_Employee_Master
    {
        public int Employee_Id { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public Nullable<System.DateTime> Employee_DateofBirth { get; set; }
        public string Employee_Address { get; set; }
        public string Employee_Department { get; set; }
        public string Employee_Qualification { get; set; }
        public Nullable<System.DateTime> Employee_DateofJoin { get; set; }
        public string Employee_ContactNo { get; set; }
        public bool Employee_Status { get; set; }
        public Nullable<System.DateTime> Employee_ModifyDate { get; set; }
        public string Employee_EmailAddress { get; set; }
        public Nullable<int> profile_employee_id { get; set; }
    }
}
