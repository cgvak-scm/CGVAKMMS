



CREATE Procedure [dbo].[Sp_Update_EmployeeAttachments]  
@MeetingId INT,
@Fname VARCHAR(50),
@Fextension VARCHAR(10),
@File IMAGE,
@IsActive bit

as   
BEGIN   
  
 UPDATE MMS_Employee_Attachment set 
 MMS_FileName =@Fname,  
 /*MMS_FileExtension=@Fextension,*/
 MMS_File=@File,
 IsActive=@IsActive

 where  Id =@MeetingId AND MMS_FileExtension=@Fextension
  
    
END



