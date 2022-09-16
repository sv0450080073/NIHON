USE [HOC_Master]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TPM_NotiTemplateVar](
	[TemplateKbn] [tinyint] NOT NULL,
	[VariableCd] [nvarchar](50) NOT NULL,
	[VariableNm] [nvarchar](50) NOT NULL,
	[MailSiyoKbn] [tinyint] NOT NULL,
	[LineSiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TPM_NotiTemplateVar] PRIMARY KEY CLUSTERED 
(
	[TemplateKbn] ASC,
	[VariableCd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

																