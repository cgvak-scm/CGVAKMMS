

-- =============================================
-- Author:		<>
-- Create date: <>
-- Description:	<Get all User Departments>
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_Employee_Department]
	-- Add the parameters for the stored procedure here
AS
BEGIN
		SELECT 

		   [EmployeeICode] as [Employee_Code]
      ,[EmployeeDisplayName] as [User_name]
      ,[DepartmentICode] as [Department_Id]
      ,[EmployeeCorporateEmailId] as [EmailID]
		
		 FROM [dbo].[vw_get_EmployeeDepartment]

		--SELECT [ID]
  --    ,[Employee_Code]
  --    ,[User_name]
  --    ,[Department_Id]
  --    ,[EmailID]
  --FROM [dbo].[vw_get_EmployeeDepartments]
END




