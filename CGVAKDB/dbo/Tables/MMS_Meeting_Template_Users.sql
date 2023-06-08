CREATE TABLE [dbo].[MMS_Meeting_Template_Users] (
    [uid]          INT IDENTITY (1, 1) NOT NULL,
    [tid]          INT NULL,
    [participants] INT NULL,
    PRIMARY KEY CLUSTERED ([uid] ASC)
);

