USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBillingListGridWithoutOutputClassification_R]    Script Date: 1/20/2021 1:35:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetBillingListAsync
-- Date			:   2020/10/20
-- Author		:   T.L.DUY
-- Description	:   Get billing list grid without classification data with conditions
------------------------------------------------------------
ALTER   PROCEDURE [dbo].[PK_dBillingListGridWithoutOutputClassification_R]
		(
		--Parameter
			@TenantCdSeq			INT,
			@BillDate				NVARCHAR(6),
			@CloseDate				TINYINT,
			@StartBillAddress		NVARCHAR(11),				
			@EndBillAddress			NVARCHAR(11),				
			@BillOffice			INT,				
			@StartReservationClassification			TINYINT,				
			@EndReservationClassification			TINYINT,	
			@StartReceiptNumber NVARCHAR(15),                -- 予約番号　開始
			@EndReceiptNumber	NVARCHAR(15),                  -- 予約番号　終了	
			@StartBillClassification NVARCHAR(10),       -- 請求区分 開始
			@EndBillClassification NVARCHAR(10),         -- 請求区分 終了
			@BillTypes			NVARCHAR(100),
			@BillIssuedClassification TINYINT,
			@Offset				INT,					--Offset rows data
			@Limit				INT,					--Limit rows data
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
-- 入金支払明細テーブル
WITH eTKD_NyShmi01 AS (
	SELECT TKD_NyShmi.UkeNo AS UkeNo,
		TKD_NyShmi.UnkRen AS UnkRen,
        TKD_NyShmi.NyuSihRen AS NyuSihRen,
        TKD_NyShmi.NyuSihTblSeq AS NyuSihTblSeq,
        TKD_NyShmi.NyuSihKbn AS NyuSihKbn,
        TKD_NyShmi.FutTumRen AS FutTumRen,
        TKD_NyShmi.SeiFutSyu AS SeiFutSyu,
        eTKD_NyuSih01.NyuSihYmd AS NyuSihYmd,
        eTKD_Yyksho01.SeiTaiYmd AS SeiTaiYmd,
        TKD_NyShmi.KesG + TKD_NyShmi.FurKesG AS NyukinG
	FROM TKD_NyShmi
	INNER JOIN TKD_NyuSih AS eTKD_NyuSih01
		ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND TKD_NyShmi.NyuSihKbn = 1
		AND TKD_NyShmi.SiyoKbn = 1
		AND eTKD_NyuSih01.SiyoKbn = 1
	INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
		ON TKD_NyShmi.UkeNo = eTKD_Yyksho01.UkeNo
		AND eTKD_Yyksho01.SiyoKbn = 1
		AND eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
),
eTKD_Kuri01 AS (
	SELECT eTKD_Kuri.SyoTokuiSeq AS SyoTokuiSeq,
		eTKD_Kuri.SyoSitenSeq AS SyoSitenSeq,
		eTKD_Kuri.YoyaKbn,
		eTKD_Kuri.SyoEigyoSeq,
		eTKD_Kuri.SyoriYm,
		SUM(
			CASE
				WHEN eTKD_Kuri.SeiFutSyu IN (1) THEN CAST(eTKD_Kuri.KuriKin as bigint)
				ELSE 0
			END
		) AS UriZenZan,
		SUM(
			CASE
				WHEN eTKD_Kuri.SeiFutSyu IN (5) THEN CAST(eTKD_Kuri.KuriKin as bigint)
				ELSE 0
			END
		) AS GaiZenZan,
		SUM(
			CASE
				WHEN eTKD_Kuri.SeiFutSyu IN(1, 5, 7) THEN 0
				ELSE CAST(eTKD_Kuri.KuriKin as bigint)
			END
		) AS EtcZenZan,
		SUM(
			CASE
				WHEN eTKD_Kuri.SeiFutSyu IN(7) THEN CAST(eTKD_Kuri.KuriKin as bigint)
				ELSE 0
			END
		) AS CanZenZan
	FROM TKD_Kuri AS eTKD_Kuri
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
		ON eTKD_Kuri.SyoEigyoSeq = eVPM_Eigyos01.EigyoCdSeq
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
		ON eTKD_Kuri.SyoTokuiSeq = eVPM_TokiSt02.TokuiSeq
		AND eTKD_Kuri.SyoSitenSeq = eVPM_TokiSt02.SitenCdSeq
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
		ON eTKD_Kuri.SyoTokuiSeq = eVPM_Tokisk02.TokuiSeq
		AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
		ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
	WHERE eTKD_Kuri.SeinKbn = 2
		AND eTKD_Kuri.SiyoKbn = 1
		AND eTKD_Kuri.KuriKin <> 0

		AND (TRIM(@BillDate) = '' OR eTKD_Kuri.SyoriYm = CAST((CAST(@BillDate AS int) - 1) as NVARCHAR(6)))																												-- 請求年月 - 1月
		AND (@CloseDate = 0 OR eVPM_TokiSt02.SimeD = @CloseDate)																													-- 締日
		AND (@BillOffice = 0 OR eTKD_Kuri.SyoEigyoSeq = @BillOffice)																													-- 請求営業所
AND (TRIM(@StartBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
AND (TRIM(@EndBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
		AND (TRIM(@BillTypes) = '' OR eTKD_Kuri.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))																												-- チェックした各種別

	GROUP BY SyoTokuiSeq,
		SyoSitenSeq,
		YoyaKbn,
		eTKD_Kuri.SyoEigyoSeq,
		eTKD_Kuri.SyoriYm
),
-- 得意先ごとの締日に合わせた入金年月日(開始と終了)を求める
eTKD_NyuSih01 AS (
	SELECT ISNULL(TKD_NyShmi.UkeNo, 0) AS UkeNo,
		ISNULL(TKD_NyShmi.UnkRen, 0) AS UnkRen,
		ISNULL(TKD_NyShmi.NyuSihRen, 0) AS NyuSihRen,
		ISNULL(TKD_NyShmi.SeiFutSyu, 0) AS SeiFutSyu,
		ISNULL(TKD_NyuSih.NyuSihYmd, '') AS NyuSihYmd,
		ISNULL(TKD_NyShmi.NyuSihTblSeq, 0) AS NyuSihTblSeq,
		CASE
			WHEN eVPM_TokiSt02.SimeD = 31 THEN CONVERT(VARCHAR, (@BillDate)) + '01'
			ELSE CONVERT(VARCHAR,DATEADD(DAY, 1, DATEADD(MONTH, -1, CONVERT(DATE, (@BillDate + RIGHT('01' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt02.SimeD, '')), 2))))), 112)
		END AS SimeStartD,
        CASE
            WHEN eVPM_TokiSt02.SimeD = 31 THEN CONVERT(VARCHAR, DATEADD(DAY, -1, DATEADD(MONTH, 1, CONVERT(DATE, @BillDate + '01'))), 112)
            ELSE CONVERT(VARCHAR, @BillDate) + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt02.SimeD, '')), 2)
        END AS SimeEndD
	FROM TKD_NyuSih
	INNER JOIN TKD_NyShmi
		ON TKD_NyuSih.NyuSihTblSeq = TKD_NyShmi.NyuSihTblSeq
		AND TKD_NyShmi.NyuSihKbn = 1
		AND TKD_NyShmi.SiyoKbn = 1
	INNER JOIN TKD_Yyksho
		ON TKD_NyShmi.UkeNo = TKD_Yyksho.UkeNo
		AND TKD_Yyksho.SiyoKbn = 1
	INNER JOIN VPM_TokiSt AS eVPM_TokiSt01
		ON TKD_Yyksho.TokuiSeq = eVPM_TokiSt01.TokuiSeq
		AND TKD_Yyksho.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
		AND TKD_NyuSih.NyuSihYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
	INNER JOIN VPM_Tokisk AS eVPM_Tokisk02 
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
		AND TKD_NyuSih.NyuSihYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
		AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	INNER JOIN VPM_TokiSt AS eVPM_TokiSt02 
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
		AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
		AND TKD_NyuSih.NyuSihYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd),
-- 入金支払明細テーブル 受付番号集計 【入金累計】
eTKD_NyShmi02 AS (
	SELECT eTKD_Yyksho.TokuiSeq AS TokuiSeq,
		eTKD_Yyksho.SitenCDSeq AS SitenCdSeq,
		eTKD_NyShmi.SeiFutSyu AS SeiFutSyu,
		SUM(CAST((eTKD_NyShmi.KesG+eTKD_NyShmi.FurKesG) as bigint)) AS NyukinG
	FROM TKD_NyShmi AS eTKD_NyShmi
	INNER JOIN eTKD_NyuSih01
		ON eTKD_NyShmi.UkeNo = eTKD_NyuSih01.UkeNo
		AND eTKD_NyShmi.UnkRen = eTKD_NyuSih01.UnkRen
		AND eTKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND eTKD_NyShmi.NyuSihRen = eTKD_NyuSih01.NyuSihRen
		AND eTKD_NyShmi.NyuSihKbn = 1
	LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Yyksho.Ukeno=eTKD_NyShmi.Ukeno
		AND eTKD_Yyksho.SiyoKbn = 1
	WHERE TRIM(@BillDate) = '' OR SUBSTRING(eTKD_yyksho.SeiTaiYmd, 1, 6) = @BillDate -- 請求年月
		AND eTKD_Yyksho.SeiTaiYmd < = eTKD_NyuSih01.NyuSihYmd
		AND eTKD_NyShmi.SiyoKbn = 1
	GROUP BY eTKD_yyksho.TokuiSeq,
		eTKD_Yyksho.SitenCdSeq,
		eTKD_NyShmi.SeiFutSyu
),
-- 入金支払明細テーブル 受付番号・運行日連番・付帯積込品連番集計 【入金累計】
eTKD_NyShmi03 AS (
	SELECT eTKD_Yyksho.TokuiSeq AS TokuiSeq,
		eTKD_Yyksho.SitenCdSeq AS SitenCdSeq,
        SUM(
			CASE
				WHEN eTKD_NyShmi.SeiFutSyu IN (1) THEN CAST((eTKD_NyShmi.KesG + eTKD_NyShmi.FurKesG) as bigint)
                ELSE 0
            END
		) AS UriNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi.SeiFutSyu IN (5) THEN CAST((eTKD_NyShmi.KesG + eTKD_NyShmi.FurKesG) as bigint)
				ELSE 0
			END
		) AS GaiNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi.SeiFutSyu IN (7) THEN CAST((eTKD_NyShmi.KesG + eTKD_NyShmi.FurKesG) as bigint)
				ELSE 0
			END
		) AS CanNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi.SeiFutSyu IN (1, 5, 7) THEN 0
				ELSE CAST((eTKD_NyShmi.KesG+eTKD_NyShmi.FurKesG) as bigint)
			END
		) AS EtcNyukinG
	FROM TKD_NyShmi AS eTKD_NyShmi
	INNER JOIN eTKD_NyuSih01
		ON eTKD_NyShmi.UkeNo = eTKD_NyuSih01.UkeNo
		AND eTKD_NyShmi.UnkRen = eTKD_NyuSih01.UnkRen
		AND eTKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND eTKD_NyShmi.NyuSihRen = eTKD_NyuSih01.NyuSihRen
		AND eTKD_NyShmi.NyuSihKbn = 1
   LEFT JOIN TKD_Yyksho AS eTKD_Yyksho
	   ON eTKD_Yyksho.Ukeno=eTKD_NyShmi.Ukeno
	   AND eTKD_Yyksho.SiyoKbn = 1
   WHERE TRIM(@BillDate) = '' OR SUBSTRING(eTKD_yyksho.SeiTaiYmd, 1, 6) = @BillDate
		AND eTKD_Yyksho.SeiTaiYmd < = eTKD_NyuSih01.NyuSihYmd
		AND eTKD_NyShmi.SiyoKbn = 1
	GROUP BY eTKD_yyksho.TokuiSeq,
		eTKD_Yyksho.SitenCdSeq
),
-- 入金支払明細テーブル 受付番号集計 【前受金】
eTKD_NyShmi04 AS (
	SELECT eTKD_NyShmi01.UkeNo AS UkeNo,
		eTKD_NyShmi01.SeiFutSyu AS SeiFutSyu,
		SUM(CAST(eTKD_NyShmi01.NyukinG as bigint)) AS NyukinG
	FROM eTKD_NyShmi01
	INNER JOIN eTKD_NyuSih01
		ON eTKD_NyShmi01.UkeNo = eTKD_NyuSih01.UkeNo
		AND eTKD_NyShmi01.UnkRen = eTKD_NyuSih01.UnkRen
		AND eTKD_NyShmi01.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND eTKD_NyShmi01.NyuSihRen = eTKD_NyuSih01.NyuSihRen
		AND eTKD_NyShmi01.NyuSihKbn = 1
	WHERE eTKD_NyShmi01.NyuSihYmd < eTKD_NyShmi01.SeiTaiYmd
	GROUP BY eTKD_NyShmi01.UkeNo,
		eTKD_NyShmi01.SeiFutSyu
),
-- 入金支払明細テーブル 受付番号・運行日連番・付帯積込品連番集計 【前受金】
eTKD_NyShmi05 AS (
	SELECT eTKD_NyShmi01.UkeNo AS UkeNo,
		eTKD_NyShmi01.UnkRen AS UnkRen,
		eTKD_NyShmi01.FutTumRen AS FutTumRen,
		eTKD_NyShmi01.SeiFutSyu AS SeiFutSyu,
		SUM(CAST(eTKD_NyShmi01.NyukinG as bigint)) AS NyukinG
	FROM eTKD_NyShmi01
	INNER JOIN eTKD_NyuSih01
		ON eTKD_NyShmi01.UkeNo = eTKD_NyuSih01.UkeNo
		AND eTKD_NyShmi01.UnkRen = eTKD_NyuSih01.UnkRen
		AND eTKD_NyShmi01.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND eTKD_NyShmi01.NyuSihRen = eTKD_NyuSih01.NyuSihRen
		AND eTKD_NyShmi01.NyuSihKbn = 1
	WHERE eTKD_NyShmi01.NyuSihYmd < eTKD_NyShmi01.SeiTaiYmd
	GROUP BY eTKD_NyShmi01.UkeNo,
		eTKD_NyShmi01.UnkRen,
		eTKD_NyShmi01.FutTumRen,
		eTKD_NyShmi01.SeiFutSyu
),
-- 入金支払明細テーブル 受付番号集計 【前受金】
eTKD_SeiMei01 AS (
	SELECT TKD_SeiMei.UkeNo AS UkeNo,
		TKD_SeiMei.MisyuRen AS MisyuRen,
		MAX(TKD_SeiPrS.SeiHatYmd) AS SeiHatYmd
	FROM TKD_SeiMei
	LEFT JOIN TKD_SeiPrS
		ON TKD_SeiMei.SeiOutSeq = TKD_SeiPrS.SeiOutSeq
		AND TKD_SeiPrS.SiyoKbn = 1
	WHERE TKD_SeiMei.SiyoKbn = 1
	GROUP BY TKD_SeiMei.UkeNo,
		TKD_SeiMei.MisyuRen
)
SELECT DISTINCT COUNT(*) OVER(ORDER BY (SELECT NULL)) AS CountNumber,
-- 請求営業所
	ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigyoCd,
	ISNULL(eVPM_Eigyos01.EigyoNm, '') AS EigyoNm,
	ISNULL(eVPM_Eigyos01.RyakuNm, '') AS EigyoRyak,
