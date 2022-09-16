USE [HOC_Kashikiri]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[TKD_RyoPrS]
GO

CREATE TABLE [dbo].[TKD_RyoPrS](
        [TenantCdSeq] [int] NOT NULL,
        [RyoOutSeq] [int] NOT NULL,
        [RyoHatYmd] [char](8) NOT NULL,
        [RyoOutTime] [nchar](4) NOT NULL,
        [InTanCdSeq] [int] NOT NULL,
        [RyoEigCdSeq] [int] NOT NULL,
        [RyoUrl] [varchar](255) NOT NULL,
        [SiyoKbn] [tinyint] NOT NULL,
        [UpdYmd] [nchar](8) NOT NULL,
        [UpdTime] [nchar](6) NOT NULL,
        [UpdSyainCd] [int] NOT NULL,
        [UpdPrgID] [nchar](10) NOT NULL,
 CONSTRAINT [PK_TKD_RyoPrS] PRIMARY KEY CLUSTERED 
(
        [TenantCdSeq] ASC,
        [RyoOutSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO