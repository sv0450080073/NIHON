USE [HOC_Kashikiri]
GO

/****** Object:  Table [dbo].[TKD_MaxMinFareFeeCause]    Script Date: 2020/10/05 14:50:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: HassyaAlrightCloud
-- SP-ID				: [TKD_MaxMinFareFeeCause]
-- DB-Name				: HOC_Kashikiri
-- Author				: N.T.HIEU
-- Create date			: 2020/10/05
-- Description			: Create Table TKD_MaxMinFareFeeCause
-- =============================================

USE [HOC_Kashikiri]
GO

/****** Object:  Table [dbo].[TKD_MaxMinFareFeeCause]    Script Date: 2020/10/05 10:16:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_MaxMinFareFeeCause](
	[UkeNo] [nchar](15) NOT NULL,
	[UnkRen] [smallint] NOT NULL,
	[TeiDanNo] [smallint] NOT NULL,
	[BunkRen] [smallint] NOT NULL,
	[SyaSyuRen] [smallint] NOT NULL,
	[UpperLowerCauseKbn] [smallint] NULL,
	[OtherCauseDetail] [varchar](200) NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_MaxMinFareFeeCause] PRIMARY KEY CLUSTERED 
(
	[UkeNo] ASC,
	[UnkRen] ASC,
	[TeiDanNo] ASC,
	[BunkRen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO												
