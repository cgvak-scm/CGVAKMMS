

CREATE PROCEDURE [dbo].[sp_insert_Attachment]
@Mid INT,
@Fname VARCHAR(50),
@Fextension VARCHAR(10),
@File IMAGE
AS
BEGIN
	INSERT INTO [dbo].MMS_Attachment
	(
		[dbo].MMS_Attachment.Meeting_Id,
		[dbo].MMS_Attachment.MMS_FileName,
		[dbo].MMS_Attachment.MMS_FileExtension,
		[dbo].MMS_Attachment.MMS_File
	)
	VALUES
	(
		@Mid,
		@Fname,
		@Fextension,
		@File
	)
RETURN
END







