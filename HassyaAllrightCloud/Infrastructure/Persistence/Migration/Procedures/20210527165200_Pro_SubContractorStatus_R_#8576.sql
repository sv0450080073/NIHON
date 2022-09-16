USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_SubContractorStatus_R]    Script Date: 6/8/2021 9:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER        PROC [dbo].[Pro_SubContractorStatus_R]
					@startDate varchar(8), @endDate varchar(8),
					@dateType int,
					@gyosyaFrom int, @gyosyaTo int, @tokuiFrom int, @tokuiTo int, @sitenFrom int, @sitenTo int, -- for you customer
					@bookingTypeFrom int, @bookingTypeTo int,
					@bookingTypes varchar(max),
					@companyIds varchar(max),
					@brandStart int, @brandEnd int,
					@ukeCdFrom varchar(10), @ukeCdTo varchar(10),
					@staffFrom int, @staffTo int,
					@jitaFlg int,
					@tenantId int,
					@isSearch bit,
					@group int, @outputOrder int,
					@itemPerPage int, @page int
AS
BEGIN
DECLARE @TotalRecord int = 0;
DECLARE @SearchReslutKey TABLE
(
	RowNum int NOT NULL PRIMARY KEY,
	UkeNo varchar(15),
	UnkRen int,
	YouTokuiSeq int,
	YouSitenCdSeq int
);

DECLARE @SummaryResults TABLE 
( 
    ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UkeNo char(15),
	UnkRen int,
	TotalRecords INT,

	TotalSyaRyoUnc INT,
	TotalZeiRui INT,
	TotalTesuRyoG INT,

	TotalGuideFee INT,
	TotalGuideTax INT,
	TotalUnitGuiderFee INT,

	TotalIncidentalFee INT,
	TotalIncidentalTax INT,
	TotalIncidentalCharge INT,

	TotalYoushaUnc INT,
	TotalYoushaSyo INT,
	TotalYoushaTes INT,

	TotalYouFutTumGuiKin INT,
	TotalYouFutTumGuiTax INT,
	TotalYouFutTumGuiTes INT,

	TotalYouFutTumKin INT,
	TotalYouFutTumTax INT,
	TotalYouFutTumTes INT
);

---------------------------------------------GET Search Result Key ---------------------------------------------------------
INSERT INTO @SearchReslutKey(RowNum, UkeNo, UnkRen, YouTokuiSeq, YouSitenCdSeq)
SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS 'RowNum', *
	FROM(
	SELECT  YOUSHA.UkeNo, YOUSHA.UnkRen																				
			, YOU_TOKISK.TokuiSeq AS 'TokuiSeq'
			,YOU_TOKIST.SitenCdSeq AS 'SitenCdSeq'
	FROM TKD_Yousha AS YOUSHA																												
	INNER JOIN TKD_Yyksho AS YY_YYKSHO 																												
	ON YY_YYKSHO.UkeNo = YOUSHA.UkeNo																												
	AND YY_YYKSHO.YoyaSyu = 1  　　　　              --予約																												
	AND YY_YYKSHO.SiyoKbn = 1 																												
	AND YY_YYKSHO.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq																												
																													
	INNER JOIN TKD_Haisha AS HAISHA 																												
	ON  HAISHA.UkeNo = YOUSHA.UkeNo																												
	AND HAISHA.UnkRen = YOUSHA.UnkRen																												
	AND HAISHA.YouTblSeq = YOUSHA.YouTblSeq
	AND HAISHA.YouTblSeq > 0
	AND HAISHA.SiyoKbn = 1	
	
	INNER JOIN TKD_Unkobi AS UNKOBI																												
	ON UNKOBI.UkeNo = YOUSHA.UkeNo 																												
	AND UNKOBI.UnkRen = YOUSHA.UnkRen																												
	AND UNKOBI.SiyoKbn = 1	
																													
	------------------------------傭車地域------------------------------------																												
																													
	LEFT JOIN VPM_Tokisk AS YOU_TOKISK 																												
	ON YOU_TOKISK.TokuiSeq = YOUSHA.YouCdSeq																												
	   AND YOU_TOKISK.SiyoStaYmd <= YOUSHA.HasYmd 																												
	   AND  YOU_TOKISK.SiyoEndYmd >= YOUSHA.HasYmd																												
	   AND YOU_TOKISK.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq																												
																													
	LEFT JOIN VPM_TokiSt AS YOU_TOKIST																												
	 ON YOU_TOKIST.TokuiSeq = YOUSHA.YouCdSeq 																												
	AND YOU_TOKIST.SitenCdSeq = YOUSHA.YouSitCdSeq 																												
	AND YOU_TOKIST.SiyoStaYmd <= YOUSHA.HasYmd																												
	AND YOU_TOKIST.SiyoEndYmd >= YOUSHA.HasYmd																												
																																																																																																		
	LEFT JOIN VPM_Eigyos AS EIGYOS																												
	ON YY_YYKSHO.UkeEigCdSeq = EIGYOS.EigyoCdSeq																												
	AND EIGYOS.SiyoKbn = 1																												
																												
	LEFT JOIN VPM_Compny AS COMPANY 																												
	ON EIGYOS.CompanyCdSeq = COMPANY.CompanyCdSeq																												
	AND COMPANY.SiyoKbn = 1																												
	AND COMPANY.TenantCdSeq = @tenantId                     --ログインユーザーのテナントコードSeq																												
																													
	LEFT JOIN VPM_YoyKbn AS YOU_YOYKBN 																												
	ON YOU_YOYKBN.TenantCdSeq = @tenantId  
	AND YOU_YOYKBN.YoyaKbnSeq = YY_YYKSHO.YoyaKbnSeq 																												
	AND YOU_YOYKBN.SiyoKbn = 1				
	
	LEFT JOIN VPM_Gyosya AS YOU_GYOSYA 																												
    ON YOU_GYOSYA.GyosyaCdSeq = YOU_TOKISK.GyosyaCdSeq																												
    AND YOU_GYOSYA.SiyoKbn = 1				
    AND YOU_GYOSYA.TenantCdSeq = YOU_TOKISK.TenantCdSeq
																													
	LEFT JOIN VPM_Syain AS SYAIN																												
	ON SYAIN.SyainCdSeq = YY_YYKSHO.EigTanCdSeq
	WHERE
		YY_YYKSHO.UkeCD  >= @ukeCdFrom         --画面で受付番号項目でのFromの番号																												
	 AND YY_YYKSHO.UkeCD <= @ukeCdTo			--画面で受付番号項目でのToの番号																									
	 AND YOUSHA.SiyoKbn = 1																											
	 AND YOUSHA.JitaFlg = CASE WHEN @jitaFlg = 0 THEN YOUSHA.JitaFlg ELSE 0 END			--画面で自他社区分項目での未出力を選択した											
	 AND( (@dateType = 1 AND HAISHA.HaiSYmd >= @startDate AND HAISHA.HaiSYmd <= @endDate)		--画面 配車年月日:  画面で年月日項目でのFromの番号/ 画面で年月日項目でのToの番号
	 OR (@dateType = 2 AND HAISHA.TouYmd >= @startDate AND HAISHA.TouYmd <= @endDate))			--画面 到着年月日:  画面で年月日項目でのFromの番号/ 画面で年月日項目でのToの番号
	 AND YOU_YOYKBN.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))	--画面で予約区分項目でのFromの番号										 
	 AND COMPANY.CompanyCdSeq IN (SELECT * FROM FN_SplitString(@companyIds, '-'))	--画面でログイン会社のコードSEQ
	 AND EIGYOS.EigyoCd >= @brandStart			--画面で受付営業所コード項目でのFromの番号											
	 AND EIGYOS.EigyoCd <= @brandEnd			--画面で受付営業所コード項目でのToの番号
	 --AND ((@tokuiFrom = 0 and @tokuiTo = 0)
		--	or
		--	(@tokuiFrom <> @tokuiTo and YOU_TOKISK.TokuiCd = @tokuiFrom and YOU_TOKIST.SitenCd >= @sitenFrom)
		--	or
		--	(@tokuiFrom <> @tokuiTo and YOU_TOKISK.TokuiCd = @tokuiTo and YOU_TOKIST.SitenCd <= @sitenTo)
		--	or
		--	(@tokuiFrom = @tokuiTo and YOU_TOKISK.TokuiCd = @tokuiFrom and YOU_TOKIST.SitenCd >= @sitenFrom and YOU_TOKIST.SitenCd <= @sitenTo)
		--	or
		--	(@tokuiFrom = 0 and @tokuiTo <> 0 and ((YOU_TOKISK.TokuiCd = @tokuiTo and YOU_TOKIST.SitenCd <= @sitenTo) or YOU_TOKISK.TokuiCd < @tokuiTo))
		--	or
		--	(@tokuiTo = 0 and @tokuiFrom <> 0 and ((YOU_TOKISK.TokuiCd = @tokuiFrom and YOU_TOKIST.SitenCd >= @sitenFrom) or YOU_TOKISK.TokuiCd > @tokuiFrom))
		--	or
		--	(YOU_TOKISK.TokuiCd < @tokuiTo and YOU_TOKISK.TokuiCd > @tokuiFrom))
	 AND (FORMAT(YOU_GYOSYA.GyosyaCd,'000') + FORMAT(YOU_TOKISK.TokuiCd,'0000') + FORMAT(YOU_TOKIST.SitenCd,'0000')) >= (FORMAT(@gyosyaFrom,'000') + FORMAT(@tokuiFrom,'0000') + FORMAT(@sitenFrom,'0000')) 																																				
	 AND (FORMAT(YOU_GYOSYA.GyosyaCd,'000') + FORMAT(YOU_TOKISK.TokuiCd,'0000') + FORMAT(YOU_TOKIST.SitenCd,'0000')) <= (FORMAT(@gyosyaTo,'000') + FORMAT(@tokuiTo,'0000') + FORMAT(@sitenTo,'0000'))	
	 AND SYAIN.SyainCd >= @staffFrom																											
	 AND SYAIN.SyainCd <= @staffTo			
	ORDER BY	
		CASE WHEN (@isSearch = 1 AND @outputOrder = 0) THEN UNKOBI.HaiSYmd END,																									
		CASE WHEN (@isSearch = 1 AND @outputOrder = 0) THEN UNKOBI.TouYmd END,	
		CASE WHEN (@isSearch = 1 AND @outputOrder = 1) THEN YOU_TOKISK.TokuiCd END,																									
		CASE WHEN (@isSearch = 1 AND @outputOrder = 1) THEN UNKOBI.TouYmd  END,
		YOUSHA.UkeNo ,																												
        YOUSHA.UnkRen ,																												
        YOUSHA.YouTblSeq
		OFFSET 0 ROWS
	) AS singleRC
	GROUP BY singleRC.UkeNo, singleRC.UnkRen, singleRC.TokuiSeq, singleRC.SitenCdSeq

