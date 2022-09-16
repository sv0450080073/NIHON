USE [HOC_Kashikiri]
GO

/****** Object:  Table [dbo].[TKD_SchYotei]    Script Date: 2020/07/17 13:52:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_SchHyoSet] (
	[SyainCdSeq] [int] NOT NULL,
	[TimeZn] [varchar](50) NOT NULL,
	[DefltDispTyp] [tinyint] NOT NULL,
	[WeekStrDay] [tinyint] NOT NULL,
	[StrTimeOfDay] [char](6) NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [TKD_SchHyoSet1] PRIMARY KEY CLUSTERED 
(
	[SyainCdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


