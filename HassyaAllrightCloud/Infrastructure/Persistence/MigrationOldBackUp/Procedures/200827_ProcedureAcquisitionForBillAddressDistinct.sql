USE [HOC_Kashikiri]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dBillCheckListForDistinctBillAddress_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data bill check list combobox bill address distinct
-- Date			:   2020/08/27
-- Author		:   N.T.Lan.Anh
-- Description	:   Get data for bill check list combobox bill address distinct  with conditions
------------------------------------------------------------
CREATE OR ALTER  PROCEDURE [dbo].[PK_dBillCheckListForDistinctBillAddress_R]
	(
	-- Parameter
		    @TenantCdSeq int                                
		,	@CompanyCdSeq int 
		,	@StartBillPeriod nvarchar(8)                -- 請求対象期間 開始       
		,	@EndBillPeriod nvarchar(8)                  -- 請求対象期間 終了
		,	@BillOffice int                             -- 請求営業所   
		,	@StartBillAddress nvarchar(11)              -- 請求先 開始  
		,	@EndBillAddress nvarchar(11)                -- 請求先 終了
		,	@StartReceiptNumber char(15)                -- 予約番号　開始
		,	@EndReceiptNumber char(15)                  -- 予約番号　終了
		,	@StartReservationClassification int         -- 予約区分　開始
		,	@EndReservationClassification int           -- 予約区分　終了
		,   @StartBillClassification nvarchar(10)       -- 請求区分 開始
	    ,   @EndBillClassification nvarchar(10)         -- 請求区分 終了
	    ,   @BillTypes nvarchar(20)                              -- 請求発行済区分 == 請求済
		,   @BillIssuedClassification nvarchar(10)
	 	-- Output
		,	@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS 
DECLARE @strSQL VARCHAR(MAX)

	-- Processing
    BEGIN
			SET	@strSQL             =
+	'  WITH eTKD_Unkobi01 AS (                                                                                                                                                                                '
+   CHAR(13)+CHAR(10)	+	'	SELECT TKD_Unkobi.UkeNo,                                                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'		TKD_Unkobi.UnkRen,                                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		TKD_Unkobi.HaiSYmd,                                                                                                                                                     '
+   CHAR(13)+CHAR(10)	+	'		TKD_Unkobi.TouYmd,                                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		TKD_Unkobi.IkNm,                                                                                                                                                        '
+   CHAR(13)+CHAR(10)	+	'		ROW_NUMBER() OVER (PARTITION BY TKD_Unkobi.UkeNo ORDER BY TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen) AS ROW_NUMBER                                                            '
+   CHAR(13)+CHAR(10)	+	'	FROM TKD_Unkobi                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	WHERE TKD_Unkobi.SiyoKbn = 1                                                                                                                                                '
+   CHAR(13)+CHAR(10)	+	'),                                                                                                                                                                             '
SET @strSQL = @strSQL
+   CHAR(13)+CHAR(10)	+	'-- 運行日テーブル 受付番号毎の最小の運行日連番のレコード                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'eTKD_Unkobi02 AS (                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	SELECT eTKD_Unkobi01.UkeNo,                                                                                                                                                 '
+   CHAR(13)+CHAR(10)	+	'		eTKD_Unkobi01.UnkRen,                                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'		eTKD_Unkobi01.HaiSYmd,                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'		eTKD_Unkobi01.TouYmd,                                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'		eTKD_Unkobi01.IkNm                                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'	FROM eTKD_Unkobi01                                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	WHERE eTKD_Unkobi01.ROW_NUMBER = 1                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'),                                                                                                                                                                             '
SET @strSQL = @strSQL
+   CHAR(13)+CHAR(10)	+	'-- 入金支払明細テーブル                                                                                                                                                        '
+   CHAR(13)+CHAR(10)	+	'eTKD_NyShmi01 AS (                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	SELECT TKD_NyShmi.UkeNo AS UkeNo,                                                                                                                                           '
+   CHAR(13)+CHAR(10)	+	'		TKD_NyShmi.UnkRen AS UnkRen,                                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	'		TKD_NyShmi.FutTumRen AS FutTumRen,                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		TKD_NyShmi.SeiFutSyu AS SeiFutSyu,                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		MAX(eTKD_NyuSih01.NyuSihYmd) AS NyuSihYmd                                                                                                                               '
+   CHAR(13)+CHAR(10)	+	'	FROM TKD_NyShmi                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	INNER JOIN TKD_NyuSih AS eTKD_NyuSih01                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq                                                                                                                 '
+   CHAR(13)+CHAR(10)	+	'		AND TKD_NyShmi.NyuSihKbn = 1                                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	'		AND TKD_NyShmi.SiyoKbn = 1                                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'		AND eTKD_NyuSih01.SiyoKbn = 1                                                                                                                                           '
+   CHAR(13)+CHAR(10)	+	'	GROUP BY TKD_NyShmi.UkeNo,                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'		TKD_NyShmi.UnkRen,                                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		TKD_NyShmi.FutTumRen,                                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'		TKD_NyShmi.SeiFutSyu                                                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'),                                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'-- 請求明細テーブル 受付番号、未収明細連番毎の行番号採番                                                                                                                       '
SET @strSQL = @strSQL
+   CHAR(13)+CHAR(10)	+	'eTKD_SeiMei01 AS (                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	SELECT TKD_SeiMei.SeiOutSeq AS SeiOutSeq,                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'		TKD_SeiMei.SeiRen AS  SeiRen,                                                                                                                                           '
+   CHAR(13)+CHAR(10)	+	'		TKD_SeiMei.SeiMeiRen AS  SeiMeiRen,                                                                                                                                     '
+   CHAR(13)+CHAR(10)	+	'		TKD_SeiMei.UkeNo AS UkeNo,                                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'		TKD_SeiMei.MisyuRen AS MisyuRen,                                                                                                                                        '
+   CHAR(13)+CHAR(10)	+	'		TKD_SeiPrS.SeiHatYmd AS SeiHatYmd,                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'		ROW_NUMBER() OVER (PARTITION BY TKD_SeiMei.UkeNo, TKD_SeiMei.MisyuRen ORDER BY TKD_SeiMei.UkeNo, TKD_SeiMei.MisyuRen, TKD_SeiPrS.SeiHatYmd DESC) AS ROW_NUMBER          '
+   CHAR(13)+CHAR(10)	+	'	FROM TKD_SeiMei                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	INNER JOIN TKD_SeiPrS                                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'		ON  TKD_SeiMei.SeiOutSeq = TKD_SeiPrS.SeiOutSeq                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'		AND TKD_SeiPrS.SiyoKbn = 1                                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'		AND TKD_SeiMei.SiyoKbn = 1                                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'),                                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'-- 請求明細テーブル 受付番号、未収明細連番毎の最大発行年月日                                                                                                                   '
SET @strSQL = @strSQL
+   CHAR(13)+CHAR(10)	+	'eTKD_SeiMei02 AS (                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	SELECT DISTINCT eTKD_SeiMei01.UkeNo,                                                                                                                                        '
+   CHAR(13)+CHAR(10)	+	'		eTKD_SeiMei01.MisyuRen,                                                                                                                                                 '
+   CHAR(13)+CHAR(10)	+	'		eTKD_SeiMei01.SeiHatYmd                                                                                                                                                 '
+   CHAR(13)+CHAR(10)	+	'	FROM eTKD_SeiMei01                                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	WHERE eTKD_SeiMei01.ROW_NUMBER = 1                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	')                                                                                                                                                                              '

SET @strSQL = @strSQL
+   CHAR(13)+CHAR(10)	+ 'SELECT    DISTINCT eVPM_Gyosya02.GyosyaCd AS GyosyaCd,                                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+ ' eVPM_Tokisk02.TokuiCd AS TokuiCd,               '
+   CHAR(13)+CHAR(10)	+ ' eVPM_TokiSt02.SitenCd AS SitenCd,                   '
+   CHAR(13)+CHAR(10)	+ ' eVPM_Gyosya02.GyosyaCdSeq AS GyosyaCdSeq,              '
+   CHAR(13)+CHAR(10)	+ ' eVPM_Tokisk02.TokuiSeq AS TokuiSeq,        '
+   CHAR(13)+CHAR(10)	+ ' eVPM_TokiSt02.SitenCdSeq AS SitenCdSeq,  '
+   CHAR(13)+CHAR(10)	+ ' ISNULL(eVPM_Tokisk02.RyakuNm, '' '') AS RyakuNm,        '
+   CHAR(13)+CHAR(10)	+ ' ISNULL(eVPM_TokiSt02.SitenNm, '' '') AS SitenNm,  '
+   CHAR(13)+CHAR(10)	+ ' ISNULL(eVPM_TokiSt02.TesuRitu, 0) AS TesuRitu,  '
+   CHAR(13)+CHAR(10)	+ ' ISNULL(eVPM_TokiSt02.TesuRituGui, 0) AS TesuRituGui,  '
+   CHAR(13)+CHAR(10)	+ ' ISNULL(eVPM_Gyosya02.GyosyaNm, '' '') AS GyosyaNm              '

