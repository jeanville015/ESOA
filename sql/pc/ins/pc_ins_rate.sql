IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_rate'))
begin
  PRINT 'Dropping...pc_ins_rate'
  DROP PROCEDURE pc_ins_rate
end
PRINT 'Creating...pc_ins_rate'
GO

create procedure pc_ins_rate
(
  @reference			varchar(150)
 ,@rateType_ipp			varchar(50)
 ,@rateType_pp_sc		varchar(50)
 ,@rateType_rta			varchar(50)
 ,@rateType_sns			varchar(50)
 ,@rateType_ippx		varchar(50)
 ,@ipp					decimal
 ,@pp_sc				decimal
 ,@rta					decimal
 ,@sns					decimal
 ,@ippx					decimal
 ,@from					date
 ,@to					date
 ,@userAccountId		uniqueidentifier=null
 ,@pkid					uniqueidentifier  = null        output
)
as 
begin transaction 
	begin try
		set @pkid = newid()
		INSERT INTO dbo.Rate (
			[pkid]
			,[reference]
			,[rateType_ipp]
			,[rateType_pp_sc]
			,[rateType_rta]
			,[rateType_sns]
			,[rateType_ippx]
			,[ipp]
			,[pp_sc]
			,[rta]
			,[sns]
			,[ippx]
			,[from]
			,[to]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			@pkid
			,@reference
			,@rateType_ipp
			,@rateType_pp_sc
			,@rateType_rta
			,@rateType_sns
			,@rateType_ippx
			,@ipp
			,@pp_sc
			,@rta
			,@sns
			,@ippx
			,@from
			,@to
			,getdate()
			,getdate()
			,@userAccountId
			,0
			)
	end try
begin catch
  if (@@trancount > 0 ) rollback tran
  declare @msg nvarchar(max) = error_message()
  declare @state int = error_state()
  raiserror(@msg, 11, @state) 
end catch
if (@@trancount > 0 ) commit transaction
go

PRINT 'Creating...pc_ins_rate...complete'
go