IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerDepositoryAccountNo]'))
BEGIN
	ALTER TABLE [dbo].[CustomerDepositoryAccountNo]
		DROP CONSTRAINT [PK_esoa.CustomerDepositoryAccountNo];
	DROP TABLE [dbo].[CustomerDepositoryAccountNo]
END

CREATE TABLE [CustomerDepositoryAccountNo](
	[pkid] [uniqueidentifier] NOT NULL,
	[customerId] [uniqueidentifier] NOT NULL, 
	[depositoryAccountNo] [varchar](150) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.CustomerDepositoryAccountNo] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_CustomerDepositoryAccountNo_Customer]'))
BEGIN
	ALTER TABLE [dbo].[CustomerDepositoryAccountNo] 
	DROP CONSTRAINT [FK_CustomerDepositoryAccountNo_Customer];
END
ALTER TABLE [CustomerDepositoryAccountNo]  WITH CHECK ADD  CONSTRAINT [FK_CustomerDepositoryAccountNo_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([pkid])
GO