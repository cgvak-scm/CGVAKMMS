

CREATE Procedure [dbo].[Sp_Update_MMS_Meeting_Details_Change]  
 @emp_id int,
@Task_Id  Int ,  
@Completion_Date Datetime,  
--@Status varchar(20),  
@Status INT,
--@Category nvarchar(20),  
@Category INT,
@MeetingId int,  
@Comments nvarchar(MAX),
@NextReviewDate datetime,
@ReviewFrequencyID int  ,
@commented_by int,
@task varchar(30),
@Priority varchar(10)
as   
BEGIN   
  
 UPDATE MMS_Meeting_Details    
 set Category =@Category ,  
 Completion_Date=@Completion_Date,  
 Status=@Status,  
Comments=@Comments,
 NextReviewDate=@NextReviewDate,
 ReviewFrequencyID=@ReviewFrequencyID,
 Priority=@Priority
 where  Task_Id =@Task_Id  
   
 --UPDATE mms_meeting_status_update  
 --set Status = @Status  
 --where TaskId = @Task_Id  

   INSERT into  mms_meeting_status (Meeting_Id,Meeting_Status_Employee_Id,meeting_status_comments,Task_Id,Meeting_Status_Task,Commented_By,Commented_By_ID,Meeting_Status_ResponsiblePerson,Meeting_Status_Date,Meeting_Status) values
   (@MeetingId,@emp_id,@comments,@Task_Id,@task,@commented_by,@emp_id,@emp_id, Format(getdate(),'M/dd/yyyy h:mm:ss tt'),@Status)
  --(@MeetingId,@emp_id,@comments,@Task_Id,@task,@commented_by,(select EmployeeFirstName from Employee_Master where 
  --EmployeeICode=@emp_id), Format(getdate(),'M/dd/yyyy h:mm:ss tt'),@Status)

   
  --insert into  mms_meeting_status (Meeting_Id,Meeting_Status_Employee_Id,meeting_status_comments,Task_Id,Meeting_Status_Task,Commented_By,Meeting_Status_ResponsiblePerson,Meeting_Status_Date,Meeting_Status) values
  --(@MeetingId,@emp_id,@comments,@Task_Id,@task,@commented_by,(select EmployeeFirstName from Employee_Master where EmployeeICode=@emp_id), CONVERT(VARCHAR(50), getdate(), 101),@Status )
    
 
   
END





