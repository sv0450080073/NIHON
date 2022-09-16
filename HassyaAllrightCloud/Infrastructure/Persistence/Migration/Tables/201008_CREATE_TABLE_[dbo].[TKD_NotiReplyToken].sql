USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_NotiReplyToken](
	[ControlNo] [nchar](25) NOT NULL,
	[NotiMethodKbn] [tinyint] NOT NULL,
	[Token] [varchar](36) NOT NULL,
	[ExpiredDate] [char](8) NOT NULL,
	[ExpiredTime] [char](6) NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_NotiReplyToken] PRIMARY KEY CLUSTERED 
(
	[ControlNo] ASC,
	[NotiMethodKbn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO																