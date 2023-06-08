

 CREATE procedure [dbo].[sp_get_mms_notification]          
(          
--@chairperson varchar(40)          
@chairperson int
)          
as          
begin          
     select a.meeting_id ,a.meeting_objective,a.meeting_date from       
    mms_meeting_master a      
   inner  join  mms_meeting_details b  on a.meeting_id=b.meeting_id     
    inner join mms_meeting_status_update c on b.meeting_id=c.meetingid         
    where b.participant=@chairperson and b.is_read_notification='false'     
    group by a.meeting_id,a.meeting_objective,a.meeting_date    
    order by a.meeting_date desc  
           
end




