

---- exec [dbo].[Proc_MyTask] 284,'2017-03-01','2019-01-05',0,0


 CREATE procedure [dbo].[Proc_MyTask]
              
 @Participant int,      
 @SDate DateTime,       
 @EDate DateTime,        
 @Category int
, @ReviewFrequency int
 AS  
       
 
BEGIN

select 
DISTINCT
meetingDetails.task,
(Select EmployeeDisplayName from [dbo].Employee_Master where MMS_Meeting_Master.Meeting_Chairperson=Employee_Master.EmployeeICode) as 'Assigned_By'
,meetingDetails.Priority,
MMS_Review_Frequency.Freq_Name
,ISNULL(ISNULL((select top 1 Meeting_Status_Comments from [MMS_Meeting_Status] where Task_Id=meetingDetails.Task_Id order by Meeting_Status_Date desc), meetingDetails.Comments),'No Comments') Comments       
,ts.Name as Status,
meetingDetails.Completion_date AS 'Completed_date'
,MMS_Meeting_Master.Meeting_Date as Allocated_Date
,convert(int,datediff(dd,meetingDetails.Completion_Date,Getdate())) as 'Overdue_Days'
 ,meetingDetails.Meeting_Id
,convert(varchar(6), meetingDetails.Percentage_Completed)+' %' as Completed_Percentage
,mmc.CategoryName as Category
,isnull(d.mms_filename,'') as mms_filename
,meetingDetails.Task_Id

From MMS_Meeting_Details meetingDetails 
inner join (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) statusUpdate 
 on statusUpdate.EmployeeId=meetingDetails.Employee_Id 
 and  statusUpdate.MeetingId=meetingDetails.Meeting_Id 
 and statusUpdate.Task=meetingDetails.Task


join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingDetails.Meeting_Id
left join MMS_Meeting_Category mmc on mmc.ID= meetingDetails.Category
 left join MMS_Review_Frequency on meetingDetails.ReviewFrequencyID=MMS_Review_Frequency.Freq_In_Days
LEFT JOIN Task_Status ts on ts.ID=meetingDetails.Status
left join mms_attachment d  on d.meeting_id=meetingDetails.meeting_id
left join MMS_Meeting_Status on MMS_Meeting_Status.Meeting_Id=meetingDetails.Meeting_Id

    where meetingDetails.IsDeleted=0
	AND meetingDetails.Participant= iif(@Participant<>0,@Participant, meetingDetails.Participant)     	
  AND (MMS_Meeting_Master.Meeting_Date Between @SDate AND @EDate)    
	AND meetingDetails.Category= iif(@Category<>0,@Category, meetingDetails.Category) 
	AND meetingDetails.ReviewFrequencyID= iif(@ReviewFrequency<>0,@ReviewFrequency, meetingDetails.ReviewFrequencyID) 


END




