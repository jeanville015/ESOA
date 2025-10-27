IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_customercontactno'))
begin
  PRINT 'Dropping...pc_get_customercontactno'
  DROP PROCEDURE pc_get_customercontactno
end
PRINT 'Creating...pc_get_customercontactno'
GO 

CREATE procedure pc_get_customercontactno
(
  @pkid uniqueidentifier=null
)
as

begin try
	select 
		[pkid],
		[customerId],
		[contactNo],
		[created],
		[updated],
		[updatedBy]
	from [dbo].[CustomerContactNo] 
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

PRINT 'Creating...pc_get_customercontactno...complete'
go