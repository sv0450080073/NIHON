USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetRulePopupData_R]    Script Date: 6/1/2021 11:28:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetRulePopupData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List Rule
-- Date			:   2021/04/12
-- Author		:   P.M.Nhat
-- Description	:   Get list rule with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dGetRulePopupData_R]
AS
BEGIN
	SELECT							
    	eVPM_KaizenKijun01.KijunSeq AS KijunSeq,							
    	eVPM_KaizenKijun01.KijunNm AS KijunNm,							
    	eVPM_KaizenKijun01.PeriodUnit AS PeriodUnit,							
    	eVPM_KaizenKijun01.PeriodValue AS PeriodValue,							
    	eVPM_KaizenKijun01.KijunRef AS KijunRef,							
    	ISNULL(eVPM_KaizenKijun02.PeriodUnit, 0) AS RefPeriodUnit,							
    	ISNULL(eVPM_KaizenKijun02.PeriodValue, 0) AS RefPeriodValue							
	FROM							
    	VPM_KaizenKijun AS eVPM_KaizenKijun01							
    	LEFT JOIN VPM_KaizenKijun AS eVPM_KaizenKijun02 ON eVPM_KaizenKijun02.KijunSeq = eVPM_KaizenKijun01.KijunRef							
    	AND eVPM_KaizenKijun02.Type = 2 -- 参照							
    	AND FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN eVPM_KaizenKijun02.SiyoStaYmd							
    	AND eVPM_KaizenKijun02.SiyoEndYmd							
	WHERE							
    	eVPM_KaizenKijun01.Type = 1							
    	AND FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN eVPM_KaizenKijun01.SiyoStaYmd							
    	AND eVPM_KaizenKijun01.SiyoEndYmd							
	ORDER BY							
    	eVPM_KaizenKijun01.PriorityOrder							
END
