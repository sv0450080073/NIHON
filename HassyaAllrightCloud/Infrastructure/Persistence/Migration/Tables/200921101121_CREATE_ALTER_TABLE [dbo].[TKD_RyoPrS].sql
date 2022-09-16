USE [HOC_Kashikiri]
GO

/****** Object:  Table [dbo].[TKD_RyoPrS]    Script Date: 2020/09/21 14:50:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: HassyaAlrightCloud
-- SP-ID				: [TKD_RyoPrS]
-- DB-Name				: HOC_Kashikiri
-- Author				: N.T.HIEU
-- Create date			: 2020/09/21
-- Description			: Create Table TKD_RyoPrS
-- =============================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TKD_RyoPrS](
	[RyoOutSeq] [int] IDENTITY(1,1) NOT NULL,
	[RyoHatYmd] [char](8) NOT NULL,
	[RyoOutTime] [nchar](4) NOT NULL,
	[InTanCdSeq] [int] NOT NULL,
	[RyoEigCdSeq] [int] NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [nchar](8) NOT NULL,
	[UpdTime] [nchar](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [nchar](10) NOT NULL,
 CONSTRAINT [PK_TKD_RyoPrS] PRIMARY KEY CLUSTERED 
(
	[RyoOutSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO														
													
