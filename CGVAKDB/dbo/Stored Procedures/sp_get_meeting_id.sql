

CREATE PROCEDURE [dbo].[sp_get_meeting_id]
AS
BEGIN
SELECT 
	Max(Meeting_ID)
FROM
	[dbo].MMS_Meeting_Master
RETURN
END






