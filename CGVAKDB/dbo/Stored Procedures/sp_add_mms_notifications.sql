


CREATE procedure [dbo].[sp_add_mms_notifications]
(
@meeting_id int,
@chairperson int
)
as
 begin
   insert into mms_notifications(meeting_id,chairperson) values(@meeting_id,@chairperson)
 end




