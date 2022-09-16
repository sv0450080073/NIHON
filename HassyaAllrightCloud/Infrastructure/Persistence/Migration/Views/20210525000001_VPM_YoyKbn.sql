USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_YoyKbn]    Script Date: 5/25/2021 3:10:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_YoyKbn]
AS
SELECT YoyaKbnSeq, YoyaKbn, YoyaKbnNm, Yoyagamsyu, UnsouKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID, TenantCdSeq
FROM   HOC_Master.dbo.TPM_YoyKbn
GO