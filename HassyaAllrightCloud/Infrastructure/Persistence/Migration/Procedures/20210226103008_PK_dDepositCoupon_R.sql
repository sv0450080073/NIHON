USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dDepositCoupon_R]    Script Date: 3/18/2021 4:47:50 PM ******/
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
-- Update		: NTLanAnh update 2020/12/10
-- Comment		: Where conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dDepositCoupon_R] 
	(
		@_TenantCdSeq			INT,				
		@_StartBillPeriod		NVARCHAR(8),				
		@_EndBillPeriod			NVARCHAR(8),				
		@_StartBillAddress			NVARCHAR(11),				
		@_EndBillAddress			NVARCHAR(11),				
		@_BillOffice			INT,				
		@_StartReservationClassification			TINYINT,				
		@_EndReservationClassification			TINYINT,				
		@_BillTypes			NVARCHAR(100),				
		@_DepositOutputClassification			NVARCHAR(100),
		@_GyosyaCd			SMALLINT,
		@_TokuiCd			SMALLINT,
		@_SitenCd			SMALLINT,
		@_UkeCd                 VARCHAR(15),
		@_Offset				INT,					--Offset rows data
		@_Limit				INT,					--Limit rows data
	-- Parameter

	-- Output
		@ROWCOUNT				INTEGER OUTPUT
	)
AS
BEGIN
		DECLARE @TenantCdSeq			INT,				
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
		@UkeCd                 VARCHAR(15),
		@Offset				INT,					--Offset rows data
		@Limit				INT					--Limit rows data

		SET	@TenantCdSeq = @_TenantCdSeq
		SET	@StartBillPeriod = @_StartBillPeriod		
		SET @EndBillPeriod = @_EndBillPeriod			
		SET @StartBillAddress = @_StartBillAddress			
		SET @EndBillAddress = @_EndBillAddress		
		SET @BillOffice	= @_BillOffice			
		SET @StartReservationClassification = @_StartReservationClassification	
		SET @EndReservationClassification = @_EndReservationClassification
		SET @BillTypes = @_BillTypes
		SET @DepositOutputClassification = @_DepositOutputClassification
		SET @GyosyaCd = @_GyosyaCd
		SET @TokuiCd = @_TokuiCd
		SET @SitenCd = @_SitenCd
		SET @UkeCd = @_UkeCd
		SET @Offset = @_Offset
		SET @Limit = @_Limit
		SET @RowCount =	0

		IF OBJECT_ID(N'tempdb..#DepositCoupon') IS NOT NULL
		BEGIN
		DROP TABLE #DepositCoupon
		END
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
	CAST(ISNULL(eTKD_NyShmi01.LastNyuYmd, '') as nvarchar) AS LastNyuYmd,
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
-- <予約書データ>
INTO #DepositCoupon
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
	AND eVPM_Tokisk01.TenantCdSeq =  @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
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
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb011
	ON eVPM_CodeKb011.CodeSyu = 'SEIFUTSYU'
	AND eTKD_Mishum02.SeiFutSyu = eVPM_CodeKb011.CodeKbn
	AND eVPM_CodeKb011.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
 LEFT JOIN VPM_CodeKb AS eVPM_CodeKb012
	ON eVPM_CodeKb012.CodeSyu = 'SEIFUTSYU'
	AND eTKD_Mishum02.SeiFutSyu = eVPM_CodeKb012.CodeKbn
	AND eVPM_CodeKb012.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- <税区分データ>
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02
	ON eVPM_CodeKb02.CodeSyu = 'ZEIKBN'
	AND ((eTKD_Mishum02.SeiFutSyu IN (1,7) AND eTKD_Yyksho01.ZeiKbn = eVPM_CodeKb02.CodeKbn ) OR (eTKD_Mishum02.SeiFutSyu NOT IN (1,7) AND eTKD_FutTum11.ZeiKbn = eVPM_CodeKb02.CodeKbn))
	AND eVPM_CodeKb02.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
-- <予約区分データ>
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
	ON eVPM_YoyKbn01.YoyaKbnSeq = eTKD_Yyksho01.YoyaKbnSeq
