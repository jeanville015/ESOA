IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserAccount]'))
BEGIN
	ALTER TABLE [dbo].[UserAccount]
		DROP CONSTRAINT [PK_esoa.UserAccount];
	DROP TABLE [dbo].[UserAccount];
END

CREATE TABLE [UserAccount](
	[pkid] [uniqueidentifier] NOT NULL, 
	[name] [varchar](150) NULL,
	[jobTitle] [varchar](150) NULL,
	[team] [varchar](150) NULL,
	[role] [varchar](150) NULL,
	[moduleAccess_admin] [bit] NULL,
	[moduleAccess_soa] [bit] NULL,
	[moduleAccess_payment] [bit] NULL,
	[moduleAccess_reports] [bit] NULL,
	[moduleAccess_granular] [bit] NULL,
	[accessRights_admin] [bit] NULL,
	[accessRights_soa] [bit] NULL,
	[accessRights_payment] [bit] NULL,
	[accessRights_reports] [bit] NULL,
	[accessRights_granular] [bit] NULL,
	[emailAddress] [varchar](150) NULL,
	[contactNo] [varchar](150) NULL,
	[password] [varchar](150) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.UserAccount] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- put all table update scripts here also
-- 
--ALTER TABLE [dbo].[UserAccount]
--ADD [_AccessRights_admin] [nvarchar](50) NULL 
--ALTER TABLE [dbo].[UserAccount]
--ADD [_AccessRights_soa] [nvarchar](50) NULL 
--ALTER TABLE [dbo].[UserAccount]
--ADD [_AccessRights_payment] [nvarchar](50) NULL 
--ALTER TABLE [dbo].[UserAccount]
--ADD [_AccessRights_reports] [nvarchar](50) NULL 
--ALTER TABLE [dbo].[UserAccount]
--ADD [_AccessRights_granular] [nvarchar](50) NULL 
