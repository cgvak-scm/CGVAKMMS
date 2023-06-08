


CREATE Procedure [dbo].[Sp_Update_MMS_Meeting_Details_Change1]  
 @MeetingId int,
 @MeetingObjective varchar(150),
 @MeetingDate datetime,
 @MeetingTime varchar(40),
 @MeetingDuration varchar(40),
 @MeetingVenue varchar(40),
 @MeetingType varchar(150),
 @MeetingDepartment varchar(50),
 @MeetingChairperson int,
 @MinutesOfMeeting varchar(500)
 

as   
BEGIN   
  
 UPDATE MMS_Meeting_Master set Meeting_Objective =@MeetingObjective,  
 Meeting_Date=@MeetingDate,
 Meeting_Time=@MeetingTime,
 Meeting_Duration=@MeetingDuration,
 Meeting_Venue=@MeetingVenue,
 Meeting_Type=@MeetingType,
 Meeting_Department=@MeetingDepartment,
 Meeting_Chairperson=@MeetingChairperson,
 Minutes_Of_Meeting=@MinutesOfMeeting
 

 where  Meeting_Id =@MeetingId  
      
END


