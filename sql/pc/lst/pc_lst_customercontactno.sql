IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customercontactno'))
BEGIN
  PRINT 'Dropping...pc_lst_customercontactno'
  DROP PROCEDURE pc_lst_customercontactno
END
PRINT 'Creating...pc_lst_customercontactno'
GO
				 
CREATE PROCEDURE pc_lst_customercontactno
(
	@customerId uniqueidentifier = null
)
AS

	SELECT 
		[pkid],
		[customerId],
		[contactNo]
	FROM [dbo].[CustomerContactNo]
	WHERE [customerId] = @customerId 
		AND ([isDeleted] = 0)
GO

PRINT 'Creating...pc_lst_customercontactno...complete'
GO
