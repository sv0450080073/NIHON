SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if not exists (select * from sysobjects where name='TKD_SchYotKSyaFb' and xtype='U')
CREATE TABLE [dbo].[TKD_SchYotKSyaFb](
	[YoteiSeq] [int] NOT NULL,
	[SyainCdSeq] [int] NOT NULL,
	[YoteiSYmd] [char](8) NOT NULL,
	[YoteiSTime] [char](6) NOT NULL,
	[KuriKbn] [tinyint] NOT NULL,
	[AcceptKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_SchYotKSyaFb] PRIMARY KEY CLUSTERED 
(
	[YoteiSeq] ASC,
	[SyainCdSeq] ASC,
	[YoteiSYmd] ASC,
	[YoteiSTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO															