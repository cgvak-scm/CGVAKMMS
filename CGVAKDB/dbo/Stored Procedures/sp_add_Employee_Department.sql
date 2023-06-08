

CREATE PROCEDURE [dbo].[sp_add_Employee_Department]
	-- Add the parameters for the stored procedure here
	@employee_code int,
	@user_name nvarchar(100),
	
	@department_id int, 
	@emailid nvarchar(100)
AS
BEGIN
		INSERT INTO   [dbo].[Employee_Department](
		[Employee_Code]
		,[User_name]
		
		,[Department_Id]
		,[EmailId]
		)
		SELECT @employee_code, @user_name, @department_id, @emailid

		DECLARE @inserted bigint

		 SET @inserted =(SELECT SCOPE_IDENTITY())

		 SELECT
			[ID]
			,[Employee_Code]
			,[User_name]
			
			,[Department_Id]
			,[EmailID]
		FROM [dbo].[Employee_Department]
		WHERE [ID]=@inserted
	

END




