IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerDipositoryBankAccount]'))
BEGIN
	ALTER TABLE [dbo].[CustomerDipositoryBankAccount]
		DROP CONSTRAINT [PK_esoa.CustomerDipositoryBankAccount];
	DROP TABLE [dbo].[CustomerDipositoryBankAccount];
END

CREATE TABLE [CustomerDipositoryBankAccount](
	[pkid] [uniqueidentifier] NOT NULL,
	[accountNo] [varchar](250) NULL,
	[bankName] [varchar](150) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.CustomerDipositoryBankAccount] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO 


--updated in above scripts
--EXEC sp_RENAME 'Acceptance.customerName' , 'origin_agent_name', 'COLUMN'