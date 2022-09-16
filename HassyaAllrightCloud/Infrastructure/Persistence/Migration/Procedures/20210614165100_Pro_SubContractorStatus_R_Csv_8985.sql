USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_SubContractorStatus_R_Csv]    Script Date: 17/06/2021 12:38:56 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER    PROC [dbo].[Pro_SubContractorStatus_R_Csv]
					@startDate varchar(8), @endDate varchar(8),
					@dateType int,
					@tokuiFrom int, @tokuiTo int, @sitenFrom int, @sitenTo int, -- for customer
					@bookingTypes varchar(max),
					@companyIds varchar(max),
					@brandStart int, @brandEnd int,
					@ukeCdFrom varchar(10), @ukeCdTo varchar(10),
					@staffFrom int, @staffTo int,
					@jitaFlg int,
					@tenantId int
AS
BEGIN

SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS 'No'
	   ,JM_YoyKbn.YoyaKbn 　  AS		'YoyaKbn'	--'予約区分'											
       ,JM_YoyKbn.YoyaKbnNm  AS		'YoyaKbnNm'	--'予約区分名'											
       ,TKD_Yousha.UkeNo     AS		'UkeNo'	--'受付番号'											
       ,TKD_Yousha.UnkRen    AS		'UnkRen'	--'運行日連番'											
       ,JT_Unkobi.HaiSYmd    AS		'HaiSYmd'	--'運行配車年月日' 											
       ,JT_Unkobi.TouYmd     AS		'TouYmd'	--'運行到着年月日' 											
       ,JM_Gyosya.GyosyaCd   AS		'GyosyaCd'	--'業者コード'											
       ,JM_Tokisk.TokuiCd    AS		'TokuiCd'	--'得意先コード' 											
       ,JM_TokiSt.SitenCd    AS		'SitenCd'	--'得意先支店コード'											
       ,JM_Tokisk.TokuiNm    AS		'SkTokuiNm'	--'得意先名'											
       ,JM_TokiSt.SitenNm    AS		'StSitenNm'	--'得意先支店名' 											
       ,JM_Tokisk.RyakuNm    AS		'SkRyakuNm'	--'得意先略名' 											
       ,JM_TokiSt.RyakuNm    AS		'StRyakuNm'	--'得意先支店略名' 											
       ,JT_Yyksho.TokuiTanNm AS		'TokuiTanNm'	--'得意先担当者名'											
       ,JT_Yyksho.TokuiTel   AS		'TokuiTel'	--'得意先電話番号'											
       ,JM_UkeEigyos.EigyoCd AS		'EigyoCd'	--'受付営業所コード'											
       ,JM_UkeEigyos.EigyoNm AS		'EigyoNm'	--'受付営業所名'											
       ,JM_UkeEigyos.RyakuNm AS		'EigyosRyakuNm'	--'受付営業所略名'											
       ,JT_Unkobi.DanTaNm	 AS		'DanTaNm'	--'団体名'											
       ,JT_Unkobi.IkNm		 AS		'IkNm'	--'行先名'											
       ,JT_Unkobi.HaiSNm	 AS		'U_HaiSNm'	--'配車地名(運行日)'											
       ,JT_Unkobi.HaiSTime   AS		'U_HaiSTime'	--'配車時間(運行日)'											
       ,JT_Unkobi.HaiSJyus1  AS		'U_HaiSJyus1'	--'配車地住所1(運行日)'											
       ,JT_Unkobi.HaiSJyus2  AS		'U_HaiSJyus2'	--'配車地住所2(運行日)'											
       ,JM_HaiSBunrui.CodeKbn AS	'BunruiCodeKbn'	--'配車地交通機関分類コード(運行日)'											
       ,JM_HaiSKoutu.KoukCd   AS	'HaiSKoukCd'	--'配車地交通機関コード(運行日)'											
       ,JM_HaiSBunrui.CodeKbnNm AS	'JM_HaiSBunrui_CodeKbnNm'	--'配車地交通機関分類名(運行日)'											
       ,JM_HaiSBunrui.RyakuNm AS	'JM_HaiSBunrui_RyakuNm'	--'配車地交通機関分類略名(運行日)'											
       ,JM_HaiSKoutu.KoukNm   AS	'JM_HaiSKoutu_KoukNm'	--'配車地交通機関名(運行日)'											
       ,JM_HaiSKoutu.RyakuNm  AS	'JM_HaiSKoutu_RyakuNm'	--'配車地交通機関略名(運行日)'											
       ,JM_HaiSBin.BinCd		AS	'JM_HaiSBin_BinCd'	--'配車地便コード(運行日)'											
       ,JM_HaiSBin.BinNm		AS	'JM_HaiSBin_BinNm'	--'配車地便名(運行日)'											
       ,JT_Unkobi.TouNm			AS	'JT_Unkobi_TouNm'	--'到着地名(運行日)'											
       ,JT_Unkobi.TouChTime		AS	'JT_Unkobi_TouChTime'	--'到着時間(運行日)'											
       ,JT_Unkobi.TouJyusyo1	AS	'JT_Unkobi_TouJyusyo1'	--'到着地住所1(運行日)'											
       ,JT_Unkobi.TouJyusyo2	AS	'JT_Unkobi_TouJyusyo2'	--'到着地住所2(運行日)'											
       ,JM_TouChaBunrui.CodeKbn AS	'JM_TouChaBunrui_CodeKbn'	--'到着地交通機関分類コード(運行日)'											
       ,JM_TouChaKoutu.KoukCd   AS	'JM_TouChaKoutu_KoukCd'	--'到着地交通機関コード(運行日)'											
       ,JM_TouChaBunrui.CodeKbnNm	AS 'JM_TouChaBunrui_CodeKbnNm'	--'到着地交通機関分類名(運行日)'											
       ,JM_TouChaBunrui.RyakuNm		AS 'JM_TouChaBunrui_RyakuNm'	--'到着地交通機関分類略名(運行日)'											
       ,JM_TouChaKoutu.KoukNm		AS	'JM_TouChaKoutu_KoukNm'	--'到着地交通機関名(運行日)' 											
       ,JM_TouChaKoutu.RyakuNm		AS	'JM_TouChaKoutu_RyakuNm'	--'到着地交通機関略名(運行日)' 											
       ,JM_TouChaBin.BinCd			AS	'JM_TouChaBin_BinCd'	--'到着地便コード(運行日)' 											
       ,JM_TouChaBin.BinNm			AS	'JM_TouChaBin_BinNm'	--'到着地便名(運行日)' 											
       ,JT_Unkobi.JyoSyaJin			AS	'JT_Unkobi_JyoSyaJin'	--'乗車人員(運行日)' 											
       ,JT_Unkobi.PlusJin			AS	'JT_Unkobi_PlusJin'	--'プラス人員(運行日)' 											
       ,JT_SumYykSyu.SumDai			AS	'JT_SumYykSyu_SumDai'	--'総台数' 											
       ,JT_SumHaisha.SumSyaRyoUnc	AS	'JT_SumHaisha_SumSyaRyoUnc'	--'運賃'											
       ,JT_Yyksho.ZeiKbn			AS	'JT_Yyksho_ZeiKbn'	--'税区分'											
       ,JM_ZeiKbn.CodeKbnNm			AS	'JM_ZeiKbn_CodeKbnNm'	--'税区分名'											
       ,JM_ZeiKbn.RyakuNm			AS	'JM_ZeiKbn_RyakuNm'	--'税区分名'											
       ,JT_Yyksho.Zeiritsu			AS	'JT_Yyksho_Zeiritsu'	--'消費税率' 											
       ,JT_SumHaisha.SumSyaRyoSyo	AS	'JT_SumHaisha_SumSyaRyoSyo'	--'消費税額' 											
       ,JT_Yyksho.TesuRitu			AS	'JT_Yyksho_TesuRitu'	--'手数料率'											
       ,JT_SumHaisha.SumSyaRyoTes	AS	'JT_SumHaisha_SumSyaRyoTes'	--'手数料額' 											
	   ,JT_SumHaisha.SumSyaRyoUnc + JT_SumHaisha.SumSyaRyoSyo AS 'JT_SumHaisha_Charge'	--'運賃合計'										
       ,JT_SumFutTumGui.SumUriGakKin AS 'JT_SumFutTumGui_SumUriGakKin'	--'ガイド料'											
       ,JT_SumFutTumGui.SumSyaRyoSyo AS 'JT_SumFutTumGui_SumSyaRyoSyo'	--'ガイド料消費税'											
       ,JT_SumFutTumGui.SumSyaRyoTes AS 'JT_SumFutTumGui_SumSyaRyoTes'	--'ガイド料手数料'  											
	   ,JT_SumFutTumGui.SumUriGakKin + JT_SumFutTumGui.SumSyaRyoSyo AS 'JT_SumFutTumGui_Charge'	--'ガイド料合計料'										
       ,JT_SumFutTum.SumUriGakKin -(JT_SumFutTumGui.SumUriGakKin)	AS 'JT_SumFutTum_SumUriGakKin'	--'付帯積込品額' 											
       ,JT_SumFutTum.SumSyaRyoSyo -(JT_SumFutTumGui.SumSyaRyoSyo)	AS 'JT_SumFutTum_SumSyaRyoSyo'	--'付帯積込品消費税'											
       ,JT_SumFutTum.SumSyaRyoTes -(JT_SumFutTumGui.SumSyaRyoTes)	AS 'JT_SumFutTum_SumSyaRyoTes'	--'付帯積込品手数料額'
	   ,JT_SumFutTum.SumUriGakKin -(JT_SumFutTumGui.SumUriGakKin) + JT_SumFutTum.SumSyaRyoSyo -(JT_SumFutTumGui.SumSyaRyoSyo) AS 'Total_YFutum'
       ,JM_YouGyosya.GyosyaKbn		AS 'GyosyaKbn'	--'傭車先業者区分'											
       ,JM_You.TokuiCd				AS 'You_TokuiCd'	--'傭車先コード'											
       ,JM_YouSiten.SitenCd			AS 'YouSiten_SitenCd'	--'傭車先支店コード'											
       ,JM_You.TokuiNm				AS 'You_TokuiNm'	--'傭車先名'											
       ,JM_YouSiten.SitenNm			AS 'YouSiten_SitenNm'	--'傭車先支店名'											
       ,JM_You.RyakuNm				AS 'You_RyakuNm'	--'傭車先略名'											
       ,JM_YouSiten.RyakuNm			AS 'YouSiten_RyakuNm'	--'傭車先支店略名'											
       ,JT_Haisha.GoSya				AS 'H_GoSya'	--'号車'											
       ,JT_Haisha.DanTaNm2			AS 'H_DanTaNm2'	--'団体名２' 											
       ,JT_Haisha.IkNm				AS 'H_IkNm'	--'行先名(配車)'											
       ,JT_Haisha.HaiSNm			AS 'H_HaiSNm'	--'配車地名(配車)'											
       ,JT_Haisha.HaiSYmd			AS 'H_HaiSYmd'	--'配車年月日(配車)'											
       ,JT_Haisha.HaiSTime			AS 'H_HaiSTime'	--'配車時間(配車)'											
       ,JT_Haisha.HaiSJyus1			AS 'H_HaiSJyus1'	--'配車地住所1(配車)'											
       ,JT_Haisha.HaiSJyus2			AS 'H_HaiSJyus2'	--'配車地住所1(配車)'											
       ,JM_YouHaiSBunrui.CodeKbn	AS 'JM_YouHaiSBunrui_CodeKb'	--'配車地交通機関分類コード(配車)'											
       ,JM_YouHaiSKoutu.KoukCd		AS 'JM_YouHaiSKoutu_KoukCd'	--'配車地交通機関コード(配車)'											
       ,JM_YouHaiSBunrui.CodeKbnNm	AS 'JM_YouHaiSBunrui_CodeKbnNm'	--'配車地交通機関分類名(配車)'											
       ,JM_YouHaiSBunrui.RyakuNm	AS 'JM_YouHaiSBunrui_RyakuNm'	--'配車地交通機関分類略名(配車)'											
       ,JM_YouHaiSKoutu.KoukNm		AS 'JM_YouHaiSKoutu_KoukNm'	--'配車地交通機関名(配車)'											
       ,JM_YouHaiSKoutu.RyakuNm		AS 'JM_YouHaiSKoutu_RyakuNm'	--'配車地交通機関略名(配車)'											
       ,JM_YouHaiSBin.BinCd			AS 'JM_YouHaiSBin_BinCd'	--'配車地便コード(配車)'											
       ,JM_YouHaiSBin.BinNm			AS 'JM_YouHaiSBin_BinNm'	--'配車地便名(配車)'											
       ,JT_Haisha.TouNm				AS 'JT_Haisha_TouNm'	--'到着地名(配車)'											
       ,JT_Haisha.TouYmd			AS 'JT_Haisha_TouYmd'	--'到着年月日(配車)'											
       ,JT_Haisha.TouChTime			AS 'JT_Haisha_TouChTime'	--'到着時間(配車)'											
       ,JT_Haisha.TouJyusyo1		AS 'JT_Haisha_TouJyusyo1'	--'到着地住所1(配車)'											
       ,JT_Haisha.TouJyusyo2		AS 'JT_Haisha_TouJyusyo2'	--'到着地住所2(配車)'											
       ,JM_YouTouChaBunrui.CodeKbn	AS 'JM_YouTouChaBunrui_CodeKbn'	--'到着地交通機関分類コード(配車)'											
       ,JM_YouTouChaKoutu.KoukCd	AS 'JM_YouTouChaKoutu_KoukCd'	--'到着地交通機関コード(配車)'											
       ,JM_YouTouChaBunrui.CodeKbnNm AS'JM_YouTouChaBunrui_CodeKbnNm'	-- '到着地交通機関分類名(配車)'											
       ,JM_YouTouChaBunrui.RyakuNm	AS 'JM_YouTouChaBunrui_RyakuNm'	--'到着地交通機関分類略名(配車)'											
       ,JM_YouTouChaKoutu.KoukNm	AS 'JM_YouTouChaKoutu_KoukNm'	--'到着地交通機関名(配車)'											
       ,JM_YouTouChaKoutu.RyakuNm	AS 'JM_YouTouChaKoutu_RyakuNm'	--'到着地交通機関略名(配車)'											
       ,JM_YouTouChaBin.BinCd		AS 'JM_YouTouChaBin_BinCd'	--'到着地便コード(配車)'											
       ,JM_YouTouChaBin.BinNm		AS 'JM_YouTouChaBin_BinNm'	--'到着地便名(配車)'											
       ,JT_Haisha.JyoSyaJin			AS 'JT_Haisha_JyoSyaJin'	--'乗車人員(配車)'											
       ,JT_Haisha.PlusJin			AS 'JT_Haisha_PlusJin'	--'プラス人員(配車)'											
       ,JT_Haisha.YoushaUnc			AS 'JT_Haisha_YoushaUnc'	--'傭車運賃' 											
       ,TKD_Yousha.ZeiKbn			AS 'TKD_Yousha_ZeiKbn'	--'傭車税区分'											
       ,JM_YouZeiKbn.CodeKbnNm		AS 'JM_YouZeiKbn_CodeKbnNm'	--'傭車税区分名'											
       ,JM_YouZeiKbn.RyakuNm		AS 'JM_YouZeiKbn_RyakuNm'	--'傭車税区分名' 											
       ,TKD_Yousha.Zeiritsu			AS 'TKD_Yousha_Zeiritsu'	--'傭車消費税率'											
       ,JT_Haisha.YoushaSyo			AS 'JT_Haisha_YoushaSyo'	--'傭車消費税額' 											
       ,TKD_Yousha.TesuRitu			AS 'TKD_Yousha_TesuRitu'	--'傭車手数料率' 											
       ,JT_Haisha.YoushaTes			AS 'JT_Haisha_YoushaTes'	--'傭車手数料額' 											
	   ,JT_Haisha.YoushaUnc + JT_Haisha.YoushaSyo AS 'JT_Haisha_YouCharge'	--'傭車運賃合計'										
       ,JT_SumYMFuTuGui.SumHaseiKin		AS '	'	--'傭車ガイド料'											
       ,JT_SumYMFuTuGui.SumSyaRyoSyo	AS 'JT_SumYMFuTuGui_SumSyaRyoSyo'	--'傭車ガイド料消費税'											
       ,JT_SumYMFuTuGui.SumSyaRyoTes	AS 'JT_SumYMFuTuGui_SumSyaRyoTes'	--'傭車ガイド料手数料' 											
	   ,JT_SumYMFuTuGui.SumHaseiKin + JT_SumYMFuTuGui.SumSyaRyoSyo	AS 'JT_SumYMFuTuGui_Charge'	--'傭車ガイド料合計'										
       ,JT_SumYMFuTu.SumHaseiKin -(JT_SumYMFuTuGui.SumHaseiKin)		AS 'JT_SumYMFuTu_SumHaseiKin'	--'傭車付帯積込品額'											
       ,JT_SumYMFuTu.SumSyaRyoSyo -(JT_SumYMFuTuGui.SumSyaRyoSyo)	AS 'JT_SumYMFuTu_SumSyaRyoSyo'	--'傭車付帯積込品消費税'											
       ,JT_SumYMFuTu.SumSyaRyoTes -(JT_SumYMFuTuGui.SumSyaRyoTes)	AS 'JT_SumYMFuTu_SumSyaRyoTes'	--'傭車付帯積込品手数料額'											
	   ,JT_SumYMFuTu.SumHaseiKin -(JT_SumYMFuTuGui.SumHaseiKin)+ JT_SumYMFuTu.SumSyaRyoSyo -(JT_SumYMFuTuGui.SumSyaRyoSyo) AS	'Total_YMFutum' --'傭車付帯積込品合計'										
