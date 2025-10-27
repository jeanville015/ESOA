IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customer_excel'))
BEGIN
  PRINT 'Dropping...pc_lst_customer_excel'
  DROP PROCEDURE pc_lst_customer_excel
END
PRINT 'Creating...pc_lst_customer_excel'
GO
				 
CREATE PROCEDURE pc_lst_customer_excel
AS

	SELECT 
		c.[pkid],
		c.[name],
		c.[legalEntityName],
		c.[tin],
		c.[address],
		c.[salesExec_LBC],
		c.[approvedAFC],
		sf.[formatName] as [soaFormat],
		r.[reference] as [RateCard],
		c.[domestic_intl],
		c.[country],
		c.[transmissionMode],
		c.[officeCode],
		c.[area],
		c.[sapCustomerId],
		c.[sapVendorId_ipp],
		c.[sapVendorId_pp_sc],
		c.[sapVendorId_rta],
		c.[sapVendorId_sns],
		c.[sapVendorId_ippx],
		c.[paymentCurrency],
		c.[SFModeOfSettlement],
		c.[withholdingTax],
		c.[vatStatus],
		c.[status]
	FROM [dbo].[customer] c
	inner join [dbo].[SOAFormat] sf on c.soaFormatId = sf.pkid
	inner join [dbo].[Rate] r on c.rateCardId = r.pkid
	WHERE c.[isDeleted] = 0
GO

PRINT 'Creating...pc_lst_customer_excel...complete'
GO
