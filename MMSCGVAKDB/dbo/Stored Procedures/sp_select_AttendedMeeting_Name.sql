

CREATE PROCEDURE [dbo].[sp_select_AttendedMeeting_Name]  
@Employee_Id INT,
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
   Meeting_Id IN ( SELECT DISTINCT   
      Meeting_Id  
     FROM   
      MMS_Meeting_Details  
     Where   
      Employee_Id = @Employee_Id)  
   AND
       Meeting_Date between @StartDate_In AND @EndDate_In     
 END  
  
  







