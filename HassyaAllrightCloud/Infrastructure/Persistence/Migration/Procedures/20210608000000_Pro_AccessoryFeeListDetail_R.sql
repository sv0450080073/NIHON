USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_AccessoryFeeListDetail_R]    Script Date: 6/8/2021 9:42:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROC [dbo].[Pro_AccessoryFeeListDetail_R]
					@startDate varchar(8),
					@endDate varchar(8),
					@dateType int,
					@customerFrom varchar(11),
					@customerTo varchar(11), -- for customer
					@invoiceTypes varchar(max),
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
	
	SELECT Gyosa.GyosyaCd				AS  GyosyaCd																							
		  ,Gyosa.GyosyaNm				AS GyosyaNm																								
		  ,Toku.TokuiCd					AS TokuTokuiCd																								
		  ,Toshi.SitenCd				AS ToshiSitenCd																									
		  ,Toku.TokuiNm					AS TokuTokuiNm																							
		  ,Toshi.SitenNm				AS ToshiSitenNm																								
		  ,Toku.RyakuNm					AS TokuRyakuNm																							
		  ,Toshi.RyakuNm				AS ToshiRyakuNm																								
		  ,Un.HaiSYmd					AS HaiSYmd																								
		  ,Un.TouYmd					AS TouYmd																									
		  ,Ei.RyakuNm					AS BranchName																							
		  ,Fu.HasYmd					AS HasYmd																									
		  ,Un.DanTaNm					AS DanTaNm																							
		  ,Yo.UkeCd						AS UkeCdText																						
		  ,Fu.FutTumNm					AS FutTumNm																										
		  ,Fu.SeisanNm					AS SeisanNm			
		  ,Fu.SeisanKbn					AS CodeKbnSeisan
		  ,Fu.IriRyoNm					AS IriRyoNm																											
		  ,Fu.DeRyoNm					AS DeRyoNm																								
		  ,Fu.Suryo						AS Suryo																						
		  ,Fu.TanKa						AS TanKa																						
		  ,Fu.UriGakKin					AS UriGakKin																						
		  ,Fu.SyaRyoSyo					AS SyaRyoSyo																						
		  ,Fu.SyaRyoTes					AS SyaRyoTes																						
		  ,Fu.BikoNm					AS BikoNm																									
  																													
	FROM TKD_FutTum As Fu																														
	LEFT JOIN TKD_Yyksho As Yo																													
	     ON Fu.UkeNo = Yo.UkeNo																													
	     AND Yo.SiyoKbn =1																													
	     AND Yo.YoyaSyu =1				
	     AND Yo.TenantCdSeq = @tenantId

	LEFT JOIN VPM_Tokisk As Toku																													
	     ON Yo.TokuiSeq = Toku.TokuiSeq																													
	     AND Fu.HasYmd BETWEEN Toku.SiyoStaYmd AND Toku.SiyoEndYmd																													
	     AND Yo.UkeYmd>=Toku.SiyoStaYmd																													
	     AND Yo.UkeYmd<=Toku.SiyoEndYmd																																											 
		 AND Toku.TenantCdSeq = @tenantId
														
	LEFT JOIN VPM_TokiSt As Toshi																													
	     ON Toku.TokuiSeq = Toshi.TokuiSeq																													
	     AND Yo.SitenCdSeq = Toshi.SitenCdSeq																													
	     AND Fu.HasYmd BETWEEN Toshi.SiyoStaYmd AND Toshi.SiyoEndYmd																													
	     AND Yo.UkeYmd >= Toshi.SiyoStaYmd																													
	     AND Yo.UkeYmd <= Toshi.SiyoEndYmd																													
																													
	LEFT JOIN VPM_Gyosya As Gyosa       																													
	     ON Gyosa.GyosyaCdSeq = Toku.GyosyaCdSeq																													
	     AND Gyosa.SiyoKbn = 1		
		 AND Gyosa.TenantCdSeq = @tenantId
																													
	 LEFT JOIN TKD_Unkobi As Un																													
	      ON Fu.UkeNo = Un.UkeNo																													
	      AND Fu.UnkRen = Un.UnkRen																													
	      AND Un.SiyoKbn = 1																													
																												
	 LEFT JOIN VPM_Futai As M_Futai 																													
	      ON Fu.FutTumCdSeq = M_Futai.FutaiCdSeq																													
	      AND M_Futai.SiyoKbn = 1																													
		  AND M_Futai.TenantCdSeq = @tenantId
																													
	 LEFT JOIN VPM_Eigyos As Ei																													
	      ON Yo.UkeEigCdSeq = Ei.EigyoCdSeq																													
	      AND Ei.SiyoKbn = 1																													
																											
	 LEFT JOIN VPM_Compny As Company																													
	      ON Company.CompanyCdSeq = Ei.CompanyCdSeq 																													
	      AND Company.SiyoKbn = 1	
		  AND Company.TenantCdSeq = @tenantId

																												
	 LEFT JOIN VPM_YoyKbn As YoyKbn																													
	      ON YoyKbn.YoyaKbnSeq = Yo.YoyaKbnSeq																													
	      AND YoyKbn.SiyoKbn = 1	
		  AND YoyKbn.TenantCdSeq = @tenantId
																											
	WHERE Fu.FutTumKbn=1 																													
		  AND Fu.SiyoKbn=1	
		  AND Yo.TenantCdSeq = @tenantId	
		  AND Yo.UkeCd BETWEEN @ukeCdFrom AND @ukeCdTo---------------------No.15
		  AND
		  ( --Compare date type      
		     (@dateType = 1 AND Un.HaiSYmd BETWEEN @startDate AND @endDate)
		     OR (@dateType = 2 AND Un.TouYmd BETWEEN @startDate AND @endDate)
		  	 OR (@dateType = 3 AND Fu.HasYmd BETWEEN @startDate AND @endDate)
		  ) 
		  AND (FORMAT(Gyosa.GyosyaCd,'000') + FORMAT(Toku.TokuiCd,'0000') + FORMAT(Toshi.SitenCd,'0000')) >= @customerFrom 
	      AND (FORMAT(Gyosa.GyosyaCd,'000') + FORMAT(Toku.TokuiCd,'0000') + FORMAT(Toshi.SitenCd,'0000')) <= @customerTo	
		  AND Fu.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-')) ------------------------------------------No.10																												
		  AND (@companyId = 0 OR Company.CompanyCdSeq = @companyId)--------------------------------------------No.10																												
		  AND Yo.UkeEigCdSeq  BETWEEN @brandStart AND @brandEnd------------------------------No.12 ,13																													
		  AND Fu.FutTumCdSeq BETWEEN @futTumStart AND @futTumEnd-------------------------------No.17,18																													
		  AND YoyKbn.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))----------------------------------No.14																													
		  AND M_Futai.FutGuiKbn IN 
			  (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN M_Futai.FutGuiKbn != 5 THEN (SELECT M_Futai.FutGuiKbn) END ) -- input empty => select all
					WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE M_Futai.FutGuiKbn = fc.FutaiType) 
			   END)---------------------------------------No.19	
	ORDER BY Un.HaiSYmd ASC																													
			,Fu.UkeNo ASC	
END
