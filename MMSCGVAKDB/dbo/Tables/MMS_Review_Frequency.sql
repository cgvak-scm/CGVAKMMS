CREATE TABLE [dbo].[MMS_Review_Frequency] (
    [Freq_Id]      INT          NOT NULL,
    [Freq_Name]    VARCHAR (50) NULL,
    [Freq_In_Days] INT          NULL,
    [Active_Flag]  VARCHAR (1)  NULL,
    CONSTRAINT [PK_MMS_Review_Frequency] PRIMARY KEY CLUSTERED ([Freq_Id] ASC)
);

