IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_customercontactperson_nvp'))
begin
  PRINT 'Dropping...pc_upd_customercontactperson_nvp'
  DROP PROCEDURE pc_upd_customercontactperson_nvp
end
PRINT 'Creating... pc_upd_customercontactperson_nvp'
GO

create procedure pc_upd_customercontactperson_nvp
(
   @pkid            uniqueidentifier
  ,@name            nvarchar(50)
  ,@value           nvarchar(2048)
  ,@userAccountId   uniqueidentifier
)
AS 

begin transaction

begin try
	if (@name = 'isDeleted')
    begin
      update [CustomerContactPerson] set [isDeleted] = @value where pkid = @pkid
    end 
end try
begin catch
  if (@@trancount > 0 ) rollback tran
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)
end catch
if (@@trancount > 0 ) commit transaction
go

PRINT 'Creating...pc_upd_customercontactperson_nvp...complete'
go