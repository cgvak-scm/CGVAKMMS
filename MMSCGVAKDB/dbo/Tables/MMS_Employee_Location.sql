CREATE TABLE [dbo].[MMS_Employee_Location] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Description] NVARCHAR (50) NULL,
    [Values]      NVARCHAR (50) NULL,
    [Active]      INT           NULL,
    CONSTRAINT [PK_MMS_Employee_Location] PRIMARY KEY CLUSTERED ([ID] ASC)
);

