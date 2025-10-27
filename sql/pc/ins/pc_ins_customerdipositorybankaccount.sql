IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_customerdipositorybankaccount'))
begin
  PRINT 'Dropping...pc_ins_customerdipositorybankaccount'
  DROP PROCEDURE pc_ins_customerdipositorybankaccount
end
PRINT 'Creating...pc_ins_customerdipositorybankaccount'
GO

create procedure pc_ins_customerdipositorybankaccount
(
  --@customerId			uniqueidentifier	= null	
  @accountNo			varchar(250)		= null
 ,@bankName				varchar(150)	    = null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null	output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.CustomerDipositoryBankAccount (
			[pkid]
			,[accountNo]
			,[bankName]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			 @pkid
			,@accountNo
			,@bankName
			,getutcdate()
			,getutcdate()
			,@userAccountId
			,0
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

PRINT 'Creating...pc_ins_customerdipositorybankaccount...complete'
go
