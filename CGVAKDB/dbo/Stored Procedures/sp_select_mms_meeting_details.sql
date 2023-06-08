


-- exec [sp_select_mms_meeting_details] 97511

CREATE PROCEDURE [dbo].[sp_select_mms_meeting_details]  
@Meeting_Id [Int]
AS   
	BEGIN  
		SELECT 
		--Convert(varchar(25),[dbo].MMS_Meeting_Details.Chairperson) as Chairperson,

		  (Select EmployeeDisplayName from [dbo].Employee_Master where [dbo].MMS_Meeting_Details.Chairperson=Employee_Master.EmployeeICode) as Chairperson,

			[dbo].mms_meeting_Details.Task_Id,
			[dbo].mms_meeting_Details.Meeting_Id,
			[dbo].mms_meeting_Details.Employee_Id,
			--[dbo].mms_meeting_Details.Chairperson,
			-- (Select EmployeeDisplayName from [dbo].Employee_Master where [dbo].MMS_Meeting_Details.Chairperson=Employee_Master.EmployeeICode) as Chairperson,

			  (Select EmployeeDisplayName from [dbo].Employee_Master where [dbo].MMS_Meeting_Details.Participant=Employee_Master.EmployeeICode) as Participant,
			--Convert(varchar(25),[dbo].mms_meeting_Details.Participant) as Participant,
			
			[dbo].mms_meeting_Details.Task,
			[dbo].mms_meeting_Details.Completion_Date,
			[dbo].mms_meeting_Details.Priority,
			--[dbo].mms_meeting_Details.Status,
			ts.Name as Status
			,[dbo].mms_meeting_Details.Comments,
			[dbo].mms_meeting_Details.Percentage_Completed,
			--[dbo].mms_meeting_Details.Category,
			mmc.CategoryName as Category,
			[dbo].mms_meeting_Details.is_read_notification,
			[dbo].mms_meeting_Details.seen,
			[dbo].mms_meeting_Details.is_template,
			[dbo].mms_meeting_Details.NextReviewDate,
			[dbo].mms_meeting_Details.ReviewFrequencyID,
			[dbo].mms_meeting_Details.IsDeleted			
		FROM 
			[dbo].mms_meeting_Details   
			LEFT JOIN Task_Status ts on ts.ID=mms_meeting_Details.Status
			left join MMS_Meeting_Category mmc on mmc.ID= mms_meeting_Details.Category
  		WHERE 
			[dbo].mms_meeting_Details.Meeting_Id=@Meeting_Id 
	END  
RETURN






