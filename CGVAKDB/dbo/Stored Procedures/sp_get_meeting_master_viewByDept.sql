

-- exec  [dbo].[sp_get_meeting_master_viewByDept] 10009,'2018-11-01','2018-11-30','All'

CREATE PROCEDURE [dbo].[sp_get_meeting_master_viewByDept] 

@Chairperson  int,
@fromDate Datetime,
@toDate Datetime,
@department varchar(50)

AS  
 IF @department = 'ALL' BEGIN      
  SET @department='%'    
 END        
BEGIN    

 select distinct
[dbo].MMS_Meeting_Master.Meeting_Id,
[dbo].MMS_Meeting_Master.Meeting_Objective,
[dbo].MMS_Meeting_Master.Meeting_Type,
--Convert(varchar(20),[dbo].MMS_Meeting_Master.Meeting_Chairperson) as[Meeting_Chairperson],

  (Select EmployeeDisplayName from [dbo].Employee_Master where [dbo].MMS_Meeting_Master.Meeting_Chairperson=Employee_Master.EmployeeICode) as [Meeting_Chairperson],


 --Convert(varchar(11),[dbo].MMS_Meeting_Master.Meeting_Date,101) AS [Meeting_Date],

 --(Select EmployeeDisplayName from [dbo].Employee_Master where meetingDetails.Participant=Employee_Master.EmployeeICode) as Participant,


 CAST(DAY([dbo].MMS_Meeting_Master.Meeting_Date) AS VARCHAR(2))+ '-' +  Convert(char(3), [dbo].MMS_Meeting_Master.Meeting_Date, 0)
           + '-' + CAST(YEAR([dbo].MMS_Meeting_Master.Meeting_Date) AS VARCHAR(4)) AS [Meeting_Date],
[dbo].MMS_Meeting_Master.Meeting_Time,
[dbo].MMS_Meeting_Master.Meeting_Duration,

[dbo].MMS_Meeting_Master.Meeting_Venue,
[dbo].MMS_Meeting_Master.Meeting_Department,
[dbo].MMS_Meeting_Master.Meeting_No_Of_Participants
from
  [dbo].MMS_Meeting_Master 
  join [dbo].MMS_Meeting_Details
  
  on
   MMS_Meeting_Master.Meeting_Id=MMS_Meeting_Details.Meeting_Id
   join MMS_Meeting_Participant on MMS_Meeting_Master.Meeting_Id=MMS_Meeting_Participant.Meeting_Id
where 

([dbo].MMS_Meeting_Master.Meeting_Date between @fromDate and @toDate
and 
[dbo].MMS_Meeting_Master.Meeting_Department like @department
and [dbo].MMS_Meeting_Details.IsDeleted=0)


END    

RETURN


