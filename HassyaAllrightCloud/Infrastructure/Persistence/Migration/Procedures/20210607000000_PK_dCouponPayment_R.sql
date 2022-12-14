USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dCouponPayment_R]    Script Date: 2020/12/10 9:19:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dCouponPayment_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Coupon Payment List
-- Date			:   2020/10/12
-- Author		:   Tra Nguyen Lam
-- Description	:   Get Coupon Payment List with conditions
-- =============================================
-- 2020/12/10-NTLanAnh update where conditions clause
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[PK_dCouponPayment_R] 
	-- Add the parameters for the stored procedure here
	@StartIssuePeriod						varchar(8),	
	@EndIssuePeriod							varchar(8),	
	@BillAddress							varchar(11),						
	@DepositOffice							int,				
	@StartReservationClassificationSort			int,
	@EndReservationClassificationSort			int,
	@BillTypes								varchar(max),
	@DepositOutputClassification			int,

	@TenantCdSeq							int,

	@IsGetSingle							bit,
	@IsFSuperMenu							bit,
	@UkeNo									varchar(15),
	@NyuSihCouRen							smallint,

	@Skip									int,
	@Take									int,
	@TotalCount								int OUTPUT,

	@TotalAllCouponApplicationAmount		int OUTPUT,
	@TotalAllCumulativeDeposit				int OUTPUT,
	@TotalAllUnpaidAmount					int OUTPUT
