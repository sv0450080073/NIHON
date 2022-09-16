USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBillCheckListReport_R]    Script Date: 2021/04/22 16:13:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dBillCheckListReport_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data bill check list report
-- Date			:   2020/10/27
-- Author		:   N.T.Lan.Anh
-- Description	:   Get data for bill check list report with conditions
------------------------------------------------------------
CREATE OR ALTER          PROCEDURE [dbo].[PK_dBillCheckListReport_R]
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
		,   @BillTypeOrderBy char(1)
	)
AS 
  WITH eTKD_Unkobi01 AS (                                                                                                                                                                                
	SELECT TKD_Unkobi.UkeNo,                                                                                                                                                    
		TKD_Unkobi.UnkRen,                                                                                                                                                      
		TKD_Unkobi.HaiSYmd,                                                                                                                                                     
		TKD_Unkobi.TouYmd,                                                                                                                                                      
		TKD_Unkobi.IkNm,                                                                                                                                                        
		ROW_NUMBER() OVER (PARTITION BY TKD_Unkobi.UkeNo ORDER BY TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen) AS ROW_NUMBER                                                            
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
SELECT TKD_Mishum.*,                                                                                                                                                           
-- 請求営業所                                                                                                                                                                  
	ISNULL(eVPM_Eigyos01.EigyoCdSeq, 0) AS SeiEigyoCdSeq,                                                                                                                       
	ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS SeiEigyoCd,                                                                                                                             
	ISNULL(eVPM_Eigyos01.EigyoNm, '') AS SeiEigyoNm,                                                                                                                            
	ISNULL(eVPM_Eigyos01.RyakuNm, '') AS SeiEigyoRyak,                                                                                                                          
-- 請求先業者                                                                                                                                                                  
	ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS SeiGyosyaCd,                                                                                                                           
-- 請求先                                                                                                                                                                      
	ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS SeiCd,                                                                                                                                  
	ISNULL(eVPM_Tokisk02.TokuiNm, '') AS SeiCdNm,                                                                                                                               
	ISNULL(eVPM_Tokisk02.RyakuNm, '') AS SeiRyakuNm,                                                                                                                            
-- 請求先支店                                                                                                                                                                  
	ISNULL(eVPM_TokiSt02.SitenCd, 0) AS SeiSitenCd,                                                                                                                             
	ISNULL(eVPM_TokiSt02.SitenNm, '') AS SeiSitenCdNm,                                                                                                                          
	ISNULL(eVPM_TokiSt02.RyakuNm, '') AS SeiSitRyakuNm,                                                                                                                         
-- 請求先業者                                                                                                                                                                  
	ISNULL(eVPM_Gyosya02.GyosyaNm, '') AS SeiGyosyaCdNm,                                                                                                                        
-- 請求対象年月日                                                                                                                                                              
	ISNULL(eTKD_Yyksho01.SeiTaiYmd, '') AS SeiTaiYmd,                                                                                                                           
-- 受付営業所                                                                                                                                                                  
	ISNULL(eVPM_Eigyos02.EigyoCd, 0) AS UkeEigyoCd,                                                                                                                             
	ISNULL(eVPM_Eigyos02.EigyoNm, '') AS UkeEigyoNm,                                                                                                                            
	ISNULL(eVPM_Eigyos02.RyakuNm, '') AS UkeRyakuNm,                                                                                                                            
-- 配車年月日                                                                                                                                                                  
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Unkobi11.HaiSYmd, '')                                                                                               
		ELSE ISNULL(eTKD_Unkobi12.HaiSYmd, '')                                                                                                                                  
	END AS HaiSYmd,                                                                                                                                                             
-- 到着年月日                                                                                                                                                                  
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Unkobi11.TouYmd, '')                                                                                                
		ELSE ISNULL(eTKD_Unkobi12.TouYmd, '')                                                                                                                                   
	END AS TouYmd,                                                                                                                                                              
-- 行き先名                                                                                                                                                                    
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Unkobi11.IkNm, '')                                                                                                  
		ELSE ISNULL(eTKD_Unkobi12.IkNm, '')                                                                                                                                     
	END AS IkNm,                                                                                                                                                                
-- 団体名                                                                                                                                                                      
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Yyksho01.YoyaNm, '')                                                                                                
		ELSE ISNULL(eTKD_Unkobi12.DanTaNm, '')                                                                                                                                  
	END AS DanTaNm,                                                                                                                                                             
-- 手数料率                                                                                                                                                                    
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Yyksho01.TesuRitu, 0)                                                                                               
		ELSE ISNULL(eTKD_FutTum11.TesuRitu, 0)                                                                                                                                  
	END AS TesuRitu,                                                                                                                                                            
-- 付帯積込品名                                                                                                                                                                
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                                                                                                                              
		ELSE ISNULL(eTKD_FutTum11.FutTumNm, '')                                                                                                                                 
	END AS FutTumNm,                                                                                                                                                            
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                                                                                                                              
		ELSE ISNULL(eTKD_FutTum11.SeisanNm, '')                                                                                                                                 
	END AS SeisanNm,                                                                                                                                                            
-- 付帯積込品区分                                                                                                                                                              
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                                                                                                                              
		ELSE ISNULL(CAST(eTKD_FutTum11.FutTumKbn AS VARCHAR), '')                                                                                                               
	END AS FutTumKbn,                                                                                                                                                           
-- 数量                                                                                                                                                                        
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                                                                                                                              
		ELSE ISNULL(CAST(eTKD_FutTum11.Suryo AS VARCHAR), '')                                                                                                                   
	END AS Suryo,                                                                                                                                                               
-- 単価                                                                                                                                                                        
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                                                                                                                              
		ELSE ISNULL(CAST(eTKD_FutTum11.TanKa AS VARCHAR), '')                                                                                                                   
	END AS TanKa,                                                                                                                                                               
	CASE                                                                  
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                        
		ELSE ISNULL(CAST(eVPM_Seisan01.SeisanCd AS VARCHAR), '')          
	END AS SeisanCd,                                                      
-- 請求付帯種別名                                                                                                                                                              
	ISNULL(eVPM_CodeKb01.CodeKbnNm, '') AS SeiFutSyuNm,                                                                                                                         
-- 入金年月日                                                                                                                                                                  
	ISNULL(eTKD_NyShmi11.NyuSihYmd, '') AS NyuKinYmd,                                                                                                                           
-- 発生年月日                                                                                                                                                                  
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ''                                                                                                                              
		ELSE ISNULL(eTKD_FutTum11.HasYmd, '')                                                                                                                                   
	END AS HasYmd,                                                                                                                                                              
-- 発行年月日                                                                                                                                                                  
	ISNULL(eTKD_SeiMei11.SeiHatYmd, '') AS SeiHatYmd,                                                                                                                           
-- 入金区分                                                                                                                                                                    
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Yyksho01.NyuKinKbn, 0)                                                                                              
		ELSE ISNULL(eTKD_FutTum11.NyuKinKbn, 0)                                                                                                                                 
	END AS NyuKinKbn,                                                                                                                                                           
-- 入金クーポン区分                                                                                                                                                            
	CASE                                                                                                                                                                        
		WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN ISNULL(eTKD_Yyksho01.NCouKbn, 0)                                                                                                
		ELSE ISNULL(eTKD_FutTum11.NCouKbn, 0)                                                                                                                                   
	END AS NCouKbn,                                                                                                                                                             
-- 未収額                                                                                                                                                                      
	TKD_Mishum.SeiKin - TKD_Mishum.NyuKinRui AS MisyuG,                                                                                                                         
-- 請求先 使用開始年月日、使用終了年月日                                                                                                                                       
	ISNULL(eVPM_Tokisk02.SiyoStaYmd, 0) AS TSiyoStaYmd,                                                                                                                         
	ISNULL(eVPM_Tokisk02.SiyoEndYmd, 0) AS TSiyoEndYmd,                                                                                                                         
-- 請求先支店 使用開始年月日、使用終了年月日                                                                                                                                   
	ISNULL(eVPM_TokiSt02.SiyoStaYmd ,0      )   AS  SSiyoStaYmd,                                                                                                                
	ISNULL(eVPM_TokiSt02.SiyoEndYmd ,0      )   AS  SSiyoEndYmd,                                                                                                                
