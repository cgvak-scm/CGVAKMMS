



CREATE PROCEDURE [dbo].[Sp_Update_EmployeeMaster]      

@EmployeeICode [int]
,@CompanyICode [int]
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

 UPDATE [dbo].[Employee_Master] set

     
 [dbo].[Employee_Master].[CompanyICode]=@CompanyICode
,[dbo].[Employee_Master].[DepartmentICode]=@DepartmentICode
,[dbo].[Employee_Master].[DesignationICode]=@DesignationICode
,[dbo].[Employee_Master].[EmployeeNumber]=@EmployeeNumber
,[dbo].[Employee_Master].[EmployeeFirstName]=@EmployeeFirstName
,[dbo].[Employee_Master].[EmployeeLastName]=@EmployeeLastName
,[dbo].[Employee_Master].[EmployeeDisplayName]=@EmployeeDisplayName
,[dbo].[Employee_Master].[EmployeeCurrentAddress1]=@EmployeeCurrentAddress1
,[dbo].[Employee_Master].[EmployeeCurrentAddress2]=@EmployeeCurrentAddress2
,[dbo].[Employee_Master].[EmployeeCurrentCity]=@EmployeeCurrentCity
,[dbo].[Employee_Master].[EmployeeCurrentState]=@EmployeeCurrentState
,[dbo].[Employee_Master].[EmployeeCurrentCountry]=@EmployeeCurrentCountry
,[dbo].[Employee_Master].[EmployeeCurrentPinCode]=@EmployeeCurrentPinCode
,[dbo].[Employee_Master].[EmployeeCurrentPhoneNo]=@EmployeeCurrentPhoneNo
,[dbo].[Employee_Master].[IsSameAsCurrentAddress]=@IsSameAsCurrentAddress
,[dbo].[Employee_Master].[EmployeePermanentAddress1]=@EmployeePermanentAddress1
,[dbo].[Employee_Master].[EmployeePermanentAddress2]=@EmployeePermanentAddress2
,[dbo].[Employee_Master].[EmployeePermanentCity]=@EmployeePermanentCity
,[dbo].[Employee_Master].[EmployeePermanentState]=@EmployeePermanentState
,[dbo].[Employee_Master].[EmployeePermanentCountry]=@EmployeePermanentCountry
,[dbo].[Employee_Master].[EmployeePermanentPinCode]=@EmployeePermanentPinCode
,[dbo].[Employee_Master].[EmployeePermanentPhoneNo]=@EmployeePermanentPhoneNo
,[dbo].[Employee_Master].[EmployeeMobileNo]=@EmployeeMobileNo
,[dbo].[Employee_Master].[EmployeeFatherName]=@EmployeeFatherName
,[dbo].[Employee_Master].[EmployeeDateofBirth]=@EmployeeDateofBirth
,[dbo].[Employee_Master].[EmployeePersonalEmailId]=@EmployeePersonalEmailId
,[dbo].[Employee_Master].[EmployeeDateOfJoining]=@EmployeeDateOfJoining
,[dbo].[Employee_Master].[EmployeeCorporateEmailId]=@EmployeeCorporateEmailId
,[dbo].[Employee_Master].[EmployeeIM]=@EmployeeIM
,[dbo].[Employee_Master].[LoginUserName]=@LoginUserName
,[dbo].[Employee_Master].[LoginPassword]=@LoginPassword
,[dbo].[Employee_Master].[EmployeeBloodGroupId]=@EmployeeBloodGroupId
,[dbo].[Employee_Master].[EmployeePANNo]=@EmployeePANNo
,[dbo].[Employee_Master].[EmployeePFNo]=@EmployeePFNo
,[dbo].[Employee_Master].[EmployeeESINo]=@EmployeeESINo
,[dbo].[Employee_Master].[EmployeeBankName]=@EmployeeBankName
,[dbo].[Employee_Master].[EmployeeBankAccountNo]=@EmployeeBankAccountNo
,[dbo].[Employee_Master].[EmployeeMaritalStatus]=@EmployeeMaritalStatus
,[dbo].[Employee_Master].[EmployeePrimarySkill]=@EmployeePrimarySkill
,[dbo].[Employee_Master].[EmployeePreviousEmployer]=@EmployeePreviousEmployer
,[dbo].[Employee_Master].[EmployeePreviousExperienceYears]=@EmployeePreviousExperienceYears
,[dbo].[Employee_Master].[EmployeePreviousExperienceMonths]=@EmployeePreviousExperienceMonths
,[dbo].[Employee_Master].[EmployeeReference1Remarks]=@EmployeeReference1Remarks
,[dbo].[Employee_Master].[EmployeeReference2Remarks]=@EmployeeReference2Remarks
,[dbo].[Employee_Master].[EmployeeBioData]=@EmployeeBioData
,[dbo].[Employee_Master].[DocumentExtension]=@DocumentExtension
,[dbo].[Employee_Master].[EmployeePhoto]=@EmployeePhoto
,[dbo].[Employee_Master].[PhotoExtension]=@PhotoExtension
,[dbo].[Employee_Master].[EmployeeDateofLeaving]=@EmployeeDateofLeaving
,[dbo].[Employee_Master].[EmployeeReasonforLeaving]=@EmployeeReasonforLeaving
,[dbo].[Employee_Master].[IsActive]=@IsActive
,[dbo].[Employee_Master].[CreatedBy]=@CreatedBy
,[dbo].[Employee_Master].[CreatedDate]=@CreatedDate
,[dbo].[Employee_Master].[ModifiedBy]=@ModifiedBy
,[dbo].[Employee_Master].[ModifiedDate]=@ModifiedDate
,[dbo].[Employee_Master].[SwipeICode]=@SwipeICode
,[dbo].[Employee_Master].[Location]=@Location
,[dbo].[Employee_Master].[Group_id]=@Groupid
,[dbo].[Employee_Master].[shift_id]=@shiftid
,[dbo].[Employee_Master].[original_name]=@originalname
,[dbo].[Employee_Master].[pseudo]=@pseudo
,[dbo].[Employee_Master].[current_technology]=@currenttechnology
,[dbo].[Employee_Master].[picture]=@picture
,[dbo].[Employee_Master].[primary_skill_stream_id]=@primaryskillstreamid
,[dbo].[Employee_Master].[emp_photo]=@empphoto
,[dbo].[Employee_Master].[night_shift]=@nightshift
 
 where EmployeeICode=@EmployeeICode

 -- Insert into MMS_Employee_Master based on Employee_Master

 DECLARE @EmployeeMasterId int;
 DECLARE @Department varchar(100);

 SET @EmployeeMasterId = SCOPE_IDENTITY();
 
