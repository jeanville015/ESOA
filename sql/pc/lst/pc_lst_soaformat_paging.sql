IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_soaformat_paging'))
begin
  PRINT 'Dropping...pc_lst_soaformat_paging'
  DROP PROCEDURE pc_lst_soaformat_paging
end
PRINT 'Creating...pc_lst_soaformat_paging'
GO

create procedure pc_lst_soaformat_paging
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
		[sf].[pkid]
		,Row_Number() OVER (
			ORDER BY CASE @sortdirection
					WHEN 'ASC'
						THEN CASE @sortindex
								WHEN 0
									THEN [sf].[formatName]
								END
					END ASC
				,CASE @sortdirection
					WHEN 'DESC'
						THEN CASE @sortindex
								WHEN 0
									THEN [sf].[formatName]
								END
					END DESC
			) AS RowNum
		FROM [SOAFormat] [sf] 
		WHERE (@FilterTerm IS NULL 
					OR  [sf].[formatName] LIKE @FilterTerm
					)
			AND ([sf].[isDeleted] = 0)
	SELECT @TotalRows = count(src.pkid)
								FROM @search src

	SELECT	
		[sf].[pkid],
		[sf].[formatName]
	FROM @search  [src]
		INNER JOIN [SOAFormat] [sf]  on [src].[pkid] = [sf].[pkid] 
	WHERE (src.RowNum BETWEEN @StartRowNum AND @EndRowNum)
		AND ([sf].[isDeleted] = 0)

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

PRINT 'Creating...pc_lst_soaformat_paging...complete'
go