USE HOC_Master
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TPM_ReportTemplate](
	[TenantCdSeq] [int] NOT NULL,
	[ReportId] [int] NOT NULL,
	[TemplateId] [int] NOT NULL,
	[TemplateNm][varchar](50) NOT NULL,
	[ImgPath] [varchar] (max) NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [TKD_ReportTemplate1] PRIMARY KEY CLUSTERED 
(
	[TenantCdSeq] ASC,
	[ReportId] ASC,
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


