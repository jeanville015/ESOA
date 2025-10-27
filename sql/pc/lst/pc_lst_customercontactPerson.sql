IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customercontactPerson'))
BEGIN
  PRINT 'Dropping...pc_lst_customercontactPerson'
  DROP PROCEDURE pc_lst_customercontactPerson
END
PRINT 'Creating...pc_lst_customercontactPerson'
GO
				 
CREATE PROCEDURE pc_lst_customercontactPerson
(
	@customerId uniqueidentifier = null
)
AS

	SELECT 
		[pkid],
		[customerId],
		[contactPerson],
		[designation]
	FROM [dbo].[CustomerContactPerson]
	WHERE [customerId] = @customerId
		AND ([isDeleted] = 0)
GO

PRINT 'Creating...pc_lst_customercontactPerson...complete'
GO