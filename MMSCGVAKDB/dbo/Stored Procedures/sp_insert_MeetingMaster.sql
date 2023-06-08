


CREATE PROCEDURE [dbo].[sp_insert_MeetingMaster]        
@Objective_In [varchar](150),        
@Date_In [datetime],        
@Time_In [varchar](50),        
@Venue_In [varchar](40),      
@Type_In [varchar](150),      
@Duration_In [varchar](50),        
@Department_In [varchar](50),        
--@Chairperson_In [varchar](50),   
@Chairperson_In int,   
@Minutes_Of_Meeting_In [varchar](500),       
@Participants_In [int] ,
@Istemplate [bit],
@TemplateID [int]
AS        
BEGIN        
--[Meeting_Objective] is Meeting_Name
--[Meeting_Type] is Meeting_Objective
 INSERT INTO [dbo].MMS_Meeting_Master        
 (   
  [dbo].MMS_Meeting_Master.[Meeting_Objective],        
  [dbo].MMS_Meeting_Master.[Meeting_Date],        
  [dbo].MMS_Meeting_Master.[Meeting_Time],        
  [dbo].MMS_Meeting_Master.[Meeting_Venue],    
  [dbo].MMS_Meeting_Master.[Meeting_Type],    
  [dbo].MMS_Meeting_Master.[Meeting_Duration],        
  [dbo].MMS_Meeting_Master.[Meeting_Department],        
  [dbo].MMS_Meeting_Master.[Meeting_Chairperson],    
  [dbo].MMS_Meeting_Master.[Minutes_Of_Meeting],       
  [dbo].MMS_Meeting_Master.[Meeting_No_Of_Participants] ,
  [dbo].MMS_Meeting_Master.[Istemplate],
  [dbo].MMS_Meeting_Master.[TemplateID]
 )        
 VALUES        
 (   
  @Objective_In,        
  @Date_In,        
  @Time_In,     
  @Venue_In,      
  @Type_In,    
  @Duration_In,        
  @Department_In,        
  @Chairperson_In,   
  @Minutes_Of_Meeting_In,        
  @Participants_In     ,
  @Istemplate,
  @TemplateID
 )        
SELECT Meeting_Id FROM MMS_Meeting_Master WHERE Meeting_Id = SCOPE_IDENTITY();
END




