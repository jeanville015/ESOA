IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Voided]'))
BEGIN
	--ALTER TABLE [dbo].[Voided]
	--	DROP CONSTRAINT [PK_esoa.Voided];
	--DROP TABLE [dbo].[Voided];
END

CREATE TABLE [Voided](
	[pkid] [int]IDENTITY(1,1) NOT NULL ,
	[origin_agent_name] [varchar](150) NULL,
	[officeCode] [varchar](50) NULL,
	[date] [varchar](50) NULL,
	[productType] [varchar](50) NULL,
	[trackingNumber] [varchar](150) NULL,
	[referenceNumber] [varchar](150) NULL,
	[entBranch] [varchar](150) NULL,
	[shipperName] [varchar](150) NULL,
	[consigneeName] [varchar](150) NULL,
	[unit] [integer] NULL,
	[principalAmount] [decimal](18,0) NULL,
	[serviceFee] [decimal](18,0) NULL,
	[refundDate] [varchar](50) NULL,
	[statusCode] [varchar](50) NULL,
	[statusDescription] [varchar](50) NULL,
	[encashmentBranchHub][varchar](150) NULL
 CONSTRAINT [PK_esoa.Voided] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

--updated in above scripts
--EXEC sp_RENAME 'Voided.customerName' , 'origin_agent_name', 'COLUMN'