USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dDepositCouponTotal_R]    Script Date: 9/30/2020 11:38:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetDepositCouponsAsync
-- Date			:   2020/08/10
-- Author		:   T.L.DUY
-- Description	:   Get deposit coupon data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dDepositCouponTotal_R] 
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
print @BillOffice
print @TenantCdSeq
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

SELECT DISTINCT COUNT(*) OVER(ORDER BY (SELECT NULL)) AS CountNumber,
SUM(CAST(eTKD_Mishum02.UriGakKin as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalSaleAmount,
SUM(CAST(eTKD_Mishum02.SyaRyoSyo as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalTaxAmount,
SUM(CAST((eTKD_Mishum02.UriGakKin + eTKD_Mishum02.SyaRyoSyo) as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalTaxIncluded,
SUM(CAST(eTKD_Mishum02.SyaRyoTes as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalCommissionAmount,
SUM(CAST(eTKD_Mishum02.SeiKin as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalBillAmount,
SUM(CAST(eTKD_Mishum02.NyuKinRui as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalCumulativeDeposit,
SUM(CAST((eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui) as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalUnpaidAmount
-- <??????????????????>
FROM TKD_Yyksho AS eTKD_Yyksho01
-- <?????????????????????>
INNER JOIN TKD_Mishum AS eTKD_Mishum02
	ON eTKD_Mishum02.UkeNo = eTKD_Yyksho01.UkeNo
	AND eTKD_Mishum02.SiyoKbn = 1
	AND eTKD_Yyksho01.SiyoKbn = 1
	AND ((eTKD_Mishum02.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (eTKD_Mishum02.SeiFutSyu =  7 AND eTKD_Yyksho01.YoyaSyu = 2))
-- <???????????????????????????>
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
	ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
-- <??????????????????>
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
	ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
	AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
-- <????????????????????????>
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01
	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
-- <????????????????????????>
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
	ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
	AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
-- <??????????????????>
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
-- <????????????????????????>
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
-- <????????????????????????>
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
-- <????????????????????????>
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos02
	ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos02.EigyoCdSeq
-- <????????????????????????????????????????????????????????????????????????>
LEFT JOIN eTKD_Unkobi001 AS eTKD_Unkobi01
	ON eTKD_Unkobi01.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_Mishum02.SeiFutSyu IN (1, 7)
-- <????????????????????????????????????????????????????????????????????????>
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi02
	ON eTKD_Unkobi02.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_Unkobi02.UnkRen = eTKD_Mishum02.FutuUnkRen
	AND eTKD_Unkobi02.SiyoKbn = 1
	AND eTKD_Mishum02.SeiFutSyu NOT IN (1, 7)
-- <??????????????????????????????????????????????????????????????????????????????>
LEFT JOIN TKD_FutTum AS eTKD_FutTum11
	ON eTKD_Mishum02.UkeNo = eTKD_FutTum11.UkeNo
	AND eTKD_Mishum02.FutuUnkRen = eTKD_FutTum11.UnkRen
	AND eTKD_Mishum02.FutTumRen = eTKD_FutTum11.FutTumRen
	AND eTKD_FutTum11.SiyoKbn = 1
	AND ((eTKD_Mishum02.SeiFutSyu = 6 AND eTKD_FutTum11.FutTumKbn = 2) OR (eTKD_Mishum02.SeiFutSyu <> 6 AND eTKD_FutTum11.FutTumKbn = 1))
-- <???????????????????????????>
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb011
	ON eVPM_CodeKb011.CodeSyu = 'SEIFUTSYU'
	AND eTKD_Mishum02.SeiFutSyu = eVPM_CodeKb011.CodeKbn
	AND eVPM_CodeKb011.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
 LEFT JOIN VPM_CodeKb AS eVPM_CodeKb012
	ON eVPM_CodeKb012.CodeSyu = 'SEIFUTSYU'
	AND eTKD_Mishum02.SeiFutSyu = eVPM_CodeKb012.CodeKbn
	AND eVPM_CodeKb012.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
-- <??????????????????>
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02
	ON eVPM_CodeKb02.CodeSyu = 'ZEIKBN'
	AND ((eTKD_Mishum02.SeiFutSyu IN (1,7) AND eTKD_Yyksho01.ZeiKbn = eVPM_CodeKb02.CodeKbn ) OR (eTKD_Mishum02.SeiFutSyu NOT IN (1,7) AND eTKD_FutTum11.ZeiKbn = eVPM_CodeKb02.CodeKbn))
	AND eVPM_CodeKb02.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
-- <?????????????????????>
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
	ON eVPM_YoyKbn01.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
-- <??????????????????????????????>
LEFT JOIN eTKD_NyShmi002 AS eTKD_NyShmi01
	ON eTKD_NyShmi01.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_NyShmi01.SeiFutSyu = eTKD_Mishum02.SeiFutSyu
	AND eTKD_NyShmi01.UnkRen = eTKD_Mishum02.FutuUnkRen
	AND eTKD_NyShmi01.FutTumRen = eTKD_Mishum02.FutTumRen
-- <??????????????????No.?????????>
LEFT JOIN eTKD_NyShCu003 AS eTKD_NyShCu01
	ON eTKD_NyShCu01.UkeNo = eTKD_Mishum02.UkeNo
	AND eTKD_NyShCu01.SeiFutSyu = eTKD_Mishum02.SeiFutSyu
	AND eTKD_NyShCu01.UnkRen = eTKD_Mishum02.FutuUnkRen
	AND eTKD_NyShCu01.FutTumRen = eTKD_Mishum02.FutTumRen
WHERE	eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
		AND ((TRIM(@StartBillPeriod) = '') OR (eTKD_Yyksho01.SeiTaiYmd >= @StartBillPeriod))
		AND ((TRIM(@EndBillPeriod) = '') OR (eTKD_Yyksho01.SeiTaiYmd <= @EndBillPeriod))
		AND ((TRIM(@StartBillAddress) = '') OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') >= @StartBillAddress))
		AND ((TRIM(@EndBillAddress) = '') OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') <= @EndBillAddress))
		AND eVPM_Eigyos01.EigyoCdSeq = CASE @BillOffice WHEN 0 THEN eVPM_Eigyos01.EigyoCdSeq ELSE @BillOffice END
		AND ((@StartReservationClassification = 0) OR (eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification))
		AND ((@EndReservationClassification = 0) OR (eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification))
		AND ((TRIM(@DepositOutputClassification) <> '????????????') OR (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui <> 0 ))
		AND ((TRIM(@DepositOutputClassification) <> '????????????') OR (eTKD_Mishum02.NyuKinRui > 0))
		AND ((TRIM(@DepositOutputClassification) <> '????????????????????????????????????') OR (RTRIM(LTRIM(ISNULL(eTKD_NyShCu01.LastCouNo, ''))) = '' AND (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui) > 0))
		AND ((TRIM(@BillTypes) = '') OR (eTKD_Mishum02.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ','))))
		AND ((@GyosyaCd = 0 OR @TokuiCd = 0 OR @SitenCd = 0 OR TRIM(@TokuiNm) = '') OR (ISNULL(eVPM_Gyosya02.GyosyaCd, 0) = @GyosyaCd AND ISNULL(eVPM_Tokisk02.TokuiCd, 0) = @TokuiCd AND ISNULL(eVPM_TokiSt02.SitenCd, 0) = @SitenCd AND ISNULL(eVPM_Tokisk02.TokuiNm, '') = @TokuiNm))

SET	@ROWCOUNT	=	@@ROWCOUNT
END
GO