-- 請求先業者
	ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS SeiGyosyaCd,
-- 請求先
	ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS SeiCd,
	ISNULL(eVPM_Tokisk02.TokuiSeq, 0) AS SeiCdSeq,
	ISNULL(eVPM_Tokisk02.TokuiNm, '') AS SeiCdNm,
	ISNULL(eVPM_Tokisk02.RyakuNm, '') AS SeiRyakuNm,
-- 請求先支店
	ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SeiSitenCd,
	ISNULL(eVPM_TokiSt02.SitenCdSeq, 0) AS SeiSitenCdSeq,
	ISNULL(eVPM_TokiSt02.SitenNm, '') AS SeiSitenCdNm,
	ISNULL(eVPM_TokiSt02.RyakuNm, '') AS SeiSitRyakuNm,
-- 請求先業者
	ISNULL(eVPM_Gyosya02.GyosyaNm, '') AS SeiGyosyaCdNm,
-- 売上・前月繰越額
	ISNULL(eTKD_Kuri11.UriZenZan, 0) AS UriZenZan,
-- 売上・売上集計
	SUM(
		CASE
			WHEN TKD_Mishum.SeiFutSyu IN (1) THEN CAST(TKD_Mishum.UriGakKin as bigint)
			ELSE 0
		END
	) AS UriUriGakKin,
