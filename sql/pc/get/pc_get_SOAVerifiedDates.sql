IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_SOAVerifiedDates'))
begin
  PRINT 'Dropping...pc_get_SOAVerifiedDates'
  DROP PROCEDURE pc_get_SOAVerifiedDates
end
PRINT 'Creating...pc_get_SOAVerifiedDates'
GO 

CREATE procedure dbo.pc_get_SOAVerifiedDates
(
  @officeCode			varchar(50)
 ,@transactionDate		varchar(50)
)
as

begin try  

	select
		[pkid],
		[officeCode],
		[transactionDate],
		[status],
		[remarks]
	from dbo.SOAVerifiedDates 
	where  transactionDate = @transactionDate
	and officeCode = @officeCode
end try

begin catch
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)

end catch

PRINT 'Creating...pc_get_SOAVerifiedDates...complete'
go