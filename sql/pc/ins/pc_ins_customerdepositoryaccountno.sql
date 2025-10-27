IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_customerdepositoryaccountno'))
begin
  PRINT 'Dropping...pc_ins_customerdepositoryaccountno'
  DROP PROCEDURE pc_ins_customerdepositoryaccountno
end
PRINT 'Creating...pc_ins_customerdepositoryaccountno'
GO

create procedure pc_ins_customerdepositoryaccountno
(
  @customerId			uniqueidentifier	= null	
 ,@depositoryAccountNo  varchar(150)		= null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null	output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.CustomerDepositoryAccountNo(
			[pkid]
			,[customerId]
			,[depositoryAccountNo]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			 @pkid
			,@customerId
			,@depositoryAccountNo
			,getdate()
			,getdate()
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

PRINT 'Creating...pc_ins_customerdepositoryaccountno...complete'
go
