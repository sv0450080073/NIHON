USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_TenantGroupDetailTokui]    Script Date: 2021/04/09 11:41:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_TenantGroupDetailTokui]
AS
SELECT                      TenantGroupCdSeq, TenantCdSeq, SitenCdSeqTenantCdSeq, TokuiSeq, SitenCdSeq, StaYmd, EndYmd, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID
FROM                         HOC_Master.dbo.TPM_TenantGroupDetailTokui
GO