-- 予約車種台数                                                                                                                                                                
	CASE TKD_Mishum.SeiFutSyu                                                                                                                                                   
		WHEN 1 THEN ISNULL(CAST(eTKD_YykSyu.Sum_SyaSyuDai AS VARCHAR), '')                                                                                                                         
		ELSE ''                                                                                                                                                                  
	END AS Sum_SyaSyuDai,                                                                                                                                                       
-- 予約車種台数                                                                                                                                                                
	CASE TKD_Mishum.SeiFutSyu                                                                                                                                                   
		WHEN 1 THEN ISNULL(CAST(eTKD_YykSyu.Sum_SyaSyuTan AS VARCHAR), '')                                                                                                                         
		ELSE ''                                                                                                                                                                  
	END AS Sum_SyaSyuTan  
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
	AND TKD_Mishum.FutuUnkRen = eTKD_FutTum11.UnkRen                                                                                                                            
	AND TKD_Mishum.FutTumRen = eTKD_FutTum11.FutTumRen                                                                                                                          
	AND eTKD_FutTum11.SiyoKbn = 1                                                                                                                                               
	AND ((TKD_Mishum.SeiFutSyu = 6 AND eTKD_FutTum11.FutTumKbn = 2) OR (TKD_Mishum.SeiFutSyu <> 6 AND eTKD_FutTum11.FutTumKbn = 1))                                             
    -- 精算マスタ                                                       
LEFT JOIN VPM_Seisan AS eVPM_Seisan01                               
   ON eTKD_FutTum11.SeisanCdSeq = eVPM_Seisan01.SeisanCdSeq            
-- コード区分マスタ 請求付帯種別名                                                                                                                                             
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01                                                                                                                                          
	ON eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'                                                                                                                                      
	AND TKD_Mishum.SeiFutSyu = eVPM_CodeKb01.CodeKbn                                                                                                                            
AND eVPM_CodeKb01.TenantCdSeq = 0
-- 入金支払明細 受付番号・運行日連番・付帯積込品連番集計                                                                                                                         
LEFT JOIN eTKD_NyShmi01 AS eTKD_NyShmi11                                                                                                                                       
	ON  TKD_Mishum.UkeNo = eTKD_NyShmi11.UkeNo                                                                                                                                  
	AND TKD_Mishum.FutuUnkRen  = eTKD_NyShmi11.UnkRen                                                                                                                           
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
		GROUP BY UkeNo ) AS SUB                                                                                                                                                 
		ON SUB.UkeNo = TKD_YykSyu.UkeNo                                                                                                                                         
		AND SUB.Min_UnkRen = TKD_YykSyu.UnkRen                                                                                                                                  
		WHERE SiyoKbn = 1                                                                                                                                                       
		GROUP BY TKD_YykSyu.UkeNo ) AS eTKD_YykSyu                                                                                                                              
	ON TKD_Mishum.UkeNo = eTKD_YykSyu.UkeNo                                                                                                                                     
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
		AND (@BillTypes IS NULL OR TKD_Mishum.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillTypes, ',')))																												-- チェックした各種別
		
		ORDER BY
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN eVPM_Eigyos01.EigyoCd END),
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN eTKD_Yyksho01.SeiTaiYmd END),
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN eVPM_Tokisk02.TokuiCd END),
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN eVPM_TokiSt02.SitenCd END),
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN eVPM_TokiSt02.SeiCdSeq END),
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN TKD_Mishum.UkeNo END),
		 (CASE WHEN (@BillTypeOrderBy IS NOT NULL) THEN TKD_Mishum.FutuUnkRen END),

		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN eVPM_Eigyos01.EigyoCd END),
		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN eVPM_Tokisk02.TokuiCd END),
		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN eVPM_TokiSt02.SitenCd END),
		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN eVPM_TokiSt02.SeiCdSeq END),
		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN eTKD_Yyksho01.SeiTaiYmd END),
		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN TKD_Mishum.UkeNo END),
		 (CASE WHEN (@BillTypeOrderBy IS NULL) THEN TKD_Mishum.FutuUnkRen END)
		 OPTION (RECOMPILE)
RETURN
