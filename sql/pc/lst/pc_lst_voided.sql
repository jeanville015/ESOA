IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_voided'))
BEGIN
  PRINT 'Dropping...pc_lst_voided'
  DROP PROCEDURE pc_lst_voided
END
PRINT 'Creating...pc_lst_voided'
GO
				 
CREATE PROCEDURE pc_lst_voided
(
	@CustomerNames  varchar(max) = NULL,
	@DateFrom  varchar(50) = NULL,
	@DateTo  varchar(50) = NULL,
	@ProductType  varchar(50) = NULL,
	@Search  varchar(150) = NULL
)
AS

	SELECT 
		[vd].[pkid],
		[vd].[origin_agent_name],
		[vd].[officeCode],
		[vd].[date],
		[vd].[productType],
		[vd].[trackingNumber],
		[vd].[referenceNumber],
		[vd].[entBranch],
		[vd].[shipperName],
		[vd].[consigneeName],
		[vd].[unit],
		[vd].[principalAmount],
		[vd].[serviceFee],
		[vd].[refundDate],
		[vd].[statusCode],
		[vd].[statusDescription],
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
		[vd].[encashmentBranchHub],
		[cus].[country]
	FROM [dbo].[Voided] vd
	LEFT JOIN [dbo].[Customer] cus on [cus].[officeCode] = [vd].[officeCode]
	WHERE (([vd].[origin_agent_name] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ';'))) OR ((@CustomerNames IS NULL)))
	--AND (([vd].[date] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([vd].[date] >= (CAST(@DateFrom as datetime)) AND [vd].[date] < DATEADD(DAY, 1, CAST(@DateTo as datetime))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([vd].[productType] like @ProductType) OR(@ProductType IS NULL))
	AND (cus.isDeleted=0 or cus.isDeleted='' or cus.isDeleted is null) 
	AND
	(
		@Search IS NULL
		OR [vd].[origin_agent_name] LIKE '%'+@Search+'%'
		OR [vd].[officeCode] LIKE '%'+@Search+'%'
		OR [vd].[trackingNumber] LIKE '%'+@Search+'%'
		OR [vd].[referenceNumber] LIKE '%'+@Search+'%'
		OR [vd].[entBranch] LIKE '%'+@Search+'%'
		OR [vd].[shipperName] LIKE '%'+@Search+'%'
		OR [vd].[consigneeName] LIKE '%'+@Search+'%'
	)
	ORDER BY [vd].[date]
GO

PRINT 'Creating...pc_lst_voided...complete'
GO