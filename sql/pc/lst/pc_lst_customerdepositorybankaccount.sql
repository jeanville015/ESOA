IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customerdepositorybankaccount'))
BEGIN
  PRINT 'Dropping...pc_lst_customerdepositorybankaccount'
  DROP PROCEDURE pc_lst_customerdepositorybankaccount
END
PRINT 'Creating...pc_lst_customerdepositorybankaccount'
GO
				 
CREATE PROCEDURE pc_lst_customerdepositorybankaccount 
AS

	SELECT 
		[pkid],
		accountNo,
		bankName
	FROM [dbo].[CustomerDipositoryBankAccount]
	WHERE ([isDeleted] = 0)
		ORDER BY created
GO

PRINT 'Creating...pc_lst_customerdepositorybankaccount...complete'
GO
