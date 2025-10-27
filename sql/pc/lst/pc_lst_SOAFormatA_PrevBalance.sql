IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_SOAFormatA_PrevBalanceVariables'))
BEGIN
  PRINT 'Dropping...pc_lst_SOAFormatA_PrevBalanceVariables'
  DROP PROCEDURE pc_lst_SOAFormatA_PrevBalanceVariables
END
PRINT 'Creating...pc_lst_SOAFormatA_PrevBalanceVariables'
GO

--It will return
--[TotalRecievable],
--[Settlement],
--[All_RVProductype_Total]
--separately
CREATE PROCEDURE pc_lst_SOAFormatA_PrevBalanceVariables
(
	@CustomerName varchar(max) = NULL, --'ACOM;',
	@DateFrom  varchar(50) = NULL --'2025-03-02' 
)
AS 

	WITH Amount_SfTotal_formulas AS
	(
		SELECT DISTINCT 
			--Amt_Total
			(
				SELECT 
				ISNULL(SUM(_acc.principalAmount),0)
				FROM [dbo].[Acceptance] _acc
				WHERE 
				--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
				(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
				AND 
				_acc.officeCode = acc.officeCode
				AND _acc.productType IN ('IPP', 'PP', 'SC', 'PP24', 'SQ', 'RTA', 'SNS')
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			) AS Amt_Total,
		
			--Sf_Total
			(
				--Sf_IPP
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND _acc.productType = 'IPP'
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT TOP 1
									(rt.ipp)
								FROM [dbo].[Acceptance] _acc
								INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
								INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
								AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
							)
							*
							(
								SELECT 
									ISNULL(SUM(_acc.principalAmount),0)
								FROM [dbo].[Acceptance] _acc
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND _acc.productType = 'IPP'
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
						)
						END
				
				)
				+
				--Sf_PP_SC
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND (_acc.productType = 'PP' OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ')
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT TOP 1
									( rt.pp_sc)
								FROM [dbo].[Acceptance] _acc
								INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
								INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
								AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT 
									ISNULL(SUM(_acc.principalAmount),0)
								FROM [dbo].[Acceptance] _acc
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND (_acc.productType = 'PP' OR _acc.productType = 'SC' OR _acc.productType = 'PP24' OR _acc.productType = 'SQ')
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
						)
						END
				)
				+
				--Sf_RTA
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND _acc.productType = 'RTA'
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT TOP 1
									( rt.rta)
								FROM [dbo].[Acceptance] _acc
								INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
								INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
								AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT 
									ISNULL(SUM(_acc.principalAmount),0)
								FROM [dbo].[Acceptance] _acc
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND _acc.productType = 'RTA'
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
						)
						END
				)
				+
				--Sf_SNS
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND _acc.productType = 'SNS'
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT TOP 1
									( rt.sns)
								FROM [dbo].[Acceptance] _acc
								INNER JOIN [dbo].[Customer] cust on _acc.officeCode = cust.officeCode
								INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND
								_acc.officeCode = acc.officeCode
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
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								--AND 
								_acc.officeCode = acc.officeCode
								AND (cust.isDeleted=0 or cust.isDeleted='' or cust.isDeleted is null) 
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
							*
							(
								SELECT 
									ISNULL(SUM(_acc.principalAmount),0)
								FROM [dbo].[Acceptance] _acc
								WHERE 
								--CAST(_acc.transactionDate AS DATE) like CAST(acc.TransactionDate AS DATE)
								(([_acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
								AND 
								_acc.officeCode = acc.officeCode
								AND _acc.productType = 'SNS'
								AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
							)
						)
						END
				) 

			)AS Sf_Total,
			(
				SELECT
				ISNULL(SUM(pm.PHPPayment),0)
				FROM [dbo].[Payment] pm
				WHERE 
				--CAST(pm.[date] AS DATE) like CAST(acc.TransactionDate AS DATE)
				(([pm].[date] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
				AND 
				pm.origin_agent_name = acc.origin_agent_name
			) AS Settlement,
			(
				--
				(
					SELECT 
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE 
					--CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([rf].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					rf.officeCode = acc.officeCode
					AND rf.productType = 'IPP'
				)
				+
				(
					SELECT 
					ISNULL(SUM(vd.principalAmount),0)
					FROM [dbo].[Voided] vd
					WHERE 
					--CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					((vd.[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					vd.officeCode = acc.officeCode
					AND vd.productType = 'IPP'
				)
			) AS Rv_IPP,
			(
				(
					SELECT 
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE 
					--CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([rf].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					rf.officeCode = acc.officeCode
					AND (rf.productType = 'PP' OR rf.productType = 'SC' OR rf.productType = 'PP24' OR rf.productType = 'SQ')
				)
				+
				(
					SELECT 
					ISNULL(SUM(vd.principalAmount),0)
					FROM [dbo].[Voided] vd
					WHERE 
					--CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([vd].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					vd.officeCode = acc.officeCode
					AND (vd.productType = 'PP' OR vd.productType = 'SC' OR vd.productType = 'PP24' OR vd.productType = 'SQ')
				)
			) AS Rv_PP_SC,
			(
				(
					SELECT 
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE 
					--CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([rf].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND
					rf.officeCode = acc.officeCode
					AND rf.productType = 'RTA'
				)
				+
				(
					SELECT 
					ISNULL(SUM(_vd.principalAmount),0)
					FROM [dbo].[Voided] _vd
					WHERE 
					--CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([_vd].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					_vd.officeCode = acc.officeCode
					AND _vd.productType = 'RTA'
				)
			) AS Rv_RTA,
			(
				(
					SELECT 
					ISNULL(SUM(rf.principalAmount),0)
					FROM [dbo].[Refund] rf
					WHERE 
					--CAST(rf.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([rf].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					rf.officeCode = acc.officeCode
					AND rf.productType = 'SNS'
				)
				+
				(
					SELECT 
					ISNULL(SUM(vd.principalAmount),0)
					FROM [dbo].[Voided] vd
					WHERE 
					--CAST(vd.refundDate AS DATE) like CAST(acc.TransactionDate AS DATE)
					(([vd].[refundDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
					AND 
					vd.officeCode = acc.officeCode
					AND vd.productType = 'SNS'
				)
			) AS Rv_SNS,
			cus.withholdingTax,
			cus.vatStatus,
			cus.beginningBalance
		FROM [dbo].[Acceptance] acc
		LEFT JOIN [dbo].[Refund] ref on acc.officeCode = ref.officeCode
		LEFT JOIN [dbo].[Voided] vd on acc.officeCode = vd.officeCode
		LEFT JOIN [dbo].[Payment] pm on acc.origin_agent_name = pm.origin_agent_name
		LEFT JOIN [dbo].[Customer] cus on [acc].[officeCode] = [cus].officeCode
		WHERE (([cus].[name] IN (SELECT value FROM STRING_SPLIT(@CustomerName, ';'))) OR ((@CustomerName IS NULL))) 
		AND (([acc].[transactionDate] < (CAST(@DateFrom as datetime))) OR @DateFrom is null)
		AND (cus.isDeleted=0 or cus.isDeleted='' or cus.isDeleted is null) 
		AND ([acc].[isDeleted]=0 or [acc].[isDeleted]='' or [acc].[isDeleted] is null) 
		GROUP BY acc.officeCode, cus.rateCardId, [acc].[origin_agent_name], cus.withholdingTax, cus.vatStatus, cus.beginningBalance --,[acc].[TransactionDate], [acc].[productType], [cus].[approvedAFC], 
	)

	SELECT  
		(
			Amt_Total 
			+ 
			Sf_Total 
			+
			--withholdingTax
			(
				CASE  
				WHEN (withholdingTax='Yes' AND vatStatus='12%')
				THEN
				(
					(((Sf_Total / 1.12) * 0.02)* -1)
				)
				WHEN (withholdingTax='Yes' AND vatStatus='Zero-Rated')
				THEN
				(
					((Sf_Total * 0.02)* -1)
				)
				WHEN (withholdingTax='Yes' AND vatStatus='Exempt')
				THEN
				(
					((Sf_Total * 0.02)* -1)
				)
				ELSE
				(
					0
				)
				END
			)
		) AS [TotalRecievable],
		Settlement AS [Settlement],
		( Rv_IPP + Rv_PP_SC + Rv_RTA + Rv_SNS ) AS [All_RVProductype_Total]

		
		-- Amt_Total,
		-- Sf_Total,
		----withholdingTax
		--(
		--	CASE  
		--	WHEN (withholdingTax='Yes' AND vatStatus='12%')
		--	THEN
		--	(
		--		(((Sf_Total / 1.12) * 0.02)* -1)
		--	)
		--	WHEN (withholdingTax='Yes' AND vatStatus='Zero-Rated')
		--	THEN
		--	(
		--		((Sf_Total * 0.02)* -1)
		--	)
		--	WHEN (withholdingTax='Yes' AND vatStatus='Exempt')
		--	THEN
		--	(
		--		((Sf_Total * 0.02)* -1)
		--	)
		--	ELSE
		--	(
		--		0
		--	)
		--	END
		--) AS WithholdingTax, 
		--Settlement AS [Settlement],
		--( Rv_IPP + Rv_PP_SC + Rv_RTA + Rv_SNS ) AS [All_RVProductype_Total]
		
	FROM Amount_SfTotal_formulas

GO

PRINT 'Creating...pc_lst_SOAFormatA_PrevBalance...complete'
GO