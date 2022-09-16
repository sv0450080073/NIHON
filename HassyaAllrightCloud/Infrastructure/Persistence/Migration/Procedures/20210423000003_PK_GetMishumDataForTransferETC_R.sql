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
-- SP-ID		:   PK_GetMidshumDataForTransferETC_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get mishum data
-- Date			:   2021/04/23
-- Author		:   P.M.Nhat
-- Description	:   Get mishum data with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_GetMishumDataForTransferETC_R
	-- Add the parameters for the stored procedure here
	@UkeNo nchar(15),
	@TenantCdSeq int
AS
BEGIN
	SELECT TKD_Mishum.*		
			,ISNULL(eVPM_CodeKb01.CodeKbnNm, ' ') AS SeiFutSyu_CodeKbnNm -- 請求付帯種別名		
			,ISNULL(eVPM_CodeKb01.RyakuNm, ' ') AS SeiFutSyu_RyakuNm -- 請求付帯種別略名		
			,ISNULL(eVPM_Syain02.SyainCd, ' ') AS UpdSyainCd_SyainCd -- 最終更新社員名		
			,ISNULL(eVPM_Syain02.SyainNm, ' ') AS UpdSyainCd_SyainNm -- 最終更新社員略名		
	FROM TKD_Mishum		
	-- 未収明細テーブルの請求付帯種別より取得（SEIFUTSYU）（コード区分マスタ）		
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'		
			AND eVPM_CodeKb01.CodeKbn = dbo.FP_RpZero(2, TKD_Mishum.SeiFutSyu)		
			AND eVPM_CodeKb01.SiyoKbn = 1		
			AND eVPM_CodeKb01.TenantCdSeq = (		
					SELECT CASE WHEN COUNT(*) = 0 THEN 0 ELSE @TenantCdSeq END AS TenantCdSeq		
					FROM VPM_CodeKb		
					WHERE VPM_CodeKb.CodeSyu = 'SEIFUTSYU'		
							AND VPM_CodeKb.SiyoKbn = 1		
							AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq		
					)		
	-- 未収明細テーブルの最終更新社員コードＳＥＱより取得（社員マスタ）		
	LEFT JOIN VPM_Syain AS eVPM_Syain02 ON TKD_Mishum.UpdSyainCd = eVPM_Syain02.SyainCdSeq		
	LEFT JOIN (		
        	SELECT		
            	SyainCdSeq,		
            	EigyoCdSeq,		
            	ROW_NUMBER() OVER(		
                	PARTITION BY SyainCdSeq		
                	ORDER BY		
                    	SyainCdSeq,		
                    	EigyoCdSeq		
            	) AS COUNTROW		
        	FROM		
            	VPM_KyoSHe		
    	) AS eVPM_KyoSHe03 ON eVPM_KyoSHe03.SyainCdSeq = eVPM_Syain02.SyainCdSeq		
    	AND eVPM_KyoSHe03.COUNTROW = 1		
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos04 ON eVPM_Eigyos04.EigyoCdSeq = eVPM_KyoSHe03.EigyoCdSeq		
	LEFT JOIN VPM_Compny AS eVPM_Compny05 ON eVPM_Compny05.CompanyCdSeq = eVPM_Eigyos04.CompanyCdSeq 		
	WHERE 1 = 1		
			AND TKD_Mishum.UkeNo = @UkeNo		
			AND eVPM_Compny05.TenantCdSeq = @TenantCdSeq		
	ORDER BY TKD_Mishum.MisyuRen DESC		
END
GO
