CREATE TABLE [dbo].[Employee_Department] (
    [ID]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Employee_Code] INT            NULL,
    [User_name]     NVARCHAR (100) NOT NULL,
    [Department_Id] INT            NOT NULL,
    [EmailID]       NVARCHAR (100) NULL
);

