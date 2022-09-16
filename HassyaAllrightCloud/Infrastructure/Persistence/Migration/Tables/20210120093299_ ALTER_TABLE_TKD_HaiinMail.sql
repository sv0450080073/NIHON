USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TKD_HaiinMail]') AND type in (N'U'))
DROP TABLE [dbo].[TKD_HaiinMail]
GO

CREATE TABLE [dbo].[TKD_HaiinMail](
        [UkeNo] [nchar](15) NOT NULL,
        [UnkRen] [smallint] NOT NULL,
        [TeiDanNo] [smallint] NOT NULL,
        [BunkRen] [smallint] NOT NULL,
        [HaiInRen] [tinyint] NOT NULL,
        [SyainCdSeq] [int] NOT NULL,
        [KinKyuTblCdSeq] [int] NOT NULL,
        [UnkYmd] [char](8) NOT NULL,
        [ControlNo] [nchar](25) NOT NULL,
        [MailExeCnt] [tinyint] NOT NULL,
        [LineExeCnt] [tinyint] NOT NULL,
        [SchReadKbn] [tinyint] NOT NULL,
        [Biko] [nvarchar](4000) NOT NULL,
        [UpdYmd] [char](8) NOT NULL,
        [UpdTime] [char](6) NOT NULL,
        [UpdSyainCd] [int] NOT NULL,
        [UpdPrgID] [char](10) NOT NULL,        
 CONSTRAINT [PK_TKD_HaiinMail] PRIMARY KEY CLUSTERED 
(
        [UkeNo] ASC,
        [UnkRen] ASC,
        [TeiDanNo] ASC,
        [BunkRen] ASC,
        [HaiInRen] ASC,
        [SyainCdSeq] ASC,
        [KinKyuTblCdSeq] ASC,
        [UnkYmd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO