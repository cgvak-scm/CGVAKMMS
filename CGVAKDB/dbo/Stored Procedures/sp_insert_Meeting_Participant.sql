

CREATE PROCEDURE [dbo].[sp_insert_Meeting_Participant]
@MeetingId_In Int,
@ParticipantId_In Int,
@ParticipantName_In Varchar(50)
AS
BEGIN
	INSERT INTO [dbo].MMS_Meeting_Participant
	(
		[dbo].MMS_Meeting_Participant.Meeting_Id,
		[dbo].MMS_Meeting_Participant.Participant_Id,
		[dbo].MMS_Meeting_Participant.Participant_Name
	)
	VALUES
	(
		@MeetingId_In, 
		@ParticipantId_In,
		@ParticipantName_In 
	)
RETURN
END