-- 売上・消費税額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1) THEN CAST(TKD_Mishum.SyaRyoSyo as bigint)
            ELSE 0
        END
	) AS UriSyaRyoSyo,
-- 売上・手数料額集計
    SUM(CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1) THEN CAST(TKD_Mishum.SyaRyoTes as bigint)
            ELSE 0
        END) AS UriSyaRyoTes,
-- 売上・請求額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1) THEN CAST(TKD_Mishum.SeiKin as bigint)
            ELSE 0
        END
	) AS UriSeiKin,
-- 売上・入金集計
    MAX(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0)
            ELSE 0
        END
	) AS UriNyuKinRui,
-- 売上・未収額計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1) THEN CAST((TKD_Mishum.Seikin+ISNULL(eTKD_Kuri11.UriZenZan, 0)) as bigint) - CAST(ISNULL(eTKD_NyShmi11.NyukinG, 0) as bigint)
            ELSE 0
        END
	) AS UriMisyuKin,
-- 売上・前受金集計
	SUM(CASE
        WHEN TKD_Mishum.SeiFutSyu IN (1) THEN CAST(ISNULL(eTKD_NyShmi13.NyukinG, 0) as bigint)
        ELSE 0
    END) AS UriMaeuke,
-- ガイド料・前月繰越額
    ISNULL(eTKD_Kuri11.GaiZenZan, 0) AS GaiZenZan,