---------------------------------------------GET Summary Result Datas-------------------------------------------------------

IF(@isSearch = 1)
BEGIN 
	INSERT INTO @SummaryResults(UkeNo, UnkRen, TotalSyaRyoUnc, TotalZeiRui, TotalTesuRyoG, TotalGuideFee, TotalGuideTax, TotalUnitGuiderFee,
	TotalIncidentalFee, TotalIncidentalTax, TotalIncidentalCharge, TotalYoushaUnc, TotalYoushaSyo, TotalYoushaTes, TotalYouFutTumGuiKin, TotalYouFutTumGuiTax,
	TotalYouFutTumGuiTes, TotalYouFutTumKin, TotalYouFutTumTax, TotalYouFutTumTes)
	SELECT *
	FROM(
	SELECT  YOUSHA.UkeNo, YOUSHA.UnkRen																				
	       ,(SELECT SUM (CAST(IsNull(TKD_YykSyu.SyaRyoUnc, 0) AS BIGINT)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo  AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1 																												
	             AND UnkRen = YOUSHA.UnkRen)			AS 'TotalSyaRyoUnc' --'運賃'																																																		
	       ,(SELECT SUM (CAST(IsNull(TKD_Yyksho.ZeiRui, 0) AS BIGINT)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1) 		AS 'TotalZeiRui'	--'消費税'																																								
	       ,(SELECT SUM (CAST(IsNull(TKD_Yyksho.TesuRyoG, 0) AS BIGINT)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1)		AS 'TotalTesuRyoG'	--'手数料' 
		   ,SumGuideTax.Fee　 AS 'TotalGuideFee'　		--'ガイド手数料'														
		   ,SumGuideTax.Tax	AS 'TotalGuideTax'			--'ガイド消費税	
			,(SELECT SUM(CAST(IsNull(TKD_YykSyu.UnitGuiderFee, 0) AS BIGINT)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1)	AS 'TotalUnitGuiderFee'		--'ガイド料'
			
			,(ISNULL(SumFutTum.SumUriGakKin, 0) + ISNULL(SumTumi.SumUriGakKin, 0))   AS 'TotalIncidentalFee'		--'付帯・積込品'																												
			,(ISNULL(SumFutTum.SumSyaRyoSyo, 0) + ISNULL(SumTumi.SumSyaRyoSyo, 0))   AS 'TotalIncidentalTax'		--'付帯・積込品_消費税' 																												
			,(ISNULL(SumFutTum.SumSyaRyoTes, 0) + ISNULL(SumTumi.SumSyaRyoTes, 0))   AS 'TotalIncidentalCharge'	--'付帯・積込品_手数料'
			
			,IsNull(HAISHA.YoushaUnc, 0)					AS 'TotalYoushaUnc'		--'傭車_運賃'
			,IsNull(HAISHA.YoushaSyo, 0)					AS 'TotalYoushaSyo'	--'傭車_消費税'
			,IsNull(HAISHA.YoushaTes, 0)					AS 'TotalYoushaTes'	--'傭車_手数料'
			,ISNULL(YOU_SumYMFuTuGui.SumHaseiKin, 0)		AS 'TotalYouFutTumGuiKin'	--'傭車_ガイド料'  																											
			,IsNull(YOU_SumYMFuTuGui.SumSyaRyoSyo, 0)	AS 'TotalYouFutTumGuiTax'	--'傭車_ガイド_消費税' 																											
			,IsNull(YOU_SumYMFuTuGui.SumSyaRyoTes, 0)	AS 'TotalYouFutTumGuiTes'	--'傭車_ガイド_手数料' 		
			,IsNull(YOU_SumYMFuTu.SumHaseiKin, 0)		AS 'TotalYouFutTumKin'		--'傭車_付帯・積込品' 																										
			,IsNull(YOU_SumYMFuTu.SumSyaRyoSyo, 0)  		AS 'TotalYouFutTumTax'		--'傭車_付帯・積込品_消費税'  																											
			,IsNull(YOU_SumYMFuTu.SumSyaRyoTes, 0)   	AS 'TotalYouFutTumTes'		--'傭車_付帯・積込品_手数料'
	FROM @SearchReslutKey search
																												
	INNER JOIN  TKD_Yousha AS YOUSHA
	ON YOUSHA.UkeNo = search.UkeNo
	AND YOUSHA.UnkRen = search.UnkRen
	AND YOUSHA.YouCdSeq = search.YouTokuiSeq
	AND YOUSHA.YouSitCdSeq = search.YouSitenCdSeq
																									
	INNER JOIN TKD_Haisha AS HAISHA 																												
	ON  HAISHA.UkeNo = YOUSHA.UkeNo																												
	AND HAISHA.UnkRen = YOUSHA.UnkRen																												
	AND HAISHA.YouTblSeq = YOUSHA.YouTblSeq
	AND HAISHA.YouTblSeq > 0
	AND HAISHA.SiyoKbn = 1	
	----------------------------ガイド--------------------
	LEFT JOIN 
	(
	    SELECT m.UkeNo, m.UnkRen, ISNULL(SUM(CAST(GuiderTax AS BIGINT)), 0) as Tax, ISNULL(SUM(CAST(GuiderFee AS BIGINT)), 0) as Fee from
			 (SELECT TKD_Yyksho.UkeNo, TKD_YykSyu.UnkRen, TKD_YykSyu.UnitGuiderFee * TKD_Yyksho.FeeGuiderRate/100 AS GuiderFee, 
				CASE																											
			     WHEN TKD_Yyksho.TaxTypeforGuider = 1 THEN (TKD_YykSyu.UnitGuiderFee * TKD_Yyksho.Zeiritsu)/100  																										
			     WHEN TKD_Yyksho.TaxTypeforGuider = 2 THEN (TKD_YykSyu.UnitGuiderFee * TKD_Yyksho.Zeiritsu)/(100+TKD_Yyksho.Zeiritsu) 																										
			     ELSE 0
				 END AS GuiderTax
			 FROM TKD_Yyksho INNER JOIN TKD_YykSyu 
			 ON 
			  TKD_Yyksho.UkeNo = TKD_YykSyu.UkeNo
			 AND TKD_Yyksho.SiyoKbn = 1
			 AND TKD_Yyksho.YoyaSyu = 1
			 AND TKD_Yyksho.TenantCdSeq = @tenantId) as m
			 
			 GROUP BY m.UkeNo, m.UnkRen) AS SumGuideTax 
			 ON YOUSHA.UkeNo = SumGuideTax.UkeNo
			 AND YOUSHA.UnkRen = SumGuideTax.UnkRen
	
	----------------------------付帯取得-------------------------																												
	LEFT JOIN																												
	  (SELECT UkeNo ,																												
	          UnkRen ,																												
	          SUM(CAST(UriGakKin AS BIGINT)) AS SumUriGakKin ,																												
	          SUM(CAST(SyaRyoSyo AS BIGINT)) AS SumSyaRyoSyo ,																												
	          SUM(CAST(SyaRyoTes AS BIGINT)) AS SumSyaRyoTes																												
	   FROM TKD_FutTum																												
	    LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_FutTum.FutTumCdSeq　																												
		      AND VPM_Futai.TenantCdSeq = @tenantId --ログインユーザーのテナントコードSeq																											
	   WHERE TKD_FutTum.SiyoKbn = 1 																												
	     AND TKD_FutTum.FutTumKbn = 1   --付帯金額のみ																												
	     AND VPM_Futai.FutGuiKbn <> 5																												
	   GROUP BY UkeNo ,																												
	            UnkRen) AS SumFutTum 																												
				ON YOUSHA.UkeNo = SumFutTum.UkeNo																									
	           AND YOUSHA.UnkRen = SumFutTum.UnkRen																												
																													
	----------------------------積み込み取得-------------------------																												
	LEFT JOIN																												
	  (SELECT UkeNo ,																												
	          UnkRen ,																												
	          SUM(CAST(UriGakKin AS BIGINT)) AS SumUriGakKin ,																												
	          SUM(CAST(SyaRyoSyo AS BIGINT)) AS SumSyaRyoSyo ,																												
	          SUM(CAST(SyaRyoTes AS BIGINT)) AS SumSyaRyoTes																												
	   FROM TKD_FutTum																												
	   WHERE TKD_FutTum.SiyoKbn = 1 																												
	     AND TKD_FutTum.FutTumKbn = 2   --積み込み金額のみ																												
	   GROUP BY UkeNo ,																												
	            UnkRen) AS SumTumi																												
				ON YOUSHA.UkeNo = SumTumi.UkeNo																									
	           AND YOUSHA.UnkRen = SumTumi.UnkRen																												
																																			
	-----------------------傭車付帯・積込品取得--------------------------------																												
	LEFT JOIN
	(SELECT cc.UkeNo, cc.UnkRen
			,SUM(CAST(cc.SumHaseiKin AS BIGINT)) AS SumHaseiKin
			,SUM(CAST(cc.SumSyaryoSyo AS BIGINT)) AS SumSyaryoSyo
			,SUM(CAST(cc.SumSyaryoTes AS BIGINT)) AS SumSyaryoTes 
	FROM(SELECT UkeNo ,																												
	          UnkRen ,																												
	          YouTblSeq ,																												
	          SUM(CAST(HaseiKin AS BIGINT)) AS SumHaseiKin ,																												
	          SUM(CAST(SyaRyoSyo AS BIGINT)) AS SumSyaryoSyo ,																												
	          SUM(CAST(SyaRyoTes AS BIGINT)) AS SumSyaryoTes																												
	   FROM TKD_YFutTu																												
	   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_YFutTu.FutTumCdSeq																												
	   WHERE TKD_YFutTu.SiyoKbn = 1																												
	   AND VPM_Futai.FutGuiKbn <> 5																												
	   GROUP BY UkeNo ,																												
	            UnkRen ,																												
	            YouTblSeq) AS cc 
	GROUP BY cc.UkeNo, cc.UnkRen )AS YOU_SumYMFuTu 
	ON YOUSHA.UkeNo=YOU_SumYMFuTu.UkeNo																												
	AND YOUSHA.UnkRen=YOU_SumYMFuTu.UnkRen																												
																																																					
	LEFT JOIN																												
	(SELECT cc.UkeNo, cc.UnkRen, 
			 SUM(CAST(cc.SumHaseiKin AS BIGINT)) AS SumHaseiKin,
			 SUM(CAST(cc.SumSyaryoSyo AS BIGINT)) AS SumSyaryoSyo,
			 SUM(CAST(cc.SumSyaryoTes AS BIGINT)) AS SumSyaryoTes 
	FROM(SELECT UkeNo ,																												
			       UnkRen ,																												
			       YouTblSeq ,																												
			       SUM(CAST(HaseiKin AS BIGINT)) AS SumHaseiKin ,																												
			       SUM(CAST(SyaRyoSyo AS BIGINT)) AS SumSyaryoSyo ,																												
			       SUM(CAST(SyaRyoTes AS BIGINT)) AS SumSyaryoTes																												
			FROM TKD_YFutTu																												
			LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_YFutTu.FutTumCdSeq																												
			AND VPM_Futai.SiyoKbn=1																												
			WHERE TKD_YFutTu.SiyoKbn = 1																												
			  AND VPM_Futai.FutGuiKbn = 5																												
			GROUP BY UkeNo ,																												
			         UnkRen ,																												
			         YouTblSeq) AS cc
	GROUP BY cc.UkeNo, cc.UnkRen) AS YOU_SumYMFuTuGui 
				ON YOUSHA.UkeNo=YOU_SumYMFuTuGui.UkeNo																												
	            AND YOUSHA.UnkRen=YOU_SumYMFuTuGui.UnkRen																																																						
	------------------------------------------------------------------------------------------------------------------																																																				
	) AS singleRC
	
	SELECT @TotalRecord = COUNT(0) FROM @SearchReslutKey
	DELETE FROM @SearchReslutKey WHERE RowNum <= ((@page - 1) * @itemPerPage) OR RowNum > (@page * @itemPerPage)

	SELECT 
		@TotalRecord				  AS	'TotalRecords'
		,SUM(CAST(di.TotalSyaRyoUnc AS BIGINT))		  AS	'TotalSyaRyoUnc'
		,SUM(CAST(di.TotalZeiRui AS BIGINT))		  AS	'TotalZeiRui'
		,SUM(CAST(di.TotalTesuRyoG AS BIGINT))		  AS	'TotalTesuRyoG'
		,SUM(CAST(di.TotalGuideFee AS BIGINT))		  AS	'TotalGuideFee'
		,SUM(CAST(di.TotalGuideTax AS BIGINT))		  AS	'TotalGuideTax'
		,SUM(CAST(di.TotalUnitGuiderFee AS BIGINT))	  AS	'TotalUnitGuiderFee'
		,SUM(CAST(di.TotalIncidentalFee AS BIGINT))	  AS	'TotalIncidentalFee'
		,SUM(CAST(di.TotalIncidentalTax AS BIGINT))	  AS	'TotalIncidentalTax'
		,SUM(CAST(di.TotalIncidentalCharge AS BIGINT))AS	'TotalIncidentalCharge'
		FROM 
		(SELECT DISTINCT s.UkeNo, s.UnkRen 
		,s.TotalSyaRyoUnc, s.TotalZeiRui, s.TotalTesuRyoG
		, s.TotalGuideFee, s.TotalGuideTax, s.TotalUnitGuiderFee
		, s.TotalIncidentalFee, s.TotalIncidentalTax, s.TotalIncidentalCharge 
		FROM @SummaryResults s) AS di

	DECLARE @TotalYoushaUnc BIGINT;
	DECLARE @TotalYoushaSyo BIGINT;	
	DECLARE @TotalYoushaTes BIGINT;	

	SELECT @TotalYoushaUnc = SUM(CAST(TotalYoushaUnc AS BIGINT))
	,@TotalYoushaSyo = SUM(CAST(TotalYoushaSyo AS BIGINT))
	,@TotalYoushaTes = SUM(CAST(TotalYoushaTes AS BIGINT))
	FROM @SummaryResults

	SELECT 
	 @TotalYoushaUnc	AS	'TotalYoushaUnc'
	,@TotalYoushaSyo	AS	'TotalYoushaSyo'
	,@TotalYoushaTes	AS	'TotalYoushaTes'
	,SUM(CAST(di.TotalYouFutTumGuiKin AS BIGINT))	AS	'TotalYouFutTumGuiKin'
	,SUM(CAST(di.TotalYouFutTumGuiTax AS BIGINT))	AS	'TotalYouFutTumGuiTax'
	,SUM(CAST(di.TotalYouFutTumGuiTes AS BIGINT))	AS	'TotalYouFutTumGuiTes'
	,SUM(CAST(di.TotalYouFutTumKin AS BIGINT))		AS	'TotalYouFutTumKin' 
	,SUM(CAST(di.TotalYouFutTumTax AS BIGINT))		AS	'TotalYouFutTumTax'
	,SUM(CAST(di.TotalYouFutTumTes AS BIGINT))		AS	'TotalYouFutTumTes'
	FROM (SELECT DISTINCT s.UkeNo, s.UnkRen 
		,s.TotalYouFutTumGuiKin, s.TotalYouFutTumGuiTax, s.TotalYouFutTumGuiTes
		, s.TotalYouFutTumKin, s.TotalYouFutTumTax, s.TotalYouFutTumTes
		FROM @SummaryResults s) AS di
END
---------------------------------------------GET Result-------------------------------------------------------
SELECT YOUSHA.UkeNo               AS 'UkeNo' --'傭車_受付番号'																												
	   ,YY_YYKSHO.UkeCd           AS 'UkeCd'					--'受付コード'																											
       ,YOUSHA.UnkRen             AS 'UnkRen' --'傭車_運行日連番'																																																						
	   ,UNKOBI.HaiSYmd            AS 'HaiSYmd'		--'運行日_配車年月日'																											
	   ,UNKOBI.TouYmd             AS 'TouYmd'		--'運行日_到着年月日'																											
	   ,YY_Tokisk.TokuiCd         AS 'SkTokuiCd' 		--'得意先コード'																											
       ,YY_Tokisk.TokuiNm         AS 'TokuiNm'		--'得意先名' 																												
       ,YY_Tokisk.RyakuNm         AS 'TokiskRyakuNm' 		--'得意先_略名'																												
       ,YY_TokiSt.SitenCd         AS 'StSitenCd' 		--'支店コード '																												
       ,YY_TokiSt.SitenNm         AS 'SitenNm'		--'支店名' 																												
       ,YY_TokiSt.RyakuNm         AS 'TokiStRyakuNm' 		--'支店_略名'	   																																																				
	   ,YY_YYKSHO.TokuiTanNm      AS 'TokuiTanNm'		--'予約_得意担当者' 																											
       ,YY_YYKSHO.TokuiTel        AS 'TokuiTel'		--'予約_得意電話番号'																																																						
	   ,UNKOBI.DanTaNm            AS 'DanTaNm'		--'運行日_団体名' 																											
       ,UNKOBI.IkNm               AS 'IkNm'		--'運行日_行き先' 																												
       ,UNKOBI.HaiSTime			  AS 'U_HaiSTime' --'運行日_配車時間' 																									
       ,UNKOBI.HaiSNm			  AS 'U_HaiSNm' --'運行日_配車地名'																									
	   ,UNKOBI.HaiSSetTime		  AS 'U_HaiSSetTime' --'運行日_配車地接続時間' 																																																			
	   ,UNKOBI.HaiSKouKNm         AS 'U_HaiSKouKNm' --'配車地交通機関名' 																																																			
       ,UNKOBI.HaiSBinNm		  AS 'U_HaiSBinNm' --'配車地便名' 																																																		
	   ,UNKOBI.TouChTime		  AS 'U_TouChTime' --'運行日_到着時間'  																									
	   ,UNKOBI.TouNm			  AS 'U_TouNm' --'運行日_到着地名'																								
	   ,UNKOBI.TouSetTime		  AS 'U_TouSetTime' --'運行日_到着接続時間'																																																		
       ,UNKOBI.TouSKouKNm		  AS 'U_TouSKouKNm' --'到着交通_交通名'																																																						
       ,UNKOBI.TouSBinNm		  AS 'U_TouSBinNm'--'到着便_便名'																																																			
       ,UNKOBI.JyoSyaJin          AS 'U_JyoSyaJin'--'乗車人' 																												
       ,UNKOBI.PlusJin			  AS 'U_PlusJin'--'プラス人員'																																																		
																												
	   ,(SELECT SUM (ISNULL(SyaSyuDai, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo 																											
             AND SiyoKbn = 1 AND UnkRen = YOUSHA.UnkRen) AS 'TotalNumber'		--'予約_車種台数'																												
			 																									
       ,(SELECT SUM (IsNull(TKD_YykSyu.SyaRyoUnc, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo  AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1 																												
	             AND UnkRen = YOUSHA.UnkRen)                 AS 'SumSyaRyoUnc'		--'運賃'																												
	    ,YY_YYKSHO.Zeiritsu     AS 'Zeiritsu'		--'税率' 																											
       ,(SELECT SUM (IsNull(TKD_Yyksho.ZeiRui, 0)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1) AS 'SumZeiRui'		--'消費税'														
       ,YY_YYKSHO.TesuRitu                               AS 'TesuRitu'		--'手数率'																												
       ,(SELECT SUM (IsNull(TKD_Yyksho.TesuRyoG, 0)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1)                 																												
														 AS 'SumTesuRyoG'		--'手数料' 														
																												
	   --,(SELECT SUM (IsNull(TKD_YykSyu.SyaRyoUnc, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo  AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1 																												
	   --          AND UnkRen = YOUSHA.UnkRen)  + 																												
	   --(SELECT SUM (IsNull(TKD_Yyksho.ZeiRui, 0)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1)           																											
	   -- 												 AS 'SumTicket'					--- cal in code						 
　　　　,SumGuideTax.Fee　 AS 'SumGuideFee'		--'ガイド手数料'																												
　　　　,SumGuideTax.Tax	  AS 'SumGuideTax'			--'ガイド消費税'																											
	　　,(SELECT SUM(IsNull(TKD_YykSyu.UnitGuiderFee, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1)	AS 'SumUnitGuiderFee'		--'ガイド料'																											
																												
																												
       ,ISNULL(SumFutTum.SumUriGakKin, 0) + ISNULL(SumTumi.SumUriGakKin, 0)   AS 'IncidentalFee'		--'付帯・積込品'																												
       ,ISNULL(SumFutTum.SumSyaRyoSyo, 0) + ISNULL(SumTumi.SumSyaRyoSyo, 0)   AS 'IncidentalTax'		--'付帯・積込品_消費税' 																												
       ,ISNULL(SumFutTum.SumSyaRyoTes, 0) + ISNULL(SumTumi.SumSyaRyoTes, 0)   AS 'IncidentalCharge'	--'付帯・積込品_手数料'																												
	   ,ISNULL(SumFutTum.SumUriGakKin, 0) + ISNULL(SumTumi.SumUriGakKin, 0) + ISNULL(SumFutTum.SumSyaRyoSyo, 0) + ISNULL(SumTumi.SumSyaRyoSyo, 0)																											
	   AS 'TotalIncidental'	--'付帯合計'																											
																												
------------------------------傭車------------------------------------																												
       ,YOU_TOKISK.TokuiSeq             AS 'YouTokuiSeq'		--'傭車_得意先SEQ' 																																																
       ,YOU_TOKISK.TokuiNm				AS 'YouSkTokuiNm'		--'傭車_得意先名' 																								
       ,YOU_TOKISK.RyakuNm				AS	'YouSkRyakuNm'	--'傭車_得意先_略名' 																								
       ,YOU_TOKIST.SitenCdSeq			AS 'YouSitenCdSeq'	--'傭車_得意先支店SEQ' 																																																
       ,YOU_TOKIST.SitenNm				AS 'YouStSitenNm'		--'傭車_得意先支店名' 																								
       ,YOU_TOKIST.RyakuNm				AS 'YouStRyakuNm'	--'傭車_得意先支店_略名'																																													
       ,HAISHA.GoSya					AS 'HAISHA_GoSya'			--'傭車_号車'																																														
       ,HAISHA.HaiSYmd					AS 'H_HaiSYmd'				--'傭車_日程_配車年月日' 																							
       ,HAISHA.TouYmd					AS 'H_TouYmd'				--'傭車_日程_到着年月日' 																							
       ,HAISHA.HaiSTime					AS 'H_HaiSTime'				--'傭車_日程_配車時間'																							
       ,HAISHA.HaiSNm					AS 'H_HaiSNm'				--'傭車_配車地名'																							
	   ,HAISHA.HaiSSetTime				AS 'H_HaiSSetTime'			--'傭車_配車接続時間'																							
	   ,HAISHA.JyoSyaJin				AS 'JyoSyaJin'	--'傭車_配車乗車人員'																							
	   ,HAISHA.PlusJin					AS 'PlusJin'	--'傭車_配車プラス人員'																																													
	   ,HAISHA.HaiSKouKNm			    AS 'H_HaiSKouKNm'			--'傭車_交通名' 																																														
       ,HAISHA.HaiSBinNm				AS 'H_HaiSBinNm'			--'傭車_便名' 																								
	   ,HAISHA.TouChTime			    AS 'H_TouChTime'			--'傭車_到着時間'																								
	   ,HAISHA.TouNm					AS 'H_TouNm'				--'傭車_到着地名'																						
	   ,HAISHA.TouSetTime		        AS 'H_TouSetTime'			--'傭車_到着接続時間'																																																
	   ,HAISHA.TouSKouKNm				AS 'H_TouSKouKNm'			--'傭車_到着交通名'																																													
	   ,HAISHA.TouBinNm					AS 'H_TouBinNm'				--'傭車_到着便_便名'																																											
       ,HAISHA.YoushaUnc				AS 'YoushaUnc'	--'傭車_運賃'																																															
       ,YOUSHA.Zeiritsu					AS 'YouZeiritsu'	--'傭車_税率'																							
       ,HAISHA.YoushaSyo				AS 'YoushaSyo'	--'傭車_消費税'																								
	   ,YOUSHA.TesuRitu					AS 'YouTesuRitu'	--'傭車_手数率'																						
       ,HAISHA.YoushaTes				AS 'YoushaTes'	--'傭車_手数料' 																																																		
       ,ISNULL(YOU_SumYMFuTuGui.SumHaseiKin, 0)	AS 'YouFutTumGuiKin'	--'傭車_ガイド料'  																											
       ,ISNULL(YOU_SumYMFuTuGui.SumSyaRyoSyo, 0)	AS 'YouFutTumGuiTax'	--'傭車_ガイド_消費税' 																											
       ,ISNULL(YOU_SumYMFuTuGui.SumSyaRyoTes, 0)	AS 'YouFutTumGuiTes'	--'傭車_ガイド_手数料' 		   																									
	   ,ISNULL(YOU_SumYMFuTuGui.SumHaseiKin, 0)　+ ISNULL(YOU_SumYMFuTuGui.SumSyaRyoSyo, 0)	AS 'TotalYouFutTumGui'			--AS '傭車_ガイド_合計' 																							
																												
       ,ISNULL(YOU_SumYMFuTu.SumHaseiKin, 0)		AS 'YouFutTumKin'		--'傭車_付帯・積込品' 																										
       ,ISNULL(YOU_SumYMFuTu.SumSyaRyoSyo, 0)  	AS 'YouFutTumTax'		--'傭車_付帯・積込品_消費税'  																											
       ,ISNULL(YOU_SumYMFuTu.SumSyaRyoTes, 0)   	AS 'YouFutTumTes'		--'傭車_付帯・積込品_手数料'  																											
	   ,ISNULL(YOU_SumYMFuTu.SumHaseiKin, 0)  + ISNULL(YOU_SumYMFuTu.SumSyaRyoSyo, 0)  AS 'TotalYouFutTum'	--'傭車_付帯・積込品_合計'																											
																					
       ,EIGYOS.RyakuNm					AS 'UkeEigyosRyaku'		--'受付営業所略名' 																							
       ,YOU_YOYKBN.YoyaKbnNm			AS 'YoyaKbn'			--'予約区分' 																									
       ,YY_YYKSHO.UkeYmd				AS 'UkeYmd'				--'受付日' 																								
																										
FROM @SearchReslutKey search
																												
INNER JOIN  TKD_Yousha AS YOUSHA
ON YOUSHA.UkeNo = search.UkeNo
AND YOUSHA.UnkRen = search.UnkRen
AND YOUSHA.YouCdSeq = search.YouTokuiSeq
AND YOUSHA.YouSitCdSeq = search.YouSitenCdSeq
	
INNER JOIN TKD_Yyksho AS YY_YYKSHO 																												
ON YY_YYKSHO.UkeNo = YOUSHA.UkeNo																												
AND YY_YYKSHO.YoyaSyu = 1  　　　　              --予約																												
AND YY_YYKSHO.SiyoKbn = 1 																												
AND YY_YYKSHO.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq																												
																												
INNER JOIN TKD_Haisha AS HAISHA 																												
ON  HAISHA.UkeNo = YOUSHA.UkeNo																												
AND HAISHA.UnkRen = YOUSHA.UnkRen																												
AND HAISHA.YouTblSeq = YOUSHA.YouTblSeq
AND HAISHA.YouTblSeq > 0
AND HAISHA.SiyoKbn = 1																												
																												
INNER JOIN TKD_YykSyu AS YYKSYU 																												
ON YYKSYU.UkeNo = HAISHA.UkeNo																												
AND  YYKSYU.UnkRen =  HAISHA.UnkRen
AND YYKSYU.SyaSyuRen = HAISHA.SyaSyuRen
AND YYKSYU.SiyoKbn = 1
																												
LEFT JOIN VPM_Tokisk AS YY_Tokisk 																												
ON YY_Tokisk.TokuiSeq = YY_YYKSHO.TokuiSeq 																												
AND  YY_Tokisk.SiyoStaYmd <= YY_YYKSHO.SeiTaiYmd																												
AND  YY_Tokisk.SiyoEndYmd >= YY_YYKSHO.SeiTaiYmd																												
AND YY_YYKSHO.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq
AND YY_Tokisk.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq
																												
LEFT JOIN VPM_TokiSt AS YY_TokiSt 																												
ON YY_TokiSt.TokuiSeq = YY_YYKSHO.TokuiSeq 																												
AND YY_TokiSt.SitenCdSeq = YY_YYKSHO.SitenCdSeq																												
AND YY_TokiSt.SiyoStaYmd <= YY_YYKSHO.SeiTaiYmd																												
AND YY_TokiSt.SiyoEndYmd >= YY_YYKSHO.SeiTaiYmd																												
																												
LEFT JOIN VPM_Gyosya AS YY_Gyosya 																												
ON YY_Gyosya.GyosyaCdSeq = YY_Tokisk.GyosyaCdSeq																												
AND YY_Gyosya.SiyoKbn = 1
AND YY_Gyosya.TenantCdSeq = YY_Tokisk.TenantCdSeq
																												
INNER JOIN TKD_Unkobi AS UNKOBI																												
ON UNKOBI.UkeNo = YOUSHA.UkeNo 																												
AND UNKOBI.UnkRen = YOUSHA.UnkRen																												
AND UNKOBI.SiyoKbn = 1																												
																												
--------配車地交通機関分類コード(運行日)取得-----------																												
LEFT JOIN VPM_Koutu AS UnKoutu 																												
ON UnKoutu.KoukCdSeq = UNKOBI.HaiSKouKCdSeq																												
AND UnKoutu.SiyoKbn = 1																												
																												
LEFT JOIN VPM_CodeKb AS UnBunrui 																												
ON UnBunrui.CodeKbnSeq = UnKoutu.BunruiCdSeq																												
AND UnBunrui.SiyoKbn = 1																												
																												
--------到着地交通機関分類コード(運行日)取得------------																												
LEFT JOIN VPM_Koutu AS UnTouChaKoutu																												
ON UnTouChaKoutu.KoukCdSeq = UNKOBI.TouKouKCdSeq																												
AND UnTouChaKoutu.SiyoKbn = 1																												
																												
LEFT JOIN VPM_CodeKb AS UnTouChaBunrui																												
ON UnTouChaBunrui.CodeKbnSeq = UnTouChaKoutu.BunruiCdSeq																												
AND UnTouChaBunrui.SiyoKbn = 1																												
----------------------------ガイド--------------------
	LEFT JOIN 
	(
	    SELECT m.UkeNo, m.UnkRen, ISNULL(SUM(CAST(GuiderTax AS BIGINT)), 0) as Tax, ISNULL(SUM(CAST(GuiderFee AS BIGINT)), 0) as Fee from
			 (SELECT TKD_Yyksho.UkeNo, TKD_YykSyu.UnkRen, TKD_YykSyu.UnitGuiderFee * TKD_Yyksho.FeeGuiderRate/100 AS GuiderFee, 
				CASE																											
			     WHEN TKD_Yyksho.TaxTypeforGuider = 1 THEN (TKD_YykSyu.UnitGuiderFee * TKD_Yyksho.Zeiritsu)/100  																										
			     WHEN TKD_Yyksho.TaxTypeforGuider = 2 THEN (TKD_YykSyu.UnitGuiderFee * TKD_Yyksho.Zeiritsu)/(100+TKD_Yyksho.Zeiritsu) 																										
			     ELSE 0
				 END AS GuiderTax
			 FROM TKD_Yyksho INNER JOIN TKD_YykSyu 
			 ON 
			  TKD_Yyksho.UkeNo = TKD_YykSyu.UkeNo
			 AND TKD_Yyksho.SiyoKbn = 1
			 AND TKD_Yyksho.YoyaSyu = 1
			 AND TKD_Yyksho.TenantCdSeq = @tenantId) as m
			 
			 group by m.UkeNo, m.UnkRen) AS SumGuideTax 
			 ON YOUSHA.UkeNo = SumGuideTax.UkeNo
			 AND YOUSHA.UnkRen = SumGuideTax.UnkRen																												
----------------------------付帯取得-------------------------																												
LEFT JOIN																												
  (SELECT UkeNo ,																												
          UnkRen ,																												
          ISNULL(SUM(CAST(UriGakKin AS BIGINT)), 0) AS SumUriGakKin,																												
          ISNULL(SUM(CAST(SyaRyoSyo AS BIGINT)), 0) AS SumSyaRyoSyo,																												
          ISNULL(SUM(CAST(SyaRyoTes AS BIGINT)), 0) AS SumSyaRyoTes																												
   FROM TKD_FutTum																												
    LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_FutTum.FutTumCdSeq　																												
	      AND VPM_Futai.TenantCdSeq = @tenantId --ログインユーザーのテナントコードSeq																											
   WHERE TKD_FutTum.SiyoKbn = 1 																												
     AND TKD_FutTum.FutTumKbn = 1   --付帯金額のみ																												
     AND VPM_Futai.FutGuiKbn <> 5																												
   GROUP BY UkeNo ,																												
            UnkRen) AS SumFutTum 																												
			ON YOUSHA.UkeNo = SumFutTum.UkeNo																									
           AND YOUSHA.UnkRen = SumFutTum.UnkRen																												
																												
----------------------------積み込み取得-------------------------																												
LEFT JOIN																												
  (SELECT UkeNo ,																												
          UnkRen ,																												
          ISNULL(SUM(CAST(UriGakKin AS BIGINT)), 0) AS SumUriGakKin,																												
          ISNULL(SUM(CAST(SyaRyoTes AS BIGINT)), 0) AS SumSyaRyoTes,																												
          ISNULL(SUM(CAST(SyaRyoSyo AS BIGINT)), 0) AS SumSyaRyoSyo																												
   FROM TKD_FutTum																												
   WHERE TKD_FutTum.SiyoKbn = 1 																												
     AND TKD_FutTum.FutTumKbn = 2   --積み込み金額のみ																												
   GROUP BY UkeNo ,																												
            UnkRen) AS SumTumi																												
			ON YOUSHA.UkeNo = SumTumi.UkeNo																									
           AND YOUSHA.UnkRen = SumTumi.UnkRen																												
																												
----------------------予約税区分取得--------------------------------																												
--LEFT JOIN																												
--  (SELECT CodeKbn ,																												
--          CodeKbnNm ,																												
--          RyakuNm																												
--   FROM VPM_CodeKb																												
--   WHERE CodeSyu = 'ZEIKBN'																												
--     AND SiyoKbn = 1 ) AS YY_ZeiKbn ON YY_YYKSHO.ZeiKbn = YY_ZeiKbn.CodeKbn																												
																												
------------------------------傭車地域------------------------------------																												
																												
LEFT JOIN VPM_Tokisk AS YOU_TOKISK 																												
ON YOU_TOKISK.TokuiSeq = YOUSHA.YouCdSeq																												
   AND YOU_TOKISK.SiyoStaYmd <= YOUSHA.HasYmd 																												
   AND  YOU_TOKISK.SiyoEndYmd >= YOUSHA.HasYmd																												
   AND YOU_TOKISK.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq																												
																												
LEFT JOIN VPM_TokiSt AS YOU_TOKIST																												
 ON YOU_TOKIST.TokuiSeq = YOUSHA.YouCdSeq 																												
AND YOU_TOKIST.SitenCdSeq = YOUSHA.YouSitCdSeq 																												
AND YOU_TOKIST.SiyoStaYmd <= YOUSHA.HasYmd																												
AND YOU_TOKIST.SiyoEndYmd >= YOUSHA.HasYmd																												
																												
----------------------配車地交通機関分類コード(配車)取得------------------																												
--LEFT JOIN VPM_Koutu AS HaiSKoutu 																												
--ON HaiSKoutu.KoukCdSeq = HAISHA.HaiSKouKCdSeq																												
--AND HaiSKoutu.SiyoKbn = 1																												
																												
--LEFT JOIN VPM_CodeKb AS HaiSBunrui 																												
--ON HaiSBunrui.CodeKbnSeq = HaiSKoutu.BunruiCdSeq																												
--AND HaiSBunrui.SiyoKbn = 1																												
--AND HaiSBunrui.TenantCdSeq = @tenantId 　　　　--ログインユーザーのテナントコードSeq																												
																												
----------------------到着地交通機関分類コード(配車)取得------------------																												
--LEFT JOIN VPM_Koutu AS HaiSTouChaKoutu																												
--ON HaiSTouChaKoutu.KoukCdSeq = HAISHA.TouKouKCdSeq																												
--AND HaiSTouChaKoutu.SiyoKbn = 1																												
																												
--LEFT JOIN VPM_CodeKb AS HaiSTouChaBunrui																												
--ON HaiSTouChaBunrui.CodeKbnSeq = HaiSTouChaKoutu.BunruiCdSeq																												
--AND HaiSTouChaBunrui.SiyoKbn = 1																												
--AND HaiSTouChaBunrui.TenantCdSeq = @tenantId 　　　　--ログインユーザーのテナントコードSeq																												
																												
-----------------------傭車税区分取得--------------------------------																												
--LEFT JOIN																												
--  (SELECT CodeKbn ,																												
--          CodeKbnNm ,																												
--          RyakuNm																												
--   FROM VPM_CodeKb																												
--   WHERE CodeSyu = 'ZEIKBN'																												
--     AND SiyoKbn = 1 ) AS YouZeiKbn ON YOUSHA.ZeiKbn = YouZeiKbn.CodeKbn																												
																												
-----------------------傭車付帯・積込品取得--------------------------------																												
LEFT JOIN																												
  (SELECT UkeNo ,																												
          UnkRen ,																												
          YouTblSeq ,																												
          ISNULL(SUM(CAST(HaseiKin AS BIGINT)), 0) AS SumHaseiKin ,																												
          ISNULL(SUM(CAST(SyaRyoSyo AS BIGINT)), 0) AS SumSyaryoSyo ,																												
          ISNULL(SUM(CAST(SyaRyoTes AS BIGINT)), 0) AS SumSyaryoTes																												
   FROM TKD_YFutTu																												
   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_YFutTu.FutTumCdSeq																												
   WHERE TKD_YFutTu.SiyoKbn = 1																												
   AND VPM_Futai.FutGuiKbn <> 5																												
   GROUP BY UkeNo ,																												
            UnkRen ,																												
            YouTblSeq) AS YOU_SumYMFuTu ON HAISHA.UkeNo=YOU_SumYMFuTu.UkeNo																												
AND HAISHA.UnkRen=YOU_SumYMFuTu.UnkRen																												
AND HAISHA.YouTblSeq=YOU_SumYMFuTu.YouTblSeq																												
																												
LEFT JOIN																												
  (SELECT UkeNo ,																												
          UnkRen ,																												
          YouTblSeq ,																												
          ISNULL(SUM(CAST(HaseiKin AS BIGINT)), 0) AS SumHaseiKin,																												
          ISNULL(SUM(CAST(SyaRyoSyo AS BIGINT)), 0) AS SumSyaryoSyo,																												
          ISNULL(SUM(CAST(SyaRyoTes AS BIGINT)), 0) AS SumSyaryoTes																												
   FROM TKD_YFutTu																												
   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_YFutTu.FutTumCdSeq																												
   AND VPM_Futai.SiyoKbn=1																												
   WHERE TKD_YFutTu.SiyoKbn = 1																												
     AND VPM_Futai.FutGuiKbn = 5																												
   GROUP BY UkeNo ,																												
            UnkRen ,																												
            YouTblSeq) AS YOU_SumYMFuTuGui ON HAISHA.UkeNo=YOU_SumYMFuTuGui.UkeNo																												
            AND HAISHA.UnkRen=YOU_SumYMFuTuGui.UnkRen																												
            AND HAISHA.YouTblSeq=YOU_SumYMFuTuGui.YouTblSeq																												
------------------------------------------------------------------------------------------------------------------																												
																												
LEFT JOIN VPM_Gyosya AS YOU_GYOSYA 																												
 ON YOU_GYOSYA.GyosyaCdSeq = YOU_TOKISK.GyosyaCdSeq																												
AND YOU_GYOSYA.SiyoKbn = 1
AND YOU_GYOSYA.TenantCdSeq = YOU_TOKISK.TenantCdSeq
																												
LEFT JOIN VPM_Eigyos AS EIGYOS																												
ON YY_YYKSHO.UkeEigCdSeq = EIGYOS.EigyoCdSeq																												
AND EIGYOS.SiyoKbn = 1																												
																												
LEFT JOIN VPM_Compny AS COMPANY 																												
ON EIGYOS.CompanyCdSeq = COMPANY.CompanyCdSeq																												
AND COMPANY.SiyoKbn = 1																												
AND COMPANY.TenantCdSeq = @tenantId                     --ログインユーザーのテナントコードSeq																												
																												
LEFT JOIN VPM_YoyKbn AS YOU_YOYKBN 																												
ON YOU_YOYKBN.TenantCdSeq = @tenantId  
AND YOU_YOYKBN.YoyaKbnSeq = YY_YYKSHO.YoyaKbnSeq 																												
AND YOU_YOYKBN.SiyoKbn = 1																												
																												
LEFT JOIN VPM_Syain AS SYAIN																												
ON SYAIN.SyainCdSeq = YY_YYKSHO.EigTanCdSeq																																																																																																											
      																									
ORDER BY
CASE WHEN ((@isSearch = 1 AND @outputOrder = 0) 
			OR (@isSearch = 0 AND @dateType = 1 AND @group = 0)
			OR (@isSearch = 0 AND @dateType = 1 AND @group = 1)) THEN UNKOBI.HaiSYmd END,																									
CASE WHEN (@isSearch = 1 AND @outputOrder = 0  OR (@isSearch = 1 AND @outputOrder = 1)
			OR (@isSearch = 0 AND @dateType = 1 AND @group = 0)
			OR (@isSearch = 0 AND @dateType = 2 AND @group = 0)
			OR (@isSearch = 0 AND @dateType = 2 AND @group = 1)) THEN UNKOBI.TouYmd END,	
CASE WHEN (@isSearch = 1 AND @outputOrder = 1 
			OR (@isSearch = 0 AND @dateType = 1 AND @group = 1)
			OR (@isSearch = 0 AND @dateType = 1 AND @group = 2)
			OR (@isSearch = 0 AND @dateType = 2 AND @group = 0)
			OR (@isSearch = 0 AND @dateType = 2 AND @group = 1)
			OR (@isSearch = 0 AND @dateType = 2 AND @group = 2)) THEN YOU_TOKISK.TokuiCd END,
CASE WHEN (@isSearch = 0 AND @dateType = 1 AND @group = 2
			OR (@isSearch = 0 AND @dateType = 2 AND @group = 2)) THEN YOU_TOKIST.SitenCd END,
YOUSHA.UkeNo ,																												
YOUSHA.UnkRen ,																												
YOUSHA.YouTblSeq																												
END