SET @strSQL = @strSQL
+   CHAR(13)+CHAR(10)	+	'-- 未収明細テーブル                                                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	'FROM TKD_Mishum                                                                                                                                                                '
+   CHAR(13)+CHAR(10)	+	'-- 予約書テーブル                                                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'INNER JOIN TKD_Yyksho AS eTKD_Yyksho01                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'	ON TKD_Mishum.UkeNo = eTKD_Yyksho01.UkeNo                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.SiyoKbn = 1                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Yyksho01.SiyoKbn = 1                                                                                                                                               '
+   CHAR(13)+CHAR(10)	+	'	AND ((TKD_Mishum.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (TKD_Mishum.SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))                                                 '
+   CHAR(13)+CHAR(10)	+	'-- 予約区分マスタ                                                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON  eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq                                                                                                                     '
+   CHAR(13)+CHAR(10)	+	'-- コード区分マスタ 請求区分                                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_CodeKb AS eVPM_CodeKb04                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON eVPM_CodeKb04.CodeSyu = ''SEIKYUKBN''                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Yyksho01.SeiKyuKbnSeq = eVPM_CodeKb04.CodeKbnSeq                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	    CONCAT('AND eVPM_CodeKb04.TenantCdSeq = ', @TenantCdSeq)   
+   CHAR(13)+CHAR(10)	+	'-- 請求営業所                                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'INNER JOIN VPM_Eigyos AS eVPM_Eigyos01                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'	 ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'-- 受付営業所                                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'INNER JOIN VPM_Eigyos AS eVPM_Eigyos02                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'	ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos02.EigyoCdSeq                                                                                                                     '
+   CHAR(13)+CHAR(10)	+	'-- 得意先支店                                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'INNER JOIN VPM_TokiSt AS eVPM_TokiSt01                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'	ON  eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq                                                                                                                     '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd                                                                                   '
+   CHAR(13)+CHAR(10)	+	'-- 請求先                                                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd                                                                                   '
+   CHAR(13)+CHAR(10)	+	    CONCAT('AND eVPM_Tokisk02.TenantCdSeq = ', @TenantCdSeq)   
+   CHAR(13)+CHAR(10)	+	'-- 請求先支店                                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd                                                                                   '
+   CHAR(13)+CHAR(10)	+	'-- 請求先業者                                                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'-- 運行日テーブル 受付番号毎の最小の運行日連番のレコード                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN eTKD_Unkobi02 AS eTKD_Unkobi11                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'	ON TKD_Mishum.UkeNo = eTKD_Unkobi11.UkeNo                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'-- 運行日テーブル 受付番号・運行日連番                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN TKD_Unkobi AS eTKD_Unkobi12                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON TKD_Mishum.UkeNo = eTKD_Unkobi12.UkeNo                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.FutuUnkRen = eTKD_Unkobi12.UnkRen                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_Unkobi12.SiyoKbn = 1                                                                                                                                               '
+   CHAR(13)+CHAR(10)	+	'-- 付帯積込品テーブル                                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN TKD_FutTum AS eTKD_FutTum11                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON TKD_Mishum.UkeNo = eTKD_FutTum11.UkeNo                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.FutuUnkRen = eTKD_FutTum11.UnkRen                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.FutTumRen = eTKD_FutTum11.FutTumRen                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	AND eTKD_FutTum11.SiyoKbn = 1                                                                                                                                               '
+   CHAR(13)+CHAR(10)	+	'	AND ((TKD_Mishum.SeiFutSyu = 6 AND eTKD_FutTum11.FutTumKbn = 2) OR (TKD_Mishum.SeiFutSyu <> 6 AND eTKD_FutTum11.FutTumKbn = 1))                                             '
+   CHAR(13)+CHAR(10)	+	'-- コード区分マスタ 請求付帯種別名                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01                                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	ON eVPM_CodeKb01.CodeSyu = ''SEIFUTSYU''                                                                                                                                      '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.SeiFutSyu = eVPM_CodeKb01.CodeKbn                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	    CONCAT('AND eVPM_CodeKb01.TenantCdSeq = ', @TenantCdSeq)   
+   CHAR(13)+CHAR(10)	+	'-- 入金支払明細 受付番号・運行日連番・付帯積込品連番集計                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN eTKD_NyShmi01 AS eTKD_NyShmi11                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'	ON  TKD_Mishum.UkeNo = eTKD_NyShmi11.UkeNo                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.FutuUnkRen  = eTKD_NyShmi11.UnkRen                                                                                                                           '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.FutTumRen = eTKD_NyShmi11.FutTumRen                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi11.SeiFutSyu                                                                                                                          '
+   CHAR(13)+CHAR(10)	+	'-- 請求明細                                                                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN eTKD_SeiMei02 AS eTKD_SeiMei11                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'	ON TKD_Mishum.UkeNo = eTKD_SeiMei11.UkeNo                                                                                                                                   '
+   CHAR(13)+CHAR(10)	+	'	AND TKD_Mishum.MisyuRen = eTKD_SeiMei11.MisyuRen                                                                                                                            '
+   CHAR(13)+CHAR(10)	+	'-- 予約車種 台数                                                                                                                                                               '
+   CHAR(13)+CHAR(10)	+	'LEFT JOIN (                                                                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'	SELECT TKD_YykSyu.UkeNo,                                                                                                                                                    '
+   CHAR(13)+CHAR(10)	+	'		SUM(SyaSyuDai) AS Sum_SyaSyuDai,                                                                                                                                        '
+   CHAR(13)+CHAR(10)	+	'		SUM(SyaSyuTan) AS Sum_SyaSyuTan                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'	FROM TKD_YykSyu                                                                                                                                                             '
+   CHAR(13)+CHAR(10)	+	'	INNER JOIN (                                                                                                                                                                '
+   CHAR(13)+CHAR(10)	+	'		SELECT UkeNo,                                                                                                                                                           '
+   CHAR(13)+CHAR(10)	+	'			Min(UnkRen) AS Min_UnkRen                                                                                                                                           '
+   CHAR(13)+CHAR(10)	+	'		FROM TKD_YykSyu                                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'		WHERE SiyoKbn = 1                                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'		GROUP BY UkeNo ) AS SUB                                                                                                                                                 '
+   CHAR(13)+CHAR(10)	+	'		ON SUB.UkeNo = TKD_YykSyu.UkeNo                                                                                                                                         '
+   CHAR(13)+CHAR(10)	+	'		AND SUB.Min_UnkRen = TKD_YykSyu.UnkRen                                                                                                                                  '
+   CHAR(13)+CHAR(10)	+	'		WHERE SiyoKbn = 1                                                                                                                                                       '
+   CHAR(13)+CHAR(10)	+	'		GROUP BY TKD_YykSyu.UkeNo ) AS eTKD_YykSyu                                                                                                                              '
+   CHAR(13)+CHAR(10)	+	'	ON TKD_Mishum.UkeNo = eTKD_YykSyu.UkeNo                                                                                                                                     '
+   CHAR(13)+CHAR(10)	+	'   WHERE                                                                                                                 '
+   CHAR(13)+CHAR(10)	+	    CONCAT('eTKD_Yyksho01.TenantCdSeq = ', @TenantCdSeq)   
IF (@StartBillPeriod IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Yyksho01.SeiTaiYmd >= ''', @StartBillPeriod, '''')
END
IF (@EndBillPeriod IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Yyksho01.SeiTaiYmd <= ''', @EndBillPeriod, '''')
END
IF (@BillOffice IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Eigyos01.EigyoCdSeq = ', @BillOffice)
END	
IF (@StartBillAddress IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND FORMAT(eVPM_Gyosya02.GyosyaCd,''000'') + FORMAT(eVPM_Tokisk02.TokuiCd,''0000'') +	FORMAT(eVPM_TokiSt02.SitenCd,''0000'') >= ''', @StartBillAddress, '''')
END	
IF (@EndBillAddress IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND FORMAT(eVPM_Gyosya02.GyosyaCd,''000'') + FORMAT(eVPM_Tokisk02.TokuiCd,''0000'') +	FORMAT(eVPM_TokiSt02.SitenCd,''0000'') <= ''', @EndBillAddress, '''')
END
IF (@StartReceiptNumber IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Yyksho01.UkeNo >= ''', @StartReceiptNumber, '''')
END
IF (@EndReceiptNumber IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Yyksho01.UkeNo <= ''', @EndReceiptNumber, '''')
END
IF (@StartReservationClassification IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_YoyKbn01.YoyaKbn >= ', @StartReservationClassification)
END	
IF (@EndReservationClassification IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_YoyKbn01.YoyaKbn <= ', @EndReservationClassification)
END
IF (@StartBillClassification IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_CodeKb04.CodeKbn >= ''', @StartBillClassification, '''')
END
IF (@EndBillClassification IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_CodeKb04.CodeKbn <= ''', @EndBillClassification, '''')
END	
IF (@BillIssuedClassification = 1)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ 'AND TRIM(eTKD_SeiMei11.SeiHatYmd) <> ''''   '
END	
IF (@BillIssuedClassification = 2)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ 'AND TRIM(eTKD_SeiMei11.SeiHatYmd) = ''''   '
END		
IF (@BillTypes IS NOT NULL)
BEGIN
	SET @strSQL = @strSQL +
		+   CHAR(13)+CHAR(10)	+ CONCAT('AND TKD_Mishum.SeiFutSyu IN ', @BillTypes)
END
--
SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ ' ORDER BY GyosyaCd,     '
					+   CHAR(13)+CHAR(10)	+ ' TokuiCd,               '
					+   CHAR(13)+CHAR(10)	+ ' SitenCd,                   '
					+   CHAR(13)+CHAR(10)	+ ' GyosyaCdSeq,              '
					+   CHAR(13)+CHAR(10)	+ ' TokuiSeq,        '
					+   CHAR(13)+CHAR(10)	+ ' SitenCdSeq  '
EXEC(@strSQL)
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN
