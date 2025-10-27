IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_customer'))
begin
  PRINT 'Dropping...pc_get_customer'
  DROP PROCEDURE pc_get_customer
end
PRINT 'Creating...pc_get_customer'
GO 

CREATE procedure pc_get_customer
(
  @pkid uniqueidentifier=null,
  @SAPIds varchar(150)=null,
  @CustomerName varchar(150)=null
)
as

begin try
	select 
		[pkid],
		[name],
		[legalEntityName],
		[tin],
		[address],
		[salesExec_LBC],
		[approvedAFC],
		[soaFormatId],
		[rateCardId],
		[domestic_intl],
		[country],
		[transmissionMode],
		[officeCode],
		[area],
		[sapCustomerId],
		[sapVendorId_ipp],
		[sapVendorId_pp_sc],
		[sapVendorId_rta],
		[sapVendorId_sns],
		[sapVendorId_ippx],
		[paymentCurrency],
		[SFModeOfSettlement],
		[withholdingTax],
		[vatStatus],
		[status],
		[depositoryBankAccount],
		[beginningBalance],
		[created],
		[updated],
		[updatedBy]
	from [dbo].[Customer] 
	where 
		(
			(
				[pkid] = @pkid
				OR
				[sapCustomerId] = @SAPIds
				OR
				[sapVendorId_ipp] = @SAPIds
				OR
				[sapVendorId_pp_sc] = @SAPIds
				OR
				[sapVendorId_rta] = @SAPIds
				OR
				[sapVendorId_sns] = @SAPIds
				OR
				[sapVendorId_ippx] = @SAPIds
				OR
				[name] = @CustomerName
			)
			AND
			(
				[isDeleted] = 0
			)
		);
end try

begin catch
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)

end catch
go

PRINT 'Creating...pc_get_customer...complete'
go