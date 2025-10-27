IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_useraccount'))
begin
  PRINT 'Dropping...pc_upd_useraccount'
  DROP PROCEDURE pc_upd_useraccount
end
PRINT 'Creating...pc_upd_useraccount'
GO 
CREATE PROCEDURE pc_upd_useraccount
(
	@pkid					uniqueidentifier,
	@name					varchar(150), 
	@jobTitle				varchar(150),
	@team					varchar(150),
	@role					varchar(150),
	@moduleAccess_admin		bit,
	@moduleAccess_soa		bit,
	@moduleAccess_payment	bit,
	@moduleAccess_reports	bit,
	@moduleAccess_granular	bit,
	@accessRights_admin		bit,
	@accessRights_soa		bit,
	@accessRights_payment	bit,
	@accessRights_reports	bit,
	@accessRights_granular	bit,
	@emailAddress			varchar(150),
	@contactNo				varchar(150),
	@userAccountId			uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[UserAccount]
			SET
				[name]					= @name,
				[jobTitle]				= @jobTitle,
				[team]					= @team,
				[role]					= @role,
				[moduleAccess_admin]	= @moduleAccess_admin,
				[moduleAccess_soa]      = @moduleAccess_soa,
				[moduleAccess_payment]	= @moduleAccess_payment,
				[moduleAccess_reports]	= @moduleAccess_reports,
				[moduleAccess_granular]	= @moduleAccess_granular,
				[accessRights_admin]	= @accessRights_admin,
				[accessRights_soa]		= @accessRights_soa,
				[accessRights_payment]	= @accessRights_payment,
				[accessRights_reports]	= @accessRights_reports,
				[accessRights_granular] = @accessRights_granular,
				[emailAddress]			= @emailAddress,
				[contactNo]				= @contactNo,
				[updated]				= GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'China Standard Time',
				[updatedBy]				= @userAccountId
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

PRINT 'Creating...pc_upd_useraccount...complete'
go

