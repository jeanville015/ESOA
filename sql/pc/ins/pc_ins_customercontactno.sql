IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_customercontactno'))
begin
  PRINT 'Dropping...pc_ins_customercontactno'
  DROP PROCEDURE pc_ins_customercontactno
end
PRINT 'Creating...pc_ins_customercontactno'
GO

create procedure pc_ins_customercontactno
(
  @customerId			uniqueidentifier	= null	
 ,@contactNo			varchar(max)		= null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null	output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.CustomerContactNo (
			[pkid]
			,[customerId]
			,[contactNo]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			 @pkid
			,@customerId
			,@contactNo
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

PRINT 'Creating...pc_ins_customercontactno...complete'
go
