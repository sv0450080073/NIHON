USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetListPaymentAddress_R]    Script Date: 2020/12/10 10:28:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   [PK_dGetListPaymentAddress_R]
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List Payment Address 
-- Date			:   2020/10/12
-- Author		:   Tra Nguyen Lam
-- Description	:   Get List Payment Address with conditions
-- =============================================
-- 2020/12/10-NTLanAnh update where conditions clause
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[PK_dGetListPaymentAddress_R] 
	-- Add the parameters for the stored procedure here
	@StartIssuePeriod						varchar(8),	
	@EndIssuePeriod							varchar(8),	
	@StartBillAddress						varchar(11),						
	@EndBillAddress							varchar(11),
	@DepositOffice							int,				
	@StartReservationClassificationSort			int,
	@EndReservationClassificationSort			int,
	@BillTypes								varchar(max),
	@DepositOutputClassification			int,
	@TenantCdSeq							int,
	@UkeNo									varchar(15)
AS
BEGIN
	;WITH　
	-- 入金支払明細テーブル 受付番号入金支払クーポン連番合計
	eTKD_NyShmi02 AS (
		SELECT TKD_NyShmi.UkeNo,
			TKD_NyShmi.NyuSihCouRen,
			SUM(TKD_NyShmi.KesG + TKD_NyShmi.FurKesG) AS NyuKinRui
		FROM TKD_NyShmi
		WHERE TKD_NyShmi.NyuSihKbn = 1
			AND TKD_NyShmi.SiyoKbn = 1
		GROUP BY TKD_NyShmi.UkeNo,
			TKD_NyShmi.NyuSihCouRen
	)
	
	SELECT DISTINCT
	(FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + '‐' + FORMAT(eVPM_Tokisk02.TokuiCd,'0000')  + '‐' +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') + ' : ' + 
	 ISNULL(eVPM_Gyosya02.GyosyaNm, '') + ISNULL(eVPM_Tokisk02.RyakuNm, '')+ ISNULL(eVPM_TokiSt02.RyakuNm, '')) AS DisplayName,
	-- 得意先
	ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS TokuiCd,
	ISNULL(eVPM_Tokisk02.RyakuNm, '') AS TokRyakuNm,
	-- 得意先支店
	ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SitenCd,
	ISNULL(eVPM_TokiSt02.RyakuNm, '') AS SitRyakuNm,
	-- 得意先業者
	ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS GyosyaCd,
	ISNULL(eVPM_Gyosya02.GyosyaNm, '') AS GyosyaNm
	FROM TKD_NyShCu
	-- クーポンテーブル
	INNER JOIN TKD_Coupon AS eTKD_Coupon01
		 ON  TKD_NyShCu.CouTblSeq = eTKD_Coupon01.CouTblSeq
		 AND TKD_NyShCu.NyuSihKbn = 1
		 AND TKD_NyShCu.SiyoKbn = 1
		 AND eTKD_Coupon01.SiyoKbn = 1
	-- 予約書テーブル
	INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
		ON TKD_NyShCu.UkeNo = eTKD_Yyksho01.UkeNo
		AND TKD_NyShCu.SiyoKbn = 1
		AND eTKD_Yyksho01.SiyoKbn = 1
		AND ((TKD_NyShCu.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR  (TKD_NyShCu.SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))
	-- 予約区分マスタ
	LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
		ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
		AND eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
	-- 入金支払営業所
	INNER JOIN VPM_Eigyos AS eVPM_Eigyos01
		ON eTKD_Coupon01.NyuSihEigSeq = eVPM_Eigyos01.EigyoCdSeq
	-- 会社マスタ
	INNER JOIN VPM_Compny AS eVPM_Compny01
		ON eVPM_Compny01.CompanyCdSeq = eVPM_Eigyos01.CompanyCdSeq
		AND eVPM_Compny01.SiyoKbn = 1
	-- 受付営業所
	INNER JOIN VPM_Eigyos AS eVPM_Eigyos02
		ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos02.EigyoCdSeq
	-- 得意先
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
		ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
		AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq
	-- 得意先支店
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
		ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
		AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
	-- 得意先業者
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01
		ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
		AND eVPM_Gyosya01.TenantCdSeq = eVPM_Tokisk01.TenantCdSeq
	-- 請求先
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	-- 請求先支店
	LEFT JOIN VPM_TokiSt AS  eVPM_TokiSt02
		ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
		AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
		AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
	-- 請求先業者
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
		ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
		AND eVPM_Gyosya02.TenantCdSeq = eVPM_Tokisk02.TenantCdSeq
	-- 入金支払明細テーブル 受付番号入金支払クーポン連番合計
	LEFT JOIN eTKD_NyShmi02 AS eTKD_NyShmi12
		ON TKD_NyShCu.UkeNo = eTKD_NyShmi12.UkeNo
		AND TKD_NyShCu.NyuSihCouRen = eTKD_NyShmi12.NyuSihCouRen
	WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
		AND (@StartIssuePeriod = '' OR eTKD_Coupon01.HakoYmd >= @StartIssuePeriod)																									-- 発行期限 開始
		AND (@EndIssuePeriod = '' OR eTKD_Coupon01.HakoYmd <= @EndIssuePeriod)																										-- 発行期限 終了
		AND (@StartBillAddress = '' OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') >= @StartBillAddress))	-- 請求先 開始
		AND (@EndBillAddress = '' OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') <= @EndBillAddress))		-- 請求先 終了
		AND (@DepositOffice = 0 OR eVPM_Eigyos01.EigyoCd = @DepositOffice)																											-- 入金営業所
		AND (@StartReservationClassificationSort = 0 OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassificationSort)																		-- 予約区分　開始
		AND (@EndReservationClassificationSort = 0 OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassificationSort)																			-- 予約区分　終了
		AND (@DepositOutputClassification <> 1 OR (TKD_NyShCu.CouKesG - IsNull(eTKD_NyShmi12.NyuKinRui, 0)) <> 0)																	-- 入金出力区分 == 未収のみ
		AND (@DepositOutputClassification <> 2 OR IsNull(eTKD_NyShmi12.NyuKinRui, 0) > 0)																							-- 入金出力区分 == 入金のみ
		AND (@BillTypes = '' OR TKD_NyShCu.SeiFutSyu IN ( SELECT value FROM STRING_SPLIT(@BillTypes,',')))																			-- チェックした各種別
	    AND (@UkeNo = '' OR eTKD_Yyksho01.UkeNo = @UkeNo)
	END
