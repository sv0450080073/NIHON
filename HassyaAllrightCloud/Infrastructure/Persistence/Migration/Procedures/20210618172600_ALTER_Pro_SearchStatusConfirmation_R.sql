USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_SearchStatusConfirmation_R]    Script Date: 2021/06/18 17:23:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   Pro_SearchStatusConfirmation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data vehicle total list
-- Date			:   
-- Author		:   
-- Description	:   Fix duplicate summary money
------------------------------------------------------------
-- Editor: DC Nguyen
-- Date: 2021/06/18
------------------------------------------------------------

ALTER     procedure [dbo].[Pro_SearchStatusConfirmation_R]
		@tenantId int,
		@startDate nvarchar(10),
		@endDate nvarchar(10),
		@company int,
		@branchStart int,
		@branchEnd int,
		@customerStart nvarchar(11),
		@customerEnd nvarchar(11),
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
	WITH SummaryResult_CTE(UkeNo, UnKren, UntKin, ZeiRui, TesuRyoG, GuitKin, TaxGuider, FeeGuider)
	AS 
	(
		SELECT DISTINCT yyksyu.UkeNo, yyksyu.UnKren
						,yyksho.UntKin
						,yyksho.ZeiRui
						,yyksho.TesuRyoG
						,yyksho.GuitKin
						,yyksho.TaxGuider
						,yyksho.FeeGuider
		FROM TKD_YykSyu yyksyu
		JOIN TKD_Yyksho yyksho on yyksho.UkeNo = yyksyu.UkeNo		
    	AND yyksho.TenantCdSeq = @tenantId		
    	AND yyksho.SiyoKbn = 1
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
		          AND syasyu.TenantCdSeq = yyksho.TenantCdSeq
		LEFT JOIN VPM_Syain eigtan on yyksho.EigTanCdSeq = eigtan.SyainCdSeq
		LEFT JOIN VPM_Syain intan on yyksho.InTanCdSeq = intan.SyainCdSeq
		LEFT JOIN VPM_Eigyos eigyos on yyksho.UkeEigCdSeq = eigyos.EigyoCdSeq
		LEFT JOIN VPM_Compny company on eigyos.CompanyCdSeq = company.CompanyCdSeq AND company.TenantCdSeq = @tenantId
		LEFT JOIN VPM_YoyKbn yoykbn on yyksho.YoyaKbnSeq = yoykbn.YoyaKbnSeq
		                           AND yyksho.TenantCdSeq = yoykbn.TenantCdSeq
		LEFT JOIN VPM_Tokisk tokisk on yyksho.TokuiSeq = tokisk.TokuiSeq
		    AND yyksho.SeiTaiYmd BETWEEN tokisk.SiyoStaYmd
    	AND tokisk.SiyoEndYmd
    	AND yyksho.TenantCdSeq = tokisk.TenantCdSeq
    	LEFT JOIN VPM_Gyosya gyosya ON tokisk.GyosyaCdSeq = gyosya.GyosyaCdSeq
    	AND tokisk.TenantCdSeq = gyosya.TenantCdSeq
		LEFT JOIN VPM_Tokist tokist on yyksho.TokuiSeq = tokist.TokuiSeq AND yyksho.SitenCdSeq= tokist.SitenCdSeq
		    AND yyksho.SeiTaiYmd BETWEEN tokist.SiyoStaYmd
    	    AND tokist.SiyoEndYmd

		LEFT JOIN VPM_Tokisk sircdseq on yyksho.SirCdSeq = sircdseq.TokuiSeq
		    AND yyksho.SeiTaiYmd BETWEEN sircdseq.SiyoStaYmd
   		    AND sircdseq.SiyoEndYmd
    	    AND yyksho.TenantCdSeq = sircdseq.TenantCdSeq

		LEFT JOIN VPM_Tokist sirkist on yyksho.SirCdSeq = sirkist.TokuiSeq AND yyksho.SirSitenCdSeq= sirkist.SitenCdSeq
		    AND yyksho.SeiTaiYmd BETWEEN sirkist.SiyoStaYmd
    	    AND sirkist.SiyoEndYmd

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

		AND (@customerStart IS NULL OR @customerStart = '' OR (FORMAT(gyosya.GyosyaCd, '000') + FORMAT(tokisk.TokuiCd, '0000') + FORMAT(tokist.SitenCd, '0000')) >= @customerStart)		-- 得意先コード　開始
		AND	(@customerEnd IS NULL OR @customerEnd = '' OR (FORMAT(gyosya.GyosyaCd, '000') + FORMAT(tokisk.TokuiCd, '0000') + FORMAT(tokist.SitenCd, '0000')) <= @customerEnd)			-- 得意先コード　終了

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
	)
	SELECT COUNT(0) as 'Count'
			, IsNull(SUM(CAST(UntKin as bigint)), 0) as SumBusFee
			, IsNull(SUM(CAST(ZeiRui as bigint)), 0) as SumBusTax
			, IsNull(SUM(CAST(TesuRyoG as bigint)), 0) as SumBusCharge
			, IsNull(SUM(CAST(GuitKin as bigint)), 0) as SumGuideFee
			, IsNull(SUM(CAST(TaxGuider as bigint)), 0) as SumGuideTax
			, IsNull(SUM(CAST(FeeGuider as bigint)), 0) as SumGuideCharge
			FROM SummaryResult_CTE
	OPTION (RECOMPILE);

	SELECT  ISNULL(tokisk.TokuiNm, '') AS TokuiSaki,
	        ISNULL(sircdseq.RyakuNm, '') AS ShiireSaki,
	        kaknin.CountKak, yyksho.KaktYmd,
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
    AND yyksho.TenantCdSeq = @tenantId		
    AND yyksho.SiyoKbn = 1
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
	          AND syasyu.TenantCdSeq = yyksho.TenantCdSeq
	LEFT JOIN VPM_Syain eigtan on yyksho.EigTanCdSeq = eigtan.SyainCdSeq
	LEFT JOIN VPM_Syain intan on yyksho.InTanCdSeq = intan.SyainCdSeq
	LEFT JOIN VPM_Eigyos eigyos on yyksho.UkeEigCdSeq = eigyos.EigyoCdSeq
	LEFT JOIN VPM_Compny company on eigyos.CompanyCdSeq = company.CompanyCdSeq AND company.TenantCdSeq = @tenantId
	LEFT JOIN VPM_YoyKbn yoykbn on yyksho.YoyaKbnSeq = yoykbn.YoyaKbnSeq
	                           AND yyksho.TenantCdSeq = yoykbn.TenantCdSeq
	LEFT JOIN VPM_Tokisk tokisk on yyksho.TokuiSeq = tokisk.TokuiSeq
	    AND yyksho.SeiTaiYmd BETWEEN tokisk.SiyoStaYmd
    AND tokisk.SiyoEndYmd
    AND yyksho.TenantCdSeq = tokisk.TenantCdSeq
    LEFT JOIN VPM_Gyosya gyosya ON tokisk.GyosyaCdSeq = gyosya.GyosyaCdSeq
    AND tokisk.TenantCdSeq = gyosya.TenantCdSeq
	LEFT JOIN VPM_Tokist tokist on yyksho.TokuiSeq = tokist.TokuiSeq AND yyksho.SitenCdSeq= tokist.SitenCdSeq
	    AND yyksho.SeiTaiYmd BETWEEN tokist.SiyoStaYmd
        AND tokist.SiyoEndYmd

	LEFT JOIN VPM_Tokisk sircdseq on yyksho.SirCdSeq = sircdseq.TokuiSeq
	    AND yyksho.SeiTaiYmd BETWEEN sircdseq.SiyoStaYmd
        AND sircdseq.SiyoEndYmd
        AND yyksho.TenantCdSeq = sircdseq.TenantCdSeq

	LEFT JOIN VPM_Tokist sirkist on yyksho.SirCdSeq = sirkist.TokuiSeq AND yyksho.SirSitenCdSeq= sirkist.SitenCdSeq
	    AND yyksho.SeiTaiYmd BETWEEN sirkist.SiyoStaYmd
        AND sirkist.SiyoEndYmd
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
	
	AND (@customerStart IS NULL OR @customerStart = '' OR (FORMAT(gyosya.GyosyaCd, '000') + FORMAT(tokisk.TokuiCd, '0000') + FORMAT(tokist.SitenCd, '0000')) >= @customerStart)		-- 得意先コード　開始
	AND	(@customerEnd IS NULL OR @customerEnd = '' OR (FORMAT(gyosya.GyosyaCd, '000') + FORMAT(tokisk.TokuiCd, '0000') + FORMAT(tokist.SitenCd, '0000')) <= @customerEnd)			-- 得意先コード　終了

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
	FETCH NEXT @take ROWS ONLY OPTION (RECOMPILE);
End
GO


