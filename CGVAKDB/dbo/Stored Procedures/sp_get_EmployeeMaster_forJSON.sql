

   
  
   
CREATE PROCEDURE [dbo].[sp_get_EmployeeMaster_forJSON]    
@dept varchar(50)   
AS      
BEGIN      
      
 SELECT   
  [dbo].[Employee_Master].EmployeeICode as [Employee_Id]      
  --,[dbo].[Employee_Master].EmployeeFirstName + ' ' + EmployeeLastName as [Employee_Name]      
  ,[dbo].[Employee_Master].EmployeeDisplayName as [Employee_Name]      
  ,[dbo].[Employee_Master].EmployeeLastName as [Employee_LastName],  
  [dbo].[Employee_Master]. DepartmentICode as Department_ICode     
 FROM      
  [dbo].[Employee_Master]     
  JOIN    
  [dbo].[MMS_Employee_Master]    
  ON    
  [dbo].[Employee_Master].EmployeeICode=[dbo].[MMS_Employee_Master].[profile_employee_id]     
 WHERE      
  DepartmentICode = (SELECT DepartmentICode      
       FROM [dbo].[Department_Master]      
       WHERE DepartmentName = @dept)      
  AND IsActive = 1      
 ORDER BY      
  [dbo].[Employee_Master].EmployeeFirstName      
      
END      
RETURN




