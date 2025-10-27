declare
	@CustomerNames  varchar(max) = 'Customer Name 1',
	@DateFrom  varchar(50) = '10/01/2023',
	@DateTo  varchar(50) = '10/15/2023',
	@BeginningBalance decimal(18,0) =20000



	declare @unit_ipp as int,
			@unit_pp_sc as int,
			@unit_rta as int,
			@unit_sns as int,
			@amt_ipp as decimal(18,0),
			@amt_pp_sc as decimal(18,0),
			@amt_rta as decimal(18,0),
			@amt_sns as decimal(18,0),
			@amt_total as decimal(18,0),
			@sf_ipp as decimal(18,0),
			@sf_pp_sc as decimal(18,0),
			@sf_rta as decimal(18,0),
			@sf_sns as decimal(18,0),
			@sf_total as decimal(18,0),
			@withholdingTax as decimal(18,0),
			@totalLBCReceivable as decimal(18,0)

	SELECT DISTINCT
		--ROW_NUMBER() OVER(order by TransactionDate),
		DENSE_RANK() OVER (ORDER BY  acc.TransactionDate),
		acc.TransactionDate as 'Date',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'IPP'
		) AS 'Unit_IPP',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'PP/SC'
		) AS 'Unit_PP_SC',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'RTA'
		) AS 'Unit_RTA',
		(
			SELECT 
			SUM(_acc.unit)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'SNS'
		) AS 'Unit_SNS',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'IPP'
		) AS 'Amt_IPP',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'PP/SC'
		) AS 'Amt_PP_SC',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'RTA'
		) AS 'Amt_RTA',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType = 'SNS'
		) AS 'Amt_SNS',
		(
			SELECT 
			SUM(_acc.principalAmount)
			FROM [dbo].[Acceptance] _acc
			WHERE _acc.transactionDate = acc.transactionDate
			AND _acc.customerName = acc.customerName
			AND _acc.productType IN ('IPP', 'PP/SC', 'RTA', 'SNS')
		) AS 'Amt_Total',
		(
			--Unit_IPP
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
				AND _acc.productType = 'IPP'
			)
			*
			(
				SELECT DISTINCT
					( rt.ipp)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.customerName = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
			)
		) AS 'Sf_IPP',
		(
			--Unit_PP_SC
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
				AND _acc.productType = 'PP/SC'
			)
			*
			(
				SELECT DISTINCT
					( rt.pp_sc)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.customerName = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
			)
		) AS 'Sf_PP_SC',
		(
			--Unit_RTA
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
				AND _acc.productType = 'RTA'
			)
			*
			(
				SELECT DISTINCT
					( rt.rta)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.customerName = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
			)
		) AS 'Sf_RTA',
		(
			--Unit_SNS
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
				AND _acc.productType = 'SNS'
			)
			*
			(
				SELECT DISTINCT
					( rt.sns)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.customerName = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate = acc.transactionDate
				AND _acc.customerName = acc.customerName
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
		) AS 'TotalLBCReceivable',
		(
			--
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE rf.refundDate = acc.transactionDate
				AND rf.customerName = acc.customerName
				AND rf.productType = 'IPP'
			)
			+
			(
				SELECT 
				SUM(vd.principalAmount)
				FROM [dbo].[Voided] vd
				WHERE vd.refundDate = acc.transactionDate
				AND vd.customerName = acc.customerName
				AND vd.productType = 'IPP'
			)
		) AS 'Rv_IPP',
		(
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE rf.refundDate = acc.transactionDate
				AND rf.customerName = acc.customerName
				AND rf.productType = 'PP/SC'
			)
			+
			(
				SELECT 
				SUM(vd.principalAmount)
				FROM [dbo].[Voided] vd
				WHERE vd.refundDate = acc.transactionDate
				AND vd.customerName = acc.customerName
				AND vd.productType = 'PP/SC'
			)
		) AS 'Rv_PP_SC',
		(
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE rf.refundDate = acc.transactionDate
				AND rf.customerName = acc.customerName
				AND rf.productType = 'RTA'
			)
			+
			(
				SELECT 
				SUM(vd.principalAmount)
				FROM [dbo].[Voided] vd
				WHERE vd.refundDate = acc.transactionDate
				AND vd.customerName = acc.customerName
				AND vd.productType = 'RTA'
			)
		) AS 'Rv_RTA',
		(
			(
				SELECT 
				SUM(rf.principalAmount)
				FROM [dbo].[Refund] rf
				WHERE rf.refundDate = acc.transactionDate
				AND rf.customerName = acc.customerName
				AND rf.productType = 'SNS'
			)
			+
			(
				SELECT 
				SUM(vd.principalAmount)
				FROM [dbo].[Voided] vd
				WHERE vd.refundDate = acc.transactionDate
				AND vd.customerName = acc.customerName
				AND vd.productType = 'SNS'
			)
		) AS 'Rv_SNS',
		(
			SELECT 
			(pm.PHPPayment * -1)
			FROM [dbo].[Payment] pm
			WHERE pm.[date] = acc.transactionDate
			AND pm.customerName = acc.customerName
		) AS 'Settlement',
		(
			0
		) AS 'RunningBalance',
		(0) AS 'BalancePerAgent',
		(0) AS 'Variance',
		('') AS 'AcceptanceDocNumber',
		('') AS 'ServiceFeeDocNumber'
	FROM [dbo].[Acceptance] acc
	INNER JOIN [dbo].[Refund] ref on acc.customerName = ref.customerName
	INNER JOIN [dbo].[Voided] vd on acc.customerName = vd.customerName
	INNER JOIN [dbo].[Payment] pm on acc.customerName = pm.customerName
	WHERE (([acc].[customerName] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ','))) OR ((@CustomerNames IS NULL)))
	AND (([acc].[transactionDate] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null))) 
	GROUP BY [acc].[transactionDate], [acc].[customerName], [acc].[productType]