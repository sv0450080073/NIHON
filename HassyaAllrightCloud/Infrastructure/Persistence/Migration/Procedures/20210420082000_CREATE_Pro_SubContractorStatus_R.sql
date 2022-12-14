USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_SubContractorStatus_R]    Script Date: 2021/04/20 8:18:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE       PROC [dbo].[Pro_SubContractorStatus_R]
					@startDate varchar(8), @endDate varchar(8),
					@dateType int,
					@tokuiFrom int, @tokuiTo int, @sitenFrom int, @sitenTo int, -- for you customer
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
	LEFT JOIN TKD_Yyksho AS YY_YYKSHO 																												
	ON YY_YYKSHO.UkeNo = YOUSHA.UkeNo																												
	AND YY_YYKSHO.YoyaSyu = 1  ????????????              --??????																												
	AND YY_YYKSHO.SiyoKbn = 1 																												
	AND YY_YYKSHO.TenantCdSeq = @tenantId                    --????????????????????????????????????????????????Seq																												
																													
	LEFT JOIN TKD_Haisha AS HAISHA 																												
	ON  HAISHA.UkeNo = YOUSHA.UkeNo																												
	AND HAISHA.UnkRen = YOUSHA.UnkRen																												
	AND HAISHA.YouTblSeq = YOUSHA.YouTblSeq
	AND HAISHA.YouTblSeq > 0
	AND HAISHA.SiyoKbn = 1	
	
	LEFT JOIN TKD_Unkobi AS UNKOBI																												
	ON UNKOBI.UkeNo = YOUSHA.UkeNo 																												
	AND UNKOBI.UnkRen = YOUSHA.UnkRen																												
	AND UNKOBI.SiyoKbn = 1	
																													
	------------------------------????????????------------------------------------																												
																													
	LEFT JOIN VPM_Tokisk AS YOU_TOKISK 																												
	ON YOU_TOKISK.TokuiSeq = YOUSHA.YouCdSeq																												
	   AND YOU_TOKISK.SiyoStaYmd <= YOUSHA.HasYmd 																												
	   AND  YOU_TOKISK.SiyoEndYmd >= YOUSHA.HasYmd																												
	   AND YOU_TOKISK.TenantCdSeq = @tenantId                    --????????????????????????????????????????????????Seq																												
																													
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
	AND COMPANY.TenantCdSeq = @tenantId                     --????????????????????????????????????????????????Seq																												
																													
	LEFT JOIN VPM_YoyKbn AS YOU_YOYKBN 																												
	ON YOU_YOYKBN.YoyaKbnSeq = YY_YYKSHO.YoyaKbnSeq 																												
	AND YOU_YOYKBN.SiyoKbn = 1																												
																													
	LEFT JOIN VPM_Syain AS SYAIN																												
	ON SYAIN.SyainCdSeq = YY_YYKSHO.EigTanCdSeq
	WHERE
		YY_YYKSHO.UkeCD  >= @ukeCdFrom         --?????????????????????????????????From?????????																												
	 AND YY_YYKSHO.UkeCD <= @ukeCdTo			--?????????????????????????????????To?????????																									
	 AND YOUSHA.SiyoKbn = 1																											
	 AND YOUSHA.JitaFlg = CASE WHEN @jitaFlg = 0 THEN YOUSHA.JitaFlg ELSE 0 END			--????????????????????????????????????????????????????????????											
	 AND( (@dateType = 1 AND HAISHA.HaiSYmd >= @startDate AND HAISHA.HaiSYmd <= @endDate)		--?????? ???????????????:  ??????????????????????????????From?????????/ ??????????????????????????????To?????????
	 OR (@dateType = 2 AND HAISHA.TouYmd >= @startDate AND HAISHA.TouYmd <= @endDate))			--?????? ???????????????:  ??????????????????????????????From?????????/ ??????????????????????????????To?????????
	 AND YOU_YOYKBN.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))	--?????????????????????????????????From?????????										 
	 AND COMPANY.CompanyCdSeq IN (SELECT * FROM FN_SplitString(@companyIds, '-'))	--???????????????????????????????????????SEQ
	 AND EIGYOS.EigyoCd >= @brandStart			--?????????????????????????????????????????????From?????????											
	 AND EIGYOS.EigyoCd <= @brandEnd			--?????????????????????????????????????????????To?????????
	 AND ((@tokuiFrom = 0 and @tokuiTo = 0)
			or
			(@tokuiFrom <> @tokuiTo and YOUSHA.YouCdSeq = @tokuiFrom and YOUSHA.YouSitCdSeq >= @sitenFrom)
			or
			(@tokuiFrom <> @tokuiTo and YOUSHA.YouCdSeq = @tokuiTo and YOUSHA.YouSitCdSeq <= @sitenTo)
			or
			(@tokuiFrom = @tokuiTo and YOUSHA.YouCdSeq = @tokuiFrom and YOUSHA.YouSitCdSeq >= @sitenFrom and YOUSHA.YouSitCdSeq <= @sitenTo)
			or
			(@tokuiFrom = 0 and @tokuiTo <> 0 and ((YOUSHA.YouCdSeq = @tokuiTo and YOUSHA.YouSitCdSeq <= @sitenTo) or YOUSHA.YouCdSeq < @tokuiTo))
			or
			(@tokuiTo = 0 and @tokuiFrom <> 0 and ((YOUSHA.YouCdSeq = @tokuiFrom and YOUSHA.YouSitCdSeq >= @sitenFrom) or YOUSHA.YouCdSeq > @tokuiFrom))
			or
			(YOUSHA.YouCdSeq < @tokuiTo and YOUSHA.YouCdSeq > @tokuiFrom))
	 AND SYAIN.SyainCdSeq >= @staffFrom		--?????????????????????????????????SEQ????????????From?????????																										
	 AND SYAIN.SyainCdSeq <= @staffTo			--?????????????????????????????????SEQ????????????To?????????	
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
	             AND UnkRen = YOUSHA.UnkRen)			AS 'TotalSyaRyoUnc' --'??????'																																																		
	       ,(SELECT SUM (CAST(IsNull(TKD_Yyksho.ZeiRui, 0) AS BIGINT)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1) 		AS 'TotalZeiRui'	--'?????????'																																								
	       ,(SELECT SUM (CAST(IsNull(TKD_Yyksho.TesuRyoG, 0) AS BIGINT)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1)		AS 'TotalTesuRyoG'	--'?????????' 
		   ,SumGuideTax.Fee??? AS 'TotalGuideFee'???		--'??????????????????'														
		   ,SumGuideTax.Tax	AS 'TotalGuideTax'			--'??????????????????	
			,(SELECT SUM(CAST(IsNull(TKD_YykSyu.UnitGuiderFee, 0) AS BIGINT)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1)	AS 'TotalUnitGuiderFee'		--'????????????'
			
			,(ISNULL(SumFutTum.SumUriGakKin, 0) + ISNULL(SumTumi.SumUriGakKin, 0))   AS 'TotalIncidentalFee'		--'??????????????????'																												
			,(ISNULL(SumFutTum.SumSyaRyoSyo, 0) + ISNULL(SumTumi.SumSyaRyoSyo, 0))   AS 'TotalIncidentalTax'		--'??????????????????_?????????' 																												
			,(ISNULL(SumFutTum.SumSyaRyoTes, 0) + ISNULL(SumTumi.SumSyaRyoTes, 0))   AS 'TotalIncidentalCharge'	--'??????????????????_?????????'
			
			,IsNull(HAISHA.YoushaUnc, 0)					AS 'TotalYoushaUnc'		--'??????_??????'
			,IsNull(HAISHA.YoushaSyo, 0)					AS 'TotalYoushaSyo'	--'??????_?????????'
			,IsNull(HAISHA.YoushaTes, 0)					AS 'TotalYoushaTes'	--'??????_?????????'
			,ISNULL(YOU_SumYMFuTuGui.SumHaseiKin, 0)		AS 'TotalYouFutTumGuiKin'	--'??????_????????????'  																											
			,IsNull(YOU_SumYMFuTuGui.SumSyaRyoSyo, 0)	AS 'TotalYouFutTumGuiTax'	--'??????_?????????_?????????' 																											
			,IsNull(YOU_SumYMFuTuGui.SumSyaRyoTes, 0)	AS 'TotalYouFutTumGuiTes'	--'??????_?????????_?????????' 		
			,IsNull(YOU_SumYMFuTu.SumHaseiKin, 0)		AS 'TotalYouFutTumKin'		--'??????_??????????????????' 																										
			,IsNull(YOU_SumYMFuTu.SumSyaRyoSyo, 0)  		AS 'TotalYouFutTumTax'		--'??????_??????????????????_?????????'  																											
			,IsNull(YOU_SumYMFuTu.SumSyaRyoTes, 0)   	AS 'TotalYouFutTumTes'		--'??????_??????????????????_?????????'
	FROM @SearchReslutKey search
																												
	INNER JOIN  TKD_Yousha AS YOUSHA
	ON YOUSHA.UkeNo = search.UkeNo
	AND YOUSHA.UnkRen = search.UnkRen
	AND YOUSHA.YouCdSeq = search.YouTokuiSeq
	AND YOUSHA.YouSitCdSeq = search.YouSitenCdSeq
																									
	LEFT JOIN TKD_Haisha AS HAISHA 																												
	ON  HAISHA.UkeNo = YOUSHA.UkeNo																												
	AND HAISHA.UnkRen = YOUSHA.UnkRen																												
	AND HAISHA.YouTblSeq = YOUSHA.YouTblSeq
	AND HAISHA.YouTblSeq > 0
	AND HAISHA.SiyoKbn = 1	
	----------------------------?????????--------------------
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
	
	----------------------------????????????-------------------------																												
	LEFT JOIN																												
	  (SELECT UkeNo ,																												
	          UnkRen ,																												
	          SUM(CAST(UriGakKin AS BIGINT)) AS SumUriGakKin ,																												
	          SUM(CAST(SyaRyoSyo AS BIGINT)) AS SumSyaRyoSyo ,																												
	          SUM(CAST(SyaRyoTes AS BIGINT)) AS SumSyaRyoTes																												
	   FROM TKD_FutTum																												
	    LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_FutTum.FutTumCdSeq???																												
		      AND VPM_Futai.TenantCdSeq = @tenantId --????????????????????????????????????????????????Seq																											
	   WHERE TKD_FutTum.SiyoKbn = 1 																												
	     AND TKD_FutTum.FutTumKbn = 1   --??????????????????																												
	     AND VPM_Futai.FutGuiKbn <> 5																												
	   GROUP BY UkeNo ,																												
	            UnkRen) AS SumFutTum 																												
				ON YOUSHA.UkeNo = SumFutTum.UkeNo																									
	           AND YOUSHA.UnkRen = SumFutTum.UnkRen																												
																													
	----------------------------??????????????????-------------------------																												
	LEFT JOIN																												
	  (SELECT UkeNo ,																												
	          UnkRen ,																												
	          SUM(CAST(UriGakKin AS BIGINT)) AS SumUriGakKin ,																												
	          SUM(CAST(SyaRyoSyo AS BIGINT)) AS SumSyaRyoSyo ,																												
	          SUM(CAST(SyaRyoTes AS BIGINT)) AS SumSyaRyoTes																												
	   FROM TKD_FutTum																												
	   WHERE TKD_FutTum.SiyoKbn = 1 																												
	     AND TKD_FutTum.FutTumKbn = 2   --????????????????????????																												
	   GROUP BY UkeNo ,																												
	            UnkRen) AS SumTumi																												
				ON YOUSHA.UkeNo = SumTumi.UkeNo																									
	           AND YOUSHA.UnkRen = SumTumi.UnkRen																												
																																			
	-----------------------??????????????????????????????--------------------------------																												
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
SELECT YOUSHA.UkeNo               AS 'UkeNo' --'??????_????????????'																												
	   ,YY_YYKSHO.UkeCd           AS 'UkeCd'					--'???????????????'																											
       ,YOUSHA.UnkRen             AS 'UnkRen' --'??????_???????????????'																																																						
	   ,UNKOBI.HaiSYmd            AS 'HaiSYmd'		--'?????????_???????????????'																											
	   ,UNKOBI.TouYmd             AS 'TouYmd'		--'?????????_???????????????'																											
	   ,YY_Tokisk.TokuiCd         AS 'SkTokuiCd' 		--'??????????????????'																											
       ,YY_Tokisk.TokuiNm         AS 'TokuiNm'		--'????????????' 																												
       ,YY_Tokisk.RyakuNm         AS 'TokiskRyakuNm' 		--'?????????_??????'																												
       ,YY_TokiSt.SitenCd         AS 'StSitenCd' 		--'??????????????? '																												
       ,YY_TokiSt.SitenNm         AS 'SitenNm'		--'?????????' 																												
       ,YY_TokiSt.RyakuNm         AS 'TokiStRyakuNm' 		--'??????_??????'	   																																																				
	   ,YY_YYKSHO.TokuiTanNm      AS 'TokuiTanNm'		--'??????_???????????????' 																											
       ,YY_YYKSHO.TokuiTel        AS 'TokuiTel'		--'??????_??????????????????'																																																						
	   ,UNKOBI.DanTaNm            AS 'DanTaNm'		--'?????????_?????????' 																											
       ,UNKOBI.IkNm               AS 'IkNm'		--'?????????_?????????' 																												
       ,UNKOBI.HaiSTime			  AS 'U_HaiSTime' --'?????????_????????????' 																									
       ,UNKOBI.HaiSNm			  AS 'U_HaiSNm' --'?????????_????????????'																									
	   ,UNKOBI.HaiSSetTime		  AS 'U_HaiSSetTime' --'?????????_?????????????????????' 																																																			
	   ,UNKOBI.HaiSKouKNm         AS 'U_HaiSKouKNm' --'????????????????????????' 																																																			
       ,UNKOBI.HaiSBinNm		  AS 'U_HaiSBinNm' --'???????????????' 																																																		
	   ,UNKOBI.TouChTime		  AS 'U_TouChTime' --'?????????_????????????'  																									
	   ,UNKOBI.TouNm			  AS 'U_TouNm' --'?????????_????????????'																								
	   ,UNKOBI.TouSetTime		  AS 'U_TouSetTime' --'?????????_??????????????????'																																																		
       ,UNKOBI.TouSKouKNm		  AS 'U_TouSKouKNm' --'????????????_?????????'																																																						
       ,UNKOBI.TouSBinNm		  AS 'U_TouSBinNm'--'?????????_??????'																																																			
       ,UNKOBI.JyoSyaJin          AS 'U_JyoSyaJin'--'?????????' 																												
       ,UNKOBI.PlusJin			  AS 'U_PlusJin'--'???????????????'																																																		
																												
	   ,(SELECT SUM (ISNULL(SyaSyuDai, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo 																											
             AND SiyoKbn = 1 AND UnkRen = YOUSHA.UnkRen) AS 'TotalNumber'		--'??????_????????????'																												
			 																									
       ,(SELECT SUM (IsNull(TKD_YykSyu.SyaRyoUnc, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo  AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1 																												
	             AND UnkRen = YOUSHA.UnkRen)                 AS 'SumSyaRyoUnc'		--'??????'																												
	    ,YY_YYKSHO.Zeiritsu     AS 'Zeiritsu'		--'??????' 																											
       ,(SELECT SUM (IsNull(TKD_Yyksho.ZeiRui, 0)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1) AS 'SumZeiRui'		--'?????????'														
       ,YY_YYKSHO.TesuRitu                               AS 'TesuRitu'		--'?????????'																												
       ,(SELECT SUM (IsNull(TKD_Yyksho.TesuRyoG, 0)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1)                 																												
														 AS 'SumTesuRyoG'		--'?????????' 														
																												
	   --,(SELECT SUM (IsNull(TKD_YykSyu.SyaRyoUnc, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo  AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1 																												
	   --          AND UnkRen = YOUSHA.UnkRen)  + 																												
	   --(SELECT SUM (IsNull(TKD_Yyksho.ZeiRui, 0)) FROM TKD_Yyksho WHERE UkeNo = YOUSHA.UkeNo AND SiyoKbn = 1)           																											
	   -- 												 AS 'SumTicket'					--- cal in code						 
????????????,SumGuideTax.Fee??? AS 'SumGuideFee'		--'??????????????????'																												
????????????,SumGuideTax.Tax	  AS 'SumGuideTax'			--'??????????????????'																											
	??????,(SELECT SUM(IsNull(TKD_YykSyu.UnitGuiderFee, 0)) FROM TKD_YykSyu WHERE UkeNo = YOUSHA.UkeNo AND UnkRen = YOUSHA.UnkRen AND SiyoKbn = 1)	AS 'SumUnitGuiderFee'		--'????????????'																											
																												
																												
       ,ISNULL(SumFutTum.SumUriGakKin, 0) + ISNULL(SumTumi.SumUriGakKin, 0)   AS 'IncidentalFee'		--'??????????????????'																												
       ,ISNULL(SumFutTum.SumSyaRyoSyo, 0) + ISNULL(SumTumi.SumSyaRyoSyo, 0)   AS 'IncidentalTax'		--'??????????????????_?????????' 																												
       ,ISNULL(SumFutTum.SumSyaRyoTes, 0) + ISNULL(SumTumi.SumSyaRyoTes, 0)   AS 'IncidentalCharge'	--'??????????????????_?????????'																												
	   ,ISNULL(SumFutTum.SumUriGakKin, 0) + ISNULL(SumTumi.SumUriGakKin, 0) + ISNULL(SumFutTum.SumSyaRyoSyo, 0) + ISNULL(SumTumi.SumSyaRyoSyo, 0)																											
	   AS 'TotalIncidental'	--'????????????'																											
																												
------------------------------??????------------------------------------																												
       ,YOU_TOKISK.TokuiSeq             AS 'YouTokuiSeq'		--'??????_?????????SEQ' 																																																
       ,YOU_TOKISK.TokuiNm				AS 'YouSkTokuiNm'		--'??????_????????????' 																								
       ,YOU_TOKISK.RyakuNm				AS	'YouSkRyakuNm'	--'??????_?????????_??????' 																								
       ,YOU_TOKIST.SitenCdSeq			AS 'YouSitenCdSeq'	--'??????_???????????????SEQ' 																																																
       ,YOU_TOKIST.SitenNm				AS 'YouStSitenNm'		--'??????_??????????????????' 																								
       ,YOU_TOKIST.RyakuNm				AS 'YouStRyakuNm'	--'??????_???????????????_??????'																																													
       ,HAISHA.GoSya					AS 'HAISHA_GoSya'			--'??????_??????'																																														
       ,HAISHA.HaiSYmd					AS 'H_HaiSYmd'				--'??????_??????_???????????????' 																							
       ,HAISHA.TouYmd					AS 'H_TouYmd'				--'??????_??????_???????????????' 																							
       ,HAISHA.HaiSTime					AS 'H_HaiSTime'				--'??????_??????_????????????'																							
       ,HAISHA.HaiSNm					AS 'H_HaiSNm'				--'??????_????????????'																							
	   ,HAISHA.HaiSSetTime				AS 'H_HaiSSetTime'			--'??????_??????????????????'																							
	   ,HAISHA.JyoSyaJin				AS 'JyoSyaJin'	--'??????_??????????????????'																							
	   ,HAISHA.PlusJin					AS 'PlusJin'	--'??????_?????????????????????'																																													
	   ,HAISHA.HaiSKouKNm			    AS 'H_HaiSKouKNm'			--'??????_?????????' 																																														
       ,HAISHA.HaiSBinNm				AS 'H_HaiSBinNm'			--'??????_??????' 																								
	   ,HAISHA.TouChTime			    AS 'H_TouChTime'			--'??????_????????????'																								
	   ,HAISHA.TouNm					AS 'H_TouNm'				--'??????_????????????'																						
	   ,HAISHA.TouSetTime		        AS 'H_TouSetTime'			--'??????_??????????????????'																																																
	   ,HAISHA.TouSKouKNm				AS 'H_TouSKouKNm'			--'??????_???????????????'																																													
	   ,HAISHA.TouBinNm					AS 'H_TouBinNm'				--'??????_?????????_??????'																																											
       ,HAISHA.YoushaUnc				AS 'YoushaUnc'	--'??????_??????'																																															
       ,YOUSHA.Zeiritsu					AS 'YouZeiritsu'	--'??????_??????'																							
       ,HAISHA.YoushaSyo				AS 'YoushaSyo'	--'??????_?????????'																								
	   ,YOUSHA.TesuRitu					AS 'YouTesuRitu'	--'??????_?????????'																						
       ,HAISHA.YoushaTes				AS 'YoushaTes'	--'??????_?????????' 																																																		
       ,ISNULL(YOU_SumYMFuTuGui.SumHaseiKin, 0)	AS 'YouFutTumGuiKin'	--'??????_????????????'  																											
       ,ISNULL(YOU_SumYMFuTuGui.SumSyaRyoSyo, 0)	AS 'YouFutTumGuiTax'	--'??????_?????????_?????????' 																											
       ,ISNULL(YOU_SumYMFuTuGui.SumSyaRyoTes, 0)	AS 'YouFutTumGuiTes'	--'??????_?????????_?????????' 		   																									
	   ,ISNULL(YOU_SumYMFuTuGui.SumHaseiKin, 0)???+ ISNULL(YOU_SumYMFuTuGui.SumSyaRyoSyo, 0)	AS 'TotalYouFutTumGui'			--AS '??????_?????????_??????' 																							
																												
       ,ISNULL(YOU_SumYMFuTu.SumHaseiKin, 0)		AS 'YouFutTumKin'		--'??????_??????????????????' 																										
       ,ISNULL(YOU_SumYMFuTu.SumSyaRyoSyo, 0)  	AS 'YouFutTumTax'		--'??????_??????????????????_?????????'  																											
       ,ISNULL(YOU_SumYMFuTu.SumSyaRyoTes, 0)   	AS 'YouFutTumTes'		--'??????_??????????????????_?????????'  																											
	   ,ISNULL(YOU_SumYMFuTu.SumHaseiKin, 0)  + ISNULL(YOU_SumYMFuTu.SumSyaRyoSyo, 0)  AS 'TotalYouFutTum'	--'??????_??????????????????_??????'																											
																					
       ,EIGYOS.RyakuNm					AS 'UkeEigyosRyaku'		--'?????????????????????' 																							
       ,YOU_YOYKBN.YoyaKbnNm			AS 'YoyaKbn'			--'????????????' 																									
       ,YY_YYKSHO.UkeYmd				AS 'UkeYmd'				--'?????????' 																								
																										
FROM @SearchReslutKey search
																												
INNER JOIN  TKD_Yousha AS YOUSHA
ON YOUSHA.UkeNo = search.UkeNo
AND YOUSHA.UnkRen = search.UnkRen
AND YOUSHA.YouCdSeq = search.YouTokuiSeq
AND YOUSHA.YouSitCdSeq = search.YouSitenCdSeq
	
LEFT JOIN TKD_Yyksho AS YY_YYKSHO 																												
ON YY_YYKSHO.UkeNo = YOUSHA.UkeNo																												
AND YY_YYKSHO.YoyaSyu = 1  ????????????              --??????																												
AND YY_YYKSHO.SiyoKbn = 1 																												
AND YY_YYKSHO.TenantCdSeq = @tenantId                    --????????????????????????????????????????????????Seq																												
																												
LEFT JOIN TKD_Haisha AS HAISHA 																												
ON  HAISHA.UkeNo = YOUSHA.UkeNo																												
AND HAISHA.UnkRen = YOUSHA.UnkRen																												
AND HAISHA.YouTblSeq = YOUSHA.YouTblSeq
AND HAISHA.YouTblSeq > 0
AND HAISHA.SiyoKbn = 1																												
																												
LEFT JOIN TKD_YykSyu AS YYKSYU 																												
ON YYKSYU.UkeNo = HAISHA.UkeNo																												
AND  YYKSYU.UnkRen =  HAISHA.UnkRen
AND YYKSYU.SyaSyuRen = HAISHA.SyaSyuRen
AND YYKSYU.SiyoKbn = 1
																												
LEFT JOIN VPM_Tokisk AS YY_Tokisk 																												
ON YY_Tokisk.TokuiSeq = YY_YYKSHO.TokuiSeq 																												
AND  YY_Tokisk.SiyoStaYmd <= YY_YYKSHO.SeiTaiYmd																												
AND  YY_Tokisk.SiyoEndYmd >= YY_YYKSHO.SeiTaiYmd																												
AND YY_YYKSHO.TenantCdSeq = @tenantId                    --????????????????????????????????????????????????Seq																												
																												
LEFT JOIN VPM_TokiSt AS YY_TokiSt 																												
ON YY_TokiSt.TokuiSeq = YY_YYKSHO.TokuiSeq 																												
AND YY_TokiSt.SitenCdSeq = YY_YYKSHO.SitenCdSeq																												
AND YY_TokiSt.SiyoStaYmd <= YY_YYKSHO.SeiTaiYmd																												
AND YY_TokiSt.SiyoEndYmd >= YY_YYKSHO.SeiTaiYmd																												
																												
LEFT JOIN VPM_Gyosya AS YY_Gyosya 																												
ON YY_Gyosya.GyosyaCdSeq = YY_Tokisk.GyosyaCdSeq																												
AND YY_Gyosya.SiyoKbn = 1																												
																												
LEFT JOIN TKD_Unkobi AS UNKOBI																												
ON UNKOBI.UkeNo = YOUSHA.UkeNo 																												
AND UNKOBI.UnkRen = YOUSHA.UnkRen																												
AND UNKOBI.SiyoKbn = 1																												
																												
--------????????????????????????????????????(?????????)??????-----------																												
LEFT JOIN VPM_Koutu AS UnKoutu 																												
ON UnKoutu.KoukCdSeq = UNKOBI.HaiSKouKCdSeq																												
AND UnKoutu.SiyoKbn = 1																												
																												
LEFT JOIN VPM_CodeKb AS UnBunrui 																												
ON UnBunrui.CodeKbnSeq = UnKoutu.BunruiCdSeq																												
AND UnBunrui.SiyoKbn = 1																												
																												
--------????????????????????????????????????(?????????)??????------------																												
LEFT JOIN VPM_Koutu AS UnTouChaKoutu																												
ON UnTouChaKoutu.KoukCdSeq = UNKOBI.TouKouKCdSeq																												
AND UnTouChaKoutu.SiyoKbn = 1																												
																												
LEFT JOIN VPM_CodeKb AS UnTouChaBunrui																												
ON UnTouChaBunrui.CodeKbnSeq = UnTouChaKoutu.BunruiCdSeq																												
AND UnTouChaBunrui.SiyoKbn = 1																												
----------------------------?????????--------------------
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
----------------------------????????????-------------------------																												
LEFT JOIN																												
  (SELECT UkeNo ,																												
          UnkRen ,																												
          ISNULL(SUM(CAST(UriGakKin AS BIGINT)), 0) AS SumUriGakKin,																												
          ISNULL(SUM(CAST(SyaRyoSyo AS BIGINT)), 0) AS SumSyaRyoSyo,																												
          ISNULL(SUM(CAST(SyaRyoTes AS BIGINT)), 0) AS SumSyaRyoTes																												
   FROM TKD_FutTum																												
    LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_FutTum.FutTumCdSeq???																												
	      AND VPM_Futai.TenantCdSeq = @tenantId --????????????????????????????????????????????????Seq																											
   WHERE TKD_FutTum.SiyoKbn = 1 																												
     AND TKD_FutTum.FutTumKbn = 1   --??????????????????																												
     AND VPM_Futai.FutGuiKbn <> 5																												
   GROUP BY UkeNo ,																												
            UnkRen) AS SumFutTum 																												
			ON YOUSHA.UkeNo = SumFutTum.UkeNo																									
           AND YOUSHA.UnkRen = SumFutTum.UnkRen																												
																												
----------------------------??????????????????-------------------------																												
LEFT JOIN																												
  (SELECT UkeNo ,																												
          UnkRen ,																												
          ISNULL(SUM(CAST(UriGakKin AS BIGINT)), 0) AS SumUriGakKin,																												
          ISNULL(SUM(CAST(SyaRyoTes AS BIGINT)), 0) AS SumSyaRyoTes,																												
          ISNULL(SUM(CAST(SyaRyoSyo AS BIGINT)), 0) AS SumSyaRyoSyo																												
   FROM TKD_FutTum																												
   WHERE TKD_FutTum.SiyoKbn = 1 																												
     AND TKD_FutTum.FutTumKbn = 2   --????????????????????????																												
   GROUP BY UkeNo ,																												
            UnkRen) AS SumTumi																												
			ON YOUSHA.UkeNo = SumTumi.UkeNo																									
           AND YOUSHA.UnkRen = SumTumi.UnkRen																												
																												
----------------------?????????????????????--------------------------------																												
--LEFT JOIN																												
--  (SELECT CodeKbn ,																												
--          CodeKbnNm ,																												
--          RyakuNm																												
--   FROM VPM_CodeKb																												
--   WHERE CodeSyu = 'ZEIKBN'																												
--     AND SiyoKbn = 1 ) AS YY_ZeiKbn ON YY_YYKSHO.ZeiKbn = YY_ZeiKbn.CodeKbn																												
																												
------------------------------????????????------------------------------------																												
																												
LEFT JOIN VPM_Tokisk AS YOU_TOKISK 																												
ON YOU_TOKISK.TokuiSeq = YOUSHA.YouCdSeq																												
   AND YOU_TOKISK.SiyoStaYmd <= YOUSHA.HasYmd 																												
   AND  YOU_TOKISK.SiyoEndYmd >= YOUSHA.HasYmd																												
   AND YOU_TOKISK.TenantCdSeq = @tenantId                    --????????????????????????????????????????????????Seq																												
																												
LEFT JOIN VPM_TokiSt AS YOU_TOKIST																												
 ON YOU_TOKIST.TokuiSeq = YOUSHA.YouCdSeq 																												
AND YOU_TOKIST.SitenCdSeq = YOUSHA.YouSitCdSeq 																												
AND YOU_TOKIST.SiyoStaYmd <= YOUSHA.HasYmd																												
AND YOU_TOKIST.SiyoEndYmd >= YOUSHA.HasYmd																												
																												
----------------------????????????????????????????????????(??????)??????------------------																												
--LEFT JOIN VPM_Koutu AS HaiSKoutu 																												
--ON HaiSKoutu.KoukCdSeq = HAISHA.HaiSKouKCdSeq																												
--AND HaiSKoutu.SiyoKbn = 1																												
																												
--LEFT JOIN VPM_CodeKb AS HaiSBunrui 																												
--ON HaiSBunrui.CodeKbnSeq = HaiSKoutu.BunruiCdSeq																												
--AND HaiSBunrui.SiyoKbn = 1																												
--AND HaiSBunrui.TenantCdSeq = @tenantId ????????????--????????????????????????????????????????????????Seq																												
																												
----------------------????????????????????????????????????(??????)??????------------------																												
--LEFT JOIN VPM_Koutu AS HaiSTouChaKoutu																												
--ON HaiSTouChaKoutu.KoukCdSeq = HAISHA.TouKouKCdSeq																												
--AND HaiSTouChaKoutu.SiyoKbn = 1																												
																												
--LEFT JOIN VPM_CodeKb AS HaiSTouChaBunrui																												
--ON HaiSTouChaBunrui.CodeKbnSeq = HaiSTouChaKoutu.BunruiCdSeq																												
--AND HaiSTouChaBunrui.SiyoKbn = 1																												
--AND HaiSTouChaBunrui.TenantCdSeq = @tenantId ????????????--????????????????????????????????????????????????Seq																												
																												
-----------------------?????????????????????--------------------------------																												
--LEFT JOIN																												
--  (SELECT CodeKbn ,																												
--          CodeKbnNm ,																												
--          RyakuNm																												
--   FROM VPM_CodeKb																												
--   WHERE CodeSyu = 'ZEIKBN'																												
--     AND SiyoKbn = 1 ) AS YouZeiKbn ON YOUSHA.ZeiKbn = YouZeiKbn.CodeKbn																												
																												
-----------------------??????????????????????????????--------------------------------																												
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
																												
LEFT JOIN VPM_Eigyos AS EIGYOS																												
ON YY_YYKSHO.UkeEigCdSeq = EIGYOS.EigyoCdSeq																												
AND EIGYOS.SiyoKbn = 1																												
																												
LEFT JOIN VPM_Compny AS COMPANY 																												
ON EIGYOS.CompanyCdSeq = COMPANY.CompanyCdSeq																												
AND COMPANY.SiyoKbn = 1																												
AND COMPANY.TenantCdSeq = @tenantId                     --????????????????????????????????????????????????Seq																												
																												
LEFT JOIN VPM_YoyKbn AS YOU_YOYKBN 																												
ON YOU_YOYKBN.YoyaKbnSeq = YY_YYKSHO.YoyaKbnSeq 																												
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
GO