USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_AccessoryFeeListSummary_R]    Script Date: 6/8/2021 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROC [dbo].[Pro_AccessoryFeeListSummary_R]
					@startDate varchar(8),
					@endDate varchar(8),
					@dateType int,
					@customerFrom varchar(11),
					@customerTo varchar(11), -- for customer
					@invoiceTypes varchar(10),
					@companyId int,
					@brandStart int, @brandEnd int,
					@futTumStart int, @futTumEnd int,
					@bookingTypes varchar(max),
					@ukeCdFrom varchar(10), @ukeCdTo varchar(10),
					@futaiTypes varchar(max),
					@tenantId int
AS
BEGIN
	DECLARE @FutaiTypesTbl TABLE (FutaiType VARCHAR(10));
	  INSERT INTO @FutaiTypesTbl (FutaiType)
	    SELECT value FROM STRING_SPLIT(@futaiTypes,'-') WHERE value <> '';


	SELECT YYKSHO.UkeNo           AS UkeNo																																		
      ,YYKSHO.UkeCd               AS UkeCdText																																
	  ,UNKOBI.UnkRen              AS UnkRen																																	
	  ,UNKOBI.HaiSYmd             AS HaiSYmd																															
	  ,UNKOBI.TouYmd              AS TouYmd																																
	  ,EIGYOS.RyakuNm             AS BranchName																															
	  ,UNKOBI.DanTaNm             AS DanTaNm	
	  ,TOKISK.TokuiCd			  AS TokuTokuiCd																								
	  ,TOKIST.SitenCd			  AS ToshiSitenCd	
	  ,TOKISK.RyakuNm			  AS TokuRyakuNm																							
	  ,TOKIST.RyakuNm			  AS ToshiRyakuNm	
	  ,Gyosa.GyosyaCd			  AS GyosyaCd		
	  ,(SELECT SUM (FUTTUM.Suryo) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        																											
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1    																																																										
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																																																												
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)																																																												
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS Suryo																														
	  ,(SELECT SUM (FUTTUM.SyaRyoTes) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId 																											
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1 																																																											
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																																																											
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)   																																																									
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS SyaRyoTes																													
	  ,(SELECT SUM (FUTTUM.SyaRyoSyo) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId 																											
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1    																																																											
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																																																												
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)																																																									
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS SyaRyoSyo																													
      ,(SELECT SUM (FUTTUM.UriGakKin) 																																
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        																											
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1																																																											
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																																																												
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END) 																																																										
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS UriGakKin																												
FROM TKD_Unkobi AS UNKOBI																																
INNER JOIN TKD_Yyksho AS YYKSHO																																
        ON YYKSHO.UkeNo = UNKOBI.UkeNo																																
	   AND YYKSHO.TenantCdSeq = @tenantId       																													
INNER JOIN VPM_Tokisk AS TOKISK																																
       ON TOKISK.TokuiSeq = YYKSHO.TokuiSeq																																
      AND TOKISK.SiyoStaYmd <= YYKSHO.UkeYmd																																
	  AND TOKISK.SiyoEndYmd >= YYKSHO.UkeYmd																															
	  AND TOKISK.TenantCdSeq = @tenantId       																														
INNER JOIN VPM_TokiSt AS TOKIST																																
        ON TOKIST.TokuiSeq = YYKSHO.TokuiSeq																																
	   AND TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq																															
	   AND TOKIST.SiyoStaYmd <= YYKSHO.UkeYmd																															
	   AND TOKIST.SiyoEndYmd >= YYKSHO.UkeYmd																												
LEFT JOIN VPM_Gyosya As Gyosa       																													
	    ON Gyosa.GyosyaCdSeq = TOKISK.GyosyaCdSeq																													
	    AND Gyosa.SiyoKbn = 1
		AND Gyosa.TenantCdSeq = @tenantId
LEFT JOIN VPM_Eigyos AS EIGYOS																																
       ON EIGYOS.EigyoCdSeq = YYKSHO.UkeEigCdSeq																																
LEFT JOIN VPM_Compny AS KAISHA																																
       ON KAISHA.CompanyCdSeq = EIGYOS.CompanyCdSeq																																
	  AND KAISHA.TenantCdSeq = @tenantId        																													
LEFT JOIN VPM_YoyKbn As YoyKbn																													
       ON YoyKbn.YoyaKbnSeq = YYKSHO.YoyaKbnSeq																													
      AND YoyKbn.SiyoKbn = 1
	  AND YoyKbn.TenantCdSeq = @tenantId
WHERE UNKOBI.SiyoKbn = 1 AND YYKSHO.SiyoKbn = 1	
  AND YYKSHO.UkeCd BETWEEN @ukeCdFrom AND @ukeCdTo
  AND YYKSHO.TenantCdSeq = @tenantId																																
  AND (YYKSHO.YoyaSyu = 1 OR YYKSHO.YoyaSyu = 3)         
  AND
( --Compare date type  
	(@dateType = 1 AND UNKOBI.HaiSYmd BETWEEN @startDate AND @endDate)
	OR (@dateType = 2 AND UNKOBI.TouYmd BETWEEN @startDate AND @endDate)
	OR (SELECT COUNT(*) 																																
       FROM TKD_FutTum 																																
	 INNER JOIN VPM_Futai																														
	        ON VPM_Futai.FutaiCdSeq = TKD_FutTum.FutTumCdSeq																														
		   AND VPM_Futai.TenantCdSeq = @tenantId  																												
	WHERE TKD_FutTum.UkeNo = UNKOBI.UkeNo 																														
	  AND TKD_FutTum.UnkRen = UNKOBI.UnkRen																														
	  AND TKD_FutTum.FutTumKbn = 1    																																																												
	  AND SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																																																														
	  AND VPM_Futai.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN VPM_Futai.FutGuiKbn != 5 THEN (SELECT VPM_Futai.FutGuiKbn) END ) -- input empty => select all
									   WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE VPM_Futai.FutGuiKbn = fc.FutaiType) 
									END)    																																																												
      AND VPM_Futai.FutaiCd BETWEEN @futTumStart AND @futTumEnd																															
	  ------------------------------------------------																																
	  AND TKD_FutTum.SiyoKbn = 1																														
	  OR (@dateType = 3 AND HaiSYmd BETWEEN @startDate AND @endDate)
	  ) > 0
)
AND (SELECT COUNT(*) 																																
       FROM TKD_FutTum 																																
	 INNER JOIN VPM_Futai																														
	        ON VPM_Futai.FutaiCdSeq = TKD_FutTum.FutTumCdSeq																														
		   AND VPM_Futai.TenantCdSeq = @tenantId  																												
	WHERE TKD_FutTum.UkeNo = UNKOBI.UkeNo 																														
	  AND TKD_FutTum.UnkRen = UNKOBI.UnkRen																														
	  AND TKD_FutTum.FutTumKbn = 1    																																																												
	  AND SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																																																														
	  AND VPM_Futai.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN VPM_Futai.FutGuiKbn != 5 THEN (SELECT VPM_Futai.FutGuiKbn) END ) -- input empty => select all
									   WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE VPM_Futai.FutGuiKbn = fc.FutaiType) 
									END)    																																																												
      AND VPM_Futai.FutaiCd BETWEEN @futTumStart AND @futTumEnd																															
	  ------------------------------------------------																																
	  AND TKD_FutTum.SiyoKbn = 1																														
	  ) > 0
---------------------------																																		 																												
  AND (FORMAT(Gyosa.GyosyaCd,'000') + FORMAT(TOKISK.TokuiCd,'0000') + FORMAT(TOKIST.SitenCd,'0000')) >= @customerFrom 
  AND (FORMAT(Gyosa.GyosyaCd,'000') + FORMAT(TOKISK.TokuiCd,'0000') + FORMAT(TOKIST.SitenCd,'0000')) <= @customerTo																															
 																														
  AND (@companyId = 0 OR KAISHA.CompanyCd = @companyId)																																
  																															
  AND EIGYOS.EigyoCd BETWEEN @brandStart AND @brandEnd																																																															
 																														
  AND YoyKbn.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))																															
ORDER BY YYKSHO.UkeNo ASC																																
        ,UNKOBI.HaiSYmd ASC																										
END
