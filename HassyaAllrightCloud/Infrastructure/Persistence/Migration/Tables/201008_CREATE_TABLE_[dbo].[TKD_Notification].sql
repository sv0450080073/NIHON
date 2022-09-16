USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_Notification](
	[ControlNo] [nchar](25) NOT NULL,
	[SyainCdSeq] [int] NOT NULL,
	[SouSYmd] [char](8) NOT NULL,
	[SouSTime] [char](6) NOT NULL,
	[SouSRen] [smallint] NOT NULL,
	[MailNowKbn] [tinyint] NOT NULL,
	[LineNowKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_Notification] PRIMARY KEY CLUSTERED 
(
	[ControlNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
														