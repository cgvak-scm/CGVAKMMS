CREATE TABLE [dbo].[MMS_Notifications] (
    [notify_id]   INT IDENTITY (1, 1) NOT NULL,
    [meeting_id]  INT NULL,
    [is_read]     INT CONSTRAINT [def_read] DEFAULT ((0)) NULL,
    [chairperson] INT NULL,
    CONSTRAINT [PK_MMS_Notifications] PRIMARY KEY CLUSTERED ([notify_id] ASC),
    CONSTRAINT [fk_meet_id] FOREIGN KEY ([meeting_id]) REFERENCES [dbo].[MMS_Meeting_Master] ([Meeting_Id])
);

