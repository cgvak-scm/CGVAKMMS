CREATE TABLE [dbo].[Department_Master] (
    [DepartmentICode] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyICode]    INT          NOT NULL,
    [DepartmentName]  VARCHAR (50) NOT NULL,
    [IsActive]        BIT          NOT NULL,
    [CreatedBy]       INT          NOT NULL,
    [CreatedDate]     DATETIME     NOT NULL,
    [ModifiedBy]      INT          NULL,
    [ModifiedDate]    DATETIME     NULL,
    CONSTRAINT [PK_Department_Master] PRIMARY KEY CLUSTERED ([DepartmentICode] ASC)
);

