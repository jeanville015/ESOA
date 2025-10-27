IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_customercontactperson'))
begin
  PRINT 'Dropping...pc_ins_customercontactperson'
  DROP PROCEDURE pc_ins_customercontactperson
end
PRINT 'Creating...pc_ins_customercontactperson'
GO

create procedure pc_ins_customercontactperson
(
  @customerId			uniqueidentifier	= null	
 ,@contactPerson		varchar(max)		= null
 ,@designation			varchar(max)		= null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null	output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.CustomerContactPerson (
			[pkid]
			,[customerId]
			,[contactPerson]
			,[designation]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			 @pkid
			,@customerId
			,@contactPerson
			,@designation
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

PRINT 'Creating...pc_ins_customercontactperson...complete'
go
