CREATE TABLE [dbo].[Task_Status] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Descriptions] NVARCHAR (100) NULL,
    [Active]       BIT            NULL,
    CONSTRAINT [PK_Task_Status] PRIMARY KEY CLUSTERED ([ID] ASC)
);

