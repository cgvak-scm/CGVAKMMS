


CREATE PROCEDURE [dbo].[sp_insert_EmployeeMaster]      

@CompanyICode [int]
,@DepartmentICode [int]
,@DesignationICode [int]
,@EmployeeNumber [varchar](10)
,@EmployeeFirstName [varchar](50)
,@EmployeeLastName [varchar](50)
,@EmployeeDisplayName [varchar](20)
,@EmployeeCurrentAddress1 [varchar](100)
,@EmployeeCurrentAddress2 [varchar](100)
,@EmployeeCurrentCity [varchar](50)
,@EmployeeCurrentState [varchar](25)
,@EmployeeCurrentCountry [varchar](25)
,@EmployeeCurrentPinCode [varchar](20)
,@EmployeeCurrentPhoneNo [varchar](20)
,@IsSameAsCurrentAddress bit
,@EmployeePermanentAddress1 [varchar](100)
,@EmployeePermanentAddress2 [varchar](100)
,@EmployeePermanentCity [varchar](50)
,@EmployeePermanentState [varchar](25)
,@EmployeePermanentCountry [varchar](25)
,@EmployeePermanentPinCode [varchar](20)
,@EmployeePermanentPhoneNo [varchar](20)
,@EmployeeMobileNo [varchar](20)
,@EmployeeFatherName [varchar](50)
,@EmployeeDateofBirth [datetime]
,@EmployeePersonalEmailId [varchar](100)
,@EmployeeDateOfJoining [datetime]
,@EmployeeCorporateEmailId [varchar](100)
,@EmployeeIM [varchar](50)
,@LoginUserName [varchar](25)
,@LoginPassword [varchar](25)
,@EmployeeBloodGroupId [int]
,@EmployeePANNo [varchar](25)
,@EmployeePFNo [varchar](25)
,@EmployeeESINo [varchar](25)
,@EmployeeBankName [varchar](50)
,@EmployeeBankAccountNo [varchar](25)
,@EmployeeMaritalStatus bit
,@EmployeePrimarySkill [varchar](50)
,@EmployeePreviousEmployer [varchar](100)
,@EmployeePreviousExperienceYears [int]
,@EmployeePreviousExperienceMonths [int]
,@EmployeeReference1Remarks [varchar](200)
,@EmployeeReference2Remarks [varchar](200)
,@EmployeeBioData [image]
,@DocumentExtension [char](10)
,@EmployeePhoto [image]
,@PhotoExtension [char](10)
,@EmployeeDateofLeaving [datetime]
,@EmployeeReasonforLeaving [varchar](200)
,@IsActive bit
,@CreatedBy int
,@CreatedDate [datetime]
,@ModifiedBy [int]
,@ModifiedDate [datetime]
,@SwipeICode [int]
,@Location [char](2)
,@Groupid [int]
,@shiftid [int]
,@originalname [varchar](100)
,@pseudo bit
,@currenttechnology [int]
,@picture [image]
,@primaryskillstreamid [int]
,@empphoto [varchar](2000)
,@nightshift bit

AS      
     
--DECLARE @EmpNoCount INT
--DECLARE @DisNameCount varchar(50)

--SET @EmpNoCount =( SELECT count(*) FROM [dbo].[Employee_Master] Where [dbo].[Employee_Master].[EmployeeNumber] = @EmployeeNumber)
--SET @EmpNoCount =( SELECT count(*) FROM [dbo].[Employee_Master] Where [dbo].[Employee_Master].[EmployeeDisplayName] = @EmployeeDisplayName)


