


---- exec [dbo].[getmeetingsummarydetailsNew] 3,284,0,'2017-3-1','2019-01-10','All','false',0


CREATE procedure [dbo].[getmeetingsummarydetailsNew]
 @ChairPerson int,              
 @Participant int,      
 @Status INT,
 @SDate DateTime,       
 @EDate DateTime,      
 @Priority varchar(10),  
 @IsNotClosed bit,   
 @TemplateID int
  AS  
    
 IF @Priority = 'ALL' BEGIN      
  SET @Priority='%'    
 END     
 
BEGIN

select 
DISTINCT
 meetingDetails.Meeting_Id 
,meetingDetails.Employee_Id  
,meetingDetails.Participant
,meetingDetails.Task
,meetingDetails.Task_Id
,meetingDetails.Completion_date AS Completion_date
,meetingDetails.Priority  
,MMS_Meeting_Master.Meeting_Date
,mmt.meeting_name as TemplateName
,meetingDetails.seen as Seen
,meetingDetails.IsDeleted as deleted
,meetingDetails.NextReviewDate as reviewdate
,ts.Name as Status
,ISNULL(MMS_Review_Frequency.Freq_Name,'') as reviewFreq  
,isnull((select TOP 1 Review_Status from MMS_Track_ReviewTasks where TaskId = meetingDetails.Task_Id order by ID desc),0) as ReviewStatus

--into MMS_Meeting_Details_test28092018
--drop table MMS_Meeting_Details_test28092018

 
From MMS_Meeting_Details meetingDetails 
inner join (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) statusUpdate 
 on statusUpdate.EmployeeId=meetingDetails.Employee_Id 
 and  statusUpdate.MeetingId=meetingDetails.Meeting_Id 
 and statusUpdate.Task=meetingDetails.Task
 --AND statusUpdate.Status=meetingDetails.Status

 --inner join MMS_Meeting_Status_Update statusUpdate on statusUpdate.EmployeeId=meetingDetails.Employee_Id 
 --and  statusUpdate.MeetingId=meetingDetails.Meeting_Id 
	-- and statusUpdate.Task=meetingDetails.Task


join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingDetails.Meeting_Id
left join MMS_Meeting_Template mmt on mmt.tid = meetingDetails.TemplateID
 left join MMS_Review_Frequency on meetingDetails.ReviewFrequencyID=MMS_Review_Frequency.Freq_In_Days
LEFT JOIN Task_Status ts on ts.ID=meetingDetails.Status
--FULL OUTER JOIN MMS_Track_ReviewTasks reviewTrack ON meetingDetails.Task_Id=reviewTrack.TaskId

    where meetingDetails.Chairperson=@ChairPerson
    AND meetingDetails.IsDeleted=0
    --AND meetingDetails.Employee_Id= iif(@Participant<>0,@Participant, meetingDetails.Employee_Id)  
	AND meetingDetails.Participant= iif(@Participant<>0,@Participant, meetingDetails.Participant)     
	AND meetingDetails.status = iif(@Status<>0,@Status, meetingDetails.status)  	
  --AND (meetingDetails.Completion_date Between @SDate AND @EDate)
	AND MMS_Meeting_Master.Meeting_Date Between @SDate AND @EDate     
    AND meetingDetails.Priority LIKE @Priority 
	AND meetingDetails.TemplateID= iif(@TemplateID<>0,@TemplateID, meetingDetails.TemplateID) 
	--	AND meetingDetails.Task_Id= iif(@Task_Id<>0,@Task_Id, meetingDetails.Task_Id) 

	order by MMS_Meeting_Master.Meeting_Date desc
	--and  meetingDetails.Meeting_Id>116107

END

-- select * from mms_meeting_details where task_id=787
-- select * from mms_meeting_master where meeting_id=97488

--select * from employeemaster where EmployeeDisplayName like '%samyu%'




