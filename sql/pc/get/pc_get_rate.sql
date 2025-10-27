IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_rate'))
begin
  PRINT 'Dropping...pc_get_rate'
  DROP PROCEDURE pc_get_rate
end
PRINT 'Creating...pc_get_rate'
GO 

CREATE procedure pc_get_rate
(
  @pkid uniqueidentifier=null
)
as

begin try
	select 
		[pkid],
		[reference],
		[rateType_ipp],
		[rateType_pp_sc],
		[rateType_rta],
		[rateType_sns],
		[rateType_ippx],
		[ipp],
		[pp_sc],
		[rta],
		[sns],
		[ippx],
		[from],
		[to],
		[created],
		[updated],
		[updatedBy]
	from [dbo].[Rate] 
	where 
		(
			(
				[pkid] = @pkid
			)
			AND
			(
				[isDeleted] = 0
			)
		);
end try

begin catch
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)

end catch
go

PRINT 'Creating...pc_get_rate...complete'
go