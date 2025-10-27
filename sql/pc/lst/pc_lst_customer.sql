IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customer'))
BEGIN
  PRINT 'Dropping...pc_lst_customer'
  DROP PROCEDURE pc_lst_customer
END
PRINT 'Creating...pc_lst_customer'
GO
				 
CREATE PROCEDURE pc_lst_customer
AS

	SELECT 
		ROW_NUMBER() OVER(ORDER BY [name]) AS [index],
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
		[status]
	FROM [dbo].[customer]
	WHERE [isDeleted] = 0
GO

PRINT 'Creating...pc_lst_customer...complete'
GO
