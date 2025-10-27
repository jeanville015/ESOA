IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SOAVerifiedDates]'))
BEGIN
	ALTER TABLE [dbo].[SOAVerifiedDates]
		DROP CONSTRAINT [PK_esoa.SOAVerifiedDates];
	DROP TABLE [dbo].[SOAVerifiedDates];
END

CREATE TABLE [SOAVerifiedDates](
	[pkid] [int]IDENTITY(1,1) NOT NULL,
	[officeCode] [varchar](50) NULL,
	[transactionDate] [date] NULL,
	[status] [varchar](50) NULL,
	[remarks] [varchar](150) NULL
 CONSTRAINT [PK_esoa.SOAVerifiedDates] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 