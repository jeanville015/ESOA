IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_customercontactperson'))
begin
  PRINT 'Dropping...pc_get_customercontactperson'
  DROP PROCEDURE pc_get_customercontactperson
end
PRINT 'Creating...pc_get_customercontactperson'
GO 

CREATE procedure pc_get_customercontactperson
(
  @pkid uniqueidentifier=null
)
as

begin try
	select 
		[pkid],
		[customerId],
		[contactPerson],
		[designation],
		[created],
		[updated],
		[updatedBy]
	from [dbo].[CustomerContactPerson] 
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


PRINT 'Creating...pc_get_customercontactperson...complete'
go