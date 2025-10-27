IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customerdepositoryaccountno'))
BEGIN
  PRINT 'Dropping...pc_lst_customerdepositoryaccountno'
  DROP PROCEDURE pc_lst_customerdepositoryaccountno
END
PRINT 'Creating...pc_lst_customerdepositoryaccountno'
GO
				 
CREATE PROCEDURE pc_lst_customerdepositoryaccountno
(
	@customerId	uniqueidentifier = null
)
AS

	SELECT 
		[pkid],
		[customerId],
		[depositoryAccountNo]
	FROM [dbo].[CustomerDepositoryAccountNo]
	WHERE [customerId]=@customerId 
		AND ([isDeleted] = 0)
		ORDER BY created
GO

PRINT 'Creating...pc_lst_customerdepositoryaccountno...complete'
GO