AS
BEGIN
	IF OBJECT_ID(N'tempdb..#TempTable') IS NOT NULL
	BEGIN
	DROP TABLE #TempTable
	END
	-- 運行日テーブル 受付番号毎の最小の運行日連番
	;WITH　eTKD_Unkobi01 AS (
		SELECT TKD_Unkobi.UkeNo,
			TKD_Unkobi.UnkRen,
			TKD_Unkobi.HaiSYmd,
			TKD_Unkobi.TouYmd,
			TKD_Unkobi.IkNm,
			ROW_NUMBER() OVER (PARTITION BY TKD_Unkobi.UkeNo ORDER BY TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen) AS ROW_NUMBER
		FROM TKD_Unkobi
		WHERE TKD_Unkobi.SiyoKbn  =   1
	),
	-- 入金支払明細テーブル 最大入金支払年月日
	eTKD_NyShmi01 AS (
		SELECT TKD_NyShmi.UkeNo AS UkeNo,
			TKD_NyShmi.SeiFutSyu AS SeiFutSyu,
			TKD_NyShmi.UnkRen AS UnkRen,
			TKD_NyShmi.YouTblSeq AS YouTblSeq,
			TKD_NyShmi.FutTumRen AS FutTumRen,
			eTKD_NyuSih01.NyuSihYmd AS NyuSihYmd,
			ROW_NUMBER() OVER (PARTITION BY TKD_NyShmi.UkeNo, TKD_NyShmi.SeiFutSyu, TKD_NyShmi.UnkRen, TKD_NyShmi.YouTblSeq, TKD_NyShmi.FutTumRen ORDER BY eTKD_NyuSih01.NyuSihYmd DESC) AS ROW_NUMBER
		FROM TKD_NyShmi
		INNER JOIN TKD_NyuSih AS eTKD_NyuSih01
			ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
			AND TKD_NyShmi.NyuSihKbn = 1
			AND TKD_NyShmi.SiyoKbn = 1
			AND eTKD_NyuSih01.SiyoKbn = 1
			AND eTKD_NyuSih01.TenantCdSeq = @TenantCdSeq
	),
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

	SELECT ROW_NUMBER() OVER (ORDER BY eTKD_Coupon01.HakoYmd ASC,
		eTKD_Yyksho01.UkeNo ASC,
		TKD_NyShCu.SeiFutSyu ASC,
		TKD_NyShCu.FutTumRen ASC) as [No],
		TKD_NyShCu.*,
	-- 入金支払営業所
		ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigyoCd,
		ISNULL(eVPM_Eigyos01.EigyoNm, '') AS EigyoNm,
		ISNULL(eVPM_Eigyos01.RyakuNm, '') AS EigyoRyak,
	-- 請求先業者
		ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS SeiGyosyaCd,
	-- 請求先
		ISNULL(eVPM_Tokisk02.TokuiCd, 0) ASSeiCd,
		ISNULL(eVPM_Tokisk02.TokuiNm, '') AS SeiCdNm,
		ISNULL(eVPM_Tokisk02.RyakuNm, '') AS SeiRyakuNm,
	-- 請求先支店
		ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SeiSitenCd,
		ISNULL(eVPM_TokiSt02.SitenNm, '') AS SeiSitenCdNm,
		ISNULL(eVPM_TokiSt02.RyakuNm, '') AS SeiSitRyakuNm,
	-- 請求先業者
		ISNULL(eVPM_Gyosya02.GyosyaNm, '') AS SeiGyosyaCdNm,
	-- 請求対象年月日
		eTKD_Yyksho01.SeiTaiYmd AS  SeiTaiYmd,
	-- キャンセル年月日（一覧表示用）
		CASE
			WHEN ISNULL(eTKD_Yyksho01.YoyaSyu, 0) = 2 THEN ISNULL(eTKD_Yyksho01.CanYmd, '')
			ELSE ''
		END AS CanYmd,
	-- 受付営業所
		ISNULL(eVPM_Eigyos02.EigyoCd, 0) AS UkeEigyoCd,
		ISNULL(eVPM_Eigyos02.EigyoNm, '') AS UkeEigyoNm,
		ISNULL(eVPM_Eigyos02.RyakuNm, '') AS UkeRyakuNm,
	-- 得意先
		ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS TokuiCd,
		ISNULL(eVPM_Tokisk01.TokuiNm, '') AS TokuiNm,
		ISNULL(eVPM_Tokisk01.RyakuNm, '') AS TokRyakuNm,
	-- 得意先支店
		ISNULL(eVPM_TokiSt01.SitenCd, 0) AS SitenCd,
		ISNULL(eVPM_TokiSt01.SitenNm, '') AS SitenNm,
		ISNULL(eVPM_TokiSt01.RyakuNm, '') AS SitRyakuNm,
	-- 得意先業者
		ISNULL(eVPM_Gyosya01.GyosyaCd, 0) AS GyosyaCd,
		ISNULL(eVPM_Gyosya01.GyosyaNm, '') AS GyosyaNm,
	-- 配車年月日
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Unkobi11.HaiSYmd, '')
			ELSE ISNULL(eTKD_Unkobi12.HaiSYmd, '')
		END AS HaiSYmd,
	-- 到着年月日
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Unkobi11.TouYmd, '')
			ELSE ISNULL(eTKD_Unkobi12.TouYmd, '')
		END AS TouYmd,
	-- 行き先名
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Unkobi11.IkNm, '')
			ELSE ISNULL(eTKD_Unkobi12.IkNm, '')
		END AS IkNm,
	-- 団体名
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Yyksho01.YoyaNm, '')
			ELSE ISNULL(eTKD_Unkobi12.DanTaNm, '')
		END AS DanTaNm,
	-- 税区分
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Yyksho01.ZeiKbn, 0)
			ELSE ISNULL(eTKD_FutTum11.ZeiKbn, 0)
		END AS ZeiKbn,
	-- 消費税率
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Yyksho01.Zeiritsu, 0)
			ELSE ISNULL(eTKD_FutTum11.Zeiritsu, 0)
		END AS Zeiritsu,
	-- 手数料率
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Yyksho01.TesuRitu, 0)
			ELSE ISNULL(eTKD_FutTum11.TesuRitu, 0)
		END AS TesuRitu,
	-- 付帯積込品名
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ''
			ELSE ISNULL(eTKD_FutTum11.FutTumNm, '')
		END AS FutTumNm,
	-- 付帯積込品区分
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ''
			ELSE ISNULL(CAST(eTKD_FutTum11.FutTumKbn AS VARCHAR), '')
		END AS FutTumKbn,
	-- 生産名
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ''
			ELSE ISNULL(eTKD_FutTum11.SeisanNm, '')
		END AS  SeisanNm,
	-- 数量
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ''
			ELSE ISNULL(CAST(eTKD_FutTum11.Suryo AS VARCHAR), '')
		END AS Suryo,
	-- 単価
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ''
			ELSE ISNULL(CAST(eTKD_FutTum11.TanKa AS VARCHAR), '')
		END AS TanKa,
	-- 売上額
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ''
			ELSE ISNULL(CAST(eTKD_FutTum11.UriGakKin AS VARCHAR), '')
		END AS UriGakKin,
	-- 精算コード
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN 0
			ELSE ISNULL(eVPM_Seisan01.SeisanCd, 0)
		END AS SeisanCd,
	-- 請求付帯種別名
		ISNULL(eVPM_CodeKb01.CodeKbn, '') AS SeiFutSyuCd,
		ISNULL(eVPM_CodeKb01.CodeKbnNm, '') AS SeiFutSyuNm,
	-- 税区分名
		ISNULL(eVPM_CodeKb02.CodeKbnNm, '') AS ZeiKbnNm,
	-- 予約区分
		ISNULL(eVPM_YoyKbn01.YoyaKbn, 0) AS YoyaKbn,
		ISNULL(eVPM_YoyKbn01.YoyaKbnNm, '') AS YoyaKbnNm,
	-- 最終入金年月日
		ISNULL(eTKD_NyShmi11.NyuSihYmd, '') AS LastNyuYmd,
	-- 入金累計
		ISNULL(eTKD_NyShmi12.NyuKinRui, 0) AS NyuKinRui,
	-- 額面
		ISNULL(eTKD_Coupon01.CouGkin, 0) AS CouGkin,
	-- 発行年月日
		ISNULL(eTKD_Coupon01.HakoYmd, '') AS HakoYmd,
	-- クーポン№
		ISNULL(eTKD_Coupon01.CouNo, '') AS CouNo,
	-- 入金区分
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1,7) THEN ISNULL(eTKD_Yyksho01.NyuKinKbn, 0)
			ELSE ISNULL(eTKD_FutTum11.NyuKinKbn, 0)
		END AS NyuKinKbn,
	-- 入金クーポン区分
		CASE
			WHEN TKD_NyShCu.SeiFutSyu IN (1, 7) THEN ISNULL(eTKD_Yyksho01.NCouKbn, 0)
			ELSE ISNULL(eTKD_FutTum11.NCouKbn, 0)
		END AS NCouKbn
	-- 入金支払クーポンテーブル
	into #TempTable
	FROM TKD_NyShCu
	-- クーポンテーブル
	INNER JOIN TKD_Coupon AS eTKD_Coupon01
		 ON  TKD_NyShCu.CouTblSeq = eTKD_Coupon01.CouTblSeq
		 AND TKD_NyShCu.NyuSihKbn = 1
		 AND TKD_NyShCu.SiyoKbn = 1
		 AND eTKD_Coupon01.SiyoKbn = 1
		 AND eTKD_Coupon01.TenantCdSeq = @TenantCdSeq
	-- 予約書テーブル
	INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
		ON TKD_NyShCu.UkeNo = eTKD_Yyksho01.UkeNo
		AND TKD_NyShCu.SiyoKbn = 1
		AND eTKD_Yyksho01.SiyoKbn = 1
		AND ((TKD_NyShCu.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR  (TKD_NyShCu.SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))
	-- 予約区分マスタ
	LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
		ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
		AND eVPM_YoyKbn01.TenantCdSeq = @TenantCdSeq
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
	-- 運行日テーブル 受付番号毎の最小の運行日連番のレコード
	LEFT JOIN eTKD_Unkobi01 AS eTKD_Unkobi11
		ON TKD_NyShCu.UkeNo = eTKD_Unkobi11.UkeNo
		AND eTKD_Unkobi11.ROW_NUMBER = 1
	-- 運行日テーブル 受付番号・運行日連番
	LEFT JOIN TKD_Unkobi AS eTKD_Unkobi12
		ON TKD_NyShCu.UkeNo = eTKD_Unkobi12.UkeNo
		AND TKD_NyShCu.UnkRen = eTKD_Unkobi12.UnkRen
		AND eTKD_Unkobi12.SiyoKbn = 1
	-- 付帯積込品テーブル
	LEFT JOIN TKD_FutTum AS eTKD_FutTum11
		ON TKD_NyShCu.UkeNo = eTKD_FutTum11.UkeNo
		AND TKD_NyShCu.UnkRen = eTKD_FutTum11.UnkRen
		AND TKD_NyShCu.FutTumRen = eTKD_FutTum11.FutTumRen
		AND eTKD_FutTum11.SiyoKbn = 1
		AND ((TKD_NyShCu.SeiFutSyu = 6 AND eTKD_FutTum11.FutTumKbn = 2) OR (TKD_NyShCu.SeiFutSyu <> 6 AND eTKD_FutTum11.FutTumKbn = 1))
	-- 精算マスタ
	LEFT JOIN VPM_Seisan AS eVPM_Seisan01
		ON eTKD_FutTum11.SeisanCdSeq = eVPM_Seisan01.SeisanCdSeq
	-- コード区分マスタ 請求付帯種別名
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
		ON eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'
		AND TKD_NyShCu.SeiFutSyu = eVPM_CodeKb01.CodeKbn
		AND eVPM_CodeKb01.TenantCdSeq = 0
	-- コード区分マスタ 税区分名
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02
		ON eVPM_CodeKb02.CodeSyu = 'ZEIKBN'
		AND (((TKD_NyShCu.SeiFutSyu = 1 OR  TKD_NyShCu.SeiFutSyu = 7) AND eTKD_Yyksho01.ZeiKbn = eVPM_CodeKb02.CodeKbn) 
			OR (NOT(TKD_NyShCu.SeiFutSyu = 1 OR TKD_NyShCu.SeiFutSyu = 7) AND eTKD_FutTum11.ZeiKbn = eVPM_CodeKb02.CodeKbn))
		AND eVPM_CodeKb02.TenantCdSeq = 0
	-- 入金支払明細テーブル 最大入金支払年月日
	LEFT JOIN eTKD_NyShmi01 AS eTKD_NyShmi11
		ON TKD_NyShCu.UkeNo = eTKD_NyShmi11.UkeNo
		AND TKD_NyShCu.SeiFutSyu = eTKD_NyShmi11.SeiFutSyu
		AND TKD_NyShCu.UnkRen = eTKD_NyShmi11.UnkRen
		AND TKD_NyShCu.YouTblSeq = eTKD_NyShmi11.YouTblSeq
		AND TKD_NyShCu.FutTumRen = eTKD_NyShmi11.FutTumRen
		AND eTKD_NyShmi11.ROW_NUMBER = 1
	-- 入金支払明細テーブル 受付番号入金支払クーポン連番合計
	LEFT JOIN eTKD_NyShmi02 AS eTKD_NyShmi12
		ON TKD_NyShCu.UkeNo = eTKD_NyShmi12.UkeNo
		AND TKD_NyShCu.NyuSihCouRen = eTKD_NyShmi12.NyuSihCouRen
	WHERE (@IsGetSingle = 0 OR (TKD_NyShCu.UkeNo = @UkeNo AND TKD_NyShCu.NyuSihCouRen = @NyuSihCouRen))
		AND  eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
		AND (@StartIssuePeriod = '' OR eTKD_Coupon01.HakoYmd >= @StartIssuePeriod)																									-- 発行期限 開始
		AND (@EndIssuePeriod = '' OR eTKD_Coupon01.HakoYmd <= @EndIssuePeriod)																										-- 発行期限 終了
		AND (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') + FORMAT(eVPM_TokiSt02.SitenCd,'0000') = @BillAddress)		
		AND (@DepositOffice = 0 OR eVPM_Eigyos01.EigyoCd = @DepositOffice)																											-- 入金営業所
		AND (@StartReservationClassificationSort = 0 OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassificationSort)																		-- 予約区分　開始
		AND (@EndReservationClassificationSort = 0 OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassificationSort)																			-- 予約区分　終了
		AND (@DepositOutputClassification <> 1 OR (TKD_NyShCu.CouKesG - IsNull(eTKD_NyShmi12.NyuKinRui, 0)) <> 0)																	-- 入金出力区分 == 未収のみ
		AND (@DepositOutputClassification <> 2 OR IsNull(eTKD_NyShmi12.NyuKinRui, 0) > 0)																							-- 入金出力区分 == 入金のみ
		AND (@BillTypes = '' OR TKD_NyShCu.SeiFutSyu IN ( SELECT value FROM STRING_SPLIT(@BillTypes,',')))																			-- チェックした各種別
	    AND (@IsFSuperMenu = 0 OR TKD_NyShCu.UkeNo = @UkeNo)	
	SELECT @TotalAllCouponApplicationAmount = ISNULL(SUM(CouKesG), 0), @TotalAllCumulativeDeposit = ISNULL(SUM(NyuKinRui), 0), @TotalAllUnpaidAmount = ISNULL(SUM(CouKesG - NyuKinRui), 0) FROM #TempTable
			SELECT @TotalCount = ISNULL(COUNT(*), 0) FROM #TempTable
			SELECT * from #TempTable 
			ORDER BY HakoYmd ASC,
				UkeNo ASC,
				SeiFutSyu ASC,
				FutTumRen ASC
			OFFSET @Skip ROWS FETCH FIRST @Take ROWS only
	END
