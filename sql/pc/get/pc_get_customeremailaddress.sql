IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_customeremailaddress'))
begin
  PRINT 'Dropping...pc_get_customeremailaddress'
  DROP PROCEDURE pc_get_customeremailaddress
end
PRINT 'Creating...pc_get_customeremailaddress'
GO 

CREATE procedure pc_get_customeremailaddress
(
  @pkid uniqueidentifier=null
)
as

begin try
	select 
		[pkid],
		[customerId],
		[emailAddress],
		[created],
		[updated],
		[updatedBy]
	from [dbo].[CustomerEmailAddress] 
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


PRINT 'Creating...pc_get_customeremailaddress...complete'
go