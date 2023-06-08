CREATE TABLE [dbo].[MMS_Login_Master] (
    [UserName]      VARCHAR (50) NOT NULL,
    [Employee_Id]   INT          NOT NULL,
    [User_Status]   BIT          NOT NULL,
    [Modify_Date]   DATETIME     NULL,
    [password]      VARCHAR (10) NULL,
    [Member_Rights] BIT          NULL,
    CONSTRAINT [PK_MMS_Login_Master] PRIMARY KEY CLUSTERED ([UserName] ASC),
    CONSTRAINT [fkey_empid] FOREIGN KEY ([Employee_Id]) REFERENCES [dbo].[MMS_Employee_Master] ([Employee_Id])
);

