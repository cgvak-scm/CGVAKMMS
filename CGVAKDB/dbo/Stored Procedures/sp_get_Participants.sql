

 CREATE PROCEDURE  [dbo].[sp_get_Participants]    
@Chairperson int
AS    
BEGIN    
 SELECT    
    DISTINCT(meetingDetails.employee_id) ,
	(empMaster.employeeFirstname+' '+empMaster.EmployeeLastName ) as name, meetingDetails.Participant
 FROM    									  
  Mms_Meeting_details  meetingDetails  
  inner join  Employee_Master empMaster  ON empMaster.EmployeeICode=meetingDetails.Participant
  -- on empMaster.employeeFirstname+' '+empMaster.EmployeeLastName = meetingDetails.Participant
 

 WHERE    
 meetingDetails.chairperson = @Chairperson
  
 END    




