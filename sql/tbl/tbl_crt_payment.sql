IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payment]'))
BEGIN
	ALTER TABLE [dbo].[Payment]
		DROP CONSTRAINT [PK_esoa.Payment];
	DROP TABLE [dbo].[Payment];
END

CREATE TABLE [Payment](
	[pkid] [int]IDENTITY(1,1) NOT NULL ,
	[uploadedBy] [varchar](50) NULL,
	[date] [varchar](50) NULL,
	[origin_agent_name] [varchar](150) NULL,
	[customerId] [varchar](50) NULL,
	[bankAccount] [varchar](50) NULL,
	[bankAccountGLCode] [varchar](50) NULL,
	[USDPayment] [decimal](18,0) NULL,
	[excRate] [decimal](18,0) NULL,
	[PHPPayment] [decimal](18,0) NULL,
	[assignment] [varchar](150) NULL,
	[text] [varchar](150) NULL,
	[SAPDocNumber] [varchar](150) NULL,
 CONSTRAINT [PK_esoa.Payment] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--updated in above scripts
--EXEC sp_RENAME 'Payment.customerName' , 'origin_agent_name', 'COLUMN'