USE [HOC_Master]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS TPM_KaizenKijun
GO

CREATE TABLE [dbo].[TPM_KaizenKijun](
	[KijunSeq] [int] IDENTITY(1,1) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[KijunNm] [nvarchar](100) NOT NULL,
	[PeriodUnit] [tinyint] NOT NULL,
	[PeriodValue] [int] NOT NULL,
	[KijunRef] [int] NOT NULL,
	[PriorityOrder] [int] NOT NULL,
	[SiyoStaYmd] [char](8) NOT NULL,
	[SiyoEndYmd] [char](8) NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [TPM_KaizenKijun1] PRIMARY KEY CLUSTERED 
(
	[KijunSeq] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [HOC_Kashikiri]
GO

CREATE OR ALTER VIEW [dbo].[VPM_KaizenKijun] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_KaizenKijun]
GO