IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_rate'))
begin
  PRINT 'Dropping...pc_upd_rate'
  DROP PROCEDURE pc_upd_rate
end
PRINT 'Creating...pc_upd_rate'
GO 
CREATE PROCEDURE pc_upd_rate
(
	@pkid			uniqueidentifier,
	@reference		varchar(150), 
	@rateType_ipp	varchar(50),
	@rateType_pp_sc	varchar(50),
	@rateType_rta	varchar(50),
	@rateType_sns	varchar(50),
	@rateType_ippx	varchar(50),
	@ipp			decimal(18,2),
	@pp_sc			decimal(18,2),
	@rta			decimal(18,2),
	@sns			decimal(18,2),
	@ippx			decimal(18,2),
	@from			varchar(50),
	@to				varchar(50),
	@userAccountId	uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[Rate]
			SET
				[reference]			= @reference,
				[rateType_ipp]		= @rateType_ipp,
				[rateType_pp_sc]	= @rateType_pp_sc,
				[rateType_rta]		= @rateType_rta,
				[rateType_sns]		= @rateType_sns,
				[rateType_ippx]		= @rateType_ippx,
				[ipp]				= @ipp,
				[pp_sc]				= @pp_sc,
				[rta]				= @rta,
				[sns]				= @sns,
				[ippx]				= @ippx,
				[from]				= @from,
				[to]				= @to,
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

PRINT 'Creating...pc_upd_rate...complete'
go

