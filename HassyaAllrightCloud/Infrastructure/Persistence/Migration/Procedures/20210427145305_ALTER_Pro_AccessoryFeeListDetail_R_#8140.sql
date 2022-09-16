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
	
	SELECT Gyosa.GyosyaCd				AS  GyosyaCd		--�Ǝ҃R�[�h																								
		  ,Gyosa.GyosyaNm				AS GyosyaNm			--�ƎҖ�   																								
		  ,Toku.TokuiCd					AS TokuTokuiCd		--���Ӑ�R�[�h																								
		  ,Toshi.SitenCd				AS ToshiSitenCd		--���Ӑ�x�X�R�[�h																								
		  ,Toku.TokuiNm					AS TokuTokuiNm		--���Ӑ於																								
		  ,Toshi.SitenNm				AS ToshiSitenNm		--���Ӑ�x�X��																								
		  ,Toku.RyakuNm					AS TokuRyakuNm		--���Ӑ旪��																								
		  ,Toshi.RyakuNm				AS ToshiRyakuNm		--���Ӑ�x�X����																								
		  ,Un.HaiSYmd					AS HaiSYmd			--�z�ԔN����																								
		  ,Un.TouYmd					AS TouYmd			--�����N����																								
		  ,Ei.RyakuNm					AS BranchName		--�c�Ə�����																								
		  ,Fu.HasYmd					AS HasYmd			--�����N����																								
		  ,Un.DanTaNm					AS DanTaNm			--�c�̖�																								
		  ,Yo.UkeCd						AS UkeCdText		--��t�ԍ�																							
		  ,Fu.FutTumNm					AS FutTumNm			--�t�ѐύ��i��																									
		  ,Fu.SeisanNm					AS SeisanNm			--�����敪																									  																											
		  ,Fu.IriRyoNm					AS IriRyoNm			--����������																										
		  ,Fu.DeRyoNm					AS DeRyoNm			--�o��������																								
		  ,Fu.Suryo						AS Suryo			--����																							
		  ,Fu.TanKa						AS TanKa			--�P��																							
		  ,Fu.UriGakKin					AS UriGakKin		--����z																								
		  ,Fu.SyaRyoSyo					AS SyaRyoSyo		--�����																								
		  ,Fu.SyaRyoTes					AS SyaRyoTes		--�萔��																								
		  ,Fu.BikoNm					AS BikoNm			--���l        																								
  																													
	FROM TKD_FutTum As Fu			--�\�񏑃f�[�^�擾																													
	LEFT JOIN TKD_Yyksho As Yo																													
	     ON Fu.UkeNo = Yo.UkeNo																													
	     AND Yo.SiyoKbn =1																													
	     AND Yo.YoyaSyu =1																													
	--���Ӑ�f�[�^�擾																													
	LEFT JOIN VPM_Tokisk As Toku																													
	     ON Yo.TokuiSeq = Toku.TokuiSeq																													
	     AND Fu.HasYmd BETWEEN Toku.SiyoStaYmd AND Toku.SiyoEndYmd																													
	     AND Yo.UkeYmd>=Toku.SiyoStaYmd																													
	     AND Yo.UkeYmd<=Toku.SiyoEndYmd																													
																														
	--���Ӑ�x�X�f�[�^�擾																													
	LEFT JOIN VPM_TokiSt As Toshi																													
	     ON Yo.TokuiSeq = Toshi.TokuiSeq																													
	     AND Yo.SitenCdSeq = Toshi.SitenCdSeq																													
	     AND Fu.HasYmd BETWEEN Toshi.SiyoStaYmd AND Toshi.SiyoEndYmd																													
	     AND Yo.UkeYmd >= Toshi.SiyoStaYmd																													
	     AND Yo.UkeYmd <= Toshi.SiyoEndYmd																													
																														
	--�Ǝ҃f�[�^�擾																													
	LEFT JOIN VPM_Gyosya As Gyosa       																													
	     ON Gyosa.GyosyaCdSeq = Toku.GyosyaCdSeq																													
	     AND Gyosa.SiyoKbn = 1																													
																														
	--�^�s���f�[�^�擾																													
	 LEFT JOIN TKD_Unkobi As Un																													
	      ON Fu.UkeNo = Un.UkeNo																													
	      AND Fu.UnkRen = Un.UnkRen																													
	      AND Un.SiyoKbn = 1																													
																														
	--�t�уf�[�^�擾																													
	 LEFT JOIN VPM_Futai As M_Futai 																													
	      ON Fu.FutTumCdSeq = M_Futai.FutaiCdSeq																													
	      AND M_Futai.SiyoKbn = 1																													
																														
	--�c�Ə��f�[�^�擾																													
	 LEFT JOIN VPM_Eigyos As Ei																													
	      ON Yo.UkeEigCdSeq = Ei.EigyoCdSeq																													
	      AND Ei.SiyoKbn = 1																													
																														
	--��Ѓf�[�^�擾																													
	 LEFT JOIN VPM_Compny As Company																													
	      ON Company.CompanyCdSeq = Ei.CompanyCdSeq 																													
	      AND Company.SiyoKbn = 1																													
																														
	--�\��敪�f�[�^�擾																													
	 LEFT JOIN VPM_YoyKbn As YoyKbn																													
	      ON YoyKbn.YoyaKbnSeq = Yo.YoyaKbnSeq																													
	      AND YoyKbn.SiyoKbn = 1																													
																											
	WHERE Fu.FutTumKbn=1 																													
		  AND Fu.SiyoKbn=1	
		  AND Yo.TenantCdSeq = @tenantId	
		  AND Yo.UkeCd BETWEEN @ukeCdFrom AND @ukeCdTo---------------------��ʂ�No.15�A16�œ��͂�����t�ԍ�
		  AND
		  ( --Compare date type         ��ʂ�No.1�Łu�z�ԔN�����v���w�肵���ꍇ
		     (@dateType = 1 AND Un.HaiSYmd BETWEEN @startDate AND @endDate)
		     OR (@dateType = 2 AND Un.TouYmd BETWEEN @startDate AND @endDate)
		  	 OR (@dateType = 3 AND Fu.HasYmd BETWEEN @startDate AND @endDate)
		  ) AND
		  ( --Compare customer				��ʂ�No.8,9�Ŏw�肵�����Ӑ�
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
		  AND Fu.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-')) ------------------------------------------��ʂ�No.10�Ŏw�肵�����Z�敪																													
		  AND (@companyId = 0 OR Company.CompanyCdSeq = @companyId)--------------------------------------------��ʂ�No.10�Ŏw�肵�����																													
		  AND Yo.UkeEigCdSeq  BETWEEN @brandStart AND @brandEnd------------------------------��ʂ�No.12 ,13�Ŏw�肵���c�Ə�																													
		  AND Fu.FutTumCdSeq BETWEEN @futTumStart AND @futTumEnd-------------------------------��ʂ�No.17,18�Ŏw�肵���t�ї����R�[�h																													
		  AND Yo.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))----------------------------------��ʂ�No.14�Ŏw�肵���\��敪																													
		  AND M_Futai.FutGuiKbn IN 
			  (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN M_Futai.FutGuiKbn != 5 THEN (SELECT M_Futai.FutGuiKbn) END ) -- input empty => select all
					WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE M_Futai.FutGuiKbn = fc.FutaiType) 
			   END)---------------------------------------��ʂ�No.19�őI�������t�ю��	
	ORDER BY Un.HaiSYmd ASC																													
			,Fu.UkeNo ASC	
END
GO


