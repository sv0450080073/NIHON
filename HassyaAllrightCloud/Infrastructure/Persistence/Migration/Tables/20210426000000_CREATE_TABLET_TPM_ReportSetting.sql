USE HOC_Master
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS TPM_ReportSetting
GO

CREATE TABLE [dbo].[TPM_ReportSetting](
	[TenantCdSeq] [int] NOT NULL,
	[EigyoCdSeq] [int] NOT NULL,
	[ReportId] [int] NOT NULL,
	[CurrentTemplateId] [int] NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [TKD_ReportSetting1] PRIMARY KEY CLUSTERED 
(
	[TenantCdSeq] ASC,
	[EigyoCdSeq] ASC,
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [HOC_Kashikiri]
GO

CREATE OR ALTER VIEW [dbo].[VPM_ReportSetting] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_ReportSetting]
	GO


