IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_SOAFormatA'))
BEGIN
  PRINT 'Dropping...pc_lst_SOAFormatA'
  DROP PROCEDURE pc_lst_SOAFormatA
END
PRINT 'Creating...pc_lst_SOAFormatA'
GO
				 
CREATE PROCEDURE pc_lst_SOAFormatA
(
	@CustomerNames  varchar(max) = 'METROBANK',
	@DateFrom  varchar(50) = '2025-03-01',
	@DateTo  varchar(50) = '2025-03-31',
	@BeginningBalance decimal(18,0) = null
	--@PreviousRunningBalance decimal(18,0) = 0 output
)
AS 
	WITH Calculations AS
	(
		SELECT DISTINCT
			--ROW_NUMBER() OVER(order by TransactionDate),
			DENSE_RANK() OVER (ORDER BY  CAST(acc.TransactionDate AS DATE)) AS [index],
			FORMAT(CAST([acc].[transactionDate] AS DATETIME), 'dd-MMM-yyyy') AS [Date],
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType = 'IPP'
			) AS [Unit_IPP],
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE  CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND (_acc.productType = 'PP' OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ')
			) AS [Unit_PP_SC],
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE  CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType = 'RTA'
			) AS [Unit_RTA],
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType = 'SNS'
			) AS [Unit_SNS],
			(
				SELECT 
				SUM(_acc.principalAmount)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType = 'IPP'
			) AS [Amt_IPP],
			(
				SELECT 
				SUM(_acc.principalAmount)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND (_acc.productType = 'PP' OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ')
			) AS [Amt_PP_SC],
			(
				SELECT 
				SUM(_acc.principalAmount)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType = 'RTA'
			) AS [Amt_RTA],
			(
				SELECT 
				SUM(_acc.principalAmount)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType = 'SNS'
			) AS [Amt_SNS],
			(
				SELECT 
				SUM(_acc.principalAmount)
				FROM [dbo].[Acceptance] _acc
				WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
				AND _acc.officeCode = acc.officeCode
				AND _acc.productType IN ('IPP', 'PP', 'SC', 'PP24', 'SQ', 'RTA', 'SNS')
			) AS [Amt_Total],
			(
				CASE (select top 1 [rt].[rateType_ipp] from [dbo].[Rate] rt where [rt].[pkid] = [cus].[rateCardId])
					--when Rate>rateType_ipp = AMOUNT
					WHEN ('AMOUNT')
					THEN
					(
						(
					
							SELECT 
							ISNULL(SUM(_acc.unit),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND _acc.productType = 'IPP'
						)
						*
						(
							SELECT TOP 1
								(rt.ipp)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
					)
					--when Rate>rateType_ipp = PERCENTAGE
					WHEN ('PERCENTAGE')
					THEN
					(
						(
							SELECT TOP 1
								(rt.ipp / 100)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
						*
						(
							SELECT 
								ISNULL(SUM(_acc.principalAmount),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND _acc.productType = 'IPP'
						)
					)
				END 
			) AS [Sf_IPP],
			(
				CASE (select top 1 [rt].[rateType_pp_sc] from [dbo].[Rate] rt where [rt].[pkid] = [cus].[rateCardId])
					--when Rate>rateType_pp_sc = AMOUNT
					WHEN ('AMOUNT')
					THEN
					(
						(
					
							SELECT 
							ISNULL(SUM(_acc.unit),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND (_acc.productType = 'PP' OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ')
						)
						*
						(
							SELECT TOP 1
								(rt.pp_sc)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
					)
					--when Rate>rateType_pp_sc = PERCENTAGE
					WHEN ('PERCENTAGE')
					THEN
					(
						(
							SELECT TOP 1
								(rt.pp_sc / 100)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
						*
						(
							SELECT 
								ISNULL(SUM(_acc.principalAmount),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND (_acc.productType = 'PP' OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ')
						)
					)
				END 
			) AS [Sf_PP_SC],
			(
				CASE (select top 1 [rt].[rateType_rta] from [dbo].[Rate] rt where [rt].[pkid] = [cus].[rateCardId])
					--when Rate>rateType_rta = AMOUNT
					WHEN ('AMOUNT')
					THEN
					(
						(
					
							SELECT 
							ISNULL(SUM(_acc.unit),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND _acc.productType = 'RTA'
						)
						*
						(
							SELECT TOP 1
								(rt.rta)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
					)
					--when Rate>rateType_rta = PERCENTAGE
					WHEN ('PERCENTAGE')
					THEN
					(
						(
							SELECT TOP 1
								(rt.rta / 100)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
						*
						(
							SELECT 
								ISNULL(SUM(_acc.principalAmount),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND _acc.productType = 'RTA'
						)
					)
				END
			) AS [Sf_RTA],
			(
				CASE (select top 1 [rt].[rateType_sns] from [dbo].[Rate] rt where [rt].[pkid] = [cus].[rateCardId])
					--when Rate>rateType_sns = AMOUNT
					WHEN ('AMOUNT')
					THEN
					(
						(
					
							SELECT 
							ISNULL(SUM(_acc.unit),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND _acc.productType = 'SNS'
						)
						*
						(
							SELECT TOP 1
								(rt.sns)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
					)
					--when Rate>rateType_sns = PERCENTAGE
					WHEN ('PERCENTAGE')
					THEN
					(
						(
							SELECT TOP 1
								(rt.sns / 100)
							FROM [dbo].[Acceptance] _acc
							INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
							INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
							WHERE _acc.officeCode = acc.officeCode
							AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
						)
						*
						(
							SELECT 
								ISNULL(SUM(_acc.principalAmount),0)
							FROM [dbo].[Acceptance] _acc
							WHERE CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
							AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							AND _acc.officeCode = acc.officeCode
							AND _acc.productType = 'SNS'
						)
					)
				END
			) AS [Sf_SNS],
			--(
			--	0
			--) AS [Sf_Total],
			--(
			--	0
			--)AS [WithholdingTax],
			(	
				0
			) AS [TotalLBCReceivable],
			(
				--
				(
					SELECT 
					ISNULL(SUM(rf.principalAmount),0) 
					FROM [dbo].[Refund] rf
					WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND rf.officeCode = acc.officeCode
					AND rf.productType = 'IPP'
				)
				+
				(
					SELECT
					ISNULL(SUM(vd.principalAmount),0) 
					FROM [dbo].[Voided] vd
					WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE) 
					AND vd.officeCode = acc.officeCode
					AND vd.productType = 'IPP'
				)
			) AS [Rv_IPP],
			(
				(
					SELECT
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND rf.officeCode = acc.officeCode
					AND (rf.productType = 'PP' OR rf.productType = 'SC' OR rf.productType = 'PP24' OR rf.productType = 'SQ')
				)
				+
				(
					SELECT
					ISNULL(SUM(vd.principalAmount),0)
					FROM [dbo].[Voided] vd
					WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND vd.officeCode = acc.officeCode
					AND (vd.productType = 'PP' OR vd.productType = 'SC' OR vd.productType = 'PP24' OR vd.productType = 'SQ')
				)
			) AS [Rv_PP_SC],
			(
				(
					SELECT
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND rf.officeCode = acc.officeCode
					AND rf.productType = 'RTA'
				)
				+
				(
					SELECT
					ISNULL(SUM(vd.principalAmount),0)
					FROM [dbo].[Voided] vd
					WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND vd.officeCode = acc.officeCode
					AND vd.productType = 'RTA'
				)
			) AS [Rv_RTA],
			(
				(
					SELECT
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND rf.officeCode = acc.officeCode
					AND rf.productType = 'SNS'
				)
				+
				(
					SELECT
					ISNULL(SUM(vd.principalAmount),0)
					FROM [dbo].[Voided] vd
					WHERE CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					AND vd.officeCode = acc.officeCode
					AND vd.productType = 'SNS'
				)
			) AS [Rv_SNS],
			(
				SELECT 
					(ISNULL(SUM(pm.PHPPayment),0))
				FROM [dbo].[Payment] pm
				WHERE CAST(pm.[date] AS DATE) like CAST(acc.TransactionDate AS DATE)
				AND pm.origin_agent_name = acc.origin_agent_name
			) AS [Settlement],
			(
				0
			) AS [RunningBalance],
			(0) AS [BalancePerAgent],
			(0) AS [Variance],
			('') AS [AcceptanceDocNumber],
			('') AS [ServiceFeeDocNumber],
			[cus].[ApprovedAFC] AS [ApprovedAFC],
			[cus].[withholdingTax],
			[cus].[vatStatus]
		FROM [dbo].[Acceptance] acc
		LEFT JOIN [dbo].[Refund] ref on acc.officeCode = ref.officeCode
		LEFT JOIN [dbo].[Voided] vd on acc.officeCode = vd.officeCode
		LEFT JOIN [dbo].[Payment] pm on acc.origin_agent_name = pm.origin_agent_name
		LEFT JOIN [dbo].[Customer] cus on [acc].[officeCode] = [cus].officeCode
		WHERE (([cus].[name] IN (SELECT value FROM STRING_SPLIT(@CustomerNames, ';'))) OR ((@CustomerNames IS NULL)))
		--AND (([acc].[transactionDate] BETWEEN (convert(datetime, @DateFrom, 110)) AND (convert(datetime, @DateTo, 110))) OR ((@DateFrom is null) OR (@DateTo is null)))
		AND (([acc].[transactionDate] >= (CAST(@DateFrom as datetime)) AND [acc].[transactionDate] < DATEADD(DAY, 1, CAST(@DateTo as datetime))) OR ((@DateFrom is null) OR (@DateTo is null)))
		AND (cus.isDeleted=0 or cus.isDeleted='' or cus.isDeleted is null) 
		AND (acc.isDeleted=0 or acc.isDeleted='' or acc.isDeleted is null) 
		GROUP BY [acc].[TransactionDate], [acc].[origin_agent_name], [acc].[productType], [cus].[approvedAFC], acc.officeCode,cus.rateCardId,cus.withholdingTax, cus.vatStatus
	)

	SELECT 
		[Date],
		[Unit_IPP],
		[Unit_PP_SC],
		[Unit_RTA],
		[Unit_SNS],
		[Amt_IPP],
		[Amt_PP_SC],
		[Amt_RTA],
		[Amt_SNS],
		[Amt_Total],
		[Sf_IPP],
		[Sf_PP_SC],
		[Sf_RTA],
		[Sf_SNS],
		(Sf_IPP + Sf_PP_SC + Sf_RTA + Sf_SNS) AS [Sf_Total],
		(
			CASE  
			WHEN (withholdingTax='Yes' AND vatStatus='12%')
			THEN
			(
				((((Sf_IPP + Sf_PP_SC + Sf_RTA + Sf_SNS) / 1.12) * 0.02) * -1)
			)
			WHEN (withholdingTax='Yes' AND vatStatus='Zero-Rated')
			THEN
			(
				(((Sf_IPP + Sf_PP_SC + Sf_RTA + Sf_SNS) * 0.02) * -1)
			)
			WHEN (withholdingTax='Yes' AND vatStatus='Exempt')
			THEN
			(
				(((Sf_IPP + Sf_PP_SC + Sf_RTA + Sf_SNS) * 0.02) * -1)
			)
			ELSE
			(
				0
			)
			END
		) AS [WithholdingTax],
		[TotalLBCReceivable],
		[Rv_IPP],
		[Rv_PP_SC],
		[Rv_RTA],
		[Rv_SNS],
		[Settlement],
		[RunningBalance],
		[BalancePerAgent],
		[Variance],
		[AcceptanceDocNumber],
		[ServiceFeeDocNumber],
		[ApprovedAFC]
	FROM Calculations
	ORDER BY [Date]
GO

PRINT 'Creating...pc_lst_SOAFormatA...complete'
GO