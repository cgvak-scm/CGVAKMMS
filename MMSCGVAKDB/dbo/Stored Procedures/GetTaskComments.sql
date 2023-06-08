



--  [dbo].[GetTaskComments] 11485
CREATE PROCEDURE [dbo].[GetTaskComments] 
	@TaskId int
AS
BEGIN
	
	SET NOCOUNT ON;    
	select * from (
select Meeting_Status_Comments as Comments,Meeting_Status_Date,  Commented_by,
Convert(varchar(20),Meeting_Status_Date) as StatusDate from MMS_Meeting_Status    
Where Task_Id = @TaskId) 
meetingstatus order by Meeting_Status_Date desc

END




