CREATE TABLE [dbo].[MMS_Employee_Master] (
    [Employee_Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Employee_FirstName]     VARCHAR (200) NOT NULL,
    [Employee_LastName]      VARCHAR (200) NOT NULL,
    [Employee_DateofBirth]   DATETIME      NULL,
    [Employee_Address]       VARCHAR (500) NULL,
    [Employee_Department]    VARCHAR (500) NULL,
    [Employee_Qualification] VARCHAR (500) NULL,
    [Employee_DateofJoin]    DATETIME      NULL,
    [Employee_ContactNo]     VARCHAR (20)  NULL,
    [Employee_Status]        BIT           NOT NULL,
    [Employee_ModifyDate]    DATETIME      NULL,
    [Employee_EmailAddress]  VARCHAR (500) NULL,
    [profile_employee_id]    INT           NULL,
    CONSTRAINT [PK_MMS_Employee_Master] PRIMARY KEY CLUSTERED ([Employee_Id] ASC)
);

