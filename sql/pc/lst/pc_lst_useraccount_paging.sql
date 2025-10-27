IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_lst_useraccount_paging'))
begin
  PRINT 'Dropping...pc_lst_useraccount_paging'
  DROP PROCEDURE pc_lst_useraccount_paging
end
PRINT 'Creating...pc_lst_useraccount_paging'
GO

create procedure pc_lst_useraccount_paging
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
		[ua].[pkid]
		,Row_Number() OVER (
			ORDER BY CASE @sortdirection
					WHEN 'ASC'
						THEN CASE @sortindex
								WHEN 0
									THEN [ua].[name]
								WHEN 1
									THEN [ua].[jobTitle]
								WHEN 2
									THEN [ua].[team]
								WHEN 3
									THEN [ua].[role]
								WHEN 4
									THEN [ua].[emailAddress]
								WHEN 5
									THEN [ua].[contactNo]
								END
					END ASC
				,CASE @sortdirection
					WHEN 'DESC'
						THEN CASE @sortindex
								WHEN 0
									THEN [ua].[name]
								WHEN 1
									THEN [ua].[jobTitle]
								WHEN 2
									THEN [ua].[team]
								WHEN 3
									THEN [ua].[role]
								WHEN 4
									THEN [ua].[emailAddress]
								WHEN 5
									THEN [ua].[contactNo]
								END
					END DESC
			) AS RowNum
		FROM [UserAccount] [ua] 
		WHERE (@FilterTerm IS NULL 
					OR  [ua].[name] LIKE @FilterTerm
					OR  [ua].[jobTitle] LIKE @FilterTerm 
					OR  [ua].[team] LIKE @FilterTerm
					OR  [ua].[role] LIKE @FilterTerm
					OR  [ua].[emailAddress] LIKE @FilterTerm
					OR  [ua].[contactNo] LIKE @FilterTerm
					)
			AND ([ua].[isDeleted] = 0)
	SELECT @TotalRows = count(src.pkid)
								FROM @search src

	SELECT	[ua].[pkid],
			[ua].[name],
			[ua].[jobTitle],
			[ua].[team],
			[ua].[role],
			[ua].[moduleAccess_admin],
			[ua].[moduleAccess_soa],
			[ua].[moduleAccess_payment],
			[ua].[moduleAccess_reports],
			[ua].[moduleAccess_granular],
			[ua].[accessRights_admin],
			[ua].[accessRights_soa],
			[ua].[accessRights_payment],
			[ua].[accessRights_reports],
			[ua].[accessRights_granular],
			[ua].[emailAddress],
			[ua].[contactNo],
			[ua].[created],
			[ua].[updated],
			[ua_updated].[name] as [updatedBy_Name]
	FROM @search  [src]
		INNER JOIN [UserAccount] [ua] on [src].[pkid] = [ua].[pkid]
		INNER JOIN [UserAccount] [ua_updated] on [ua].[updatedBy] = [ua_updated].[pkid]
	WHERE (src.RowNum BETWEEN @StartRowNum AND @EndRowNum)
		AND ([ua].[isDeleted] = 0)

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

PRINT 'pc_lst_useraccount_paging...complete'
go