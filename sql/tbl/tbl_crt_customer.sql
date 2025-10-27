IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]'))
BEGIN
	ALTER TABLE [dbo].[Customer]
		DROP CONSTRAINT [PK_esoa.Customer];
	DROP TABLE [dbo].[Customer];
END

CREATE TABLE [Customer](
	[pkid] [uniqueidentifier] NOT NULL, 
	[name] [varchar](150) NULL,
	[legalEntityName] [varchar](max) NULL,
	[tin] [varchar](max) NULL,
	[address] [varchar](max) NULL,
	[salesExec_LBC] [varchar](max) NULL,
	[approvedAFC] [decimal] NULL,
	[soaFormatId] [uniqueidentifier] NULL,
	[rateCardId] [uniqueidentifier] NULL,
	[domestic_intl] [varchar](50),
	[country] [varchar](max) NULL,
	[transmissionMode] [varchar](50) NULL,
	[officeCode] [varchar](max) NULL,
	[area] [varchar](max) NULL,
	[sapCustomerId] [int] NULL,
	[sapVendorId_ipp] [int] NULL,
	[sapVendorId_pp_sc] [int] NULL,
	[sapVendorId_rta] [int] NULL,
	[sapVendorId_sns] [int] NULL,
	[sapVendorId_ippx] [int] NULL,
	[paymentCurrency] [varchar](50) NULL,
	[SFModeOfSettlement] [varchar](150) NULL,
	[withholdingTax] [varchar](50) NULL,
	[vatStatus] [varchar](100) NULL,
	[status] [varchar](100) NULL,
	[depositoryBankAccount] [uniqueidentifier] NULL,
	[beginningBalance] [decimal](18,2) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.Customer] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- put all table update scripts here also 
--ALTER TABLE [dbo].[Customer]
--	ADD	 [salesExec_LBC] [varchar](max) NULL

--ALTER TABLE [dbo].[Customer]
--DROP COLUMN [depositoryBankAccount];
--ALTER TABLE [dbo].[Customer]
--	ADD	 [depositoryBankAccount] [uniqueidentifier] NULL

--ALTER TABLE [dbo].[Customer]
--	DROP COLUMN [beginningBalance];
--ALTER TABLE [dbo].[Customer]
--	ADD	 [beginningBalance] [decimal](18,2) NULL

--alter table [dbo].[Customer]
--alter column [officeCode] varchar(50);

----officeCode
--CREATE NONCLUSTERED INDEX IDX_Customer_officeCode
--ON Customer (officeCode);

----name
--CREATE NONCLUSTERED INDEX IDX_Customer_name
--ON Customer (name);