FROM TKD_Yousha											
INNER JOIN											
  (SELECT TKD_Yousha.UkeNo ,											
          TKD_Yousha.UnkRen											
   FROM TKD_Yousha											
   LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Yousha.UkeNo = JT_Yyksho.UkeNo											
   AND JT_Yyksho.YoyaSyu = 1											
   LEFT JOIN TKD_Haisha AS JT_Haisha ON TKD_Yousha.UkeNo = JT_Haisha.UkeNo											
   AND TKD_Yousha.UnkRen = JT_Haisha.UnkRen											
   AND TKD_Yousha.YouTblSeq = JT_Haisha.YouTblSeq											
   AND JT_Haisha.SiyoKbn = 1											
   LEFT JOIN VPM_Tokisk AS JM_You ON TKD_Yousha.YouCdSeq = JM_You.TokuiSeq											
   AND TKD_Yousha.HasYmd >= JM_You.SiyoStaYmd											
   AND TKD_Yousha.HasYmd <= JM_You.SiyoEndYmd											
   LEFT JOIN VPM_TokiSt AS JM_YouSiten ON TKD_Yousha.YouCdSeq = JM_YouSiten.TokuiSeq											
   AND TKD_Yousha.YouSitCdSeq = JM_YouSiten.SitenCdSeq											
   AND TKD_Yousha.HasYmd >= JM_YouSiten.SiyoStaYmd											
   AND TKD_Yousha.HasYmd <= JM_YouSiten.SiyoEndYmd											
   LEFT JOIN VPM_Gyosya AS JM_YouGyosya ON JM_You.GyosyaCdSeq = JM_YouGyosya.GyosyaCdSeq											
   AND JM_YouGyosya.SiyoKbn = 1											
   LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq											
   AND JM_UkeEigyos.SiyoKbn = 1											
   LEFT JOIN VPM_Compny AS JM_Compny ON JM_UkeEigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq											
   AND JM_Compny.SiyoKbn = 1											
   LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JM_YoyKbn.TenantCdSeq = @tenantId
   AND JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq											
   AND JM_YoyKbn.SiyoKbn = 1											
   LEFT JOIN VPM_Syain AS JM_Syain ON JM_Syain.SyainCdSeq = JT_Yyksho.EigTanCdSeq											
   WHERE TKD_Yousha.SiyoKbn = 1											
     --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) >= '002'											
     --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) <= '002'	
	 AND ISNULL(JT_Yyksho.TenantCdSeq, 0) = @tenantId
     AND( (@dateType = 1 AND JT_Haisha.HaiSYmd >= @startDate AND JT_Haisha.HaiSYmd <= @endDate)		--画面 配車年月日:  画面で年月日項目でのFromの番号/ 画面で年月日項目でのToの番号
	 OR (@dateType = 2 AND JT_Haisha.TouYmd >= @startDate AND JT_Haisha.TouYmd <= @endDate))			--画面 到着年月日:  画面で年月日項目でのFromの番号/ 画面で年月日項目でのToの番号
      AND ((@tokuiFrom = 0 and @tokuiTo = 0)
			or
			(@tokuiFrom <> @tokuiTo and TKD_Yousha.YouCdSeq = @tokuiFrom and TKD_Yousha.YouSitCdSeq >= @sitenFrom)
			or
			(@tokuiFrom <> @tokuiTo and TKD_Yousha.YouCdSeq = @tokuiTo and TKD_Yousha.YouSitCdSeq <= @sitenTo)
			or
			(@tokuiFrom = @tokuiTo and TKD_Yousha.YouCdSeq = @tokuiFrom and TKD_Yousha.YouSitCdSeq >= @sitenFrom and TKD_Yousha.YouSitCdSeq <= @sitenTo)
			or
			(@tokuiFrom = 0 and @tokuiTo <> 0 and ((TKD_Yousha.YouCdSeq = @tokuiTo and TKD_Yousha.YouSitCdSeq <= @sitenTo) or TKD_Yousha.YouCdSeq < @tokuiTo))
			or
			(@tokuiTo = 0 and @tokuiFrom <> 0 and ((TKD_Yousha.YouCdSeq = @tokuiFrom and TKD_Yousha.YouSitCdSeq >= @sitenFrom) or TKD_Yousha.YouCdSeq > @tokuiFrom))
			or
			(TKD_Yousha.YouCdSeq < @tokuiTo and TKD_Yousha.YouCdSeq > @tokuiFrom))
     AND ISNULL(JT_Yyksho.UkeCd, 0) >= @ukeCdFrom											
     AND ISNULL(JT_Yyksho.UkeCd, 0) <= @ukeCdTo	
     AND JM_YoyKbn.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))	--画面で予約区分項目でのFromの番号										 
	 AND JM_Compny.CompanyCdSeq IN (SELECT * FROM FN_SplitString(@companyIds, '-'))	--画面でログイン会社のコードSEQ
     AND JM_UkeEigyos.EigyoCd >= @brandStart			--画面で受付営業所コード項目でのFromの番号											
	 AND JM_UkeEigyos.EigyoCd <= @brandEnd			--画面で受付営業所コード項目でのToの番号
     AND TKD_Yousha.JitaFlg = CASE WHEN @jitaFlg = 0 THEN TKD_Yousha.JitaFlg ELSE 0 END			--画面で自他社区分項目での未出力を選択した		
	 AND JM_Syain.SyainCdSeq >= @staffFrom		--画面で営業担当者コードSEQ項目でのFromの番号																										
	 AND JM_Syain.SyainCdSeq <= @staffTo			--画面で営業担当者コードSEQ項目でのToの番号	
   GROUP BY TKD_Yousha.UkeNo ,											
            TKD_Yousha.UnkRen) AS WHERETABLE ON TKD_Yousha.UkeNo = WHERETABLE.UkeNo											
