IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_customercontactno'))
begin
  PRINT 'Dropping...pc_upd_customercontactno'
  DROP PROCEDURE pc_upd_customercontactno
end
PRINT 'Creating...pc_upd_customercontactno'
GO 
CREATE PROCEDURE pc_upd_customercontactno
(
	 @pkid					uniqueidentifier
	,@contactNo				varchar(max)		= null
	,@userAccountId			uniqueidentifier	= null
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[CustomerContactNo]
			SET
				[contactNo]		= @contactNo,
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

PRINT 'Creating...pc_upd_customercontactno...complete'
go

