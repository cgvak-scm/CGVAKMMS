



-- =============================================
-- Author:		<Gokul Thangavel>
-- Create date: <23-May-2017>
-- Description:	<Get the UserDepartments>
-- =============================================
CREATE View [dbo].[vw_get_EmployeeDepartments]
AS
	SELECT 
	[ID]
	,[Employee_Code]
	,[User_name]
	,[Department_Id]
	,[EmailID]
	 FROM [dbo].[Employee_Department]



