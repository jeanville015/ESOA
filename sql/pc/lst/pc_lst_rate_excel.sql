IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_rate_excel'))
BEGIN
  PRINT 'Dropping...pc_lst_rate_excel'
  DROP PROCEDURE pc_lst_rate_excel
END
PRINT 'Creating...pc_lst_rate_excel'
GO
				 
CREATE PROCEDURE pc_lst_rate_excel
AS

	SELECT 
		[pkid],
		[reference],
		[ipp],
		[rateType_ipp],
		[pp_sc],
		[rateType_pp_sc],
		[rta],
		[rateType_rta],
		[sns],
		[rateType_sns],
		[ippx],
		[rateType_ippx],
		[from],
		[to]
	FROM [dbo].[Rate]
	WHERE ([isDeleted] = 0)
GO

PRINT 'Creating...pc_lst_rate_excel...complete'
GO