LEFT JOIN VPM_YoyaKbnSort AS eVPM_YoyKbnSort01
     ON eVPM_YoyKbn01.YoyaKbnSeq = eVPM_YoyKbnSort01.YoyaKbnSeq
     AND eVPM_YoyKbnSort01.TenantCdSeq = @TenantCdSeq
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
	AND eTKD_Yyksho01.SeiTaiYmd >= ISNULL(@StartBillPeriod, eTKD_Yyksho01.SeiTaiYmd)
	AND eTKD_Yyksho01.SeiTaiYmd <= ISNULL(@EndBillPeriod, eTKD_Yyksho01.SeiTaiYmd)
	AND (@StartBillAddress IS NULL OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) >= CAST(@StartBillAddress as bigint))
	AND (@EndBillAddress IS NULL OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) * 100000000 + ISNULL(eVPM_Tokisk02.TokuiCd, 0) * 10000 + ISNULL(eVPM_TokiSt02.SitenCd,0) <= CAST(@EndBillAddress as bigint))
	AND eVPM_Eigyos01.EigyoCdSeq = ISNULL(@BillOffice, eVPM_Eigyos01.EigyoCdSeq)
AND (@StartReservationClassification = 0 
OR CONCAT(CASE WHEN eVPM_YoyKbnSort01.PriorityNum IS NULL THEN '99' ELSE FORMAT(eVPM_YoyKbnSort01.PriorityNum, '00') END, FORMAT(eVPM_YoyKbn01.YoyaKbn, '00')) >= @StartReservationClassification)
AND (@EndReservationClassification = 0 
OR CONCAT(CASE WHEN eVPM_YoyKbnSort01.PriorityNum IS NULL THEN '99' ELSE FORMAT(eVPM_YoyKbnSort01.PriorityNum, '00') END, FORMAT(eVPM_YoyKbn01.YoyaKbn, '00'))<= @EndReservationClassification)
	AND (@DepositOutputClassification IS NULL
	OR (@DepositOutputClassification = '未収のみ' AND eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui <> 0)
	OR (@DepositOutputClassification = '入金のみ' AND eTKD_Mishum02.NyuKinRui > 0)
	OR (@DepositOutputClassification = 'クーポン未入力の未収のみ' AND (RTRIM(LTRIM(ISNULL(eTKD_NyShCu01.LastCouNo, ''))) = '' AND (eTKD_Mishum02.SeiKin - eTKD_Mishum02.NyuKinRui) > 0)))
	AND (@BillTypes IS NULL OR (eTKD_Mishum02.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ','))))
	AND (@GyosyaCd IS NULL OR ISNULL(eVPM_Gyosya02.GyosyaCd, 0) = @GyosyaCd)
	AND (@TokuiCd IS NULL OR ISNULL(eVPM_Tokisk02.TokuiCd, 0) = @TokuiCd)
	AND (@SitenCd IS NULL OR ISNULL(eVPM_TokiSt02.SitenCd, 0) = @SitenCd)
	AND eTKD_Yyksho01.UkeNo = ISNULL(@UkeCd, eTKD_Yyksho01.UkeNo)

	Select * from #DepositCoupon
	ORDER BY #DepositCoupon.SeiTaiYmd ASC,
	#DepositCoupon.UkeNo ASC
	OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY

	SELECT
	COUNT(*) AS CountNumber,
	SUM(CAST(#DepositCoupon.UriGakKin as bigint)) as TotalSaleAmount,
	SUM(CAST(#DepositCoupon.SyaRyoSyo as bigint)) as TotalTaxAmount,
	SUM(CAST(#DepositCoupon.UriGakKin as bigint) + CAST(#DepositCoupon.SyaRyoSyo as bigint)) as TotalTaxIncluded,
	SUM(CAST(#DepositCoupon.SyaRyoTes as bigint)) as TotalCommissionAmount,
	SUM(CAST(#DepositCoupon.SeiKin as bigint)) as TotalBillAmount,
	SUM(CAST(#DepositCoupon.NyuKinRui as bigint)) as TotalCumulativeDeposit,
	SUM(CAST(#DepositCoupon.SeiKin as bigint) - CAST(#DepositCoupon.NyuKinRui as bigint)) as TotalUnpaidAmount
	from #DepositCoupon
	OPTION(RECOMPILE)
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN