USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetKashikiriSetting_R]    Script Date: 05/27/2021 2:57:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tra Nguyen Lam
-- Create date: 2021/05/28
-- Description:	Get regulation setting data 
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[PK_dGetKashikiriSetting_R]
	@CompanyFrom 	int,
	@CompanyTo		int,
	@TenantCdSeq	int
AS
BEGIN

SELECT TKM_KasSet.*,
     ISNULL(eVPM_Compny01.CompanyCd, '') AS CompanyCdSeq_CompanyCd,
     ISNULL(eVPM_Compny01.CompanyNm, '') AS CompanyCdSeq_CompanyNm,
     ISNULL(eVPM_Compny01.RyakuNm, '') AS CompanyCdSeq_RyakuNm,
     eVPM_Syain01.SyainCd,
     eVPM_Syain01.SyainNm
	 FROM TKM_KasSet
LEFT JOIN VPM_Compny AS eVPM_Compny01
     ON TKM_KasSet.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain01
     ON TKM_KasSet.UpdSyainCd = eVPM_Syain01.SyainCdSeq
WHERE eVPM_Compny01.TenantCdSeq = @TenantCdSeq
     AND (@CompanyFrom = 0 OR eVPM_Compny01.CompanyCd >= @CompanyFrom)
     AND (@CompanyTo = 0 OR eVPM_Compny01.CompanyCd <= @CompanyTo)
ORDER BY eVPM_Compny01.CompanyCd
END
