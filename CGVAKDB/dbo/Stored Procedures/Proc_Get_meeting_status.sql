

CREATE Procedure [dbo].[Proc_Get_meeting_status]    
 @Employee_id int,   
 @Meeting_Id int  
as  
select * from (
select Meeting_Status_Comments as Comments,Commented_by,Meeting_Status_Date as StatusDate from MMS_Meeting_Status   
Where Meeting_Status_Employee_Id=@Employee_id and Meeting_id = @Meeting_Id
union 
select Comments[Comments], meetdet.Chairperson, null[StatusDate] from mms_meeting_details meetdet
inner join MMS_Employee_Master empmaster on meetdet.employee_id=empmaster.employee_id 
where meetdet.employee_id=@Employee_id and meetdet.meeting_id=@Meeting_Id
) meetingstatus order by [StatusDate] desc




