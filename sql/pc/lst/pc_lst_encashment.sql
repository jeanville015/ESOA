IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_encashment'))
BEGIN
  PRINT 'Dropping...pc_lst_encashment'
  DROP PROCEDURE pc_lst_encashment
END
PRINT 'Creating...pc_lst_encashment'
GO
				 
CREATE PROCEDURE pc_lst_encashment
(
	@CustomerNames  varchar(max) = NULL,
	@DateFrom  varchar(50) = NULL,
	@DateTo  varchar(50) = NULL,
	@ProductType  varchar(50) = NULL,
	@Search  varchar(150) = NULL
)
AS

	SELECT 
		[acc].[pkid],
		[acc].[origin_agent_name],
		--[acc].[tagging],
		'Acceptance' AS [tagging],
		[acc].[officeCode],
		FORMAT(CAST([acc].[transactionDate] AS DATETIME), 'dd-MMM-yyyy') AS [transactionDate],
		[acc].[productType],
		[acc].[trackingNumber],
		[acc].[referenceNumber],
		[acc].[encashmentBranch],
		[acc].[shipperName],
		[acc].[consigneeName],
		[acc].[unit],
		[acc].[principalAmount],
		FORMAT(CAST([acc].[encashmentDate] AS DATETIME), 'dd-MMM-yyyy') AS [encashmentDate],
		[acc].[statusCode],
		[acc].[statusDescription],
		(
			CASE	WHEN [statusCode] = 'DEL' OR [statusCode] = 'REP' OR [statusCode] = 'PIK' 
						THEN 'FULFILLED'
					WHEN [productType] = 'IPP' AND [statusCode] = '1'
						THEN 'FULFILLED'
					WHEN [productType] = 'RTA' AND [statusDescription] = 'DEPOSITED' OR [statusDescription] = 'CONVERTED' OR [statusDescription] = 'REFUNDED'
						THEN 'FULFILLED'
					ELSE 'PENDING'
			END
		)AS 'status',
		[encashmentBranchHub],
		[cus].[country]
	FROM [dbo].[Encashment] acc
	LEFT JOIN [dbo].[Customer] cus on [cus].[officeCode] = [acc].[officeCode]
	WHERE (([acc].[origin_agent_name] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ';'))) OR ((@CustomerNames IS NULL)))
	--AND (([acc].[transactionDate] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([acc].[transactionDate] >= (CAST(@DateFrom as datetime)) AND [acc].[transactionDate] < DATEADD(DAY, 1, CAST(@DateTo as datetime))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([acc].[productType] like @ProductType) OR(@ProductType IS NULL))
	AND (cus.isDeleted=0 or cus.isDeleted='' or cus.isDeleted is null) 
	AND
	(
		@Search IS NULL
		OR [acc].[origin_agent_name] LIKE '%'+@Search+'%'
		OR [acc].[officeCode] LIKE '%'+@Search+'%'
		OR [acc].[trackingNumber] LIKE '%'+@Search+'%'
		OR [acc].[referenceNumber] LIKE '%'+@Search+'%'
		OR [acc].[encashmentBranch] LIKE '%'+@Search+'%'
		OR [acc].[shipperName] LIKE '%'+@Search+'%'
		OR [acc].[consigneeName] LIKE '%'+@Search+'%'
	)
	ORDER BY [acc].[transactionDate]
GO

PRINT 'Creating...pc_lst_encashment...complete'
GO