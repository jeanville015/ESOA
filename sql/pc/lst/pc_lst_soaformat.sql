IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_soaformat'))
BEGIN
  PRINT 'Dropping...pc_lst_soaformat'
  DROP PROCEDURE pc_lst_soaformat
END
PRINT 'Creating...pc_lst_soaformat'
GO
				 
CREATE PROCEDURE pc_lst_soaformat
AS

	SELECT 
		[pkid],
		[formatName]
	FROM [dbo].[SOAFormat]
	WHERE ([isDeleted] = 0)
	ORDER BY [formatName]
GO

PRINT 'Creating...pc_lst_soaformat...complete'
GO
