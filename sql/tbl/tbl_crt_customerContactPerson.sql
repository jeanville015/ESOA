IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerContactPerson]'))
BEGIN
	ALTER TABLE [dbo].[CustomerContactPerson]
		DROP CONSTRAINT [PK_esoa.CustomerContactPerson];
	DROP TABLE [dbo].[CustomerContactPerson]
END

CREATE TABLE [CustomerContactPerson](
	[pkid] [uniqueidentifier] NOT NULL,
	[customerId] [uniqueidentifier] NOT NULL, 
	[contactPerson] [varchar](max) NULL,
	[designation] [varchar](max) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.CustomerContactPerson] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_CustomerContactPerson_Customer]'))
BEGIN
	ALTER TABLE [dbo].[CustomerContactPerson] 
	DROP CONSTRAINT [FK_CustomerContactPerson_Customer];
END
ALTER TABLE [CustomerContactPerson]  WITH CHECK ADD  CONSTRAINT [FK_CustomerContactPerson_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([pkid])
GO