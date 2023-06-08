

---- exec [sp_select_meeting_status] 11553

CREATE PROCEDURE [dbo].[sp_select_meeting_status]  
--@Meeting_Id_In [int],    
@Status_Task_In  [int] 

--@Status_ResponsiblePerson_In varchar(20)

--@Status_ResponsiblePerson_In int

AS  
 BEGIN  
  SELECT  Meeting_Status_Employee_Id,
Meeting_Status_Task,
Convert(varchar(20),Meeting_Status_ResponsiblePerson) as Meeting_Status_ResponsiblePerson,
--Convert(varchar(20),Meeting_Status_Date,105) as Meeting_Status_Date,
Convert(VARCHAR(20), Meeting_Status_Date, 105) + ' '  + convert(VARCHAR(8), Meeting_Status_Date, 14)  as Meeting_Status_Date,
Meeting_Status_Comments,
--Convert(varchar(20),Meeting_Status) as Meeting_Status,
ts.Name as Meeting_Status,
MMS_Meeting_Status.Meeting_Id,
Meeting_Status_Hours,
Convert(varchar(20),Commented_By) as Commented_By,
Commented_By_ID,
MeetingStatusAutoId,
MMS_Meeting_Status.Task_Id
--from
--  select * from [dbo].MMS_Meeting_Status.*  
  FROM    
   [dbo].MMS_Meeting_Status 
   LEFT JOIN Task_Status ts on ts.ID=[dbo].MMS_Meeting_Status .Meeting_Status 
   --join MMS_Meeting_Details MMD on MMD.Task_Id=MMS_Meeting_Status.Task_Id
  WHERE    
  -- [dbo].MMS_Meeting_Status.Meeting_Id = @Meeting_Id_In   
  --AND  
     [dbo].MMS_Meeting_Status.Task_Id=@Status_Task_In 
	 
	-- or MMD.Task_Id=@Status_Task_In
	
 -- AND COnvert(varchar(20),[dbo].MMS_Meeting_Status.Meeting_Status_ResponsiblePerson)=@Status_ResponsiblePerson_In

  
  
  ORDER BY  
   [dbo].MMS_Meeting_Status.Meeting_Status_Date DESC  
 END  






