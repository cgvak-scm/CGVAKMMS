CREATE TABLE [dbo].[MMS_Blood_Details] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Descriptions] NVARCHAR (MAX) NULL,
    [Values]       INT            NULL,
    [Active]       NCHAR (10)     NULL,
    CONSTRAINT [PK_MMS_Blood _Details] PRIMARY KEY CLUSTERED ([ID] ASC)
);

