USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[PK_dDailyRevenueDetailItems_R]    Script Date: 2020/08/14 11:54:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   RevenueSummary
-- SP-ID		:   PK_dDailyRevenueDetailItems_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetDailyRevenueDetailItems
-- Date			:   2020/08/12
-- Author		:   Tra Nguyen 
-- Description	:   Get Daily Transportation Revenue Details Data
------------------------------------------------------------
CREATE  OR ALTER PROCEDURE [dbo].[PK_dDailyRevenueDetailItems_R]
(
	-- Parameter
		@CompanyCd 				INT,
		@UkeNoFrom 				NCHAR(15),
		@UkeNoTo 				NCHAR(15),
		@YoyaKbnFrom 			INT,
		@YoyaKbnTo 				INT,
		@TenantCdSeq 			INT,
		@EigyoKbn 				INT,
		@TesuInKbn 				INT,
		@EigyoCdSeq 			INT,
		@UriYmdFrom 			CHAR(8),
		@UriYmdTo 				CHAR(8),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
)
AS
BEGIN

	SELECT 3 AS MesaiKbn
        ,eTKD_Unkobi01.UkeNo AS UkeNo
        ,eTKD_Unkobi01.UnkRen AS UnkRen
        ,eTKD_Yousha03.YouTblSeq AS YouTblSeq
        ,ISNULL(eVPM_Tokisk11.TokuiCd, 0) AS YouCd
        ,ISNULL(eVPM_Tokisk11.TokuiNm, ' ') AS YouNm
        ,ISNULL(eVPM_Tokisk11.RyakuNm, ' ') AS YouRyakuNm
        ,ISNULL(eVPM_TokiSt12.SitenCd, 0) AS YouSitCd
        ,ISNULL(eVPM_TokiSt12.SitenNm, ' ') AS YouSitenNm
        ,ISNULL(eVPM_TokiSt12.RyakuNm, ' ') AS YouSitRyakuNm
        ,ISNULL(eVPM_Gyosya13.GyosyaCd, 0) AS YouGyosyaCd
        ,ISNULL(eVPM_Gyosya13.GyosyaNm, ' ') AS YouGyosyaNm
        ,eTKD_Yousha03.SyaRyoUnc AS YouSyaRyoUnc
        ,eTKD_Yousha03.ZeiKbn AS YouZeiKbn
        ,eTKD_Yousha03.Zeiritsu AS YouZeiritsu
        ,eTKD_Yousha03.SyaRyoSyo AS YouSyaRyoSyo
        ,eTKD_Yousha03.TesuRitu AS YouTesuRitu
        ,eTKD_Yousha03.SyaRyoTes AS YouSyaRyoTes
        ,CASE 
                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                        THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo
                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                        THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - eTKD_Yousha03.SyaRyoTes
                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                        THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - (
                                        CASE 
                                                WHEN eVPM_TokiSt12.TesKbn = 2
                                                        THEN eTKD_Yousha03.SyaRyoTes
                                                ELSE 0
                                                END
                                        )
                END AS S_YouSyaRyo
        ,ISNULL(eTKD_YouSyu04.SyaSyuDai, 0) AS YouSyaSyuDai
        ,ISNULL(eTKD_YFutTu05.HaseiKin, 0) AS YfuUriGakKin
        ,ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) AS YfuSyaRyoSyo
        ,ISNULL(eTKD_YFutTu05.SyaRyoTes, 0) AS YfuSyaRyoTes
        ,CASE 
                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                        THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)
                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                        THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                        THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - (
                                        CASE 
                                                WHEN eVPM_TokiSt12.TesKbnFut = 2
                                                        THEN ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                                                ELSE 0
                                                END
                                        )
                END AS S_YfuSyaRyo
        ,ISNULL(eVPM_CodeKb14.CodeKbnNm, ' ') AS YouZKbnNm
