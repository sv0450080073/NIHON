USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Calend]    Script Date: 2021/04/23 10:40:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_Calend]
AS
SELECT                      TenantCdSeq, CompanyCdSeq, CountryCdSeq, CalenSyu, CalenYM, CalenRen, CalenKbn, CalenRank, CalenCom, CalenYmd, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID
FROM                         HOC_Master.dbo.TPM_Calend
GO