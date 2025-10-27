IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_del_SOAVerifiedDates'))
begin
  PRINT 'Dropping...pc_del_SOAVerifiedDates'
  DROP PROCEDURE pc_del_SOAVerifiedDates
end
PRINT 'Creating...pc_del_SOAVerifiedDates'
GO 
CREATE procedure pc_del_SOAVerifiedDates
(
    @officeCode			varchar(50)
    ,@transactionDate	varchar(50)
)
as

begin try 

	delete from	dbo.[SOAVerifiedDates] 
    where transactionDate = @transactionDate
    and officeCode = @officeCode

end try
begin catch
 
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)

end catch


go

PRINT 'Creating...pc_del_SOAVerifiedDates...complete'
go