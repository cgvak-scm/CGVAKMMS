

-- =============================================
-- Author:		<>
-- Create date: <>
-- Description:	<Get all User Departments>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_Employee_By_Email]
	-- Add the parameters for the stored procedure here
	@email varchar(100)
AS
BEGIN
		SELECT   
		[ID]
		,[Employee_Code]
		,[User_name]
		,[Department_Id]
		,[EmailId]
		FROM [dbo].[Employee_Department]
		WHERE [EmailId]=@email
		

END




