CREATE TABLE [dbo].[MMS_Meeting_Participant] (
    [Meeting_Id]           INT          NULL,
    [Participant_Id]       INT          NULL,
    [Participant_Name]     VARCHAR (50) NULL,
    [MeetingParticipantId] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_MMS_Meeting_Participant] PRIMARY KEY CLUSTERED ([MeetingParticipantId] ASC),
    CONSTRAINT [FK__MMS_Meeti__Meeti__0B91BA14] FOREIGN KEY ([Meeting_Id]) REFERENCES [dbo].[MMS_Meeting_Master] ([Meeting_Id])
);

