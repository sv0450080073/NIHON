USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_VenderRequestMain_R]    Script Date: 2020/11/20 14:51:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER   PROCEDURE [dbo].[Pro_VenderRequestMain_R]
		@minUkeCd varchar(10),
		@maxUkeCd varchar(10),
		@startDate varchar(8),
		@endDate varchar(8),
		@branchId int,
		@reservationList nvarchar(MAX),
		@customerStart int,
		@customerEnd int,
		@customerSirStart int,
		@customerSirEnd int,
		@tenantId int
AS
BEGIN

SET NOCOUNT ON

SELECT 
	   CAST(1 AS BIT)					   AS 'IsMainReport'
	  ,YYKSHO.UkeNo            AS 'UkeNo'
	  ,UNKOBI.UnkRen           AS 'UnkRen'
	  ,YOUSHA.YouTblSeq																								
	  ,YYKSHO.TokuiTel         AS  'TokuiTel'																									
	  ,YYKSHO.TokuiTanNm        AS  'TokuiTanNm'																									
	  ,YYKSHO_TOKISK.TokuiNm   AS 'TokuiNm'																									
	  ,YYKSHO_TOKIST.SitenNm   AS 'TokuiSitenNm'																																																
	  ,YOU_TOKISK.TokuiNm      AS 'TokiskTokuiNm'																									
      ,YOU_TOKIST.SitenNm      AS 'TokistTokuiSitenNm'																										
	  ,YOU_TOKIST.FaxNo        AS 'YouskFax'																																																	
	  ,UNKOBI.HaiSYmd          AS  'HaisYmd' 																									
	  ,UNKOBI.TouYmd           AS  'TouYmd' 																									
	  ,UNKOBI.DanTaNm          AS  'DanTaNm'																									
	  ,UNKOBI.KanjJyus1        AS  'KanjJyus1'																									
      ,UNKOBI.KanjJyus2        AS  'KanjJyus2'																										
	  ,UNKOBI.KanjTel          AS  'KanjTel'																									
	  ,UNKOBI.KanJNm           AS  'KanjNm'																									
	  ,UNKOBI.IkNm             AS  'IkNm'																									
	  ,UNKOBI.HaiSBinNm        AS  'HaiSBinNm'																									
	  ,UNKOBI.HaiSKouKNm       AS  'HaiSKoukRyaku'																									
	  ,UNKOBI.HaiSSetTime      AS  'HaiSSetTime'																									
	  ,UNKOBI.HaiSTime         AS  'HaiSTime'																									
	  ,UNKOBI.SyuPaTime        AS  'SyuPaTime'																									
	  ,UNKOBI.HaiSNm           AS　'HaiSNm'																									
	  ,UNKOBI.HaiSJyus1        AS  'HaiSJyus1１'																									
	  ,UNKOBI.HaiSJyus2        AS  'HaiSJyus22'																									
	  ,UNKOBI.TouSKouKNm       AS  'TouChaKoukRyaku'																																																	
	  ,UNKOBI.TouSBinNm        AS  'TouChaBinNm'																									
	  ,UNKOBI.TouSetTime       AS  'TouSetTime'																									
	  ,UNKOBI.TouNm            AS  'TouNm'																									
	  ,UNKOBI.TouJyusyo1       AS  'TouJyus1'																									
	  ,UNKOBI.TouJyusyo2       AS  'TouJyus2'																									
	  ,UNKOBI.TouChTime        AS  'TouChTime'																									
	  ,UNKOBI.JyoSyaJin        AS 'JyoSyaJin'																									
	  ,UNKOBI.PlusJin          AS 'PlusJin'																									
	  ,UNKOBI.OthJinKbn1       AS 'OthJinKbn1Ryaku'																																																		
	  ,UNKOBI.OthJin1          AS 'OthJin1'																									
	  ,UNKOBI.OthJinKbn2       AS 'OthJinKbn2Ryaku'																																																	
	  ,UNKOBI.OthJin2          AS 'OthJin2'		
	  ,UNKOBI.BikoNm           AS 'Biko'	
	  ,UNKOBI.SijJoKbn1        AS 'SijJokbn1Ryaku'																																																																					
	  ,UNKOBI.SijJoKbn2        AS 'SijJokbn2Ryaku'																																															
	  ,UNKOBI.SijJoKbn3        AS 'SijJokbn3Ryaku'																																															
	  ,UNKOBI.SijJoKbn4        AS 'SijJokbn4Ryaku'																																																
	  ,UNKOBI.SijJoKbn5        AS 'SijJokbn5Ryaku'	
	  ,(SELECT SUM (SyaSyuDai) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1 
				AND UnkRen = YOUSHA.UnkRen) AS 'TotalBusRequired'																								
	  ,(SELECT SUM (SyaSyuDai) FROM TKD_YouSyu WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1 
				AND UnkRen = YOUSHA.UnkRen AND YouTblSeq = YOUSHA.YouTblSeq) AS 'TotalBusBorrow'
	  ,YOUSHA.ZeiKbn      AS 'YoushaZeiKbn'
      ,YOUSHA.SyaRyoUnc   AS 'SyaRyoUnc'																										
      ,YOUSHA.Zeiritsu   AS 'Zeiritsu'																										
      ,YOUSHA.SyaRyoSyo   AS 'SyaRyoSyo'																										
      ,YOUSHA.TesuRitu       AS 'TesRitu'																										
      ,YOUSHA.SyaRyoTes   AS 'SyaRyoTes'																										
	  ,(YOUSHA.SyaRyoUnc + YOUSHA.SyaRyoSyo) AS 'TotalPrice'																																																																										
      ,EIGYOS.EigyoNm   AS  'EigyoSNm'																										
	  ,EIGYOS.ZipCd     AS  'EigyoSZipCd'																								
	  ,EIGYOS.Jyus1     AS  'EigyoSJyuS1'																								
	  ,EIGYOS.Jyus2     AS  'EigyoSJyuS2'																								
	  ,EIGYOS.TelNo     AS  'EigyoSTel'																								
	  ,EIGYOS.FaxNo     AS  'EigyoSFax'																																											
FROM 
(SELECT * FROM TKD_Yousha y	WHERE y.SiyoKbn = 1) YOUSHA																						
INNER JOIN TKD_Yyksho AS YYKSHO																							
       ON YYKSHO.UkeNo = YOUSHA.UkeNo																							
	  AND YYKSHO.SiyoKbn = 1																						
	  AND YYKSHO.TenantCdSeq = 1                    --ログインユーザーのテナントコードSeq																						
	  																						
LEFT JOIN VPM_Eigyos AS EIGYOS 																							
       ON EIGYOS.EigyoCdSeq = YYKSHO.UkeEigCdSeq																							
	  AND EIGYOS.SiyoKbn = 1																						
																							
LEFT JOIN VPM_Compny AS COMPNY 																							
       ON COMPNY.EigyoCdSeq = EIGYOS.EigyoCdSeq																							
	  AND COMPNY.CompanyCdSeq  = EIGYOS.CompanyCdSeq																						
	  AND COMPNY.SiyoKbn = 1 																						
	  AND COMPNY.TenantCdSeq = 1                    --ログインユーザーのテナントコードSeq																						
LEFT JOIN TKD_Unkobi AS UNKOBI																							
       ON UNKOBI.UkeNo = YOUSHA.UkeNo																							
	  AND UNKOBI.UnkRen = YOUSHA.UnkRen																						
	  AND UNKOBI.SiyoKbn = 1																						
																							
LEFT JOIN VPM_YoyKbn AS YOYKBN																							
       ON YOYKBN.YoyaKbnSeq = YYKSHO.YoyaKbnSeq																							
   　 AND YOYKBN.SiyoKbn = 1																							
																							
