

CREATE procedure [dbo].[sp_getCompReport_2442018]
@SDate DateTime, @EDate DateTime,@ChairPerson int,@Participant int
 AS  
 --[dbo].[sp_getCompReport_2442018] '2017-3-1','2018-8-16',3,0
 BEGIN  
SELECT DISTINCT
(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighAssigned from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'High'  
AND MeetingDetail.chairperson = @ChairPerson 
AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) 
AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighAssigned ,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as MediumAssigned from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority ='Medium' AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as MediumAssigned, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as LowAssigned from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'Low' AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as LowAssigned, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighCompleted from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority ='High' and MeetingDetail.Status = 2 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighCompleted,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as MediumCompleted from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'Medium' and MeetingDetail.Status = 2 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as MediumCompleted, 
   
(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as LowCompleted from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority like 'Low' and MeetingDetail.Status = 2 AND MeetingDetail.chairperson = @ChairPerson  AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as LowCompleted, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighInProgress from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority ='High' and MeetingDetail.Status = 3 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighInProgress, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as MediumInProgress from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority ='Medium' and MeetingDetail.Status = 3 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as MediumInProgress, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as LowInProgress from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority ='Low' and MeetingDetail.Status = 3 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as LowInProgress, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighYetToBegin from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority like 'High' and MeetingDetail.Status = 5 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighYetToBegin, 

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as MediumYetToBegin from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority like'Medium' and MeetingDetail.Status = 5 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as MediumYetToBegin,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as LowYetToBegin from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority ='Low' and MeetingDetail.Status = 5 AND MeetingDetail.chairperson = @ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as LowYetToBegin,

   (SELECT ISNULL(AVG(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as HighAVERAGEDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority ='High' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as HighAVERAGEDays, 
   
   (SELECT ISNULL(AVG(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as MediumAVERAGEDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority = 'Medium' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as MediumAVERAGEDays,

 (SELECT ISNULL(AVG(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as LowAVERAGEDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority = 'Low' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as LowAVERAGEDays, 

 (SELECT ISNULL(MAX(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as HighMAXIMUMDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority = 'High' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as HighMAXIMUMDays, 
   
   (SELECT ISNULL(MAX(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as MediumMAXIMUMDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority = 'Medium' AND meetdetails.chairperson = @ChairPerson
AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as MediumMAXIMUMDays,

 (SELECT ISNULL(MAX(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as LowMAXIMUMDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority ='Low' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as LowMAXIMUMDays,

(SELECT ISNULL(MIN(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as HighMINIMUMDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId
 JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE meetdetails.Priority ='High' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as HighMINIMUMDays , 
   
   (SELECT ISNULL(MIN(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as MediumMINIMUMDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId
    JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE  meetdetails.Priority = 'Medium' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as MediumMINIMUMDays ,

 (SELECT ISNULL(MIN(DateDiff(DAY, mmsmaster.Meeting_Date,meetdetails.Completion_Date)),0) as LowMINIMUMDays FROM MMS_Meeting_Details meetdetails  
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   meetdetails.Employee_Id = StatusUpdate.EmployeeId                
   AND meetdetails.Task = StatusUpdate.Task                 
   and meetdetails.IsDeleted=0      
   AND meetdetails.Meeting_Id = StatusUpdate.MeetingId
 JOIN MMS_Meeting_Master mmsmaster ON meetdetails.Meeting_Id = mmsmaster.Meeting_Id WHERE meetdetails.Priority = 'Low' AND meetdetails.chairperson = @ChairPerson AND meetdetails.Participant= iif(@Participant<>0,@Participant, meetdetails.Participant) AND meetdetails.Completion_Date between @SDate and @EDate and meetdetails.IsDeleted=0) as LowMINIMUMDays ,

 --New

 (select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighClosed from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
 where MeetingDetail.Priority = 'High' and MeetingDetail.Status = 1 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighClosed,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as HighReOpened from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'High' and MeetingDetail.Status = 4 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as HighReOpened,

 (select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as MediumClosed from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
 where MeetingDetail.Priority = 'Medium' and MeetingDetail.Status = 1 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as MediumClosed,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as MediumReOpened from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'Medium' and MeetingDetail.Status = 4 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as MediumReOpened,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as LowClosed from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'Low' and MeetingDetail.Status = 1 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as LowClosed,

(select Distinct(ISNULL(count(StatusUpdate.MeetingID),0)) as LowReOpened from MMS_Meeting_Details MeetingDetail 
INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) StatusUpdate ON 
   MeetingDetail.Employee_Id = StatusUpdate.EmployeeId                
   AND MeetingDetail.Task = StatusUpdate.Task                 
   and MeetingDetail.IsDeleted=0      
   AND MeetingDetail.Meeting_Id = StatusUpdate.MeetingId  
where MeetingDetail.Priority = 'Low' and MeetingDetail.Status = 4 AND MeetingDetail.chairperson =@ChairPerson AND MeetingDetail.Participant= iif(@Participant<>0,@Participant, MeetingDetail.Participant) AND MeetingDetail.Completion_Date between @SDate and @EDate ) as LowReOpened

--  exec sp_getCompReport_2442018 @SDate='2017-03-01',@EDate='2018-03-1',@ChairPerson='Charu',@Participant='abc',@Priority='ALL'

END


--exec sp_getCompReport_2442018 '2017-3-1','2018-7-12','10013',0


