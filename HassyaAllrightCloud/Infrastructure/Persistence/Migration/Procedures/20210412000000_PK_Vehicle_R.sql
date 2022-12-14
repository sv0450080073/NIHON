USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_Vehicle_R]    Script Date: 10/20/2020 10:22:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_Vehicle_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Vehicle Data
-- Date			:   2020/10/20
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].PK_Vehicle_R
    (
     -- Parameter
	       @TenantCdSeq           VARCHAR(8)
		,  @UkeNo                 VARCHAR(15)
	 
        -- Output
	    ,  @ROWCOUNT	          INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
		SELECT TKD_Yousha.*
                ,ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS YouCdSeq_TokuiCd
                ,ISNULL(eVPM_Tokisk01.Kana, ' ') AS YouCdSeq_Kana
                ,ISNULL(eVPM_Tokisk01.TokuiNm, ' ') AS YouCdSeq_TokuiNm
                ,ISNULL(eVPM_Tokisk01.RyakuNm, ' ') AS YouCdSeq_RyakuNm
                ,ISNULL(eVPM_Tokisk01.GyosyaCdSeq, 0) AS YouCdSeq_GyosyaCdSeq
                ,ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS YouGyosyaCdSeq_GyosyaCd
                ,ISNULL(eVPM_Gyosya02.GyosyaNm, ' ') AS YouGyosyaCdSeq_GyosyaNm
                ,ISNULL(eVPM_Gyosya02.GyosyaKbn, 0) AS YouGyosyaCdSeq_GyosyaKbn
                ,ISNULL(eVPM_CodeKb03.CodeKbnNm, ' ') AS GyoSyaKbn_CodeKbnNm
                ,ISNULL(eVPM_CodeKb03.RyakuNm, ' ') AS GyoSyaKbn_RyakuNm
                ,ISNULL(eVPM_TokiSt04.SitenCd, 0) AS YouSitCdSeq_SitenCd
                ,ISNULL(eVPM_TokiSt04.Kana, ' ') AS YouSitCdSeq_Kana
                ,ISNULL(eVPM_TokiSt04.SitenNm, ' ') AS YouSitCdSeq_SitenNm
                ,ISNULL(eVPM_TokiSt04.RyakuNm, ' ') AS YouSitCdSeq_RyakuNm
                ,ISNULL(eVPM_CodeKb05.CodeKbnNm, ' ') AS ZeiKbn_CodeKbnNm
                ,ISNULL(eVPM_CodeKb05.RyakuNm, ' ') AS ZeiKbn_RyakuNm
                ,ISNULL(eVPM_CodeKb06.CodeKbnNm, ' ') AS JitaFlg_CodeKbnNm
                ,ISNULL(eVPM_CodeKb06.RyakuNm, ' ') AS JitaFlg_RyakuNm
                ,ISNULL(eVPM_Compny07.CompanyCd, 0) AS CompanyCdSeq_CompnyCd
                ,ISNULL(eVPM_Compny07.CompanyNm, ' ') AS CompanyCdSeq_CompnyNm
                ,ISNULL(eVPM_Compny07.RyakuNm, ' ') AS CompanyCdSeq_RyakuNm
                ,ISNULL(eVPM_CodeKb08.CodeKbnNm, ' ') AS SihKbn_CodeKbnNm
                ,ISNULL(eVPM_CodeKb08.RyakuNm, ' ') AS SihKbn_RyakuNm
                ,ISNULL(eVPM_CodeKb10.CodeKbnNm, ' ') AS SCouKbn_CodeKbnNm
                ,ISNULL(eVPM_CodeKb10.RyakuNm, ' ') AS SCouKbn_RyakuNm
                ,ISNULL(eVPM_Syain09.SyainCd, ' ') AS UpdSyainCd_SyainCd
                ,ISNULL(eVPM_Syain09.SyainNm, ' ') AS UpdSyainCd_SyainNm
        FROM TKD_Yousha
        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01 ON TKD_Yousha.YouCdSeq = eVPM_Tokisk01.TokuiSeq
                AND TKD_Yousha.SihYotYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
                AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02 ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb03 ON eVPM_CodeKb03.CodeSyu = 'GYOSYAKBN'
                AND CONVERT(VARCHAR(10), eVPM_Gyosya02.GyoSyaKbn) = eVPM_CodeKb03.CodeKbn
                AND eVPM_CodeKb03.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'GYOSYAKBN'
                                AND KanriKbn <> 1
                        )
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt04 ON TKD_Yousha.YouCdSeq = eVPM_TokiSt04.TokuiSeq
                AND TKD_Yousha.YouSitCdSeq = eVPM_TokiSt04.SitenCdSeq
                AND TKD_Yousha.SihYotYmd BETWEEN eVPM_TokiSt04.SiyoStaYmd AND eVPM_TokiSt04.SiyoEndYmd
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb05 ON eVPM_CodeKb05.CodeKbn = dbo.FP_RpZero(2, TKD_Yousha.ZeiKbn)
                AND eVPM_CodeKb05.CodeSyu = 'ZEIKBN'
                AND eVPM_CodeKb05.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'ZEIKBN'
                                AND KanriKbn <> 1
                        )
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb06 ON eVPM_CodeKb06.CodeSyu = 'JITAFLG'
                AND CONVERT(VARCHAR(10), TKD_Yousha.JitaFlg) = eVPM_CodeKb06.CodeKbn
                AND eVPM_CodeKb06.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'JITAFLG'
                                AND KanriKbn <> 1
                        )
        LEFT JOIN VPM_Compny AS eVPM_Compny07 ON TKD_Yousha.CompanyCdSeq = eVPM_Compny07.CompanyCdSeq
                AND eVPM_Compny07.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb08 ON eVPM_CodeKb08.CodeKbn = dbo.FP_RpZero(2, TKD_Yousha.SihKbn)
                AND eVPM_CodeKb08.CodeSyu = 'SIHKBN'
                AND eVPM_CodeKb08.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'SIHKBN'
                                AND KanriKbn <> 1
                        )
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb10 ON eVPM_CodeKb10.CodeKbn = dbo.FP_RpZero(2, TKD_Yousha.SCouKbn)
                AND eVPM_CodeKb10.CodeSyu = 'SCOUKBN'
                AND eVPM_CodeKb10.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'SCOUKBN'
                                AND KanriKbn <> 1
                        )
        LEFT JOIN VPM_Syain AS eVPM_Syain09 ON TKD_Yousha.UpdSyainCd = eVPM_Syain09.SyainCdSeq
        WHERE 1 = 1
                AND TKD_Yousha.UkeNo = @UkeNo
                AND TKD_Yousha.SiyoKbn = 1
        ORDER BY TKD_Yousha.UkeNo
                ,TKD_Yousha.UnkRen

    SET	@ROWCOUNT	=	@@ROWCOUNT
	END
RETURN																													
