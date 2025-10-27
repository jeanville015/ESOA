IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_refund'))
BEGIN
  PRINT 'Dropping...pc_lst_refund'
  DROP PROCEDURE pc_lst_refund
END
PRINT 'Creating...pc_lst_refund'
GO
				 
CREATE PROCEDURE pc_lst_refund
(
	@CustomerNames  varchar(max) = NULL,
	@DateFrom  varchar(50) = NULL,
	@DateTo  varchar(50) = NULL,
	@ProductType  varchar(50) = NULL,
	@Search  varchar(150) = NULL
)
AS

	SELECT 
		[rf].[pkid],
		[rf].[origin_agent_name],
		[rf].[officeCode],
		[rf].[date],
		[rf].[productType],
		[rf].[trackingNumber],
		[rf].[referenceNumber],
		[rf].[entBranch],
		[rf].[shipperName],
		[rf].[consigneeName],
		[rf].[unit],
		[rf].[principalAmount],
		[rf].[serviceFee],
		[rf].[refundDate],
		[rf].[statusCode],
		[rf].[statusDescription],
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
		[rf].[encashmentBranchHub],
		[cus].[country]
	FROM [dbo].[Refund] rf
	LEFT JOIN [dbo].[Customer] cus on [cus].[officeCode] = [rf].[officeCode]
	WHERE (([rf].[origin_agent_name] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ';'))) OR ((@CustomerNames IS NULL)))
	--AND (([rf].[date] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([rf].[date] >= (CAST(@DateFrom as datetime)) AND [rf].[date] < DATEADD(DAY, 1, CAST(@DateTo as datetime))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([rf].[productType] like @ProductType) OR(@ProductType IS NULL))
	AND (cus.isDeleted=0 or cus.isDeleted='' or cus.isDeleted is null) 
	AND
	(
		@Search IS NULL
		OR [rf].[origin_agent_name] LIKE '%'+@Search+'%'
		OR [rf].[officeCode] LIKE '%'+@Search+'%'
		OR [rf].[trackingNumber] LIKE '%'+@Search+'%'
		OR [rf].[referenceNumber] LIKE '%'+@Search+'%'
		OR [rf].[entBranch] LIKE '%'+@Search+'%'
		OR [rf].[shipperName] LIKE '%'+@Search+'%'
		OR [rf].[consigneeName] LIKE '%'+@Search+'%'
	)
	ORDER BY [rf].[date]
GO

PRINT 'Creating...pc_lst_refund...complete'
GO