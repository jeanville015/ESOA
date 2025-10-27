IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_payment'))
BEGIN
  PRINT 'Dropping...pc_lst_payment'
  DROP PROCEDURE pc_lst_payment
END
PRINT 'Creating...pc_lst_payment'
GO
				 
CREATE PROCEDURE pc_lst_payment
(
	@CustomerNames  varchar(max) = NULL,
	@DateFrom  varchar(50) = NULL,
	@DateTo  varchar(50) = NULL
)
AS

	SELECT 
		[pkid],
		[uploadedBy],
		FORMAT(CAST([date] AS DATETIME), 'dd-MMM-yyyy') AS [date],
		[origin_agent_name],
		[customerId],
		[bankAccount],
		[bankAccountGLCode],
		[USDPayment],
		[excRate],
		[PHPPayment],
		[assignment],
		[text],
		[SAPDocNumber]
	FROM [dbo].[Payment]
	WHERE (([origin_agent_name] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ';'))) OR ((@CustomerNames IS NULL)))
	AND (([date] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null)))
GO

PRINT 'Creating...pc_lst_payment...complete'
GO