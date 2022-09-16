USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicleCrew_R]    Script Date: 2021/05/18 8:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dVehicleCrew_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data of crews
-- Date			:   2020/08/11
-- Author		:   N.T.Lan.Anh
-- Description	:   Get data of crews for super menu vehicle with conditions
------------------------------------------------------------
CREATE OR ALTER           PROCEDURE [dbo].[PK_dVehicleCrew_R]
(
	-- Parameter
		    @TenantCdSeq int    
		,	@CompanyCdSeq int
		,	@StartDispatchDate nvarchar(8)              -- 配車日 開始         
		,	@EndDispatchDate nvarchar(8)                -- 配車日 終了
		,	@StartArrivalDate nvarchar(8)               -- 到着日　開始
		,	@EndArrivalDate nvarchar(8)                 -- 到着日　終了
		,	@StartReservationDate nvarchar(8)           -- 予約日　開始
		,	@EndReservationDate nvarchar(8)             -- 予約日　終了
		,	@StartReceiptNumber char(15)                -- 受付番号　開始
		,	@EndReceiptNumber char(15)                  -- 受付番号　終了
		,	@StartReservationClassification int         -- 予約区分　開始 
		,	@EndReservationClassification int           -- 予約区分　終了
		,	@StartServicePerson nvarchar(10)                     -- 営業担当　開始
		,	@EndServicePerson nvarchar(10)                       -- 営業担当　終了
		,	@StartRegistrationOffice int                -- 受付営業所　開始
		,	@EndRegistrationOffice int                  -- 受付営業所　終了
		,	@StartInputPerson nvarchar(10)                       -- 入力担当　開始
		,	@EndInputPerson nvarchar(10)                        -- 入力担当　終了
		,	@StartCustomer nvarchar(11)                 -- 得意先　開始
		,	@EndCustomer nvarchar(11)                   -- 得意先　終了
		,	@StartSupplier nvarchar(11)                 -- 仕入先　開始
		,	@EndSupplier nvarchar(11)                   -- 仕入先　終了
		,	@StartGroupClassification nvarchar(10)      -- 団体区分　開始
		,	@EndGroupClassification nvarchar(10)        -- 団体区分　終了
		,	@StartCustomerTypeClassification int        -- 客種区分　開始
		,	@EndCustomerTypeClassification int          -- 客種区分　終了
		,	@StartDestination nvarchar(10)              -- 行先　開始               
		,	@EndDestination nvarchar(10)                -- 行先　終了               
		,	@StartDispatchPlace nvarchar(10)            -- 配車地　開始             
		,	@EndDispatchPlace nvarchar(10)              -- 配車地　終了             
		,	@StartOccurrencePlace nvarchar(10)          -- 発生地　開始             
		,	@EndOccurrencePlace nvarchar(10)            -- 発生地　終了             
		,	@StartArea nvarchar(10)                     -- エリア　開始              
		,	@EndArea nvarchar(10)                       -- エリア　終了 
		,   @StartReceiptCondition nvarchar(10)
	    ,   @EndReceiptCondition nvarchar(10)
	    ,   @StartCarType int
	    ,   @EndCarType int
	    ,   @StartCarTypePrice int
	    ,   @EndCarTypePrice int
		,   @DantaNm nvarchar(100)
		,   @StartFilterNo nvarchar(30)
	    ,   @EndFilterNo nvarchar(30)
	)
