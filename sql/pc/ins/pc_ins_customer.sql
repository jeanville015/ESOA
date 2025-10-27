IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_customer'))
begin
  PRINT 'Dropping...pc_ins_customer'
  DROP PROCEDURE pc_ins_customer
end
PRINT 'Creating...pc_ins_customer'
GO

create procedure pc_ins_customer
(
  @name					varchar(150)		= null
 ,@legalEntityName		varchar(max)		= null
 ,@tin					varchar(max)		= null
 ,@address				varchar(max)		= null
 ,@salesExec_LBC			varchar(max)		= null
 ,@approvedAFC			decimal				= null
 ,@soaFormatId			uniqueidentifier	= null
 ,@rateCardId			uniqueidentifier	= null
 ,@domestic_intl		varchar(50)			= null
 ,@country				varchar(max)		= null
 ,@transmissionMode		varchar(50)			= null
 ,@officeCode			varchar(max)		= null
 ,@area					varchar(max)		= null
 ,@sapCustomerId		BIGINT					= null
 ,@sapVendorId_ipp		BIGINT					= null
 ,@sapVendorId_pp_sc	BIGINT					= null
 ,@sapVendorId_rta		BIGINT					= null
 ,@sapVendorId_sns		BIGINT					= null
 ,@sapVendorId_ippx		BIGINT					= null
 ,@paymentCurrency		varchar(50)			= null
 ,@SFModeOfSettlement	varchar(150)		= null
 ,@withholdingTax		varchar(50)			= null
 ,@vatStatus			varchar(100)		= null
 ,@status				varchar(100)		= null
 ,@depositoryBankAccount uniqueidentifier		= null
 ,@beginningBalance		decimal(18,2)		= null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.Customer(
			[pkid]
			,[name]
			,[legalEntityName]
			,[tin]
			,[address]
			,[salesExec_LBC]
			,[approvedAFC]
			,[soaFormatId]
			,[rateCardId]
			,[domestic_intl]
			,[country]
			,[TransmissionMode]
			,[officeCode]
			,[area]
			,[sapCustomerId]
			,[sapVendorId_ipp]
			,[sapVendorId_pp_sc]
			,[sapVendorId_rta]
			,[sapVendorId_sns]
			,[sapVendorId_ippx]
			,[paymentCurrency]
			,[SFModeOfSettlement]
			,[withholdingTax]
			,[vatStatus]
			,[status]
			,[depositoryBankAccount]
			,[beginningBalance]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			@pkid
			,@name
			,@legalEntityName
			,@tin
			,@address
			,@salesExec_LBC
			,@approvedAFC
			,@soaFormatId
			,@rateCardId
			,@domestic_intl
			,@country
			,@transmissionMode
			,@officeCode
			,@area
			,@sapCustomerId
			,@sapVendorId_ipp
			,@sapVendorId_pp_sc
			,@sapVendorId_rta
			,@sapVendorId_sns
			,@sapVendorId_ippx
			,@paymentCurrency
			,@SFModeOfSettlement
			,@withholdingTax
			,@vatStatus
			,@status
			,@depositoryBankAccount
			,@beginningBalance
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

PRINT 'Creating...pc_ins_customer...complete'
go