-- ガイド料・売上集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN CAST(TKD_Mishum.UriGakKin as bigint)
            ELSE 0
        END
	) AS GaiGaiGakKin,
-- ガイド料・消費税額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN CAST(TKD_Mishum.SyaRyoSyo as bigint)
            ELSE 0
        END
	) AS GaiSyaRyoSyo,
-- ガイド料・手数料額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN CAST(TKD_Mishum.SyaRyoTes as bigint)
            ELSE 0
        END
	) AS GaiSyaRyoTes,
-- ガイド料・請求額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN CAST(TKD_Mishum.SeiKin as bigint)
            ELSE 0
        END
	) AS GaiSeiKin,
-- ガイド料・入金集計
    MAX(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN ISNULL(eTKD_NyShmi12.GaiNyukinG, 0)
            ELSE 0
        END
	) AS GaiNyuKinRui,
-- ガイド料・未収額計 
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN CAST((TKD_Mishum.Seikin+ISNULL(eTKD_Kuri11.GaiZenZan, 0)) as bigint) - CAST(ISNULL(eTKD_NyShmi12.GaiNyukinG, 0) as bigint)
            ELSE 0
        END
	) AS GaiMisyuKin,
-- ガイド料・前受金集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (5) THEN CAST(ISNULL(eTKD_NyShmi14.NyukinG, 0) as bigint)
            ELSE 0
        END
	) AS GaiMaeuke,
