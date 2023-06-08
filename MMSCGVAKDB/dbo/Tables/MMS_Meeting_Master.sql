CREATE TABLE [dbo].[MMS_Meeting_Master] (
    [Meeting_Id]                 INT           NOT NULL,
    [Meeting_Objective]          VARCHAR (150) NOT NULL,
    [Meeting_Date]               DATETIME      NOT NULL,
    [Meeting_Time]               VARCHAR (20)  NOT NULL,
    [Meeting_Venue]              VARCHAR (40)  NULL,
    [Meeting_Type]               VARCHAR (150) NULL,
    [Meeting_Duration]           VARCHAR (20)  NOT NULL,
    [Meeting_Department]         VARCHAR (50)  NOT NULL,
    [Meeting_Chairperson]        INT           NULL,
    [Minutes_Of_Meeting]         VARCHAR (500) NULL,
    [Meeting_No_Of_Participants] INT           NOT NULL,
    [istemplate]                 BIT           CONSTRAINT [DF__MMS_Meeti__istem__25869641] DEFAULT ((0)) NULL,
    [TemplateID]                 INT           NULL,
    CONSTRAINT [PK_MMS_Meeting_Master] PRIMARY KEY CLUSTERED ([Meeting_Id] ASC)
);

