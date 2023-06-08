



-- exec [dbo].[sp_get_MeetingSearch_Summary_01102019] '3',10087,'0','2017-3-1','2018-11-30','all','0'

CREATE PROCEDURE [dbo].[sp_get_MeetingSearch_Summary_01102019]
  @chairperson INT
, @participant INT
, @status      INT
, @sdate       DATETIME
, @edate       DATETIME
, @priority    VARCHAR(10)
, @template    INT

AS

--IF @priority = 'ALL'
--    BEGIN
--        SET @priority = '%'
--    END

IF @participant = 0
    BEGIN
        SET @participant = 0
    END

    BEGIN
        SELECT 
               SUM(total)                  total
             , SUM(assigned)               assigned
             , SUM(inprogress)             inprogress
             , SUM(completed)              completed
             , SUM(closed)                 closed
             , SUM(reopened)               reopened
             , SUM(high)                   high
             , SUM(medium)                 medium
             , SUM(low)                    low
             , Convert(varchar(20),ISNULL(temp.participant, 0)) participant
			  --(select Distinct Convert(varchar(20),temp.Participant))as Participant,

             , isnull(temp.employee_id, 0) employee_id
             , isnull(temp.chairperson, 0) chairperson


			
			--   ,isnull(temp.review_Status,0) review_status
		



          FROM
                        (
                          SELECT DISTINCT
                                 ISNULL(COUNT(statusupdate.meetingid), 0) total
                               , 0                                        assigned
                               , 0                                        inprogress
                               , 0                                        completed
                               , 0                                        closed
                               , 0                                        reopened
                               , 0                                        high
                               , 0                                        medium
                               , 0                                        low
                               , ISNULL(meetingdetail.participant, 0)     participant
                               , employee_id
                               , meetingdetail.chairperson


							--   ,reviewTrack.review_Status

                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	  -- and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								--     Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            WHERE meetingdetail.chairperson = @chairperson
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.isdeleted = 0
                                
                                AND meetingdetail.Status = IIF(@status <> 0, @status, meetingdetail.status)     
								 AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)     
                               --   AND meetingdetail.priority LIKE @priority		
							    AND meetingdetail.priority = IIF(@priority <> 'ALL', @priority, meetingdetail.priority)						
                                  --AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								  	AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate  
									--and  meetingdetail.Meeting_Id>116107   
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson
								  
								 --  ,reviewTrack.review_Status

                          UNION ALL
                          SELECT DISTINCT
                                 0                                        total
                               , ISNULL(COUNT(statusupdate.meetingid), 0) assigned
                               , 0                                        inprogress
                               , 0                                        completed
                               , 0                                        closed
                               , 0                                        reopened
                               , 0                                        high
                               , 0                                        medium
                               , 0                                        low
                               , ISNULL(meetingdetail.participant, 0)     participant
                               , employee_id
                               , meetingdetail.chairperson

							  -- ,reviewTrack.review_Status


                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	   --and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								  --   Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            WHERE meetingdetail.chairperson = @chairperson
--AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.status = IIF(@status=5 or @status='0',5 ,-1) --5
                                 -- AND meetingdetail.status LIKE @status
                                  AND meetingdetail.priority = IIF(@priority <> 'ALL', @priority, meetingdetail.priority)                                    
								 AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)     
                                  AND meetingdetail.isdeleted = 0
                                 --AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								 	AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson
								   
								--   ,reviewTrack.review_Status



                          UNION ALL
                          SELECT DISTINCT
                                 0                                        total
                               , 0                                        assigned
                               , ISNULL(COUNT(statusupdate.meetingid), 0) inprogress
                               , 0                                        completed
                               , 0                                        closed
                               , 0                                        reopened
                               , 0                                        high
                               , 0                                        medium
                               , 0                                        low
                               , isnull(meetingdetail.participant, 0)     participant
                               , employee_id
                               , meetingdetail.chairperson
							   
							 --  ,reviewTrack.review_Status



                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	   --and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update) statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

							--	     Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            WHERE meetingdetail.chairperson = @chairperson
-- AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.status = IIF(@status=3 or @status='0',3 ,-1)  --3
                                   
								 AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)         
                                 -- AND meetingdetail.priority LIKE @priority
								 AND meetingdetail.priority = IIF(@priority <> 'ALL', @priority, meetingdetail.priority)
                                  AND meetingdetail.isdeleted = 0
                                  --AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								  	AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson
							

