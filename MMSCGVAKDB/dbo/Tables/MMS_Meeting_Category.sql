CREATE TABLE [dbo].[MMS_Meeting_Category] (
    [ID]           INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName] VARCHAR (50) NULL,
    CONSTRAINT [PK_MMS_Meeting_Category] PRIMARY KEY CLUSTERED ([ID] ASC)
);

