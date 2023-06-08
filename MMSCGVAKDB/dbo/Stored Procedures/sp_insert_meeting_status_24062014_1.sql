

CREATE PROCEDURE [dbo].[sp_insert_meeting_status_24062014_1]  
 @EmpId_In [int],  
 @Status_Task_In [varchar](250),  
 @Status_ResponsiblePerson_In [varchar](50),  
 @Status_Date_In [varchar](50),  
 @Status_Comments_In [varchar](250),  
 --@Meeting_Status_In [varchar](50),
 @Meeting_Status_In INT,
 @Meeting_Status_Hours	VARCHAR(50),
 @Meeting_Id INT,
 @Task_Id int
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
				Meeting_Status_Hours,[dbo].[MMS_Meeting_Status].Task_Id

			)
   				 
  		VALUES  
   			(	@EmpId_In,  
   				@Status_Task_In,  
   				@Status_ResponsiblePerson_In,  
   				@Status_Date_In,  
   				@Status_Comments_In,  
   				@Meeting_Status_In,
				@Meeting_Id,
				@Meeting_Status_Hours,@Task_Id
   			)  
		Update  
			dbo.MMS_Meeting_Details
		Set  
			Status =@Meeting_Status_In   
		Where    
			Meeting_Id=@Meeting_Id     
		AND    
			 Employee_Id=@EmpId_In    
		AND    
			Task = @Status_Task_In 
		AND
		Task_Id=@Task_Id
			

		IF @@ERROR != 0 --check @@ERROR variable after each DML statements..

			BEGIN

				ROLLBACK TRANSACTION --RollBack Transaction if Error..

				RETURN

			END

			ELSE

				BEGIN

				COMMIT TRANSACTION --finally, Commit the transaction if Success..

				RETURN

			END
 	END  



