
USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[PK_dMonthlyRevenueDetailItems_R]    Script Date: 2020/08/14 16:52:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   RevenueSummary
-- SP-ID		:   PK_dMonthlyRevenueDetailItems_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetMonthlyRevenueDetailItems
-- Date			:   2020/08/12
-- Author		:   Tra Nguyen 
-- Description	:   Get Monthly Transportation Revenue Details Data
------------------------------------------------------------
CREATE OR ALTER  PROCEDURE [dbo].[PK_dMonthlyRevenueDetailItems_R]
	(
	-- Parameter
		@CompanyCd 				INT,
		@UkeNoFrom 				CHAR(15),
		@UkeNoTo 				CHAR(15),
		@YoyaKbnFrom 			INT,
		@YoyaKbnTo 				INT,
		@TenantCdSeq 			INT,
		@EigyoKbn 				INT,
		@TesuInKbn 				INT,
		@EigyoCdSeq 			INT,
		@UriYmdTo 				CHAR(8),
		@UriYmdFrom 			CHAR(8),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS
BEGIN
	
WITH eTKD_Unkobi01
AS (
        SELECT eTKD_Yyksho01.UkeNo AS UkeNo -- 受付番号
                ,ISNULL(eTKD_Unkobi02.UnkRen, 0) AS UnkRen -- 運行日連番
                ,eVPM_YoyKbn15.YoyaKbn AS YoyaKbn -- 予約区分
                ,CASE 
                        WHEN eTKD_Yyksho01.YoyaSyu = 1
                                THEN ISNULL(eTKD_Unkobi02.UriYmd, ' ')
                        WHEN eTKD_Yyksho01.YoyaSyu = 2
                                THEN eTKD_Yyksho01.CanYmd
                        END AS UriYmd -- 売上年月日（キャンセル年月日）
                ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS EigyoCd -- 請求営業所コード / 受付営業所コード
                ,ISNULL(eVPM_Eigyos06.CompanyCdSeq, ' ') AS CompanyCdSeq -- 請求会社コードＳＥＱ / 受付会社コードＳＥＱ
                ,eTKD_Yyksho01.SiyoKbn AS Y_SiyoKbn -- 予約書・使用区分
                ,ISNULL(eTKD_Unkobi02.SiyoKbn, 1) AS U_SiyoKbn -- 運行日・使用区分
        FROM TKD_Yyksho AS eTKD_Yyksho01
        JOIN TKD_Unkobi AS eTKD_Unkobi02 ON eTKD_Unkobi02.UkeNo = eTKD_Yyksho01.UkeNo
                AND eTKD_Yyksho01.YoyaSyu = 1
        JOIN VPM_YoyKbn AS eVPM_YoyKbn15 ON eVPM_YoyKbn15.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
                        AND eVPM_YoyKbn15.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eTKD_Yyksho01.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON eVPM_Eigyos06.EigyoCdSeq = CASE WHEN @EigyoKbn = 1 THEN eTKD_Yyksho01.SeiEigCdSeq
																				ELSE eTKD_Yyksho01.UkeEigCdSeq END
        WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
        )
        ,eTKD_YouSyu04
AS (
        SELECT eTKD_Haisha04.UkeNo AS UkeNo
                ,eTKD_Haisha04.UnkRen AS UnkRen
                ,eTKD_Haisha04.YouTblSeq AS YouTblSeq
                ,count(*) AS SyaSyuDai
        FROM TKD_Haisha AS eTKD_Haisha04
        WHERE eTKD_Haisha04.SiyoKbn = 1
                AND eTKD_Haisha04.YouTblSeq <> 0
                AND SUBSTRING(eTKD_Haisha04.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
        GROUP BY eTKD_Haisha04.UkeNo
                ,eTKD_Haisha04.UnkRen
                ,eTKD_Haisha04.YouTblSeq
        )
        ,eTKD_YFutTu05
AS (
        SELECT eTKD_YFutTu01.UkeNo AS UkeNo
                ,eTKD_YFutTu01.UnkRen AS UnkRen
                ,eTKD_YFutTu01.YouTblSeq AS YouTblSeq
                ,SUM(eTKD_YFutTu01.HaseiKin) AS HaseiKin -- 発生額
                ,SUM(eTKD_YFutTu01.SyaRyoSyo) AS SyaRyoSyo -- 消費税額
                ,SUM(eTKD_YFutTu01.SyaRyoTes) AS SyaRyoTes -- 手数料額
        FROM TKD_YFutTu AS eTKD_YFutTu01
        WHERE eTKD_YFutTu01.SiyoKbn = 1
        AND SUBSTRING(eTKD_YFutTu01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
        GROUP BY eTKD_YFutTu01.UkeNo
                ,eTKD_YFutTu01.UnkRen
                ,eTKD_YFutTu01.YouTblSeq
        )
-- ********************************************************************************************************************************
-- 明細区分：３（明細）
-- ********************************************************************************************************************************
SELECT 3 AS MesaiKbn -- 明細区分
        ,eTKD_Unkobi01.UriYmd AS UriYmd -- 売上年月日
        ,eTKD_Unkobi01.EigyoCd AS EigyoCd --請求営業所コード/受付営業所コード
        ,ISNULL(eVPM_Tokisk11.TokuiCd, 0) AS YouCd -- 傭車先コード
        ,ISNULL(eVPM_Tokisk11.TokuiNm, ' ') AS YouNm -- 傭車先名
        ,ISNULL(eVPM_Tokisk11.RyakuNm, ' ') AS YouRyakuNm -- 傭車先略名
        ,ISNULL(eVPM_TokiSt12.SitenCd, 0) AS YouSitCd -- 傭車先支店コード
        ,ISNULL(eVPM_TokiSt12.SitenNm, ' ') AS YouSitenNm -- 傭車先支店名
        ,ISNULL(eVPM_TokiSt12.RyakuNm, ' ') AS YouSitRyakuNm -- 傭車先支店略名
        ,ISNULL(eVPM_Gyosya13.GyosyaCd, 0) AS YouGyosyaCd -- 傭車先業者コード
        ,ISNULL(eVPM_Gyosya13.GyosyaNm, ' ') AS YouGyosyaNm -- 傭車先業者コード名
        ,SUM(eTKD_Yousha03.SyaRyoUnc) AS YouSyaRyoUnc -- 傭車項目・運賃
        ,SUM(eTKD_Yousha03.SyaRyoSyo) AS YouSyaRyoSyo -- 傭車項目・消費税額
        ,SUM(eTKD_Yousha03.SyaRyoTes) AS YouSyaRyoTes -- 傭車項目・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - eTKD_Yousha03.SyaRyoTes
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - (
                                                CASE 
                                                        WHEN eVPM_TokiSt12.TesKbn = 2
                                                                THEN eTKD_Yousha03.SyaRyoTes
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_YouSyaRyo -- 傭車項目・合計
        ,SUM(ISNULL(eTKD_YouSyu04.SyaSyuDai, 0)) AS YouSyaSyuDai -- 傭車項目・台数
        ,SUM(ISNULL(eTKD_YFutTu05.HaseiKin, 0)) AS YfuUriGakKin -- 傭車付帯・売上額
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)) AS YfuSyaRyoSyo -- 傭車付帯・消費税額
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)) AS YfuSyaRyoTes -- 傭車付帯・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt12.TesKbnFut = 2
                                                                THEN ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_YfuSyaRyo -- 傭車付帯・合計
FROM eTKD_Unkobi01
JOIN TKD_Yousha AS eTKD_Yousha03 ON eTKD_Yousha03.UkeNo = eTKD_Unkobi01.UkeNo
        AND eTKD_Yousha03.UnkRen = eTKD_Unkobi01.UnkRen
        AND eTKD_Yousha03.SiyoKbn = 1
LEFT JOIN eTKD_YouSyu04 ON eTKD_YouSyu04.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YouSyu04.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YouSyu04.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN eTKD_YFutTu05 ON eTKD_YFutTu05.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YFutTu05.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YFutTu05.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON eVPM_Tokisk11.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd AND eVPM_Tokisk11.SiyoEndYmd
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt12 ON eVPM_TokiSt12.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eVPM_TokiSt12.SitenCdSeq = eTKD_Yousha03.YouSitCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt12.SiyoStaYmd AND eVPM_TokiSt12.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya13 ON eVPM_Gyosya13.GyosyaCdSeq = eVPM_Tokisk11.GyosyaCdSeq
LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
        AND eVPM_Tenant.SiyoKbn = 1
WHERE	eVPM_Compny17.CompanyCd = CASE WHEN @CompanyCd IS NOT NULL THEN @CompanyCd
								ELSE eVPM_Compny17.CompanyCd END
        AND eTKD_Unkobi01.EigyoCd = CASE WHEN @EigyoCdSeq IS NOT NULL THEN @EigyoCdSeq
								ELSE eTKD_Unkobi01.EigyoCd END
        AND eTKD_Unkobi01.UriYmd >= CASE WHEN @UriYmdFrom IS NOT NULL THEN @UriYmdFrom
								ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UriYmd <= CASE WHEN @UriYmdTo IS NOT NULL THEN @UriYmdTo
								ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UkeNo >= CASE WHEN @UkeNoFrom IS NOT NULL THEN @UkeNoFrom
								ELSE eTKD_Unkobi01.UkeNo END
        AND eTKD_Unkobi01.UkeNo <= CASE WHEN @UkeNoTo IS NOT NULL THEN @UkeNoTo
								ELSE eTKD_Unkobi01.UkeNo END
        AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND eVPM_Tenant.TenantCdSeq = CASE WHEN @TenantCdSeq IS NOT NULL THEN @TenantCdSeq
								ELSE eVPM_Tenant.TenantCdSeq END
GROUP BY eTKD_Unkobi01.UriYmd -- 売上年月日
        ,eTKD_Unkobi01.EigyoCd -- 請求営業所コード/受付営業所コード
        ,ISNULL(eVPM_Tokisk11.TokuiCd, 0) -- 傭車先コード
        ,ISNULL(eVPM_Tokisk11.TokuiNm, ' ') -- 傭車先名
        ,ISNULL(eVPM_Tokisk11.RyakuNm, ' ') -- 傭車先略名
        ,ISNULL(eVPM_TokiSt12.SitenCd, 0) -- 傭車先支店コード
        ,ISNULL(eVPM_TokiSt12.SitenNm, ' ') -- 傭車先支店名
        ,ISNULL(eVPM_TokiSt12.RyakuNm, ' ') -- 傭車先支店略名
        ,ISNULL(eVPM_Gyosya13.GyosyaCd, 0) -- 傭車先業者コード
        ,ISNULL(eVPM_Gyosya13.GyosyaNm, ' ') -- 傭車先業者コード名

-- ********************************************************************************************************************************
-- 明細区分：２（累計）
-- ********************************************************************************************************************************

UNION ALL

SELECT 2 AS MesaiKbn
        ,' ' AS UriYmd
        ,0 AS EigyoCd
        ,0 AS YouCd
        ,' ' AS YouNm
        ,' ' AS YouRyakuNm
        ,0 AS YouSitCd
        ,' ' AS YouSitenNm
        ,' ' AS YouSitRyakuNm
        ,0 AS YouGyosyaCd
        ,' ' AS YouGyosyaNm
        ,SUM(eTKD_Yousha03.SyaRyoUnc) AS YouSyaRyoUnc
        ,SUM(eTKD_Yousha03.SyaRyoSyo) AS YouSyaRyoSyo
        ,SUM(eTKD_Yousha03.SyaRyoTes) AS YouSyaRyoTes
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - eTKD_Yousha03.SyaRyoTes
                        END) AS S_YouSyaRyo
        ,SUM(ISNULL(eTKD_YouSyu04.SyaSyuDai, 0)) AS YouSyaSyuDai
        ,SUM(ISNULL(eTKD_YFutTu05.HaseiKin, 0)) AS YfuUriGakKin
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)) AS YfuSyaRyoSyo
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)) AS YfuSyaRyoTes
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                        END) AS S_YfuSyaRyo
FROM eTKD_Unkobi01
JOIN TKD_Yousha AS eTKD_Yousha03 ON eTKD_Yousha03.UkeNo = eTKD_Unkobi01.UkeNo
        AND eTKD_Yousha03.UnkRen = eTKD_Unkobi01.UnkRen
        AND eTKD_Yousha03.SiyoKbn = 1
LEFT JOIN eTKD_YouSyu04 ON eTKD_YouSyu04.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YouSyu04.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YouSyu04.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN eTKD_YFutTu05 ON eTKD_YFutTu05.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YFutTu05.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YFutTu05.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON eVPM_Tokisk11.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd AND eVPM_Tokisk11.SiyoEndYmd
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt12 ON eVPM_TokiSt12.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eVPM_TokiSt12.SitenCdSeq = eTKD_Yousha03.YouSitCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt12.SiyoStaYmd AND eVPM_TokiSt12.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya13 ON eVPM_Gyosya13.GyosyaCdSeq = eVPM_Tokisk11.GyosyaCdSeq
LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
        AND eVPM_Tenant.SiyoKbn = 1
WHERE	eVPM_Compny17.CompanyCd = CASE WHEN @CompanyCd IS NOT NULL THEN @CompanyCd
								ELSE eVPM_Compny17.CompanyCd END
        AND eTKD_Unkobi01.EigyoCd = CASE WHEN @EigyoCdSeq IS NOT NULL THEN @EigyoCdSeq
								ELSE eTKD_Unkobi01.EigyoCd END
        AND eTKD_Unkobi01.UriYmd >= CASE WHEN @UriYmdFrom IS NOT NULL THEN @UriYmdFrom
								ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UriYmd <= CASE WHEN @UriYmdTo IS NOT NULL THEN @UriYmdTo
								ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UkeNo >= CASE WHEN @UkeNoFrom IS NOT NULL THEN @UkeNoFrom
								ELSE eTKD_Unkobi01.UkeNo END
        AND eTKD_Unkobi01.UkeNo <= CASE WHEN @UkeNoTo IS NOT NULL THEN @UkeNoTo
								ELSE eTKD_Unkobi01.UkeNo END
        AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND eVPM_Tenant.TenantCdSeq = CASE WHEN @TenantCdSeq IS NOT NULL THEN @TenantCdSeq
								ELSE eVPM_Tenant.TenantCdSeq END
ORDER BY EigyoCd
        ,UriYmd
        ,YouCd

SET @ROWCOUNT = @@ROWCOUNT
END
GO


