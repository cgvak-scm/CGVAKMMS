


-- sp_select_meeting_name '3','2017-03-1','2018-11-26','All',0


CREATE PROCEDURE [dbo].[sp_select_Meeting_Name]      
@ChairPerson int,    
@StartDate_In DATETIME,    
@EndDate_In DATETIME,
@priority VARCHAR(20),
--@status VARCHAR(20)   
@status INT
AS      
 BEGIN      
   
    
  if(@priority='All')
    SET @priority='%'
  --if(@status='All')
  -- SET @status='%'
   
   --Convert(varchar(20),[dbo].MMS_Meeting_Details.Participant) As Participant


   
 if @status=0

   SELECT meetingDetails.meeting_id,meetingDetails.employee_id,meetingDetails.Task_Id,meetingDetails.Task,
 
  -- Convert(varchar(20),meetingDetails.participant) as Participant,
  --after change of names to Id, this below code is used to populate the database and the above is commented. 

  (Select EmployeeDisplayName from [dbo].Employee_Master where meetingDetails.Participant=Employee_Master.EmployeeICode) as Participant,


   meetingMaster.meeting_date,meetingDetails.completion_date,meetingDetails.priority,
   --meetingDetails.status, 
    Convert(varchar(20),ts.Name) as status,
   convert(int,datediff(dd,meetingDetails.Completion_Date,Getdate())) as 'Overdue_Days'
    from mms_meeting_master meetingMaster 
   join
   mms_meeting_details meetingDetails
   on meetingMaster.meeting_id=meetingDetails.meeting_id
   full join Task_Status ts on meetingDetails.Status=ts.ID
   where
    meetingMaster.Meeting_chairperson=@ChairPerson
   and 
   meetingDetails.IsDeleted=0
   and
	meetingDetails.Completion_Date BETWEEN @StartDate_In AND @EndDate_In
	and
	meetingDetails.priority like @priority
	 --and meetingDetails.status like @status


	 else if @status!=0

	  SELECT meetingDetails.meeting_id,meetingDetails.employee_id,meetingDetails.Task_Id,meetingDetails.Task,
 
  -- Convert(varchar(20),meetingDetails.participant) as Participant,
  --after change of names to Id, this below code is used to populate the database and the above is commented. 

  (Select EmployeeDisplayName from [dbo].Employee_Master where meetingDetails.Participant=Employee_Master.EmployeeICode) as Participant,


   meetingMaster.meeting_date,meetingDetails.completion_date,meetingDetails.priority,
   --meetingDetails.status, 
   ts.Name as status,
   convert(int,datediff(dd,meetingDetails.Completion_Date,Getdate())) as 'Overdue_Days'
    from mms_meeting_master meetingMaster 
   join
   mms_meeting_details meetingDetails
   on meetingMaster.meeting_id=meetingDetails.meeting_id
   full join Task_Status ts on meetingDetails.Status=ts.ID
   where
    meetingMaster.Meeting_chairperson=@ChairPerson
	and 
   meetingDetails.IsDeleted=0
   and 
	meetingDetails.Completion_Date BETWEEN @StartDate_In AND @EndDate_In
	and
	meetingDetails.priority like @priority
	 and meetingDetails.status like @status
 END      
RETURN



