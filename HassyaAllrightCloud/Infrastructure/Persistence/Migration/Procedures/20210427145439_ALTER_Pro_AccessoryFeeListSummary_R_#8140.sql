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


	SELECT YYKSHO.UkeNo           AS UkeNo		--'��t�ԍ�'																																
      ,YYKSHO.UkeCd               AS UkeCdText	--'��t�R�[�h'																																
	  ,UNKOBI.UnkRen              AS UnkRen		--'�^�s���A��'																															
	  ,UNKOBI.HaiSYmd             AS HaiSYmd	--'�z�ԔN����'																															
	  ,UNKOBI.TouYmd              AS TouYmd		--'�����N����'																															
	  ,EIGYOS.RyakuNm             AS BranchName	--'�c�Ə���'																															
	  ,UNKOBI.DanTaNm             AS DanTaNm	--'�c�̖�'	
	  ,TOKISK.TokuiCd			  AS TokuTokuiCd	--���Ӑ�R�[�h																								
	  ,TOKIST.SitenCd			  AS ToshiSitenCd	--���Ӑ�x�X�R�[�h
	  ,TOKISK.RyakuNm			  AS TokuRyakuNm	--���Ӑ旪��																								
	  ,TOKIST.RyakuNm			  AS ToshiRyakuNm	--���Ӑ�x�X����	
	  ,Gyosa.GyosyaCd			  AS GyosyaCd		--�Ǝ҃R�[�h	
	  ,(SELECT SUM (FUTTUM.Suryo) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1�F�t��																													
---------��ʂ̐��Z�敪�ɑI�������l																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
--------��ʂ̕t�ю�ʂɑI�������l																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:�t�� 3:�ʍs�� 4:��z�� 5:�K�C�h�� �R�[�h�敪�}�X�^�i�V�X�e���FFUTGUIKBN�j																													
---------��ʂ̕t�ї����R�[�h�ɑI�������l�iFROM/TO�j																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS Suryo	--'����'																														
	  ,(SELECT SUM (FUTTUM.SyaRyoTes) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1�F�t��																													
---------��ʂ̐��Z�敪�ɑI�������l																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
--------��ʂ̕t�ю�ʂɑI�������l																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:�t�� 3:�ʍs�� 4:��z�� 5:�K�C�h�� �R�[�h�敪�}�X�^�i�V�X�e���FFUTGUIKBN�j																													
---------��ʂ̕t�ї����R�[�h�ɑI�������l�iFROM/TO�j																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS SyaRyoTes	--'�萔��'																														
	  ,(SELECT SUM (FUTTUM.SyaRyoSyo) 																															
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1�F�t��																													
--------��ʂ̐��Z�敪�ɑI�������l																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
---------��ʂ̕t�ю�ʂɑI�������l																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:�t�� 3:�ʍs�� 4:��z�� 5:�K�C�h�� �R�[�h�敪�}�X�^�i�V�X�e���FFUTGUIKBN�j																													
---------��ʂ̕t�ї����R�[�h�ɑI�������l�iFROM/TO�j																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS SyaRyoSyo	--'�����'																														
      ,(SELECT SUM (FUTTUM.UriGakKin) 																																
	      FROM TKD_FutTum AS FUTTUM																															
		  INNER JOIN VPM_Futai AS FUTAI																														
		         ON FUTAI.FutaiCdSeq = FUTTUM.FutTumCdSeq																														
				AND FUTAI.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																												
		  WHERE FUTTUM.UkeNo = UNKOBI.UkeNo																														
		    AND FUTTUM.UnkRen = UNKOBI.UnkRen																														
		    AND FUTTUM.SiyoKbn = 1																														
			AND FUTTUM.FutTumKbn = 1      --1�F�t��																													
--------��ʂ̐��Z�敪�ɑI�������l																																
			AND FUTTUM.SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																													
---------��ʂ̕t�ю�ʂɑI�������l																																
			AND FUTAI.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN FUTAI.FutGuiKbn != 5 THEN (SELECT FUTAI.FutGuiKbn) END) -- input empty => select all
										 WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE FUTAI.FutGuiKbn = fc.FutaiType) 
									END)       --2:�t�� 3:�ʍs�� 4:��z�� 5:�K�C�h�� �R�[�h�敪�}�X�^�i�V�X�e���FFUTGUIKBN�j																													
---------��ʂ̕t�ї����R�[�h�ɑI�������l�iFROM/TO�j																																
			AND FUTAI.FutaiCd BETWEEN @futTumStart AND @futTumEnd																													
		) AS UriGakKin	--'���z'																														
FROM TKD_Unkobi AS UNKOBI																																
INNER JOIN TKD_Yyksho AS YYKSHO																																
        ON YYKSHO.UkeNo = UNKOBI.UkeNo																																
	   AND YYKSHO.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																															
INNER JOIN VPM_Tokisk AS TOKISK																																
       ON TOKISK.TokuiSeq = YYKSHO.TokuiSeq																																
      AND TOKISK.SiyoStaYmd <= YYKSHO.UkeYmd																																
	  AND TOKISK.SiyoEndYmd >= YYKSHO.UkeYmd																															
	  AND TOKISK.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																															
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
	  AND KAISHA.TenantCdSeq = @tenantId        --���O�C�����[�U�[�̃e�i���g�R�[�hSeq		
	  --�\��敪�f�[�^�擾																													
LEFT JOIN VPM_YoyKbn As YoyKbn																													
       ON YoyKbn.YoyaKbnSeq = YYKSHO.YoyaKbnSeq																													
      AND YoyKbn.SiyoKbn = 1
WHERE UNKOBI.SiyoKbn = 1 AND YYKSHO.SiyoKbn = 1	
  AND YYKSHO.UkeCd BETWEEN @ukeCdFrom AND @ukeCdTo
  AND YYKSHO.TenantCdSeq = @tenantId																																
  AND (YYKSHO.YoyaSyu = 1 OR YYKSHO.YoyaSyu = 3)         --1�F��	
  AND
--�P�^�@��ʂ̓��t�w��Ɂy�z�ԔN�����z�̏ꍇ																																
--- AND UNKOBI.HaiSYmd BETWEEN 20201106 AND 20201110      --��ʂ̑Ώۓ��t�iFROM/TO�j																																
--�Q�^�@��ʂ̓��t�w��Ɂy�����N�����z�̏ꍇ																																
-- AND UNKOBI.TouYmd BETWEEN 20201106 AND 20201110      --��ʂ̑Ώۓ��t�iFROM/TO�j
( --Compare date type         ��ʂ�No.1�Łu�z�ԔN�����v���w�肵���ꍇ
	(@dateType = 1 AND UNKOBI.HaiSYmd BETWEEN @startDate AND @endDate)
	OR (@dateType = 2 AND UNKOBI.TouYmd BETWEEN @startDate AND @endDate)
)
---------------------------																																
AND (SELECT COUNT(*) 																																
       FROM TKD_FutTum 																																
	 INNER JOIN VPM_Futai																														
	        ON VPM_Futai.FutaiCdSeq = TKD_FutTum.FutTumCdSeq																														
		   AND VPM_Futai.TenantCdSeq = @tenantId  --���O�C�����[�U�[�̃e�i���g�R�[�hSeq																													
	WHERE TKD_FutTum.UkeNo = UNKOBI.UkeNo 																														
	  AND TKD_FutTum.UnkRen = UNKOBI.UnkRen																														
	  AND TKD_FutTum.FutTumKbn = 1              --1:�t�ѓ���																														
	  ---------��ʂ̐��Z�敪�ɑI�������l-------------																																
	  AND SeisanKbn IN (SELECT * FROM FN_SplitString(@invoiceTypes, '-'))																														
	  --��ʂ̕t�ю�ʂɑI�������l																																
	  AND VPM_Futai.FutGuiKbn IN (CASE WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) = 0 THEN (CASE WHEN VPM_Futai.FutGuiKbn != 5 THEN (SELECT VPM_Futai.FutGuiKbn) END ) -- input empty => select all
									   WHEN (SELECT COUNT(*) FROM @FutaiTypesTbl) > 0 THEN (SELECT TOP 1 * FROM @FutaiTypesTbl fc WHERE VPM_Futai.FutGuiKbn = fc.FutaiType) 
									END)       --2:�t�� 3:�ʍs�� 4:��z�� 5:�K�C�h�� �R�[�h�敪�}�X�^�i�V�X�e���FFUTGUIKBN�j																														
	  --��ʂ̕t�ї����R�[�h�ɑI�������l�iFROM/TO�j																																
      AND VPM_Futai.FutaiCd BETWEEN @futTumStart AND @futTumEnd																															
	  ------------------------------------------------																																
	  AND TKD_FutTum.SiyoKbn = 1																														
	  --�R�^�@��ʂ̓��t�w��Ɂy�����N�����z�̏ꍇ																																
        --AND HaiSYmd BETWEEN 20201106 AND 20201110      --��ʂ̑Ώۓ��t�iFROM/TO�j	
	  OR (@dateType = 3 AND HaiSYmd BETWEEN @startDate AND @endDate)
	  ) > 0		 																												
  --��ʂ̓��Ӑ悪�w�肷��ꍇ	
  AND
  ( --Compare customer				��ʂ�No.8,9�Ŏw�肵�����Ӑ�
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
  --��ʂ̉�ЃR�[�h�ɑI�������l																																
  AND (@companyId = 0 OR KAISHA.CompanyCd = @companyId)																																
  --��ʂ̉c�Ə��ɑI�������l�iFROM/TO�j																																
  AND EIGYOS.EigyoCd BETWEEN @brandStart AND @brandEnd																																																															
  --��ʂ̗\��敪�ɑI�������l																																
  AND YoyKbn.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))--��ʂ̎�t�ԍ��ɓ��͂����l�iFROM�^TO�j�i��t�R�[�h�j																																
ORDER BY YYKSHO.UkeNo ASC																																
        ,UNKOBI.HaiSYmd ASC																										
END
GO


