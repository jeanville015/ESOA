IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_payment'))
begin
  PRINT 'Dropping...pc_ins_payment'
  DROP PROCEDURE pc_ins_payment
end
PRINT 'Creating...pc_ins_payment'
GO

create procedure pc_ins_payment
(
  @uploadedBy			varchar(150)
 ,@date					varchar(50)
 ,@origin_agent_name	varchar(150)
 ,@customerId			varchar(50)
 ,@bankAccount			varchar(50)
 ,@bankAccountGLCode	varchar(50)
 ,@USDPayment			decimal(18,0)
 ,@excRate			decimal(18,0)
 ,@PHPPayment			decimal(18,0)
 ,@assignment			varchar(150)
 ,@text					varchar(150)
 ,@SAPDocNumber			varchar(150)
 ,@pkid					int  = null        output
)
as 
begin transaction 
	begin try 
		INSERT INTO dbo.Payment( 
			 [uploadedBy]
			,[date]
			,[origin_agent_name]
			,[customerId]
			,[bankAccount]
			,[bankAccountGLCode]
			,[USDPayment]
			,[excRate]
			,[PHPPayment]
			,[assignment]
			,[text]
			,[SAPDocNumber]
			)
		VALUES (
			--@pkid
			 @uploadedBy
			,@date
			,@origin_agent_name
			,@customerId
			,@bankAccount
			,@bankAccountGLCode
			,@USDPayment
			,@excRate
			,@PHPPayment
			,@assignment
			,@text
			,@SAPDocNumber
			)
		set @pkid = IDENT_CURRENT('dbo.Payment')
		select @pkid
	end try
begin catch
  if (@@trancount > 0 ) rollback tran
  declare @msg nvarchar(max) = error_message()
  declare @state int = error_state()
  raiserror(@msg, 11, @state) 
end catch
if (@@trancount > 0 ) commit transaction
go

PRINT 'Creating...pc_ins_payment...complete'
go