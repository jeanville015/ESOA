IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_customerdepositorybankaccount'))
begin
  PRINT 'Dropping...pc_get_customerdepositorybankaccount'
  DROP PROCEDURE pc_get_customerdepositorybankaccount
end
PRINT 'Creating...pc_get_customerdepositorybankaccount'
GO 

CREATE procedure pc_get_customerdepositorybankaccount
(
  @pkid uniqueidentifier=null
)
as

begin try
	select 
		[pkid],
		[accountNo],
		[bankName]
	from [dbo].[CustomerDipositoryBankAccount] 
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

PRINT 'Creating...pc_get_customerdepositorybankaccount...complete'
go