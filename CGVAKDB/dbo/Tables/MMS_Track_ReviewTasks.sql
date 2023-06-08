CREATE TABLE [dbo].[MMS_Track_ReviewTasks] (
    [ID]                 INT      IDENTITY (1, 1) NOT NULL,
    [TaskId]             INT      NULL,
    [NextReviewDate]     DATETIME NULL,
    [ActualReviewedDate] DATETIME NULL,
    [Review_Status]      BIT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

