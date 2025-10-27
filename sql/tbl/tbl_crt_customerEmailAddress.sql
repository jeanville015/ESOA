IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerEmailAddress]'))
BEGIN
	ALTER TABLE [dbo].[CustomerEmailAddress]
		DROP CONSTRAINT [PK_esoa.CustomerEmailAddress];
	DROP TABLE [dbo].[CustomerEmailAddress]
END

CREATE TABLE [CustomerEmailAddress](
	[pkid] [uniqueidentifier] NOT NULL,
	[customerId] [uniqueidentifier] NOT NULL, 
	[emailAddress] [varchar](max) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.CustomerEmailAddress] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_CustomerEmailAddress_Customer]'))
BEGIN
	ALTER TABLE [dbo].[CustomerEmailAddress] 
	DROP CONSTRAINT [FK_CustomerEmailAddress_Customer];
END
ALTER TABLE [CustomerEmailAddress]  WITH CHECK ADD  CONSTRAINT [FK_CustomerEmailAddress_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([pkid])
GO