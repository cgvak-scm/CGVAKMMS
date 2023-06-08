

CREATE PROCEDURE [dbo].[sp_insert_status_update]    
@MeetingId int,    
@EmployeeId int,    
@Task varchar(250),    
--@Status varchar(30),
@Status INT,
@TaskId int    
AS    
BEGIN
	INSERT INTO [dbo].mms_meeting_status_update    
	(
		[dbo].mms_meeting_status_update.MeetingId,
		[dbo].mms_meeting_status_update.EmployeeId,
		[dbo].mms_meeting_status_update.Task,
		[dbo].mms_meeting_status_update.status,
		[dbo].mms_meeting_status_update.TaskId
	)
	VALUES    
	(    
		@MeetingId ,    
		@EmployeeId,    
		@Task ,    
		@Status,
		@TaskId    
	)    
RETURN   
END




