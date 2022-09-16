USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBillingListGridWithOutputClassification_R]    Script Date: 1/20/2021 1:35:58 PM ******/
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
-- Description	:   Get billing list grid with classification data with conditions
------------------------------------------------------------
ALTER   PROCEDURE [dbo].[PK_dBillingListGridWithOutputClassification_R]
		(
		--Parameter
			@TenantCdSeq			INT,
			@BillDate				NVARCHAR(6),
			@CloseDate				TINYINT,
			@StartBillAddress		NVARCHAR(11),				
			@EndBillAddress			NVARCHAR(11),				
			@BillOffice			INT,				
			@BillTypes			NVARCHAR(100),
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
-- (繰越あり用)請求付帯種別ごとの未収データ取得
eTKD_Mishum01 AS (
	SELECT eVPM_Eigyos01.EigyoCdSeq,
		eVPM_TokiSt02.TokuiSeq,
		eVPM_TokiSt02.SitenCdSeq,
        SUM(
			CASE
                WHEN SeiFutSyu IN (1) THEN CAST(ISNULL(UriGakKin, 0) as bigint)
                ELSE 0
            END
		) AS UriUriGakKin,
        SUM(
			CASE
                WHEN SeiFutSyu IN (1) THEN CAST(ISNULL(SyaRyoSyo, 0) as bigint)
                ELSE 0
            END
		) AS UriSyaRyoSyo,
        SUM(
			CASE
                WHEN SeiFutSyu IN (1) THEN CAST(ISNULL(SyaRyoTes, 0) as bigint)
                ELSE 0
            END
		) AS UriSyaRyoTes,
        SUM(
			CASE
                WHEN SeiFutSyu IN (1) THEN CAST(ISNULL(SeiKin, 0) as bigint)
                ELSE 0
            END
		) AS UriSeiKin,
        SUM(
			CASE
                WHEN SeiFutSyu IN (5) THEN CAST(ISNULL(UriGakKin, 0) as bigint)
                ELSE 0
            END
		) AS GaiGaiGakKin,
        SUM(
			CASE
                WHEN SeiFutSyu IN (5) THEN CAST(ISNULL(SyaRyoSyo, 0) as bigint)
                ELSE 0
            END
		) AS GaiSyaRyoSyo,
        SUM(
			CASE
                WHEN SeiFutSyu IN (5) THEN CAST(ISNULL(SyaRyoTes, 0) as bigint)
                ELSE 0
            END
		) AS GaiSyaRyoTes,
        SUM(
			CASE
                WHEN SeiFutSyu IN (5) THEN CAST(ISNULL(SeiKin, 0) as bigint)
                ELSE 0
            END
		) AS GaiSeiKin,
        SUM(
			CASE
                WHEN SeiFutSyu NOT IN (1, 5, 7) THEN CAST(ISNULL(UriGakKin, 0) as bigint)
                ELSE 0
            END
		) AS EtcEtcGakKin,
        SUM(
			CASE
                WHEN SeiFutSyu NOT IN (1, 5, 7) THEN CAST(ISNULL(SyaRyoSyo, 0) as bigint)
                ELSE 0
            END
		) AS EtcSyaRyoSyo,
        SUM(
			CASE
                WHEN SeiFutSyu NOT IN (1, 5, 7) THEN CAST(ISNULL(SyaRyoTes, 0) as bigint)
                ELSE 0
            END
		) AS EtcSyaRyoTes,
        SUM(
			CASE
                WHEN SeiFutSyu NOT IN (1, 5, 7) THEN CAST(ISNULL(SeiKin, 0) as bigint)
                ELSE 0
            END
		) AS EtcSeiKin,
        SUM(
			CASE
                WHEN SeiFutSyu IN (7) THEN CAST(ISNULL(UriGakKin, 0) as bigint)
                ELSE 0
            END
		) AS CanCanGakKin,
        SUM(
			CASE
                WHEN SeiFutSyu IN (7) THEN CAST(ISNULL(SyaRyoSyo, 0) as bigint)
                ELSE 0
            END
		) AS CanSyaRyoSyo,
        SUM(
			CASE
                WHEN SeiFutSyu IN (7) THEN CAST(ISNULL(SyaRyoTes, 0) as bigint)
                ELSE 0
            END
		) AS CanSyaRyoTes,
        SUM(
			CASE
                WHEN SeiFutSyu IN (7) THEN CAST(ISNULL(SeiKin, 0) as bigint)
                ELSE 0
            END
		) AS CanSeiKin
	FROM TKD_Mishum
	LEFT JOIN TKD_Yyksho AS eTKD_Yyksho01 
		ON TKD_Mishum.UkeNo = eTKD_Yyksho01.UkeNo
		AND eTKD_Yyksho01.SiyoKbn = 1
		AND TKD_Mishum.SiyoKbn = 1
		AND ((SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))
	INNER JOIN VPM_Eigyos AS eVPM_Eigyos01
		ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
		ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
		AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
		AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
		AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
		ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq

	WHERE (@BillDate = '' OR eTKD_Yyksho01.SeikYm = @BillDate)																														-- 請求年月
		AND (@CloseDate = 0 OR eVPM_TokiSt02.SimeD = @CloseDate)																													-- 締日
		AND (@BillOffice = 0 OR eVPM_Eigyos01.EigyoCd = @BillOffice)																													-- 請求営業所
AND (TRIM(@StartBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
AND (TRIM(@EndBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
		AND (@BillTypes = '' OR TKD_Mishum.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))																												-- チェックした各種別

	GROUP BY eVPM_Eigyos01.EigyoCdSeq,
		eVPM_TokiSt02.TokuiSeq,
		eVPM_TokiSt02.SitenCdSeq
),
eTKD_NyShmi06 AS (
	SELECT ISNULL(eVPM_Eigyos01.EigyoCdSeq, 0) AS EigyoCdSeq,
		ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigyoCd,
		ISNULL(eVPM_Gyosya02.GyosyaCdSeq, 0) AS GyosyaCdSeq,
		ISNULL(eVPM_Tokisk02.TokuiSeq, 0) AS TokuiSeq,
		ISNULL(eVPM_TokiSt02.SitenCdSeq, 0) AS SitenCdSeq,
		ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS GyosyaCd,
		ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS TokuiCd,
		ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SitenCd,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu IN (1) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd <= eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS UriNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu IN (1) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd > eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS UriMaeUke,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu IN (5) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd <= eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS GaiNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu IN (5) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd > eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS GaiMaeUke,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu NOT IN (1, 5, 7) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd <= eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS EtcNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu NOT IN (1, 5, 7) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd > eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS ETCMaeUke,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu IN (7) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd <= eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS CanNyukinG,
		SUM(
			CASE
				WHEN eTKD_NyShmi01.SeiFutSyu IN (7) THEN
					CASE
						WHEN eTKD_Yyksho01.SeiTaiYmd > eTKD_NyuSih01.NyuSihYmd THEN CAST(eTKD_NyShmi01.NyukinG as bigint)
						ELSE 0
					END
				ELSE 0
			END
		) AS CanMaeUke
	FROM eTKD_NyShmi01
	LEFT JOIN TKD_Yyksho AS eTKD_Yyksho01
		ON eTKD_NyShmi01.UkeNo = eTKD_Yyksho01.UkeNo
		AND eTKD_Yyksho01.SiyoKbn = 1
		AND ((SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))
	INNER JOIN VPM_Eigyos AS eVPM_Eigyos01
		ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
		ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
		AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
		AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
		ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
	INNER JOIN eTKD_NyuSih01
		ON eTKD_NyShmi01.UkeNo = eTKD_NyuSih01.UkeNo
		AND eTKD_NyShmi01.UnkRen = eTKD_NyuSih01.UnkRen
		AND eTKD_NyShmi01.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND eTKD_NyShmi01.NyuSihRen = eTKD_NyuSih01.NyuSihRen
		AND eTKD_NyShmi01.NyuSihKbn = 1
	WHERE eTKD_NyuSih01.NyuSihYmd BETWEEN eTKD_NyuSih01.SimeStartD AND eTKD_NyuSih01.SimeEndD
		AND ((NOT(SUBSTRING(eTKD_Yyksho01.SeiTaiYmd, 1, 6) = @BillDate AND eTKD_Yyksho01.SeiTaiYmd > eTKD_NyuSih01.NyuSihYmd)) OR (eTKD_NyShmi01.NyuSihYmd < eTKD_NyShmi01.SeiTaiYmd))
		AND (@CloseDate = 0 OR eVPM_TokiSt02.SimeD = @CloseDate)																													-- 締日
		AND (@BillOffice = 0 OR eVPM_Eigyos01.EigyoCd = @BillOffice)																													-- 請求営業所
AND (TRIM(@StartBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
AND (TRIM(@EndBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
		AND (@BillTypes = '' OR eTKD_NyShmi01.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))																												-- チェックした各種別

	GROUP BY eVPM_Eigyos01.EigyoCdSeq,
		eVPM_Eigyos01.EigyoCd,
		eVPM_Gyosya02.GyosyaCdSeq,
		eVPM_Tokisk02.TokuiSeq,
		eVPM_TokiSt02.SitenCdSeq,
		eVPM_Gyosya02.GyosyaCd,
		eVPM_Tokisk02.TokuiCd,
		eVPM_TokiSt02.SitenCd
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
		AND (@BillDate = '' OR eTKD_Kuri.SyoriYm = CAST((CAST(@BillDate AS int) - 1) as NVARCHAR(6)))																											-- 請求年月 - 1月
		AND (@CloseDate = 0 OR eVPM_TokiSt02.SimeD = @CloseDate)																													-- 締日
		AND (@BillOffice = 0 OR eTKD_Kuri.SyoEigyoSeq = @BillOffice)																													-- 請求営業所
AND (TRIM(@StartBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
AND (TRIM(@EndBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
		AND (@BillTypes = '' OR eTKD_Kuri.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))																												-- チェックした各種別

	GROUP BY SyoTokuiSeq,
		SyoSitenSeq,
		YoyaKbn,
		eTKD_Kuri.SyoEigyoSeq,
		eTKD_Kuri.SyoriYm
)

SELECT
-- メインデータ
	ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigyoCd,
	ISNULL(eVPM_Eigyos01.EigyoNm, '') AS EigyoNm,
	ISNULL(eVPM_Eigyos01.RyakuNm, '') AS EigyoRyak,
	ISNULL(eVPM_Gyosya01.GyosyaCd, 0) AS SeiGyosyaCd,
	ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS SeiCd,
	ISNULL(eVPM_Tokisk01.TokuiSeq, 0) AS SeiCdSeq,
	ISNULL(eVPM_Tokisk01.TokuiNm, '') AS SeiCdNm,
	ISNULL(eVPM_Tokisk01.RyakuNm, '') AS SeiRyakuNm,
	ISNULL(eVPM_TokiSt01.SitenCd, 0) AS SeiSitenCd,
	ISNULL(eVPM_TokiSt01.SitenCdSeq, 0) AS SeiSitenCdSeq,
	ISNULL(eVPM_TokiSt01.SitenNm, '') AS SeiSitenCdNm,
	ISNULL(eVPM_TokiSt01.RyakuNm, '') AS SeiSitRyakuNm,
	ISNULL(eVPM_Gyosya01.GyosyaNm, '') AS SeiGyosyaCdNm,
-- [メイン]運賃
	SUM(CAST(ISNULL(UriZenZan, 0) as bigint)) AS UriZenZan,
	SUM(CAST(ISNULL(UriUriGakKin, 0) as bigint)) AS UriUriGakKin,
	SUM(CAST(ISNULL(UriSyaRyoSyo, 0) as bigint)) AS UriSyaRyoSyo,
	SUM(CAST(ISNULL(UriSyaRyoTes, 0) as bigint)) AS UriSyaRyoTes,
	SUM(CAST(ISNULL(UriSeiKin, 0) as bigint))AS UriSeiKin,
	SUM(CAST(ISNULL(UriNyuKinRui, 0) as bigint)) AS UriNyuKinRui,
	SUM(CAST((ISNULL(UriSeiKin, 0) +ISNULL(UriZenZan, 0) - ISNULL(UriNyuKinRui, 0)) as bigint)) AS UriMisyuKin,
	SUM(CAST(ISNULL(UriMaeuke, 0) as bigint)) AS UriMaeuke,
-- [メイン]ガイド料
	SUM(CAST(ISNULL(GaiZenZan, 0) as bigint)) AS GaiZenZan,
	SUM(CAST(ISNULL(GaiGaiGakKin, 0) as bigint)) AS GaiGaiGakKin,
	SUM(CAST(ISNULL(GaiSyaRyoSyo, 0) as bigint)) AS GaiSyaRyoSyo,
	SUM(CAST(ISNULL(GaiSyaRyoTes, 0) as bigint)) AS GaiSyaRyoTes,
	SUM(CAST(ISNULL(GaiSeiKin, 0) as bigint))AS GaiSeiKin,
	SUM(CAST(ISNULL(GaiNyuKinRui, 0) as bigint)) AS GaiNyuKinRui,
	SUM(CAST((ISNULL(GaiSeiKin, 0) +ISNULL(GaiZenZan, 0) - ISNULL(GaiNyuKinRui, 0)) as bigint)) AS GaiMisyuKin,
	SUM(CAST(ISNULL(GaiMaeuke, 0) as bigint)) AS GaiMaeuke,
-- [メイン]付帯
	SUM(CAST(ISNULL(EtcZenZan, 0) as bigint)) AS EtcZenZan,
	SUM(CAST(ISNULL(EtcEtcGakKin, 0) as bigint)) AS EtcEtcGakKin,
	SUM(CAST(ISNULL(EtcSyaRyoSyo, 0) as bigint)) AS EtcSyaRyoSyo,
	SUM(CAST(ISNULL(EtcSyaRyoTes, 0) as bigint)) AS EtcSyaRyoTes,
	SUM(CAST(ISNULL(EtcSeiKin, 0) as bigint))AS EtcSeiKin,
	SUM(CAST(ISNULL(EtcNyuKinRui, 0) as bigint)) AS EtcNyuKinRui,
	SUM(CAST((ISNULL(EtcSeiKin, 0) +ISNULL(EtcZenZan, 0) - ISNULL(EtcNyuKinRui, 0)) as bigint)) AS EtcMisyuKin,
	SUM(CAST(ISNULL(EtcMaeuke, 0) as bigint)) AS EtcMaeuke,
-- [メイン]キャンセル料
	SUM(CAST(ISNULL(CanZenZan, 0) as bigint)) AS CanZenZan,
	SUM(CAST(ISNULL(CanCanGakKin, 0) as bigint)) AS CanCanGakKin,
	SUM(CAST(ISNULL(CanSyaRyoSyo, 0) as bigint)) AS CanSyaRyoSyo,
	SUM(CAST(ISNULL(CanSyaRyoTes, 0) as bigint)) AS CanSyaRyoTes,
	SUM(CAST(ISNULL(CanSeiKin, 0) as bigint))AS CanSeiKin,
	SUM(CAST(ISNULL(CanNyuKinRui, 0) as bigint)) AS CanNyuKinRui,
	SUM(CAST((ISNULL(CanSeiKin, 0) +ISNULL(CanZenZan, 0) - ISNULL(CanNyuKinRui, 0)) as bigint)) AS CanMisyuKin,
	SUM(CAST(ISNULL(CanMaeuke, 0) as bigint)) AS CanMaeuke,
	ISNULL(eVPM_TokiSt01.TesKbn, 0) AS UriTesKbn,
	ISNULL(eVPM_TokiSt01.TesKbnFut, 0) AS GaiTesKbn,
	ISNULL(eVPM_TokiSt01.TesKbnGui, 0) AS EtcTesKbn,
	ISNULL(eVPM_CodeKb01.CodeKbnNm, 0) AS UriTesKbnNm,
	ISNULL(eVPM_CodeKb02.CodeKbnNm, 0) AS GaiTesKbnNm,
	ISNULL(eVPM_CodeKb03.CodeKbnNm, 0) AS EtcTesKbnNm
FROM (
	SELECT ISNULL(eTKD_Mishum01.EigyoCdSeq, 0) AS EigyoCdSeq,
		ISNULL(eTKD_Mishum01.TokuiSeq, 0) AS SeiCdSeq,
		ISNULL(eTKD_Mishum01.SitenCdSeq, 0) AS SeiSitenCdSeq,
-- 運賃
		ISNULL(eTKD_Mishum01.UriUriGakKin, 0) AS UriUriGakKin,
		ISNULL(eTKD_Mishum01.UriSyaRyoSyo, 0) AS UriSyaRyoSyo,
		ISNULL(eTKD_Mishum01.UriSyaRyoTes, 0) AS UriSyaRyoTes,
		ISNULL(eTKD_Mishum01.UriSeiKin, 0) AS UriSeiKin,
		0 AS UriNyuKinRui,
		0 AS UriZenZan,
		0 AS UriMaeuke,
-- ガイド料
		ISNULL(eTKD_Mishum01.GaiGaiGakKin, 0) AS GaiGaiGakKin,
		ISNULL(eTKD_Mishum01.GaiSyaRyoSyo, 0) AS GaiSyaRyoSyo,
		ISNULL(eTKD_Mishum01.GaiSyaRyoTes, 0) AS GaiSyaRyoTes,
		ISNULL(eTKD_Mishum01.GaiSeiKin, 0) AS GaiSeiKin,
		0 AS GaiNyuKinRui,
		0 AS GaiZenZan,
		0 AS GaiMaeuke,
-- 付帯
		ISNULL(eTKD_Mishum01.EtcEtcGakKin, 0) AS EtcEtcGakKin,
		ISNULL(eTKD_Mishum01.EtcSyaRyoSyo, 0) AS EtcSyaRyoSyo,
		ISNULL(eTKD_Mishum01.EtcSyaRyoTes, 0) AS EtcSyaRyoTes,
		ISNULL(eTKD_Mishum01.EtcSeiKin, 0) AS EtcSeiKin,
		0 AS EtcNyuKinRui,
		0 AS EtcZenZan,
		0 AS EtcMaeuke,
-- キャンセル料
		ISNULL(eTKD_Mishum01.CanCanGakKin, 0) AS CanCanGakKin,
		ISNULL(eTKD_Mishum01.CanSyaRyoSyo, 0) AS CanSyaRyoSyo,
		ISNULL(eTKD_Mishum01.CanSyaRyoTes, 0) AS CanSyaRyoTes,
		ISNULL(eTKD_Mishum01.CanSeiKin, 0) AS CanSeiKin,
		0 AS CanNyuKinRui,
		0 AS CanZenZan,
		0 AS CanMaeuke
	FROM eTKD_Mishum01
	UNION ALL SELECT ISNULL(eTKD_NyShmi06.EigyoCdSeq, 0) AS EigyoCdSeq,
		ISNULL(eTKD_NyShmi06.TokuiSeq, 0) AS SeiCdSeq,
		ISNULL(eTKD_NyShmi06.SitenCdSeq, 0) AS SeiSitenCdSeq,
-- 運賃
		0 AS UriUriGakKin,
		0 AS UriSyaRyoSyo,
		0 AS UriSyaRyoTes,
		0 AS UriSeiKin,
		ISNULL(eTKD_NyShmi06.UriNyuKinG, 0) AS UriNyuKinRui,
		0 AS UriZenZan,
		ISNULL(eTKD_NyShmi06.UriMaeUke, 0) AS UriMaeuke,
-- ガイド料
		0 AS GaiGaiGakKin,
		0 AS GaiSyaRyoSyo,
		0 AS GaiSyaRyoTes,
		0 AS GaiSeiKin,
		ISNULL(eTKD_NyShmi06.GaiNyukinG, 0) AS GaiNyuKinRui,
		0 AS GaiZenZan,
		ISNULL(eTKD_NyShmi06.GaiMaeuke, 0) AS GaiMaeuke,
-- 付帯
		0 AS EtcEtcGakKin,
		0 AS ETCSyaRyoSyo,
		0 AS ETCSyaRyoTes,
		0 AS ETCSeiKin,
		ISNULL(eTKD_NyShmi06.EtcNyukinG, 0) AS ETCNyuKinRui,
		0 AS ETCZenZan,
		ISNULL(eTKD_NyShmi06.ETCMaeuke, 0) AS ETCMaeuke,
-- キャンセル料
		0 AS CanCanGakKin,
		0 AS CanSyaRyoSyo,
		0 AS CanSyaRyoTes,
		0 AS CanSeiKin,
		ISNULL(eTKD_NyShmi06.CanNyukinG, 0) AS CanNyuKinRui,
		0 AS CanZenZan,
		ISNULL(eTKD_NyShmi06.CanMaeuke, 0) AS CanMaeuke
	FROM eTKD_NyShmi06
	UNION ALL SELECT ISNULL(eTKD_Kuri01.SyoEigyoSeq, 0) AS EigyoCdSeq,
		ISNULL(eTKD_Kuri01.SyoTokuiSeq, 0) AS SeiCdSeq,
		ISNULL(eTKD_Kuri01.SyoSitenSeq, 0) AS SeiSitenCdSeq,
-- 運賃
		0 AS UriUriGakKin,
		0 AS UriSyaRyoSyo,
		0 AS UriSyaRyoTes,
		0 AS UriSeiKin,
		0 AS UriNyuKinRui,
		ISNULL(eTKD_Kuri01.UriZenZan, 0) AS UriZenZan,
		0 AS UriMaeuke,
-- ガイド料
		0 AS GaiGaiGakKin,
		0 AS GaiSyaRyoSyo,
		0 AS GaiSyaRyoTes,
		0 AS GaiSeiKin,
		0 AS GaiNyuKinRui,
		ISNULL(eTKD_Kuri01.GaiZenZan, 0) AS GaiZenZan,
		0 AS GaiMaeuke,
-- 付帯
		0 AS EtcEtcGakKin,
		0 AS ETCSyaRyoSyo,
		0 AS ETCSyaRyoTes,
		0 AS ETCSeiKin,
		0 AS ETCNyuKinRui,
		ISNULL(eTKD_Kuri01.EtcZenZan, 0) AS ETCZenZan,
		0 AS ETCMaeuke,
-- キャンセル料
		0 AS CanCanGakKin,
		0 AS CanSyaRyoSyo,
		0 AS CanSyaRyoTes,
		0 AS CanSeiKin,
		0 AS CanNyuKinRui,
		ISNULL(eTKD_Kuri01.CanZenZan, 0) AS CanZenZan,
		0 AS CanMaeuke
	FROM eTKD_Kuri01
) AS Main
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
	ON Main.EigyoCdSeq = eVPM_Eigyos01.EigyoCdSeq
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
	ON Main.SeiCdSeq = eVPM_TokiSt01.TokuiSeq
	AND Main.SeiSitenCdSeq = eVPM_TokiSt01.SitenCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
	ON eVPM_TokiSt01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
	AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01
	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
	ON eVPM_CodeKb01.CodeSyu = 'TESKBN'
	AND eVPM_TokiSt01.TesKbn = eVPM_CodeKb01.CodeKbn
	AND eVPM_CodeKb01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02
	ON eVPM_CodeKb02.CodeSyu = 'TESKBNFUT'
	AND eVPM_TokiSt01.TesKbnFut = eVPM_CodeKb02.CodeKbn
	AND eVPM_CodeKb02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb03
	ON eVPM_CodeKb03.CodeSyu = 'TESKBNGUI'
	AND eVPM_TokiSt01.TesKbnGui = eVPM_CodeKb03.CodeKbn
	AND eVPM_CodeKb03.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
GROUP BY ISNULL(eVPM_Eigyos01.EigyoCd, 0),
	ISNULL(eVPM_Eigyos01.EigyoNm, ''),
	ISNULL(eVPM_Eigyos01.RyakuNm, ''),
	ISNULL(eVPM_Gyosya01.GyosyaCd, 0),
	ISNULL(eVPM_Tokisk01.TokuiCd, 0),
	ISNULL(eVPM_Tokisk01.TokuiSeq, 0),
	ISNULL(eVPM_Tokisk01.TokuiNm, ''),
	ISNULL(eVPM_Tokisk01.RyakuNm, ''),
	ISNULL(eVPM_TokiSt01.SitenCd, 0),
	ISNULL(eVPM_TokiSt01.SitenCdSeq, 0),
	ISNULL(eVPM_TokiSt01.SitenNm, ''),
	ISNULL(eVPM_TokiSt01.RyakuNm, ''),
	ISNULL(eVPM_Gyosya01.GyosyaNm, ''),
	ISNULL(eVPM_TokiSt01.TesKbn, 0),
	ISNULL(eVPM_TokiSt01.TesKbnFut, 0),
	ISNULL(eVPM_TokiSt01.TesKbnGui, 0),
	ISNULL(eVPM_CodeKb01.CodeKbnNm, 0),
	ISNULL(eVPM_CodeKb02.CodeKbnNm, 0),
	ISNULL(eVPM_CodeKb03.CodeKbnNm, 0)
ORDER BY EigyoCd,
	SeiGyosyaCd,
	SeiCd,
	SeiSitenCd

OFFSET @Offset ROWS
FETCH NEXT @Limit ROWS ONLY
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN