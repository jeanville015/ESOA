IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customeremailaddress'))
BEGIN
  PRINT 'Dropping...pc_lst_customeremailaddress'
  DROP PROCEDURE pc_lst_customeremailaddress
END
PRINT 'Creating...pc_lst_customeremailaddress'
GO
				 
CREATE PROCEDURE pc_lst_customeremailaddress
(
	@customerId	uniqueidentifier = null
)
AS

	SELECT 
		[pkid],
		[customerId],
		[emailAddress]
	FROM [dbo].[CustomerEmailAddress]
	WHERE [customerId]=@customerId 
		AND ([isDeleted] = 0)
GO

PRINT 'Creating...pc_lst_customeremailaddress...complete'
GO
