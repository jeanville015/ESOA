IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_useraccount_excel'))
BEGIN
  PRINT 'Dropping...pc_lst_useraccount_excel'
  DROP PROCEDURE pc_lst_useraccount_excel
END
PRINT 'Creating...pc_lst_useraccount_excel'
GO
				 
CREATE PROCEDURE pc_lst_useraccount_excel
AS

	SELECT 
		[pkid],
		[name],
		[jobTitle],
		[team],
		[role],
		[moduleAccess_admin],
		[moduleAccess_soa],
		[moduleAccess_payment],
		[moduleAccess_reports],
		[moduleAccess_granular],
		[accessRights_admin],
		[accessRights_soa],
		[accessRights_payment],
		[accessRights_reports],
		[accessRights_granular],
		[emailAddress],
		[contactNo]
	FROM [dbo].[UserAccount] 
	WHERE ([isDeleted] = 0)
GO

PRINT 'Creating...pc_lst_useraccount_excel...complete'
GO
