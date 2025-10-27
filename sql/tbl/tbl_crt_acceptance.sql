IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Acceptance]'))
BEGIN
	ALTER TABLE [dbo].[Acceptance]
		DROP CONSTRAINT [PK_esoa.Acceptance];
	DROP TABLE [dbo].[Acceptance];
END

CREATE TABLE [Acceptance](
	[pkid] [int]IDENTITY(1,1) NOT NULL ,
	[origin_agent_name] [varchar](150) NULL,
	[tagging] [varchar](50) NULL,
	[officeCode] [varchar](50) NULL,
	[transactionDate] [datetime] NULL,
	[productType] [varchar](50) NULL,
	[trackingNumber] [varchar](150) NULL,
	[referenceNumber] [varchar](150) NULL,
	[encashmentBranch] [varchar](150) NULL,
	[shipperName] [varchar](150) NULL,
	[consigneeName] [varchar](150) NULL,
	[unit] [integer] NULL,
	[principalAmount] [decimal](18,0) NULL,
	[encashmentDate] [datetime] NULL,
	[statusCode] [varchar](50) NULL,
	[statusDescription] [varchar](50) NULL,
	[encashmentBranchHub][varchar](150) NULL,
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.Acceptance] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 


--updated in above scripts
--EXEC sp_RENAME 'Acceptance.customerName' , 'origin_agent_name', 'COLUMN'

----- execute later tonight (06/05/2025)
----- this changes use proper datatype in date-type columns for performance optimization
---- Add a new column to the table
--ALTER TABLE [dbo].[Acceptance]
--ADD _transactionDate [DATETIME] NULL; 

--ALTER TABLE [dbo].[Acceptance]
--ADD _encashmentDate [DATETIME] NULL;  

---- Copy data from existing column to the new column
--UPDATE [dbo].[Acceptance]
--SET [_transactionDate] = CAST([transactionDate] AS DATETIME);
--UPDATE [dbo].[Acceptance]
--SET [_encashmentDate] = CAST([encashmentDate] AS DATETIME);

---- update the names of orignal columns into different one to exclude it in the process
--EXEC sp_RENAME 'Acceptance.transactionDate' , 'transactionDate_oldFormat', 'COLUMN'
--EXEC sp_RENAME 'Acceptance.encashmentDate' , 'encashmentDate_oldFormat', 'COLUMN'
----Need to delete these columns to have clean database, after succesful changes are verified

---- update the names of new columns into proper one or the names of the deleted ones
--EXEC sp_RENAME 'Acceptance._transactionDate' , 'transactionDate', 'COLUMN'
--EXEC sp_RENAME 'Acceptance._encashmentDate' , 'encashmentDate', 'COLUMN'

--execute 06/24/2025 : to facilitate soft delete feature
--ALTER TABLE [dbo].[Acceptance]
--ADD isDeleted [BIT] NULL; 

----To optimize SEARCH

----statusCode
--CREATE NONCLUSTERED INDEX IDX_Acceptance_statusCode
--ON Acceptance (statusCode);

----productType
--CREATE NONCLUSTERED INDEX IDX_Acceptance_productType
--ON Acceptance (productType);

----statusDescription
--CREATE NONCLUSTERED INDEX IDX_Acceptance_statusDescription
--ON Acceptance (statusDescription);

----transactionDate
--CREATE NONCLUSTERED INDEX IDX_Acceptance_transactionDate
--ON Acceptance (transactionDate); 

----origin_agent_name
--CREATE NONCLUSTERED INDEX IDX_Acceptance_origin_agent_name
--ON Acceptance (origin_agent_name);

----officeCode
--CREATE NONCLUSTERED INDEX IDX_Acceptance_officeCode
--ON Acceptance (officeCode);

----trackingNumber
--CREATE NONCLUSTERED INDEX IDX_Acceptance_trackingNumber
--ON Acceptance (trackingNumber);

----referenceNumber
--CREATE NONCLUSTERED INDEX IDX_Acceptance_referenceNumber
--ON Acceptance (referenceNumber);

----encashmentBranch
--CREATE NONCLUSTERED INDEX IDX_Acceptance_encashmentBranch
--ON Acceptance (encashmentBranch);

----shipperName
--CREATE NONCLUSTERED INDEX IDX_Acceptance_shipperName
--ON Acceptance (shipperName);

----consigneeName
--CREATE NONCLUSTERED INDEX IDX_Acceptance_consigneeName
--ON Acceptance (consigneeName);