AND TKD_Yousha.UnkRen = WHERETABLE.UnkRen											
LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Yousha.UkeNo = JT_Yyksho.UkeNo											
LEFT JOIN TKD_Unkobi AS JT_Unkobi ON TKD_Yousha.UkeNo = JT_Unkobi.UkeNo											
AND TKD_Yousha.UnkRen = JT_Unkobi.UnkRen											
INNER JOIN TKD_Haisha AS JT_Haisha ON TKD_Yousha.UkeNo = JT_Haisha.UkeNo											
AND TKD_Yousha.UnkRen = JT_Haisha.UnkRen											
AND TKD_Yousha.YouTblSeq = JT_Haisha.YouTblSeq											
AND JT_Haisha.SiyoKbn = 1											
LEFT JOIN VPM_Tokisk AS JM_Tokisk ON JT_Yyksho.TokuiSeq = JM_Tokisk.TokuiSeq											
AND JT_Yyksho.SeiTaiYmd >= JM_Tokisk.SiyoStaYmd											
AND JT_Yyksho.SeiTaiYmd <= JM_Tokisk.SiyoEndYmd											
LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq											
AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq											
AND JT_Yyksho.SeiTaiYmd >= JM_TokiSt.SiyoStaYmd											
AND JT_Yyksho.SeiTaiYmd <= JM_TokiSt.SiyoEndYmd											
LEFT JOIN VPM_Gyosya AS JM_Gyosya ON JM_Tokisk.GyosyaCdSeq = JM_Gyosya.GyosyaCdSeq											
AND JM_Gyosya.SiyoKbn = 1											
LEFT JOIN VPM_Koutu AS JM_HaiSKoutu ON JT_Unkobi.HaiSKouKCdSeq = JM_HaiSKoutu.KoukCdSeq											
AND JM_HaiSKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_HaiSBunrui ON JM_HaiSKoutu.BunruiCdSeq = JM_HaiSBunrui.CodeKbnSeq											
AND JM_HaiSBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_HaiSBin ON JT_Unkobi.HaiSBinCdSeq = JM_HaiSBin.BinCdSeq											
AND JT_Unkobi.HaiSYmd BETWEEN JM_HaiSBin.SiyoStaYmd AND JM_HaiSBin.SiyoEndYmd											
LEFT JOIN VPM_Koutu AS JM_TouChaKoutu ON JT_Unkobi.TouKouKCdSeq = JM_TouChaKoutu.KoukCdSeq											
AND JM_TouChaKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_TouChaBunrui ON JM_TouChaKoutu.BunruiCdSeq = JM_TouChaBunrui.CodeKbnSeq											
AND JM_TouChaBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_TouChaBin ON JT_Unkobi.TouBinCdSeq = JM_TouChaBin.BinCdSeq											
AND JT_Unkobi.HaiSYmd BETWEEN JM_TouChaBin.SiyoStaYmd AND JM_TouChaBin.SiyoEndYmd											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(SyaSyuDai) AS SumDai											
   FROM TKD_YykSyu											
   WHERE SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumYykSyu ON TKD_Yousha.UkeNo = JT_SumYykSyu.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumYykSyu.UnkRen											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(SyaRyoUnc) AS SumSyaRyoUnc ,											
          SUM(SyaRyoSyo) AS SumSyaRyoSyo ,											
          SUM(SyaRyoTes) AS SumSyaRyoTes											
   FROM TKD_Haisha											
   WHERE SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumHaisha ON TKD_Yousha.UkeNo = JT_SumHaisha.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumHaisha.UnkRen											
LEFT JOIN											
  (SELECT CodeKbn ,											
          CodeKbnNm ,											
          RyakuNm											
   FROM VPM_CodeKb											
   WHERE CodeSyu = 'ZEIKBN'											
     AND SiyoKbn = 1 ) AS JM_ZeiKbn ON JT_Yyksho.ZeiKbn = CONVERT(TINYINT,JM_ZeiKbn.CodeKbn)											
LEFT JOIN											
  (SELECT CodeKbn ,											
          CodeKbnNm ,											
          RyakuNm											
   FROM VPM_CodeKb											
   WHERE CodeSyu = 'ZEIKBN'											
     AND SiyoKbn = 1 ) AS JM_YouZeiKbn ON TKD_Yousha.ZeiKbn = CONVERT(TINYINT,JM_YouZeiKbn.CodeKbn)											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(UriGakKin) AS SumUriGakKin ,											
          SUM(SyaRyoSyo) AS SumSyaRyoSyo ,											
          SUM(SyaRyoTes) AS SumSyaRyoTes											
   FROM TKD_FutTum											
   WHERE TKD_FutTum.SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumFutTum ON TKD_Yousha.UkeNo = JT_SumFutTum.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumFutTum.UnkRen											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(UriGakKin) AS SumUriGakKin ,											
          SUM(SyaRyoSyo) AS SumSyaRyoSyo ,											
          SUM(SyaRyoTes) AS SumSyaRyoTes											
   FROM TKD_FutTum											
   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_FutTum.FutTumCdSeq											
   AND VPM_Futai.SiyoKbn=1											
   WHERE TKD_FutTum.SiyoKbn = 1											
     AND VPM_Futai.FutGuiKbn=5											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumFutTumGui ON TKD_Yousha.UkeNo = JT_SumFutTumGui.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumFutTumGui.UnkRen											