AS
BEGIN
WITH 
eTKD_Haisha00 AS (
	SELECT TKD_Haisha.*,
		CASE WHEN ISNULL(VPM_Eigyos.CompanyCdSeq, 0) = @CompanyCdSeq
				THEN 
					CASE WHEN TKD_Haisha.YouTblSeq <> 0 AND	ISNULL(TKD_Yousha.JitaFlg, 1) = 0 THEN '自社受傭車'
						ELSE '自社受自社'
						END
			ELSE CASE WHEN TKD_Haisha.YouTblSeq = 0 THEN '自社受自社'
						ELSE '他社受自社'
						END
			END AS JiTaAtukaiFlg,
		CASE WHEN TKD_Haisha.HaiSKbn =	2 THEN	TKD_Haisha.HaiSSryCdSeq
			WHEN TKD_Haisha.HaiSKbn <> 2 AND TKD_Haisha.KSKbn = 2 THEN TKD_Haisha.KSSyaRSeq
			ELSE 0
			END AS HsKsSryCdSeq
	FROM TKD_Haisha
	LEFT JOIN TKD_Yyksho
		ON		TKD_Haisha.UkeNo		=		TKD_Yyksho.UkeNo
		AND		TKD_Yyksho.SiyoKbn		=		1
	LEFT JOIN	VPM_Eigyos ON TKD_Yyksho.UkeEigCdSeq = VPM_Eigyos.EigyoCdSeq
	LEFT JOIN	TKD_Yousha ON TKD_Haisha.UkeNo = TKD_Yousha.UkeNo
				AND		TKD_Haisha.UnkRen		=		TKD_Yousha.UnkRen
				AND		TKD_Haisha.YouTblSeq	=		TKD_Yousha.YouTblSeq
				AND		TKD_Yousha.SiyoKbn		=		1
    WHERE TKD_Haisha.SiyoKbn = 1 -- 2021/04/23 ADD
),
eTKD_Haisha10 AS (
	SELECT TKD_Haisha.*,
		ISNULL(eTKD_Haiin10.HaiInRen, 0) AS HaiInRen,
		ISNULL(eTKD_Haiin10.SyainCdSeq, 0) AS SyainCdSeq,
		ISNULL(eVPM_Syain02.SyainCd, 0) AS SyainCd,
		CASE WHEN TKD_Haisha.HaiSKbn = 2 THEN ISNULL(eVPM_Syain02.SyainNm, ' ' )
			WHEN TKD_Haisha.HaiSKbn <> 2 AND TKD_Haisha.KSKbn = 2 THEN ISNULL(eVPM_Syain02.KariSyainNm, ' ')
			ELSE ' '
			END AS SyainNm
	FROM TKD_Haisha
	LEFT JOIN TKD_Yyksho
		ON TKD_Haisha.UkeNo = TKD_Yyksho.UkeNo
		AND TKD_Yyksho.SiyoKbn = 1
	LEFT JOIN TKD_Haiin AS eTKD_Haiin10
		ON TKD_Haisha.UkeNo = eTKD_Haiin10.UkeNo
		AND TKD_Haisha.UnkRen = eTKD_Haiin10.UnkRen
		AND TKD_Haisha.TeiDanNo = eTKD_Haiin10.TeiDanNo
		AND TKD_Haisha.BunkRen = eTKD_Haiin10.BunkRen
		AND eTKD_Haiin10.SiyoKbn = 1
	LEFT JOIN VPM_Syain AS eVPM_Syain02
		ON eTKD_Haiin10.SyainCdSeq = eVPM_Syain02.SyainCdSeq
	WHERE TKD_Haisha.SiyoKbn = 1 -- 2021/04/23 ADD
),
eTKD_Haisha20 AS (
	SELECT	DISTINCT
		eTKD_Haisha10.UkeNo								AS		UkeNo,
		eTKD_Haisha10.UnkRen							AS		UnkRen,
		eTKD_Haisha10.TeiDanNo							AS		TeiDanNo,
		eTKD_Haisha10.BunkRen							AS		BunkRen,
		eTKD_Haisha10.SyainCd                           AS		SyainCd,
		MAX(eTKD_Haisha10.SyainNm)                      AS		SyainNm
	FROM eTKD_Haisha10
	GROUP BY eTKD_Haisha10.UkeNo,
		eTKD_Haisha10.UnkRen,
		eTKD_Haisha10.TeiDanNo,
		eTKD_Haisha10.BunkRen,
		eTKD_Haisha10.SyainCd
)

SELECT ISNULL(TKD_Yyksho.UkeNo				,0		)	AS		UkeNo,
	ISNULL(eTKD_Unkobi01.UnkRen				,0		)	AS		UnkRen,
	ISNULL(eTKD_Haisha01.TeiDanNo			,0		)	AS		TeiDanNo,
	ISNULL(eTKD_Haisha01.BunkRen			,0		)	AS		BunkRen,
	ISNULL(eTKD_Haisha20.SyainCd			,' '	)	AS		SyainCd,
	ISNULL(eTKD_Haisha20.SyainNm			,' '	)	AS		SyainNm
FROM eTKD_Haisha00 AS eTKD_Haisha01
LEFT JOIN TKD_Unkobi AS	eTKD_Unkobi01 
	ON eTKD_Unkobi01.UkeNo = eTKD_Haisha01.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_Haisha01.UnkRen
	AND eTKD_Unkobi01.SiyoKbn =	1
	JOIN eTKD_Haisha20
	ON eTKD_Haisha01.UkeNo = eTKD_Haisha20.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Haisha20.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Haisha20.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Haisha20.BunkRen
LEFT JOIN TKD_Yyksho
	ON eTKD_Haisha01.UkeNo = TKD_Yyksho.UkeNo
