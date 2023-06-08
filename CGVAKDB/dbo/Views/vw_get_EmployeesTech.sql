

CREATE VIEW [dbo].[vw_get_EmployeesTech]
AS
SELECT        TOP (100) PERCENT EM.EmployeeFirstName + ' ' + EM.EmployeeLastName AS EmployeeName, CONVERT(VARCHAR(25), EM.EmployeeDateOfJoining, 106) AS DOJ, EM.EmployeePrimarySkill, 
                         DM.Designation
FROM            dbo.Employee_Master AS EM LEFT OUTER JOIN
                         dbo.Designation_Master AS DM ON DM.DesignationICode = EM.DesignationICode
WHERE        (EM.IsActive = 1) AND (EM.DepartmentICode = 5) AND (EM.DesignationICode <> 32)
ORDER BY EmployeeName



