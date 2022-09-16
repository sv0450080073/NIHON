--USE [HOC_Kashikiri]
--GO

/****** Object:  View [dbo].[VPM_Ryokin]    Script Date: 2020/09/15 15:37:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VPM_Ryokin]
AS
SELECT                      RyokinTikuCd, RyokinCd, DouroCdSeq, RyokinNm, RyakuNm, SiyoStaYmd, SiyoEndYmd, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID
FROM                         HOC_Master.dbo.TPM_Ryokin
GO