-- その他付帯・前月繰越額
    ISNULL(eTKD_Kuri11.EtcZenZan, 0) AS EtcZenZan,
-- その他付帯・売上集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
            ELSE CAST(TKD_Mishum.UriGakKin as bigint)
        END) AS EtcEtcGakKin,
-- その他付帯・消費税額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
            ELSE CAST(TKD_Mishum.SyaRyoSyo as bigint)
        END
	) AS EtcSyaRyoSyo,
-- その他付帯・手数料額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
            ELSE CAST(TKD_Mishum.SyaRyoTes as bigint)
        END
	) AS EtcSyaRyoTes,
-- その他付帯・請求額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
            ELSE CAST(TKD_Mishum.SeiKin as bigint)
        END
	) AS EtcSeiKin,
-- その他付帯・入金集計
	MAX(ISNULL(eTKD_NyShmi12.EtcNyukinG, 0)) AS EtcNyuKinRui,
-- その他付帯・未収額計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(1, 5, 7) THEN 0
            ELSE CAST((TKD_Mishum.SeiKin+ISNULL(eTKD_Kuri11.EtcZenZan, 0)) as bigint) - CAST(ISNULL(eTKD_NyShmi12.EtcNyukinG, 0) as bigint)
        END
	) AS EtcMisyuKin,
-- その他付帯・前受金集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(1, 5, 7) THEN 0
            ELSE CAST(ISNULL(eTKD_NyShmi14.NyukinG, 0) as bigint)
        END
	) AS EtcMaeuke,
-- キャンセル料・前月繰越額
    ISNULL(eTKD_Kuri11.CanZenZan, 0) AS CanZenZan,
-- キャンセル料・売上集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN CAST(TKD_Mishum.UriGakKin as bigint)
            ELSE 0
        END
	) AS CanCanGakKin,
-- キャンセル料・消費税額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN CAST(TKD_Mishum.SyaRyoSyo as bigint)
            ELSE 0
        END
	) AS CanSyaRyoSyo,
-- キャンセル料・手数料額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN CAST(TKD_Mishum.SyaRyoTes as bigint)
            ELSE 0
        END
	) AS CanSyaRyoTes,
-- キャンセル料・請求額集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN CAST(TKD_Mishum.SeiKin as bigint)
            ELSE 0
        END
	) AS CanSeiKin,
-- キャンセル料・入金集計
    MAX(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0)
            ELSE 0
        END
	) AS CanNyuKinRui,
-- キャンセル料・未収額計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN CAST((TKD_Mishum.Seikin+ISNULL(eTKD_Kuri11.CanZenZan, 0)) as bigint) - CAST(ISNULL(eTKD_NyShmi11.NyukinG, 0) as bigint)
            ELSE 0
        END
	) AS CanMisyuKin,
-- キャンセル料・前受金集計
    SUM(
		CASE
            WHEN TKD_Mishum.SeiFutSyu IN(7) THEN CAST(ISNULL(eTKD_NyShmi13.NyukinG, 0) as bigint)
            ELSE 0
        END
	) AS CanMaeuke,
-- 売上・手数料区分
	ISNULL(eVPM_TokiSt02.TesKbn, 0) AS UriTesKbn,
