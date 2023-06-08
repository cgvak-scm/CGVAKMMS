CREATE TABLE [dbo].[MMS_Meeting_Status] (
    [Meeting_Status_Employee_Id]       INT           NOT NULL,
    [Meeting_Status_Task]              VARCHAR (250) NULL,
    [Meeting_Status_ResponsiblePerson] INT           NULL,
    [Meeting_Status_Date]              DATETIME      NULL,
    [Meeting_Status_Comments]          VARCHAR (MAX) NULL,
    [Meeting_Status]                   INT           NULL,
    [Meeting_Id]                       INT           NULL,
    [Meeting_Status_Hours]             NVARCHAR (50) CONSTRAINT [DF_MMS_Meeting_Status_Meeting_Status_Hours] DEFAULT (N'HH:MM') NULL,
    [Commented_By]                     INT           NULL,
    [Commented_By_ID]                  INT           NULL,
    [MeetingStatusAutoId]              INT           IDENTITY (1, 1) NOT NULL,
    [Task_Id]                          INT           NULL,
    [Meeting_Date1]                    DATETIME      NULL,
    CONSTRAINT [Pk_StatusAutoId] PRIMARY KEY CLUSTERED ([MeetingStatusAutoId] ASC)
);

