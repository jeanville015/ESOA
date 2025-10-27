IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rate]'))
BEGIN
	ALTER TABLE [dbo].[Rate]
		DROP CONSTRAINT [PK_esoa.Rate];
	DROP TABLE [dbo].[Rate];
END

CREATE TABLE [Rate](
	[pkid] [uniqueidentifier] NOT NULL, 
	[reference] [varchar](150) NULL,
	[rateType_ipp] [varchar](50) NULL,
	[rateType_pp_sc] [varchar](50) NULL,
	[rateType_rta] [varchar](50) NULL,
	[rateType_sns] [varchar](50) NULL,
	[rateType_ippx] [varchar](50) NULL,
	[ipp] [decimal] NULL,
	[pp_sc] [decimal] NULL,
	[rta] [decimal] NULL,
	[sns] [decimal] NULL,
	[ippx] [decimal] NULL,
	[from] [varchar](50) NULL,
	[to] [varchar](50) NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[updatedBy] [uniqueidentifier] NULL, 
	[isDeleted] [bit] NULL
 CONSTRAINT [PK_esoa.Rate] PRIMARY KEY CLUSTERED 
(
	[pkid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--ALTER TABLE [dbo].[Rate]
--	DROP COLUMN [from], [to]
--ALTER TABLE [dbo].[Rate]
--	ADD	 [from] [varchar](50) 
--		,[to] [varchar](50) 
--EXEC sp_RENAME 'Rate.rateType', 'rateType_ipp', 'COLUMN'

--ALTER TABLE [dbo].[Rate]
--	ADD	 [rateType_pp_sc] [varchar](50) NULL
--		,[rateType_rta] [varchar](50) NULL		
--		,[rateType_sns] [varchar](50) NULL		
--		,[rateType_ippx] [varchar](50) NULL

--alter table [RATE]
-- add  CONSTRAINT [PK_esoa.Rate] PRIMARY KEY CLUSTERED ([pkid] ASC)

--alter table [dbo].[Rate]
--alter column [ipp] decimal(18,2);

--alter table [dbo].[Rate]
--alter column [pp_sc] decimal(18,2);

--alter table [dbo].[Rate]
--alter column [rta] decimal(18,2);

--alter table [dbo].[Rate]
--alter column [sns] decimal(18,2);

--alter table [dbo].[Rate]
--alter column [ippx] decimal(18,2);