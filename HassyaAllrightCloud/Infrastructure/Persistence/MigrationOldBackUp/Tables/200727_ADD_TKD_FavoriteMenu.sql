/****** Object:  Table [dbo].[TKD_FavoriteMenu]    Script Date: 2020/06/04 15:30:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TKD_FavoriteMenu]') AND type in (N'U'))
DROP TABLE [dbo].[TKD_FavoriteMenu]
GO

/****** Object:  Table [dbo].[TKD_FavoriteMenu]    Script Date: 2020/06/04 15:30:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_FavoriteMenu](
        [FavoriteMenuCdSeq] [int] IDENTITY(1,1) NOT NULL,
        [SyainCdSeq] [int] NOT NULL,
        [MenuTitle] [varchar](50) NOT NULL,
        [MenuUrl] [varchar](250) NOT NULL,
        [DisplayOrder] [smallint] NOT NULL,
        [UpdYmd] [char](8) NOT NULL,
        [UpdTime] [char](6) NOT NULL,
        [UpdSyainCd] [int] NOT NULL,
        [UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_FavoriteMenu] PRIMARY KEY CLUSTERED 
(
        [FavoriteMenuCdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO