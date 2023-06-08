using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;


namespace MMS.Models
{
    public class EmpPersonalViewDetails
    {
        public int? EmployeeICode { get; set; }
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
        public bool IsSameAsCurrentAddress { get; set; } = false;
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
        public bool EmployeeMaritalStatus { get; set; } = false;
        public string Location { get; set; }

    }

    public class EmpOthersViewDetails
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
        public string LoginConfirmPassword { get; set; }
        public string EmployeePANNo { get; set; }
        public string EmployeePFNo { get; set; }
        public string EmployeeESINo { get; set; }
        public string EmployeeBankName { get; set; }
        public string EmployeeBankAccountNo { get; set; }
        public string EmployeePrimarySkill { get; set; }
        public string current_technology { get; set; }
        public string EmployeePreviousEmployer { get; set; }
        public int? EmployeePreviousExperienceYears { get; set; }
        public int? EmployeePreviousExperienceMonths { get; set; }
        public string EmployeeReference1Remarks { get; set; }
        public string EmployeeReference2Remarks { get; set; }

        public int? SwipeICode { get; set; }
        public int? GroupId { get; set; }
        public int? ShiftId { get; set; }

        public string[] files { get; set; }
        //public string DocumentExtension { get; set; }
        public string[] EmployeePhoto { get; set; }
        //public string PhotoExtension { get; set; }
        public string EmployeeDateofLeaving { get; set; }
        public string EmployeeReasonforLeaving { get; set; }

        public bool IsActive { get; set; } = false;
        public bool pseudo { get; set; } = false;
        public byte[] picture { get; set; }
        public bool nightshift { get; set; } = false;

    }
    
    public class EmployeeSummary
    {
        public int? EmployeeICode { get; set; }
        public string LoginUserName { get; set; }
        public string Location { get; set; }
        public string EmployeeCorporateEmailId { get; set; }
        public DateTime EmployeeDateOfJoining { get; set; }
        public string EmployeePrimarySkill { get; set; }
        public int? EmployeePreviousExperienceYears { get; set; }
        public int? EmployeePreviousExperienceMonths { get; set; }
        public string EmployeeNumber { get; set; }
        public int? SwipeICode { get; set; }

    }

    public class getdata
    {
        public string EmpDisplayName { get; set; }

        public string EmpMobileNo { get; set; }
    }

    public class CreateEmployeeViewModel
    {
        private static MMSCGVAKDBEntities db;

        //newly created FOr edit EmployeeView--1
        public static dynamic EditIndexDetails(int? EmployeeICode)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                dynamic
 res = (from employeeMaster in db.Employee_Master
        where employeeMaster.EmployeeICode == EmployeeICode
        select new
        {
            employeeFirstName = employeeMaster.EmployeeFirstName,
            employeeLastName = employeeMaster.EmployeeLastName,
            employeeDisplayName = employeeMaster.EmployeeDisplayName,
            employeeFatherName = employeeMaster.EmployeeFatherName,
            isSameAsCurrentAddress = (employeeMaster.IsSameAsCurrentAddress),
            employeeCurrentAddress1 = employeeMaster.EmployeeCurrentAddress1,
            employeeCurrentAddress2 = employeeMaster.EmployeeCurrentAddress2,
            employeeCurrentCity = employeeMaster.EmployeeCurrentCity,
            employeeCurrentState = employeeMaster.EmployeeCurrentState,
            employeeCurrentCountry = employeeMaster.EmployeeCurrentCountry,
            employeeCurrentPinCode = employeeMaster.EmployeeCurrentPinCode,
            employeeCurrentPhoneNo = employeeMaster.EmployeeCurrentPhoneNo,
            employeeMobileNo = employeeMaster.EmployeeMobileNo,
            employeeDateofBirth = employeeMaster.EmployeeDateofBirth,
            employeePersonalEmailId = employeeMaster.EmployeePersonalEmailId,
            location = employeeMaster.Location,
            employeeBloodGroupId = employeeMaster.EmployeeBloodGroupId,
            employeeMaritalStatus = employeeMaster.EmployeeMaritalStatus

        }).ToList();
                return res;
            }
        }

        public static dynamic EditEmpOtherDetails(int? EmployeeICode)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                dynamic res = (from employeeMaster in db.Employee_Master
                               where employeeMaster.EmployeeICode == EmployeeICode
                               select new
                               {
                                   CompanyICode = employeeMaster.CompanyICode,
                                   DepartmentICode = employeeMaster.DepartmentICode,
                                   DesignationICode = employeeMaster.DesignationICode,
                                   EmployeeNumber = employeeMaster.EmployeeNumber,
                                   EmployeeDateOfJoining = employeeMaster.EmployeeDateOfJoining,
                                   EmployeeCorporateEmailId = employeeMaster.EmployeeCorporateEmailId,
                                   EmployeePrimarySkill = employeeMaster.EmployeePrimarySkill,
                                   EmployeePreviousExperienceYears = employeeMaster.EmployeePreviousExperienceYears,
                                   EmployeePreviousExperienceMonths = employeeMaster.EmployeePreviousExperienceMonths,
                                   SwipeICode = employeeMaster.SwipeICode,
                                   GroupId = employeeMaster.Group_id,
                                   ShiftId = employeeMaster.shift_id,
                                   EmployeeDateofLeaving = employeeMaster.EmployeeDateofLeaving,
                                   EmployeeReasonforLeaving = employeeMaster.EmployeeReasonforLeaving,
                                   EmployeeReference1Remarks = employeeMaster.EmployeeReference1Remarks,
                                   EmployeeReference2Remarks = employeeMaster.EmployeeReference2Remarks,
                                   IsActive = employeeMaster.IsActive,
                                   EmployeeBankName = employeeMaster.EmployeeBankName,
                                   EmployeeBankAccountNo = employeeMaster.EmployeeBankAccountNo,
                                   EmployeeIM = employeeMaster.EmployeeIM,
                                   EmployeePANNo = employeeMaster.EmployeePANNo,
                                   EmployeePFNo = employeeMaster.EmployeePFNo,
                                   EmployeeESINo = employeeMaster.EmployeeESINo,
                                   LoginUserName = employeeMaster.LoginUserName,
                                   LoginPassword = employeeMaster.LoginPassword

                               }).ToList();
                return res;
            }
        }

    }

    public class UpdateEmpOtherDetails
    {
        public string files { get; set; }

    }

    public class EmpBloodViewDetails
    {
        public int? Values { get; set; }
        public string BloodName { get; set; }
    }
}

