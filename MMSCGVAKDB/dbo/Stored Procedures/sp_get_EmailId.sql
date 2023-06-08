

CREATE PROCEDURE [dbo].[sp_get_EmailId]  
@ParticpanId_In int  
AS      
BEGIN       
 SELECT       
  [dbo].employee_Master.employeecorporateemailid  
 FROM      
  [dbo].employee_master      
 WHERE      
  [dbo].employee_master.employeeicode=@ParticpanId_In     
 END      
RETURN




