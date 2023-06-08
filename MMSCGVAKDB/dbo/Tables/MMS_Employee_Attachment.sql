CREATE TABLE [dbo].[MMS_Employee_Attachment] (
    [Id]                INT          NULL,
    [MMS_FileName]      VARCHAR (50) NULL,
    [MMS_FileExtension] VARCHAR (10) NULL,
    [MMS_File]          IMAGE        NULL,
    [IsActive]          BIT          NOT NULL,
    [AttachmentID]      INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_MMS_ImageAttachment] PRIMARY KEY CLUSTERED ([AttachmentID] ASC)
);

