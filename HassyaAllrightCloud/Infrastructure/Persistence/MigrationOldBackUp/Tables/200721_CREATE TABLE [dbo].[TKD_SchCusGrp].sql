USE [HOC_Kashikiri]
GO

/****** Object:  Table [dbo].[TKD_SchYotei]    Script Date: 2020/07/17 13:52:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_SchCusGrp] (
	[CusGrpSeq] [int] IDENTITY(1,1) NOT NULL,
	[SyainCdSeq] [int] NOT NULL,
	[GrpNnm] [varchar](50) NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [TKD_SchCusGrp1] PRIMARY KEY CLUSTERED 
(
	[CusGrpSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO																

