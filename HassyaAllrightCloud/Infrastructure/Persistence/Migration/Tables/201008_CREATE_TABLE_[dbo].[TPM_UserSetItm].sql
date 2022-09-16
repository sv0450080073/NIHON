USE [HOC_Master]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TPM_UserSetItm](
	[ItemSeq] [int] IDENTITY(1,1) NOT NULL,
	[ItemType] [varchar](20) NOT NULL,
	[ItemCd] [varchar](50) NOT NULL,
	[ItemNm] [nvarchar](50) NOT NULL,
	[SeiKbn] [tinyint] NOT NULL,
	[DefaultVal] [nvarchar](50) NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TPM_UserSetItm] PRIMARY KEY CLUSTERED 
(
	[ItemSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO																