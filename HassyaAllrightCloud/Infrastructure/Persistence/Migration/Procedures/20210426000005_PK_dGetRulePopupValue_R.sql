-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetRulePopupValue_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List Rule Value
-- Date			:   2021/04/12
-- Author		:   P.M.Nhat
-- Description	:   Get list rule value with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dGetRulePopupValue_R
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int
AS
BEGIN
	SELECT		
    	VPM_KaizenKijunJk.KijunSeq AS KijunSeq,		
    	VPM_KaizenKijunJk.JokenNo AS JokenNo,		
    	VPM_KaizenKijunJk.RestrictedTarget AS RestrictedTarget,		
    	VPM_KaizenKijunJk.RestrictedExp AS RestrictedExp,		
    	ISNULL(		
        	eVPM_KaizenKijunJkValue1.RestrictedValue,		
        	eVPM_KaizenKijunJkValue0.RestrictedValue		
    	) AS RestrictedValue		
	FROM		
    	VPM_KaizenKijunJk		
    	LEFT JOIN VPM_KaizenKijunJkValue AS eVPM_KaizenKijunJkValue0 ON eVPM_KaizenKijunJkValue0.KijunSeq = VPM_KaizenKijunJk.KijunSeq		
    	AND eVPM_KaizenKijunJkValue0.JokenNo = VPM_KaizenKijunJk.JokenNo		
    	AND eVPM_KaizenKijunJkValue0.TenantCdSeq = 0		
    	LEFT JOIN VPM_KaizenKijunJkValue AS eVPM_KaizenKijunJkValue1 ON eVPM_KaizenKijunJkValue1.KijunSeq = VPM_KaizenKijunJk.KijunSeq		
    	AND eVPM_KaizenKijunJkValue1.JokenNo = VPM_KaizenKijunJk.JokenNo		
    	AND eVPM_KaizenKijunJkValue1.TenantCdSeq = @TenantCdSeq		
    	JOIN VPM_KaizenKijun ON (		
        	VPM_KaizenKijun.KijunSeq = VPM_KaizenKijunJk.KijunSeq		
        	OR VPM_KaizenKijun.KijunRef = VPM_KaizenKijunJk.KijunSeq		
    	)		
    	AND VPM_KaizenKijun.Type = 1 -- 運用		
    	AND FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN VPM_KaizenKijun.SiyoStaYmd		
    	AND VPM_KaizenKijun.SiyoEndYmd		
	ORDER BY		
    	VPM_KaizenKijunJk.KijunSeq,		
    	VPM_KaizenKijunJk.JokenNo		
END
GO
