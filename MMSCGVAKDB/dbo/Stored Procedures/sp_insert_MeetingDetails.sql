

    
CREATE PROCEDURE [dbo].[sp_insert_MeetingDetails]      
@Mid [int],      
@Eid [int],      
@Cperson int,       
@Participant int,      
@Task [varchar](250),      
@Ctime [datetime],      
@Priority  [varchar](50),      
@Comments  [varchar](max),     
@Category  [varchar](max),    
@Isread   varchar(10),  
@seen bit ,
@istemplate bit   ,
@NextRevDt [datetime],      
@Revfreq  [varchar](50)

--,

--@Review_Status bit



AS      
BEGIN      
 INSERT INTO [dbo].MMS_Meeting_Details      
 (      
  [dbo].MMS_Meeting_Details.[Meeting_Id],      
  [dbo].MMS_Meeting_Details.[Employee_Id],      
  [dbo].MMS_Meeting_Details.[Chairperson],      
  [dbo].MMS_Meeting_Details.[Participant],      
  [dbo].MMS_Meeting_Details.[Task],      
  [dbo].MMS_Meeting_Details.[Completion_Date],      
  [dbo].MMS_Meeting_Details.[Priority],      
  [dbo].MMS_Meeting_Details.[Comments],      
  [dbo].MMS_Meeting_Details.[Category],    
  [dbo].MMS_Meeting_Details.[is_read_notification] ,   
  [dbo].MMS_Meeting_Details.[seen],
  [dbo].MMS_meeting_details.[is_template]  ,
  [dbo].MMS_meeting_details.[NextReviewDate]  ,
  [dbo].MMS_meeting_details.[ReviewFrequencyID]  


  --,


  --[dbo].MMS_Meeting_Details.Review_Status
  )      
 VALUES      
 (      
  @Mid,      
  @Eid,      
  @Cperson,      
  @Participant,      
  @Task,      
  @Ctime,      
  @Priority,      
  @Comments,    
  @Category,    
  @Isread,  
  @seen,
 @istemplate,
 @NextRevDt,
 @Revfreq

 --,

 -- 0


 )      
--RETURN      
SELECT Task_Id FROM MMS_Meeting_Details WHERE Task_Id = SCOPE_IDENTITY();
END