LEFT JOIN (select * from VPM_Tokisk where TenantCdSeq = @TenantCdSeq) AS eVPM_Tokisk01
	ON TKD_Yyksho.TokuiSeq = eVPM_Tokisk01.TokuiSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
	
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt01
	ON TKD_Yyksho.TokuiSeq = eVPM_TokiSt01.TokuiSeq	
	AND TKD_Yyksho.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd >= eVPM_TokiSt01.SiyoStaYmd 
	AND TKD_Yyksho.SeiTaiYmd <= eVPM_TokiSt01.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya02
	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
	AND eVPM_Tokisk01.TenantCdSeq = eVPM_Gyosya02.TenantCdSeq -- 2021/05/25 ADD
LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk04
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk04.TokuiSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk04.SiyoStaYmd AND eVPM_Tokisk04.SiyoEndYmd
	AND eVPM_Tokisk04.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt05
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt05.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt05.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd >= eVPM_TokiSt05.SiyoStaYmd 
	AND TKD_Yyksho.SeiTaiYmd <= eVPM_TokiSt05.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya03
	ON eVPM_Tokisk04.GyosyaCdSeq = eVPM_Gyosya03.GyosyaCdSeq
	AND eVPM_Tokisk04.TenantCdSeq = eVPM_Gyosya03.TenantCdSeq -- 2021/05/25 ADD
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos05
	ON TKD_Yyksho.UkeEigCdSeq = eVPM_Eigyos05.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny01
	ON eVPM_Eigyos05.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain01
	ON TKD_Yyksho.EigTanCdSeq = eVPM_Syain01.SyainCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain02
	ON TKD_Yyksho.InTanCdSeq = eVPM_Syain02.SyainCdSeq
LEFT JOIN VPM_YoyKbn AS	eVPM_YoyKbn01
	ON TKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
	AND eVPM_YoyKbn01.TenantCdSeq = @TenantCdSeq   --				2021/05/25 ADD
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb17
	ON TKD_Yyksho.SeiKyuKbnSeq = eVPM_CodeKb17.CodeKbnSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo01
	ON eTKD_Unkobi01.HasMapCdSeq = eVPM_Basyo01.BasyoMapCdSeq
	AND eVPM_Basyo01.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq 2021/05/18 ADD
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb05
	ON eVPM_Basyo01.BasyoKenCdSeq = eVPM_CodeKb05.CodeKbnSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo02
	ON eTKD_Unkobi01.AreaMapSeq	= eVPM_Basyo02.BasyoMapCdSeq
	AND eVPM_Basyo02.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq 2021/05/18 ADD
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb06
	ON eVPM_Basyo02.BasyoKenCdSeq = eVPM_CodeKb06.CodeKbnSeq
LEFT JOIN VPM_JyoKya AS	eVPM_JyoKya01
	ON eTKD_Unkobi01.JyoKyakuCdSeq = eVPM_JyoKya01.JyoKyakuCdSeq
	AND eVPM_JyoKya01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb07
	ON eVPM_JyoKya01.DantaiCdSeq = eVPM_CodeKb07.CodeKbnSeq
LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo03
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo03.SyaRyoCdSeq
LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu01
	ON eVPM_SyaRyo03.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq
	AND eVPM_SyaSyu01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo03
	ON eTKD_Haisha01.IkMapCdSeq	= eVPM_Basyo03.BasyoMapCdSeq
	AND eVPM_Basyo03.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq 2021/05/18 ADD
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb08
	ON eVPM_Basyo03.BasyoKenCdSeq = eVPM_CodeKb08.CodeKbnSeq
LEFT JOIN VPM_Haichi AS	eVPM_Haichi01
	ON eTKD_Haisha01.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq
	AND eVPM_Haichi01.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq 2021/05/18 ADD
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb09
	ON eVPM_Haichi01.BunruiCdSeq = eVPM_CodeKb09.CodeKbnSeq
LEFT JOIN TKD_YykSyu AS	eTKD_YykSyu01
	ON eTKD_Haisha01.UkeNo = eTKD_YykSyu01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YykSyu01.UnkRen
	AND eTKD_Haisha01.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen
	AND eTKD_YykSyu01.SiyoKbn = 1
