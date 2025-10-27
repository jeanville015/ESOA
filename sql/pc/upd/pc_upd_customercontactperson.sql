IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_customercontactperson'))
begin
  PRINT 'Dropping...pc_upd_customercontactperson'
  DROP PROCEDURE pc_upd_customercontactperson
end
PRINT 'Creating...pc_upd_customercontactperson'
GO 
CREATE PROCEDURE pc_upd_customercontactperson
(
	 @pkid					uniqueidentifier
	,@contactPerson			varchar(max)		= null
	,@designation			varchar(max)		= null
	,@userAccountId			uniqueidentifier	= null
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[CustomerContactPerson]
			SET
				[contactPerson]	= @contactPerson,
				[designation]	= @designation,
				[updated]		= GETDATE(),
				[updatedBy]		= @userAccountId
			WHERE
				[pkid] = @pkid
				    
			select 
				@pkid
  
		END TRY
		BEGIN CATCH
			IF (@@trancount > 0 ) ROLLBACK TRAN
			DECLARE @msg nvarchar(max) = ERROR_MESSAGE()
			RAISERROR(@msg, 11, 1)
		END CATCH
	if (@@trancount > 0 ) commit transaction
END
go

PRINT 'Creating...pc_upd_customercontactperson...complete'
go

