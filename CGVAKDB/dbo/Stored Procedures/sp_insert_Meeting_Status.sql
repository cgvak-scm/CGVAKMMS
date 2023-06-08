
CREATE PROCEDURE [dbo].[sp_insert_Meeting_Status]
 @EmpId_In						[INT],  
 @Status_Task_In				[varchar](250),  
 @Status_ResponsiblePerson_In	int,  
 --@Status_Date_In				[varchar](50),  
 @Status_Date_In                [datetime],
 @Status_Comments_In			[varchar](MAX),  
-- @Meeting_Status_In				[varchar](50),
@Meeting_status_In INT,
 @Meeting_Status_Hours			[varchar](50),
 @Meeting_Id					[INT],  
 @Commented_By					[INT],
 @Commented_By_ID				[INT],
 @Task_Id						[INT]

 AS  
	BEGIN
		BEGIN TRANSACTION  
  		INSERT INTO [dbo].[MMS_Meeting_Status]  
   			(	[dbo].[MMS_Meeting_Status].[Meeting_Status_Employee_Id],  
   				[dbo].[MMS_Meeting_Status].[Meeting_Status_Task],  
   				[dbo].[MMS_Meeting_Status].[Meeting_Status_ResponsiblePerson],  
   				[dbo].[MMS_Meeting_Status].[Meeting_Status_Date],  
   				[dbo].[MMS_Meeting_Status].[Meeting_Status_Comments],  
   				[dbo].[MMS_Meeting_Status].[Meeting_Status],
   				[dbo].[MMS_Meeting_Status].[Meeting_Id],
				[dbo].[MMS_Meeting_Status].[Meeting_Status_Hours],
				[dbo].[MMS_Meeting_Status].[Commented_By],				
				[dbo].[MMS_Meeting_Status].[Commented_By_ID],
				[dbo].[MMS_Meeting_Status].[Task_Id]
			)
   				 
  		VALUES  
   			(	@EmpId_In,  
   				@Status_Task_In,  
   				@Status_ResponsiblePerson_In,  
   				--Convert(varchar(50),@Status_Date_In,109),  	
				Format(getdate(),'M/dd/yyyy h:mm:ss tt'),
				--@Status_Date_In,			
   				@Status_Comments_In,  
   				@Meeting_Status_In,
				@Meeting_Id,
				@Meeting_Status_Hours,
				@Commented_By,
				@Commented_By_ID,
				@Task_Id
   			)  
		Update  
			dbo.MMS_Meeting_Details
		Set  
			Status =@Meeting_Status_In   ,
			--below line has been added to check---
			 Comments=@Status_Comments_In
		where Task_Id=@Task_Id
		--Where    
		--	Meeting_Id=@Meeting_Id     
		--AND    
		--	 Employee_Id=@EmpId_In    
		--AND    
		--	Task = @Status_Task_In 
		--MMS_Track_ReviewTasks



		IF @@ERROR != 0 BEGIN --check @@ERROR variable after each DML statements.. 			
			ROLLBACK TRANSACTION --RollBack Transaction if Error..
			RETURN
		END
		ELSE BEGIN
			COMMIT TRANSACTION --finally, Commit the transaction if Success..
			RETURN
		END
 	END  
RETURN







