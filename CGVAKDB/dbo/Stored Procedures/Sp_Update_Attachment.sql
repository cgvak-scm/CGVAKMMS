


CREATE Procedure [dbo].[Sp_Update_Attachment]  
@MeetingId INT,
@Fname VARCHAR(50),
@Fextension VARCHAR(10),
@File IMAGE

as   
BEGIN   
  
 UPDATE MMS_Attachment set 
 MMS_FileName =@Fname,  
 MMS_FileExtension=@Fextension,  
 MMS_File=@File

 where  Meeting_Id =@MeetingId  
  
    
END


