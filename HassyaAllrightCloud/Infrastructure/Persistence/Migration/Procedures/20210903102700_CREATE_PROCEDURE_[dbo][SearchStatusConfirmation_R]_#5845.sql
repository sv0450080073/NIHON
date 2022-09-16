USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Test_Search_StatusConfirmation]    Script Date: 2021/03/08 8:21:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[Pro_SearchStatusConfirmation_R]
		@tenantId int,
		@startDate nvarchar(10),
		@endDate nvarchar(10),
		@company int,
		@branchStart int,
		@branchEnd int,
		@customerStart int,
		@customerEnd int,
		@customerSirStart int,
		@customerSirEnd int,
		@isFixed bit, -- [0 => KaktYmd = '', 1 => KaktYmd != '']
		@isConfirm bit, -- [0, 1]
		@numberOfConfirm int, -- [0 => CountKak > 0, 1..9 => CountKak = 1..9, 10 => CountKak >= 10]
		@saikFlg bit, -- [null => true, 0 => SaikFlg = 0 OR SaikFlg = null, 1 => SaikFlg = 1]
		@daiSuFlg bit, -- [null => true, 0 => DaiSuFlg = 0 OR DaiSuFlg = null, 1 => DaiSuFlg = 1]
		@kingFlg bit, -- [null => true, 0 => KingFlg = 0 OR KingFlg = null, 1 => KingFlg = 1]
		@nitteiFlg bit, -- [null => true, 0 => NitteiFlg = 0 OR NitteiFlg = null, 1 => NitteiFlg = 1]
		@skip int,
		@take tinyint
