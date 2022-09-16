/****** Object:  Table [dbo].[TKD_PersonalNote]    Script Date: 2020/06/05 10:17:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TKD_PersonalNote]') AND type in (N'U'))
DROP TABLE [dbo].[TKD_PersonalNote]
GO

/****** Object:  Table [dbo].[TKD_PersonalNote]    Script Date: 2020/06/05 10:17:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_PersonalNote](
        [SyainCdSeq] [int] NOT NULL,
        [Note] [varchar](max) NOT NULL,
        [UpdYmd] [char](8) NOT NULL,
        [UpdTime] [char](6) NOT NULL,
        [UpdSyainCd] [int] NOT NULL,
        [UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_PersonalNote] PRIMARY KEY CLUSTERED 
(
        [SyainCdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO