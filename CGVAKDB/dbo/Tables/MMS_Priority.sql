CREATE TABLE [dbo].[MMS_Priority] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Descriptions] NVARCHAR (100) NULL,
    [Active]       BIT            NULL
);

