


CREATE PROCEDURE [dbo].[sp_Check_Login_Details]  
 @Username [varchar](50)      
,@Password [varchar](50)            
      
AS            
BEGIN            
 SELECT       
   [dbo].[Employee_Master].[EmployeeIcode]  
 FROM      
   [dbo].[Employee_Master]  
  JOIN  
  [dbo].[MMS_Employee_Master]  
  ON  
   [dbo].[Employee_Master].EmployeeICode= [dbo].[MMS_Employee_Master].[profile_employee_id]  
 WHERE      
   [dbo].[Employee_Master].[LoginUserName] = @UserName      
  AND  [dbo].[Employee_Master].[LoginPassword] = @Password      
  AND  [dbo].[Employee_Master].[IsActive] = 1      
END




