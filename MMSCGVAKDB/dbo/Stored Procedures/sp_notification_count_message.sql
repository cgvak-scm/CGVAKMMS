

CREATE PROCEDURE [dbo].[sp_notification_count_message]  
@Meeting_Id_In [int]
AS  
 BEGIN  
  SELECT    
  count(*) as totalCounts 
  FROM    
   [dbo].MMS_Meeting_Status  
  WHERE    
   [dbo].MMS_Meeting_Status.Meeting_Id = @Meeting_Id_In   
  END  
RETURN




