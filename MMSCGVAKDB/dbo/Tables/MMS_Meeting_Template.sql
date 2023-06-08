CREATE TABLE [dbo].[MMS_Meeting_Template] (
    [tid]                 INT           IDENTITY (1, 1) NOT NULL,
    [meeting_name]        VARCHAR (150) NULL,
    [meeting_time]        VARCHAR (40)  NULL,
    [meeting_duration]    VARCHAR (30)  NULL,
    [meeting_venue]       VARCHAR (150) NULL,
    [department]          VARCHAR (50)  NULL,
    [objective]           VARCHAR (150) NULL,
    [meeting_chairperson] INT           NULL,
    PRIMARY KEY CLUSTERED ([tid] ASC)
);