FROM (
        SELECT eTKD_Yyksho01.UkeNo AS UkeNo
                ,ISNULL(eTKD_Unkobi02.UnkRen, 0) AS UnkRen
                ,eVPM_YoyKbn15.YoyaKbn AS YoyaKbn
                ,CASE 
                        WHEN eTKD_Yyksho01.YoyaSyu = 1
                                THEN ISNULL(eTKD_Unkobi02.UriYmd, ' ')
                        WHEN eTKD_Yyksho01.YoyaSyu = 2
                                THEN eTKD_Yyksho01.CanYmd
                        END AS UriYmd
                ,eTKD_Yyksho01.SeiEigCdSeq AS SeiEigCdSeq
                ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS SeiEigyoCd
                ,ISNULL(eVPM_Eigyos07.EigyoCd, 0) AS UkeEigyoCd
                ,ISNULL(eVPM_Eigyos06.CompanyCdSeq, ' ') AS CompanyCdSeq
                ,eTKD_Yyksho01.SiyoKbn AS Y_SiyoKbn
                ,ISNULL(eTKD_Unkobi02.SiyoKbn, 1) AS U_SiyoKbn
        FROM TKD_Yyksho AS eTKD_Yyksho01
        JOIN TKD_Unkobi AS eTKD_Unkobi02 ON eTKD_Unkobi02.UkeNo = eTKD_Yyksho01.UkeNo
                AND eTKD_Yyksho01.YoyaSyu = 1
        JOIN VPM_YoyKbn AS eVPM_YoyKbn15 ON eVPM_YoyKbn15.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
                        AND eVPM_YoyKbn15.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON eVPM_Eigyos06.EigyoCdSeq = eTKD_Yyksho01.SeiEigCdSeq
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos07 ON eVPM_Eigyos07.EigyoCdSeq = eTKD_Yyksho01.UkeEigCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eTKD_Yyksho01.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
        ) AS eTKD_Unkobi01
JOIN TKD_Yousha AS eTKD_Yousha03 ON eTKD_Yousha03.UkeNo = eTKD_Unkobi01.UkeNo
        AND eTKD_Yousha03.UnkRen = eTKD_Unkobi01.UnkRen
        AND eTKD_Yousha03.SiyoKbn = 1
LEFT JOIN (
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
        ) AS eTKD_YouSyu04 ON eTKD_YouSyu04.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YouSyu04.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YouSyu04.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN (
        SELECT eTKD_YFutTu01.UkeNo AS UkeNo
                ,eTKD_YFutTu01.UnkRen AS UnkRen
                ,eTKD_YFutTu01.YouTblSeq AS YouTblSeq
                ,SUM(eTKD_YFutTu01.HaseiKin) AS HaseiKin
                ,SUM(eTKD_YFutTu01.SyaRyoSyo) AS SyaRyoSyo
                ,SUM(eTKD_YFutTu01.SyaRyoTes) AS SyaRyoTes
        FROM TKD_YFutTu AS eTKD_YFutTu01
        WHERE eTKD_YFutTu01.SiyoKbn = 1
        AND SUBSTRING(eTKD_YFutTu01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
        GROUP BY eTKD_YFutTu01.UkeNo
                ,eTKD_YFutTu01.UnkRen
                ,eTKD_YFutTu01.YouTblSeq
        ) AS eTKD_YFutTu05 ON eTKD_YFutTu05.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YFutTu05.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YFutTu05.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb14 ON eVPM_CodeKb14.CodeSyu = 'ZEIKBN'
        AND eVPM_CodeKb14.CodeKbn = dbo.FP_RpZero(2, eTKD_Yousha03.ZeiKbn)
        AND eVPM_CodeKb14.TenantCdSeq = (
                SELECT CASE 
                                WHEN COUNT(*) = 0
                                        THEN 0
                                ELSE @TenantCdSeq -- Login Tenant
                                END AS TenantCdSeq
                FROM VPM_CodeKb
                WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'
                        AND VPM_CodeKb.SiyoKbn = 1
                        AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant
                )
LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON eVPM_Tokisk11.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd AND eVPM_Tokisk11.SiyoEndYmd
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt12 ON eVPM_TokiSt12.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eVPM_TokiSt12.SitenCdSeq = eTKD_Yousha03.YouSitCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt12.SiyoStaYmd AND eVPM_TokiSt12.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya13 ON eVPM_Gyosya13.GyosyaCdSeq = eVPM_Tokisk11.GyosyaCdSeq
LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
        AND eVPM_Tenant.SiyoKbn = 1
WHERE 
		eVPM_Compny17.CompanyCd = CASE WHEN @CompanyCd IS NOT NULL THEN @CompanyCd
											ELSE eVPM_Compny17.CompanyCd END
        AND eTKD_Unkobi01.SeiEigyoCd = CASE WHEN @EigyoKbn = 1 THEN @EigyoCdSeq
                                            ELSE eTKD_Unkobi01.SeiEigyoCd END
        AND eTKD_Unkobi01.UkeEigyoCd = CASE WHEN @EigyoKbn = 2 THEN @EigyoCdSeq
                                            ELSE eTKD_Unkobi01.UkeEigyoCd END 
        AND eTKD_Unkobi01.UriYmd = CASE WHEN @UriYmdTo IS NOT NULL THEN @UriYmdTo
											ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UkeNo >= CASE WHEN @UkeNoFrom IS NOT NULL THEN @UkeNoFrom
											ELSE eTKD_Unkobi01.UkeNo END
		AND eTKD_Unkobi01.UkeNo <= CASE WHEN @UkeNoTo IS NOT NULL THEN @UkeNoTo
										ELSE eTKD_Unkobi01.UkeNo END
		AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn  >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn  <= @YoyaKbnTo))
        AND eVPM_Tenant.TenantCdSeq = @TenantCdSeq
