


--exec [sp_getCompletionEffectivenessReport] '2017-3-1','2018-8-16',3,0,0,1
CREATE procedure [dbo].[sp_getCompletionEffectivenessReport]
@SDate DateTime, @EDate DateTime,@ChairPerson int,@Participant int,@Task_Id int,@ReviewFrequencyID int
 AS     

 IF @ReviewFrequencyID='1'
 BEGIN
 SET @ReviewFrequencyID='1' 
 END
 
 IF @ReviewFrequencyID='2'
 BEGIN
 SET @ReviewFrequencyID='7' 
 END

 IF @ReviewFrequencyID='3'
 BEGIN
 SET @ReviewFrequencyID='14' 
 END

 IF @ReviewFrequencyID='4'
 BEGIN
 SET @ReviewFrequencyID='30' 
 END

 IF @ReviewFrequencyID='5'
 BEGIN
 SET @ReviewFrequencyID='90' 
 END

 IF @ReviewFrequencyID='6'
 BEGIN
 SET @ReviewFrequencyID='180' 
 END

 IF @ReviewFrequencyID='7'
 BEGIN
 SET @ReviewFrequencyID='365' 
 END
 
 --IF @Participant = '0' BEGIN      
 -- SET @Participant='%'    
 --END        
    

	--IF @Participant !='0' BEGIN
	-- SET @Participant=@Participant
 --END 
    
 --IF @Priority = 'ALL' BEGIN      
 -- SET @Priority='%'    
 --END  
 
 BEGIN  
select

--(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighAssigned from MMS_Meeting_Details MeetingDetail 
--INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
--   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
--   AND MeetingDetail.Task = StatusUpdate.Task                 
--   and MeetingDetail.IsDeleted=0      
--   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
--where MeetingDetail.Priority = 'High'  
--AND MeetingDetail.chairperson = @ChairPerson 
--AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) 
--AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighAssigned 



(select DISTINCT ISNULL(count(Meeting_Id),0) as HighTotal from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId 
where MeetingDetail.Priority like 'H%'  AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Employee_Id= iif(@Participant<>0,@Participant, MeetingDetail.Employee_Id) AND MeetingDetail.Completion_Date between @SDate and @EDate) as HighTotal ,

(select ISNULL(COUNT(Meeting_Id),0) as MediumTotal from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId 
where MeetingDetail.Priority like'M%' AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Employee_Id= iif(@Participant<>0,@Participant, MeetingDetail.Employee_Id) AND MeetingDetail.Completion_Date between @SDate and @EDate) as MediumTotal, 

(select ISNULL(COUNT(Meeting_Id),0)  as LowTotal from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId where MeetingDetail.Priority like 'L%' 
AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Employee_Id= iif(@Participant<>0,@Participant, MeetingDetail.Employee_Id) AND MeetingDetail.Completion_Date between @SDate and @EDate) as LowTotal,

(select ISNULL(COUNT(Meeting_Id),0) as HighCompleted from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId
   where MeetingDetail.Priority like'H%' and MeetingDetail.Status like 'Com%' AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Employee_Id= iif(@Participant<>0,@Participant, MeetingDetail.Employee_Id) AND MeetingDetail.Completion_Date between @SDate and @EDate) as HighCompleted,

(select ISNULL(COUNT(Meeting_Id),0) as MediumCompleted from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId
where MeetingDetail.Priority like 'M%' and MeetingDetail.Status like 'Com%' AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Employee_Id= iif(@Participant<>0,@Participant, MeetingDetail.Employee_Id) AND MeetingDetail.Completion_Date between @SDate and @EDate) as MediumCompleted, 
   
(select ISNULL(COUNT(Meeting_Id),0) as LowCompleted from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId where MeetingDetail.Priority like 'L%' and MeetingDetail.Status like 'Com%' AND MeetingDetail.chairperson = @ChairPerson  AND MeetingDetail.Employee_Id= iif(@Participant<>0,@Participant, MeetingDetail.Employee_Id) AND MeetingDetail.Completion_Date between @SDate and @EDate) as LowCompleted,
  


  --select count(*) as HighDelay from MMS_Meeting_Details where NOT Exists(SELECT ISNULL(DateDiff(DAY, (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=4279 order by Meeting_Status_Date desc ) ,meetdetails.Completion_Date),0) as MediumDelay FROM MMS_Meeting_Details meetdetails JOIN MMS_Meeting_Status mmsstatus ON meetdetails.Task_Id = 4279 and mmsstatus.Task_Id=4279  WHERE   meetdetails.chairperson = 'charu' AND meetdetails.Participant LIKE 'abc' and meetdetails.Priority like 'High%' and  meetdetails.Completion_Date between '4/1/2017' and '4/26/2018 5:10:52 PM'and not exists(SELECT Comments FROM MMS_Meeting_Details WHERE Completion_Date between DATEADD(DAY,-10,GETDATE()) and '4/26/2018 5:10:52 PM' and Task_Id =4279)) and MMS_Meeting_Details.Task_Id = 4279
  

 
(select count(Task_Id) as HighDelay from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId 
 where NOT Exists(SELECT ISNULL(DateDiff(DAY, (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=@Task_Id order by Meeting_Status_Date desc ) ,meetdetails.Completion_Date),0) FROM MMS_Meeting_Details meetdetails JOIN MMS_Meeting_Status mmsstatus ON meetdetails.Task_Id = @Task_Id and mmsstatus.Task_Id=@Task_Id WHERE meetdetails.chairperson = @ChairPerson AND meetdetails.Employee_Id= iif(@Participant<>0,@Participant, meetdetails.Employee_Id) and meetdetails.Priority like 'High%' and  meetdetails.Completion_Date between @SDate and @EDate and not exists(SELECT Comments FROM MMS_Meeting_Details WHERE Completion_Date between DATEADD(DAY,-(@ReviewFrequencyID),GETDATE()) and (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=@Task_Id order by Meeting_Status_Date desc ) and Task_Id =@Task_Id)) and MeetingDetail.Task_Id = @Task_Id and Priority like 'High%') as HighDelay,

 
  (select count(Task_Id) as MediumDelay from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId 
  where NOT Exists(SELECT ISNULL(DateDiff(DAY, (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=@Task_Id order by Meeting_Status_Date desc ) ,meetdetails.Completion_Date),0) FROM MMS_Meeting_Details meetdetails JOIN MMS_Meeting_Status mmsstatus ON meetdetails.Task_Id = @Task_Id and mmsstatus.Task_Id=@Task_Id WHERE meetdetails.chairperson = @ChairPerson AND meetdetails.Employee_Id= iif(@Participant<>0,@Participant, meetdetails.Employee_Id) and meetdetails.Priority like 'Medium%' and  meetdetails.Completion_Date between @SDate and @EDate and not exists(SELECT Comments FROM MMS_Meeting_Details WHERE Completion_Date between DATEADD(DAY,-(@ReviewFrequencyID),GETDATE()) and (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=@Task_Id order by Meeting_Status_Date desc ) and Task_Id =@Task_Id)) and MeetingDetail.Task_Id = @Task_Id and Priority like 'Mediu%') as MediumDelay,

  

  (select count(Task_Id) as LowDelay from MMS_Meeting_Details MeetingDetail
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId   
   AND MeetingDetail.Task = StatusUpdate.Task            
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId 
   where NOT Exists(SELECT ISNULL(DateDiff(DAY, (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=@Task_Id order by Meeting_Status_Date desc ) ,meetdetails.Completion_Date),0) FROM MMS_Meeting_Details meetdetails JOIN MMS_Meeting_Status mmsstatus ON meetdetails.Task_Id = @Task_Id and mmsstatus.Task_Id=@Task_Id WHERE meetdetails.chairperson = @ChairPerson AND meetdetails.Employee_Id= iif(@Participant<>0,@Participant, meetdetails.Employee_Id) and meetdetails.Priority like 'Low%' and  meetdetails.Completion_Date between @SDate and @EDate and not exists(SELECT Comments FROM MMS_Meeting_Details WHERE Completion_Date between DATEADD(DAY,-(@ReviewFrequencyID),GETDATE()) and (select top 1 Meeting_Status_Date from MMS_Meeting_Status where Task_Id=@Task_Id order by Meeting_Status_Date desc ) and Task_Id =@Task_Id)) and MeetingDetail.Task_Id = @Task_Id and Priority like 'Low%') as LowDelay

END



