USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_AccessoryFeeListDetail_R]    Script Date: 2021/04/27 14:53:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[Pro_AccessoryFeeListDetail_R]
					@startDate varchar(8),
					@endDate varchar(8),
					@dateType int,
					@tokuiFrom int, @tokuiTo int, @sitenFrom int, @sitenTo int, -- for customer
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
	
	SELECT Gyosa.GyosyaCd				AS  GyosyaCd		--業者コード																								
		  ,Gyosa.GyosyaNm				AS GyosyaNm			--業者名   																								
		  ,Toku.TokuiCd					AS TokuTokuiCd		--得意先コード																								
		  ,Toshi.SitenCd				AS ToshiSitenCd		--得意先支店コード																								
		  ,Toku.TokuiNm					AS TokuTokuiNm		--得意先名																								
		  ,Toshi.SitenNm				AS ToshiSitenNm		--得意先支店名																								
		  ,Toku.RyakuNm					AS TokuRyakuNm		--得意先略名																								
		  ,Toshi.RyakuNm				AS ToshiRyakuNm		--得意先支店略名																								
		  ,Un.HaiSYmd					AS HaiSYmd			--配車年月日																								
		  ,Un.TouYmd					AS TouYmd			--到着年月日																								
		  ,Ei.RyakuNm					AS BranchName		--営業所略名																								
		  ,Fu.HasYmd					AS HasYmd			--発生年月日																								
		  ,Un.DanTaNm					AS DanTaNm			--団体名																								
		  ,Yo.UkeCd						AS UkeCdText		--受付番号																							
		  ,Fu.FutTumNm					AS FutTumNm			--付帯積込品名																									
		  ,Fu.SeisanNm					AS SeisanNm			--請求区分																									  																											
		  ,Fu.IriRyoNm					AS IriRyoNm			--入料金所名																										
		  ,Fu.DeRyoNm					AS DeRyoNm			--出料金所名																								
		  ,Fu.Suryo						AS Suryo			--数量																							
		  ,Fu.TanKa						AS TanKa			--単価																							
		  ,Fu.UriGakKin					AS UriGakKin		--売上額																								
		  ,Fu.SyaRyoSyo					AS SyaRyoSyo		--消費税																								
		  ,Fu.SyaRyoTes					AS SyaRyoTes		--手数料																								
		  ,Fu.BikoNm					AS BikoNm			--備考        																								
  																													
	FROM TKD_FutTum As Fu			--予約書データ取得																													
	LEFT JOIN TKD_Yyksho As Yo																													
	     ON Fu.UkeNo = Yo.UkeNo																													
	     AND Yo.SiyoKbn =1																													
	     AND Yo.YoyaSyu =1																													
	--得意先データ取得																													
	LEFT JOIN VPM_Tokisk As Toku																													
	     ON Yo.TokuiSeq = Toku.TokuiSeq																													
	     AND Fu.HasYmd BETWEEN Toku.SiyoStaYmd AND Toku.SiyoEndYmd																													
	     AND Yo.UkeYmd>=Toku.SiyoStaYmd																													
	     AND Yo.UkeYmd<=Toku.SiyoEndYmd																													
																														
	--得意先支店データ取得																													
	LEFT JOIN VPM_TokiSt As Toshi																													
	     ON Yo.TokuiSeq = Toshi.TokuiSeq																													
	     AND Yo.SitenCdSeq = Toshi.SitenCdSeq																													
	     AND Fu.HasYmd BETWEEN Toshi.SiyoStaYmd AND Toshi.SiyoEndYmd																													
	     AND Yo.UkeYmd >= Toshi.SiyoStaYmd																													
	     AND Yo.UkeYmd <= Toshi.SiyoEndYmd																													
																														
	--業者データ取得																													
	LEFT JOIN VPM_Gyosya As Gyosa       																													
	     ON Gyosa.GyosyaCdSeq = Toku.GyosyaCdSeq																													
	     AND Gyosa.SiyoKbn = 1																													
																														
	--運行日データ取得																													
	 LEFT JOIN TKD_Unkobi As Un																													
	      ON Fu.UkeNo = Un.UkeNo																													
	      AND Fu.UnkRen = Un.UnkRen																													
	      AND Un.SiyoKbn = 1																													
																														
	--付帯データ取得																													
	 LEFT JOIN VPM_Futai As M_Futai 																													
	      ON Fu.FutTumCdSeq = M_Futai.FutaiCdSeq																													
	      AND M_Futai.SiyoKbn = 1																													
																														
	--営業所データ取得																													
	 LEFT JOIN VPM_Eigyos As Ei																													
	      ON Yo.UkeEigCdSeq = Ei.EigyoCdSeq																													
	      AND Ei.SiyoKbn = 1																													
																														
	--会社データ取得																													
	 LEFT JOIN VPM_Compny As Company																													
	      ON Company.CompanyCdSeq = Ei.CompanyCdSeq 																													
	      AND Company.SiyoKbn = 1																													
																														
	--予約区分データ取得																													
	 LEFT JOIN VPM_YoyKbn As YoyKbn																													
	      ON YoyKbn.YoyaKbnSeq = Yo.YoyaKbnSeq																													
	      AND YoyKbn.SiyoKbn = 1																													
																											
	WHERE Fu.FutTumKbn=1 																													
		  AND Fu.SiyoKbn=1	
		  AND Yo.TenantCdSeq = @tenantId	
		  AND Yo.UkeCd BETWEEN @ukeCdFrom AND @ukeCdTo---------------------画面のNo.15、16で入力した受付番号
		  AND
		  ( --Compare date type         画面のNo.1で「配車年月日」を指定した場合
		     (@dateType = 1 AND Un.HaiSYmd BETWEEN @startDate AND @endDate)
		     OR (@dateType = 2 AND Un.TouYmd BETWEEN @startDate AND @endDate)
		  	 OR (@dateType = 3 AND Fu.HasYmd BETWEEN @startDate AND @endDate)
		  ) AND
		  ( --Compare customer				画面のNo.8,9で指定した得意先
		  	(@tokuiFrom = 0 and @tokuiTo = 0)
		  	or
		  	(@tokuiFrom <> @tokuiTo and Yo.TokuiSeq = @tokuiFrom and Yo.SitenCdSeq >= @sitenFrom)
		  	or
		  	(@tokuiFrom <> @tokuiTo and Yo.TokuiSeq = @tokuiTo and Yo.SitenCdSeq <= @sitenTo)
		  	or
		  	(@tokuiFrom = @tokuiTo and Yo.TokuiSeq = @tokuiFrom and Yo.SitenCdSeq >= @sitenFrom and Yo.SitenCdSeq <= @sitenTo)
		  	or
		  	(@tokuiFrom = 0 and @tokuiTo <> 0 and ((Yo.TokuiSeq = @tokuiTo and Yo.SitenCdSeq <= @sitenTo) or Yo.TokuiSeq < @tokuiTo))
		  	or
		  	(@tokuiTo = 0 and @tokuiFrom <> 0 and ((Yo.TokuiSeq = @tokuiFrom and Yo.SitenCdSeq >= @sitenFrom) or Yo.TokuiSeq > @tokuiFrom))
		  	or
		  	(Yo.TokuiSeq < @tokuiTo and Yo.TokuiSeq > @tokuiFrom)
		  ) 																												
		  AND Fu.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-')) ------------------------------------------画面のNo.10で指定した清算区分																													
		  AND (@companyId = 0 OR Company.CompanyCdSeq = @companyId)--------------------------------------------画面のNo.10で指定した会社																													
		  AND Yo.UkeEigCdSeq  BETWEEN @brandStart AND @brandEnd------------------------------画面のNo.12 ,13で指定した営業所																													
		  AND Fu.FutTumCdSeq BETWEEN @futTumStart AND @futTumEnd-------------------------------画面のNo.17,18で指定した付帯料金コード																													
		  AND Yo.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))----------------------------------画面のNo.14で指定した予約区分																													
		  AND M_Futai.FutGuiKbn IN 
			  (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN M_Futai.FutGuiKbn != 5 THEN (SELECT M_Futai.FutGuiKbn) END ) -- input empty => select all
					WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE M_Futai.FutGuiKbn = fc.FutaiType) 
			   END)---------------------------------------画面のNo.19で選択した付帯種別	
	ORDER BY Un.HaiSYmd ASC																													
			,Fu.UkeNo ASC	
END
GO


