IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_voided'))
begin
  PRINT 'Dropping...pc_ins_voided'
  DROP PROCEDURE pc_ins_voided
end
PRINT 'Creating...pc_ins_voided'
GO

create procedure pc_ins_voided
(
  @origin_agent_name varchar(150)
 ,@officeCode		 varchar(50)
 ,@date				 varchar(50)
 ,@productType		 varchar(50)
 ,@trackingNumber	 varchar(150)
 ,@referenceNumber	 varchar(150)
 ,@entBranch		 varchar(150)
 ,@shipperName		 varchar(150)
 ,@consigneeName	 varchar(150)
 ,@unit				 integer
 ,@principalAmount	 decimal(18,0)
 ,@serviceFee		 decimal(18,0)
 ,@refundDate		 varchar(50)
 ,@statusCode		 varchar(50)
 ,@statusDescription varchar(50)
 ,@encashmentBranchHub	varchar(150)
 ,@pkid					int  = null        output
)
as 
begin transaction 
	begin try 
		INSERT INTO dbo.Voided( 
			 [origin_agent_name] 
			,[officeCode]
			,[date]
			,[productType]
			,[trackingNumber]
			,[referenceNumber]
			,[entBranch]
			,[shipperName]
			,[consigneeName]
			,[unit]
			,[principalAmount]
			,[serviceFee]
			,[refundDate]
			,[statusCode]
			,[statusDescription]
			,[encashmentBranchHub]
			)
		VALUES (
			--@pkid
			 @origin_agent_name
			,@officeCode
			,@date
			,@productType
			,@trackingNumber
			,@referenceNumber
			,@entBranch
			,@shipperName
			,@consigneeName
			,@unit
			,@principalAmount
			,@serviceFee
			,@refundDate
			,@statusCode
			,@statusDescription
			,@encashmentBranchHub
			)
		set @pkid = IDENT_CURRENT('dbo.Voided')
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

PRINT 'Creating...pc_ins_voided...complete'
go