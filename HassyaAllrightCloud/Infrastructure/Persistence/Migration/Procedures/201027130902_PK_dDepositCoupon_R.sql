USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dDepositCoupon_R]    Script Date: 9/3/2020 9:45:04 AM ******/
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
CREATE OR ALTER PROCEDURE [dbo].[PK_dDepositCoupon_R] 
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
		@Offset				INT,					--Offset rows data
		@Limit				INT,					--Limit rows data
	-- Output
		@ROWCOUNT				INTEGER OUTPUT
	)
AS
BEGIN
SET		@RowCount =	0;
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

SELECT
eTKD_Mishum02.*,
	ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigyoCd,
	eTKD_Yyksho01.SeiEigCdSeq AS EigyoCdSeq,
	ISNULL(eVPM_Eigyos01.EigyoNm, '') AS EigyoNm,
	ISNULL(eVPM_Eigyos01.RyakuNm, '') AS EigyoRyak,
	ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS SeiGyosyaCd,
	ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS SeiCd,
	ISNULL(eVPM_Tokisk02.TokuiNm, '') AS SeiCdNm,
	ISNULL(eVPM_Tokisk02.RyakuNm, '') AS SeiRyakuNm,
	ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SeiSitenCd,
	ISNULL(eVPM_TokiSt02.SitenNm, '') AS SeiSitenCdNm,
	ISNULL(eVPM_TokiSt02.RyakuNm, '') AS SeiSitRyakuNm,
	ISNULL(eVPM_Gyosya02.GyosyaNm, '') AS SeiGyosyaCdNm,
	eTKD_Yyksho01.SeiTaiYmd AS SeiTaiYmd,
	CASE
		WHEN ISNULL(eTKD_Yyksho01.YoyaSyu, 0) = 2 THEN ISNULL(eTKD_Yyksho01.CanYmd , '')
		ELSE ''
	END AS CanYmd,
	ISNULL(eVPM_Eigyos02.EigyoCd, 0) AS UkeEigCd,
	ISNULL(eVPM_Eigyos02.EigyoNm, '') AS UkeEigyoNm,
	ISNULL(eVPM_Eigyos02.RyakuNm, '') AS UkeRyakuNm,
	ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS TokuiCd,
	ISNULL(eVPM_Tokisk01.TokuiNm, '') AS TokuiNm,
	ISNULL(eVPM_Tokisk01.RyakuNm, '') AS TokRyakuNm,
	ISNULL(eVPM_TokiSt01.SitenCd, 0) AS SitenCd,
	ISNULL(eVPM_TokiSt01.SitenNm, '') AS SitenNm,
	ISNULL(eVPM_TokiSt01.RyakuNm, '') AS SitRyakuNm,
	ISNULL(eVPM_Gyosya01.GyosyaCd, 0) AS GyosyaCd,
	ISNULL(eVPM_Gyosya01.GyosyaNm, '') AS GyosyaNm,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Unkobi01.HaiSYmd, '')
		ELSE ISNULL(eTKD_Unkobi02.HaiSYmd, '')
	END AS HaiSYmd,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Unkobi01.HaiSTime, '')
		ELSE ISNULL(eTKD_Unkobi02.HaiSTime, '')
	END AS HaiSTime,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Unkobi01.TouYmd, '')
		ELSE ISNULL(eTKD_Unkobi02.TouYmd, '')
	END AS TouYmd,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Unkobi01.TouChTime, '')
		ELSE ISNULL(eTKD_Unkobi02.TouChTime, '')
	END AS TouChTime,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Unkobi01.IkNm, '')
		ELSE ISNULL(eTKD_Unkobi02.IkNm, '')
	END AS IkNm,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN eTKD_Yyksho01.YoyaNm
		ELSE ISNULL(eTKD_Unkobi02.DanTaNm, '')
	END AS DanTaNm,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Yyksho01.ZeiKbn, '')
		ELSE ISNULL(eTKD_FutTum11.ZeiKbn, 0)
	END AS ZeiKbn,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Yyksho01.Zeiritsu, '')
		ELSE ISNULL(eTKD_FutTum11.Zeiritsu, 0)
	END AS Zeiritsu,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Yyksho01.TesuRitu, '')
		ELSE ISNULL(eTKD_FutTum11.TesuRitu, 0)
	END AS TesuRitu,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ''
		ELSE ISNULL(eTKD_FutTum11.FutTumNm, '')
	END AS FutTumNm,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ''
		ELSE ISNULL(CAST(eTKD_FutTum11.FutTumKbn AS VARCHAR), '')
	END AS FutTumKbn,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ''
		ELSE ISNULL(eTKD_FutTum11.SeisanNm, '')
	END AS SeisanNm,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ''
		ELSE ISNULL(CAST(eTKD_FutTum11.Suryo AS VARCHAR), '')
	END AS Suryo,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN ''
		ELSE ISNULL(CAST(eTKD_FutTum11.TanKa AS VARCHAR), '')
	END AS TanKa,
	ISNULL(eVPM_CodeKb011.CodeKbnNm, eVPM_CodeKb012.CodeKbnNm) AS SeiFutSyuNm,
	ISNULL(eVPM_CodeKb02.CodeKbnNm,  '') AS ZeiKbnNm,
	ISNULL(eVPM_YoyKbn01.YoyaKbn, 0) AS YoyaKbn,
	ISNULL(eVPM_YoyKbn01.YoyaKbnNm, '') AS YoyaKbnNm,
	ISNULL(eTKD_NyShmi01.LastNyuYmd, '') AS LastNyuYmd,
	ISNULL(eTKD_NyShCu01.LastCouNo, '') AS LastCouNo,
	ISNULL(eTKD_Unkobi01.YouKbn, '') AS YouKbn,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN eTKD_Yyksho01.NyuKinKbn
		ELSE ISNULL(eTKD_FutTum11.NyuKinKbn, 0)
	END AS NyuKinKbn,
	CASE
		WHEN eTKD_Mishum02.SeiFutSyu IN (1, 7) THEN eTKD_Yyksho01.NCouKbn
		ELSE ISNULL(eTKD_FutTum11.NCouKbn, 0)
	END AS NCouKbn
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
	AND eVPM_Tokisk01.TenantCdSeq =  @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
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
WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq -- ?????????????????????????????????TenantCdSeq
	 AND (TRIM(@StartBillPeriod) = '' OR eTKD_Yyksho01.SeiTaiYmd >= @StartBillPeriod)
	 AND (TRIM(@EndBillPeriod) = '' OR eTKD_Yyksho01.SeiTaiYmd <= @EndBillPeriod)
	 AND (TRIM(@StartBillAddress) = '' OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') >= @StartBillAddress))
	 AND (TRIM(@EndBillAddress) = '' OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') <= @EndBillAddress))
	AND eVPM_Eigyos01.EigyoCdSeq = CASE @BillOffice WHEN 0 THEN eVPM_Eigyos01.EigyoCdSeq ELSE @BillOffice END
	AND (@StartReservationClassification = 0 OR (eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification))
	AND (@EndReservationClassification = 0 OR (eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification))
	AND (TRIM(@DepositOutputClassification) <> '????????????' OR (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui <> 0))
	AND (TRIM(@DepositOutputClassification) <> '????????????' OR (eTKD_Mishum02.NyuKinRui > 0))
	AND (TRIM(@DepositOutputClassification) <> '????????????????????????????????????' OR (RTRIM(LTRIM(ISNULL(eTKD_NyShCu01.LastCouNo, ''))) = '' AND (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui) > 0))
	AND (TRIM(@BillTypes) = '' OR (eTKD_Mishum02.SeiFutSyu IN (SELECT [value] FROM STRING_SPLIT(@BillTypes, ','))))
	AND ((@GyosyaCd = 0 OR @TokuiCd = 0 OR @SitenCd = '0' OR TRIM(@TokuiNm) = '') OR 
		( ISNULL(eVPM_Gyosya02.GyosyaCd, 0) = @GyosyaCd AND ISNULL(eVPM_Tokisk02.TokuiCd, 0) = @TokuiCd AND ISNULL(eVPM_TokiSt02.SitenCd, 0) = @SitenCd AND ISNULL(eVPM_Tokisk02.TokuiNm, '') = @TokuiNm))
ORDER BY eTKD_Yyksho01.SeiTaiYmd ASC,
	eTKD_Yyksho01.UkeNo ASC,
	eTKD_Mishum02.SeiFutSyu ASC,
	eTKD_Mishum02.FutTumRen ASC
	OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY

SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN