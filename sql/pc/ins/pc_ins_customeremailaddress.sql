IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_customeremailaddress'))
begin
  PRINT 'Dropping...pc_ins_customeremailaddress'
  DROP PROCEDURE pc_ins_customeremailaddress
end
PRINT 'Creating...pc_ins_customeremailaddress'
GO

create procedure pc_ins_customeremailaddress
(
  @customerId			uniqueidentifier	= null	
 ,@emailAddress			varchar(max)		= null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null	output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.CustomerEmailAddress (
			[pkid]
			,[customerId]
			,[emailAddress]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			 @pkid
			,@customerId
			,@emailAddress
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

PRINT 'Creating...pc_ins_customeremailaddress...complete'
go
