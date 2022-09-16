USE [HOC_Master]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[TPM_KaizenKijunJkValue](
	[TenantCdSeq] [int] NOT NULL, 
	[KijunSeq] [int] NOT NULL,
	[JokenNo] [int] NOT NULL,
	[RestrictedValue] [int] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [TPM_KaizenKijunJkValue1] PRIMARY KEY CLUSTERED 
(
	[TenantCdSeq] ASC,
	[KijunSeq] ASC,
	[JokenNo] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [HOC_Kashikiri]
GO

CREATE OR ALTER VIEW [dbo].[VPM_KaizenKijunJkValue] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_KaizenKijunJkValue]
GO