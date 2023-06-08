CREATE TABLE [dbo].[MMS_Attachment] (
    [Meeting_Id]          INT          NULL,
    [MMS_FileName]        VARCHAR (50) NULL,
    [MMS_FileExtension]   VARCHAR (10) NULL,
    [MMS_File]            IMAGE        NULL,
    [MeetingAttachmentID] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [Pk_AttachementAutoId] PRIMARY KEY CLUSTERED ([MeetingAttachmentID] ASC)
);

