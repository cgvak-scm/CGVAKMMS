CREATE TABLE [dbo].[Designation_Master] (
    [DesignationICode] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyICode]     INT          NOT NULL,
    [DepartmentICode]  INT          NOT NULL,
    [Designation]      VARCHAR (50) NOT NULL,
    [IsActive]         BIT          NOT NULL,
    [CreatedBy]        INT          NOT NULL,
    [CreatedDate]      DATETIME     NOT NULL,
    [ModifiedBy]       INT          NULL,
    [ModifiedDate]     DATETIME     NULL,
    [RoleICode]        INT          NULL,
    [Isapprove]        INT          NULL,
    [parenticode]      INT          NULL,
    CONSTRAINT [PK_Designation_Master] PRIMARY KEY CLUSTERED ([DesignationICode] ASC)
);