-- 付帯・手数料区分
	ISNULL(eVPM_TokiSt02.TesKbnFut, 0) AS GaiTesKbn,
-- ガイド料・手数料区分
	ISNULL(eVPM_TokiSt02.TesKbnGui, 0) AS EtcTesKbn,
-- 売上・手数料区分名
	ISNULL(eVPM_CodeKb01.CodeKbnNm, 0) AS UriTesKbnNm,
-- 付帯・手数料区分名
	ISNULL(eVPM_CodeKb02.CodeKbnNm, 0) AS GaiTesKbnNm,
-- ガイド料・手数料区分名
	ISNULL(eVPM_CodeKb03.CodeKbnNm, 0) AS EtcTesKbnNm
FROM TKD_Mishum
INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
	ON TKD_Mishum.UkeNo = eTKD_Yyksho01.UkeNo
	AND TKD_Mishum.SiyoKbn = 1
	AND eTKD_Yyksho01.SiyoKbn = 1
	AND ((TKD_Mishum.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (TKD_Mishum.SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))
-- 予約区分マスタ
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
	ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
-- 請求営業所
INNER JOIN VPM_Eigyos AS eVPM_Eigyos01
	ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
-- 得意先支店
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
	ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
	AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
-- 請求先
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- 請求先支店
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
-- 請求先業者
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
LEFT JOIN eTKD_Kuri01 AS eTKD_Kuri11
	ON eTKD_Kuri11.SyoTokuiSeq=eVPM_TokiSt02.SeiCdSeq
	AND eTKD_Kuri11.SyoSitenSeq=eVPM_TokiSt02.SeiSitenCdSeq
-- 入金支払明細 受付番号集計【入金累計】
LEFT JOIN eTKD_NyShmi02 AS eTKD_NyShmi11
	ON eTKD_NyShmi11.TokuiSeq = eTKD_Yyksho01.TokuiSeq
	AND eTKD_NyShmi11.SitenCdSeq = eTKD_Yyksho01.SitenCdSeq
	AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi11.SeiFutSyu
-- 入金支払明細 受付番号・運行日連番・付帯積込品連番集計【入金累計】
LEFT JOIN eTKD_NyShmi03 AS eTKD_NyShmi12
	ON eTKD_NyShmi12.TokuiSeq = eTKD_Yyksho01.TOkuiSeq
	AND eTKD_NyShmi12.SitenCdSeq = eTKD_Yyksho01.SitenCdSeq
-- 入金支払明細 受付番号集計【前受金】
LEFT JOIN eTKD_NyShmi04 AS eTKD_NyShmi13
	ON TKD_Mishum.UkeNo = eTKD_NyShmi13.UkeNo
	AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi13.SeiFutSyu
-- 入金支払明細 受付番号・運行日連番・付帯積込品連番集計【前受金】
LEFT JOIN eTKD_NyShmi05 AS eTKD_NyShmi14
	ON TKD_Mishum.UkeNo = eTKD_NyShmi14.UkeNo
	AND TKD_Mishum.FutuUnkRen = eTKD_NyShmi14.UnkRen
	AND TKD_Mishum.FutTumRen = eTKD_NyShmi14.FutTumRen
	AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi14.SeiFutSyu
-- コード区分マスタ 売上・手数料区分
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
	ON eVPM_CodeKb01.CodeSyu = 'TESKBN'
	AND eVPM_CodeKb01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	AND eVPM_TokiSt02.TesKbn = eVPM_CodeKb01.CodeKbn
-- コード区分マスタ 付帯・手数料区分
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02
	ON eVPM_CodeKb02.CodeSyu = 'TESKBNFUT'
	AND eVPM_TokiSt02.TesKbnFut = eVPM_CodeKb02.CodeKbn
	AND eVPM_CodeKb02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- コード区分マスタ ガイド料・手数料区分
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb03
	ON eVPM_CodeKb03.CodeSyu = 'TESKBNGUI'
	AND eVPM_TokiSt02.TesKbnGui = eVPM_CodeKb03.CodeKbn
	AND eVPM_CodeKb03.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- コード区分マスタ 請求区分
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb04
	ON eVPM_CodeKb04.CodeSyu = 'SEIKYUKBN'
	AND eTKD_Yyksho01.SeiKyuKbnSeq = eVPM_CodeKb04.CodeKbnSeq
	AND eVPM_CodeKb04.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- 請求明細
LEFT JOIN eTKD_SeiMei01 AS eTKD_SeiMei11
	ON TKD_Mishum.UkeNo = eTKD_SeiMei11.UkeNo
	AND TKD_Mishum.MisyuRen = eTKD_SeiMei11.MisyuRen
WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq

AND (@BillDate = '' OR eTKD_Yyksho01.SeikYm = @BillDate)
AND (@CloseDate = 0 OR eVPM_TokiSt02.SimeD = @CloseDate)
AND (@BillOffice = 0 OR eVPM_Eigyos01.EigyoCd = @BillOffice)
AND (TRIM(@StartBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
AND (TRIM(@EndBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
AND (@StartReceiptNumber = '' OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber)
AND (@EndReceiptNumber = '' OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber)
AND (@StartReservationClassification = 0 OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)
AND (@EndReservationClassification = 0 OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)
AND (@StartBillClassification = 0 OR eVPM_CodeKb04.CodeKbn >= @StartBillClassification)
AND (@EndBillClassification = 0 OR eVPM_CodeKb04.CodeKbn <= @EndBillClassification)	
AND ((@BillIssuedClassification = 0) OR (@BillIssuedClassification = 1 AND eTKD_SeiMei11.SeiHatYmd <> '') OR (@BillIssuedClassification = 2 AND eTKD_SeiMei11.SeiHatYmd = ''))
GROUP BY ISNULL(eVPM_Eigyos01.EigyoCd, 0),
	ISNULL(eVPM_Eigyos01.EigyoNm, ''),
	ISNULL(eVPM_Eigyos01.RyakuNm, ''),
	ISNULL(eVPM_Gyosya02.GyosyaCd, 0),
	ISNULL(eVPM_Tokisk02.TokuiCd, 0),
	ISNULL(eVPM_Tokisk02.TokuiSeq, 0),
	ISNULL(eVPM_Tokisk02.TokuiNm, ''),
	ISNULL(eVPM_Tokisk02.RyakuNm, ''),
	ISNULL(eVPM_TokiSt02.SitenCd, 0),
	ISNULL(eVPM_TokiSt02.SitenCdSeq, 0),
	ISNULL(eVPM_TokiSt02.SitenNm, ''),
	ISNULL(eVPM_TokiSt02.RyakuNm, ''),
	ISNULL(eVPM_Gyosya02.GyosyaNm, ''),
	ISNULL(eVPM_TokiSt02.TesKbn, 0),
	ISNULL(eVPM_TokiSt02.TesKbnFut, 0),
	ISNULL(eVPM_TokiSt02.TesKbnGui, 0),
	ISNULL(eVPM_CodeKb01.CodeKbnNm, 0),
	ISNULL(eVPM_CodeKb02.CodeKbnNm, 0),
	ISNULL(eVPM_CodeKb03.CodeKbnNm, 0),
	ISNULL(eTKD_Kuri11.UriZenZan, 0),
	ISNULL(eTKD_Kuri11.GaiZenZan, 0),
	ISNULL(eTKD_Kuri11.EtcZenZan, 0),
	ISNULL(eTKD_Kuri11.CanZenZan, 0)
ORDER BY EigyoCd,
	SeiGyosyaCd,
	SeiCd,
	SeiSitenCd,
	SeiCdSeq,
	SeiSitenCdSeq
OFFSET @Offset ROWS
FETCH NEXT @Limit ROWS ONLY
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN