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
-- SP-ID		:   PK_dYoyakuInfos_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get YoyakuInfo List
-- Date			:   2020/09/11
-- Author		:   P.M.Nhat
-- Description	:   Get YoyakuInfo list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dYoyakuInfos_R
	-- Add the parameters for the stored procedure here								
	@UnkYmdFrom				varchar(8),			-- 開始ETC利用年月日								
	@UnkYmdTo				varchar(8),			-- 終了ETC利用年月日				
	@KikoYmdFrom			varchar(8),			-- 開始帰庫年月日				
	@KikoYmdTo				varchar(8),			-- 開始帰庫年月日				
	@SyaryoCd				int,				-- 並び順	
	@TenantCdSeq			int,
	@ScreenType				tinyint
AS
BEGIN
	SELECT TKD_Shabni.UkeNo																														
        ,TKD_Shabni.UnkRen																														
        ,TKD_Shabni.TeiDanNo																														
        ,TKD_Shabni.BunkRen																														
        ,ISNULL(JT_Unkobi.DanTaNm, '') AS DanTaNm																														
        ,ISNULL(JM_SyaRyo.SyaRyoCd, 0) AS SyaRyoCd																														
        ,ISNULL(JM_SyaRyo.SyaRyoNm, '') AS SyaRyoNm																														
        ,ISNULL(JT_Unkobi.DanTaNm, '') AS DantaNm1																														
        ,ISNULL(JM_SyaRyo.SyaRyoCd, 0) AS SyaryoCd1																														
        ,ISNULL(JM_TokiSt.TesuRituFut, 0) AS TesuRituFut																														
        ,ISNULL(JM_TokiSt.TesuRituGui, 0) AS TesuRituGui																														
        ,ISNULL(JM_TokiSt.SitenNm, '') AS SitenNm																														
        ,ISNULL(JM_TokiSk.TokuiNm, '') AS TokuiNm																														
        ,ISNULL(JM_SeikyuTokiSt.TesKbnFut, 0) AS SeikyuTesKbnFut																														
        ,ISNULL(JM_SeikyuTokiSt.TesuRituFut, 0) AS SeikyuTesuRituFut																														
        ,ISNULL(JM_Futai.CountFutai, 0) AS CountFutai																														
        ,ISNULL(JM_MFutu.CountMFutu, 0) AS CountMFutu																														
        ,ISNULL(JT_Haisha.SyuKoYmd, '') AS SyuKoYmd																														
        ,ISNULL(JT_Haisha.SyuKoTime, '') AS SyuKoTime																														
        ,ISNULL(JT_Haisha.KikYmd, '') AS KikYmd																														
        ,ISNULL(JT_Haisha.KikTime, '') AS KikTime																														
        ,TKD_Shabni.UnkYmd																														
        ,ISNULL(JT_Unkobi.HaiSYmd, '') AS HaiSYmd																														
        ,ISNULL(JT_Unkobi.HaiSTime, '') AS HaiSTime																														
        ,ISNULL(JT_Unkobi.TouYmd, '') AS TouYmd																														
        ,ISNULL(JT_Unkobi.TouChTime, '') AS TouChTime																														
        ,ISNULL(JT_Haisha02.SyuKoYmd, '') AS UnkSyuKoYmd																														
        ,ISNULL(JT_Haisha02.SyuKoTime, '') AS UnkSyuKoTime																														
        ,ISNULL(JT_Haisha02.KikYmd, '') AS UnkKikYmd																														
        ,ISNULL(JT_Haisha02.KikTime, '') AS UnkKikTime																														
	FROM TKD_Shabni																														
	LEFT JOIN TKD_Haisha AS JT_Haisha ON TKD_Shabni.UkeNo = JT_Haisha.UkeNo																														
			AND TKD_Shabni.UnkRen = JT_Haisha.UnkRen																														
			AND TKD_Shabni.TeiDanNo = JT_Haisha.TeiDanNo																														
			AND TKD_Shabni.BunkRen = JT_Haisha.BunkRen																														
			AND JT_Haisha.SiyoKbn = 1																														
	LEFT JOIN TKD_Unkobi AS JT_Unkobi ON TKD_Shabni.UkeNo = JT_Unkobi.UkeNo																														
			AND TKD_Shabni.UnkRen = JT_Unkobi.UnkRen																														
			AND JT_Unkobi.SiyoKbn = 1																														
	LEFT JOIN VPM_SyaRyo AS JM_SyaRyo ON JT_Haisha.HaiSSryCdSeq = JM_SyaRyo.SyaRyoCdSeq																														
	LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Shabni.UkeNo = JT_Yyksho.UkeNo																														
			AND JT_Yyksho.YoyaSyu = 1																														
			AND JT_Yyksho.SiyoKbn = 1																														
	LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq																														
			AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq																														
			AND JT_Yyksho.UkeYmd >= JM_TokiSt.SiyoStaYmd																														
			AND JT_Yyksho.UkeYmd <= JM_TokiSt.SiyoEndYmd																														
	LEFT JOIN VPM_TokiSk AS JM_TokiSk ON JM_TokiSk.TokuiSeq = JM_TokiSt.TokuiSeq																														
	LEFT JOIN VPM_TokiSt AS JM_SeikyuTokiSt ON JM_TokiSt.SeiCdSeq = JM_SeikyuTokiSt.TokuiSeq																														
			AND JM_TokiSt.SeiSitenCdSeq = JM_SeikyuTokiSt.SitenCdSeq																														
			AND JT_Yyksho.UkeYmd >= JM_SeikyuTokiSt.SiyoStaYmd																														
			AND JT_Yyksho.UkeYmd <= JM_SeikyuTokiSt.SiyoEndYmd																														
	LEFT JOIN VPM_Tenant AS JM_Tenant ON JT_Yyksho.TenantCdSeq = JM_Tenant.TenantCdSeq																														
			AND JM_Tenant.SiyoKbn = 1																														
	LEFT JOIN (																														
			SELECT UkeNo																														
					,UnkRen																														
					,FutTumKbn																														
					,COUNT(TKD_FutTum.FutTumRen) AS CountFutai																														
			FROM TKD_FutTum																														
			WHERE TKD_FutTum.FutTumKbn = 1																														
					AND TKD_FutTum.SiyoKbn = 1																														
			GROUP BY TKD_FutTum.UkeNo																														
					,TKD_FutTum.UnkRen																														
					,TKD_FutTum.FutTumKbn																														
			) AS JM_Futai ON TKD_Shabni.UkeNo = JM_Futai.UkeNo																														
			AND TKD_Shabni.UnkRen = JM_Futai.UnkRen																														
	LEFT JOIN (																														
			SELECT TKD_FutTum.UkeNo																														
					,TKD_FutTum.UnkRen																														
					,TKD_MFutTu.TeiDanNo																														
					,TKD_MFutTu.BunkRen																														
					,TKD_FutTum.FutTumKbn																														
					,COUNT(TKD_FutTum.FutTumRen) AS CountMFutu																														
			FROM TKD_FutTum																														
			LEFT JOIN TKD_MFutTu ON TKD_FutTum.UkeNo = TKD_MFutTu.UkeNo																														
					AND TKD_FutTum.UnkRen = TKD_MFutTu.UnkRen																														
					AND TKD_FutTum.FutTumRen = TKD_MFutTu.FutTumRen																														
					AND TKD_FutTum.FutTumKbn = TKD_MFutTu.FutTumKbn																														
					AND TKD_MFutTu.SiyoKbn = 1																														
			WHERE TKD_FutTum.FutTumKbn = 1																														
					AND TKD_FutTum.SiyoKbn = 1																														
			GROUP BY TKD_FutTum.UkeNo																														
					,TKD_FutTum.UnkRen																														
					,TKD_MFutTu.TeiDanNo																														
					,TKD_MFutTu.BunkRen																														
					,TKD_FutTum.FutTumKbn																														
			) AS JM_MFutu ON TKD_Shabni.UkeNo = JM_MFutu.UkeNo																														
			AND TKD_Shabni.UnkRen = JM_MFutu.UnkRen																														
			AND TKD_Shabni.TeiDanNo = JM_MFutu.TeiDanNo																														
			AND TKD_Shabni.BunkRen = JM_MFutu.BunkRen																														
	LEFT JOIN (																														
			SELECT UkeNo																														
					,UnkRen																														
					,MIN(SyuKoYmd) AS SyuKoYmd																														
					,MIN(SyuKoTime) AS SyuKoTime																														
					,MAX(KikYmd) AS KikYmd																														
					,MAX(KikTime) AS KikTime																														
			FROM TKD_Haisha																														
			WHERE SiyoKbn = 1																														
			GROUP BY UkeNo																														
					,UnkRen																														
			) AS JT_Haisha02 ON TKD_Shabni.UkeNo = JT_Haisha02.UkeNo																														
			AND TKD_Shabni.UnkRen = JT_Haisha02.UnkRen																														
	WHERE	JM_Tenant.TenantCdSeq = @TenantCdSeq																														
			AND (@UnkYmdFrom = '' OR TKD_Shabni.UnkYmd >= @UnkYmdFrom)																														
			AND (@UnkYmdTo = '' OR TKD_Shabni.UnkYmd <= @UnkYmdTo)																														
			AND (@KikoYmdFrom = '' OR JT_Haisha.KikYmd >=  CASE 
															WHEN @ScreenType = 1 
															THEN @KikoYmdFrom ELSE JT_Haisha.KikYmd END)
			AND (@KikoYmdTo = '' OR JT_Haisha.KikYmd <= CASE 
															WHEN @ScreenType = 1 
															THEN @KikoYmdTo ELSE JT_Haisha.KikYmd END)  -- ETCデータ転送画面で: Add this condition																														
			AND (@SyaryoCd = 0 OR JM_SyaRyo.SyaryoCd = CASE 
														WHEN @ScreenType = 0 
														THEN @SyaryoCd ELSE JM_SyaRyo.SyaryoCd END) -- ETC付帯入力画面で: Add this condition																														
			AND JT_Yyksho.YoyaSyu = 1																														
			AND TKD_Shabni.SiyoKbn = 1																												
END
GO
