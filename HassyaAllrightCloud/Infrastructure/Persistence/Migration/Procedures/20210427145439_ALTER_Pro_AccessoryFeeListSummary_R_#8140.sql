USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_AccessoryFeeListSummary_R]    Script Date: 2021/04/27 14:54:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[Pro_AccessoryFeeListSummary_R]
					@startDate varchar(8),
					@endDate varchar(8),
					@dateType int,
					@tokuiFrom int, @tokuiTo int, @sitenFrom int, @sitenTo int, -- for customer
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


	SELECT YYKSHO.UkeNo           AS UkeNo		--'受付番号'																																
      ,YYKSHO.UkeCd               AS UkeCdText	--'受付コード'																																
	  ,UNKOBI.UnkRen              AS UnkRen		--'運行日連番'																															
	  ,UNKOBI.HaiSYmd             AS HaiSYmd	--'配車年月日'																															
	  ,UNKOBI.TouYmd              AS TouYmd		--'到着年月日'																															
	  ,EIGYOS.RyakuNm             AS BranchName	--'営業所名'																															
	  ,UNKOBI.DanTaNm             AS DanTaNm	--'団体名'	
	  ,TOKISK.TokuiCd			  AS TokuTokuiCd	--得意先コード																								
	  ,TOKIST.SitenCd			  AS ToshiSitenCd	--得意先支店コード
	  ,TOKISK.RyakuNm			  AS TokuRyakuNm	--得意先略名																								
	  ,TOKIST.RyakuNm			  AS ToshiRyakuNm	--得意先支店略名	
	  ,Gyosa.GyosyaCd			  AS GyosyaCd		--業者コード	
	  ,(SELECT SUM (FUTTUM.Suryo) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1：付帯																													
---------画面の清算区分に選択した値																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
--------画面の付帯種別に選択した値																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:付帯 3:通行料 4:手配料 5:ガイド料 コード区分マスタ（システム：FUTGUIKBN）																													
---------画面の付帯料金コードに選択した値（FROM/TO）																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS Suryo	--'数量'																														
	  ,(SELECT SUM (FUTTUM.SyaRyoTes) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1：付帯																													
---------画面の清算区分に選択した値																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
--------画面の付帯種別に選択した値																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:付帯 3:通行料 4:手配料 5:ガイド料 コード区分マスタ（システム：FUTGUIKBN）																													
---------画面の付帯料金コードに選択した値（FROM/TO）																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS SyaRyoTes	--'手数料'																														
	  ,(SELECT SUM (FUTTUM.SyaRyoSyo) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1：付帯																													
--------画面の清算区分に選択した値																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
---------画面の付帯種別に選択した値																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:付帯 3:通行料 4:手配料 5:ガイド料 コード区分マスタ（システム：FUTGUIKBN）																													
---------画面の付帯料金コードに選択した値（FROM/TO）																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS SyaRyoSyo	--'消費税'																														
      ,(SELECT SUM (FUTTUM.UriGakKin) 																																
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1：付帯																													
--------画面の清算区分に選択した値																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
---------画面の付帯種別に選択した値																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:付帯 3:通行料 4:手配料 5:ガイド料 コード区分マスタ（システム：FUTGUIKBN）																													
---------画面の付帯料金コードに選択した値（FROM/TO）																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS UriGakKin	--'金額'																														
FROM TKD_Unkobi AS UNKOBI																																
INNER JOIN TKD_Yyksho AS YYKSHO																																
        ON YYKSHO.UkeNo = UNKOBI.UkeNo																																
	   AND YYKSHO.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq																															
INNER JOIN VPM_Tokisk AS TOKISK																																
       ON TOKISK.TokuiSeq = YYKSHO.TokuiSeq																																
      AND TOKISK.SiyoStaYmd <= YYKSHO.UkeYmd																																
	  AND TOKISK.SiyoEndYmd >= YYKSHO.UkeYmd																															
	  AND TOKISK.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq																															
INNER JOIN VPM_TokiSt AS TOKIST																																
        ON TOKIST.TokuiSeq = YYKSHO.TokuiSeq																																
	   AND TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq																															
	   AND TOKIST.SiyoStaYmd <= YYKSHO.UkeYmd																															
	   AND TOKIST.SiyoEndYmd >= YYKSHO.UkeYmd																												
LEFT JOIN VPM_Gyosya As Gyosa       																													
	    ON Gyosa.GyosyaCdSeq = TOKISK.GyosyaCdSeq																													
	    AND Gyosa.SiyoKbn = 1
LEFT JOIN VPM_Eigyos AS EIGYOS																																
       ON EIGYOS.EigyoCdSeq = YYKSHO.UkeEigCdSeq																																
LEFT JOIN VPM_Compny AS KAISHA																																
       ON KAISHA.CompanyCdSeq = EIGYOS.CompanyCdSeq																																
	  AND KAISHA.TenantCdSeq = @tenantId        --ログインユーザーのテナントコードSeq		
	  --予約区分データ取得																													
LEFT JOIN VPM_YoyKbn As YoyKbn																													
       ON YoyKbn.YoyaKbnSeq = YYKSHO.YoyaKbnSeq																													
      AND YoyKbn.SiyoKbn = 1
WHERE UNKOBI.SiyoKbn = 1 AND YYKSHO.SiyoKbn = 1	
  AND YYKSHO.UkeCd BETWEEN @ukeCdFrom AND @ukeCdTo
  AND YYKSHO.TenantCdSeq = @tenantId																																
  AND (YYKSHO.YoyaSyu = 1 OR YYKSHO.YoyaSyu = 3)         --1：受注	
  AND
--１／　画面の日付指定に【配車年月日】の場合																																
--- AND UNKOBI.HaiSYmd BETWEEN 20201106 AND 20201110      --画面の対象日付（FROM/TO）																																
--２／　画面の日付指定に【到着年月日】の場合																																
-- AND UNKOBI.TouYmd BETWEEN 20201106 AND 20201110      --画面の対象日付（FROM/TO）
( --Compare date type         画面のNo.1で「配車年月日」を指定した場合
	(@dateType = 1 AND UNKOBI.HaiSYmd BETWEEN @startDate AND @endDate)
	OR (@dateType = 2 AND UNKOBI.TouYmd BETWEEN @startDate AND @endDate)
)
---------------------------																																
AND (SELECT COUNT(*) 																																
       FROM TKD_FutTum 																																
	 INNER JOIN VPM_Futai																														
	        ON VPM_Futai.FutaiCdSeq = TKD_FutTum.FutTumCdSeq																														
		   AND VPM_Futai.TenantCdSeq = @tenantId  --ログインユーザーのテナントコードSeq																													
	WHERE TKD_FutTum.UkeNo = UNKOBI.UkeNo 																														
	  AND TKD_FutTum.UnkRen = UNKOBI.UnkRen																														
	  AND TKD_FutTum.FutTumKbn = 1              --1:付帯入力																														
	  ---------画面の清算区分に選択した値-------------																																
	  AND SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																														
	  --画面の付帯種別に選択した値																																
	  AND VPM_Futai.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN VPM_Futai.FutGuiKbn != 5 THEN (SELECT VPM_Futai.FutGuiKbn) END ) -- input empty => select all
									   WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE VPM_Futai.FutGuiKbn = fc.FutaiType) 
									END)       --2:付帯 3:通行料 4:手配料 5:ガイド料 コード区分マスタ（システム：FUTGUIKBN）																														
	  --画面の付帯料金コードに選択した値（FROM/TO）																																
      AND VPM_Futai.FutaiCd BETWEEN @futTumStart AND @futTumEnd																															
	  ------------------------------------------------																																
	  AND TKD_FutTum.SiyoKbn = 1																														
	  --３／　画面の日付指定に【発生年月日】の場合																																
        --AND HaiSYmd BETWEEN 20201106 AND 20201110      --画面の対象日付（FROM/TO）	
	  OR (@dateType = 3 AND HaiSYmd BETWEEN @startDate AND @endDate)
	  ) > 0		 																												
  --画面の得意先が指定する場合	
  AND
  ( --Compare customer				画面のNo.8,9で指定した得意先
	(@tokuiFrom = 0 AND @tokuiTo = 0)
	OR
	(@tokuiFrom <> @tokuiTo AND TOKISK.TokuiCd = @tokuiFrom AND TOKIST.SitenCd >= @sitenFrom)
	OR
	(@tokuiFrom <> @tokuiTo AND TOKISK.TokuiCd = @tokuiTo AND TOKIST.SitenCd <= @sitenTo)
	OR
	(@tokuiFrom = @tokuiTo AND TOKISK.TokuiCd = @tokuiFrom AND TOKIST.SitenCd >= @sitenFrom AND TOKIST.SitenCd <= @sitenTo)
	OR
	(@tokuiFrom = 0 AND @tokuiTo <> 0 AND ((TOKISK.TokuiCd = @tokuiTo AND TOKIST.SitenCd <= @sitenTo) or TOKISK.TokuiCd < @tokuiTo))
	OR
	(@tokuiTo = 0 AND @tokuiFrom <> 0 AND ((TOKISK.TokuiCd = @tokuiFrom AND TOKIST.SitenCd >= @sitenFrom) or TOKISK.TokuiCd > @tokuiFrom))
	OR
	(TOKISK.TokuiCd < @tokuiTo AND TOKISK.TokuiCd > @tokuiFrom)
  )																															
  --画面の会社コードに選択した値																																
  AND (@companyId = 0 OR KAISHA.CompanyCd = @companyId)																																
  --画面の営業所に選択した値（FROM/TO）																																
  AND EIGYOS.EigyoCd BETWEEN @brandStart AND @brandEnd																																																															
  --画面の予約区分に選択した値																																
  AND YoyKbn.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))--画面の受付番号に入力した値（FROM／TO）（受付コード）																																
ORDER BY YYKSHO.UkeNo ASC																																
        ,UNKOBI.HaiSYmd ASC																										
END
GO


