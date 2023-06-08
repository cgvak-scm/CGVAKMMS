

CREATE PROCEDURE [dbo].[sp_select_Meeting_Id]  
@Chairperson int,
@StartDate_In DateTime,
@EndDate_In DateTime    
AS  
 BEGIN  
  SELECT   
   [dbo].MMS_Meeting_Master.Meeting_Objective,  
   [dbo].MMS_Meeting_Master.Meeting_Id   
  FROM  
   [dbo].MMS_Meeting_Master  
  WHERE  
   [dbo].MMS_Meeting_Master.Meeting_Chairperson=@Chairperson  
  AND
    Meeting_Date between @StartDate_In AND @EndDate_In   
 END  
RETURN  