IF @DepartmentICode='1'
BEGIN
SET @Department ='Management' 
END
IF @DepartmentICode='2'
BEGIN
SET @Department ='Human Resource' 
END
IF @DepartmentICode='3'
BEGIN
SET @Department ='Accounts and Administration' 
END
IF @DepartmentICode='4'
BEGIN
SET @Department ='Marketing' 
END
IF @DepartmentICode='5'
BEGIN
SET @Department ='Software Developer' 
END
IF @DepartmentICode='7'
BEGIN
SET @Department ='System Administration' 
END
IF @DepartmentICode='10'
BEGIN
SET @Department ='Technical' 
END
IF @DepartmentICode='11'
BEGIN
SET @Department ='Accounts' 
END
IF @DepartmentICode='12'
BEGIN
SET @Department ='Administration' 
END
IF @DepartmentICode='13'
BEGIN
SET @Department ='IT Recruitment' 
END
IF @DepartmentICode='14'
BEGIN
SET @Department ='Processing' 
END

 UPDATE [dbo].[MMS_Employee_Master] set

    [dbo].[MMS_Employee_Master].[Employee_FirstName] = @EmployeeFirstName
    ,[dbo].[MMS_Employee_Master].[Employee_LastName] = @EmployeeLastName
    ,[dbo].[MMS_Employee_Master].[Employee_DateofBirth] = @EmployeeDateofBirth
    ,[dbo].[MMS_Employee_Master].[Employee_Address] = @EmployeeCurrentAddress1
    ,[dbo].[MMS_Employee_Master].[Employee_Department] = @Department
    ,[dbo].[MMS_Employee_Master].[Employee_Qualification] = @EmployeePrimarySkill
    ,[dbo].[MMS_Employee_Master].[Employee_DateofJoin] = @EmployeeDateOfJoining
    ,[dbo].[MMS_Employee_Master].[Employee_ContactNo] = @EmployeeMobileNo
    ,[dbo].[MMS_Employee_Master].[Employee_Status] = @IsActive
    ,[dbo].[MMS_Employee_Master].[Employee_ModifyDate] = @ModifiedDate
    ,[dbo].[MMS_Employee_Master].[Employee_EmailAddress] = @EmployeeCorporateEmailId
    
	where [dbo].[MMS_Employee_Master].[profile_employee_id] = @EmployeeICode
--END

End




