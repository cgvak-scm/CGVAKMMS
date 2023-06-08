


CREATE View [dbo].[MailDetail_TaskNotCompleted] as 
select [MMS_Meeting_Details].Task ,  
   [MMS_Meeting_Details].Chairperson as [Assigned_By],  
   [MMS_Meeting_Details].[Priority]  ,
   [MMS_Meeting_Details].Comments,  
   [MMS_Meeting_Master].Meeting_Date as Allocated_Date,  
   [MMS_Meeting_Details].Completion_Date as Completed_date,  
 convert(int,datediff(dd,  [MMS_Meeting_Details].Completion_Date,Getdate())) as Overdue_Days, 
 [MMS_Meeting_Details].Meeting_Id, 
   [MMS_Employee_Master] .Employee_EmailAddress,
   [MMS_Employee_Master] .Employee_FirstName
from   
  [MMS_Meeting_Details] ,  
  [MMS_Meeting_Master] ,  
  [Mms_Meeting_Status_Update] ,
  [MMS_Employee_Master]  
Where   
   [Mms_Meeting_Status_Update].MeetingId =  [MMS_Meeting_Details].Meeting_Id   
 and   
   [MMS_Meeting_Details].Meeting_Id =  [MMS_Meeting_Master].Meeting_Id   
 and  
   [Mms_Meeting_Status_Update].EmployeeId =  [MMS_Meeting_Details].Employee_Id   
 and  
   [Mms_Meeting_Status_Update].EmployeeId=  [MMS_Employee_Master].profile_employee_id 
 and  
   [MMS_Meeting_Details].Task =  [Mms_Meeting_Status_Update].Task
 and  
   [MMS_Meeting_Details].Completion_Date between DateAdd(DD,-90,GETDATE() ) and DateAdd(DD,-1,GETDATE() )  
 and  
   [Mms_Meeting_Status_Update].[status] <> 'Closed'  
 and  
   [Mms_Meeting_Status_Update].[status] <> 'Completed'  
 and 
   [MMS_Employee_Master] .Employee_EmailAddress is not null



