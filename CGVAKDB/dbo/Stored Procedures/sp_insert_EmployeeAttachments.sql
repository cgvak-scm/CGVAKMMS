


CREATE PROCEDURE [dbo].[sp_insert_EmployeeAttachments]
@Mid INT,
@Fname VARCHAR(50),
@Fextension VARCHAR(10),
@File IMAGE,
@IsActive bit
AS
BEGIN
	INSERT INTO [dbo].MMS_Employee_Attachment
	(
		[dbo].MMS_Employee_Attachment.Id,
		[dbo].MMS_Employee_Attachment.MMS_FileName,
		[dbo].MMS_Employee_Attachment.MMS_FileExtension,
		[dbo].MMS_Employee_Attachment.MMS_File,
		[dbo].MMS_Employee_Attachment.IsActive
	)
	VALUES
	(
		@Mid,
		@Fname,
		@Fextension,
		@File,
		@IsActive
	)
RETURN
END








