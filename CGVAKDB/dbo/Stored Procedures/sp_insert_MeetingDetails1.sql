

CREATE PROCEDURE [dbo].[sp_insert_MeetingDetails1]      
@Mid [int],      
@Eid [int],      
@Cperson [int],       
@Participant [int],      
@Task [varchar](250),      
@Ctime [datetime],      
@Priority  [varchar](50), 
@Status [int],      
@Comments  [varchar](max),     
@Category  [varchar](max),    
@Isread   varchar(10),  
@seen bit ,
@istemplate bit   ,
@NextRevDt [datetime],      
@Revfreq  [varchar](50),
@TemplateID [int]

--,@Review_Status bit


AS      
     
--	 IF @Revfreq = '1' BEGIN      
--  SET @Revfreq='1'    
-- END 
--  IF @Revfreq = '7' BEGIN      
--  SET @Revfreq='2'    
-- END 
--  IF @Revfreq = '14' BEGIN      
--  SET @Revfreq='3'    
-- END 
--  IF @Revfreq = '30' BEGIN      
--  SET @Revfreq='4'    
-- END 
--  IF @Revfreq = '90' BEGIN      
--  SET @Revfreq='5'    
-- END 
-- IF @Revfreq = '180' BEGIN      
--  SET @Revfreq='6'    
-- END
-- IF @Revfreq = '365' BEGIN      
--  SET @Revfreq='7'    
-- END

 BEGIN  
 INSERT INTO [dbo].MMS_Meeting_Details      
 (      
  [dbo].MMS_Meeting_Details.[Meeting_Id],      
  [dbo].MMS_Meeting_Details.[Employee_Id],      
  [dbo].MMS_Meeting_Details.[ChairPerson],          
  [dbo].MMS_Meeting_Details.[Participant],      
  [dbo].MMS_Meeting_Details.[Task],      
  [dbo].MMS_Meeting_Details.[Completion_Date],      
  [dbo].MMS_Meeting_Details.[Priority], 
  [dbo].MMS_Meeting_Details.[Status],     
  [dbo].MMS_Meeting_Details.[Comments],      
  [dbo].MMS_Meeting_Details.[Category],    
  [dbo].MMS_Meeting_Details.[is_read_notification] ,   
  [dbo].MMS_Meeting_Details.[seen],
  [dbo].MMS_meeting_details.[is_template]  ,
  [dbo].MMS_meeting_details.[NextReviewDate]  ,
  [dbo].MMS_meeting_details.[ReviewFrequencyID],
  [dbo].MMS_meeting_details.[TemplateID]


--  [dbo].MMS_Meeting_Details.Review_Status

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
  @Status,       
  @Comments,    
  @Category,    
  @Isread,  
  @seen,
 @istemplate,
 @NextRevDt,
 @Revfreq,
 @TemplateID
 
 --, 0

 )

INSERT INTO MMS_Track_ReviewTasks(TaskId,NextReviewDate,ActualReviewedDate,Review_Status)
SELECT MAX(mmd.Task_Id), @NextRevDt,GETDATE(),0
FROM dbo.MMS_Meeting_Details mmd
WHERE Task = @Task

END


