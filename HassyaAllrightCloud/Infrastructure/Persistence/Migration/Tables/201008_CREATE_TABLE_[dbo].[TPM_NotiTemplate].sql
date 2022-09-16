USE [HOC_Master]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TPM_NotiTemplate](
	[TenantCdSeq] [int] NOT NULL,
	[NotiContentKbn] [int] NOT NULL,
	[NotiMethodKbn] [tinyint] NOT NULL,
	[LineNum] [tinyint] NOT NULL,
	[ContentKbn] [tinyint] NOT NULL,
	[LineContent] [nvarchar](102) NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TPM_NotiTemplate] PRIMARY KEY CLUSTERED 
(
	[TenantCdSeq] ASC,
	[NotiContentKbn] ASC,
	[NotiMethodKbn] ASC,
	[LineNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

																