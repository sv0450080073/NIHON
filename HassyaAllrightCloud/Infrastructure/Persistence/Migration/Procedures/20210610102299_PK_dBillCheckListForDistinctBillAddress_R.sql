USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBillCheckListForDistinctBillAddress_R]    Script Date: 2021/06/10 10:21:24 ******/
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
-- Date			:   2020/10/27
-- Author		:   N.T.Lan.Anh
-- Description	:   Get data for bill check list combobox bill address distinct  with conditions
------------------------------------------------------------
CREATE OR ALTER         PROCEDURE [dbo].[PK_dBillCheckListForDistinctBillAddress_R]
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
	)
AS 
WITH eTKD_SeiMei01 AS (                                                                                                                                                             
	SELECT TKD_SeiMei.SeiOutSeq AS SeiOutSeq,                                                                                                                                   
		TKD_SeiMei.SeiRen AS  SeiRen,                                                                                                                                           
		TKD_SeiMei.SeiMeiRen AS  SeiMeiRen,                                                                                                                                     
		TKD_SeiMei.UkeNo AS UkeNo,                                                                                                                                              
		TKD_SeiMei.MisyuRen AS MisyuRen,                                                                                                                                        
		TKD_SeiPrS.SeiHatYmd AS SeiHatYmd,                                                                                                                                      
		ROW_NUMBER() OVER (PARTITION BY TKD_SeiMei.UkeNo, TKD_SeiMei.MisyuRen ORDER BY TKD_SeiMei.UkeNo, TKD_SeiMei.MisyuRen, TKD_SeiPrS.SeiHatYmd DESC) AS ROW_NUMBER          
	FROM TKD_SeiMei                                                                                                                                                             
	INNER JOIN TKD_SeiPrS                                                                                                                                                       
		ON  TKD_SeiMei.SeiOutSeq = TKD_SeiPrS.SeiOutSeq                                                                                                                         
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
SELECT    DISTINCT 
 ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS GyosyaCd,                                                                                                                                                       
 ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS TokuiCd,               
 ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SitenCd,                   
 ISNULL(eVPM_Gyosya02.GyosyaCdSeq, 0) AS GyosyaCdSeq,              
 ISNULL(eVPM_Tokisk02.TokuiSeq, 0) AS TokuiSeq,        
 ISNULL(eVPM_TokiSt02.SitenCdSeq, 0) AS SitenCdSeq,  
 ISNULL(eVPM_Tokisk02.RyakuNm, ' ') AS RyakuNm,        
 ISNULL(eVPM_TokiSt02.SitenNm, ' ') AS SitenNm,  
 ISNULL(eVPM_TokiSt02.TesuRitu, 0) AS TesuRitu,  
 ISNULL(eVPM_TokiSt02.TesuRituGui, 0) AS TesuRituGui,  
 ISNULL(eVPM_Gyosya02.GyosyaNm, ' ') AS GyosyaNm              
-- 未収明細テーブル                                                                                                                                                            
FROM TKD_Mishum                                                                                                                                                                
-- 予約書テーブル                                                                                                                                                              
INNER JOIN TKD_Yyksho AS eTKD_Yyksho01                                                                                                                                         
	ON TKD_Mishum.UkeNo = eTKD_Yyksho01.UkeNo                                                                                                                                   
	AND TKD_Mishum.SiyoKbn = 1                                                                                                                                                  
	AND eTKD_Yyksho01.SiyoKbn = 1                                                                                                                                               
	AND ((TKD_Mishum.SeiFutSyu <> 7 AND eTKD_Yyksho01.YoyaSyu = 1) OR (TKD_Mishum.SeiFutSyu = 7 AND eTKD_Yyksho01.YoyaSyu = 2))                                                 
-- 予約区分マスタ                                                                                                                                                              
LEFT JOIN  VPM_YoyKbn AS eVPM_YoyKbn01                                                                                                                                          
	ON  eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq   
	AND eVPM_YoyKbn01.TenantCdSeq = @TenantCdSeq
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
	ON  eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq                                                                                                                         
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
    AND eVPM_Gyosya02.TenantCdSeq = eVPM_Tokisk02.TenantCdSeq                                                                                                                  
-- 請求明細                                                                                                                                                                    
LEFT JOIN eTKD_SeiMei02 AS eTKD_SeiMei11                                                                                                                                       
	ON TKD_Mishum.UkeNo = eTKD_SeiMei11.UkeNo                                                                                                                                   
	AND TKD_Mishum.MisyuRen = eTKD_SeiMei11.MisyuRen                                                                                                                            
                                                                                                                                   
WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
      AND (@StartBillPeriod IS NULL OR eTKD_Yyksho01.SeiTaiYmd >= @StartBillPeriod)
	  AND (@EndBillPeriod IS NULL OR eTKD_Yyksho01.SeiTaiYmd <= @EndBillPeriod)
	  AND (@BillOffice IS NULL OR eVPM_Eigyos01.EigyoCdSeq = @BillOffice)
	  AND (@StartBillAddress IS NULL OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000')) >= @StartBillAddress)
	  AND (@EndBillAddress IS NULL OR (FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000')) <= @EndBillAddress)
	  AND (@StartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber)
	  AND (@EndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber)
	  AND (@StartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)
	  AND (@EndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)
	  AND (@StartBillClassification IS NULL OR eVPM_CodeKb04.CodeKbn >= @StartBillClassification)
	  AND (@EndBillClassification IS NULL OR eVPM_CodeKb04.CodeKbn <= @EndBillClassification)
	  AND (@BillIssuedClassification IS NULL  OR (@BillIssuedClassification = 1 AND TRIM(eTKD_SeiMei11.SeiHatYmd) <> '' ) OR (@BillIssuedClassification = 2 AND TRIM(eTKD_SeiMei11.SeiHatYmd) = '' ))
	  AND (@BillTypes IS NULL OR TKD_Mishum.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))
 ORDER BY GyosyaCd, 
          TokuiCd,               
          SitenCd,                   
          GyosyaCdSeq,              
          TokuiSeq,        
          SitenCdSeq  
OPTION (RECOMPILE)
RETURN
