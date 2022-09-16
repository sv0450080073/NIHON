USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetDepositCouponCodesAsync
-- Date			:   2020/09/28
-- Author		:   T.L.DUY
-- Description	:   Get deposit coupon code data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dDepositCouponCode_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,				
		@StartBillPeriod		NVARCHAR(8),				
		@EndBillPeriod			NVARCHAR(8),				
		@StartBillAddress			NVARCHAR(11),				
		@EndBillAddress			NVARCHAR(11),				
		@BillOffice			INT,				
		@StartReservationClassification			TINYINT,				
		@EndReservationClassification			TINYINT,				
		@BillTypes			NVARCHAR(100),				
		@DepositOutputClassification			NVARCHAR(100),
		@GyosyaCd			SMALLINT,
		@TokuiCd			SMALLINT,
		@SitenCd			SMALLINT,
		@TokuiNm			NVARCHAR(1000),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
;WITH eTKD_Unkobi001 AS (
	SELECT eTKD_Unkobi01.UkeNo AS UkeNo,
		eTKD_Unkobi01.UnkRen AS UnkRen,
		eTKD_Unkobi01.HaiSYmd AS HaiSYmd,
		eTKD_Unkobi01.HaiSTime AS HaiSTime,
		eTKD_Unkobi01.TouYmd AS TouYmd,
		eTKD_Unkobi01.TouChTime AS TouChTime,
		eTKD_Unkobi01.IkNm AS IkNm,
		eTKD_Unkobi01.YouKbn AS YouKbn
	FROM (
		SELECT eTKD_Unkobi01.UkeNo AS UkeNo,
			eTKD_Unkobi01.UnkRen AS UnkRen,
			eTKD_Unkobi01.HaiSYmd AS HaiSYmd,
			eTKD_Unkobi01.HaiSTime AS HaiSTime,
			eTKD_Unkobi01.TouYmd AS TouYmd,
			eTKD_Unkobi01.TouChTime AS TouChTime,
			eTKD_Unkobi01.IkNm AS IkNm,
			eTKD_Unkobi01.YouKbn AS YouKbn,
			ROW_NUMBER() OVER (PARTITION BY eTKD_Unkobi01.UkeNo ORDER BY eTKD_Unkobi01.UkeNo, eTKD_Unkobi01.UnkRen) AS ROW_NUMBER
		FROM TKD_Unkobi AS eTKD_Unkobi01
		WHERE eTKD_Unkobi01.SiyoKbn = 1) AS eTKD_Unkobi01
	WHERE eTKD_Unkobi01.ROW_NUMBER = 1
),
eTKD_NyShmi002 AS (
	SELECT eTKD_NyShmi01.UkeNo AS UkeNo,
		eTKD_NyShmi01.SeiFutSyu AS SeiFutSyu,
		eTKD_NyShmi01.UnkRen AS UnkRen,
		eTKD_NyShmi01.FutTumRen AS FutTumRen,
		MAX( eTKD_NyuSih02.NyuSihYmd ) AS LastNyuYmd
	FROM TKD_NyShmi AS eTKD_NyShmi01
	INNER JOIN TKD_NyuSih AS eTKD_NyuSih02
		ON eTKD_NyShmi01.NyuSihTblSeq = eTKD_NyuSih02.NyuSihTblSeq
		AND eTKD_NyShmi01.NyuSihKbn = 1
		AND eTKD_NyShmi01.SiyoKbn = 1
		AND eTKD_NyuSih02.SiyoKbn = 1
	GROUP BY eTKD_NyShmi01.UkeNo,
		eTKD_NyShmi01.SeiFutSyu,
		eTKD_NyShmi01.UnkRen,
		eTKD_NyShmi01.FutTumRen
),
eTKD_NyShCu003 AS (
	SELECT eTKD_NyShCu01.UkeNo AS UkeNo,
		eTKD_NyShCu01.SeiFutSyu AS SeiFutSyu,
		eTKD_NyShCu01.UnkRen AS UnkRen,
		eTKD_NyShCu01.FutTumRen AS FutTumRen,
		MAX( eTKD_Coupon02.CouNo ) AS LastCouNo
	FROM TKD_NyShCu AS eTKD_NyShCu01
	INNER JOIN TKD_Coupon AS eTKD_Coupon02
		ON eTKD_NyShCu01.CouTblSeq = eTKD_Coupon02.CouTblSeq
		AND eTKD_NyShCu01.NyuSihKbn = 1
		AND eTKD_NyShCu01.SiyoKbn = 1
		AND eTKD_Coupon02.SiyoKbn = 1
	GROUP BY eTKD_NyShCu01.UkeNo,
		eTKD_NyShCu01.SeiFutSyu,
		eTKD_NyShCu01.UnkRen,
		eTKD_NyShCu01.FutTumRen
)

SELECT DISTINCT CONCAT(FORMAT(ISNULL(eVPM_Gyosya02.GyosyaCd,0), '000'),'－',FORMAT(ISNULL(eVPM_Tokisk02.TokuiCd, 0), '0000'),'－',FORMAT(ISNULL(eVPM_TokiSt02.SitenCd, 0), '0000'),' : ',ISNULL(eVPM_Tokisk02.TokuiNm, '')) AS Code
-- <予約書データ>
FROM TKD_Yyksho AS eTKD_Yyksho01
-- <未収明細データ>
INNER JOIN TKD_Mishum AS eTKD_Mishum02
	ON eTKD_Mishum02.UkeNo = eTKD_Yyksho01.UkeNo
	AND eTKD_Mishum02.SiyoKbn = 1
	AND eTKD_Yyksho01.SiyoKbn = 1
	AND ((eTKD_Mishum02.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (eTKD_Mishum02.SeiFutSyu =  7 AND eTKD_Yyksho01.YoyaSyu = 2))
-- <請求先営業所データ>
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
	ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
-- <得意先データ>
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
	ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
	AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- <得意先業者データ>
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01
	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
-- <得意先支店データ>
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
	ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
	AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
-- <請求先データ>
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- <請求先業者データ>
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
-- <請求先支店データ>
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
-- <受付営業所データ>
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos02
	ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos02.EigyoCdSeq
-- <運行日データ（請求付帯種別＝運賃／キャンセル料）>
LEFT JOIN eTKD_Unkobi001 AS eTKD_Unkobi01
	ON eTKD_Unkobi01.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_Mishum02.SeiFutSyu IN (1, 7)
-- <運行日データ（請求付帯種別≠運賃／キャンセル料）>
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi02
	ON eTKD_Unkobi02.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_Unkobi02.UnkRen = eTKD_Mishum02.FutuUnkRen
	AND eTKD_Unkobi02.SiyoKbn = 1
	AND eTKD_Mishum02.SeiFutSyu NOT IN (1, 7)
-- <付帯積込品データ（請求付帯種別≠運賃／キャンセル料）>
LEFT JOIN TKD_FutTum AS eTKD_FutTum11
	ON eTKD_Mishum02.UkeNo = eTKD_FutTum11.UkeNo
	AND eTKD_Mishum02.FutuUnkRen = eTKD_FutTum11.UnkRen
	AND eTKD_Mishum02.FutTumRen = eTKD_FutTum11.FutTumRen
	AND eTKD_FutTum11.SiyoKbn = 1
	AND ((eTKD_Mishum02.SeiFutSyu = 6 AND eTKD_FutTum11.FutTumKbn = 2) OR (eTKD_Mishum02.SeiFutSyu <> 6 AND eTKD_FutTum11.FutTumKbn = 1))
-- <請求付帯種別データ>
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
	ON eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'
	AND eTKD_Mishum02.SeiFutSyu = eVPM_CodeKb01.CodeKbn
	AND eVPM_CodeKb01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- <税区分データ>
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02
	ON eVPM_CodeKb02.CodeSyu = 'ZEIKBN'
	AND ((eTKD_Mishum02.SeiFutSyu IN (1,7) AND eTKD_Yyksho01.ZeiKbn = eVPM_CodeKb02.CodeKbn ) OR (eTKD_Mishum02.SeiFutSyu NOT IN (1,7) AND eTKD_FutTum11.ZeiKbn = eVPM_CodeKb02.CodeKbn))
	AND eVPM_CodeKb02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- <予約区分データ>
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
	ON eVPM_YoyKbn01.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
-- <最終入金年月日データ>
LEFT JOIN eTKD_NyShmi002 AS eTKD_NyShmi01
	ON eTKD_NyShmi01.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_NyShmi01.SeiFutSyu = eTKD_Mishum02.SeiFutSyu
	AND eTKD_NyShmi01.UnkRen = eTKD_Mishum02.FutuUnkRen
	AND eTKD_NyShmi01.FutTumRen = eTKD_Mishum02.FutTumRen
-- <最終クーポンNo.データ>
LEFT JOIN eTKD_NyShCu003 AS eTKD_NyShCu01
	ON eTKD_NyShCu01.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_NyShCu01.SeiFutSyu = eTKD_Mishum02.SeiFutSyu
	AND eTKD_NyShCu01.UnkRen = eTKD_Mishum02.FutuUnkRen
	AND eTKD_NyShCu01.FutTumRen = eTKD_Mishum02.FutTumRen
WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	AND ((TRIM(@StartBillPeriod) = '') OR (eTKD_Yyksho01.SeiTaiYmd >= @StartBillPeriod))
	AND ((TRIM(@EndBillPeriod) = '') OR (eTKD_Yyksho01.SeiTaiYmd <= @EndBillPeriod))
	AND ((TRIM(@StartBillAddress) = '') OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') >= @StartBillAddress))
	AND ((TRIM(@EndBillAddress) = '') OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') <= @EndBillAddress))
	AND eVPM_Eigyos01.EigyoCdSeq = CASE @BillOffice WHEN 0 THEN eVPM_Eigyos01.EigyoCdSeq ELSE @BillOffice END
	AND ((@StartReservationClassification = 0) OR (eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification))
	AND ((@EndReservationClassification = 0) OR (eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification))
	AND ((TRIM(@DepositOutputClassification) <> '未収のみ') OR (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui <> 0))
	AND ((TRIM(@DepositOutputClassification) <> '入金のみ') OR (eTKD_Mishum02.NyuKinRui > 0))
	AND ((TRIM(@DepositOutputClassification) <> 'クーポン未入力の未収のみ') OR (RTRIM(LTRIM(ISNULL(eTKD_NyShCu01.LastCouNo, ''))) = '' AND (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui) > 0))
	AND ((TRIM(@BillTypes) = '') OR (eTKD_Mishum02.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ','))))
	AND ((@GyosyaCd = 0 OR @TokuiCd = 0 OR @SitenCd = 0 OR TRIM(@TokuiNm) = '') OR (ISNULL(eVPM_Gyosya02.GyosyaCd, 0) = @GyosyaCd AND ISNULL(eVPM_Tokisk02.TokuiCd, 0) = @TokuiCd AND ISNULL(eVPM_TokiSt02.SitenCd, 0) = @SitenCd AND ISNULL(eVPM_Tokisk02.TokuiNm, '') = @TokuiNm))
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN