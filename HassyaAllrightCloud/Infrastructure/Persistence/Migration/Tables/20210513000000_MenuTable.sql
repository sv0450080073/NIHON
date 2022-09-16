USE [HOC_Master]
GO

/****** Object:  Table [dbo].[TPM_MenuResource]    Script Date: 5/13/2021 2:00:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[TPM_Kinou] ADD KinouUrl NVARCHAR(50)  NOT NULL CONSTRAINT KinouUrl_DEFAULT_VAL DEFAULT ''
ALTER TABLE [dbo].[TPM_Kinou] ADD ServiceCdSeq INT NOT NULL CONSTRAINT ServiceCdSeq_DEFAULT_VAL DEFAULT 1;

CREATE TABLE [dbo].[TPM_Menu](
	[MenuCdSeq] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCdSeq] [tinyint] NOT NULL,
	[Node] [tinyint] NOT NULL,
	[ParentMenuCdSeq] [int] NOT NULL,
	[Sort] [smallint] NOT NULL,
	[Icon] [nvarchar](50) NOT NULL,
	[MenuNm] [nvarchar](50) NOT NULL,
	[KinouID] [char](10) NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TPM_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuCdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TPM_MenuResource](
	[MenuCdSeq] [int] NOT NULL,
	[LangCode] [varchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL
 CONSTRAINT [PK_MenuResource] PRIMARY KEY CLUSTERED 
(
	[MenuCdSeq] ASC,
	[LangCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



