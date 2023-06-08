using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class EmpPersonalDetails
    {
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
        public bool IsSameAsCurrentAddress { get; set; }
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
        public string EmployeeDateofBirth { get; set; }
        public string EmployeePersonalEmailId { get; set; }
        public string EmployeeBloodGroupId { get; set; }
        public bool EmployeeMaritalStatus { get; set; }
        public string Location { get; set; }

    }

    public class EmpCompanyDetails
    {
        public int? CompanyICode { get; set; }
        public int? DepartmentICode { get; set; }
        public int? DesignationICode { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeDateOfJoining { get; set; }
        public string EmployeeCorporateEmailId { get; set; }
        public string EmployeeIM { get; set; }
        public string LoginUserName { get; set; }
        public string LoginPassword { get; set; }
        public string EmployeePANNo { get; set; }
        public string EmployeePFNo { get; set; }
        public string EmployeeESINo { get; set; }
        public string EmployeeBankName { get; set; }
        public string EmployeeBankAccountNo { get; set; }
        public string EmployeePrimarySkill { get; set; }
        public string EmployeePreviousEmployer { get; set; }
        public int? EmployeePreviousExperienceYears { get; set; }
        public int? EmployeePreviousExperienceMonths { get; set; }
        public string EmployeeReference1Remarks { get; set; }
        public string EmployeeReference2Remarks { get; set; }

        public int? SwipeICode { get; set; }
        public int? Group_id { get; set; }
        public int? shift_id { get; set; }

        public byte[] files { get; set; }
        //public string DocumentExtension { get; set; }
        public byte[] EmployeePhoto { get; set; }
        //public string PhotoExtension { get; set; }
        public string EmployeeDateofLeaving { get; set; }
        public string EmployeeReasonforLeaving { get; set; }

        public bool IsActive { get; set; }
        public bool pseudo { get; set; }
        public byte[] picture { get; set; }
        public bool nightshift { get; set; }

    }

    public class EmpOtherDetails
    {
        //public string Location { get; set; }
        //public string original_name { get; set; }
        //public string pseudo { get; set; }
        //public string current_technology { get; set; }
        //public string picture { get; set; }
        //public string primary_skill_stream_id { get; set; }
        //public string emp_photo { get; set; }
        //public string night_shift { get; set; }
    }

}