LEFT JOIN											
  (SELECT CodeKbn ,											
          CodeKbnNm ,											
          RyakuNm											
   FROM VPM_CodeKb											
   WHERE CodeSyu = 'TESKBN'											
     AND SiyoKbn = 1 ) AS JM_TesKbnFut ON JM_TokiSt.TesKbnFut = CONVERT(TINYINT,JM_TesKbnFut.CodeKbn)											
LEFT JOIN VPM_Tokisk AS JM_You ON TKD_Yousha.YouCdSeq = JM_You.TokuiSeq											
AND TKD_Yousha.HasYmd >= JM_You.SiyoStaYmd											
AND TKD_Yousha.HasYmd <= JM_You.SiyoEndYmd											
LEFT JOIN VPM_TokiSt AS JM_YouSiten ON TKD_Yousha.YouCdSeq = JM_YouSiten.TokuiSeq											
AND TKD_Yousha.YouSitCdSeq = JM_YouSiten.SitenCdSeq											
AND TKD_Yousha.HasYmd >= JM_YouSiten.SiyoStaYmd											
AND TKD_Yousha.HasYmd <= JM_YouSiten.SiyoEndYmd											
LEFT JOIN VPM_Gyosya AS JM_YouGyosya ON JM_You.GyosyaCdSeq = JM_YouGyosya.GyosyaCdSeq											
AND JM_YouGyosya.SiyoKbn = 1											
LEFT JOIN VPM_Koutu AS JM_YouHaiSKoutu ON JT_Haisha.HaiSKouKCdSeq = JM_YouHaiSKoutu.KoukCdSeq											
AND JM_YouHaiSKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_YouHaiSBunrui ON JM_YouHaiSKoutu.BunruiCdSeq = JM_YouHaiSBunrui.CodeKbnSeq											
AND JM_YouHaiSBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_YouHaiSBin ON JT_Haisha.HaiSBinCdSeq = JM_YouHaiSBin.BinCdSeq											
AND JT_Haisha.HaiSYmd BETWEEN JM_YouHaiSBin.SiyoStaYmd AND JM_YouHaiSBin.SiyoEndYmd											
LEFT JOIN VPM_Koutu AS JM_YouTouChaKoutu ON JT_Haisha.TouKouKCdSeq = JM_YouTouChaKoutu.KoukCdSeq											
AND JM_YouTouChaKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_YouTouChaBunrui ON JM_YouTouChaKoutu.BunruiCdSeq = JM_YouTouChaBunrui.CodeKbnSeq											
AND JM_YouTouChaBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_YouTouChaBin ON JT_Haisha.TouBinCdSeq = JM_YouTouChaBin.BinCdSeq											
AND JT_Haisha.HaiSYmd BETWEEN JM_YouTouChaBin.SiyoStaYmd AND JM_YouTouChaBin.SiyoEndYmd											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          YouTblSeq ,											
          SUM(HaseiKin) AS SumHaseiKin ,											
          SUM(SyaRyoSyo) AS SumSyaryoSyo ,											
          SUM(SyaRyoTes) AS SumSyaryoTes											
   FROM TKD_YFutTu											
   WHERE TKD_YFutTu.SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen ,											
            YouTblSeq) AS JT_SumYMFuTu ON JT_Haisha.UkeNo=JT_SumYMFuTu.UkeNo											