AS
BEGIN
	SELECT Count(0) as 'Count'
			, IsNull(SUM(yyksho.UntKin), 0) as SumBusFee
			, IsNull(SUM(yyksho.ZeiRui), 0) as SumBusTax
			, IsNull(SUM(yyksho.TesuRyoG), 0) as SumBusCharge
			, IsNull(SUM(yyksho.GuitKin), 0) as SumGuideFee
			, IsNull(SUM(yyksho.TaxGuider), 0) as SumGuideTax
			, IsNull(SUM(yyksho.FeeGuider), 0) as SumGuideCharge
	FROM TKD_YykSyu yyksyu
	JOIN TKD_Yyksho yyksho on yyksho.UkeNo = yyksyu.UkeNo
	INNER JOIN VPM_Tokisk AS Toku ON yyksho.TokuiSeq=Toku.TokuiSeq
	INNER JOIN VPM_TokiSt AS Toshi ON yyksho.SitenCdSeq=Toshi.SitenCdSeq And yyksho.TokuiSeq = Toshi.TokuiSeq
	INNER JOIN VPM_Tokisk AS Toki ON yyksho.SirCdSeq=Toki.TokuiSeq
	INNER JOIN VPM_TokiSt AS Toshiten ON yyksho.SirSitenCdSeq=Toshiten.SitenCdSeq and yyksho.SirCdSeq = Toshiten.TokuiSeq
	JOIN TKD_Unkobi unkobi on yyksho.UkeNo = unkobi.UkeNo
	LEFT JOIN
			(SELECT kak.UkeNo, kk.KaknYmd, kk.KaknNin, kk.KaknAit, kak.SaikFlg, kak.DaiSuFlg, kak.KingFlg, kak.NitteiFlg, IsNull(kak.CountKak, 0) as CountKak
			FROM (SELECT Count(k.KaknRen) as CountKak, Max(k.KaknRen) as KaknRen, Max(k.KaknAit) as KaknAit, Max(k.SaikFlg) as SaikFlg, Max(k.DaiSuFlg) as DaiSuFlg, Max(k.KingFlg) as KingFlg, Max(k.NitteiFlg) as NitteiFlg, k.UkeNo
					FROM TKD_Kaknin k group by k.UkeNo) kak
	inner JOIN TKD_Kaknin kk on kak.UkeNo = kk.UkeNo AND kak.KaknRen = kk.KaknRen) kaknin on kaknin.UkeNo = yyksho.UkeNo
	LEFT JOIN VPM_SyaSyu syasyu on yyksyu.SyaSyuCdSeq = syasyu.SyaSyuCdSeq
	LEFT JOIN VPM_Syain eigtan on yyksho.EigTanCdSeq = eigtan.SyainCdSeq
	LEFT JOIN VPM_Syain intan on yyksho.InTanCdSeq = intan.SyainCdSeq
	LEFT JOIN VPM_Eigyos eigyos on yyksho.UkeEigCdSeq = eigyos.EigyoCdSeq
	LEFT JOIN VPM_Compny company on eigyos.CompanyCdSeq = company.CompanyCdSeq AND company.TenantCdSeq = @tenantId
	LEFT JOIN VPM_YoyKbn yoykbn on yyksho.YoyaKbnSeq = yoykbn.YoyaKbnSeq
	LEFT JOIN VPM_Tokisk tokisk on yyksho.TokuiSeq = tokisk.TokuiSeq
	LEFT JOIN VPM_Tokist tokist on yyksho.TokuiSeq = tokist.TokuiSeq AND yyksho.SitenCdSeq= tokist.SitenCdSeq
	LEFT JOIN VPM_Tokisk sircdseq on yyksho.SirCdSeq = sircdseq.TokuiSeq
	LEFT JOIN VPM_Tokist sirkist on yyksho.SirCdSeq = sirkist.TokuiSeq AND yyksho.SirSitenCdSeq= sirkist.SitenCdSeq
	LEFT JOIN (SELECT RyakuNm, CodeKbn FROM VPM_CodeKb WHERE CodeSyu='KATAKBN' AND TenantCdSeq=0 ) AS KataKbn on KataKbn.CodeKbn = yykSyu.KataKbn
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countKotei FROM TKD_Kotei group by UkeNo) kotei on kotei.UkeNo = yyksho.UkeNo
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countTehai FROM TKD_Tehai group by UkeNo) tehai on tehai.UkeNo = yyksho.UkeNo
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countFutai FROM TKD_FutTum WHERE FutTumKbn = 1 group by UkeNo) futai on futai.UkeNo = yyksho.UkeNo
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countTsumi FROM TKD_FutTum WHERE FutTumKbn = 2 group by UkeNo) tsumi on tsumi.UkeNo = yyksho.UkeNo
	WHERE Cast(unkobi.HaiSYmd as date) >= Cast(@startDate as date) AND Cast(unkobi.TouYmd as date) <= Cast(@endDate as date)
	AND yyksho.TenantCdSeq = @tenantId
	AND ((@company != 0 AND company.CompanyCdSeq = @company) OR @company = 0)
	AND ((@branchStart != 0 AND eigyos.EigyoCd >= @branchStart) OR @branchStart = 0)
	AND ((@branchEnd != 0 AND eigyos.EigyoCd <= @branchEnd) OR @branchEnd = 0)

	--AND yyksho.TokuiSeq >= 0 AND yyksho.TokuiSeq <= 100
	--AND yyksho.SitenCdSeq >= 0 AND yyksho.SitenCdSeq <= 100
	AND ((@customerStart = 0 and @customerEnd = 0)
			or
			(@customerStart <> @customerEnd and Toku.TokuiCd = @customerStart and Toshi.SitenCd >= @customerSirStart)
			or
			(@customerStart <> @customerEnd and Toku.TokuiCd = @customerEnd and Toshi.SitenCd <= @customerSirEnd)
			or
			(@customerStart = @customerEnd and Toku.TokuiCd = @customerStart and Toshi.SitenCd >= @customerSirStart and Toshi.SitenCd <= @customerSirEnd)
			or
			(@customerStart = 0 and @customerEnd <> 0 and ((Toku.TokuiCd = @customerEnd and Toshi.SitenCd <= @customerSirEnd) or Toku.TokuiCd < @customerEnd))
			or
			(@customerEnd = 0 and @customerStart <> 0 and ((Toku.TokuiCd = @customerStart and Toshi.SitenCd >= @customerSirStart) or Toku.TokuiCd > @customerStart))
			or
			(Toku.TokuiCd < @customerEnd and Toku.TokuiCd > @customerStart))

	AND ((@isFixed = 0 AND yyksho.KaktYmd = '') OR (@isFixed = 1 AND yyksho.KaktYmd != ''))
	AND ((@isConfirm = 1 AND kaknin.CountKak > 0) OR (@isConfirm = 0 AND IsNULL(kaknin.CountKak, 0) = 0))
	AND ((@isConfirm = 1
			AND ((@numberOfConfirm = 10 AND kaknin.CountKak >= 10) OR (kaknin.CountKak = @numberOfConfirm) OR @numberOfConfirm = 0))
		OR @isConfirm = 0)
	AND ((@saikFlg is not null AND IsNULL(kaknin.SaikFlg, 0) = @saikFlg)
		OR @saikFlg is null)
	AND ((@daiSuFlg is not null AND IsNULL(kaknin.DaiSuFlg, 0) = @daiSuFlg)
		OR @daiSuFlg is null)
	AND ((@kingFlg is not null AND IsNULL(kaknin.KingFlg, 0) = @kingFlg)
		OR @kingFlg is null)
	AND ((@nitteiFlg is not null AND IsNULL(kaknin.NitteiFlg, 0) = @nitteiFlg)
		OR @nitteiFlg is null)
	AND yyksho.YoyaSyu = 1 AND yyksho.SiyoKbn = 1 AND unkobi.SiyoKbn = 1 AND yyksyu.SiyoKbn = 1

	SELECT  kaknin.CountKak, yyksho.KaktYmd,
			ROW_NUMBER() OVER(ORDER BY yyksyu.UkeNo) AS 'No',
			IsNull(yyksyu.UkeNo, '') as UkeNo,
			IsNull(unkobi.UnkRen, '') as UnkRen,
			IsNull(syasyu.SyaSyuNm, '指定なし') as BusName,
			IsNull(KataKbn.RyakuNm, '') as Bustype,
			IsNull(yyksyu.SyaSyuDai, '') as Daisu,
			IsNull(yyksho.TokuiTanNm, '') as TokuiStaff,
			IsNull(sircdseq.RyakuNm, '') as ShiireSaki,
			IsNull(yyksho.KaknKais, '') as ConfirmedTime,
			IsNull(unkobi.DanTaNm, '') as DanTaiName,
			IsNull(unkobi.IkNm, '') as DestinationName,
			IsNull(unkobi.KanJNm, '') as KanjiName,
			IsNull(tokisk.TokuiNm, '') as TokuiSaki,
			IsNull(kaknin.KaknYmd, '') as ConfirmedYmd,
			IsNull(yyksho.KaktYmd, '') as FixedYmd,
			IsNull(kaknin.KaknNin, '') as ConfirmedPerson,
			IsNull(kaknin.KaknAit, '') as ConfirmedBy,
			IsNull(unkobi.BikoNm, '') as NoteContent,
			IsNull(kaknin.SaikFlg, 0) as Saikou,
			IsNull(kaknin.DaiSuFlg, 0) as SumDai,
			IsNull(kaknin.KingFlg, 0) as SumAmount,
			IsNull(kaknin.NitteiFlg, 0) as ScheduledDate,
			IsNull(unkobi.HaiSYmd, '') as HaishaYmd,
			IsNull(unkobi.HaiSTime, '') as HaishaTime,
			IsNull(unkobi.HaiSNm, '') as HaiSNm,
			IsNull(unkobi.TouYmd, '') as TouYmd,
			IsNull(unkobi.TouChTime, '') as TouTime,
			IsNull(unkobi.TouNm, '') as TouNm,
			IsNull(unkobi.JyoSyaJin, '') as PassengerQuantity,
			IsNull(unkobi.PlusJin, '') as PlusPassenger,
			CONVERT(varchar, yyksho.UntKin) as BusFee,
			CONVERT(varchar, yyksho.ZeiRui) as BusTaxAmount,
			CONVERT(varchar, yyksho.Zeiritsu) as BusTaxRate,
			CONVERT(varchar, yyksho.TesuRyoG) as BusCharge,
			CONVERT(varchar, yyksho.TesuRitu) as BusChargeRate,
			CONVERT(varchar, yyksho.GuitKin) as GuideFee,
			CONVERT(varchar, yyksho.TaxGuider) as GuideTax,
			CONVERT(varchar, yyksho.FeeGuider) as GuideCharge,
			IsNull(eigyos.RyakuNm, '') as ReceivedBranch,
			IsNull(eigtan.SyainNm, '') as ReceivedBy,
			IsNull(intan.SyainNm, '') as InputBy,
			IsNull(yoykbn.YoyaKbnNm, '') as BookingType,
			IsNull(yyksho.UkeYmd, '') as ReceivedYmd,
			IsNull(yyksho.UkeCd, '') as BookingNo,
			IsNull(yyksho.GuiWNin, '') as Guide,
			IsNull(kotei.countKotei, 0) as Kotei,
			IsNull(tsumi.countTsumi, 0) as TsuMi,
			IsNull(tehai.countTehai, 0) as Tehai,
			IsNull(futai.countFutai, 0) as Futai


	FROM TKD_YykSyu yyksyu
	JOIN TKD_Yyksho yyksho on yyksho.UkeNo = yyksyu.UkeNo
	INNER JOIN VPM_Tokisk AS Toku ON yyksho.TokuiSeq=Toku.TokuiSeq
	INNER JOIN VPM_TokiSt AS Toshi ON yyksho.SitenCdSeq=Toshi.SitenCdSeq And yyksho.TokuiSeq = Toshi.TokuiSeq
	INNER JOIN VPM_Tokisk AS Toki ON yyksho.SirCdSeq=Toki.TokuiSeq
	INNER JOIN VPM_TokiSt AS Toshiten ON yyksho.SirSitenCdSeq=Toshiten.SitenCdSeq and yyksho.SirCdSeq = Toshiten.TokuiSeq
	JOIN TKD_Unkobi unkobi on yyksho.UkeNo = unkobi.UkeNo
	LEFT JOIN
			(SELECT kak.UkeNo, kk.KaknYmd, kk.KaknNin, kk.KaknAit, kak.SaikFlg, kak.DaiSuFlg, kak.KingFlg, kak.NitteiFlg, IsNull(kak.CountKak, 0) as CountKak
			FROM (SELECT Count(k.KaknRen) as CountKak, Max(k.KaknRen) as KaknRen, Max(k.KaknAit) as KaknAit, Max(k.SaikFlg) as SaikFlg, Max(k.DaiSuFlg) as DaiSuFlg, Max(k.KingFlg) as KingFlg, Max(k.NitteiFlg) as NitteiFlg, k.UkeNo
					FROM TKD_Kaknin k group by k.UkeNo) kak
	inner JOIN TKD_Kaknin kk on kak.UkeNo = kk.UkeNo AND kak.KaknRen = kk.KaknRen) kaknin on kaknin.UkeNo = yyksho.UkeNo
	LEFT JOIN VPM_SyaSyu syasyu on yyksyu.SyaSyuCdSeq = syasyu.SyaSyuCdSeq
	LEFT JOIN VPM_Syain eigtan on yyksho.EigTanCdSeq = eigtan.SyainCdSeq
	LEFT JOIN VPM_Syain intan on yyksho.InTanCdSeq = intan.SyainCdSeq
	LEFT JOIN VPM_Eigyos eigyos on yyksho.UkeEigCdSeq = eigyos.EigyoCdSeq
	LEFT JOIN VPM_Compny company on eigyos.CompanyCdSeq = company.CompanyCdSeq AND company.TenantCdSeq = @tenantId
	LEFT JOIN VPM_YoyKbn yoykbn on yyksho.YoyaKbnSeq = yoykbn.YoyaKbnSeq
	LEFT JOIN VPM_Tokisk tokisk on yyksho.TokuiSeq = tokisk.TokuiSeq
	LEFT JOIN VPM_Tokist tokist on yyksho.TokuiSeq = tokist.TokuiSeq AND yyksho.SitenCdSeq= tokist.SitenCdSeq
	LEFT JOIN VPM_Tokisk sircdseq on yyksho.SirCdSeq = sircdseq.TokuiSeq
	LEFT JOIN VPM_Tokist sirkist on yyksho.SirCdSeq = sirkist.TokuiSeq AND yyksho.SirSitenCdSeq= sirkist.SitenCdSeq
	LEFT JOIN (SELECT RyakuNm, CodeKbn FROM VPM_CodeKb WHERE CodeSyu='KATAKBN' AND TenantCdSeq=0 ) AS KataKbn on KataKbn.CodeKbn = yykSyu.KataKbn
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countKotei FROM TKD_Kotei group by UkeNo) kotei on kotei.UkeNo = yyksho.UkeNo
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countTehai FROM TKD_Tehai group by UkeNo) tehai on tehai.UkeNo = yyksho.UkeNo
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countFutai FROM TKD_FutTum WHERE FutTumKbn = 1 group by UkeNo) futai on futai.UkeNo = yyksho.UkeNo
	LEFT JOIN (SELECT UkeNo, Count(UkeNo) as countTsumi FROM TKD_FutTum WHERE FutTumKbn = 2 group by UkeNo) tsumi on tsumi.UkeNo = yyksho.UkeNo
	WHERE Cast(unkobi.HaiSYmd as date) >= Cast(@startDate as date) AND Cast(unkobi.TouYmd as date) <= Cast(@endDate as date)
	AND yyksho.TenantCdSeq = @tenantId
	AND ((@company != 0 AND company.CompanyCdSeq = @company) OR @company = 0)
	AND ((@branchStart != 0 AND eigyos.EigyoCd >= @branchStart) OR @branchStart = 0)
	AND ((@branchEnd != 0 AND eigyos.EigyoCd <= @branchEnd) OR @branchEnd = 0)
	
	--AND yyksho.TokuiSeq >= 0 AND yyksho.TokuiSeq <= 100
	--AND yyksho.SitenCdSeq >= 0 AND yyksho.SitenCdSeq <= 100
	AND ((@customerStart = 0 and @customerEnd = 0)
			or
			(@customerStart <> @customerEnd and Toku.TokuiCd = @customerStart and Toshi.SitenCd >= @customerSirStart)
			or
			(@customerStart <> @customerEnd and Toku.TokuiCd = @customerEnd and Toshi.SitenCd <= @customerSirEnd)
			or
			(@customerStart = @customerEnd and Toku.TokuiCd = @customerStart and Toshi.SitenCd >= @customerSirStart and Toshi.SitenCd <= @customerSirEnd)
			or
			(@customerStart = 0 and @customerEnd <> 0 and ((Toku.TokuiCd = @customerEnd and Toshi.SitenCd <= @customerSirEnd) or Toku.TokuiCd < @customerEnd))
			or
			(@customerEnd = 0 and @customerStart <> 0 and ((Toku.TokuiCd = @customerStart and Toshi.SitenCd >= @customerSirStart) or Toku.TokuiCd > @customerStart))
			or
			(Toku.TokuiCd < @customerEnd and Toku.TokuiCd > @customerStart))

	AND ((@isFixed = 0 AND yyksho.KaktYmd = '') OR (@isFixed = 1 AND yyksho.KaktYmd != ''))
	AND ((@isConfirm = 1 AND kaknin.CountKak > 0) OR (@isConfirm = 0 AND IsNULL(kaknin.CountKak, 0) = 0))
	AND ((@isConfirm = 1
			AND ((@numberOfConfirm = 10 AND kaknin.CountKak >= 10) OR (kaknin.CountKak = @numberOfConfirm) OR @numberOfConfirm = 0))
		OR @isConfirm = 0)
	AND ((@saikFlg is not null AND IsNULL(kaknin.SaikFlg, 0) = @saikFlg)
		OR @saikFlg is null)
	AND ((@daiSuFlg is not null AND IsNULL(kaknin.DaiSuFlg, 0) = @daiSuFlg)
		OR @daiSuFlg is null)
	AND ((@kingFlg is not null AND IsNULL(kaknin.KingFlg, 0) = @kingFlg)
		OR @kingFlg is null)
	AND ((@nitteiFlg is not null AND IsNULL(kaknin.NitteiFlg, 0) = @nitteiFlg)
		OR @nitteiFlg is null)
	AND yyksho.YoyaSyu = 1 AND yyksho.SiyoKbn = 1 AND unkobi.SiyoKbn = 1 AND yyksyu.SiyoKbn = 1
	order by UkeNo
	OFFSET @skip ROWS
	FETCH NEXT @take ROWS ONLY;
End
GO