WHERE
	TKD_Yyksho.SiyoKbn = 1
	AND TKD_Yyksho.YoyaSyu = 1
	AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq																																			-- ログインユーザの会社のCompanyCdSeq
	AND (@StartDispatchDate IS NULL OR eTKD_Haisha01.HaiSYmd >= @StartDispatchDate)																										-- 配車日　開始
	AND (@EndDispatchDate IS NULL OR eTKD_Haisha01.HaiSYmd <= @EndDispatchDate)																											-- 配車日　終了
	AND (@StartArrivalDate IS NULL OR eTKD_Haisha01.TouYmd >= @StartArrivalDate)																										-- 到着日　開始
	AND (@EndArrivalDate IS NULL OR eTKD_Haisha01.TouYmd <= @EndArrivalDate)																											-- 到着日　終了
	AND (@StartReservationDate IS NULL OR TKD_Yyksho.UkeYmd >= @StartReservationDate)																									-- 予約日　開始
	AND (@EndReservationDate IS NULL OR TKD_Yyksho.UkeYmd <= @EndReservationDate)																										-- 予約日　終了
	AND (@StartReceiptNumber IS NULL OR TKD_Yyksho.UkeNo >= @StartReceiptNumber)																										-- 予約番号　開始
	AND (@EndReceiptNumber IS NULL OR TKD_Yyksho.UkeNo <= @EndReceiptNumber)																											-- 予約番号　終了
	AND (@StartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)																			-- 予約区分　開始
	AND (@EndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)																				-- 予約区分　終了
	AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson)																									-- 営業担当者　開始
	AND (@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson)																										-- 営業担当者　終了
	AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos05.EigyoCd >= @StartRegistrationOffice)																							-- 営業所　開始
	AND (@EndRegistrationOffice IS NULL OR eVPM_Eigyos05.EigyoCd <= @EndRegistrationOffice)																								-- 営業所　終了
	AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson)																										-- 入力担当者　開始
	AND (@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson)																											-- 入力担当者　終了
	AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt01.SitenCd,'0000') >= @StartCustomer)				-- 得意先コード　開始
	AND (@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt01.SitenCd,'0000') <= @EndCustomer)					-- 得意先コード　終了
	AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') + FORMAT(eVPM_Tokisk04.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt05.SitenCd,'0000') >= @StartSupplier)				-- 仕入先コード　開始
	AND (@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') + FORMAT(eVPM_Tokisk04.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt05.SitenCd,'0000') <= @EndSupplier)					-- 仕入先コード　終了
	AND (@StartGroupClassification IS NULL OR eVPM_CodeKb07.CodeKbn >= @StartGroupClassification)																						-- 団体区分　開始
	AND (@EndGroupClassification IS NULL OR eVPM_CodeKb07.CodeKbn <= @EndGroupClassification)																							-- 団体区分　終了
	AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification)																		-- 乗客コード　開始
	AND (@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification)																			-- 乗客コード　終了
	AND (@StartDestination IS NULL OR eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartDestination)																				-- 行先　開始
	AND (@EndDestination IS NULL OR eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndDestination)																					-- 行先　終了
	AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace)																			-- 配車地　開始
	AND (@EndDispatchPlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace)																				-- 配車地　終了
	AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartOccurrencePlace)																		-- 発生地　開始
	ANd (@EndOccurrencePlace IS NULL OR eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndOccurrencePlace)																			-- 発生地　終了
	AND (@StartArea IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartArea)																							-- エリア　開始
	AND (@EndArea IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndArea)																								-- エリア　終了
	AND (@StartReceiptCondition IS NULL OR eTKD_Haisha01.UkeJyKbnCd >= @StartReceiptCondition)																							-- 受付条件　開始
	AND (@EndReceiptCondition IS NULL OR eTKD_Haisha01.UkeJyKbnCd <= @EndReceiptCondition)																								-- 受付条件　終了
	AND (@StartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @StartCarType)																												-- 車種　開始
	AND (@EndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @EndCarType)																													-- 車種　終了
	AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice)																									-- 車種単価　開始
	AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice)																										-- 車種単価　終了
	AND (@DantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @DantaNm, '%'))
	AND TKD_Yyksho.UkeNo * 1000000000000000 +  ISNULL(eTKD_Unkobi01.UnkRen, 0) * 10000000000 +  ISNULL(eTKD_Haisha01.TeiDanNo, 0) * 100000  + ISNULL(eTKD_Haisha01.BunkRen,0) >=  CAST(@StartFilterNo as DECIMAL(30,0))
	AND TKD_Yyksho.UkeNo * 1000000000000000 +  ISNULL(eTKD_Unkobi01.UnkRen, 0) * 10000000000 +  ISNULL(eTKD_Haisha01.TeiDanNo, 0) * 100000  + ISNULL(eTKD_Haisha01.BunkRen,0) <= CAST(@EndFilterNo as DECIMAL(30,0))
	OPTION (RECOMPILE)
END
