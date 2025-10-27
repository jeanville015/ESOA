IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pc_get_useraccount'))
begin
  PRINT 'Dropping...pc_get_useraccount'
  DROP PROCEDURE pc_get_useraccount
end
PRINT 'Creating...pc_get_useraccount'
GO 

CREATE procedure pc_get_useraccount
(
  @pkid uniqueidentifier=null, 
  @email varchar(150)=null,
  @password varchar(150)=null,
  @checkEmail varchar(150)=null
)
as

begin try 

	--password
	declare @passNoConvert nvarchar(150);
	SET @passNoConvert = @password  -- for raw data
	SET @password = (Select CONVERT(VARCHAR(50), HashBytes('MD5', @password), 2) as MD5Hash)

	select 
		[pkid],
		[name],
		[jobTitle],
		[team],
		[role],
		[moduleAccess_admin],
		[moduleAccess_soa],
		[moduleAccess_payment],
		[moduleAccess_reports],
		[moduleAccess_granular],
		[accessRights_admin],
		[accessRights_soa],
		[accessRights_payment],
		[accessRights_reports],
		[accessRights_granular],
		[emailAddress],
		[contactNo],
		[created],
		[updated],
		[updatedBy]
	from dbo.UserAccount 
	where 
		(
			(	
				--logic for getting the details of user
				(pkid = @pkid) 
				AND (@email IS NULL AND @password IS NULL)
				AND (@checkEmail IS NULL)
				AND [isDeleted] = 0
			)
			OR 
			(
				--logic for login validation
				([emailAddress] = @email AND [password] = @password)
				AND (@pkid IS NULL)
				AND (@checkEmail IS NULL)
				AND [isDeleted] = 0
			)
			OR
			(
				--logic for reset password validation
				([emailAddress] = @checkEmail) 
				AND (@pkid IS NULL)
				AND (@email IS NULL AND @password IS NULL) 
				AND [isDeleted] = 0
			) 
		);
end try

begin catch
  declare @msg nvarchar(max) = ERROR_MESSAGE()
  raiserror(@msg, 11, 1)

end catch
go


PRINT 'Creating...pc_get_useraccounts...complete'
go