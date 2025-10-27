IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_rate_paging'))
begin
  PRINT 'Dropping...pc_lst_rate_paging'
  DROP PROCEDURE pc_lst_rate_paging
end
PRINT 'Creating...pc_lst_rate_paging'
GO

create procedure pc_lst_rate_paging
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
		[r].[pkid]
		,Row_Number() OVER (
			ORDER BY CASE @sortdirection
					WHEN 'ASC'
						THEN CASE @sortindex
								WHEN 0
									THEN [r].[reference]
								--WHEN 1
								--	THEN [r].[rateType]
								--WHEN 2
								--	THEN [r].[ipp]
								--WHEN 3
								--	THEN [r].[pp_sc]
								--WHEN 4
								--	THEN [r].[rta]
								--WHEN 5
								--	THEN [r].[sns]
								--WHEN 6
								--	THEN [r].[ippx]
								--WHEN 2
								--	THEN [r].[from]
								--WHEN 3
								--	THEN [r].[to]
								END
					END ASC
				,CASE @sortdirection
					WHEN 'DESC'
						THEN CASE @sortindex
								WHEN 0
									THEN [r].[reference]
								--WHEN 1
								--	THEN [r].[rateType]
								--WHEN 2
								--	THEN [r].[ipp]
								--WHEN 3
								--	THEN [r].[pp_sc]
								--WHEN 4
								--	THEN [r].[rta]
								--WHEN 5
								--	THEN [r].[sns]
								--WHEN 6
								--	THEN [r].[ippx]
								--WHEN 2
								--	THEN [r].[from]
								--WHEN 3
								--	THEN [r].[to]
								END
					END DESC
			) AS RowNum
		FROM [Rate] [r] 
		WHERE (@FilterTerm IS NULL 
					OR  [r].[reference] LIKE @FilterTerm
					OR  [r].[ipp] LIKE @FilterTerm
					OR  [r].[pp_sc] LIKE @FilterTerm
					OR  [r].[rta] LIKE @FilterTerm
					OR  [r].[sns] LIKE @FilterTerm
					OR  [r].[ippx] LIKE @FilterTerm
					OR  [r].[from] LIKE @FilterTerm
					OR  [r].[to] LIKE @FilterTerm
					)
			AND ([r].[isDeleted] = 0)
	SELECT @TotalRows = count(src.pkid)
								FROM @search src

	SELECT	[r].[pkid],
			[r].[reference],
			[r].[rateType_ipp],
			[r].[rateType_pp_sc],
			[r].[rateType_rta],
			[r].[rateType_sns],
			[r].[rateType_ippx],
			[r].[ipp],
			[r].[pp_sc],
			[r].[rta],
			[r].[sns],
			[r].[ippx],
			[r].[from],
			[r].[to]
	FROM @search  [src]
		INNER JOIN [Rate] [r] on [src].[pkid] = [r].[pkid] 
	WHERE (src.RowNum BETWEEN @StartRowNum AND @EndRowNum)
		AND ([r].[isDeleted] = 0)

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

PRINT 'Creating...pc_lst_rate_paging...complete'
go