--								   ,reviewTrack.review_Status



                          UNION ALL
                          SELECT DISTINCT
                                 0                                        total
                               , 0                                        assigned
                               , 0                                        inprogress
                               , ISNULL(COUNT(statusupdate.meetingid), 0) completed
                               , 0                                        closed
                               , 0                                        reopened
                               , 0                                        high
                               , 0                                        medium
                               , 0                                        low
                               , isnull(meetingdetail.participant, 0)     participant
                               , employee_id
                               , meetingdetail.chairperson


							 --  ,reviewTrack.Review_Status


                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	   --and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								--     Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            WHERE meetingdetail.chairperson = @chairperson
--AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.status = IIF(@status=2 or @status='0',2 ,-1) -- 2
                                
                                --  AND meetingdetail.priority LIKE @priority
								 AND meetingdetail.priority = IIF(@priority <> 'ALL', @priority, meetingdetail.priority)                                                  
								 AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)     
                                  AND meetingdetail.isdeleted = 0
                                  --AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								  AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson



								 --  ,reviewTrack.review_Status




                          UNION ALL
                          SELECT DISTINCT
                                 0                                        total
                               , 0                                        assigned
                               , 0                                        inprogress
                               , 0                                        completed
                               , ISNULL(COUNT(statusupdate.meetingid), 0) closed
                               , 0                                        reopened
                               , 0                                        high
                               , 0                                        medium
                               , 0                                        low
                               , isnull(meetingdetail.participant, 0)     participant
                               , employee_id
                               , meetingdetail.chairperson


							--   ,reviewTrack.review_Status



                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	  -- and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                    AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								 --   Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            WHERE meetingdetail.chairperson = @chairperson
--AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.status = IIF(@status=1 or @status='0',1 ,-1)  -- 1
                                                                                              
								  AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)        
                                 -- AND meetingdetail.priority LIKE @priority	
								  AND meetingdetail.priority = IIF(@priority <> 'ALL', @priority, meetingdetail.priority)							  
                                 -- AND meetingdetail.completion_date BETWEEN @sdate AND @edate
                                 AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
									AND meetingdetail.isdeleted = 0
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson

								 --  ,reviewTrack.Review_Status


                          UNION ALL
                          SELECT DISTINCT
                                 0                                        total
                               , 0                                        assigned
                               , 0                                        inprogress
                               , 0                                        completed
                               , 0                                        closed
                               , ISNULL(COUNT(statusupdate.meetingid), 0) reopened
                               , 0                                        high
                               , 0                                        medium
                               , 0                                        low
                               , isnull(meetingdetail.participant, 0)     participant
                               , employee_id
                               , meetingdetail.chairperson



						 	 --  ,reviewTrack.review_Status

                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	  -- and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								   --  Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            WHERE meetingdetail.chairperson = @chairperson
-- AND MeetingDetail.Participant= @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.status = IIF(@status=4 or @status='0',4 ,-1)  --4
                                                                                    
								  AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)
                                  --AND meetingdetail.priority LIKE @priority
								   AND meetingdetail.priority = IIF(@priority <> 'ALL', @priority, meetingdetail.priority)
								 
                                 -- AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								 AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
                                  AND meetingdetail.isdeleted = 0
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson


								 --  ,reviewTrack.review_Status


                          UNION ALL
                          SELECT DISTINCT
                                 0                                    total
                               , 0                                    assigned
                               , 0                                    inprogress
                               , 0                                    completed
                               , 0                                    closed
                               , 0                                    reopened
                               , COUNT(priority)                      high
                               , 0                                    medium
                               , 0                                    low
                               , isnull(meetingdetail.participant, 0) participant
                               , employee_id
                               , meetingdetail.chairperson


							  -- ,reviewTrack.review_Status


                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	   --and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								  --   Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId
								  --  AND meetingdetail.status = IIF(@status=4 or @status='0',4 ,-1)  --4


                            --WHERE meetingdetail.priority = 'High'
							Where --meetingdetail.Priority=IIF(@priority='High','High','a')
							meetingdetail.priority = IIF(@priority = 'ALL' or @priority = 'High', 'High', 'a')
                                 AND meetingdetail.Status = IIF(@status <> 0, @status, meetingdetail.status)                                                            
								  AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)
                                  AND meetingdetail.chairperson = @chairperson
                                 -- AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								  AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
--AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.isdeleted = 0
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson
								   
								   								   
								   --,reviewTrack.review_Status



                          UNION ALL
                          SELECT DISTINCT
                                 0                                    total
                               , 0                                    assigned
                               , 0                                    inprogress
                               , 0                                    completed
                               , 0                                    closed
                               , 0                                    reopened
                               , 0                                    high
                               , COUNT(priority)                      medium
                               , 0                                    low
                               , isnull(meetingdetail.participant, 0) participant
                               , meetingdetail.employee_id
                               , meetingdetail.chairperson

						     --  ,reviewTrack.Review_Status


                            FROM   dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	   --and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson


                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								--     Full JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                           -- WHERE meetingdetail.priority = 'Medium'

						   Where -- meetingdetail.Priority=IIF(@priority='Medium','Medium','a')
                                 meetingdetail.priority = IIF(@priority = 'ALL' or @priority = 'Medium', 'Medium', 'a')
                                 AND meetingdetail.Status = IIF(@status <> 0, @status, meetingdetail.status)                                                            
								  AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)
                                  AND meetingdetail.chairperson = @chairperson
								  
                                  --AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								  AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
--AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.isdeleted = 0
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson
								   
								  -- ,reviewTrack.Review_Status
								   

                          UNION ALL
                          SELECT DISTINCT
                                 0                                    total
                               , 0                                    assigned
                               , 0                                    inprogress
                               , 0                                    completed
                               , 0                                    closed
                               , 0                                    reopened
                               , 0                                    high
                               , 0                                    medium
                               , COUNT(priority)                      low
                               , ISNULL(meetingdetail.participant, 0) participant
                               , meetingdetail.employee_id
                               , meetingdetail.chairperson
							   
							 --  ,reviewTrack.Review_Status
							   							 
                            FROM  dbo.mms_meeting_details meetingdetail

							join MMS_Meeting_Master on MMS_Meeting_Master.Meeting_Id=meetingdetail.Meeting_Id
	   --and MMS_Meeting_Master.Meeting_Chairperson=meetingdetail.Chairperson

	--   full join MMS_Review_Frequency on   meetingdetail.ReviewFrequencyID=MMS_Review_Frequency.Freq_Id



                            INNER JOIN (SELECT DISTINCT MeetingId, EmployeeId, Task from MMS_Meeting_Status_Update)  statusupdate
                                ON meetingdetail.employee_id = statusupdate.employeeid
                                   AND meetingdetail.task = statusupdate.task
                                   AND meetingdetail.meeting_id = statusupdate.meetingid

								--   LEFT JOIN MMS_Track_ReviewTasks reviewTrack ON meetingdetail.Task_Id=reviewTrack.TaskId

                            --WHERE meetingdetail.priority = 'Low'

							 Where --meetingdetail.Priority=IIF(@priority='Low','Low','a')
							  meetingdetail.priority = IIF(@priority = 'ALL' or @priority = 'Low', 'Low', 'a') 
							AND meetingdetail.chairperson = @chairperson
                                   AND meetingdetail.Status = IIF(@status <> 0, @status, meetingdetail.status)                                                            
								  AND meetingdetail.TemplateID = IIF(@template <> 0, @template, meetingdetail.TemplateID)
                                 -- AND meetingdetail.completion_date BETWEEN @sdate AND @edate
								  AND MMS_Meeting_Master.Meeting_Date Between @sdate AND @edate     
-- AND MeetingDetail.Participant = @Participant
                                  AND meetingdetail.participant = IIF(@participant <> 0, @participant, meetingdetail.participant)
                                  AND meetingdetail.isdeleted = 0
                            GROUP BY
                                     meetingdetail.participant
                                   , meetingdetail.employee_id
                                   , meetingdetail.chairperson
								   

								--  ,reviewTrack.Review_Status



								


                        ) temp
          GROUP BY
                   temp.participant
                 , temp.employee_id
                 , temp.chairperson
  

  --,temp.review_Status



               ORDER BY
                        temp.participant DESC

    END

-- exec [dbo].[sp_get_MeetingSearch_Summary_16072018] '10013','0','Closed','2017-3-1','2018-7-13','Low'
-- exec [dbo].[sp_get_MeetingSearch_Summary_13072018] '10009','0','All','2017-3-1','2018-7-31','All'
--select * from Employee_Master where EmployeeICode=10010





