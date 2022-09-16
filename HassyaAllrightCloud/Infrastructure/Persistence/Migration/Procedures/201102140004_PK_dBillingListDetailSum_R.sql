USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBillingListDetailSum_R]    Script Date: 1/20/2021 1:34:26 PM ******/
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
-- Description	:   Get billing list detail data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dBillingListDetailSum_R]
		(
		--Parameter
			@BillDate				NVARCHAR(6),
			@CloseDate				TINYINT,
			@TenantCdSeq			INT,				
			@StartBillAddress			NVARCHAR(11),				
			@EndBillAddress			NVARCHAR(11),				
			@BillOffice			INT,				
			@StartReservationClassification			TINYINT,				
			@EndReservationClassification			TINYINT,	
			@StartReceiptNumber NVARCHAR(15),                -- 予約番号　開始
			@EndReceiptNumber	NVARCHAR(15),                  -- 予約番号　終了	
			@StartBillClassification BIGINT,       -- 請求区分 開始
			@EndBillClassification BIGINT,         -- 請求区分 終了
			@BillTypes			NVARCHAR(100),
			@BillIssuedClassification TINYINT,
			@BillTypeOrder NVARCHAR(100),
			@GyosyaCd			SMALLINT,
			@TokuiCd			SMALLINT,
			@SitenCd			SMALLINT,
			@TokuiNm			NVARCHAR(1000),
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
-- 運行日テーブル 受付番号毎の最小の運行日連番
WITH eTKD_Unkobi01 AS (
	SELECT TKD_Unkobi.UkeNo,
		TKD_Unkobi.UnkRen,
		TKD_Unkobi.HaiSYmd,
		TKD_Unkobi.TouYmd,
		TKD_Unkobi.IkNm,
		ROW_NUMBER() OVER (PARTITION BY TKD_Unkobi.UkeNo ORDER BY TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen) ROW_NUMBER
   FROM TKD_Unkobi
   WHERE TKD_Unkobi.SiyoKbn = 1
),
-- 運行日テーブル 受付番号毎の最小の運行日連番のレコード
eTKD_Unkobi02 AS (
	SELECT eTKD_Unkobi01.UkeNo,
		eTKD_Unkobi01.UnkRen,
		eTKD_Unkobi01.HaiSYmd,
		eTKD_Unkobi01.TouYmd,
		eTKD_Unkobi01.IkNm
   FROM eTKD_Unkobi01
   WHERE eTKD_Unkobi01.ROW_NUMBER = 1 
),
-- 入金支払明細テーブル
eTKD_NyShmi01 AS (
	SELECT TKD_NyShmi.UkeNo AS UkeNo,
		TKD_NyShmi.UnkRen AS UnkRen,
		TKD_NyShmi.FutTumRen AS FutTumRen,
		TKD_NyShmi.SeiFutSyu AS SeiFutSyu,
		MAX(eTKD_NyuSih01.NyuSihYmd) AS NyuSihYmd
	FROM TKD_NyShmi
	INNER JOIN TKD_NyuSih AS eTKD_NyuSih01
		ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND TKD_NyShmi.NyuSihKbn = 1
		AND TKD_NyShmi.SiyoKbn = 1
		AND eTKD_NyuSih01.SiyoKbn = 1
   GROUP BY TKD_NyShmi.UkeNo,
	TKD_NyShmi.UnkRen,
	TKD_NyShmi.FutTumRen,
	TKD_NyShmi.SeiFutSyu
),
-- 請求明細テーブル 受付番号、未収明細連番毎の行番号採番
eTKD_SeiMei01 AS (
	SELECT TKD_SeiMei.SeiOutSeq AS SeiOutSeq,
		TKD_SeiMei.SeiRen AS SeiRen,
		TKD_SeiMei.SeiMeiRen AS SeiMeiRen,
		TKD_SeiMei.UkeNo AS UkeNo,
		TKD_SeiMei.MisyuRen AS MisyuRen,
		TKD_SeiPrS.SeiHatYmd AS SeiHatYmd,
		ROW_NUMBER() OVER (PARTITION BY TKD_SeiMei.UkeNo, TKD_SeiMei.MisyuRen ORDER BY TKD_SeiMei.UkeNo, TKD_SeiMei.MisyuRen, TKD_SeiPrS.SeiHatYmd DESC) AS ROW_NUMBER
	FROM TKD_SeiMei
	INNER JOIN TKD_SeiPrS
		ON TKD_SeiMei.SeiOutSeq = TKD_SeiPrS.SeiOutSeq
		AND TKD_SeiPrS.SiyoKbn = 1
		AND TKD_SeiMei.SiyoKbn = 1
),
-- 請求明細テーブル 受付番号、未収明細連番毎の最大発行年月日
eTKD_SeiMei02 AS (
	SELECT DISTINCT eTKD_SeiMei01.UkeNo,
		eTKD_SeiMei01.MisyuRen,
		eTKD_SeiMei01.SeiHatYmd
   FROM eTKD_SeiMei01
   WHERE eTKD_SeiMei01.ROW_NUMBER = 1
)

SELECT
TKD_Mishum.SeiFutSyu as SeiFutSyu,
SUM(CAST(TKD_Mishum.SeiKin as bigint)) BillAmount,
SUM(CAST(TKD_Mishum.NyuKinRui as bigint)) DepositAmount,
SUM(CAST(TKD_Mishum.UriGakKin as bigint)) SalesAmount,
SUM(CAST((TKD_Mishum.SeiKin - TKD_Mishum.NyuKinRui) as bigint)) AS UnpaidAmount,
SUM(CAST(TKD_Mishum.SyaRyoSyo as bigint)) TaxAmount,
SUM(CAST(TKD_Mishum.SyaRyoTes as bigint)) CommissionAmount
-- 未収明細テーブル
FROM TKD_Mishum
-- 予約書テーブル
INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
	ON TKD_Mishum.UkeNo = eTKD_Yyksho01.UkeNo
	AND TKD_Mishum.SiyoKbn = 1
	AND eTKD_Yyksho01.SiyoKbn = 1
	AND ((TKD_Mishum.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (TKD_Mishum.SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))
-- 予約区分マスタ
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
	ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
-- コード区分マスタ 請求区分
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb04
	ON eVPM_CodeKb04.CodeSyu = 'SEIKYUKBN'
	AND eTKD_Yyksho01.SeiKyuKbnSeq = eVPM_CodeKb04.CodeKbnSeq
	AND eVPM_CodeKb04.TenantCdSeq = @TenantCdSeq
-- 請求営業所
INNER JOIN VPM_Eigyos AS eVPM_Eigyos01
	ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
-- 受付営業所
INNER JOIN VPM_Eigyos AS eVPM_Eigyos02
	ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos02.EigyoCdSeq
-- 得意先支店
INNER JOIN VPM_TokiSt AS eVPM_TokiSt01
	ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
	AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
-- 請求先
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
-- 請求先支店
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
-- 請求先業者 
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
-- 運行日テーブル 受付番号毎の最小の運行日連番のレコード
LEFT JOIN eTKD_Unkobi02 AS eTKD_Unkobi11
	ON TKD_Mishum.UkeNo = eTKD_Unkobi11.UkeNo
-- 運行日テーブル 受付番号・運行日連番
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi12
	ON TKD_Mishum.UkeNo = eTKD_Unkobi12.UkeNo
	AND TKD_Mishum.FutuUnkRen = eTKD_Unkobi12.UnkRen
	AND eTKD_Unkobi12.SiyoKbn = 1
-- 付帯積込品テーブル
LEFT JOIN TKD_FutTum AS eTKD_FutTum11 
	ON TKD_Mishum.UkeNo = eTKD_FutTum11.UkeNo
	AND FutuUnkRen = eTKD_FutTum11.UnkRen
	AND TKD_Mishum.FutTumRen = eTKD_FutTum11.FutTumRen
	AND eTKD_FutTum11.SiyoKbn = 1
	AND ((TKD_Mishum.SeiFutSyu = 6 AND eTKD_FutTum11.FutTumKbn = 2) OR (TKD_Mishum.SeiFutSyu <> 6 AND eTKD_FutTum11.FutTumKbn = 1))
-- 清算マスター
LEFT JOIN VPM_Seisan AS eVPM_Seisan01
    ON eTKD_FutTum11.SeisanCdSeq = eVPM_Seisan01.SeisanCdSeq
-- コード区分マスタ 請求付帯種別名
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
	ON eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'
	AND TKD_Mishum.SeiFutSyu = eVPM_CodeKb01.CodeKbn
	AND eVPM_CodeKb01.TenantCdSeq = @TenantCdSeq
-- 入金支払明細 受付番号・運行日連番・付帯積込品連番集計
LEFT JOIN eTKD_NyShmi01 AS eTKD_NyShmi11
	ON TKD_Mishum.UkeNo = eTKD_NyShmi11.UkeNo
	AND TKD_Mishum.FutuUnkRen = eTKD_NyShmi11.UnkRen
	AND TKD_Mishum.FutTumRen = eTKD_NyShmi11.FutTumRen
	AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi11.SeiFutSyu
-- 請求明細
LEFT JOIN eTKD_SeiMei02 AS eTKD_SeiMei11
	ON TKD_Mishum.UkeNo = eTKD_SeiMei11.UkeNo
	AND TKD_Mishum.MisyuRen = eTKD_SeiMei11.MisyuRen
-- 予約車種 台数
LEFT JOIN (
	SELECT TKD_YykSyu.UkeNo,
		SUM(SyaSyuDai) AS Sum_SyaSyuDai,
		SUM(SyaSyuTan) AS Sum_SyaSyuTan
	FROM TKD_YykSyu
	INNER JOIN (
		SELECT UkeNo,
			Min(UnkRen) AS Min_UnkRen
		FROM TKD_YykSyu
		WHERE SiyoKbn = 1
		GROUP BY UkeNo) AS SUB 
		ON SUB.UkeNo = TKD_YykSyu.UkeNo
		AND SUB.Min_UnkRen = TKD_YykSyu.UnkRen
   WHERE SiyoKbn = 1
   GROUP BY TKD_YykSyu.UkeNo) AS eTKD_YykSyu 
   ON TKD_Mishum.UkeNo = eTKD_YykSyu.UkeNo
WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
AND (@BillDate = '' OR eTKD_Yyksho01.SeikYm = @BillDate)
AND (@CloseDate = 0 OR eVPM_TokiSt02.SimeD = @CloseDate)
AND (@BillOffice = 0 OR eVPM_Eigyos01.EigyoCd = @BillOffice)
AND (TRIM(@StartBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
AND (TRIM(@EndBillAddress) = '' OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
AND (@StartReceiptNumber = '' OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber)
AND (@EndReceiptNumber = '' OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber)
AND (@StartReservationClassification = 0 OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)
AND (@EndReservationClassification = 0 OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)
AND (@StartBillClassification = 0 OR CAST(eVPM_CodeKb04.CodeKbn as bigint) >= @StartBillClassification)
AND (@EndBillClassification = 0 OR CAST(eVPM_CodeKb04.CodeKbn as bigint) <= @EndBillClassification)	
AND ((@BillIssuedClassification = 0) OR (@BillIssuedClassification = 1 AND eTKD_SeiMei11.SeiHatYmd <> '') OR (@BillIssuedClassification = 2 AND eTKD_SeiMei11.SeiHatYmd = ''))
AND (@BillTypes = '' OR TKD_Mishum.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))
AND ((@GyosyaCd = 0 AND @TokuiCd = 0 AND @SitenCd = 0 AND TRIM(@TokuiNm) = '') 
OR ((ISNULL(eVPM_Gyosya02.GyosyaCd, 0) = @GyosyaCd AND ISNULL(eVPM_Tokisk02.TokuiCd, 0) = @TokuiCd 
AND ISNULL(eVPM_TokiSt02.SitenCd, 0) = @SitenCd AND ISNULL(eVPM_Tokisk02.TokuiNm, '') = @TokuiNm)))
GROUP BY TKD_Mishum.SeiFutSyu
order by TKD_Mishum.SeiFutSyu
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN
