CREATE TABLE [dbo].[MMS_Meeting_Status_Update] (
    [MeetingId]           INT           NULL,
    [EmployeeId]          INT           NULL,
    [Task]                VARCHAR (500) NULL,
    [status]              INT           NULL,
    [MeetingStatusAutoId] INT           IDENTITY (1, 1) NOT NULL,
    [TaskId]              INT           NULL,
    CONSTRAINT [PK_MMS_Meeting_Status_Update] PRIMARY KEY CLUSTERED ([MeetingStatusAutoId] ASC)
);

