

CREATE PROCEDURE [dbo].[sp_getname_EmployeeMaster]  
@User [varchar](50)
  
AS  
BEGIN  
	SELECT
		[dbo].[Employee_Master].[EmployeeFirstName] + ' ' +
		[dbo].[Employee_Master].[EmployeeLastName]
	FROM
		[dbo].[Employee_Master]
	WHERE
		[dbo].[Employee_Master].[LoginUserName] = @User
END  
RETURN