--      ********************************************************************************************************************************
--      明細区分：１（頁計）
--      ********************************************************************************************************************************

UNION ALL

SELECT 1 AS MesaiKbn -- 明細区分
        ,'' AS UkeNo --  受付番号
        ,0 AS UnkRen -- 運行日連番
        ,0 AS YouTblSeq --      傭車テーブルＳＥＱ
        ,0 AS YouCd --  傭車先コード
        ,' ' AS YouNm --        傭車先名
        ,' ' AS YouRyakuNm --   傭車先略名
        ,0 AS YouSitCd --       傭車先支店コード
        ,' ' AS YouSitenNm --   傭車先支店名
        ,' ' AS YouSitRyakuNm --        傭車先支店略名
        ,0 AS YouGyosyaCd --    傭車先業者コード
        ,' ' AS YouGyosyaNm --  傭車先業者コード名
        ,SUM(eTKD_Yousha03.SyaRyoUnc) AS YouSyaRyoUnc --        傭車項目・運賃
        ,0 AS YouZeiKbn --      傭車項目・税区分
        ,0 AS YouZeiritsu --    傭車項目・消費税率
        ,SUM(eTKD_Yousha03.SyaRyoSyo) AS YouSyaRyoSyo --        傭車項目・消費税額
        ,0 AS YouTesuRitu --    傭車項目・手数料率
        ,SUM(eTKD_Yousha03.SyaRyoTes) AS YouSyaRyoTes --        傭車項目・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - eTKD_Yousha03.SyaRyoTes
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - (
                                                CASE 
                                                        WHEN eVPM_TokiSt12.TesKbn = 2
                                                                THEN eTKD_Yousha03.SyaRyoTes
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_YouSyaRyo --  傭車項目・合計
        ,SUM(ISNULL(eTKD_YouSyu04.SyaSyuDai, 0)) AS YouSyaSyuDai --     傭車項目・台数
        ,SUM(ISNULL(eTKD_YFutTu05.HaseiKin, 0)) AS YfuUriGakKin --      傭車付帯・売上額
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)) AS YfuSyaRyoSyo --     傭車付帯・消費税額
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)) AS YfuSyaRyoTes --     傭車付帯・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt12.TesKbnFut = 2
                                                                THEN ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_YfuSyaRyo --  傭車付帯・合計
        ,' ' AS YouZKbnNm --    傭車項目・税区分名
FROM (
        SELECT eTKD_Yyksho01.UkeNo AS UkeNo --  受付番号
                ,ISNULL(eTKD_Unkobi02.UnkRen, 0) AS UnkRen --   運行日連番
                ,eVPM_YoyKbn15.YoyaKbn AS YoyaKbn --    予約区分
                ,CASE 
                        WHEN eTKD_Yyksho01.YoyaSyu = 1
                                THEN ISNULL(eTKD_Unkobi02.UriYmd, ' ')
                        WHEN eTKD_Yyksho01.YoyaSyu = 2
                                THEN eTKD_Yyksho01.CanYmd
                        END AS UriYmd --        売上年月日（キャンセル年月日）
                ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS SeiEigyoCd --      請求営業所コード
                ,ISNULL(eVPM_Eigyos07.EigyoCd, 0) AS UkeEigyoCd --      受付営業所コード
				,ISNULL(CASE 
                WHEN @EigyoKbn = 1 THEN eVPM_Eigyos06.CompanyCdSeq	--     請求会社コードＳＥＱ
                WHEN @EigyoKbn = 2 THEN eVPM_Eigyos07.CompanyCdSeq --     受付会社コードＳＥＱ
				END, ' ') AS CompanyCdSeq
                ,eTKD_Yyksho01.SiyoKbn AS Y_SiyoKbn --  予約書・使用区分
                ,ISNULL(eTKD_Unkobi02.SiyoKbn, 1) AS U_SiyoKbn --       運行日・使用区分
        FROM TKD_Yyksho AS eTKD_Yyksho01
        JOIN TKD_Unkobi AS eTKD_Unkobi02 ON eTKD_Unkobi02.UkeNo = eTKD_Yyksho01.UkeNo
                AND eTKD_Yyksho01.YoyaSyu = 1
        JOIN VPM_YoyKbn AS eVPM_YoyKbn15 ON eVPM_YoyKbn15.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
        AND eVPM_YoyKbn15.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON eVPM_Eigyos06.EigyoCdSeq = eTKD_Yyksho01.SeiEigCdSeq
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos07 ON eVPM_Eigyos07.EigyoCdSeq = eTKD_Yyksho01.UkeEigCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eTKD_Yyksho01.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
        ) AS eTKD_Unkobi01
