IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_encashment'))
begin
  PRINT 'Dropping...pc_ins_encashment'
  DROP PROCEDURE pc_ins_encashment
end
PRINT 'Creating...pc_ins_encashment'
GO

create procedure pc_ins_encashment
(
  @origin_agent_name varchar(150)
 ,@tagging			 varchar(50)
 ,@officeCode		 varchar(50)
 ,@transactionDate	 varchar(50)
 ,@productType		 varchar(50)
 ,@trackingNumber	 varchar(150)
 ,@referenceNumber	 varchar(150)
 ,@encashmentBranch	 varchar(150)
 ,@shipperName		 varchar(150)
 ,@consigneeName	 varchar(150)
 ,@unit				 integer
 ,@principalAmount	 decimal(18,0)
 ,@encashmentDate	 varchar(50)
 ,@statusCode		 varchar(50)
 ,@statusDescription varchar(50)
 ,@encashmentBranchHub	varchar(150)
 ,@pkid					int  = null        output
)
as 
begin transaction 
	begin try 
		INSERT INTO dbo.Encashment ( 
			 [origin_agent_name]
			,[tagging]
			,[officeCode]
			,[transactionDate]
			,[productType]
			,[trackingNumber]
			,[referenceNumber]
			,[encashmentBranch]
			,[shipperName]
			,[consigneeName]
			,[unit]
			,[principalAmount]
			,[encashmentDate]
			,[statusCode]
			,[statusDescription]
			,[encashmentBranchHub]
			)
		VALUES (
			--@pkid
			 @origin_agent_name
			,@tagging
			,@officeCode
			,@transactionDate
			,@productType
			,@trackingNumber
			,@referenceNumber
			,@encashmentBranch
			,@shipperName
			,@consigneeName
			,@unit
			,@principalAmount
			,@encashmentDate
			,@statusCode
			,@statusDescription
			,@encashmentBranchHub
			)
		set @pkid = IDENT_CURRENT('dbo.Encashment')
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

PRINT 'Creating...pc_ins_encashment...complete'
go