LEFT JOIN VPM_Tokisk AS YOU_TOKISK																							
       ON YOU_TOKISK.TokuiSeq = YOUSHA.YouCdSeq																							
      AND YOU_TOKISK.TenantCdSeq = 1                    --ログインユーザーのテナントコードSeq																							
      AND YOU_TOKISK.SiyoStaYmd <= UNKOBI.HaiSYmd																							
      AND YOU_TOKISK.SiyoEndYmd >= UNKOBI.HaiSYmd																							
																							
LEFT JOIN VPM_TokiSt AS YOU_TOKIST 																							
       ON YOU_TOKIST.TokuiSeq = YOUSHA.YouCdSeq																							
	  AND YOU_TOKIST.SitenCdSeq = YOUSHA.YouSitCdSeq																						
	  AND YOU_TOKIST.SiyoStaYmd <= UNKOBI.HaiSYmd																						
      AND YOU_TOKIST.SiyoEndYmd >= UNKOBI.HaiSYmd																							
																							
LEFT JOIN VPM_Tokisk AS YYKSHO_TOKISK																							
       ON YYKSHO_TOKISK.TokuiSeq = YYKSHO.TokuiSeq																							
      AND YYKSHO_TOKISK.TenantCdSeq = 1                    --ログインユーザーのテナントコードSeq																							
      AND YYKSHO_TOKISK.SiyoStaYmd <= UNKOBI.HaiSYmd																							
      AND YYKSHO_TOKISK.SiyoEndYmd >= UNKOBI.HaiSYmd																							
	  
LEFT JOIN VPM_TokiSt AS YYKSHO_TOKIST 																							
       ON YYKSHO_TOKIST.TokuiSeq = YYKSHO.TokuiSeq																							
	  AND YYKSHO_TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq																						
	  AND YYKSHO_TOKIST.SiyoStaYmd <= UNKOBI.HaiSYmd																						
      AND YYKSHO_TOKIST.SiyoEndYmd >= UNKOBI.HaiSYmd

WHERE  
	YYKSHO.UkeCd BETWEEN @minUkeCd AND @maxUkeCd 
	AND YYKSHO.YoyaSyu = 1 
    AND YOYKBN.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@reservationList, '-'))              --画面の予約区分																							
    AND UNKOBI.HaiSYmd BETWEEN @startDate AND @endDate   --画面の配車年月日																							
    AND YYKSHO.UkeEigCdSeq = CASE WHEN @branchId = 0 THEN YYKSHO.UkeEigCdSeq ELSE @branchId END                         --画面の受付営業所																							
    AND ((@customerStart = 0 and @customerEnd = 0)
			or
			(@customerStart <> @customerEnd and YOUSHA.YouCdSeq = @customerStart and YOUSHA.YouSitCdSeq >= @customerSirStart)
			or
			(@customerStart <> @customerEnd and YOUSHA.YouCdSeq = @customerEnd and YOUSHA.YouSitCdSeq <= @customerSirEnd)
			or
			(@customerStart = @customerEnd and YOUSHA.YouCdSeq = @customerStart and YOUSHA.YouSitCdSeq >= @customerSirStart and YOUSHA.YouSitCdSeq <= @customerSirEnd)
			or
			(@customerStart = 0 and @customerEnd <> 0 and ((YOUSHA.YouCdSeq = @customerEnd and YOUSHA.YouSitCdSeq <= @customerSirEnd) or YOUSHA.YouCdSeq < @customerEnd))
			or
			(@customerEnd = 0 and @customerStart <> 0 and ((YOUSHA.YouCdSeq = @customerStart and YOUSHA.YouSitCdSeq >= @customerSirStart) or YOUSHA.YouCdSeq > @customerStart))
			or
			(YOUSHA.YouCdSeq < @customerEnd and YOUSHA.YouCdSeq > @customerStart))

END
GO


