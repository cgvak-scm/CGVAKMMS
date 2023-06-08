CREATE TABLE [dbo].[MMS_Meeting_Template_Users] (
    [uid]          INT IDENTITY (1, 1) NOT NULL,
    [tid]          INT NULL,
    [participants] INT NULL,
    CONSTRAINT [PK_MMS_Meeting_Template_Users] PRIMARY KEY CLUSTERED ([uid] ASC)
);

