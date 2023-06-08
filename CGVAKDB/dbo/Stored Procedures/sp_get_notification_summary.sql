


CREATE  PROCEDURE [dbo].[sp_get_notification_summary]         
 @ChairPerson int,                    
 @Participant int,            
 --@Status varchar(20),                   
 @Status INT,
 @Priority varchar(10),
 @MeetingId int                           
AS                    
 --IF @Participant = '0'          
 --BEGIN            
 -- SET @Participant='%'          
 --END               
           
 --IF @Status = 'ALL'          
 --BEGIN            
 -- SET @Status='%'          
 --END           
             
 IF @Priority = 'ALL'          
 BEGIN            
  SET @Priority='%'          
 END           
           
BEGIN    
SELECT    
  [dbo].mms_meeting_details.Employee_Id, EmpMast.Employee_FirstName + ' ' + EmpMast.Employee_LastName as Participant,   
  Count(*) as TotalTasks,   
  (SELECT COUNT(StatusUpdate.MeetingID)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail     
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId        
   AND MeetingDetail.Task = StatusUpdate.Task         
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId   
  WHERE   
   MeetingDetail.chairperson = @ChairPerson   
   AND StatusUpdate.EmployeeID = [dbo].mms_meeting_details.Employee_Id   
   AND StatusUpdate.status = 5   
   AND MeetingDetail.Meeting_Id=@MeetingId
  ) as Assigned,   
  (SELECT COUNT(StatusUpdate.MeetingID)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail     
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId        
   AND MeetingDetail.Task = StatusUpdate.Task         
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId   
  WHERE   
   MeetingDetail.chairperson = @ChairPerson   
   AND StatusUpdate.EmployeeID = [dbo].mms_meeting_details.Employee_Id   
   AND StatusUpdate.status = 3   
   AND MeetingDetail.Meeting_Id=@MeetingId 
  ) as InProgress,   
  (SELECT COUNT(StatusUpdate.MeetingID)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail     
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId        
   AND MeetingDetail.Task = StatusUpdate.Task         
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId   
  WHERE   
   MeetingDetail.chairperson = @ChairPerson   
   AND StatusUpdate.EmployeeID = [dbo].mms_meeting_details.Employee_Id   
   AND StatusUpdate.status = 2   
   AND MeetingDetail.Meeting_Id=@MeetingId
  ) as Completed,   
  (SELECT COUNT(StatusUpdate.MeetingID)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail     
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId        
   AND MeetingDetail.Task = StatusUpdate.Task         
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId   
  WHERE   
   MeetingDetail.chairperson = @ChairPerson   
   AND StatusUpdate.EmployeeID = [dbo].mms_meeting_details.Employee_Id   
   AND StatusUpdate.status = 1   
   AND MeetingDetail.Meeting_Id=@MeetingId
  ) as Closed,   
  (SELECT COUNT(Priority)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail     
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id= StatusUpdate.EmployeeId        
   AND MeetingDetail.Task= StatusUpdate.Task         
   AND MeetingDetail.Meeting_Id= StatusUpdate.MeetingId     
  WHERE    
   MeetingDetail.Priority = 'High'     
   AND MeetingDetail.chairperson = @ChairPerson   
   AND MeetingDetail.Meeting_Id=@MeetingId
   AND MeetingDetail.Employee_Id = [dbo].mms_meeting_details.Employee_Id 
   AND StatusUpdate.Status != 1 
  ) as High,     
(SELECT COUNT(Priority)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail    
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id= StatusUpdate.EmployeeId        
   AND MeetingDetail.Task= StatusUpdate.Task        
   AND MeetingDetail.Meeting_Id= StatusUpdate.MeetingId     
  WHERE    
   MeetingDetail.Priority = 'Medium'     
   AND MeetingDetail.chairperson = @ChairPerson   
   AND MeetingDetail.Meeting_Id=@MeetingId
   AND MeetingDetail.Employee_Id = [dbo].mms_meeting_details.Employee_Id  
   AND StatusUpdate.Status != 1  
  ) as Medium,     
  (SELECT COUNT(Priority)     
  FROM    
   [dbo].mms_meeting_details MeetingDetail    
   INNER JOIN [dbo].mms_meeting_status_update StatusUpdate ON     
   MeetingDetail.Employee_Id= StatusUpdate.EmployeeId        
   AND MeetingDetail.Task= StatusUpdate.Task         
   AND MeetingDetail.Meeting_Id= StatusUpdate.MeetingId     
  WHERE    
   MeetingDetail.Priority = 'Low'     
   AND MeetingDetail.chairperson = @ChairPerson   
   AND MeetingDetail.Meeting_Id=@MeetingId
   AND MeetingDetail.Employee_Id = [dbo].mms_meeting_details.Employee_Id  
   AND StatusUpdate.Status != 1
  ) as Low     
FROM     
 [dbo].mms_meeting_details     
 INNER JOIN [dbo].mms_meeting_status_update ON    
 [dbo].mms_meeting_details.Employee_Id= [dbo].mms_meeting_status_update.EmployeeId        
 AND [dbo].mms_meeting_details.Task= [dbo].mms_meeting_status_update.Task         
 AND [dbo].mms_meeting_details.Meeting_Id= [dbo].mms_meeting_status_update.MeetingId   
 INNER JOIN [dbo].MMS_Employee_Master EmpMast ON   
 EmpMast.Employee_Id = [dbo].mms_meeting_details.Employee_Id   
WHERE     
 [dbo].mms_meeting_details.chairperson = @ChairPerson   
 AND --[dbo].mms_meeting_details.Employee_Id LIKE @Participant   
 [dbo].mms_meeting_details.Employee_Id= iif(@Participant<>0,@Participant, [dbo].mms_meeting_details.Employee_Id)
 AND [dbo].mms_meeting_status_update.status = @Status   
 AND mms_meeting_details.Meeting_Id=@MeetingId
 AND [dbo].mms_meeting_details.Priority LIKE @Priority   
GROUP BY [dbo].mms_meeting_details.Employee_Id, EmpMast.Employee_FirstName, EmpMast.Employee_LastName 
ORDER BY EmpMast.Employee_FirstName, EmpMast.Employee_LastName 
END            
RETURN




