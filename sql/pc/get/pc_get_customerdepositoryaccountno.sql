IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_customerdepositoryaccountno'))
begin
  PRINT 'Dropping...pc_get_customerdepositoryaccountno'
  DROP PROCEDURE pc_get_customerdepositoryaccountno
end
PRINT 'Creating...pc_get_customerdepositoryaccountno'
GO 

CREATE procedure pc_get_customerdepositoryaccountno
(
  @pkid uniqueidentifier=null
)
as

begin try
	select 
		[pkid],
		[customerId],
		[depositoryAccountNo],
		[created],
		[updated],
		[updatedBy]
	from [dbo].[CustomerDepositoryAccountNo] 
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


PRINT 'Creating...pc_get_customerdepositoryaccountno...complete'
go