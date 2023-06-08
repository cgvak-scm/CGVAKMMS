

CREATE VIEW [dbo].[vw_get_Employees]
AS
SELECT     EmployeeICode, CompanyICode, DepartmentICode, DesignationICode, EmployeeNumber, EmployeeFirstName, EmployeeLastName, 
                      EmployeeFirstName + ' ' + EmployeeLastName AS EmployeeName, EmployeeDisplayName, EmployeeCurrentAddress1, EmployeeCurrentAddress2, 
                      EmployeeCurrentAddress3, EmployeeCurrentCity, EmployeeCurrentState, EmployeeCurrentCountry, EmployeeCurrentPinCode, 
                      EmployeeCurrentPhoneNo, IsSameAsCurrentAddress, EmployeePermanentAddress1, EmployeePermanentAddress2, EmployeePermanentAddress3, 
                      EmployeePermanentCity, EmployeePermanentState, EmployeePermanentCountry, EmployeePermanentPinCode, EmployeePermanentPhoneNo, 
                      EmployeeMobileNo, EmployeeFatherName, EmployeeDateofBirth, EmployeePersonalEmailId, EmployeeCorporateEmailId, LoginUserName, 
                      LoginPassword, EmployeePANNo, EmployeePFNo, EmployeeESINo, EmployeeBankName, EmployeeBankAccountNo, EmployeeMaritalStatus, 
                      EmployeePrimarySkill, EmployeePreviousEmployer, EmployeePreviousExperienceYears, EmployeePreviousExperienceMonths, 
                      EmployeeReference1Remarks, EmployeeReference2Remarks, EmployeeBioData, DocumentExtension, EmployeePhoto, PhotoExtension, 
                      EmployeeDateofLeaving, EmployeeReasonforLeaving, IsActive, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, EmployeeIM, 
                      EmployeeBloodGroupId, EmployeeDateOfJoining
FROM         dbo.Employee_Master