AND JT_Haisha.UnkRen=JT_SumYMFuTu.UnkRen											
AND JT_Haisha.YouTblSeq=JT_SumYMFuTu.YouTblSeq											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          YouTblSeq ,											
          SUM(HaseiKin) AS SumHaseiKin ,											
          SUM(SyaRyoSyo) AS SumSyaryoSyo ,											
          SUM(SyaRyoTes) AS SumSyaryoTes											
   FROM TKD_YFutTu											
   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_YFutTu.FutTumCdSeq											
   AND VPM_Futai.SiyoKbn=1											
   WHERE TKD_YFutTu.SiyoKbn = 1											
     AND VPM_Futai.FutGuiKbn=5											
   GROUP BY UkeNo ,											
            UnkRen ,											
            YouTblSeq) AS JT_SumYMFuTuGui ON JT_Haisha.UkeNo=JT_SumYMFuTuGui.UkeNo											
AND JT_Haisha.UnkRen=JT_SumYMFuTuGui.UnkRen											
AND JT_Haisha.YouTblSeq=JT_SumYMFuTuGui.YouTblSeq											
LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq											
AND JM_UkeEigyos.SiyoKbn = 1											
LEFT JOIN VPM_Compny AS JM_Compny ON JM_UkeEigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq											
AND JM_Compny.SiyoKbn = 1											
LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JM_YoyKbn.TenantCdSeq = @tenantId
AND JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq											
AND JM_YoyKbn.SiyoKbn = 1											
--WHERE TKD_Yousha.SiyoKbn = 1											
  --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) >= '002'											
  --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) <= '002'											

ORDER BY JT_Unkobi.HaiSYmd ,											
         TKD_Yousha.UkeNo ,											
         TKD_Yousha.UnkRen ,											
         JM_You.TokuiCd ,											
         TKD_Yousha.YouTblSeq											
END										
											
											
