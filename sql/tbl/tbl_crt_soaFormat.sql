IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SOAFormat]'))
BEGIN
	ALTER TABLE [dbo].[SOAFormat]
		DROP CONSTRAINT [PK_esoa.SOAFormat];
	DROP TABLE [dbo].[SOAFormat];
END

CREATE TABLE [SOAFormat](
	[pkid] [uniqueidentifier] NOT NULL, 
	[formatName] [varchar](150) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.SOAFormat] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO