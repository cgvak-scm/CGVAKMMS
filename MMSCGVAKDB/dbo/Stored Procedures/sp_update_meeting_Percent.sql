

 CREATE PROCEDURE [dbo].[sp_update_meeting_Percent]      
@MeetingId int,      
@EmployeeId int,      
@Percent numeric(5,2),
@TaskId  int
AS      
BEGIN  
 Update    
  [dbo].MMS_Meeting_Details      
 Set    
  [dbo].MMS_Meeting_Details.Percentage_Completed =@Percent      
 Where      
  [dbo].MMS_Meeting_Details.Task_Id=@TaskId       
 AND      
  [dbo].MMS_Meeting_Details.Employee_Id =@EmployeeId       
RETURN    
END




