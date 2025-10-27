IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_useraccount_nvp'))
begin
  PRINT 'Dropping...pc_upd_useraccount_nvp'
  DROP PROCEDURE pc_upd_useraccount_nvp
end
PRINT 'Creating... pc_upd_useraccount_nvp'
GO

create procedure pc_upd_useraccount_nvp
(
   @pkid            uniqueidentifier
  ,@name            nvarchar(50)
  ,@value           nvarchar(2048)
  ,@userAccountId   uniqueidentifier
)
AS 

begin transaction

begin try

    --if (@name = 'isFirstTimeLogin')
    --begin
    --  update UserAccount set isFirstTimeLogin = @value, updated = getdate() where pkid = @pkid
    --end    
    if (@name = 'password')
    begin
	  SET @value = (Select CONVERT(VARCHAR(50), HashBytes('MD5', @value), 2) as MD5Hash)
      update UserAccount set [password] = @value, updated = getdate() where pkid = @pkid
    end
	if (@name = 'isDeleted')
    begin
      update UserAccount set [isDeleted] = @value where pkid = @pkid
    end 
end try
begin catch
  if (@@trancount > 0 ) rollback tran
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)
end catch
if (@@trancount > 0 ) commit transaction
go

PRINT 'Creating...pc_upd_useraccount_nvp...complete'
go