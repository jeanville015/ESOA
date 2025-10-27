IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_upd_customer'))
begin
  PRINT 'Dropping...pc_upd_customer'
  DROP PROCEDURE pc_upd_customer
end
PRINT 'Creating...pc_upd_customer'
GO 
CREATE PROCEDURE pc_upd_customer
(
	 @pkid					uniqueidentifier
	,@name					varchar(150)		= null
	,@legalEntityName		varchar(max)		= null
	,@tin					varchar(max)		= null
	,@address				varchar(max)		= null
	,@salesExec_LBC			varchar(max)		= null
	,@approvedAFC			decimal				= null
	,@soaFormatId			uniqueidentifier	= null
	,@rateCardId			uniqueidentifier	= null
	,@domestic_intl			varchar(50)			= null
	,@country				varchar(max)		= null
	,@transmissionMode		varchar(50)			= null
	,@officeCode			varchar(max)		= null
	,@area					varchar(max)		= null
	,@sapCustomerId			BIGINT				= null
	,@sapVendorId_ipp		BIGINT				= null
	,@sapVendorId_pp_sc		BIGINT				= null
	,@sapVendorId_rta		BIGINT				= null
	,@sapVendorId_sns		BIGINT				= null
	,@sapVendorId_ippx		BIGINT				= null
	,@paymentCurrency		varchar(50)			= null
	,@SFModeOfSettlement	varchar(150)		= null
	,@withholdingTax		varchar(50)			= null
	,@vatStatus				varchar(100)		= null
	,@status				varchar(100)		= null
	,@depositoryBankAccount uniqueidentifier	= null
	,@beginningBalance		decimal(18,2)		= null
	,@userAccountId			uniqueidentifier	= null
)
AS
BEGIN
    SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY 
				    					
			UPDATE  [dbo].[Customer]
			SET
				[name]				= @name,
				[legalEntityName]	= @legalEntityName,
				[tin]				= @tin,
				[address]			= @address,
				[salesExec_LBC]		= @salesExec_LBC,
				[approvedAFC]		= @approvedAFC,
				[soaFormatId]		= @soaFormatId,
				[rateCardId]		= @rateCardId,
				[domestic_intl]		= @domestic_intl,
				[country]			= @country,
				[TransmissionMode]	= @transmissionMode,
				[officeCode]		= @officeCode,
				[area]				= @area,
				[sapCustomerId]		= @sapCustomerId,
				[sapVendorId_ipp]	= @sapVendorId_ipp,
				[sapVendorId_pp_sc]	= @sapVendorId_pp_sc,
				[sapVendorId_rta]	= @sapVendorId_rta,
				[sapVendorId_sns]	= @sapVendorId_sns,
				[sapVendorId_ippx]	= @sapVendorId_ippx,
				[paymentCurrency]	= @paymentCurrency,
				[SFModeOfSettlement]= @SFModeOfSettlement,
				[withholdingTax]	= @withholdingTax,
				[vatStatus]			= @vatStatus,
				[status]			= @status,
				[depositoryBankAccount] = @depositoryBankAccount,
				[beginningBalance]	= @beginningBalance,
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

PRINT 'Creating...pc_upd_customer...complete'
go

