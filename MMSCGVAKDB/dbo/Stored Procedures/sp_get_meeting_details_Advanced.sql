




CREATE PROCEDURE [dbo].[sp_get_meeting_details_Advanced] 
@Meeting_Id  int,
@Priority	VARCHAR(50),
--@Status		VARCHAR(50) 
@Status INT
AS  
BEGIN  
--IF @Status = 'ALL'  
--  BEGIN    
--  SET @Status='%'  
--  END   
     
 IF @Priority = 'ALL'  
  BEGIN    
  SET @Priority='%'  
  END  
 
 SELECT    
  [dbo].MMS_Meeting_Details.Meeting_Id,     
  [dbo].MMS_Meeting_Details.Employee_Id,
  [dbo].MMS_Meeting_Details.Chairperson,
  [dbo].MMS_Meeting_Details.Participant,
  [dbo].MMS_Meeting_Details.Task,
  Convert(varchar(11),[dbo].MMS_Meeting_Details.Completion_Date,103) AS [Completion_Date],
  [dbo].MMS_Meeting_Details.Priority,
  --[dbo].MMS_Meeting_Details.Status, 
  ts.Name as Status,
  convert(int,datediff(dd,mms_meeting_details.Completion_Date,Getdate())) as 'Overdue_Days',
  [dbo].MMS_Meeting_Details.Comments
 FROM   
  [dbo].MMS_Meeting_Details  
  LEFT JOIN Task_Status ts on ts.ID=mms_meeting_Details.Status
 WHERE   
  (
	[dbo].MMS_Meeting_Details.Meeting_Id =@Meeting_Id  AND
	Priority LIKE @Priority AND
	Status = @Status
  )

END




