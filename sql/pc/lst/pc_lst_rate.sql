IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_rate'))
BEGIN
  PRINT 'Dropping...pc_lst_rate'
  DROP PROCEDURE pc_lst_rate
END
PRINT 'Creating...pc_lst_rate'
GO
				 
CREATE PROCEDURE pc_lst_rate
AS

	SELECT 
		[pkid],
		[reference],
		[rateType_ipp],
		[rateType_pp_sc],
		[rateType_rta],
		[rateType_sns],
		[rateType_ippx],
		[ipp],
		[pp_sc],
		[rta],
		[sns],
		[ippx],
		[from],
		[to]
	FROM [dbo].[Rate]
	WHERE ([isDeleted] = 0)
	ORDER BY [reference]
GO

PRINT 'Creating...pc_lst_rate...complete'
GO
