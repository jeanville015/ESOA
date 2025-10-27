IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerContactNo]'))
BEGIN
	ALTER TABLE [dbo].[CustomerContactNo]
		DROP CONSTRAINT [PK_esoa.CustomerContactNo];
	DROP TABLE [dbo].[CustomerContactNo]
END

CREATE TABLE CustomerContactNo(
	[pkid] [uniqueidentifier] NOT NULL,
	[customerId] [uniqueidentifier] NOT NULL, 
	[contactNo] [varchar](max) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.CustomerContactNo] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_CustomerContactNo_Customer]'))
BEGIN
	ALTER TABLE [dbo].[CustomerContactNo] 
	DROP CONSTRAINT [FK_CustomerContactNo_Customer];
END
ALTER TABLE [CustomerContactNo]  WITH CHECK ADD  CONSTRAINT [FK_CustomerContactNo_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([pkid])
GO