JOIN TKD_Yousha AS eTKD_Yousha03 ON eTKD_Yousha03.UkeNo = eTKD_Unkobi01.UkeNo
        AND eTKD_Yousha03.UnkRen = eTKD_Unkobi01.UnkRen
LEFT JOIN (
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
        ) AS eTKD_YouSyu04 ON eTKD_YouSyu04.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YouSyu04.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YouSyu04.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN (
        SELECT eTKD_YFutTu01.UkeNo AS UkeNo
                ,eTKD_YFutTu01.UnkRen AS UnkRen
                ,eTKD_YFutTu01.YouTblSeq AS YouTblSeq
                ,SUM(eTKD_YFutTu01.HaseiKin) AS HaseiKin --     発生額
                ,SUM(eTKD_YFutTu01.SyaRyoSyo) AS SyaRyoSyo --   消費税額
                ,SUM(eTKD_YFutTu01.SyaRyoTes) AS SyaRyoTes --   手数料額
        FROM TKD_YFutTu AS eTKD_YFutTu01
        WHERE eTKD_YFutTu01.SiyoKbn = 1
        AND SUBSTRING(eTKD_YFutTu01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
        GROUP BY eTKD_YFutTu01.UkeNo
                ,eTKD_YFutTu01.UnkRen
                ,eTKD_YFutTu01.YouTblSeq
        ) AS eTKD_YFutTu05 ON eTKD_YFutTu05.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YFutTu05.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YFutTu05.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb14 ON eVPM_CodeKb14.CodeSyu = 'ZEIKBN'
        AND eVPM_CodeKb14.CodeKbn = dbo.FP_RpZero(2, eTKD_Yousha03.ZeiKbn)
        AND eVPM_CodeKb14.TenantCdSeq = (
                SELECT CASE 
                                WHEN COUNT(*) = 0
                                        THEN 0
                                ELSE @TenantCdSeq -- Login Tenant                               
                                END AS TenantCdSeq
                FROM VPM_CodeKb
                WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'
                        AND VPM_CodeKb.SiyoKbn = 1
                        AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant                               
                )
LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON eVPM_Tokisk11.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd AND eVPM_Tokisk11.SiyoEndYmd
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt12 ON eVPM_TokiSt12.TokuiSeq = eTKD_Yousha03.YouCdSeq
        AND eVPM_TokiSt12.SitenCdSeq = eTKD_Yousha03.YouSitCdSeq
        AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt12.SiyoStaYmd AND eVPM_TokiSt12.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya13 ON eVPM_Gyosya13.GyosyaCdSeq = eVPM_Tokisk11.GyosyaCdSeq
LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
        AND eVPM_Tenant.SiyoKbn = 1
WHERE 
		eVPM_Compny17.CompanyCd = CASE WHEN @CompanyCd IS NOT NULL THEN @CompanyCd
											ELSE eVPM_Compny17.CompanyCd END
        AND eTKD_Unkobi01.SeiEigyoCd = CASE WHEN @EigyoKbn = 1 THEN @EigyoCdSeq
                                            ELSE eTKD_Unkobi01.SeiEigyoCd END
        AND eTKD_Unkobi01.UkeEigyoCd = CASE WHEN @EigyoKbn = 2 THEN @EigyoCdSeq
                                            ELSE eTKD_Unkobi01.UkeEigyoCd END 
        AND eTKD_Unkobi01.UriYmd = CASE WHEN @UriYmdTo IS NOT NULL THEN @UriYmdTo
											ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UkeNo >= CASE WHEN @UkeNoFrom IS NOT NULL THEN @UkeNoFrom
											ELSE eTKD_Unkobi01.UkeNo END
		AND eTKD_Unkobi01.UkeNo <= CASE WHEN @UkeNoTo IS NOT NULL THEN @UkeNoTo
										ELSE eTKD_Unkobi01.UkeNo END
		AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND eVPM_Tenant.TenantCdSeq = @TenantCdSeq

--      ********************************************************************************************************************************
--      明細区分：２（累計）
--      ********************************************************************************************************************************

UNION ALL

SELECT 2 AS MesaiKbn -- 明細区分1
        ,'' AS UkeNo --  受付番号2
        ,0 AS UnkRen -- 運行日連番3
        ,0 AS YouTblSeq --      傭車テーブルＳＥＱ4
        ,0 AS YouCd --  傭車先コード5
        ,' ' AS YouNm --        傭車先名6
        ,' ' AS YouRyakuNm --   傭車先略名7
        ,0 AS YouSitCd --       傭車先支店コード8
        ,' ' AS YouSitenNm --   傭車先支店名9
        ,' ' AS YouSitRyakuNm --        傭車先支店略名0
        ,0 AS YouGyosyaCd --    傭車先業者コード1
        ,' ' AS YouGyosyaNm --  傭車先業者コード名2
        ,SUM(eTKD_Yousha03.SyaRyoUnc) AS YouSyaRyoUnc --        傭車項目・運賃3
        ,0 AS YouZeiKbn --      傭車項目・税区分4
        ,0 AS YouZeiritsu --    傭車項目・消費税率5
        ,SUM(eTKD_Yousha03.SyaRyoSyo) AS YouSyaRyoSyo --        傭車項目・消費税額6
        ,0 AS YouTesuRitu --    傭車項目・手数料率7
        ,SUM(eTKD_Yousha03.SyaRyoTes) AS YouSyaRyoTes --        傭車項目・手数料額8
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - eTKD_Yousha03.SyaRyoTes
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN eTKD_Yousha03.SyaRyoUnc + eTKD_Yousha03.SyaRyoSyo - (
                                                CASE 
                                                        WHEN eVPM_TokiSt12.TesKbn = 2
                                                                THEN eTKD_Yousha03.SyaRyoTes
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_YouSyaRyo --  傭車項目・合計9
        ,SUM(ISNULL(eTKD_YouSyu04.SyaSyuDai, 0)) AS YouSyaSyuDai --     傭車項目・台数0
        ,SUM(ISNULL(eTKD_YFutTu05.HaseiKin, 0)) AS YfuUriGakKin --      傭車付帯・売上額1
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)) AS YfuSyaRyoSyo --     傭車付帯・消費税額2
        ,SUM(ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)) AS YfuSyaRyoTes --     傭車付帯・手数料額3
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_YFutTu05.HaseiKin, 0) + ISNULL(eTKD_YFutTu05.SyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt12.TesKbnFut = 2
                                                                THEN ISNULL(eTKD_YFutTu05.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_YfuSyaRyo --  傭車付帯・合計4
        ,' ' AS YouZKbnNm --    傭車項目・税区分名
FROM (
        SELECT eTKD_Yyksho01.UkeNo AS UkeNo --  受付番号
                ,ISNULL(eTKD_Unkobi02.UnkRen, 0) AS UnkRen --   運行日連番
                ,eVPM_YoyKbn15.YoyaKbn AS YoyaKbn --    予約区分
                ,CASE 
                        WHEN eTKD_Yyksho01.YoyaSyu = 1
                                THEN ISNULL(eTKD_Unkobi02.UriYmd, ' ')
                        WHEN eTKD_Yyksho01.YoyaSyu = 2
                                THEN eTKD_Yyksho01.CanYmd
                        END AS UriYmd --        売上年月日（キャンセル年月日）
                ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS SeiEigyoCd --      請求営業所コード
                ,ISNULL(eVPM_Eigyos07.EigyoCd, 0) AS UkeEigyoCd --      受付営業所コード
                ,ISNULL(CASE 
                WHEN @EigyoKbn = 1 THEN eVPM_Eigyos06.CompanyCdSeq	--     請求会社コードＳＥＱ
                WHEN @EigyoKbn = 2 THEN eVPM_Eigyos07.CompanyCdSeq --     受付会社コードＳＥＱ
				END, ' ') AS CompanyCdSeq
                ,eTKD_Yyksho01.SiyoKbn AS Y_SiyoKbn --  予約書・使用区分
                ,ISNULL(eTKD_Unkobi02.SiyoKbn, 1) AS U_SiyoKbn --       運行日・使用区分
        FROM TKD_Yyksho AS eTKD_Yyksho01
        JOIN TKD_Unkobi AS eTKD_Unkobi02 ON eTKD_Unkobi02.UkeNo = eTKD_Yyksho01.UkeNo
                AND eTKD_Yyksho01.YoyaSyu = 1
        JOIN VPM_YoyKbn AS eVPM_YoyKbn15 ON eVPM_YoyKbn15.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
        AND eVPM_YoyKbn15.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON eVPM_Eigyos06.EigyoCdSeq = eTKD_Yyksho01.SeiEigCdSeq
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos07 ON eVPM_Eigyos07.EigyoCdSeq = eTKD_Yyksho01.UkeEigCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eTKD_Yyksho01.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
        ) AS eTKD_Unkobi01
JOIN TKD_Yousha AS eTKD_Yousha03 ON eTKD_Yousha03.UkeNo = eTKD_Unkobi01.UkeNo
        AND eTKD_Yousha03.UnkRen = eTKD_Unkobi01.UnkRen
        AND eTKD_Yousha03.SiyoKbn = 1
LEFT JOIN (
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
        ) AS eTKD_YouSyu04 ON eTKD_YouSyu04.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YouSyu04.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YouSyu04.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN (
        SELECT eTKD_YFutTu01.UkeNo AS UkeNo
                ,eTKD_YFutTu01.UnkRen AS UnkRen
                ,eTKD_YFutTu01.YouTblSeq AS YouTblSeq
                ,SUM(eTKD_YFutTu01.HaseiKin) AS HaseiKin --     発生額
                ,SUM(eTKD_YFutTu01.SyaRyoSyo) AS SyaRyoSyo --   消費税額
                ,SUM(eTKD_YFutTu01.SyaRyoTes) AS SyaRyoTes --   手数料額
        FROM TKD_YFutTu AS eTKD_YFutTu01
        WHERE eTKD_YFutTu01.SiyoKbn = 1
        AND SUBSTRING(eTKD_YFutTu01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
        GROUP BY eTKD_YFutTu01.UkeNo
                ,eTKD_YFutTu01.UnkRen
                ,eTKD_YFutTu01.YouTblSeq
        ) AS eTKD_YFutTu05 ON eTKD_YFutTu05.UkeNo = eTKD_Yousha03.UkeNo
        AND eTKD_YFutTu05.UnkRen = eTKD_Yousha03.UnkRen
        AND eTKD_YFutTu05.YouTblSeq = eTKD_Yousha03.YouTblSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb14 ON eVPM_CodeKb14.CodeSyu = 'ZEIKBN'
        AND eVPM_CodeKb14.CodeKbn = dbo.FP_RpZero(2, eTKD_Yousha03.ZeiKbn)
        AND eVPM_CodeKb14.TenantCdSeq = (
                SELECT CASE 
                                WHEN COUNT(*) = 0
                                        THEN 0
                                ELSE @TenantCdSeq -- Login Tenant
                                END AS TenantCdSeq
                FROM VPM_CodeKb
                WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'
                        AND VPM_CodeKb.SiyoKbn = 1
                        AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant
                )
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
        AND eTKD_Unkobi01.SeiEigyoCd = CASE WHEN @EigyoKbn = 1 THEN @EigyoCdSeq
                                            ELSE eTKD_Unkobi01.SeiEigyoCd END
        AND eTKD_Unkobi01.UkeEigyoCd = CASE WHEN @EigyoKbn = 2 THEN @EigyoCdSeq
                                            ELSE eTKD_Unkobi01.UkeEigyoCd END 
        AND eTKD_Unkobi01.UriYmd >= CASE WHEN @UriYmdFrom IS NOT NULL THEN @UriYmdFrom
											ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UriYmd <= CASE WHEN @UriYmdTo IS NOT NULL THEN @UriYmdTo
											ELSE eTKD_Unkobi01.UriYmd END
        AND eTKD_Unkobi01.UkeNo >= CASE WHEN @UkeNoFrom IS NOT NULL THEN @UkeNoFrom
											ELSE eTKD_Unkobi01.UkeNo END
		AND eTKD_Unkobi01.UkeNo <= CASE WHEN @UkeNoTo IS NOT NULL THEN @UkeNoTo
										ELSE eTKD_Unkobi01.UkeNo END
		AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn  >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn  <= @YoyaKbnTo))
        AND eVPM_Tenant.TenantCdSeq = @TenantCdSeq
ORDER BY UkeNo
        ,UnkRen
        ,YouCd
END
GO