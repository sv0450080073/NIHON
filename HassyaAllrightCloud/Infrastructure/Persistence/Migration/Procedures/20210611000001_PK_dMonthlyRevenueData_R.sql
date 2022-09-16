USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[PK_dMonthlyRevenueData_R]    Script Date: 2020/08/14 16:52:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   RevenueSummary
-- SP-ID		:   PK_dMonthlyRevenueData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetMonthlyRevenueData
-- Date			:   2020/08/12
-- Author		:   Tra Nguyen 
-- Description	:   Get Monthly Transportation Revenue Data
------------------------------------------------------------
-- Update		:   2020/10/16
-- Comment		:	Remove join to table TKD_HaishaExp
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dMonthlyRevenueData_R]
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
IF (@TesuInKbn = 2)
	BEGIN
		;WITH eTKD_Unkobi_1
        AS (
                SELECT eTKD_Yyksho01.UkeNo AS UkeNo -- 受付番号
                        ,ISNULL(eTKD_Unkobi02.UnkRen, 0) AS UnkRen -- 運行日連番
                        ,eTKD_Yyksho01.YoyaSyu AS YoyaSyu -- 予約種別
                        ,eVPM_YoyKbn15.YoyaKbn AS YoyaKbn -- 予約区分
                        ,CASE 
                                WHEN eTKD_Yyksho01.YoyaSyu = 1
                                        THEN ISNULL(eTKD_Unkobi02.UriYmd, ' ')
                                WHEN eTKD_Yyksho01.YoyaSyu = 2
                                        THEN eTKD_Yyksho01.CanYmd
                                END AS UriYmd -- 売上年月日（キャンセル年月日）
                        ,CASE WHEN @EigyoKbn = 1 THEN eTKD_Yyksho01.SeiEigCdSeq -- 請求営業所コードＳＥＱ
					            ELSE eTKD_Yyksho01.UkeEigCdSeq END AS EigCdSeq  -- 受付営業所コードＳＥＱ -- @EigyoKbn = 2
                        ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS EigyoCd -- 請求営業所コード / 受付営業所コード
                        ,ISNULL(eVPM_Eigyos06.EigyoNm, ' ') AS EigyoNm -- 請求営業所名 / 受付営業所名
                        ,ISNULL(eVPM_Eigyos06.RyakuNm, ' ') AS RyakuNm -- 請求営業所略名 / 受付営業所略名
                        ,ISNULL(eVPM_Eigyos06.CompanyCdSeq, ' ') AS CompanyCdSeq -- 請求会社コードＳＥＱ / 受付会社コードＳＥＱ
                        -- IF @TesuInKbn = 2:  Add start
                        ,eTKD_Yyksho01.TokuiSeq AS TokuiSeq -- 得意先ＳＥＱ
                        ,eTKD_Yyksho01.SitenCdSeq AS SitenCdSeq -- 支店コードＳＥＱ
                        -- IF @TesuInKbn = 2:  Add end
                        ,eTKD_Yyksho01.SirCdSeq AS SirCdSeq -- 仕入先コードＳＥＱ
                        ,eTKD_Yyksho01.SirSitenCdSeq AS SirSitenCdSeq -- 仕入先支店コードＳＥＱ
                        ,CASE 
                                WHEN eTKD_Yyksho01.YoyaSyu = 1
                                        THEN 0
                                WHEN eTKD_Yyksho01.YoyaSyu = 2
                                        THEN eTKD_Yyksho01.CanUnc
                                END AS CanUnc -- キャンセル料・運賃
                        ,CASE 
                                WHEN eTKD_Yyksho01.YoyaSyu = 1
                                        THEN 0
                                WHEN eTKD_Yyksho01.YoyaSyu = 2
                                        THEN eTKD_Yyksho01.CanSyoG
                                END AS CanSyoG -- キャンセル料・消費税額
                        ,eTKD_Yyksho01.SiyoKbn AS Y_SiyoKbn -- 予約書・使用区分
                        ,ISNULL(eTKD_Unkobi02.SiyoKbn, 1) AS U_SiyoKbn -- 運行日・使用区分
                FROM TKD_Yyksho AS eTKD_Yyksho01
                LEFT JOIN TKD_Unkobi AS eTKD_Unkobi02 ON eTKD_Unkobi02.UkeNo = eTKD_Yyksho01.UkeNo
                        AND eTKD_Yyksho01.YoyaSyu = 1
                JOIN VPM_YoyKbn AS eVPM_YoyKbn15 ON eVPM_YoyKbn15.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
                                AND eVPM_YoyKbn15.TenantCdSeq = @TenantCdSeq
                INNER JOIN VPM_Tokisk AS eVPM_Tokisk06 ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk06.TokuiSeq
                        AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk06.SiyoStaYmd AND eVPM_Tokisk06.SiyoEndYmd
                INNER JOIN VPM_TokiSt AS eVPM_TokiSt07 ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt07.TokuiSeq
                        AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt07.SitenCdSeq
                        AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt07.SiyoStaYmd AND eVPM_TokiSt07.SiyoEndYmd
                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eTKD_Yyksho01.TenantCdSeq
                        AND eVPM_Tenant.SiyoKbn = 1
                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON eVPM_Eigyos06.EigyoCdSeq = CASE WHEN @EigyoKbn = 1 THEN eTKD_Yyksho01.SeiEigCdSeq
																				ELSE eTKD_Yyksho01.UkeEigCdSeq END -- @EigyoKbn = 2
                WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
                )
                ,eTKD_YykSyu_1
        AS (
                SELECT eTKD_Haisha05.UkeNo AS UkeNo
                        ,eTKD_Haisha05.UnkRen AS UnkRen
                        ,count(*) AS SyaSyuDai --       台数
                FROM TKD_Haisha AS eTKD_Haisha05
                WHERE eTKD_Haisha05.SiyoKbn = 1
                        AND eTKD_Haisha05.YouTblSeq = 0
                        AND SUBSTRING(eTKD_Haisha05.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_Haisha05.UkeNo
                        ,eTKD_Haisha05.UnkRen
                )
                ,eTKD_Haisha_1
        AS (
                SELECT eTKD_Haisha01.UkeNo AS UkeNo
                        ,eTKD_Haisha01.UnkRen AS UnkRen
                        ,CONVERT(bigint, SUM(eTKD_Haisha01.SyaRyoUnc)) AS SyaRyoUnc -- 受注運賃
                        ,CONVERT(bigint, SUM(eTKD_Haisha01.SyaRyoSyo)) AS SyaRyoSyo -- 受注消費税額
                        ,CONVERT(bigint, SUM(eTKD_Haisha01.SyaRyoTes)) AS SyaRyoTes -- 受注手数料額
                        ,CONVERT(bigint, SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN eTKD_Haisha01.SyaRyoUnc
                                        ELSE 0
                                        END)) AS JisSyaRyoUnc -- 自社運賃
                        ,CONVERT(bigint, SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN eTKD_Haisha01.SyaRyoSyo
                                        ELSE 0
                                        END)) AS JisSyaRyoSyo -- 自社消費税額
                        ,CONVERT(bigint, SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN eTKD_Haisha01.SyaRyoTes
                                        ELSE 0
                                        END)) AS JisSyaRyoTes -- 自社手数料額
                        ,SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN 0
                                        ELSE eTKD_Haisha01.YoushaUnc
                                        END) AS YoushaUnc -- 傭車運賃
                        ,SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN 0
                                        ELSE eTKD_Haisha01.YoushaSyo
                                        END) AS YoushaSyo -- 傭車消費税
                        ,SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN 0
                                        ELSE eTKD_Haisha01.YoushaTes
                                        END) AS YoushaTes -- 傭車手数料額
                FROM TKD_Haisha AS eTKD_Haisha01
                WHERE eTKD_Haisha01.SiyoKbn = 1
                AND SUBSTRING(eTKD_Haisha01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_Haisha01.UkeNo
                        ,eTKD_Haisha01.UnkRen
                )
                ,eTKD_FutTum_1
        AS (
                SELECT eTKD_FutTum01.UkeNo AS UkeNo
                        ,eTKD_FutTum01.UnkRen AS UnkRen
                        ,eTKD_FutTum01.FutGuiKbn AS FutGuiKbn
                        ,eTKD_FutTum01.FutHotelCd AS FutHotelCd
                        ,eTKD_FutTum01.FutParkingCd AS FutParkingCd
                        ,SUM(eTKD_FutTum01.UriGakKin) AS UriGakKin
                        ,SUM(eTKD_FutTum01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_FutTum01.SyaRyoTes) AS SyaRyoTes
                FROM (
                        SELECT eTKD_FutTum01.UkeNo AS UkeNo
                                ,eTKD_FutTum01.UnkRen AS UnkRen
                                ,CASE 
                                        WHEN eVPM_Futai02.FutGuiKbn IN (3, 5)
                                                THEN eVPM_Futai02.FutGuiKbn
                                        ELSE 0
                                        END AS FutGuiKbn
                                ,ISNULL(eVPM_CodeKb01.CodeKbn, '') AS FutHotelCd
                                ,ISNULL(eVPM_CodeKb02.CodeKbn, '') AS FutParkingCd
                                ,eTKD_FutTum01.UriGakKin AS UriGakKin
                                ,eTKD_FutTum01.SyaRyoSyo AS SyaRyoSyo
                                ,eTKD_FutTum01.SyaRyoTes AS SyaRyoTes
                        FROM TKD_FutTum AS eTKD_FutTum01
                        LEFT JOIN VPM_Futai AS eVPM_Futai02 ON eVPM_Futai02.FutaiCdSeq = eTKD_FutTum01.FutTumCdSeq
                        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeKbn = eVPM_Futai02.FutaiCd
                                AND eVPM_CodeKb01.CodeSyu = 'HOTELFUT'
                                AND eVPM_CodeKb01.TenantCdSeq = (
                                        SELECT CASE 
                                                        WHEN COUNT(*) = 0
                                                                THEN 0
                                                        ELSE @TenantCdSeq -- Login Tenant
                                                        END AS TenantCdSeq
                                        FROM VPM_CodeKb
                                        WHERE VPM_CodeKb.CodeSyu = 'HOTELFUT'
                                                AND VPM_CodeKb.SiyoKbn = 1
                                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant
                                        )
                        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_CodeKb02.CodeKbn = eVPM_Futai02.FutaiCd
                                AND eVPM_CodeKb02.CodeSyu = 'PARKINGFUT'
                                AND eVPM_CodeKb02.TenantCdSeq = (
                                        SELECT CASE 
                                                        WHEN COUNT(*) = 0
                                                                THEN 0
                                                        ELSE @TenantCdSeq -- Login Tenant
                                                        END AS TenantCdSeq
                                        FROM VPM_CodeKb
                                        WHERE VPM_CodeKb.CodeSyu = 'PARKINGFUT'
                                                AND VPM_CodeKb.SiyoKbn = 1
                                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant
                                        )
                        WHERE eTKD_FutTum01.SeisanKbn = 1
                                AND eTKD_FutTum01.SiyoKbn = 1
                                AND SUBSTRING(eTKD_FutTum01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                        ) AS eTKD_FutTum01
                GROUP BY eTKD_FutTum01.UkeNo
                        ,eTKD_FutTum01.UnkRen
                        ,eTKD_FutTum01.FutGuiKbn
                        ,eTKD_FutTum01.FutHotelCd
                        ,eTKD_FutTum01.FutParkingCd
                )
                ,eTKD_FutTum_2
        AS (
                SELECT eTKD_FutTum01.UkeNo AS UkeNo
                        ,eTKD_FutTum01.UnkRen AS UnkRen
                        ,SUM(eTKD_FutTum01.UriGakKin) AS UriGakKin
                        ,SUM(eTKD_FutTum01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_FutTum01.SyaRyoTes) AS SyaRyoTes
                FROM eTKD_FutTum_1 AS eTKD_FutTum01
                WHERE SUBSTRING(eTKD_FutTum01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_FutTum01.UkeNo
                        ,eTKD_FutTum01.UnkRen
                )
                ,eTKD_FutTum_3
        AS (
                SELECT eTKD_FutTum01.UkeNo AS UkeNo
                        ,eTKD_FutTum01.UnkRen AS UnkRen
                        ,CASE 
                                WHEN eTKD_FutTum01.FutGuiKbn = 5
                                        THEN eTKD_FutTum01.FutGuiKbn
                                ELSE 0
                                END AS FutGuiKbn
                        ,SUM(eTKD_FutTum01.UriGakKin) AS UriGakKin
                        ,SUM(eTKD_FutTum01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_FutTum01.SyaRyoTes) AS SyaRyoTes
                FROM eTKD_FutTum_1 AS eTKD_FutTum01
                WHERE SUBSTRING(eTKD_FutTum01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_FutTum01.UkeNo
                        ,eTKD_FutTum01.UnkRen
                        ,CASE 
                                WHEN eTKD_FutTum01.FutGuiKbn = 5
                                        THEN eTKD_FutTum01.FutGuiKbn
                                ELSE 0
                                END
                )
                ,eTKD_YFutTu_1
        AS (
                SELECT eTKD_YFutTu01.UkeNo AS UkeNo
                        ,eTKD_YFutTu01.UnkRen AS UnkRen
                        ,SUM(eTKD_YFutTu01.HaseiKin) AS HaseiKin
                        ,SUM(eTKD_YFutTu01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_YFutTu01.SyaRyoTes) AS SyaRyoTes
                FROM TKD_YFutTu AS eTKD_YFutTu01
                WHERE eTKD_YFutTu01.SiyoKbn = 1
                AND SUBSTRING(eTKD_YFutTu01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_YFutTu01.UkeNo
                        ,eTKD_YFutTu01.UnkRen
                )
        -- ********************************************************************************************************************************
        -- 明細区分：３（明細）
        -- ********************************************************************************************************************************
        SELECT 3 AS MesaiKbn -- 明細区分
                ,eTKD_Unkobi01.UriYmd AS UriYmd -- 売上年月日
                ,SUM(eTKD_Unkobi01.CanUnc) AS CanUnc -- キャンセル料・運賃
                ,SUM(eTKD_Unkobi01.CanSyoG) AS CanSyoG -- キャンセル料・消費税額
                ,SUM(eTKD_Unkobi01.CanUnc + eTKD_Unkobi01.CanSyoG) AS CanSum -- キャンセル料・合計
                ,eTKD_Unkobi01.EigCdSeq AS EigCdSeq -- 営業所コードＳＥＱ
                ,eTKD_Unkobi01.EigyoCd AS EigyoCd -- 営業所コード
                ,eTKD_Unkobi01.EigyoNm AS EigyoNm -- 営業所名
                ,eTKD_Unkobi01.RyakuNm AS EigyoRyak -- 営業所略名
                ,SUM(ISNULL(eTKD_YykSyu02.SyaSyuDai, 0)) AS JisSyaSyuDai -- 自社項目・台数
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha03.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha03.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS KeiKin -- 契約運賃
                ,SUM(ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0)) AS JisSyaRyoUnc -- 自社項目・運賃
                ,SUM(ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0)) AS JisSyaRyoSyo -- 自社項目・消費税額
                ,SUM(ISNULL(eTKD_Haisha03.JisSyaRyoTes, 0)) AS JisSyaRyoTes -- 自社項目・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha03.JisSyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha03.JisSyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS JisSyaRyoSum -- 自社項目・合計
                ,SUM(ISNULL(eTKD_FutTum04.UriGakKin, 0)) AS GaiUriGakKin -- 自社ガイド料・売上額
                ,SUM(ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)) AS GaiSyaRyoSyo -- 自社ガイド料・消費税額
                ,SUM(ISNULL(eTKD_FutTum04.SyaRyoTes, 0)) AS GaiSyaRyoTes -- 自社ガイド料・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum04.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbnGui = 2
                                                                        THEN ISNULL(eTKD_FutTum04.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS GaiSyaRyoSum -- 自社ガイド料・合計
                ,SUM(ISNULL(eTKD_FutTum05.UriGakKin, 0)) AS EtcUriGakKin -- 自社その他付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)) AS EtcSyaRyoSyo -- 自社その他付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum05.SyaRyoTes, 0)) AS EtcSyaRyoTes -- 自社その他付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum05.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbnFut = 2
                                                                        THEN ISNULL(eTKD_FutTum05.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS EtcSyaRyoSum -- 自社その他付帯・合計
                ,SUM(ISNULL(eTKD_FutTum06.UriGakKin, 0)) AS HighwayUriGakKin -- 自社通行料付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum06.SyaRyoSyo, 0)) AS HighwaySyaRyoSyo -- 自社通行料付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum06.SyaRyoTes, 0)) AS HighwaySyaRyoTes -- 自社通行料付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum06.UriGakKin, 0) + ISNULL(eTKD_FutTum06.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum06.UriGakKin, 0) + ISNULL(eTKD_FutTum06.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum06.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_FutTum06.UriGakKin, 0) + ISNULL(eTKD_FutTum06.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbnFut = 2
                                                                        THEN ISNULL(eTKD_FutTum06.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS HighwaySyaRyoSum -- 自社通行料付帯・合計
                ,SUM(ISNULL(eTKD_FutTum07.UriGakKin, 0)) AS HotelUriGakKin -- 自社宿泊料付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum07.SyaRyoSyo, 0)) AS HotelSyaRyoSyo -- 自社宿泊料付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum07.SyaRyoTes, 0)) AS HotelSyaRyoTes -- 自社宿泊料付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum07.UriGakKin, 0) + ISNULL(eTKD_FutTum07.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum07.UriGakKin, 0) + ISNULL(eTKD_FutTum07.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum07.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_FutTum07.UriGakKin, 0) + ISNULL(eTKD_FutTum07.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbnFut = 2
                                                                        THEN ISNULL(eTKD_FutTum07.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS HotelSyaRyoSum -- 自社宿泊料付帯・合計
                ,SUM(ISNULL(eTKD_FutTum08.UriGakKin, 0)) AS ParkingUriGakKin -- 自社駐車料付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum08.SyaRyoSyo, 0)) AS ParkingSyaRyoSyo -- 自社駐車料付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum08.SyaRyoTes, 0)) AS ParkingSyaRyoTes -- 自社駐車料付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum08.UriGakKin, 0) + ISNULL(eTKD_FutTum08.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum08.UriGakKin, 0) + ISNULL(eTKD_FutTum08.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum08.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_FutTum08.UriGakKin, 0) + ISNULL(eTKD_FutTum08.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbnFut = 2
                                                                        THEN ISNULL(eTKD_FutTum08.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS ParkingSyaRyoSum -- 自社駐車料付帯・合計
                ,SUM(ISNULL(eTKD_FutTum09.UriGakKin, 0)) AS OtherUriGakKin -- 自社その他付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum09.SyaRyoSyo, 0)) AS OtherSyaRyoSyo -- 自社その他付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum09.SyaRyoTes, 0)) AS OtherSyaRyoTes -- 自社その他付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum09.UriGakKin, 0) + ISNULL(eTKD_FutTum09.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum09.UriGakKin, 0) + ISNULL(eTKD_FutTum09.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum09.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_FutTum09.UriGakKin, 0) + ISNULL(eTKD_FutTum09.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbnFut = 2
                                                                        THEN ISNULL(eTKD_FutTum09.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS OtherSyaRyoSum -- 自社その他付帯・合計
                ,0 AS S_JyuSyaRyoUnc -- 日計/累計・受注運賃
                ,0 AS S_JyuSyaRyoSyo -- 日計/累計・受注消費税
                ,0 AS S_JyuSyaRyoTes -- 日計/累計・受注手数料額
                ,0 AS S_JyuSyaRyoRui -- 日計/累計・受注累計
                ,0 AS S_JisSyaRyoUnc -- 日計/累計・自社運賃
                ,0 AS S_JisSyaRyoSyo -- 日計/累計・自社消費税
                ,0 AS S_JisSyaRyoTes -- 日計/累計・自社手数料額
                ,0 AS S_FutUriGakKin -- 日計/累計・自社付帯売上
                ,0 AS S_FutSyaRyoSyo -- 日計/累計・自社付帯消費税
                ,0 AS S_FutSyaRyoTes -- 日計/累計・自社付帯手数料額
                ,0 AS S_YoushaUnc -- 日計/累計・傭車運賃
                ,0 AS S_YoushaSyo -- 日計/累計・傭車消費税
                ,0 AS S_YoushaTes -- 日計/累計・傭車手数料額
                ,0 AS S_YfuUriGakKin -- 日計/累計・傭車付帯売上
                ,0 AS S_YfuSyaRyoSyo -- 日計/累計・傭車付帯消費税
                ,0 AS S_YfuSyaRyoTes -- 日計/累計・傭車付帯手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha03.SyaRyoTes, 0)
                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha03.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS JyuSyaRyoRui -- 日計/累計・受注累計
                ,SUM(ISNULL(eTKD_Haisha03.SyaRyoSyo, 0)) AS SyaRyoSyo -- 自社項目・消費税額
                ,SUM(ISNULL(eTKD_Haisha03.SyaRyoTes, 0)) AS SyaRyoTes -- 自社項目・手数料額
                ,0 AS S_Soneki
        FROM eTKD_Unkobi_1 AS eTKD_Unkobi01
        LEFT JOIN eTKD_YykSyu_1 AS eTKD_YykSyu02 ON eTKD_YykSyu02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YykSyu02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_Haisha_1 AS eTKD_Haisha03 ON eTKD_Haisha03.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_Haisha03.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum04 ON eTKD_FutTum04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum04.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum04.FutGuiKbn = 5
        LEFT JOIN eTKD_FutTum_3 AS eTKD_FutTum05 ON eTKD_FutTum05.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum05.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum05.FutGuiKbn = 0
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum06 ON eTKD_FutTum06.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum06.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum06.FutGuiKbn = 3
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum07 ON eTKD_FutTum07.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum07.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum07.FutGuiKbn = 0
                AND eTKD_FutTum07.FutHotelCd <> ''
                AND eTKD_FutTum07.FutParkingCd = ''
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum08 ON eTKD_FutTum08.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum08.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum08.FutGuiKbn = 0
                AND eTKD_FutTum08.FutHotelCd = ''
                AND eTKD_FutTum08.FutParkingCd <> ''
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum09 ON eTKD_FutTum09.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum09.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum09.FutGuiKbn = 0
                AND eTKD_FutTum09.FutHotelCd = ''
                AND eTKD_FutTum09.FutParkingCd = ''
        LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        -- IF @TesuInKbn = 2:  Add start
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt09 ON eVPM_TokiSt09.TokuiSeq = eTKD_Unkobi01.TokuiSeq
                AND eVPM_TokiSt09.SitenCdSeq = eTKD_Unkobi01.SitenCdSeq
                AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt09.SiyoStaYmd AND eVPM_TokiSt09.SiyoEndYmd
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt16 ON eVPM_TokiSt16.TokuiSeq = eVPM_TokiSt09.SeiCdSeq
                AND eVPM_TokiSt16.SitenCdSeq = eVPM_TokiSt09.SeiSitenCdSeq
                AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt16.SiyoStaYmd AND eVPM_TokiSt16.SiyoEndYmd
        -- IF @TesuInKbn = 2:  Add end
        WHERE	(@CompanyCd IS NULL OR eVPM_Compny17.CompanyCd = @CompanyCd)
        AND (@EigyoCdSeq IS NULL OR eTKD_Unkobi01.EigyoCd = @EigyoCdSeq)
        AND (@UriYmdFrom IS NULL OR eTKD_Unkobi01.UriYmd >= @UriYmdFrom)
        AND (@UriYmdTo IS NULL OR eTKD_Unkobi01.UriYmd <= @UriYmdTo)
        AND (@UkeNoFrom IS NULL OR eTKD_Unkobi01.UkeNo >= @UkeNoFrom)
        AND (@UkeNoTo IS NULL OR eTKD_Unkobi01.UkeNo <= @UkeNoTo)
        AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND (@TenantCdSeq IS NULL OR eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
        GROUP BY eTKD_Unkobi01.UriYmd
                ,eTKD_Unkobi01.EigCdSeq
                ,eTKD_Unkobi01.EigyoCd
                ,eTKD_Unkobi01.EigyoNm
                ,eTKD_Unkobi01.RyakuNm
        -- ********************************************************************************************************************************
        -- 明細区分：１（頁計）
        -- ********************************************************************************************************************************

        UNION ALL

        SELECT 1 AS MesaiKbn -- 明細区分
                ,' ' AS UriYmd -- 売上年月日
                ,0 AS CanUnc -- キャンセル料・運賃
                ,0 AS CanSyoG -- キャンセル料・消費税額
                ,0 AS CanSum -- キャンセル料・合計
                ,0 AS EigCdSeq -- 営業所コードＳＥＱ
                ,0 AS EigyoCd -- 営業所コード
                ,' ' AS EigyoNm -- 営業所名
                ,' ' AS EigyoRyak -- 営業所略名
                ,0 AS JisSyaSyuDai -- 自社項目・台数
                ,0 AS KeiKin -- 契約運賃
                ,0 AS JisSyaRyoUnc -- 自社項目・運賃
                ,0 AS JisSyaRyoSyo -- 自社項目・消費税額
                ,0 AS JisSyaRyoTes -- 自社項目・手数料額
                ,0 AS JisSyaRyoSum -- 自社項目・合計
                ,0 AS GaiUriGakKin -- ガイド料・売上額
                ,0 AS GaiSyaRyoSyo -- ガイド料・消費税額
                ,0 AS GaiSyaRyoTes -- ガイド料・手数料額
                ,0 AS GaiSyaRyoSum -- 自社ガイド料・合計
                ,0 AS EtcUriGakKin -- その他付帯・売上額
                ,0 AS EtcSyaRyoSyo -- その他付帯・消費税額
                ,0 AS EtcSyaRyoTes -- その他付帯・手数料額
                ,0 AS EtcSyaRyoSum -- 自社その他付帯・合計
                ,0 AS HighwayUriGakKin --       通行料付帯・売上額
                ,0 AS HighwaySyaRyoSyo --       通行料付帯・消費税額
                ,0 AS HighwaySyaRyoTes --       通行料付帯・手数料額
                ,0 AS HighwaySyaRyoSum --       自社通行料付帯・合計
                ,0 AS HotelUriGakKin -- 宿泊料付帯・売上額
                ,0 AS HotelSyaRyoSyo -- 宿泊料付帯・消費税額
                ,0 AS HotelSyaRyoTes -- 宿泊料付帯・手数料額
                ,0 AS HotelSyaRyoSum -- 自社宿泊料付帯・合計
                ,0 AS ParkingUriGakKin --       駐車料付帯・売上額
                ,0 AS ParkingSyaRyoSyo --       駐車料付帯・消費税額
                ,0 AS ParkingSyaRyoTes --       駐車料付帯・手数料額
                ,0 AS ParkingSyaRyoSum --       自社駐車料付帯・合計
                ,0 AS OtherUriGakKin -- その他付帯・売上額
                ,0 AS OtherSyaRyoSyo -- その他付帯・消費税額
                ,0 AS OtherSyaRyoTes -- その他付帯・手数料額
                ,0 AS OtherSyaRyoSum -- 自社その他付帯・合計
                ,SUM(ISNULL(eTKD_Haisha02.SyaRyoUnc, 0)) AS S_JyuSyaRyoUnc -- 日計/累計・受注運賃
                ,SUM(ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) AS S_JyuSyaRyoSyo -- 日計/累計・受注消費税
                ,SUM(ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) AS S_JyuSyaRyoTes -- 日計/累計・受注手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - (
                                                        CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS S_JyuSyaRyoRui -- 日計/累計・受注累計
                ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0)) AS S_JisSyaRyoUnc -- 日計/累計・自社運賃
                ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) AS S_JisSyaRyoSyo -- 日計/累計・自社消費税
                ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) AS S_JisSyaRyoTes -- 日計/累計・自社手数料額
                ,SUM(ISNULL(eTKD_FutTum03.UriGakKin, 0)) AS S_FutUriGakKin -- 日計/累計・自社付帯売上
                ,SUM(ISNULL(eTKD_FutTum03.SyaRyoSyo, 0)) AS S_FutSyaRyoSyo -- 日計/累計・自社付帯消費税
                ,SUM(ISNULL(eTKD_FutTum03.SyaRyoTes, 0)) AS S_FutSyaRyoTes -- 日計/累計・自社付帯手数料額
                ,SUM(ISNULL(eTKD_Haisha02.YoushaUnc, 0)) AS S_YoushaUnc -- 日計/累計・傭車運賃
                ,SUM(ISNULL(eTKD_Haisha02.YoushaSyo, 0)) AS S_YoushaSyo -- 日計/累計・傭車消費税
                ,SUM(ISNULL(eTKD_Haisha02.YoushaTes, 0)) AS S_YoushaTes -- 日計/累計・傭車手数料額
                ,SUM(ISNULL(eTKD_YFutTu04.HaseiKin, 0)) AS S_YfuUriGakKin -- 日計/累計・傭車付帯売上
                ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoSyo, 0)) AS S_YfuSyaRyoSyo -- 日計/累計・傭車付帯消費税
                ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoTes, 0)) AS S_YfuSyaRyoTes -- 日計/累計・傭車付帯手数料額
                ,0 AS JyuSyaRyoUnc -- 受注運賃
                ,0 AS JyuSyaRyoSyo -- 受注消費税
                ,0 AS JyuSyaRyoTes -- 受注手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0))
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0) - ISNULL(eTKD_Haisha02.YoushaTes, 0))
                                                -- IF @TesuInKbn = 2:  Add below WHEN
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                        THEN (
                                                        ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        ) - (
                                                        ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)
                                                                ELSE 0
                                                                END
                                                        ) - (
                                                        ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0) - CASE 
                                                                WHEN eVPM_TokiSt16.TesKbn = 2
                                                                        THEN ISNULL(eTKD_Haisha02.YoushaTes, 0)
                                                                ELSE 0
                                                                END
                                                        )
                                END) AS S_Soneki
        FROM eTKD_Unkobi_1 AS eTKD_Unkobi01
        LEFT JOIN eTKD_Haisha_1 AS eTKD_Haisha02 ON eTKD_Haisha02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_Haisha02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_2 AS eTKD_FutTum03 ON eTKD_FutTum03.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum03.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_YFutTu_1 AS eTKD_YFutTu04 ON eTKD_YFutTu04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YFutTu04.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        -- IF @TesuInKbn = 2:  Add start
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt09 ON eVPM_TokiSt09.TokuiSeq = eTKD_Unkobi01.TokuiSeq
                AND eVPM_TokiSt09.SitenCdSeq = eTKD_Unkobi01.SitenCdSeq
                AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt09.SiyoStaYmd AND eVPM_TokiSt09.SiyoEndYmd
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt16 ON eVPM_TokiSt16.TokuiSeq = eVPM_TokiSt09.SeiCdSeq
                AND eVPM_TokiSt16.SitenCdSeq = eVPM_TokiSt09.SeiSitenCdSeq
                AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt16.SiyoStaYmd AND eVPM_TokiSt16.SiyoEndYmd
        -- IF @TesuInKbn = 2:  Add end
        WHERE       (@CompanyCd IS NULL OR eVPM_Compny17.CompanyCd = @CompanyCd)
                AND (@EigyoCdSeq IS NULL OR eTKD_Unkobi01.EigyoCd = @EigyoCdSeq)
                AND (@UriYmdFrom IS NULL OR eTKD_Unkobi01.UriYmd >= @UriYmdFrom)
                AND (@UriYmdTo IS NULL OR  eTKD_Unkobi01.UriYmd <= @UriYmdTo)
                AND (@UkeNoFrom IS NULL OR eTKD_Unkobi01.UkeNo >= @UkeNoFrom)
                AND (@UkeNoTo IS NULL OR   eTKD_Unkobi01.UkeNo <= @UkeNoTo)
                AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		        AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
                AND (@TenantCdSeq IS NULL OR   eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
        -- ********************************************************************************************************************************
        -- 明細区分：２（累計）
        -- ********************************************************************************************************************************

        UNION ALL

        SELECT 2 AS MesaiKbn -- 明細区分
        ,' ' AS UriYmd -- 売上年月日
        ,SUM(eTKD_Unkobi01.CanUnc) AS CanUnc -- キャンセル料・運賃
        ,SUM(eTKD_Unkobi01.CanSyoG) AS CanSyoG -- キャンセル料・消費税額
        ,SUM(eTKD_Unkobi01.CanUnc + eTKD_Unkobi01.CanSyoG) AS CanSum -- キャンセル料・合計
        ,0 AS EigCdSeq -- 営業所コードＳＥＱ
        ,0 AS EigyoCd -- 営業所コード
        ,' ' AS EigyoNm -- 営業所名
        ,' ' AS EigyoRyak -- 営業所略名
        ,SUM(ISNULL(eTKD_YykSyu02.SyaSyuDai, 0)) AS JisSyaSyuDai -- 自社項目・台数
        ,0 AS KeiKin -- 契約運賃
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0)) AS JisSyaRyoUnc -- 自社項目・運賃
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) AS JisSyaRyoSyo -- 自社項目・消費税額
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) AS JisSyaRyoTes -- 自社項目・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt16.TesKbn = 2
                                                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS JisSyaRyoSum -- 自社項目・合計
        ,SUM(ISNULL(eTKD_FutTum04.UriGakKin, 0)) AS GaiUriGakKin -- 自社ガイド料・売上額
        ,SUM(ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)) AS GaiSyaRyoSyo -- 自社ガイド料・消費税額
        ,SUM(ISNULL(eTKD_FutTum04.SyaRyoTes, 0)) AS GaiSyaRyoTes -- 自社ガイド料・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum04.SyaRyoTes, 0)
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt16.TesKbnGui = 2
                                                                THEN ISNULL(eTKD_FutTum04.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS GaiSyaRyoSum -- 自社ガイド料・合計
        ,SUM(ISNULL(eTKD_FutTum05.UriGakKin, 0)) AS EtcUriGakKin -- 自社その他付帯・売上額
        ,SUM(ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)) AS EtcSyaRyoSyo -- 自社その他付帯・消費税額
        ,SUM(ISNULL(eTKD_FutTum05.SyaRyoTes, 0)) AS EtcSyaRyoTes -- 自社その他付帯・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum05.SyaRyoTes, 0)
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt16.TesKbnFut = 2
                                                                THEN ISNULL(eTKD_FutTum05.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS EtcSyaRyoSum -- 自社その他付帯・合計
        ,0 AS HighwayUriGakKin --       通行料付帯・売上額
        ,0 AS HighwaySyaRyoSyo --       通行料付帯・消費税額
        ,0 AS HighwaySyaRyoTes --       通行料付帯・手数料額
        ,0 AS HighwaySyaRyoSum --       自社通行料付帯・合計
        ,0 AS HotelUriGakKin -- 宿泊料付帯・売上額
        ,0 AS HotelSyaRyoSyo -- 宿泊料付帯・消費税額
        ,0 AS HotelSyaRyoTes -- 宿泊料付帯・手数料額
        ,0 AS HotelSyaRyoSum -- 自社宿泊料付帯・合計
        ,0 AS ParkingUriGakKin --       駐車料付帯・売上額
        ,0 AS ParkingSyaRyoSyo --       駐車料付帯・消費税額
        ,0 AS ParkingSyaRyoTes --       駐車料付帯・手数料額
        ,0 AS ParkingSyaRyoSum --       自社駐車料付帯・合計
        ,0 AS OtherUriGakKin -- その他付帯・売上額
        ,0 AS OtherSyaRyoSyo -- その他付帯・消費税額
        ,0 AS OtherSyaRyoTes -- その他付帯・手数料額
        ,0 AS OtherSyaRyoSum -- 自社その他付帯・合計
        ,SUM(ISNULL(eTKD_Haisha02.SyaRyoUnc, 0)) AS S_JyuSyaRyoUnc -- 日計/累計・受注運賃
        ,SUM(ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) AS S_JyuSyaRyoSyo -- 日計/累計・受注消費税
        ,SUM(ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) AS S_JyuSyaRyoTes -- 日計/累計・受注手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - (
                                                CASE 
                                                        WHEN eVPM_TokiSt16.TesKbn = 2
                                                                THEN ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_JyuSyaRyoRui -- 日計/累計・受注累計
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0)) AS S_JisSyaRyoUnc -- 日計/累計・自社運賃
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) AS S_JisSyaRyoSyo -- 日計/累計・自社消費税
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) AS S_JisSyaRyoTes -- 日計/累計・自社手数料額
        ,SUM(ISNULL(eTKD_FutTum03.UriGakKin, 0)) AS S_FutUriGakKin -- 日計/累計・自社付帯売上
        ,SUM(ISNULL(eTKD_FutTum03.SyaRyoSyo, 0)) AS S_FutSyaRyoSyo -- 日計/累計・自社付帯消費税
        ,SUM(ISNULL(eTKD_FutTum03.SyaRyoTes, 0)) AS S_FutSyaRyoTes -- 日計/累計・自社付帯手数料額
        ,SUM(ISNULL(eTKD_Haisha02.YoushaUnc, 0)) AS S_YoushaUnc -- 日計/累計・傭車運賃
        ,SUM(ISNULL(eTKD_Haisha02.YoushaSyo, 0)) AS S_YoushaSyo -- 日計/累計・傭車消費税
        ,SUM(ISNULL(eTKD_Haisha02.YoushaTes, 0)) AS S_YoushaTes -- 日計/累計・傭車手数料額
        ,SUM(ISNULL(eTKD_YFutTu04.HaseiKin, 0)) AS S_YfuUriGakKin -- 日計/累計・傭車付帯売上
        ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoSyo, 0)) AS S_YfuSyaRyoSyo -- 日計/累計・傭車付帯消費税
        ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoTes, 0)) AS S_YfuSyaRyoTes -- 日計/累計・傭車付帯手数料額
        ,0 AS JyuSyaRyoUnc -- 受注運賃
        ,0 AS JyuSyaRyoSyo -- 受注消費税
        ,0 AS JyuSyaRyoTes -- 受注手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0))
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0) - ISNULL(eTKD_Haisha02.YoushaTes, 0))
                                        -- IF @TesuInKbn = 2:  Add below WHEN
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '2'
                                THEN (
                                                ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - CASE 
                                                        WHEN eVPM_TokiSt16.TesKbn = 2
                                                                THEN ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                ) - (
                                                ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - CASE 
                                                        WHEN eVPM_TokiSt16.TesKbn = 2
                                                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)
                                                        ELSE 0
                                                        END
                                                ) - (
                                                ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0) - CASE 
                                                        WHEN eVPM_TokiSt16.TesKbn = 2
                                                                THEN ISNULL(eTKD_Haisha02.YoushaTes, 0)
                                                        ELSE 0
                                                        END
                                                )
                        END) AS S_Soneki
        FROM eTKD_Unkobi_1 AS eTKD_Unkobi01
        LEFT JOIN eTKD_Haisha_1 AS eTKD_Haisha02 ON eTKD_Haisha02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_Haisha02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_2 AS eTKD_FutTum03 ON eTKD_FutTum03.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum03.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_YFutTu_1 AS eTKD_YFutTu04 ON eTKD_YFutTu04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YFutTu04.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_YykSyu_1 AS eTKD_YykSyu02 ON eTKD_YykSyu02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YykSyu02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum04 ON eTKD_FutTum04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum04.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum04.FutGuiKbn = 5
        LEFT JOIN eTKD_FutTum_3 AS eTKD_FutTum05 ON eTKD_FutTum05.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum05.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum05.FutGuiKbn = 0
        LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        -- IF @TesuInKbn = 2:  Add start
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt09 ON eVPM_TokiSt09.TokuiSeq = eTKD_Unkobi01.TokuiSeq
                AND eVPM_TokiSt09.SitenCdSeq = eTKD_Unkobi01.SitenCdSeq
                AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt09.SiyoStaYmd AND eVPM_TokiSt09.SiyoEndYmd
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt16 ON eVPM_TokiSt16.TokuiSeq = eVPM_TokiSt09.SeiCdSeq
                AND eVPM_TokiSt16.SitenCdSeq = eVPM_TokiSt09.SeiSitenCdSeq
                AND eTKD_Unkobi01.UriYmd BETWEEN eVPM_TokiSt16.SiyoStaYmd AND eVPM_TokiSt16.SiyoEndYmd
        -- IF @TesuInKbn = 2:  Add end
        WHERE (@CompanyCd IS NULL OR	eVPM_Compny17.CompanyCd = @CompanyCd)
        AND  (@EigyoCdSeq  IS NULL OR  eTKD_Unkobi01.EigyoCd = @EigyoCdSeq)
        AND (@UriYmdFrom IS NULL OR    eTKD_Unkobi01.UriYmd >= @UriYmdFrom)
        AND (@UriYmdTo IS NULL OR eTKD_Unkobi01.UriYmd <= @UriYmdTo)
        AND (@UkeNoFrom IS NULL OR eTKD_Unkobi01.UkeNo >= @UkeNoFrom)
        AND (@UkeNoTo IS NULL OR eTKD_Unkobi01.UkeNo <= @UkeNoTo)
        AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND (@TenantCdSeq IS NULL OR eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
        ORDER BY EigyoCd
                ,UriYmd
	END
ELSE
    BEGIN
        ;WITH eTKD_Unkobi_1
        AS (
                SELECT eTKD_Yyksho01.UkeNo AS UkeNo -- 受付番号
                        ,ISNULL(eTKD_Unkobi02.UnkRen, 0) AS UnkRen -- 運行日連番
                        ,eTKD_Yyksho01.YoyaSyu AS YoyaSyu -- 予約種別
                        ,eVPM_YoyKbn15.YoyaKbn AS YoyaKbn -- 予約区分
                        ,CASE 
                                WHEN eTKD_Yyksho01.YoyaSyu = 1
                                        THEN ISNULL(eTKD_Unkobi02.UriYmd, ' ')
                                WHEN eTKD_Yyksho01.YoyaSyu = 2
                                        THEN eTKD_Yyksho01.CanYmd
                                END AS UriYmd -- 売上年月日（キャンセル年月日）
                        ,CASE WHEN @EigyoKbn = 1 THEN eTKD_Yyksho01.SeiEigCdSeq -- 請求営業所コードＳＥＱ
					            ELSE eTKD_Yyksho01.UkeEigCdSeq END AS EigCdSeq  -- 受付営業所コードＳＥＱ -- @EigyoKbn = 2
                        ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS EigyoCd -- 請求営業所コード / 受付営業所コード
                        ,ISNULL(eVPM_Eigyos06.EigyoNm, ' ') AS EigyoNm -- 請求営業所名 / 受付営業所名
                        ,ISNULL(eVPM_Eigyos06.RyakuNm, ' ') AS RyakuNm -- 請求営業所略名 / 受付営業所略名
                        ,ISNULL(eVPM_Eigyos06.CompanyCdSeq, ' ') AS CompanyCdSeq -- 請求会社コードＳＥＱ / 受付会社コードＳＥＱ
                        ,eTKD_Yyksho01.SirCdSeq AS SirCdSeq -- 仕入先コードＳＥＱ
                        ,eTKD_Yyksho01.SirSitenCdSeq AS SirSitenCdSeq -- 仕入先支店コードＳＥＱ
                        ,CASE 
                                WHEN eTKD_Yyksho01.YoyaSyu = 1
                                        THEN 0
                                WHEN eTKD_Yyksho01.YoyaSyu = 2
                                        THEN eTKD_Yyksho01.CanUnc
                                END AS CanUnc -- キャンセル料・運賃
                        ,CASE 
                                WHEN eTKD_Yyksho01.YoyaSyu = 1
                                        THEN 0
                                WHEN eTKD_Yyksho01.YoyaSyu = 2
                                        THEN eTKD_Yyksho01.CanSyoG
                                END AS CanSyoG -- キャンセル料・消費税額
                        ,eTKD_Yyksho01.SiyoKbn AS Y_SiyoKbn -- 予約書・使用区分
                        ,ISNULL(eTKD_Unkobi02.SiyoKbn, 1) AS U_SiyoKbn -- 運行日・使用区分
                FROM TKD_Yyksho AS eTKD_Yyksho01
                LEFT JOIN TKD_Unkobi AS eTKD_Unkobi02 ON eTKD_Unkobi02.UkeNo = eTKD_Yyksho01.UkeNo
                        AND eTKD_Yyksho01.YoyaSyu = 1
                JOIN VPM_YoyKbn AS eVPM_YoyKbn15 ON eVPM_YoyKbn15.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
                AND eVPM_YoyKbn15.TenantCdSeq = @TenantCdSeq
                INNER JOIN VPM_Tokisk AS eVPM_Tokisk06 ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk06.TokuiSeq
                        AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk06.SiyoStaYmd AND eVPM_Tokisk06.SiyoEndYmd
                INNER JOIN VPM_TokiSt AS eVPM_TokiSt07 ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt07.TokuiSeq
                        AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt07.SitenCdSeq
                        AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt07.SiyoStaYmd AND eVPM_TokiSt07.SiyoEndYmd
                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eTKD_Yyksho01.TenantCdSeq
                        AND eVPM_Tenant.SiyoKbn = 1
                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON eVPM_Eigyos06.EigyoCdSeq = CASE WHEN @EigyoKbn = 1 THEN eTKD_Yyksho01.SeiEigCdSeq
																				ELSE eTKD_Yyksho01.UkeEigCdSeq END -- @EigyoKbn = 2
                WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
                )
                ,eTKD_YykSyu_1
        AS (
                SELECT eTKD_Haisha05.UkeNo AS UkeNo
                        ,eTKD_Haisha05.UnkRen AS UnkRen
                        ,count(*) AS SyaSyuDai --       台数
                FROM TKD_Haisha AS eTKD_Haisha05
                WHERE eTKD_Haisha05.SiyoKbn = 1
                        AND eTKD_Haisha05.YouTblSeq = 0
                        AND SUBSTRING(eTKD_Haisha05.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_Haisha05.UkeNo
                        ,eTKD_Haisha05.UnkRen
                )
                ,eTKD_Haisha_1
        AS (
                SELECT eTKD_Haisha01.UkeNo AS UkeNo
                        ,eTKD_Haisha01.UnkRen AS UnkRen
                        ,CONVERT(bigint, SUM(eTKD_Haisha01.SyaRyoUnc)) AS SyaRyoUnc -- 受注運賃
                        ,CONVERT(bigint, SUM(eTKD_Haisha01.SyaRyoSyo)) AS SyaRyoSyo -- 受注消費税額
                        ,CONVERT(bigint, SUM(eTKD_Haisha01.SyaRyoTes)) AS SyaRyoTes -- 受注手数料額
                        ,CONVERT(bigint, SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN eTKD_Haisha01.SyaRyoUnc
                                        ELSE 0
                                        END)) AS JisSyaRyoUnc -- 自社運賃
                        ,CONVERT(bigint, SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN eTKD_Haisha01.SyaRyoSyo
                                        ELSE 0
                                        END)) AS JisSyaRyoSyo -- 自社消費税額
                        ,CONVERT(bigint, SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN eTKD_Haisha01.SyaRyoTes
                                        ELSE 0
                                        END)) AS JisSyaRyoTes -- 自社手数料額
                        ,SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN 0
                                        ELSE eTKD_Haisha01.YoushaUnc
                                        END) AS YoushaUnc -- 傭車運賃
                        ,SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN 0
                                        ELSE eTKD_Haisha01.YoushaSyo
                                        END) AS YoushaSyo -- 傭車消費税
                        ,SUM(CASE 
                                        WHEN eTKD_Haisha01.YouTblSeq = 0
                                                THEN 0
                                        ELSE eTKD_Haisha01.YoushaTes
                                        END) AS YoushaTes -- 傭車手数料額
                FROM TKD_Haisha AS eTKD_Haisha01
                WHERE eTKD_Haisha01.SiyoKbn = 1
                AND SUBSTRING(eTKD_Haisha01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_Haisha01.UkeNo
                        ,eTKD_Haisha01.UnkRen
                )
                ,eTKD_FutTum_1
        AS (
                SELECT eTKD_FutTum01.UkeNo AS UkeNo
                        ,eTKD_FutTum01.UnkRen AS UnkRen
                        ,eTKD_FutTum01.FutGuiKbn AS FutGuiKbn
                        ,eTKD_FutTum01.FutHotelCd AS FutHotelCd
                        ,eTKD_FutTum01.FutParkingCd AS FutParkingCd
                        ,SUM(eTKD_FutTum01.UriGakKin) AS UriGakKin
                        ,SUM(eTKD_FutTum01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_FutTum01.SyaRyoTes) AS SyaRyoTes
                FROM (
                        SELECT eTKD_FutTum01.UkeNo AS UkeNo
                                ,eTKD_FutTum01.UnkRen AS UnkRen
                                ,CASE 
                                        WHEN eVPM_Futai02.FutGuiKbn IN (3, 5)
                                                THEN eVPM_Futai02.FutGuiKbn
                                        ELSE 0
                                        END AS FutGuiKbn
                                ,ISNULL(eVPM_CodeKb01.CodeKbn, '') AS FutHotelCd
                                ,ISNULL(eVPM_CodeKb02.CodeKbn, '') AS FutParkingCd
                                ,eTKD_FutTum01.UriGakKin AS UriGakKin
                                ,eTKD_FutTum01.SyaRyoSyo AS SyaRyoSyo
                                ,eTKD_FutTum01.SyaRyoTes AS SyaRyoTes
                        FROM TKD_FutTum AS eTKD_FutTum01
                        LEFT JOIN VPM_Futai AS eVPM_Futai02 ON eVPM_Futai02.FutaiCdSeq = eTKD_FutTum01.FutTumCdSeq
                        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeKbn = eVPM_Futai02.FutaiCd
                                AND eVPM_CodeKb01.CodeSyu = 'HOTELFUT'
                                AND eVPM_CodeKb01.TenantCdSeq = (
                                        SELECT CASE 
                                                        WHEN COUNT(*) = 0
                                                                THEN 0
                                                        ELSE @TenantCdSeq -- Login Tenant
                                                        END AS TenantCdSeq
                                        FROM VPM_CodeKb
                                        WHERE VPM_CodeKb.CodeSyu = 'HOTELFUT'
                                                AND VPM_CodeKb.SiyoKbn = 1
                                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant
                                        )
                        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_CodeKb02.CodeKbn = eVPM_Futai02.FutaiCd
                                AND eVPM_CodeKb02.CodeSyu = 'PARKINGFUT'
                                AND eVPM_CodeKb02.TenantCdSeq = (
                                        SELECT CASE 
                                                        WHEN COUNT(*) = 0
                                                                THEN 0
                                                        ELSE @TenantCdSeq -- Login Tenant
                                                        END AS TenantCdSeq
                                        FROM VPM_CodeKb
                                        WHERE VPM_CodeKb.CodeSyu = 'PARKINGFUT'
                                                AND VPM_CodeKb.SiyoKbn = 1
                                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq -- Login Tenant
                                        )
                        WHERE eTKD_FutTum01.SeisanKbn = 1
                                AND eTKD_FutTum01.SiyoKbn = 1
                                AND SUBSTRING(eTKD_FutTum01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                        ) AS eTKD_FutTum01
                GROUP BY eTKD_FutTum01.UkeNo
                        ,eTKD_FutTum01.UnkRen
                        ,eTKD_FutTum01.FutGuiKbn
                        ,eTKD_FutTum01.FutHotelCd
                        ,eTKD_FutTum01.FutParkingCd
                )
                ,eTKD_FutTum_2
        AS (
                SELECT eTKD_FutTum01.UkeNo AS UkeNo
                        ,eTKD_FutTum01.UnkRen AS UnkRen
                        ,SUM(eTKD_FutTum01.UriGakKin) AS UriGakKin
                        ,SUM(eTKD_FutTum01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_FutTum01.SyaRyoTes) AS SyaRyoTes
                FROM eTKD_FutTum_1 AS eTKD_FutTum01
                WHERE SUBSTRING(eTKD_FutTum01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_FutTum01.UkeNo
                        ,eTKD_FutTum01.UnkRen
                )
                ,eTKD_FutTum_3
        AS (
                SELECT eTKD_FutTum01.UkeNo AS UkeNo
                        ,eTKD_FutTum01.UnkRen AS UnkRen
                        ,CASE 
                                WHEN eTKD_FutTum01.FutGuiKbn = 5
                                        THEN eTKD_FutTum01.FutGuiKbn
                                ELSE 0
                                END AS FutGuiKbn
                        ,SUM(eTKD_FutTum01.UriGakKin) AS UriGakKin
                        ,SUM(eTKD_FutTum01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_FutTum01.SyaRyoTes) AS SyaRyoTes
                FROM eTKD_FutTum_1 AS eTKD_FutTum01
                WHERE SUBSTRING(eTKD_FutTum01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_FutTum01.UkeNo
                        ,eTKD_FutTum01.UnkRen
                        ,CASE 
                                WHEN eTKD_FutTum01.FutGuiKbn = 5
                                        THEN eTKD_FutTum01.FutGuiKbn
                                ELSE 0
                                END
                )
                ,eTKD_YFutTu_1
        AS (
                SELECT eTKD_YFutTu01.UkeNo AS UkeNo
                        ,eTKD_YFutTu01.UnkRen AS UnkRen
                        ,SUM(eTKD_YFutTu01.HaseiKin) AS HaseiKin
                        ,SUM(eTKD_YFutTu01.SyaRyoSyo) AS SyaRyoSyo
                        ,SUM(eTKD_YFutTu01.SyaRyoTes) AS SyaRyoTes
                FROM TKD_YFutTu AS eTKD_YFutTu01
                WHERE eTKD_YFutTu01.SiyoKbn = 1
                AND SUBSTRING(eTKD_YFutTu01.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                GROUP BY eTKD_YFutTu01.UkeNo
                        ,eTKD_YFutTu01.UnkRen
                )
        -- ********************************************************************************************************************************
        -- 明細区分：３（明細）
        -- ********************************************************************************************************************************
        SELECT 3 AS MesaiKbn -- 明細区分
                ,eTKD_Unkobi01.UriYmd AS UriYmd -- 売上年月日
                ,SUM(eTKD_Unkobi01.CanUnc) AS CanUnc -- キャンセル料・運賃
                ,SUM(eTKD_Unkobi01.CanSyoG) AS CanSyoG -- キャンセル料・消費税額
                ,SUM(eTKD_Unkobi01.CanUnc + eTKD_Unkobi01.CanSyoG) AS CanSum -- キャンセル料・合計
                ,eTKD_Unkobi01.EigCdSeq AS EigCdSeq -- 営業所コードＳＥＱ
                ,eTKD_Unkobi01.EigyoCd AS EigyoCd -- 営業所コード
                ,eTKD_Unkobi01.EigyoNm AS EigyoNm -- 営業所名
                ,eTKD_Unkobi01.RyakuNm AS EigyoRyak -- 営業所略名
                ,SUM(ISNULL(eTKD_YykSyu02.SyaSyuDai, 0)) AS JisSyaSyuDai -- 自社項目・台数
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha03.SyaRyoTes, 0)
                                END) AS KeiKin -- 契約運賃
                ,SUM(ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0)) AS JisSyaRyoUnc -- 自社項目・運賃
                ,SUM(ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0)) AS JisSyaRyoSyo -- 自社項目・消費税額
                ,SUM(ISNULL(eTKD_Haisha03.JisSyaRyoTes, 0)) AS JisSyaRyoTes -- 自社項目・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha03.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha03.JisSyaRyoTes, 0)
                                END) AS JisSyaRyoSum -- 自社項目・合計
                ,SUM(ISNULL(eTKD_FutTum04.UriGakKin, 0)) AS GaiUriGakKin -- 自社ガイド料・売上額
                ,SUM(ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)) AS GaiSyaRyoSyo -- 自社ガイド料・消費税額
                ,SUM(ISNULL(eTKD_FutTum04.SyaRyoTes, 0)) AS GaiSyaRyoTes -- 自社ガイド料・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum04.SyaRyoTes, 0)
                                END) AS GaiSyaRyoSum -- 自社ガイド料・合計
                ,SUM(ISNULL(eTKD_FutTum05.UriGakKin, 0)) AS EtcUriGakKin -- 自社その他付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)) AS EtcSyaRyoSyo -- 自社その他付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum05.SyaRyoTes, 0)) AS EtcSyaRyoTes -- 自社その他付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum05.SyaRyoTes, 0)
                                END) AS EtcSyaRyoSum -- 自社その他付帯・合計
                ,SUM(ISNULL(eTKD_FutTum06.UriGakKin, 0)) AS HighwayUriGakKin -- 自社通行料付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum06.SyaRyoSyo, 0)) AS HighwaySyaRyoSyo -- 自社通行料付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum06.SyaRyoTes, 0)) AS HighwaySyaRyoTes -- 自社通行料付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum06.UriGakKin, 0) + ISNULL(eTKD_FutTum06.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum06.UriGakKin, 0) + ISNULL(eTKD_FutTum06.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum06.SyaRyoTes, 0)
                                END) AS HighwaySyaRyoSum -- 自社通行料付帯・合計
                ,SUM(ISNULL(eTKD_FutTum07.UriGakKin, 0)) AS HotelUriGakKin -- 自社宿泊料付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum07.SyaRyoSyo, 0)) AS HotelSyaRyoSyo -- 自社宿泊料付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum07.SyaRyoTes, 0)) AS HotelSyaRyoTes -- 自社宿泊料付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum07.UriGakKin, 0) + ISNULL(eTKD_FutTum07.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum07.UriGakKin, 0) + ISNULL(eTKD_FutTum07.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum07.SyaRyoTes, 0)
                                END) AS HotelSyaRyoSum -- 自社宿泊料付帯・合計
                ,SUM(ISNULL(eTKD_FutTum08.UriGakKin, 0)) AS ParkingUriGakKin -- 自社駐車料付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum08.SyaRyoSyo, 0)) AS ParkingSyaRyoSyo -- 自社駐車料付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum08.SyaRyoTes, 0)) AS ParkingSyaRyoTes -- 自社駐車料付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum08.UriGakKin, 0) + ISNULL(eTKD_FutTum08.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum08.UriGakKin, 0) + ISNULL(eTKD_FutTum08.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum08.SyaRyoTes, 0)
                                END) AS ParkingSyaRyoSum -- 自社駐車料付帯・合計
                ,SUM(ISNULL(eTKD_FutTum09.UriGakKin, 0)) AS OtherUriGakKin -- 自社その他付帯・売上額
                ,SUM(ISNULL(eTKD_FutTum09.SyaRyoSyo, 0)) AS OtherSyaRyoSyo -- 自社その他付帯・消費税額
                ,SUM(ISNULL(eTKD_FutTum09.SyaRyoTes, 0)) AS OtherSyaRyoTes -- 自社その他付帯・手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_FutTum09.UriGakKin, 0) + ISNULL(eTKD_FutTum09.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_FutTum09.UriGakKin, 0) + ISNULL(eTKD_FutTum09.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum09.SyaRyoTes, 0)
                                END) AS OtherSyaRyoSum -- 自社その他付帯・合計
                ,0 AS S_JyuSyaRyoUnc -- 日計/累計・受注運賃
                ,0 AS S_JyuSyaRyoSyo -- 日計/累計・受注消費税
                ,0 AS S_JyuSyaRyoTes -- 日計/累計・受注手数料額
                ,0 AS S_JyuSyaRyoRui -- 日計/累計・受注累計
                ,0 AS S_JisSyaRyoUnc -- 日計/累計・自社運賃
                ,0 AS S_JisSyaRyoSyo -- 日計/累計・自社消費税
                ,0 AS S_JisSyaRyoTes -- 日計/累計・自社手数料額
                ,0 AS S_FutUriGakKin -- 日計/累計・自社付帯売上
                ,0 AS S_FutSyaRyoSyo -- 日計/累計・自社付帯消費税
                ,0 AS S_FutSyaRyoTes -- 日計/累計・自社付帯手数料額
                ,0 AS S_YoushaUnc -- 日計/累計・傭車運賃
                ,0 AS S_YoushaSyo -- 日計/累計・傭車消費税
                ,0 AS S_YoushaTes -- 日計/累計・傭車手数料額
                ,0 AS S_YfuUriGakKin -- 日計/累計・傭車付帯売上
                ,0 AS S_YfuSyaRyoSyo -- 日計/累計・傭車付帯消費税
                ,0 AS S_YfuSyaRyoTes -- 日計/累計・傭車付帯手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha03.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha03.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha03.SyaRyoTes, 0)
                                END) AS JyuSyaRyoRui -- 日計/累計・受注累計
                ,SUM(ISNULL(eTKD_Haisha03.SyaRyoSyo, 0)) AS SyaRyoSyo -- 自社項目・消費税額
                ,SUM(ISNULL(eTKD_Haisha03.SyaRyoTes, 0)) AS SyaRyoTes -- 自社項目・手数料額
                ,0 AS S_Soneki
        FROM eTKD_Unkobi_1 AS eTKD_Unkobi01
        LEFT JOIN eTKD_YykSyu_1 AS eTKD_YykSyu02 ON eTKD_YykSyu02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YykSyu02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_Haisha_1 AS eTKD_Haisha03 ON eTKD_Haisha03.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_Haisha03.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum04 ON eTKD_FutTum04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum04.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum04.FutGuiKbn = 5
        LEFT JOIN eTKD_FutTum_3 AS eTKD_FutTum05 ON eTKD_FutTum05.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum05.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum05.FutGuiKbn = 0
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum06 ON eTKD_FutTum06.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum06.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum06.FutGuiKbn = 3
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum07 ON eTKD_FutTum07.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum07.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum07.FutGuiKbn = 0
                AND eTKD_FutTum07.FutHotelCd <> ''
                AND eTKD_FutTum07.FutParkingCd = ''
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum08 ON eTKD_FutTum08.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum08.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum08.FutGuiKbn = 0
                AND eTKD_FutTum08.FutHotelCd = ''
                AND eTKD_FutTum08.FutParkingCd <> ''
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum09 ON eTKD_FutTum09.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum09.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum09.FutGuiKbn = 0
                AND eTKD_FutTum09.FutHotelCd = ''
                AND eTKD_FutTum09.FutParkingCd = ''
        LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        WHERE	(@CompanyCd IS NULL OR eVPM_Compny17.CompanyCd = @CompanyCd)
        AND (@EigyoCdSeq IS NULL OR eTKD_Unkobi01.EigyoCd = @EigyoCdSeq)
        AND (@UriYmdFrom IS NULL OR eTKD_Unkobi01.UriYmd >= @UriYmdFrom)
        AND (@UriYmdTo IS NULL OR eTKD_Unkobi01.UriYmd <= @UriYmdTo)
        AND (@UkeNoFrom IS NULL OR eTKD_Unkobi01.UkeNo >= @UkeNoFrom)
        AND (@UkeNoTo IS NULL OR eTKD_Unkobi01.UkeNo <= @UkeNoTo)
        AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND (@TenantCdSeq IS NULL OR eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
        GROUP BY eTKD_Unkobi01.UriYmd
                ,eTKD_Unkobi01.EigCdSeq
                ,eTKD_Unkobi01.EigyoCd
                ,eTKD_Unkobi01.EigyoNm
                ,eTKD_Unkobi01.RyakuNm
        -- ********************************************************************************************************************************
        -- 明細区分：１（頁計）
        -- ********************************************************************************************************************************

        UNION ALL

        SELECT 1 AS MesaiKbn -- 明細区分
                ,' ' AS UriYmd -- 売上年月日
                ,0 AS CanUnc -- キャンセル料・運賃
                ,0 AS CanSyoG -- キャンセル料・消費税額
                ,0 AS CanSum -- キャンセル料・合計
                ,0 AS EigCdSeq -- 営業所コードＳＥＱ
                ,0 AS EigyoCd -- 営業所コード
                ,' ' AS EigyoNm -- 営業所名
                ,' ' AS EigyoRyak -- 営業所略名
                ,0 AS JisSyaSyuDai -- 自社項目・台数
                ,0 AS KeiKin -- 契約運賃
                ,0 AS JisSyaRyoUnc -- 自社項目・運賃
                ,0 AS JisSyaRyoSyo -- 自社項目・消費税額
                ,0 AS JisSyaRyoTes -- 自社項目・手数料額
                ,0 AS JisSyaRyoSum -- 自社項目・合計
                ,0 AS GaiUriGakKin -- ガイド料・売上額
                ,0 AS GaiSyaRyoSyo -- ガイド料・消費税額
                ,0 AS GaiSyaRyoTes -- ガイド料・手数料額
                ,0 AS GaiSyaRyoSum -- 自社ガイド料・合計
                ,0 AS EtcUriGakKin -- その他付帯・売上額
                ,0 AS EtcSyaRyoSyo -- その他付帯・消費税額
                ,0 AS EtcSyaRyoTes -- その他付帯・手数料額
                ,0 AS EtcSyaRyoSum -- 自社その他付帯・合計
                ,0 AS HighwayUriGakKin --       通行料付帯・売上額
                ,0 AS HighwaySyaRyoSyo --       通行料付帯・消費税額
                ,0 AS HighwaySyaRyoTes --       通行料付帯・手数料額
                ,0 AS HighwaySyaRyoSum --       自社通行料付帯・合計
                ,0 AS HotelUriGakKin -- 宿泊料付帯・売上額
                ,0 AS HotelSyaRyoSyo -- 宿泊料付帯・消費税額
                ,0 AS HotelSyaRyoTes -- 宿泊料付帯・手数料額
                ,0 AS HotelSyaRyoSum -- 自社宿泊料付帯・合計
                ,0 AS ParkingUriGakKin --       駐車料付帯・売上額
                ,0 AS ParkingSyaRyoSyo --       駐車料付帯・消費税額
                ,0 AS ParkingSyaRyoTes --       駐車料付帯・手数料額
                ,0 AS ParkingSyaRyoSum --       自社駐車料付帯・合計
                ,0 AS OtherUriGakKin -- その他付帯・売上額
                ,0 AS OtherSyaRyoSyo -- その他付帯・消費税額
                ,0 AS OtherSyaRyoTes -- その他付帯・手数料額
                ,0 AS OtherSyaRyoSum -- 自社その他付帯・合計
                ,SUM(ISNULL(eTKD_Haisha02.SyaRyoUnc, 0)) AS S_JyuSyaRyoUnc -- 日計/累計・受注運賃
                ,SUM(ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) AS S_JyuSyaRyoSyo -- 日計/累計・受注消費税
                ,SUM(ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) AS S_JyuSyaRyoTes -- 日計/累計・受注手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                                END) AS S_JyuSyaRyoRui -- 日計/累計・受注累計
                ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0)) AS S_JisSyaRyoUnc -- 日計/累計・自社運賃
                ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) AS S_JisSyaRyoSyo -- 日計/累計・自社消費税
                ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) AS S_JisSyaRyoTes -- 日計/累計・自社手数料額
                ,SUM(ISNULL(eTKD_FutTum03.UriGakKin, 0)) AS S_FutUriGakKin -- 日計/累計・自社付帯売上
                ,SUM(ISNULL(eTKD_FutTum03.SyaRyoSyo, 0)) AS S_FutSyaRyoSyo -- 日計/累計・自社付帯消費税
                ,SUM(ISNULL(eTKD_FutTum03.SyaRyoTes, 0)) AS S_FutSyaRyoTes -- 日計/累計・自社付帯手数料額
                ,SUM(ISNULL(eTKD_Haisha02.YoushaUnc, 0)) AS S_YoushaUnc -- 日計/累計・傭車運賃
                ,SUM(ISNULL(eTKD_Haisha02.YoushaSyo, 0)) AS S_YoushaSyo -- 日計/累計・傭車消費税
                ,SUM(ISNULL(eTKD_Haisha02.YoushaTes, 0)) AS S_YoushaTes -- 日計/累計・傭車手数料額
                ,SUM(ISNULL(eTKD_YFutTu04.HaseiKin, 0)) AS S_YfuUriGakKin -- 日計/累計・傭車付帯売上
                ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoSyo, 0)) AS S_YfuSyaRyoSyo -- 日計/累計・傭車付帯消費税
                ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoTes, 0)) AS S_YfuSyaRyoTes -- 日計/累計・傭車付帯手数料額
                ,0 AS JyuSyaRyoUnc -- 受注運賃
                ,0 AS JyuSyaRyoSyo -- 受注消費税
                ,0 AS JyuSyaRyoTes -- 受注手数料額
                ,SUM(CASE 
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                        THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0))
                                WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                        THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0) - ISNULL(eTKD_Haisha02.YoushaTes, 0))
                                END) AS S_Soneki
        FROM eTKD_Unkobi_1 AS eTKD_Unkobi01
        LEFT JOIN eTKD_Haisha_1 AS eTKD_Haisha02 ON eTKD_Haisha02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_Haisha02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_2 AS eTKD_FutTum03 ON eTKD_FutTum03.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum03.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_YFutTu_1 AS eTKD_YFutTu04 ON eTKD_YFutTu04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YFutTu04.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
          WHERE       (@CompanyCd IS NULL OR eVPM_Compny17.CompanyCd = @CompanyCd)
                AND (@EigyoCdSeq IS NULL OR eTKD_Unkobi01.EigyoCd = @EigyoCdSeq)
                AND (@UriYmdFrom IS NULL OR eTKD_Unkobi01.UriYmd >= @UriYmdFrom)
                AND (@UriYmdTo IS NULL OR  eTKD_Unkobi01.UriYmd <= @UriYmdTo)
                AND (@UkeNoFrom IS NULL OR eTKD_Unkobi01.UkeNo >= @UkeNoFrom)
                AND (@UkeNoTo IS NULL OR   eTKD_Unkobi01.UkeNo <= @UkeNoTo)
                AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		        AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
                AND (@TenantCdSeq IS NULL OR   eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
        -- ********************************************************************************************************************************
        -- 明細区分：２（累計）
        -- ********************************************************************************************************************************

        UNION ALL

        SELECT 2 AS MesaiKbn -- 明細区分
        ,' ' AS UriYmd -- 売上年月日
        ,SUM(eTKD_Unkobi01.CanUnc) AS CanUnc -- キャンセル料・運賃
        ,SUM(eTKD_Unkobi01.CanSyoG) AS CanSyoG -- キャンセル料・消費税額
        ,SUM(eTKD_Unkobi01.CanUnc + eTKD_Unkobi01.CanSyoG) AS CanSum -- キャンセル料・合計
        ,0 AS EigCdSeq -- 営業所コードＳＥＱ
        ,0 AS EigyoCd -- 営業所コード
        ,' ' AS EigyoNm -- 営業所名
        ,' ' AS EigyoRyak -- 営業所略名
        ,SUM(ISNULL(eTKD_YykSyu02.SyaSyuDai, 0)) AS JisSyaSyuDai -- 自社項目・台数
        ,0 AS KeiKin -- 契約運賃
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0)) AS JisSyaRyoUnc -- 自社項目・運賃
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) AS JisSyaRyoSyo -- 自社項目・消費税額
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) AS JisSyaRyoTes -- 自社項目・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)
                        END) AS JisSyaRyoSum -- 自社項目・合計
        ,SUM(ISNULL(eTKD_FutTum04.UriGakKin, 0)) AS GaiUriGakKin -- 自社ガイド料・売上額
        ,SUM(ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)) AS GaiSyaRyoSyo -- 自社ガイド料・消費税額
        ,SUM(ISNULL(eTKD_FutTum04.SyaRyoTes, 0)) AS GaiSyaRyoTes -- 自社ガイド料・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_FutTum04.UriGakKin, 0) + ISNULL(eTKD_FutTum04.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum04.SyaRyoTes, 0)
                        END) AS GaiSyaRyoSum -- 自社ガイド料・合計
        ,SUM(ISNULL(eTKD_FutTum05.UriGakKin, 0)) AS EtcUriGakKin -- 自社その他付帯・売上額
        ,SUM(ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)) AS EtcSyaRyoSyo -- 自社その他付帯・消費税額
        ,SUM(ISNULL(eTKD_FutTum05.SyaRyoTes, 0)) AS EtcSyaRyoTes -- 自社その他付帯・手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_FutTum05.UriGakKin, 0) + ISNULL(eTKD_FutTum05.SyaRyoSyo, 0) - ISNULL(eTKD_FutTum05.SyaRyoTes, 0)
                        END) AS EtcSyaRyoSum -- 自社その他付帯・合計
        ,0 AS HighwayUriGakKin --       通行料付帯・売上額
        ,0 AS HighwaySyaRyoSyo --       通行料付帯・消費税額
        ,0 AS HighwaySyaRyoTes --       通行料付帯・手数料額
        ,0 AS HighwaySyaRyoSum --       自社通行料付帯・合計
        ,0 AS HotelUriGakKin -- 宿泊料付帯・売上額
        ,0 AS HotelSyaRyoSyo -- 宿泊料付帯・消費税額
        ,0 AS HotelSyaRyoTes -- 宿泊料付帯・手数料額
        ,0 AS HotelSyaRyoSum -- 自社宿泊料付帯・合計
        ,0 AS ParkingUriGakKin --       駐車料付帯・売上額
        ,0 AS ParkingSyaRyoSyo --       駐車料付帯・消費税額
        ,0 AS ParkingSyaRyoTes --       駐車料付帯・手数料額
        ,0 AS ParkingSyaRyoSum --       自社駐車料付帯・合計
        ,0 AS OtherUriGakKin -- その他付帯・売上額
        ,0 AS OtherSyaRyoSyo -- その他付帯・消費税額
        ,0 AS OtherSyaRyoTes -- その他付帯・手数料額
        ,0 AS OtherSyaRyoSum -- 自社その他付帯・合計
        ,SUM(ISNULL(eTKD_Haisha02.SyaRyoUnc, 0)) AS S_JyuSyaRyoUnc -- 日計/累計・受注運賃
        ,SUM(ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) AS S_JyuSyaRyoSyo -- 日計/累計・受注消費税
        ,SUM(ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) AS S_JyuSyaRyoTes -- 日計/累計・受注手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)
                        END) AS S_JyuSyaRyoRui -- 日計/累計・受注累計
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0)) AS S_JisSyaRyoUnc -- 日計/累計・自社運賃
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) AS S_JisSyaRyoSyo -- 日計/累計・自社消費税
        ,SUM(ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) AS S_JisSyaRyoTes -- 日計/累計・自社手数料額
        ,SUM(ISNULL(eTKD_FutTum03.UriGakKin, 0)) AS S_FutUriGakKin -- 日計/累計・自社付帯売上
        ,SUM(ISNULL(eTKD_FutTum03.SyaRyoSyo, 0)) AS S_FutSyaRyoSyo -- 日計/累計・自社付帯消費税
        ,SUM(ISNULL(eTKD_FutTum03.SyaRyoTes, 0)) AS S_FutSyaRyoTes -- 日計/累計・自社付帯手数料額
        ,SUM(ISNULL(eTKD_Haisha02.YoushaUnc, 0)) AS S_YoushaUnc -- 日計/累計・傭車運賃
        ,SUM(ISNULL(eTKD_Haisha02.YoushaSyo, 0)) AS S_YoushaSyo -- 日計/累計・傭車消費税
        ,SUM(ISNULL(eTKD_Haisha02.YoushaTes, 0)) AS S_YoushaTes -- 日計/累計・傭車手数料額
        ,SUM(ISNULL(eTKD_YFutTu04.HaseiKin, 0)) AS S_YfuUriGakKin -- 日計/累計・傭車付帯売上
        ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoSyo, 0)) AS S_YfuSyaRyoSyo -- 日計/累計・傭車付帯消費税
        ,SUM(ISNULL(eTKD_YFutTu04.SyaRyoTes, 0)) AS S_YfuSyaRyoTes -- 日計/累計・傭車付帯手数料額
        ,0 AS JyuSyaRyoUnc -- 受注運賃
        ,0 AS JyuSyaRyoSyo -- 受注消費税
        ,0 AS JyuSyaRyoTes -- 受注手数料額
        ,SUM(CASE 
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '0'
                                THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0))
                        WHEN '' + ISNULL(@TesuInKbn, ' ') + '' = '1'
                                THEN (ISNULL(eTKD_Haisha02.SyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.SyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.SyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.JisSyaRyoUnc, 0) + ISNULL(eTKD_Haisha02.JisSyaRyoSyo, 0) - ISNULL(eTKD_Haisha02.JisSyaRyoTes, 0)) - (ISNULL(eTKD_Haisha02.YoushaUnc, 0) + ISNULL(eTKD_Haisha02.YoushaSyo, 0) - ISNULL(eTKD_Haisha02.YoushaTes, 0))
                        END) AS S_Soneki
        FROM eTKD_Unkobi_1 AS eTKD_Unkobi01
        LEFT JOIN eTKD_Haisha_1 AS eTKD_Haisha02 ON eTKD_Haisha02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_Haisha02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_2 AS eTKD_FutTum03 ON eTKD_FutTum03.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum03.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_YFutTu_1 AS eTKD_YFutTu04 ON eTKD_YFutTu04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YFutTu04.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_YykSyu_1 AS eTKD_YykSyu02 ON eTKD_YykSyu02.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_YykSyu02.UnkRen = eTKD_Unkobi01.UnkRen
        LEFT JOIN eTKD_FutTum_1 AS eTKD_FutTum04 ON eTKD_FutTum04.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum04.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum04.FutGuiKbn = 5
        LEFT JOIN eTKD_FutTum_3 AS eTKD_FutTum05 ON eTKD_FutTum05.UkeNo = eTKD_Unkobi01.UkeNo
                AND eTKD_FutTum05.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_FutTum05.FutGuiKbn = 0
        LEFT JOIN VPM_Compny AS eVPM_Compny17 ON eVPM_Compny17.CompanyCdSeq = eTKD_Unkobi01.CompanyCdSeq
        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny17.TenantCdSeq
                AND eVPM_Tenant.SiyoKbn = 1
        WHERE (@CompanyCd IS NULL OR	eVPM_Compny17.CompanyCd = @CompanyCd)
        AND  (@EigyoCdSeq  IS NULL OR  eTKD_Unkobi01.EigyoCd = @EigyoCdSeq)
        AND (@UriYmdFrom IS NULL OR    eTKD_Unkobi01.UriYmd >= @UriYmdFrom)
        AND (@UriYmdTo IS NULL OR eTKD_Unkobi01.UriYmd <= @UriYmdTo)
        AND (@UkeNoFrom IS NULL OR eTKD_Unkobi01.UkeNo >= @UkeNoFrom)
        AND (@UkeNoTo IS NULL OR eTKD_Unkobi01.UkeNo <= @UkeNoTo)
        AND (@YoyaKbnFrom IS NULL OR (eTKD_Unkobi01.YoyaKbn >= @YoyaKbnFrom))
		AND (@YoyaKbnTo IS NULL OR (eTKD_Unkobi01.YoyaKbn <= @YoyaKbnTo))
        AND (@TenantCdSeq IS NULL OR eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
        ORDER BY EigyoCd
                ,UriYmd
	END

SET @ROWCOUNT = @@ROWCOUNT
END
GO


