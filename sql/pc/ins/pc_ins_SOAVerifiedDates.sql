IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_SOAVerifiedDates'))
begin
  PRINT 'Dropping...pc_ins_SOAVerifiedDates'
  DROP PROCEDURE pc_ins_SOAVerifiedDates
end
PRINT 'Creating...pc_ins_SOAVerifiedDates'
GO

create procedure pc_ins_SOAVerifiedDates
(
  @officeCode			varchar(50)
 ,@transactionDate		varchar(50)
 ,@status				varchar(50)
 ,@remarks				varchar(150)
)
as 
begin transaction 
	begin try 
		INSERT INTO dbo.[SOAVerifiedDates] ( 
			 [officeCode]
			,[transactionDate]
			,[status]
			,[remarks] 
			)
		VALUES (
			@officeCode
			,@transactionDate
			,@status
			,@remarks
			)
	end try
begin catch
  if (@@trancount > 0 ) rollback tran
  declare @msg nvarchar(max) = error_message()
  declare @state int = error_state()
  raiserror(@msg, 11, @state) 
end catch
if (@@trancount > 0 ) commit transaction
go

PRINT 'Creating...pc_ins_SOAVerifiedDates...complete'
go