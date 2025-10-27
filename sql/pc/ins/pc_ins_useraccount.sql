IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_ins_useraccount'))
begin
  PRINT 'Dropping...pc_ins_useraccount'
  DROP PROCEDURE pc_ins_useraccount
end
PRINT 'Creating...pc_ins_useraccount'
GO

create procedure pc_ins_useraccount
(
  @name					 varchar(150)		= null
 ,@jobTitle				 varchar(150)		= null
 ,@team					 varchar(150)		= null
 ,@role					 varchar(150)		= null
 ,@moduleAccess_admin	 bit				= null
 ,@moduleAccess_soa		 bit				= null
 ,@moduleAccess_payment	 bit				= null
 ,@moduleAccess_reports	 bit				= null
 ,@moduleAccess_granular bit				= null
 ,@accessRights_admin	 bit				= null
 ,@accessRights_soa		 bit				= null
 ,@accessRights_payment	 bit				= null
 ,@accessRights_reports	 bit				= null
 ,@accessRights_granular bit				= null
 ,@emailAddress			 varchar(150)		= null
 ,@contactNo			 varchar(150)		= null
 ,@password				 varchar(150)		= null
 ,@userAccountId		uniqueidentifier	= null
 ,@pkid					uniqueidentifier	= null	output
)
as 
begin transaction 
	begin try

		--password
		declare @passNoConvert nvarchar(150);
		SET @passNoConvert = @password  -- for raw data
		SET @password = (Select CONVERT(VARCHAR(50), HashBytes('MD5', @password), 2) as MD5Hash)

		set @pkid = newid()
		INSERT INTO dbo.UserAccount (
			[pkid]
			,[name]
			,[jobTitle]
			,[team]
			,[role]
			,[moduleAccess_admin]
			,[moduleAccess_soa]
			,[moduleAccess_payment]
			,[moduleAccess_reports]
			,[moduleAccess_granular]
			,[accessRights_admin]
			,[accessRights_soa]
			,[accessRights_payment]
			,[accessRights_reports]
			,[accessRights_granular]
			,[emailAddress]
			,[contactNo]
			,[password]
			,[created]
			,[updated]
			,[updatedBy]
			,[isDeleted]
			)
		VALUES (
			 @pkid
			,@name
			,@jobTitle
			,@team
			,@role
			,@moduleAccess_admin
			,@moduleAccess_soa
			,@moduleAccess_payment
			,@moduleAccess_reports
			,@moduleAccess_granular
			,@accessRights_admin
			,@accessRights_soa
			,@accessRights_payment
			,@accessRights_reports
			,@accessRights_granular
			,@emailAddress
			,@contactNo
			,@password
			,GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'China Standard Time'
			,GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'China Standard Time'
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

PRINT 'Creating...pc_ins_useraccount...complete'
go
