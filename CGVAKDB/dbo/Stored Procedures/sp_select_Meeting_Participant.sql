

CREATE PROCEDURE [dbo].[sp_select_Meeting_Participant]
@MeetingId_In Int
AS
BEGIN
	SELECT
		[dbo].MMS_Meeting_Participant.Participant_Id,
		[dbo].MMS_Meeting_Participant.Participant_Name
	FROM
		[dbo].MMS_Meeting_Participant		
	WHERE
		[dbo].MMS_Meeting_Participant.Meeting_Id=@MeetingId_In
END
RETURN






