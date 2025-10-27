IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_SOAFormatB'))
BEGIN
  PRINT 'Dropping...pc_lst_SOAFormatB'
  DROP PROCEDURE pc_lst_SOAFormatB
END
PRINT 'Creating...pc_lst_SOAFormatB'
GO
				 
CREATE PROCEDURE pc_lst_SOAFormatB
(
	@CustomerNames  varchar(max) = NULL,
	@DateFrom  varchar(50) = NULL,
	@DateTo  varchar(50) = NULL,
	@BeginningBalance decimal(18,0)
)
AS

	SELECT DISTINCT
		--ROW_NUMBER() OVER(order by TransactionDate),
		DENSE_RANK() OVER (ORDER BY  CAST(acc.TransactionDate AS DATE)),
		--acc.TransactionDate as 'Date',
		CAST(acc.TransactionDate AS DATE) as 'Date',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType = 'IPP'
		) AS 'Unit_IPP',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE  CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND (_acc.productType = 'PP' --OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ'
			)
		) AS 'Unit_PP_SC',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE  CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType = 'RTA'
		) AS 'Unit_RTA',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType = 'SNS'
		) AS 'Unit_SNS',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType = 'IPP'
		) AS 'Amt_IPP',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND (_acc.productType = 'PP' --OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ'
			)
		) AS 'Amt_PP_SC',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType = 'RTA'
		) AS 'Amt_RTA',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType = 'SNS'
		) AS 'Amt_SNS',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			AND _acc.origin_agent_name = acc.origin_agent_name
			AND _acc.productType IN ('IPP', 'PP', --'SC', 'PP24', 'SQ', 
			'RTA', 'SNS')
		) AS 'Amt_Total',
		(
			--Unit_IPP
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
				AND _acc.productType = 'IPP'
			)
			*
			(
				SELECT DISTINCT
					( rt.ipp)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
			)
		) AS 'Sf_IPP',
		(
			--Unit_PP_SC
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
				AND (_acc.productType = 'PP' --OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ'
				)
			)
			*
			(
				SELECT DISTINCT
					( rt.pp_sc)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
			)
		) AS 'Sf_PP_SC',
		(
			--Unit_RTA
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
				AND _acc.productType = 'RTA'
			)
			*
			(
				SELECT DISTINCT
					( rt.rta)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
			)
		) AS 'Sf_RTA',
		(
			--Unit_SNS
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
				AND _acc.productType = 'SNS'
			)
			*
			(
				SELECT DISTINCT
					( rt.sns)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.origin_agent_name = acc.origin_agent_name
			)
		) AS 'Sf_SNS',
		(
			0
		) AS 'Sf_Total',
		(
			0
		)AS 'WithholdingTax',
		(	
			0
		) AS 'Total',
		(
			--
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND rf.origin_agent_name = acc.origin_agent_name
				AND rf.productType = 'IPP'
			)
			--+
			--(
			--	SELECT 
			--	SUM(vd.principalAmount)
			--	FROM [dbo].[Voided] vd
			--	WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			--	AND vd.origin_agent_name = acc.origin_agent_name
			--	AND vd.productType = 'IPP'
			--)
		) AS 'Rv_IPP',
		(
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND rf.origin_agent_name = acc.origin_agent_name
				AND (rf.productType = 'PP' --OR rf.productType = 'SC' OR rf.productType = 'PP24' OR rf.productType = 'SQ'
				)
			)
			--+
			--(
			--	SELECT 
			--	SUM(vd.principalAmount)
			--	FROM [dbo].[Voided] vd
			--	WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			--	AND vd.origin_agent_name = acc.origin_agent_name
			--	AND (vd.productType = 'PP' OR vd.productType = 'SC' OR vd.productType = 'PP24' OR vd.productType = 'SQ')
			--)
		) AS 'Rv_PP_SC',
		(
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND rf.origin_agent_name = acc.origin_agent_name
				AND rf.productType = 'RTA'
			)
			--+
			--(
			--	SELECT 
			--	SUM(vd.principalAmount)
			--	FROM [dbo].[Voided] vd
			--	WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			--	AND vd.origin_agent_name = acc.origin_agent_name
			--	AND vd.productType = 'RTA'
			--)
		) AS 'Rv_RTA',
		(
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND rf.origin_agent_name = acc.origin_agent_name
				AND rf.productType = 'SNS'
			)
			--+
			--(
			--	SELECT 
			--	SUM(vd.principalAmount)
			--	FROM [dbo].[Voided] vd
			--	WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
			--	AND vd.origin_agent_name = acc.origin_agent_name
			--	AND vd.productType = 'SNS'
			--)
		) AS 'Rv_SNS',
		(
			SELECT 
			(pm.PHPPayment * -1)
			FROM [dbo].[Payment] pm
			WHERE CAST(pm.[date] AS DATE) like CAST(acc.TransactionDate AS DATE)
			AND pm.origin_agent_name = acc.origin_agent_name
		) AS 'Settlement',
		(
			0
		) AS 'RunningBalance',
		(0) AS 'BalancePerAgent',
		(0) AS 'Variance',
		('') AS 'AcceptanceDocNumber',
		('') AS 'ServiceFeeDocNumber',
		[cus].[ApprovedAFC] AS 'ApprovedAFC'
	FROM [dbo].[Acceptance] acc
	LEFT JOIN [dbo].[Refund] ref on acc.origin_agent_name = ref.origin_agent_name
	LEFT JOIN [dbo].[Voided] vd on acc.origin_agent_name = vd.origin_agent_name
	LEFT JOIN [dbo].[Payment] pm on acc.origin_agent_name = pm.origin_agent_name
	LEFT JOIN [dbo].[Customer] cus on [cus].[name] = [acc].origin_agent_name
	WHERE (([acc].[origin_agent_name] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ';'))) OR ((@CustomerNames IS NULL)))
	--AND (([acc].[transactionDate] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (([acc].[transactionDate] >= (CAST(@DateFrom as datetime)) AND [acc].[transactionDate] < DATEADD(DAY, 1, CAST(@DateTo as datetime))) OR ((@DateFrom is null) OR (@DateTo is null)))
	AND (acc.isDeleted=0 or acc.isDeleted='' or acc.isDeleted is null) 
	GROUP BY [acc].[TransactionDate], [acc].[origin_agent_name], [acc].[productType], [cus].[approvedAFC]
GO

PRINT 'Creating...pc_lst_SOAFormatB...complete'
GO