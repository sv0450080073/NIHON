	/****** Object:  Table [dbo].[TKD_Notice]    Script Date: 2020/06/05 16:04:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TKD_Notice]') AND type in (N'U'))
DROP TABLE [dbo].[TKD_Notice]
GO

/****** Object:  Table [dbo].[TKD_Notice]    Script Date: 2020/06/05 16:04:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_Notice](
	[NoticeCdSeq] [int] IDENTITY(1,1) NOT NULL,
	[NoticeContent] [varchar](max) NOT NULL,
	[NoticeDisplayKbn] [tinyint] NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_Notice1] PRIMARY KEY CLUSTERED 
(
	[NoticeCdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO															
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																
																