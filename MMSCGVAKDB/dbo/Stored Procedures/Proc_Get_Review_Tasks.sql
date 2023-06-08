

 --  exec [dbo].[Proc_Get_Review_Tasks] '3',0,'0','All'

CREATE procedure [dbo].[Proc_Get_Review_Tasks]
  @Chairperson int,
  @Category_id INT,
  @Participant int,
  @PriorityType varchar(20)
AS  

IF @PriorityType = 'ALL' BEGIN      
  SET @PriorityType='%'    
 END 

 IF @Participant = 0 BEGIN      
  SET @Participant=0    
 END

-- declare @check_status INT;
--set @check_status=(SELECT top 1 Review_Status FROM MMS_Track_ReviewTasks WHERE Review_Status=0 order by id desc)



  SELECT DISTINCT 
  MMD.Task_Id,
 MMD.Meeting_Id,     
  MMD.Employee_Id,
  CP.EmployeeDisplayName as Chairperson, 
  P.employeeDisplayName as Participant,
MMD.Task,
--(select top 1 MMD.Task from MMS_Meeting_Status where mms.Task_Id=MMD.Task_Id  Order by MeetingStatusAutoId desc) as Task,
  MMC.CategoryName as Category,
  Convert(varchar(11),MMD.Completion_Date,101) AS [Completion_Date],
  MMD.Priority,
MMD.Status,
  
  MMD.Comments,  
  0 Review_Status,
  MAX(MMR.NextReviewDate) AS Review_Date,
   MMM.Meeting_Date AS Assigned_Date,


  --convert(date,MMR.ActualReviewedDate ) AS Review_Date,

	-- -1 * convert(INT,datediff(dd,Getdate(),DATEADD(dd, ISNULL((DATEDIFF(DAY,MMD.NextReviewDate,GETDATE())/ISNULL(RF.Freq_In_Days, 1)),0) * ISNULL(RF.Freq_In_Days, 1), MMD.NextReviewDate))) as Overdue_days
	-1 * convert(INT,datediff(dd,Getdate(),MMM.Meeting_Date)) as Overdue_days 
	FROM MMS_Track_ReviewTasks MMR
	INNER JOIN MMS_Meeting_Details MMD on MMD.Task_Id = MMR.TaskId
	INNER JOIN MMS_Review_Frequency rf ON MMD.ReviewFrequencyID = RF.Freq_In_Days
INNER JOIN Employee_Master CP on CP.EmployeeICode=MMD.Participant
 INNER JOIN Employee_Master P on P.EmployeeICode=MMD.Participant
 INNER JOIN MMS_Meeting_Category MMC on MMC.ID= MMD.Category
  INNER JOIN MMS_Meeting_Master MMM on MMM.Meeting_Id=MMD.Meeting_Id
--left join MMS_Meeting_Status MMS on MMS.Task_Id=MMD.Task_Id
 
 WHERE 
 MMD.IsDeleted=0 
	--	and md.Status!='Closed'
		and MMD.Status!=1
    --CONVERT(date,MMR.ActualReviewedDate,108) = CONVERT(date, getdate()
	AND ((DATEADD(dd, ISNULL((DATEDIFF(DAY,MMD.nextreviewdate,GETDATE())/ISNULL(rf.freq_in_days, 1)),0) * ISNULL(rf.freq_in_days, 1), MMD.nextreviewdate)) between '2017-03-01' and GETDATE()+1
	AND MMD.ChairPerson = @Chairperson
	AND MMD.Category = IIF(@Category_id <> 0, @Category_id, MMD.Category)  
	AND MMD.Priority LIKE @PriorityType 
	AND MMD.Participant = IIF(@participant <> 0, @participant, MMD.participant)  )
	AND 
	(SELECT TOP 1 Review_Status from MMS_Track_ReviewTasks where TaskId=MMD.Task_Id  Order by ID desc) = 0
	AND MMM.Meeting_Date<>DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))
	--and 
	--(select Meeting_Status_Task from MMS_Meeting_Status where mms.Task_Id=MMD.Task_Id  Order by MeetingStatusAutoId desc) =mmd.Task

GROUP BY  MMD.Task_Id,
 MMD.Meeting_Id,     
  MMD.Employee_Id,
  CP.EmployeeDisplayName, 
  P.employeeDisplayName ,
MMD.Task,
--(select top 1 MMD.Task from MMS_Meeting_Status where mms.Task_Id=MMD.Task_Id  Order by MeetingStatusAutoId desc) as Task,
  MMC.CategoryName ,
  Convert(varchar(11),MMD.Completion_Date,101) ,
  MMD.Priority,
MMD.Status,
  MMD.Comments
  , MMD.NextReviewDate
  , rf.freq_in_days
  ,  MMM.Meeting_Date


