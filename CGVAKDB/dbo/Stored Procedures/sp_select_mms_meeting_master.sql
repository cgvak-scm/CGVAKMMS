

CREATE PROCEDURE [dbo].[sp_select_mms_meeting_master]  
@Meeting_Id [Int]
AS   
	BEGIN  --Convert(varchar(25),[dbo].MMS_Meeting_Details.Chairperson) as Chairperson,
		SELECT 
			[dbo].mms_meeting_master.Meeting_Id,
			[dbo].mms_meeting_master.Meeting_Objective,
			[dbo].mms_meeting_master.Meeting_Date,
			[dbo].mms_meeting_master.Meeting_Time,
			[dbo].mms_meeting_master.Meeting_Venue,
			[dbo].mms_meeting_master.Meeting_Type,
			[dbo].mms_meeting_master.Meeting_Duration,
			[dbo].mms_meeting_master.Meeting_Department,
			--Convert(varchar(25),[dbo].mms_meeting_master.Meeting_Chairperson) as Meeting_Chairperson,
			(Select EmployeeDisplayName from [dbo].Employee_Master where [dbo].mms_meeting_master.Meeting_Chairperson=Employee_Master.EmployeeICode) as Meeting_Chairperson,
			[dbo].mms_meeting_master.Minutes_Of_Meeting,
			[dbo].mms_meeting_master.Meeting_No_Of_Participants,
			[dbo].mms_meeting_master.istemplate 
		FROM 
			[dbo].mms_meeting_master  
  		WHERE 
			[dbo].mms_meeting_master.Meeting_Id=@Meeting_Id 
	END  
RETURN






