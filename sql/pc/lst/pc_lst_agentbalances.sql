IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_agentbalances'))
BEGIN
  PRINT 'Dropping...pc_lst_agentbalances'
  DROP PROCEDURE pc_lst_agentbalances
END
PRINT 'Creating...pc_lst_agentbalances'
GO
				 
CREATE PROCEDURE pc_lst_agentbalances
( 
	@DateAsOf varchar(50) = NULL 
)
AS
	set @DateAsOf = CONVERT(VARCHAR(10), convert(datetime, @DateAsOf, 110), 101)
	SELECT DISTINCT
		--CustomerName
		[c].[name] AS 'CustomerName',
		[c].[officeCode] AS 'OfficeCode',
		--Amt_total
		(
				SELECT 
				SUM(_acc.principalAmount)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND _acc.productType IN ('IPP', 'PP/SC', 'RTA', 'SNS')
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
		) AS 'Amt_Total',
		(
			--Unit_IPP
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND _acc.productType = 'IPP'
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
			*
			(
				SELECT DISTINCT
					( rt.ipp)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			) 
		) AS 'Sf_IPP',
		(
			--Unit_PP_SC
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND _acc.productType = 'PP/SC'
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
			*
			(
				SELECT DISTINCT
					( rt.pp_sc)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
		) AS 'Sf_PP_SC',
		(
			--Unit_RTA
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND _acc.productType = 'RTA'
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
			*
			(
				SELECT DISTINCT
					( rt.rta)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
		) AS  'Sf_RTA',
		(
			--Unit_SNS
			(
				SELECT 
				SUM(_acc.unit)
				FROM [dbo].[Acceptance] _acc
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND _acc.productType = 'SNS'
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
			*
			(
				SELECT DISTINCT
					( rt.sns)
				FROM [dbo].[Acceptance] _acc
				INNER JOIN [dbo].[Customer] cust on _acc.origin_agent_name = cust.[name]
				INNER JOIN [dbo].[Rate] rt on cust.rateCardId = rt.pkid
				WHERE _acc.transactionDate >= @DateAsOf
				AND _acc.origin_agent_name = c.[name]
				AND (_acc.isDeleted=0 or _acc.isDeleted='' or _acc.isDeleted is null) 
			)
		) AS 'Sf_SNS',
		(
			SELECT 
			(pm.PHPPayment * -1)
			FROM [dbo].[Payment] pm
			WHERE pm.[date] >= @DateAsOf
			AND pm.origin_agent_name = c.[name]
		) AS 'Settlement'
		
	FROM [dbo].[Acceptance] acc
	INNER JOIN [dbo].[Refund] ref on acc.origin_agent_name = ref.origin_agent_name
	INNER JOIN [dbo].[Voided] vd on acc.origin_agent_name = vd.origin_agent_name
	INNER JOIN [dbo].[Payment] pm on acc.origin_agent_name = pm.origin_agent_name
	INNER JOIN [dbo].[Customer] c on acc.origin_agent_name = c.[name]
	WHERE [acc].[transactionDate] >= @DateAsOf
	AND ([acc].[isDeleted]=0 or [acc].[isDeleted]='' or [acc].[isDeleted] is null) 
	GROUP BY [c].[name], [c].[officeCode]
GO

PRINT 'Creating...pc_lst_agentbalances...complete'
GO