

 CREATE procedure [dbo].[sp_update_read_notification]
 (@id int)
 as
 begin
   update mms_meeting_details set is_read_notification='true' where meeting_id=@id 
 end



