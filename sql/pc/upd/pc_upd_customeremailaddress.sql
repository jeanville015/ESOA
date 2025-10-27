IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_customeremailaddress'))
begin
  PRINT 'Dropping...pc_upd_customeremailaddress'
  DROP PROCEDURE pc_upd_customeremailaddress
end
PRINT 'Creating...pc_upd_customeremailaddress'
GO 
CREATE PROCEDURE pc_upd_customeremailaddress
(
	 @pkid					uniqueidentifier
	,@emailAddress			varchar(max)		= null
	,@userAccountId			uniqueidentifier	= null
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[CustomerEmailAddress]
			SET
				[emailAddress]		= @emailAddress,
				[updated]			= GETDATE(),
				[updatedBy]			= @userAccountId
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

PRINT 'Creating...pc_upd_customeremailaddress...complete'
go

