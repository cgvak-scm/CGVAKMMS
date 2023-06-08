

CREATE VIEW [dbo].[vw_get_Designations]
AS
SELECT     DesignationICode, CompanyICode, DepartmentICode, Designation, IsActive, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RoleICode
FROM         dbo.Designation_Master



