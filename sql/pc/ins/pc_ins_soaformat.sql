IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_soaformat'))
begin
  PRINT 'Dropping...pc_ins_soaformat'
  DROP PROCEDURE pc_ins_soaformat
end
PRINT 'Creating...pc_ins_soaformat'
GO

create procedure pc_ins_soaformat
(
  @formatName			varchar(150)	=null
 ,@userAccountId		uniqueidentifier=null
 ,@pkid					uniqueidentifier  = null        output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.SOAFormat (
			[pkid]
			,[formatName]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			@pkid
			,@formatName
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

PRINT 'Creating...pc_ins_soaformat...complete'
go
