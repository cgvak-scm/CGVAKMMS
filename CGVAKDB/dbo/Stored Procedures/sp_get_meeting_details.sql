



CREATE PROCEDURE [dbo].[sp_get_meeting_details] 

@Meeting_Id  int  

AS  

BEGIN    

 SELECT    

  [dbo].MMS_Meeting_Details.Task_Id,

  [dbo].MMS_Meeting_Details.Meeting_Id,     

  [dbo].MMS_Meeting_Details.Employee_Id,

  [dbo].MMS_Meeting_Details.Chairperson,

  [dbo].MMS_Meeting_Details.Participant,

  [dbo].MMS_Meeting_Details.Task,
 [dbo].MMS_Meeting_Details.Category,
 -- mmc.CategoryName as Category,


  Convert(varchar(11),[dbo].MMS_Meeting_Details.Completion_Date,101) AS [Completion_Date],

  [dbo].MMS_Meeting_Details.Priority,

  [dbo].MMS_Meeting_Details.Status,
 -- ts.Name as Status,

  convert(int,datediff(dd,mms_meeting_details.Completion_Date,Getdate())) as 'Overdue_Days',

  [dbo].MMS_Meeting_Details.Comments

 FROM   

  [dbo].MMS_Meeting_Details  
  --LEFT JOIN Task_Status ts on ts.ID=mms_meeting_Details.Status
  --LEFT JOIN MMS_Meeting_Category mmc on mmc.ID=mms_meeting_Details.Status


 WHERE   

  [dbo].MMS_Meeting_Details.Meeting_Id =@Meeting_Id 

END    

RETURN





