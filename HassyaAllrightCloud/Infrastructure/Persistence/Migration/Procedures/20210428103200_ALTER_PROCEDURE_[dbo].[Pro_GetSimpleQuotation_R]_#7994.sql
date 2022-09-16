USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetSimpleQuotatiON_R]    Script Date: 2021/04/28 10:10:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER   procedure [dbo].[Pro_GetSimpleQuotatiON_R]
		@tenantId int,
		@isWithJourney bit,
		@bookingKeys Tblt_BookingKeyType READONLY
AS
BEGIN
	DECLARE @checkedKataKbnTenant INT = (SELECT dbo.CheckCodeKbFunct('KATAKBN', @tenantId))
	DECLARE @checkedZeihyoKbnTenant INT = (SELECT dbo.CheckCodeKbFunct('ZEIHYOKBN', @tenantId))
	-- Query 5
	SELECT 
		  Yo.UkeCd					AS 'UkeCd'
		  ,IsNULL(Un.DanTaNm, '')	AS 'UnDanTaNm'
		  ,Company.CompanyNm		AS 'CompanyNm'
		  ,Ei.EigyoNm				AS 'EiEigyoNm'
		  ,Ei.Jyus1					AS 'EiJyus1'
		  ,Ei.Jyus2					AS 'EiJyus2'
		  ,Ei.TelNo					AS 'EiTelNo'
		  ,Ei.FaxNo					AS 'EiFaxNo'
		  ,Staff.SyainNm			AS 'StaffSyainNm'
		  ,IsNULL(Un.HaiSYmd, '')	AS 'UnHaiSYmd'
		  ,IsNULL(Un.TouYmd, '')	AS 'UnTouYmd'
		  ,IsNULL(Un.JyoSyaJin, '')	AS 'UnJyoSyaJin'
		  ,IsNULL(Un.PlusJin, '')	AS 'UnPlusJin'
		  ,Toku.TokuiNm				AS 'TokuTokuiNm'
		  ,Toku.RyakuNm				AS 'TokuRyakuNm'
		  ,Un.KanJNm				AS 'UnKanJNm'
		  ,Shiten.TelNo				AS 'ShitenTelNo'
		  ,Shiten.FaxNo				AS 'ShitenFaxNo'
		  ,Un.BikoNm				AS 'MitBiko'

		  ,IsNULL(Un.UkeNo, '')		AS 'UkeNo'
		  ,IsNULL(Un.UnkRen, '0')	AS 'UnkRen'
		  ,Yo.YoyaNm				AS 'YoYoyaNm'
		  ,Yo.UkeEigCdSeq			AS 'YoUkeEigCdSeq'
		  ,Yo.EigTanCdSeq			AS 'YoEigTanCdSeq'
		  ,Yo.TokuiSeq				AS 'YoTokuiSeq'
		  ,Yo.SirSitenCdSeq			AS 'YoSirSitenCdSeq'
		  ,Yo.Zeiritsu				AS 'YoZeiritsu'
	FROM @bookingKeys keys
	INNER JOIN TKD_Yyksho AS Yo ON keys.UkeNo = Yo.UkeNo
	LEFT JOIN TKD_Unkobi AS Un ON Un.UkeNo = keys.UkeNo AND Un.UnkRen = keys.UnKren
	INNER JOIN VPM_Eigyos AS Ei ON Ei.EigyoCdSeq=Yo.UkeEigCdSeq
	INNER JOIN VPM_Tokisk AS Toku
		  ON Toku.TenantCdSeq=Yo.TenantCdSeq
		  AND Toku.TokuiSeq=Yo.TokuiSeq
		  AND Yo.UkeYmd BETWEEN Toku.SiyoStaYmd AND Toku.SiyoEndYmd
	INNER JOIN VPM_TokiSt AS Shiten
		  ON Shiten.SitenCdSeq=Yo.SirSitenCdSeq
		  AND Shiten.TokuiSeq=Yo.TokuiSeq
		  AND Yo.UkeYmd BETWEEN Shiten.SiyoStaYmd AND Shiten.SiyoEndYmd
	INNER JOIN VPM_Syain AS Staff ON Staff.SyainCdSeq=Yo.EigTanCdSeq
	INNER JOIN VPM_Compny AS Company ON Company.CompanyCdSeq=Ei.CompanyCdSeq
	WHERE Yo.YoyaSyu=3
			AND Yo.SiyoKbn=1
			AND Un.SiyoKbn=1
			AND Yo.TenantCdSeq=@tenantId
			AND Company.TenantCdSeq=@tenantId
	ORDER BY UkeNo, UnkRen

	-- Query 9
	SELECT cal.UkeNo AS 'UkeNo', cal.UnkRen AS 'UnkRen'
		  ,SUM(CAST(RunningKmCalc AS int)) AS 'SoukouKiro'
		  ,SUM(CAST(LEFT(RestraintTimeCalc,2) AS int)) AS 'SoukouTime'
		  ,SUM(FareMaxAmount) AS 'MaxPrice'
		  ,SUM(FareMinAmount)　AS 'MinPrice'
	FROM TKD_BookingMaxMinFareFeeCalc AS cal
	INNER JOIN @bookingKeys keys ON cal.UkeNo = keys.UkeNo AND cal.UnkRen = keys.UnkRen
	GROUP BY cal.UkeNo, cal.UnkRen
	ORDER BY cal.UkeNo, cal.UnkRen

	-- Query 6, 7 init
	SELECT	
			--query 6
			 Shu.SyaSyuDai				AS 'SyaSyuDai' -- suryo
			,Shu.SyaSyuTan				AS 'SyaSyuTan' --tanka
			--query 7
			,Shu.GuiderNum				AS 'GuiderNum' -- suryo
			,Shu.UnitGuiderPrice		AS 'UnitGuiderPrice' --tanka
			,Shu.SyaSyuRen				AS 'SyaSyuRen' -- order
			--commON
			,Shu.UkeNo					AS 'UkeNo'
			,Shu.UnkRen					AS 'UnkRen'
			,IsNULL(Yo.ZeiKbn, 0)　			AS 'ZeiKbnCar'		--tax type CAR
			,IsNULL(Yo.Zeiritsu, 0)			AS 'ZeiritsuCar' -- tax rate CAR
			,IsNULL(Yo.TaxTypeforGuider, 0) AS 'ZeiKbnGuider'		--tax type GUIDER
			,IsNULL(Yo.FeeGuiderRate, 0)		AS 'ZeiritsuGuider' -- tax rate GUIDER
			,IsNULL(Kaset.SyohiHasu, 1)	AS 'SyohiHasu'--round type
			,IsNULL(CodeKb.CodeKbnNm, '') AS 'CarType'
			,IsNULL(CodeKb.CodeKbn, '') AS 'CodeKbn'
	INTO #query6AND7
	FROM TKD_YykSyu AS Shu
	INNER JOIN @bookingKeys keys ON Shu.UkeNo = keys.UkeNo AND Shu.UnkRen = keys.UnkRen
	LEFT JOIN TKD_YykSho AS Yo ON Shu.UkeNo=Yo.UkeNo
	LEFT JOIN VPM_Eigyos AS Ei ON Yo.UkeEigCdSeq=Ei.EigyoCdSeq
	LEFT JOIN VPM_Compny AS Company ON Company.CompanyCdSeq=Ei.CompanyCdSeq AND Company.TenantCdSeq=@tenantId
	LEFT JOIN TKM_KasSet AS Kaset ON Kaset.CompanyCdSeq=Company.CompanyCdSeq
	LEFT JOIN VPM_CodeKb AS CodeKb
		 ON CONVERT(tinyint, CodeKb.CodeKbn)=Shu.KataKbn
		 AND CodeSyu='KATAKBN'
		 AND CodeKb.TenantCdSeq = @checkedKataKbnTenant
	WHERE Shu.SiyoKbn=1 AND Yo.TenantCdSeq=@tenantId

	-- Query 6 - fare
	SELECT UkeNo
			, UnkRen
			, SyaSyuRen
			, SyaSyuDai AS 'Quantity'
			, SyaSyuTan AS 'Price'
			, ZeiKbnCar AS 'ZeiKbn'
			, CodeKbTax.CodeKbnNm AS 'TaxName'
			, ZeiritsuCar AS 'Zeiritsu'
			, SyohiHasu
			, 1 AS 'RowType'
			, '貸切バス料金(' + CarType + ')' AS ItemName
	FROM #query6AND7
	LEFT JOIN VPM_CodeKb AS CodeKbTax
		  ON CodeKbTax.CodeKbn=ZeiKbnCar
		  AND CodeKbTax.CodeSyu='ZEIHYOKBN'
		  AND CodeKbTax.TenantCdSeq=@checkedZeihyoKbnTenant
	UNION
	-- Query 7 - guider fee list
	(SELECT UkeNo
			, UnkRen
			, SyaSyuRen
			, GuiderNum AS 'Quantity'
			, UnitGuiderPrice AS 'Price'
			, ZeiKbnGuider AS 'ZeiKbn'
			, CodeKbTax.CodeKbnNm AS 'TaxName'
			, ZeiritsuGuider AS 'Zeiritsu'
			, SyohiHasu
			, 2 AS 'RowType'
			, 'ガイド料' AS ItemName
	FROM #query6AND7
	LEFT JOIN VPM_CodeKb AS CodeKbTax
		  ON CodeKbTax.CodeKbn=ZeiKbnGuider
		  AND CodeKbTax.CodeSyu='ZEIHYOKBN'
		  AND CodeKbTax.TenantCdSeq=@checkedZeihyoKbnTenant
	WHERE GuiderNum != 0)
	order by UkeNo, UnkRen, SyaSyuRen, RowType

	-- Query 8 - incidental
	Select 
			FuTum.UkeNo						AS 'UkeNo'
		   ,FuTum.UnkRen					AS 'UnkRen'
		   ,FuTum.FutTumKbn					AS 'FutTumKbn'
		   ,FuTum.FutTumNm					AS 'FutTumNm'
		   ,FuTum.Suryo						AS 'Suryo'
		   ,FuTum.TanKa						AS 'TanKa'
		   ,IsNULL(FuTum.Zeiritsu, 0)		AS 'Zeiritsu' -- tax rate
		   ,IsNULL(FuTum.ZeiKbn, 0)			AS 'ZeiKbn' --tax type
		   ,IsNULL(CodeKB.CodeKbnNm, '')	AS 'TaxName' --tax name
		   ,IsNULL(Kaset.SyohiHasu, 1)		AS 'SyohiHasu' --round type
		   ,IsNULL(FuTum.BikoNm, '')		AS 'BikoNm'
		   ,IsNULL(MFutai.FutGuiKbn, '0')	AS 'FutGuiKbn'
	FROM TKD_FutTum AS FuTum
	INNER JOIN @bookingKeys keys ON FuTum.UkeNo = keys.UkeNo AND FuTum.UnkRen = keys.UnkRen
	LEFT JOIN VPM_CodeKb AS CodeKB
		  ON CodeKB.CodeKbn=FuTum.ZeiKbn
		  AND CodeKB.CodeSyu='ZEIHYOKBN'
		  AND CodeKB.TenantCdSeq=@checkedZeihyoKbnTenant
	LEFT JOIN TKD_Yyksho AS Yo ON Yo.UkeNo=FuTum.UkeNo
	LEFT JOIN VPM_Eigyos AS Ei ON Yo.UkeEigCdSeq=Ei.EigyoCdSeq
	LEFT JOIN VPM_Compny AS Company ON Company.CompanyCdSeq=Ei.CompanyCdSeq AND Company.TenantCdSeq=@tenantId
	LEFT JOIN TKM_KasSet AS Kaset ON Kaset.CompanyCdSeq=Company.CompanyCdSeq
	LEFT JOIN VPM_Futai AS MFutai ON Futum.FutTumCdSeq=MFutai.FutaiCdSeq
	WHERE FuTum.SiyoKbn=1

	IF(@isWithJourney = 0)
	BEGIN
		-- Query 10
		SELECT Shu.UkeNo AS 'UkeNo'
				,Shu.UnkRen AS 'UnkRen'
				,SUM(Shu.SyaSyuDai) AS 'SumSyaSyuDai'
				,IsNULL(CodeKb.CodeKbnNm, '') AS 'CodeKbnNm'
				,IsNULL(CodeKb.CodeKbn, '') AS 'CodeKbn'
		FROM TKD_YykSyu AS Shu
		INNER JOIN @bookingKeys keys ON Shu.UkeNo = keys.UkeNo AND Shu.UnkRen = keys.UnkRen
		LEFT JOIN VPM_CodeKb AS CodeKb
			 ON CodeKb.CodeKbn=Shu.KataKbn
			 AND CodeSyu='KATAKBN'
			 AND CodeKb.TenantCdSeq = @checkedKataKbnTenant
		WHERE Shu.SiyoKbn = 1
		GROUP BY Shu.UkeNo, Shu.UnkRen, CodeKb.CodeKbnNm, CodeKb.CodeKbn
		ORDER BY shu.UkeNo, shu.UnkRen, CodeKb.CodeKbn
	END
	ELSE
	BEGIN
		-- Query 10, 11 journey
		SELECT Unkobi.HaiSYmd
			  ,Kotei.UkeNo
			  ,Kotei.UnkRen
			  ,Kotei.Nittei
			  ,Kotei.KouRen
			  ,Kotei.Koutei
		FROM @bookingKeys keys
		INNER JOIN TKD_Kotei AS Kotei ON keys.UkeNo=Kotei.UkeNo AND keys.UnKren=Kotei.UnkRen
		LEFT JOIN TKD_Unkobi AS Unkobi ON Unkobi.UkeNo=Kotei.UkeNo AND Unkobi.UnkRen=Kotei.UnkRen
		WHERE Kotei.TeiDanNo=0 AND Kotei.TomKbn=1 AND Kotei.SiyoKbn=1
		ORDER BY Kotei.UkeNo, Kotei.Nittei, Kotei.KouRen

		SELECT Unkobi.HaiSYmd
			  ,Tehai.UkeNo
			  ,Tehai.UnkRen
			  ,Tehai.Nittei
			  ,Tehai.TehRen
			  ,Tehai.TehNm
		FROM @bookingKeys keys
		INNER JOIN TKD_Tehai AS Tehai ON keys.UkeNo=Tehai.UkeNo AND keys.UnKren=Tehai.UnkRen
		LEFT JOIN TKD_Unkobi AS Unkobi ON Unkobi.UkeNo=Tehai.UkeNo AND Unkobi.UnkRen=Tehai.UnkRen
		WHERE Tehai.TeiDanNo=0 AND Tehai.TomKbn=1 AND Tehai.SiyoKbn=1
		ORDER BY Tehai.UkeNo, Tehai.Nittei, Tehai.TehRen
		
		DROP TABLE #journeyKeys
		
	END

	DROP TABLE #query6AND7
END

GO