--IF @EmpNoCount = 1 and @DisNameCount = 1
--BEGIN
 BEGIN  
 INSERT INTO [dbo].[Employee_Master]    
 (      
  [dbo].[Employee_Master].[CompanyICode]
,[dbo].[Employee_Master].[DepartmentICode]
,[dbo].[Employee_Master].[DesignationICode]
,[dbo].[Employee_Master].[EmployeeNumber]
,[dbo].[Employee_Master].[EmployeeFirstName]
,[dbo].[Employee_Master].[EmployeeLastName]
,[dbo].[Employee_Master].[EmployeeDisplayName]
,[dbo].[Employee_Master].[EmployeeCurrentAddress1]
,[dbo].[Employee_Master].[EmployeeCurrentAddress2]
,[dbo].[Employee_Master].[EmployeeCurrentCity]
,[dbo].[Employee_Master].[EmployeeCurrentState]
,[dbo].[Employee_Master].[EmployeeCurrentCountry]
,[dbo].[Employee_Master].[EmployeeCurrentPinCode]
,[dbo].[Employee_Master].[EmployeeCurrentPhoneNo]
,[dbo].[Employee_Master].[IsSameAsCurrentAddress]
,[dbo].[Employee_Master].[EmployeePermanentAddress1]
,[dbo].[Employee_Master].[EmployeePermanentAddress2]
,[dbo].[Employee_Master].[EmployeePermanentCity]
,[dbo].[Employee_Master].[EmployeePermanentState]
,[dbo].[Employee_Master].[EmployeePermanentCountry]
,[dbo].[Employee_Master].[EmployeePermanentPinCode]
,[dbo].[Employee_Master].[EmployeePermanentPhoneNo]
,[dbo].[Employee_Master].[EmployeeMobileNo]
,[dbo].[Employee_Master].[EmployeeFatherName]
,[dbo].[Employee_Master].[EmployeeDateofBirth]
,[dbo].[Employee_Master].[EmployeePersonalEmailId]
,[dbo].[Employee_Master].[EmployeeDateOfJoining]
,[dbo].[Employee_Master].[EmployeeCorporateEmailId]
,[dbo].[Employee_Master].[EmployeeIM]
,[dbo].[Employee_Master].[LoginUserName]
,[dbo].[Employee_Master].[LoginPassword]
,[dbo].[Employee_Master].[EmployeeBloodGroupId]
,[dbo].[Employee_Master].[EmployeePANNo]
,[dbo].[Employee_Master].[EmployeePFNo]
,[dbo].[Employee_Master].[EmployeeESINo]
,[dbo].[Employee_Master].[EmployeeBankName]
,[dbo].[Employee_Master].[EmployeeBankAccountNo]
,[dbo].[Employee_Master].[EmployeeMaritalStatus]
,[dbo].[Employee_Master].[EmployeePrimarySkill]
,[dbo].[Employee_Master].[EmployeePreviousEmployer]
,[dbo].[Employee_Master].[EmployeePreviousExperienceYears]
,[dbo].[Employee_Master].[EmployeePreviousExperienceMonths]
,[dbo].[Employee_Master].[EmployeeReference1Remarks]
,[dbo].[Employee_Master].[EmployeeReference2Remarks]
,[dbo].[Employee_Master].[EmployeeBioData]
,[dbo].[Employee_Master].[DocumentExtension]
,[dbo].[Employee_Master].[EmployeePhoto]
,[dbo].[Employee_Master].[PhotoExtension]
,[dbo].[Employee_Master].[EmployeeDateofLeaving]
,[dbo].[Employee_Master].[EmployeeReasonforLeaving]
,[dbo].[Employee_Master].[IsActive]
,[dbo].[Employee_Master].[CreatedBy]
,[dbo].[Employee_Master].[CreatedDate]
,[dbo].[Employee_Master].[ModifiedBy]
,[dbo].[Employee_Master].[ModifiedDate]
,[dbo].[Employee_Master].[SwipeICode]
,[dbo].[Employee_Master].[Location]
,[dbo].[Employee_Master].[Group_id]
,[dbo].[Employee_Master].[shift_id]
,[dbo].[Employee_Master].[original_name]
,[dbo].[Employee_Master].[pseudo]
,[dbo].[Employee_Master].[current_technology]
,[dbo].[Employee_Master].[picture]
,[dbo].[Employee_Master].[primary_skill_stream_id]
,[dbo].[Employee_Master].[emp_photo]
,[dbo].[Employee_Master].[night_shift]
  )      
 VALUES      
 (      
 @CompanyICode   
,@DepartmentICode
,@DesignationICode
,@EmployeeNumber
,@EmployeeFirstName
,@EmployeeLastName
,@EmployeeDisplayName
,@EmployeeCurrentAddress1
,@EmployeeCurrentAddress2
,@EmployeeCurrentCity
,@EmployeeCurrentState
,@EmployeeCurrentCountry
,@EmployeeCurrentPinCode
,@EmployeeCurrentPhoneNo
,@IsSameAsCurrentAddress
,@EmployeePermanentAddress1
,@EmployeePermanentAddress2
,@EmployeePermanentCity
,@EmployeePermanentState
,@EmployeePermanentCountry
,@EmployeePermanentPinCode
,@EmployeePermanentPhoneNo
,@EmployeeMobileNo
,@EmployeeFatherName
,@EmployeeDateofBirth
,@EmployeePersonalEmailId
,@EmployeeDateOfJoining
,@EmployeeCorporateEmailId
,@EmployeeIM
,@LoginUserName
,@LoginPassword
,@EmployeeBloodGroupId
,@EmployeePANNo
,@EmployeePFNo
,@EmployeeESINo
,@EmployeeBankName
,@EmployeeBankAccountNo
,@EmployeeMaritalStatus
,@EmployeePrimarySkill
,@EmployeePreviousEmployer
,@EmployeePreviousExperienceYears
,@EmployeePreviousExperienceMonths
,@EmployeeReference1Remarks
,@EmployeeReference2Remarks
,@EmployeeBioData
,@DocumentExtension
,@EmployeePhoto
,@PhotoExtension
,@EmployeeDateofLeaving
,@EmployeeReasonforLeaving
,@IsActive
,@CreatedBy
,@CreatedDate
,@ModifiedBy
,@ModifiedDate
,@SwipeICode
,@Location
,@Groupid
,@shiftid
,@originalname
,@pseudo
,@currenttechnology
,@picture
,@primaryskillstreamid
,@empphoto
,@nightshift

 )


 -- Insert into MMS_Employee_Master based on Employee_Master

 DECLARE @EmployeeMasterId int;
 DECLARE @Department varchar(100);

 SET @EmployeeMasterId = SCOPE_IDENTITY();
 
 IF @DepartmentICode='5'

 BEGIN
 SET @Department ='Developer' 
 END

 INSERT INTO [dbo].[MMS_Employee_Master]    
 (

 [dbo].[MMS_Employee_Master].[Employee_FirstName]
      ,[dbo].[MMS_Employee_Master].[Employee_LastName]
      ,[dbo].[MMS_Employee_Master].[Employee_DateofBirth]
      ,[dbo].[MMS_Employee_Master].[Employee_Address]
      ,[dbo].[MMS_Employee_Master].[Employee_Department]
      ,[dbo].[MMS_Employee_Master].[Employee_Qualification]
      ,[dbo].[MMS_Employee_Master].[Employee_DateofJoin]
      ,[dbo].[MMS_Employee_Master].[Employee_ContactNo]
      ,[dbo].[MMS_Employee_Master].[Employee_Status]
      ,[dbo].[MMS_Employee_Master].[Employee_ModifyDate]
      ,[dbo].[MMS_Employee_Master].[Employee_EmailAddress]
      ,[dbo].[MMS_Employee_Master].[profile_employee_id]
 )
 values
 (

 @EmployeeFirstName
,@EmployeeLastName
,@EmployeeDateofBirth
,@EmployeeCurrentAddress1
,@DepartmentICode
,@EmployeePrimarySkill
,@EmployeeDateOfJoining
,@EmployeeMobileNo
,@IsActive
,@ModifiedDate
,@EmployeeCorporateEmailId
,@EmployeeMasterId

 )

END

--END



