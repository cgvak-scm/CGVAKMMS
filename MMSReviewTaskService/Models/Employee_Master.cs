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
    
    public partial class Employee_Master
    {
        public int EmployeeICode { get; set; }
        public int CompanyICode { get; set; }
        public int DepartmentICode { get; set; }
        public int DesignationICode { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeDisplayName { get; set; }
        public string EmployeeCurrentAddress1 { get; set; }
        public string EmployeeCurrentAddress2 { get; set; }
        public string EmployeeCurrentAddress3 { get; set; }
        public string EmployeeCurrentCity { get; set; }
        public string EmployeeCurrentState { get; set; }
        public string EmployeeCurrentCountry { get; set; }
        public string EmployeeCurrentPinCode { get; set; }
        public string EmployeeCurrentPhoneNo { get; set; }
        public Nullable<bool> IsSameAsCurrentAddress { get; set; }
        public string EmployeePermanentAddress1 { get; set; }
        public string EmployeePermanentAddress2 { get; set; }
        public string EmployeePermanentAddress3 { get; set; }
        public string EmployeePermanentCity { get; set; }
        public string EmployeePermanentState { get; set; }
        public string EmployeePermanentCountry { get; set; }
        public string EmployeePermanentPinCode { get; set; }
        public string EmployeePermanentPhoneNo { get; set; }
        public string EmployeeMobileNo { get; set; }
        public string EmployeeFatherName { get; set; }
        public Nullable<System.DateTime> EmployeeDateofBirth { get; set; }
        public string EmployeePersonalEmailId { get; set; }
        public System.DateTime EmployeeDateOfJoining { get; set; }
        public string EmployeeCorporateEmailId { get; set; }
        public string EmployeeIM { get; set; }
        public string LoginUserName { get; set; }
        public string LoginPassword { get; set; }
        public Nullable<int> EmployeeBloodGroupId { get; set; }
        public string EmployeePANNo { get; set; }
        public string EmployeePFNo { get; set; }
        public string EmployeeESINo { get; set; }
        public string EmployeeBankName { get; set; }
        public string EmployeeBankAccountNo { get; set; }
        public string EmployeeMaritalStatus { get; set; }
        public string EmployeePrimarySkill { get; set; }
        public string EmployeePreviousEmployer { get; set; }
        public Nullable<int> EmployeePreviousExperienceYears { get; set; }
        public Nullable<int> EmployeePreviousExperienceMonths { get; set; }
        public string EmployeeReference1Remarks { get; set; }
        public string EmployeeReference2Remarks { get; set; }
        public byte[] EmployeeBioData { get; set; }
        public string DocumentExtension { get; set; }
        public byte[] EmployeePhoto { get; set; }
        public string PhotoExtension { get; set; }
        public Nullable<System.DateTime> EmployeeDateofLeaving { get; set; }
        public string EmployeeReasonforLeaving { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> SwipeICode { get; set; }
        public string Location { get; set; }
        public Nullable<byte> Group_id { get; set; }
        public Nullable<int> shift_id { get; set; }
        public string original_name { get; set; }
        public Nullable<bool> pseudo { get; set; }
        public Nullable<int> current_technology { get; set; }
        public byte[] picture { get; set; }
        public Nullable<int> primary_skill_stream_id { get; set; }
        public string emp_photo { get; set; }
        public Nullable<bool> night_shift { get; set; }
    }
}
