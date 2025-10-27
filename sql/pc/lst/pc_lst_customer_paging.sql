IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_customer_paging'))
begin
  PRINT 'Dropping...pc_lst_customer_paging'
  DROP PROCEDURE pc_lst_customer_paging
end
PRINT 'Creating...pc_lst_customer_paging'
GO

create procedure pc_lst_customer_paging
(
	@FilterTerm        nvarchar(250) = NULL      -- parameter to search all columns by, 
	,@SortIndex         int           = 0         -- a zero based index to indicate which column to order by
	,@SortDirection     nvarchar(4)   = 'ASC'     -- the direction to sort in, either ASC or DESC
	,@StartRowNum       INT           = 1         -- the first row to return
	,@EndRowNum         INT           = 10        -- the last row to return
	,@TotalRows         int           = 0 output
	,@FilteredRowsCount int           = 0 output

)
As

begin try 
	declare @search table (pkid uniqueidentifier, rowNum int) 

    insert into @search
    SELECT DISTINCT 
		[c].[pkid]
		,Row_Number() OVER (
			ORDER BY CASE @sortdirection
					WHEN 'ASC'
						THEN CASE @sortindex
								WHEN 0
									THEN [c].[name]
								WHEN 1
									THEN [c].[legalEntityName]
								WHEN 2
									THEN [c].[tin]
								WHEN 3
									THEN [c].[address]
								WHEN 4
									THEN [c].[salesExec_LBC]
								WHEN 5
									THEN CONVERT(varchar, [c].[approvedAFC])
								WHEN 6
									THEN [sf].[formatName]
								WHEN 7
									THEN [r].[reference]
								WHEN 8
									THEN [c].[domestic_intl]
								WHEN 9
									THEN [c].[country]
								WHEN 10
									THEN [c].[transmissionMode]
								WHEN 11
									THEN [c].[officeCode]
								WHEN 12
									THEN [c].[area]
								WHEN 13
									THEN CONVERT(varchar, [c].[sapCustomerId])
								WHEN 14
									THEN CONVERT(varchar, [c].[sapVendorId_ipp])
								WHEN 15
									THEN CONVERT(varchar, [c].[sapVendorId_pp_sc])
								WHEN 16
									THEN CONVERT(varchar, [c].[sapVendorId_rta])
								WHEN 17
									THEN CONVERT(varchar, [c].[sapVendorId_sns])
								WHEN 18
									THEN CONVERT(varchar, [c].[sapVendorId_ippx])
								WHEN 19
									THEN [c].[paymentCurrency]
								WHEN 20
									THEN [c].[SFModeOfSettlement]
								WHEN 21
									THEN [c].[withholdingTax]
								WHEN 22
									THEN [c].[vatStatus]
								WHEN 23
									THEN [c].[status]
								END
					END ASC
				,CASE @sortdirection
					WHEN 'DESC'
						THEN CASE @sortindex
								WHEN 0
									THEN [c].[name]
								WHEN 1
									THEN [c].[legalEntityName]
								WHEN 2
									THEN [c].[tin]
								WHEN 3
									THEN [c].[address]
								WHEN 4
									THEN [c].[salesExec_LBC]
								WHEN 5
									THEN CONVERT(varchar, [c].[approvedAFC])
								WHEN 6
									THEN [sf].[formatName]
								WHEN 7
									THEN [r].[reference]
								WHEN 8
									THEN [c].[domestic_intl]
								WHEN 9
									THEN [c].[country]
								WHEN 10
									THEN [c].[transmissionMode]
								WHEN 11
									THEN [c].[officeCode]
								WHEN 12
									THEN [c].[area]
								WHEN 13
									THEN CONVERT(varchar, [c].[sapCustomerId])
								WHEN 14
									THEN CONVERT(varchar, [c].[sapVendorId_ipp])
								WHEN 15
									THEN CONVERT(varchar, [c].[sapVendorId_pp_sc])
								WHEN 16
									THEN CONVERT(varchar, [c].[sapVendorId_rta])
								WHEN 17
									THEN CONVERT(varchar, [c].[sapVendorId_sns])
								WHEN 18
									THEN CONVERT(varchar, [c].[sapVendorId_ippx])
								WHEN 19
									THEN [c].[paymentCurrency]
								WHEN 20
									THEN [c].[SFModeOfSettlement]
								WHEN 21
									THEN [c].[withholdingTax]
								WHEN 22
									THEN [c].[vatStatus]
								WHEN 23
									THEN [c].[status]
								END
					END DESC
			) AS RowNum
		FROM [Customer] c
			INNER JOIN [SOAFormat] sf on [c].[soaFormatId] = [sf].[pkid]
			INNER JOIN [Rate] r on [c].[rateCardId] = [r].[pkid]
		WHERE (@FilterTerm IS NULL 
					OR  [c].[name] LIKE @FilterTerm
					OR  [c].[legalEntityName] LIKE @FilterTerm 
					OR  [c].[tin] LIKE @FilterTerm
					OR  [c].[address] LIKE @FilterTerm
					OR  [c].[salesExec_LBC] LIKE @FilterTerm
					OR  [c].[approvedAFC] LIKE @FilterTerm
					OR  [sf].[formatName] LIKE @FilterTerm
					OR  [r].[reference] LIKE @FilterTerm
					OR  [c].[domestic_intl] LIKE @FilterTerm
					OR  [c].[country] LIKE @FilterTerm
					OR  [c].[transmissionMode] LIKE @FilterTerm
					OR  [c].[officeCode] LIKE @FilterTerm
					OR  [c].[area] LIKE @FilterTerm
					OR  [c].[sapCustomerId] LIKE @FilterTerm
					OR  [c].[sapVendorId_ipp] LIKE @FilterTerm
					OR  [c].[sapVendorId_pp_sc] LIKE @FilterTerm
					OR  [c].[sapVendorId_rta] LIKE @FilterTerm
					OR  [c].[sapVendorId_sns] LIKE @FilterTerm
					OR  [c].[sapVendorId_ippx] LIKE @FilterTerm
					OR  [c].[paymentCurrency] LIKE @FilterTerm
					OR  [c].[SFModeOfSettlement] LIKE @FilterTerm
					OR  [c].[withholdingTax] LIKE @FilterTerm
					OR  [c].[vatStatus] LIKE @FilterTerm
					OR  [c].[status] LIKE @FilterTerm
					)
		AND ([C].[isDeleted] = 0)
	SELECT @TotalRows = count(src.pkid)
								FROM @search src

	SELECT	[c].[pkid],
			[c].[name],
			[c].[legalEntityName],
			[c].[tin],
			[c].[address],
			[c].[salesExec_LBC],
			[c].[approvedAFC],
			[sf].[formatName] as [soaTemplateName],
			[r].[reference] as [rateCardName],
			[c].[domestic_intl],
			[c].[country],
			[c].[transmissionMode],
			[c].[officeCode],
			[c].[area],
			[c].[sapCustomerId],
			[c].[sapVendorId_ipp],
			[c].[sapVendorId_pp_sc],
			[c].[sapVendorId_rta],
			[c].[sapVendorId_sns],
			[c].[sapVendorId_ippx],
			[c].[paymentCurrency],
			[c].[SFModeOfSettlement],
			[c].[withholdingTax],
			[c].[vatStatus],
			[c].[status],
			[c].[created],
			[c].[updated]
	FROM @search  src
		INNER JOIN [Customer] c on src.pkid = c.pkid 
		INNER JOIN [SOAFormat] sf on [c].[soaFormatId] = [sf].[pkid]
		INNER JOIN [Rate] r on [c].[rateCardId] = [r].[pkid]
	WHERE (src.RowNum BETWEEN @StartRowNum AND @EndRowNum)
		AND ([C].[isDeleted] = 0)

	select @FilteredRowsCount = COUNT(*) 
	from   @search	 src
		WHERE src.RowNum BETWEEN @StartRowNum
									AND @EndRowNum
  
end try
begin catch
 
  if (@@trancount > 0 ) rollback tran
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)

end catch


go

PRINT 'Creating...pc_lst_customer_paging...complete'
go