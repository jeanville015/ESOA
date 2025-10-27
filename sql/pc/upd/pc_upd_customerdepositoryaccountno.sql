IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_customerdepositoryaccountno'))
begin
  PRINT 'Dropping...pc_upd_customerdepositoryaccountno'
  DROP PROCEDURE pc_upd_customerdepositoryaccountno
end
PRINT 'Creating...pc_upd_customerdepositoryaccountno'
GO 
CREATE PROCEDURE pc_upd_customerdepositoryaccountno
(
	 @pkid					uniqueidentifier
	,@depositoryAccountNo	varchar(150)		= null
	,@userAccountId			uniqueidentifier	= null
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[CustomerDepositoryAccountNo]
			SET
				[depositoryAccountNo] = @depositoryAccountNo,
				[updated]			  = GETDATE(),
				[updatedBy]			  = @userAccountId
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

PRINT 'Creating...pc_upd_customerdepositoryaccountno...complete'
go

