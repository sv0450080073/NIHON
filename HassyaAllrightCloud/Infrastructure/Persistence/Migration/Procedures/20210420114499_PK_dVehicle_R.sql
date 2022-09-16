USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicle_R]    Script Date: 2021/04/06 13:04:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dVehicle_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data vehicle list
-- Date			:   2020/08/11
-- Author		:   N.T.Lan.Anh
-- Description	:   Get data for super menu vehicle list with conditions
------------------------------------------------------------
CREATE OR ALTER             PROCEDURE [dbo].[PK_dVehicle_R]
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
		,	@StartServicePerson int                     -- 営業担当　開始
		,	@EndServicePerson int                       -- 営業担当　終了
		,	@StartRegistrationOffice int                -- 受付営業所　開始
		,	@EndRegistrationOffice int                  -- 受付営業所　終了
		,	@StartInputPerson int                       -- 入力担当　開始
		,	@EndInputPerson int                         -- 入力担当　終了
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
		,	@OffSet int
		,	@Fetch int
	)
AS
IF (@OffSet IS NOT NULL)
BEGIN
WITH eVPM_CodeKb AS (
				SELECT VPM_CodeKb.CodeKbnSeq,
					VPM_CodeKb.CodeKbn,
					VPM_CodeKb.CodeSyu,
					VPM_CodeKb.CodeKbnNm,
					VPM_CodeKb.RyakuNm
				FROM VPM_CodeKb
				LEFT JOIN VPM_CodeSy
					ON VPM_CodeKb.CodeSyu = VPM_CodeSy.CodeSyu
				WHERE VPM_CodeKb.CodeSyu IN ('SIJJOKBN1', 'SIJJOKBN2', 'SIJJOKBN3', 'SIJJOKBN4', 'SIJJOKBN5', 'UkeJyKbnCd', 'KATAKBN')
					AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn <> 1 AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq))
					AND VPM_CodeKb.SiyoKbn = 1
			),
eTKD_FutTum00 AS (
	SELECT TKD_FutTum.UkeNo								AS		UkeNo,
		TKD_FutTum.UnkRen								AS		UnkRen,
		TKD_MFutTu.TeiDanNo								AS		TeiDanNo,
		TKD_MFutTu.BunkRen								AS		BunkRen,
		TKD_FutTum.FutTumKbn							AS		FutTumKbn,
		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0
			ELSE VPM_Futai.FutGuiKbn
			END AS FutGuiKbn,
		SUM(CAST(TKD_MFutTu.UriGakKin AS bigint))						AS		UriGakKin_S,
		SUM(CAST(TKD_MFutTu.SyaRyoSyo as bigint))						AS		SyaRyoSyo_S,
		SUM(CAST(TKD_MFutTu.SyaRyoTes as bigint))						AS		SyaRyoTes_S
	FROM TKD_FutTum
	LEFT JOIN VPM_Futai
		ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq
		AND VPM_Futai.SiyoKbn = 1
	JOIN TKD_MFutTu
		ON TKD_FutTum.UkeNo = TKD_MFutTu.UkeNo
		AND TKD_FutTum.UnkRen = TKD_MFutTu.UnkRen
		AND TKD_FutTum.FutTumKbn = TKD_MFutTu.FutTumKbn
		AND TKD_FutTum.FutTumRen = TKD_MFutTu.FutTumRen
		AND TKD_MFutTu.SiyoKbn = 1
	GROUP BY
		TKD_FutTum.UkeNo,
		TKD_FutTum.UnkRen,
		TKD_MFutTu.TeiDanNo,
		TKD_MFutTu.BunkRen,
		TKD_FutTum.FutTumKbn,
		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0
			ELSE VPM_Futai.FutGuiKbn
			END
),
eTKD_YFutTu10 AS (
	SELECT TKD_YMFuTu.*,
		VPM_Futai.FutGuiKbn								AS		SeiFutSyu,
		ISNULL(TKD_Yousha.JitaFlg,1)					AS		JitaFlg
	FROM TKD_YMFuTu
	LEFT JOIN TKD_YFutTu
		ON TKD_YMFuTu.UkeNo = TKD_YFutTu.UkeNo
		AND TKD_YMFuTu.UnkRen = TKD_YFutTu.UnkRen
		AND TKD_YMFuTu.YouTblSeq = TKD_YFutTu.YouTblSeq
		AND TKD_YMFuTu.FutTumKbn = TKD_YFutTu.FutTumKbn
		AND TKD_YMFuTu.YouFutTumRen = TKD_YFutTu.YouFutTumRen
	LEFT JOIN VPM_Futai
		ON TKD_YFutTu.FutTumCdSeq = VPM_Futai.FutaiCdSeq
	LEFT JOIN TKD_Yousha
		ON TKD_YMFuTu.UkeNo = TKD_Yousha.UkeNo
		AND TKD_YMFuTu.UnkRen = TKD_Yousha.UnkRen
		AND TKD_YMFuTu.YouTblSeq = TKD_Yousha.YouTblSeq
		AND TKD_Yousha.SiyoKbn = 1
	WHERE TKD_YMFuTu.SiyoKbn = 1
),
eTKD_YFutTu00 AS (
	SELECT eTKD_YFutTu10.UkeNo							AS		UkeNo,
		eTKD_YFutTu10.UnkRen							AS		UnkRen,
		eTKD_YFutTu10.FutTumKbn							AS		FutTumKbn,
		eTKD_YFutTu10.SeiFutSyu							AS		SeiFutSyu,
		eTKD_YFutTu10.TeiDanNo							AS		TeiDanNo,
		eTKD_YFutTu10.BunkRen							AS		BunkRen,
		SUM(CAST(eTKD_YFutTu10.HaseiKin as bigint)	)					AS		HaseiKin_S,
		SUM(CAST(eTKD_YFutTu10.SyaRyoSyo as bigint)	)					AS		SyaRyoSyo_S,
		SUM(CAST(eTKD_YFutTu10.SyaRyoTes as bigint)	)					AS		SyaRyoTes_S
	FROM eTKD_YFutTu10
	WHERE eTKD_YFutTu10.JitaFlg	= 0
	GROUP BY eTKD_YFutTu10.UkeNo,
		eTKD_YFutTu10.UnkRen,
		eTKD_YFutTu10.FutTumKbn,
		eTKD_YFutTu10.SeiFutSyu,
		eTKD_YFutTu10.TeiDanNo,
		eTKD_YFutTu10.BunkRen
),
eTKD_FutTumCnt AS (
	SELECT TKD_MFutTu.UkeNo AS UkeNo,
		TKD_MFutTu.UnkRen AS UnkRen,
		TKD_MFutTu.TeiDanNo AS TeiDanNo,
		TKD_MFutTu.BunkRen AS BunkRen,
		TKD_MFutTu.FutTumKbn AS FutTumKbn,
		COUNT(TKD_MFutTu.UkeNo) AS CntInS_FutTum
	FROM TKD_MFutTu
	WHERE TKD_MFutTu.SiyoKbn = 1
		AND TKD_MFutTu.Suryo <> 0
	GROUP BY TKD_MFutTu.UkeNo,
		TKD_MFutTu.UnkRen,
		TKD_MFutTu.TeiDanNo,
		TKD_MFutTu.BunkRen,
		TKD_MFutTu.FutTumKbn
),
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
),
eTKD_Haisha20 AS (
	SELECT	DISTINCT
		eTKD_Haisha10.UkeNo								AS		UkeNo,
		eTKD_Haisha10.UnkRen							AS		UnkRen,
		eTKD_Haisha10.TeiDanNo							AS		TeiDanNo,
		eTKD_Haisha10.BunkRen							AS		BunkRen
	FROM eTKD_Haisha10
	GROUP BY eTKD_Haisha10.UkeNo,
		eTKD_Haisha10.UnkRen,
		eTKD_Haisha10.TeiDanNo,
		eTKD_Haisha10.BunkRen
)

	SELECT ISNULL(TKD_Yyksho.UkeNo				,0		)	AS		UkeNo,
	ISNULL(eTKD_Unkobi01.UnkRen				,0		)	AS		UnkRen,
	ISNULL(eTKD_Haisha01.TeiDanNo			,0		)	AS		TeiDanNo,
	ISNULL(eTKD_Haisha01.BunkRen			,0		)	AS		BunkRen,
	ISNULL(TKD_Yyksho.KaknKais				,0		)	AS		KaknKais,
	ISNULL(TKD_Yyksho.KaktYmd				,' '	)	AS		KaktYmd,
	ISNULL(TKD_Yyksho.NyuKinKbn				,0		)	AS		NyuKinKbn,
	ISNULL(TKD_Yyksho.TokuiSeq				,0		)	AS		TokuiSeq,
	ISNULL(eVPM_Tokisk01.TokuiCd			,0		)	AS		TokuiCd,
	ISNULL(TKD_Yyksho.CanFuYmd				,' ')		AS		CanFuYmd,
	ISNULL(TKD_Yyksho.CanFuRiy				,' ')		AS		CanFuRiy,
	ISNULL(eVPM_Syain03.SyainCd				,0)			AS		CanFutanCd,
	ISNULL(eVPM_Syain03.SyainNm				,' ')		AS		CanFutanNm,
	ISNULL(eVPM_Gyosya02.GyosyaCd			,' '	)	AS		GyosyaCd,
	ISNULL(eVPM_Gyosya02.GyosyaNm			,' '	)	AS		GyosyaNm,
	ISNULL(eVPM_Tokisk01.TokuiNm			,' '	)	AS		TokuiNm,
	ISNULL(eVPM_Tokisk01.RyakuNm			,' '	)	AS		TokuiRyakuNm,
	ISNULL(TKD_Yyksho.SitenCdSeq			,0		)	AS		SitenCdSeq,
	ISNULL(eVPM_TokiSt01.SitenCd			,0		)	AS		SitenCd,
	ISNULL(eVPM_TokiSt01.SitenNm			,' '	)	AS		SitenNm,
	ISNULL(eVPM_TokiSt01.RyakuNm			,' '	)	AS		SitenRyakuNm,
	ISNULL(eVPM_TokiSt02.TesKbn				,0		)	AS		SeiSitTesKbn,
	ISNULL(eVPM_TokiSt02.TesKbnFut			,0		)	AS		SeiSitTesKbnFut,
	ISNULL(eVPM_TokiSt02.TesKbnGui			,0		)	AS		SeiSitTesKbnGui,
	ISNULL(TKD_Yyksho.TokuiTanNm			,' '	)	AS		TokuiTanNm,
	ISNULL(eTKD_Unkobi01.DanTaNm			,' '	)	AS		DanTaNm,
	ISNULL(eTKD_Haisha01.DanTaNm2			,' '	)	AS		DanTaNm2,
	ISNULL(eTKD_Haisha01.IkNm				,' '	)	AS		IkNm,
	ISNULL(eTKD_Haisha01.HaiSYmd			,' '	)	AS		HaiSYmd,
	ISNULL(eTKD_Haisha01.HaiSTime			,' '	)	AS		HaiSTime,
	ISNULL(eTKD_Haisha01.HaiSNm				,' '	)	AS		HaiSNm,
	ISNULL(eTKD_Haisha01.HaiSJyuS1			,' '	)	AS		HaiSJyuS1,
	ISNULL(eTKD_Haisha01.HaiSJyuS2			,' '	)	AS		HaiSJyuS2,
	ISNULL(eTKD_Haisha01.TouYmd				,' '	)	AS		TouYmd,
	ISNULL(eTKD_Haisha01.TouChTime			,' '	)	AS		TouChTime,
	ISNULL(eTKD_Haisha01.TouNm				,' '	)	AS		TouNm,
	ISNULL(eTKD_Haisha01.TouJyusyo1			,' '	)	AS		TouJyusyo1,
	ISNULL(eTKD_Haisha01.TouJyusyo2			,' '	)	AS		TouJyusyo2,
	ISNULL(eTKD_Haisha01.DrvJin				,0		)	AS		DrvJin,
	ISNULL(eTKD_Haisha01.GuiSu				,0		)	AS		GuiSu,
	ISNULL(eTKD_Haisha01.SyuPaTime			,' '	)	AS		SyuPaTime,
	ISNULL(eTKD_Haisha01.HsKsSryCdSeq		,0		)	AS		SyaryoCdSeq,
	ISNULL(eVPM_Eigyos01.EigyoCd			,0		)	AS		SyaryoEigCd,
	ISNULL(eVPM_Eigyos01.EigyoNm			,' '	)	AS		SyaryoEigyoNm,
	ISNULL(eVPM_Eigyos01.RyakuNm			,' '	)	AS		SyaryoEigRyakuNm,
	CASE WHEN eTKD_Haisha01.HaiSKbn = 2 THEN ISNULL(eVPM_SyaRyo01.SyaRyoNm ,' '	)
		WHEN eTKD_Haisha01.HaiSKbn <> 2 AND eTKD_Haisha01.KSKbn =	2 THEN ISNULL(eVPM_SyaRyo01.KariSyaRyoNm, ' ')
		ELSE ' '	
		END	AS SyaRyoNm,
	ISNULL(eVPM_SyaRyo01.SyaRyoCd			,' '	)	AS		SyaRyoCd,
	ISNULL(eTKD_Haisha01.GoSya				,' '	)	AS		GoSya,
	ISNULL(eTKD_Yousha01.YouCdSeq			,0		)	AS		YouCdSeq,
	ISNULL(eVPM_Tokisk02.TokuiCd			,0		)	AS		YouCd,
	ISNULL(eVPM_Gyosya04.GyosyaCd			,0		)	AS		YouGyoCd,
	ISNULL(eVPM_Gyosya04.GyosyaNm			,' '	)	AS		YouGyoNm,
	ISNULL(eVPM_Tokisk02.TokuiNm			,' '		)	AS		YouNm,
	ISNULL(eVPM_Tokisk02.RyakuNm			,' '	)	AS		YouRyakuNm,
	ISNULL(eTKD_Yousha01.YouSitCdSeq		,0		)	AS		YouSitCdSeq,
	ISNULL(eVPM_TokiSt03.SitenCd			,0		)	AS		YouSitCd,
	ISNULL(eVPM_TokiSt03.SitenNm			,' '		)	AS		YouSitNm,
	ISNULL(eVPM_TokiSt03.RyakuNm			,' '	)	AS		YouSitCdRyakuNm,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.SyaRyoUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaUnc
	END AS SyaRyoUnc,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.SyaRyoSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaSyo
	END AS SyaRyoSyo,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.SyaRyoTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaTes
	END AS SyaRyoTes,

	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaUnc
	END AS JiSyaRyoUnc,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaSyo
	END AS JiSyaRyoSyo,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaTes
	END AS JiSyaRyoTes,

	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.YoushaUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YoushaUnc,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.YoushaSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YoushaSyo,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.YoushaTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YoushaTes,
	ISNULL(eTKD_FutTum01.UriGakKin_S		,0		)	AS		Gui_UriGakKin_S,
	ISNULL(eTKD_FutTum01.SyaRyoSyo_S		,0		)	AS		Gui_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum01.SyaRyoTes_S		,0		)	AS		Gui_SyaRyoTes_S,
	ISNULL(eTKD_FutTum02.UriGakKin_S		,0		)	AS		Tum_UriGakKin_S,
	ISNULL(eTKD_FutTum02.SyaRyoSyo_S		,0		)	AS		Tum_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum02.SyaRyoTes_S		,0		)	AS		Tum_SyaRyoTes_S,
	ISNULL(eTKD_FutTum03.UriGakKin_S		,0		)	AS		Fut_UriGakKin_S,
	ISNULL(eTKD_FutTum03.SyaRyoSyo_S		,0		)	AS		Fut_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum03.SyaRyoTes_S		,0		)	AS		Fut_SyaRyoTes_S,
	ISNULL(eTKD_FutTum04.UriGakKin_S		,0		)	AS		Tuk_UriGakKin_S,
	ISNULL(eTKD_FutTum04.SyaRyoSyo_S		,0		)	AS		Tuk_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum04.SyaRyoTes_S		,0		)	AS		Tuk_SyaRyoTes_S,
	ISNULL(eTKD_FutTum05.UriGakKin_S		,0		)	AS		Teh_UriGakKin_S,
	ISNULL(eTKD_FutTum05.SyaRyoSyo_S		,0		)	AS		Teh_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum05.SyaRyoTes_S		,0		)	AS		Teh_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu01.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YGui_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu01.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YGui_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu01.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YGui_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu02.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytum_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu02.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytum_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu02.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytum_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu04.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yfut_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu04.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yfut_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu04.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yfut_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu05.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytuk_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu05.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytuk_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu05.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytuk_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu06.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yteh_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu06.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yteh_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu06.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yteh_SyaRyoTes_S,
	ISNULL(eTKD_Haisha01.JyoSyaJin			,0		)	AS		JyoSyaJin,
	ISNULL(eTKD_Haisha01.PlusJin			,0		)	AS		PlusJin,
	ISNULL(eTKD_Haisha01.SyuKoYmd			,' '	)	AS		SyuKoYmd,
	ISNULL(eTKD_Haisha01.SyuKoTime			,' '	)	AS		SyuKoTime,
	ISNULL(eTKD_Haisha01.SyuEigCdSeq		,0		)	AS		SyuEigCdSeq,
	ISNULL(eVPM_Eigyos03.EigyoCd			,0		)	AS		SyuEigCd,
	ISNULL(eVPM_Eigyos03.EigyoNm			,0		)	AS		SyuEigNm,
	ISNULL(eVPM_Eigyos03.RyakuNm			,' '	)	AS		SyuEigRyakuNm,
	ISNULL(eTKD_Haisha01.KikYmd				,' '	)	AS		KikYmd,
	ISNULL(eTKD_Haisha01.KikTime			,' '	)	AS		KikTime,
	ISNULL(eTKD_Haisha01.KikEigSeq			,0		)	AS		KikEigCdSeq,
	ISNULL(eVPM_Eigyos04.EigyoCd			,0		)	AS		KikEigCd,
	ISNULL(eVPM_Eigyos04.EigyoNm			,0		)	AS		KikEigNm,
	ISNULL(eVPM_Eigyos04.RyakuNm			,' '	)	AS		KikEigRyakuNm,
	ISNULL(eTKD_Shabni01.JisaIPKm			,0		)	AS		JisaIPKm,
	ISNULL(eTKD_Shabni01.JisaKSKm			,0		)	AS		JisaKSKm,
	ISNULL(eTKD_Shabni01.KisoIPkm			,0		)	AS		KisoIPkm,
	ISNULL(eTKD_Shabni01.KisoKOKm			,0		)	AS		KisoKOKm,
	ISNULL(eTKD_Shabni01.OthKm				,0		)	AS		OthKm,
	ISNULL(eTKD_Shabni01.NipJyoSyaJin		,0		)	AS		NipJyoSyaJin,
	ISNULL(eTKD_Shabni01.NipPlusJin			,0		)	AS		NipPlusJin,
	ISNULL(eVPM_SyaRyo03.NenryoCd1Seq		,0		)	AS		NenryoCd1Seq,
	ISNULL(eVPM_CodeKb01.CodeKbn			,' '	)	AS		NenryoCd1,
	ISNULL(eVPM_CodeKb01.CodeKbnNm			,' '	)	AS		NenryoName1,
	ISNULL(eVPM_CodeKb01.RyakuNm			,' '	)	AS		NenryoRyak1,
	ISNULL(eVPM_SyaRyo03.NenryoCd2Seq		,0		)	AS		NenryoCd2Seq,
	ISNULL(eVPM_CodeKb02.CodeKbn			,' '	)	AS		NenryoCd2,
	ISNULL(eVPM_CodeKb02.CodeKbnNm			,' '	)	AS		NenryoName2,
	ISNULL(eVPM_CodeKb02.RyakuNm			,' '	)	AS		NenryoRyak2,
	ISNULL(eVPM_SyaRyo03.NenryoCd3Seq		,0		)	AS		NenryoCd3Seq,
	ISNULL(eVPM_CodeKb03.CodeKbn			,' '	)	AS		NenryoCd3,
	ISNULL(eVPM_CodeKb03.CodeKbnNm			,' '	)	AS		NenryoName3,
	ISNULL(eVPM_CodeKb03.RyakuNm			,' '	)	AS		NenryoRyak3,
	ISNULL(TKD_Yyksho.UkeEigCdSeq			,0		)	AS		UkeEigCdSeq,
	ISNULL(eVPM_Eigyos05.EigyoCd			,0		)	AS		UkeEigCd,
	ISNULL(eVPM_Eigyos05.EigyoNm			,' '  )		AS		UkeEigNm,
	ISNULL(eVPM_Eigyos05.RyakuNm			,' '	)	AS		UkeEigRyakuNm,
	ISNULL(TKD_Yyksho.EigTanCdSeq			,0		)	AS		EigTanCdSeq,
	ISNULL(eVPM_Syain01.SyainCd				,' '	)	AS		EigTanSyainCd,
	ISNULL(eVPM_Syain01.SyainNm				,' '	)	AS		EigTanSyainNm,
	ISNULL(TKD_Yyksho.InTanCdSeq			,0		)	AS		InTanCdSeq,
	ISNULL(eVPM_Syain02.SyainCd				,' '	)	AS		InputTanSyainCd,
	ISNULL(eVPM_Syain02.SyainNm				,' '	)	AS		InputTanSyainNm,
	ISNULL(eVPM_YoyKbn01.YoyaKbn			,0		)	AS		YoyaKbn,
	ISNULL(eVPM_YoyKbn01.YoyaKbnNm			,' '	)	AS		YoyaKbnNm,
	ISNULL(TKD_Yyksho.UkeYmd				,' '	)	AS		UkeYmd,
	ISNULL(eTKD_Koteik01.CntInS_Kotei		,0		)	AS		CntInS_Kotei,
	ISNULL(eTKD_Tehai01.CntInS_Tehai		,0		)	AS		CntInS_Tehai,
	ISNULL(eTKD_FutTum06.CntInS_FutTum		,0		)	AS		CntInS_Tum,
	ISNULL(eTKD_FutTum07.CntInS_FutTum		,0		)	AS		CntInS_Fut,
	ISNULL(eTKD_Biko01.CntInS_Biko			,0		)	AS		CntInS_Biko,
	ISNULL(TKD_Yyksho.NCouKbn				,0		)	AS		NCouKbn,
	ISNULL(TKD_Yyksho.SihKbn				,0		)	AS		SihKbn,
	ISNULL(TKD_Yyksho.SCouKbn				,0		)	AS		SCouKbn,
	ISNULL(eTKD_Haisha01.KSKbn				,0		)	AS		KSKbn,
	ISNULL(eTKD_Haisha01.KHinKbn			,0		)	AS		KHinKbn,
	ISNULL(eTKD_Haisha01.HaiSKbn			,0		)	AS		HaiSKbn,
	ISNULL(eTKD_Haisha01.HaiIKbn			,0		)	AS		HaiIKbn,
	ISNULL(eTKD_Haisha01.GuiWNin			,0		)	AS		GuiWNin,
	ISNULL(eTKD_Haisha01.NippoKbn			,0		)	AS		NippoKbn,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.SyaSyuCd, 0)
		ELSE ISNULL(eVPM_SyaSyu01.SyaSyuCd, 0)
	END AS SyaSyuCd,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.SyaSyuNm, 0)
		ELSE ISNULL(eVPM_SyaSyu01.SyaSyuNm, 0)
	END AS SyaSyuNm,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.KataKbn, 0)
		ELSE ISNULL(eVPM_SyaSyu01.KataKbn, 0)
	END AS KataKbn,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_CodeKb13.RyakuNm, 0)
		ELSE ISNULL(eVPM_CodeKb12.RyakuNm, 0)
	END AS KataNm,
	ISNULL(eTKD_Shabni01.Nenryo1			,0		)	AS		Nenryo1,
	ISNULL(eTKD_Shabni01.Nenryo2			,0		)	AS		Nenryo2,
	ISNULL(eTKD_Shabni01.Nenryo3			,0		)	AS		Nenryo3,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 1
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 2
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 1
	END AS YouKbn,
	ISNULL(eTKD_Haisha01.BikoTblSeq			,0		)	AS		BikoTblSeq,
	ISNULL(eTKD_Unkobi01.BikoTblSeq			,0		)	AS		BikoTblSeq_Unk,
	ISNULL(JM_UkeJyKbn.RyakuNm				,' '		)	AS		UkeJyKbn,
	ISNULL(JM_SijJoKbn1.RyakuNm				,' '		)	AS		SijJoKbn1,
	ISNULL(JM_SijJoKbn2.RyakuNm				,' '		)	AS		SijJoKbn2,
	ISNULL(JM_SijJoKbn3.RyakuNm				,' '		)	AS		SijJoKbn3,
	ISNULL(JM_SijJoKbn4.RyakuNm				,' '		)	AS		SijJoKbn4,
	ISNULL(JM_SijJoKbn5.RyakuNm				,' '		)	AS		SijJoKbn5,
	ISNULL(eVPM_CodeKb07.CodeKbn			,' ' 	)	AS		DantaiCd,
	ISNULL(eVPM_CodeKb07.CodeKbnNm			,' '	)   AS		DantaiCdNm,
	ISNULL(eVPM_JyoKya01.JyoKyakuCd			,' ' 	)	AS		JyoKyakuCd,
	ISNULL(eVPM_JyoKya01.JyoKyakuNm			,' '  )		AS		JyoKyakuNm,
	ISNULL(eVPM_HenSya01.TenkoNo			,' '  )		AS		TenkoNo
FROM eTKD_Haisha00 AS eTKD_Haisha01
JOIN eTKD_Haisha20
	ON eTKD_Haisha01.UkeNo = eTKD_Haisha20.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Haisha20.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Haisha20.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Haisha20.BunkRen
LEFT JOIN TKD_Unkobi AS	eTKD_Unkobi01 
	ON eTKD_Unkobi01.UkeNo = eTKD_Haisha01.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_Haisha01.UnkRen
	AND eTKD_Unkobi01.SiyoKbn =	1
LEFT JOIN eTKD_FutTum00	AS eTKD_FutTum01
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum01.BunkRen
	AND eTKD_FutTum01.FutTumKbn = 1
	AND eTKD_FutTum01.FutGuiKbn = 5
LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum02
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum02.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum02.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum02.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum02.BunkRen
	AND eTKD_FutTum02.FutGuiKbn = 0
LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum03
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum03.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum03.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum03.TeiDanNo
	AND eTKD_Haisha01.BunkRen =	eTKD_FutTum03.BunkRen
	AND eTKD_FutTum03.FutTumKbn = 1
	AND eTKD_FutTum03.FutGuiKbn = 2
LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum04
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum04.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum04.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum04.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum04.BunkRen
	AND eTKD_FutTum04.FutTumKbn = 1
	AND eTKD_FutTum04.FutGuiKbn = 3
LEFT JOIN eTKD_FutTum00	AS eTKD_FutTum05
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum05.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum05.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum05.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum05.BunkRen
	AND eTKD_FutTum05.FutTumKbn = 1
	AND eTKD_FutTum05.FutGuiKbn = 4
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu01
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu01.BunkRen
	AND eTKD_YFutTu01.FutTumKbn = 1
	AND eTKD_YFutTu01.SeiFutSyu = 5
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu02
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu02.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu02.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu02.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu02.BunkRen
	AND eTKD_YFutTu02.FutTumKbn = 2
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu03
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu03.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu03.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu03.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu03.BunkRen
	AND eTKD_YFutTu03.FutTumKbn = 1
	AND eTKD_YFutTu03.SeiFutSyu = 1
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu04
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu04.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu04.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu04.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu04.BunkRen
	AND eTKD_YFutTu04.FutTumKbn = 1
	AND eTKD_YFutTu04.SeiFutSyu = 2
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu05
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu05.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu05.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu05.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu05.BunkRen
	AND eTKD_YFutTu05.FutTumKbn = 1
	AND eTKD_YFutTu05.SeiFutSyu = 3
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu06
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu06.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu06.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu06.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu06.BunkRen
	AND eTKD_YFutTu06.FutTumKbn = 1
	AND eTKD_YFutTu06.SeiFutSyu = 4
LEFT JOIN
	(
		SELECT UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen,
			COUNT(UkeNo) AS CntInS_Kotei
		FROM TKD_Koteik
		WHERE SiyoKbn = 1
		GROUP BY UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen) AS eTKD_Koteik01
	ON eTKD_Haisha01.UkeNo = eTKD_Koteik01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Koteik01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Koteik01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Koteik01.BunkRen
LEFT JOIN
	(
		SELECT UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen,
			COUNT(UkeNo) AS CntInS_Tehai
		FROM TKD_Tehai
		WHERE SiyoKbn = 1
		GROUP BY UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen) AS	eTKD_Tehai01
	ON eTKD_Haisha01.UkeNo = eTKD_Tehai01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Tehai01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Tehai01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Tehai01.BunkRen
LEFT JOIN eTKD_FutTumCnt AS	eTKD_FutTum06
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum06.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum06.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum06.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum06.BunkRen
	AND eTKD_FutTum06.FutTumKbn = 2
LEFT JOIN eTKD_FutTumCnt AS	eTKD_FutTum07
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum07.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum07.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum07.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum07.BunkRen
	AND eTKD_FutTum07.FutTumKbn = 1
	LEFT JOIN TKD_YykSyu AS	eTKD_YykSyu01
	ON eTKD_Haisha01.UkeNo = eTKD_YykSyu01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YykSyu01.UnkRen
	AND eTKD_Haisha01.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen
	AND eTKD_YykSyu01.SiyoKbn = 1
LEFT JOIN
	(
		SELECT TKD_Shabni.UkeNo,
			TKD_Shabni.UnkRen,
			TKD_Shabni.TeiDanNo,
			TKD_Shabni.BunkRen,
			SUM(TKD_Shabni.JisaIPKm) AS JisaIPKm,
			SUM(TKD_Shabni.JisaKSKm) AS JisaKSKm,
			SUM(TKD_Shabni.KisoIPkm) AS KisoIPkm,
			SUM(TKD_Shabni.KisoKOKm) AS KisoKOKm,
			SUM(TKD_Shabni.OthKm) AS OthKm,
			SUM(TKD_Shabni.Nenryo1) AS Nenryo1,
			SUM(TKD_Shabni.Nenryo2) AS Nenryo2,
			SUM(TKD_Shabni.Nenryo3) AS Nenryo3,
			MAX(TKD_Shabni.JyoSyaJin) AS NipJyoSyaJin,
			MAX(TKD_Shabni.PlusJin) AS NipPlusJin
		FROM TKD_Shabni
		WHERE TKD_Shabni.SiyoKbn = 1
		GROUP BY TKD_Shabni.UkeNo,
			TKD_Shabni.UnkRen,
			TKD_Shabni.TeiDanNo,
			TKD_Shabni.BunkRen) AS eTKD_Shabni01
	ON eTKD_Haisha01.UkeNo = eTKD_Shabni01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Shabni01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Shabni01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Shabni01.BunkRen
LEFT JOIN TKD_Yousha AS eTKD_Yousha01
	ON eTKD_Haisha01.UkeNo = eTKD_Yousha01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Yousha01.UnkRen
	AND eTKD_Haisha01.YouTblSeq = eTKD_Yousha01.YouTblSeq
	AND eTKD_Yousha01.SiyoKbn = 1
LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo03
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo03.SyaRyoCdSeq
LEFT JOIN TKD_Yyksho
	ON eTKD_Haisha01.UkeNo = TKD_Yyksho.UkeNo
LEFT JOIN VPM_HenSya AS	eVPM_HenSya01
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_HenSya01.SyaRyoCdSeq
	AND eTKD_Haisha01.SyuKoYmd BETWEEN eVPM_HenSya01.StaYmd AND	eVPM_HenSya01.EndYmd
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos01
	ON eVPM_HenSya01.EigyoCdSeq = eVPM_Eigyos01.EigyoCdSeq
LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo01
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo01.SyaRyoCdSeq

LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos03
	ON eTKD_Haisha01.SyuEigCdSeq = eVPM_Eigyos03.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny02
	ON eVPM_Eigyos03.CompanyCdSeq = eVPM_Compny02.CompanyCdSeq
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos04
	ON eTKD_Haisha01.KikEigSeq = eVPM_Eigyos04.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny03
	ON eVPM_Eigyos04.CompanyCdSeq = eVPM_Compny03.CompanyCdSeq

LEFT JOIN VPM_Basyo AS eVPM_Basyo03
	ON eTKD_Haisha01.IkMapCdSeq	= eVPM_Basyo03.BasyoMapCdSeq
LEFT JOIN
	(
		SELECT UkeNo,
			BikoTblSeq,
			COUNT(UkeNo) AS CntInS_Biko
		FROM TKD_Biko
		WHERE SiyoKbn = 1
		GROUP BY UkeNo,
			BikoTblSeq ) AS eTKD_Biko01
	ON eTKD_Haisha01.UkeNo = eTKD_Biko01.UkeNo
	AND eTKD_Haisha01.BikoTblSeq = eTKD_Biko01.BikoTblSeq
LEFT JOIN VPM_Haichi AS	eVPM_Haichi01
	ON eTKD_Haisha01.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn1
	ON	eTKD_Unkobi01.SijJoKbn1 = CONVERT(tinyint, JM_SijJoKbn1.CodeKbn)
	AND JM_SijJoKbn1.CodeSyu = 'SIJJOKBN1'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn2
	ON	eTKD_Unkobi01.SijJoKbn2 = CONVERT(tinyint, JM_SijJoKbn2.CodeKbn)
	 AND JM_SijJoKbn2.CodeSyu = 'SIJJOKBN2'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn3
	ON	eTKD_Unkobi01.SijJoKbn4 = CONVERT(tinyint, JM_SijJoKbn3.CodeKbn)
	 AND JM_SijJoKbn3.CodeSyu = 'SIJJOKBN3'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn4
	ON	eTKD_Unkobi01.SijJoKbn4 = CONVERT(tinyint, JM_SijJoKbn4.CodeKbn)
	 AND JM_SijJoKbn4.CodeSyu = 'SIJJOKBN4'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn5
	ON	eTKD_Unkobi01.SijJoKbn5 = CONVERT(tinyint, JM_SijJoKbn5.CodeKbn)
	AND JM_SijJoKbn5.CodeSyu = 'SIJJOKBN5'
LEFT JOIN eVPM_CodeKb AS JM_UkeJyKbn
	ON	eTKD_Unkobi01.UkeJyKbnCd = CONVERT(tinyint, JM_UkeJyKbn.CodeKbn)
	   AND JM_UkeJyKbn.CodeSyu = 'UkeJyKbnCd'
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb01
	ON eVPM_SyaRyo03.NenryoCd1Seq = eVPM_CodeKb01.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb02
	ON eVPM_SyaRyo03.NenryoCd2Seq = eVPM_CodeKb02.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb03
	ON eVPM_SyaRyo03.NenryoCd3Seq = eVPM_CodeKb03.CodeKbnSeq
LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu01
	ON eVPM_SyaRyo03.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq
	AND eVPM_SyaSyu01.TenantCdSeq = @TenantCdSeq
LEFT JOIN eVPM_CodeKb AS	eVPM_CodeKb12
	ON eVPM_CodeKb12.CodeSyu = 'KATAKBN'
	AND CONVERT(VARCHAR(10),eVPM_SyaSyu01.KataKbn) = eVPM_CodeKb12.CodeKbn
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
	ON TKD_Yyksho.TokuiSeq = eVPM_Tokisk01.TokuiSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
	AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt01
	ON TKD_Yyksho.TokuiSeq = eVPM_TokiSt01.TokuiSeq	
	AND TKD_Yyksho.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya02
	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt02.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk04
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk04.TokuiSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk04.SiyoStaYmd AND eVPM_Tokisk04.SiyoEndYmd
	AND eVPM_Tokisk04.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt05
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt05.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt05.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt05.SiyoStaYmd AND eVPM_TokiSt05.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya03
	ON eVPM_Tokisk04.GyosyaCdSeq = eVPM_Gyosya03.GyosyaCdSeq
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos05
	ON TKD_Yyksho.UkeEigCdSeq = eVPM_Eigyos05.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny01
	ON eVPM_Eigyos05.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain01
	ON TKD_Yyksho.EigTanCdSeq = eVPM_Syain01.SyainCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain02
	ON TKD_Yyksho.InTanCdSeq = eVPM_Syain02.SyainCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain03
	ON TKD_Yyksho.CANFUTanSeq = eVPM_Syain03.SyainCdSeq
LEFT JOIN VPM_YoyKbn AS	eVPM_YoyKbn01
	ON TKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb17
	ON TKD_Yyksho.SeiKyuKbnSeq = eVPM_CodeKb17.CodeKbnSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo01
	ON eTKD_Unkobi01.HasMapCdSeq = eVPM_Basyo01.BasyoMapCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb05
	ON eVPM_Basyo01.BasyoKenCdSeq = eVPM_CodeKb05.CodeKbnSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo02
	ON eTKD_Unkobi01.AreaMapSeq	= eVPM_Basyo02.BasyoMapCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb06
	ON eVPM_Basyo02.BasyoKenCdSeq = eVPM_CodeKb06.CodeKbnSeq
LEFT JOIN VPM_JyoKya AS	eVPM_JyoKya01
	ON eTKD_Unkobi01.JyoKyakuCdSeq = eVPM_JyoKya01.JyoKyakuCdSeq
	AND eVPM_JyoKya01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb07
	ON eVPM_JyoKya01.DantaiCdSeq = eVPM_CodeKb07.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb08
	ON eVPM_Basyo03.BasyoKenCdSeq = eVPM_CodeKb08.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb09
	ON eVPM_Haichi01.BunruiCdSeq = eVPM_CodeKb09.CodeKbnSeq
LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk02
	ON eTKD_Yousha01.YouCdSeq = eVPM_Tokisk02.TokuiSeq
	AND eTKD_Yousha01.HasYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt03
	ON eTKD_Yousha01.YouCdSeq =	 eVPM_TokiSt03.TokuiSeq
	AND eTKD_Yousha01.YouSitCdSeq = eVPM_TokiSt03.SitenCdSeq
	AND eTKD_Yousha01.HasYmd BETWEEN eVPM_TokiSt03.SiyoStaYmd AND eVPM_TokiSt03.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya04
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya04.GyosyaCdSeq
LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu02
	ON eTKD_YykSyu01.SyaSyuCdSeq = eVPM_SyaSyu02.SyaSyuCdSeq
	   AND eVPM_SyaSyu02.TenantCdSeq  = @TenantCdSeq
LEFT JOIN eVPM_CodeKb AS	eVPM_CodeKb13
	ON eVPM_CodeKb13.CodeSyu = 'KATAKBN'
	AND	CONVERT(VARCHAR(10),eVPM_SyaSyu02.KataKbn) = eVPM_CodeKb13.CodeKbn
WHERE
	TKD_Yyksho.SiyoKbn = 1
	AND TKD_Yyksho.YoyaSyu = 1	
	AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
	AND (@StartDispatchDate IS NULL OR eTKD_Haisha01.HaiSYmd >= @StartDispatchDate)																								-- 配車日　開始
	AND (@EndDispatchDate IS NULL OR eTKD_Haisha01.HaiSYmd <= @EndDispatchDate)																									-- 配車日　終了
	AND (@StartArrivalDate IS NULL OR eTKD_Haisha01.TouYmd >= @StartArrivalDate)																								-- 到着日　開始
	AND (@EndArrivalDate IS NULL OR eTKD_Haisha01.TouYmd <= @EndArrivalDate)																									-- 到着日　終了
	AND (@StartReservationDate IS NULL OR TKD_Yyksho.UkeYmd >= @StartReservationDate)																							-- 予約日　開始
	AND (@EndReservationDate IS NULL OR TKD_Yyksho.UkeYmd<= @EndReservationDate)																								-- 予約日　終了
	AND (@StartReceiptNumber IS NULL OR TKD_Yyksho.UkeNo >= @StartReceiptNumber)																								-- 予約番号　開始
	AND	(@EndReceiptNumber IS NULL OR TKD_Yyksho.UkeNo <= @EndReceiptNumber)																									-- 予約番号　終了
	AND (@StartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)																	-- 予約区分　開始	
	AND	(@EndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)																		-- 予約区分　終了
	AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson)																							-- 営業担当者　開始	
	AND	(@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson)																								-- 営業担当者　終了
	AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos05.EigyoCd >= @StartRegistrationOffice)																					-- 営業所　開始
	AND	(@EndRegistrationOffice IS NULL OR eVPM_Eigyos05.EigyoCd <= @EndRegistrationOffice)																						-- 営業所　終了
	AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson)																								-- 入力担当者　開始
	AND	(@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson)																									-- 入力担当者　終了
	AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') + FORMAT(eVPM_TokiSt01.SitenCd,'0000') >= @StartCustomer)		-- 得意先コード　開始
	AND	(@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') + FORMAT(eVPM_TokiSt01.SitenCd,'0000') <= @EndCustomer)			-- 得意先コード　終了
	AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') + FORMAT(eVPM_Tokisk04.TokuiCd,'0000') + FORMAT(eVPM_TokiSt05.SitenCd,'0000') >= @StartSupplier)		-- 仕入先コード　開始
	AND	(@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') +	FORMAT(eVPM_Tokisk04.TokuiCd,'0000') + FORMAT(eVPM_TokiSt05.SitenCd,'0000') <= @EndSupplier)			-- 仕入先コード　終了
	AND (@StartGroupClassification IS NULL OR eVPM_CodeKb17.CodeKbn >= @StartGroupClassification)																				-- 団体区分　開始
	AND	(@EndGroupClassification IS NULL OR eVPM_CodeKb17.CodeKbn <= @EndGroupClassification)																					-- 団体区分　終了
	AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification)																-- 乗客コード　開始
	AND	(@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification)																	-- 乗客コード　終了
	AND (@StartDestination IS NULL OR eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartDestination)																		-- 行先　開始
	AND	(@EndDestination IS NULL OR eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndDestination)																			-- 行先　終了
	AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace)																	-- 配車地　開始
	AND	(@EndDispatchPlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace)																		-- 配車地　終了
	AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartOccurrencePlace)																-- 発生地　開始
	AND	(@EndOccurrencePlace IS NULL OR eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndOccurrencePlace)																	-- 発生地　終了
	AND (@StartArea IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartArea)																					-- エリア　開始
	AND	(@EndArea IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndArea)																						-- エリア　終了
	AND (@StartReceiptCondition IS NULL OR eTKD_Haisha01.UkeJyKbnCd >= @StartReceiptCondition)																					-- 受付条件　開始
	AND (@EndReceiptCondition IS NULL OR eTKD_Haisha01.UkeJyKbnCd <= @EndReceiptCondition)																						-- 受付条件　終了
	AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice)																							-- 車種単価 開始
	AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice)																								-- 車種単価 終了
	AND (@DantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @DantaNm, '%'))
ORDER BY UkeNo, UnkRen
OFFSET @OffSet ROWS FETCH NEXT @Fetch ROWS ONLY

OPTION (RECOMPILE)
END
ELSE
BEGIN
WITH eVPM_CodeKb AS (
				SELECT VPM_CodeKb.CodeKbnSeq,
					VPM_CodeKb.CodeKbn,
					VPM_CodeKb.CodeSyu,
					VPM_CodeKb.CodeKbnNm,
					VPM_CodeKb.RyakuNm
				FROM VPM_CodeKb
				LEFT JOIN VPM_CodeSy
					ON VPM_CodeKb.CodeSyu = VPM_CodeSy.CodeSyu
				WHERE VPM_CodeKb.CodeSyu IN ('SIJJOKBN1', 'SIJJOKBN2', 'SIJJOKBN3', 'SIJJOKBN4', 'SIJJOKBN5', 'UkeJyKbnCd', 'KATAKBN')
					AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn <> 1 AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq))
					AND VPM_CodeKb.SiyoKbn = 1
			),
eTKD_FutTum00 AS (
	SELECT TKD_FutTum.UkeNo								AS		UkeNo,
		TKD_FutTum.UnkRen								AS		UnkRen,
		TKD_MFutTu.TeiDanNo								AS		TeiDanNo,
		TKD_MFutTu.BunkRen								AS		BunkRen,
		TKD_FutTum.FutTumKbn							AS		FutTumKbn,
		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0
			ELSE VPM_Futai.FutGuiKbn
			END AS FutGuiKbn,
		SUM(CAST(TKD_MFutTu.UriGakKin AS bigint))						AS		UriGakKin_S,
		SUM(CAST(TKD_MFutTu.SyaRyoSyo as bigint))						AS		SyaRyoSyo_S,
		SUM(CAST(TKD_MFutTu.SyaRyoTes as bigint))						AS		SyaRyoTes_S
	FROM TKD_FutTum
	LEFT JOIN VPM_Futai
		ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq
		AND VPM_Futai.SiyoKbn = 1
	JOIN TKD_MFutTu
		ON TKD_FutTum.UkeNo = TKD_MFutTu.UkeNo
		AND TKD_FutTum.UnkRen = TKD_MFutTu.UnkRen
		AND TKD_FutTum.FutTumKbn = TKD_MFutTu.FutTumKbn
		AND TKD_FutTum.FutTumRen = TKD_MFutTu.FutTumRen
		AND TKD_MFutTu.SiyoKbn = 1
	GROUP BY
		TKD_FutTum.UkeNo,
		TKD_FutTum.UnkRen,
		TKD_MFutTu.TeiDanNo,
		TKD_MFutTu.BunkRen,
		TKD_FutTum.FutTumKbn,
		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0
			ELSE VPM_Futai.FutGuiKbn
			END
),
eTKD_YFutTu10 AS (
	SELECT TKD_YMFuTu.*,
		VPM_Futai.FutGuiKbn								AS		SeiFutSyu,
		ISNULL(TKD_Yousha.JitaFlg,1)					AS		JitaFlg
	FROM TKD_YMFuTu
	LEFT JOIN TKD_YFutTu
		ON TKD_YMFuTu.UkeNo = TKD_YFutTu.UkeNo
		AND TKD_YMFuTu.UnkRen = TKD_YFutTu.UnkRen
		AND TKD_YMFuTu.YouTblSeq = TKD_YFutTu.YouTblSeq
		AND TKD_YMFuTu.FutTumKbn = TKD_YFutTu.FutTumKbn
		AND TKD_YMFuTu.YouFutTumRen = TKD_YFutTu.YouFutTumRen
	LEFT JOIN VPM_Futai
		ON TKD_YFutTu.FutTumCdSeq = VPM_Futai.FutaiCdSeq
	LEFT JOIN TKD_Yousha
		ON TKD_YMFuTu.UkeNo = TKD_Yousha.UkeNo
		AND TKD_YMFuTu.UnkRen = TKD_Yousha.UnkRen
		AND TKD_YMFuTu.YouTblSeq = TKD_Yousha.YouTblSeq
		AND TKD_Yousha.SiyoKbn = 1
	WHERE TKD_YMFuTu.SiyoKbn = 1
),
eTKD_YFutTu00 AS (
	SELECT eTKD_YFutTu10.UkeNo							AS		UkeNo,
		eTKD_YFutTu10.UnkRen							AS		UnkRen,
		eTKD_YFutTu10.FutTumKbn							AS		FutTumKbn,
		eTKD_YFutTu10.SeiFutSyu							AS		SeiFutSyu,
		eTKD_YFutTu10.TeiDanNo							AS		TeiDanNo,
		eTKD_YFutTu10.BunkRen							AS		BunkRen,
		SUM(CAST(eTKD_YFutTu10.HaseiKin as bigint)	)					AS		HaseiKin_S,
		SUM(CAST(eTKD_YFutTu10.SyaRyoSyo as bigint)	)					AS		SyaRyoSyo_S,
		SUM(CAST(eTKD_YFutTu10.SyaRyoTes as bigint)	)					AS		SyaRyoTes_S
	FROM eTKD_YFutTu10
	WHERE eTKD_YFutTu10.JitaFlg	= 0
	GROUP BY eTKD_YFutTu10.UkeNo,
		eTKD_YFutTu10.UnkRen,
		eTKD_YFutTu10.FutTumKbn,
		eTKD_YFutTu10.SeiFutSyu,
		eTKD_YFutTu10.TeiDanNo,
		eTKD_YFutTu10.BunkRen
),
eTKD_FutTumCnt AS (
	SELECT TKD_MFutTu.UkeNo AS UkeNo,
		TKD_MFutTu.UnkRen AS UnkRen,
		TKD_MFutTu.TeiDanNo AS TeiDanNo,
		TKD_MFutTu.BunkRen AS BunkRen,
		TKD_MFutTu.FutTumKbn AS FutTumKbn,
		COUNT(TKD_MFutTu.UkeNo) AS CntInS_FutTum
	FROM TKD_MFutTu
	WHERE TKD_MFutTu.SiyoKbn = 1
		AND TKD_MFutTu.Suryo <> 0
	GROUP BY TKD_MFutTu.UkeNo,
		TKD_MFutTu.UnkRen,
		TKD_MFutTu.TeiDanNo,
		TKD_MFutTu.BunkRen,
		TKD_MFutTu.FutTumKbn
),
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
),
eTKD_Haisha20 AS (
	SELECT	DISTINCT
		eTKD_Haisha10.UkeNo								AS		UkeNo,
		eTKD_Haisha10.UnkRen							AS		UnkRen,
		eTKD_Haisha10.TeiDanNo							AS		TeiDanNo,
		eTKD_Haisha10.BunkRen							AS		BunkRen
	FROM eTKD_Haisha10
	GROUP BY eTKD_Haisha10.UkeNo,
		eTKD_Haisha10.UnkRen,
		eTKD_Haisha10.TeiDanNo,
		eTKD_Haisha10.BunkRen
)

	SELECT ISNULL(TKD_Yyksho.UkeNo				,0		)	AS		UkeNo,
	ISNULL(eTKD_Unkobi01.UnkRen				,0		)	AS		UnkRen,
	ISNULL(eTKD_Haisha01.TeiDanNo			,0		)	AS		TeiDanNo,
	ISNULL(eTKD_Haisha01.BunkRen			,0		)	AS		BunkRen,
	ISNULL(TKD_Yyksho.KaknKais				,0		)	AS		KaknKais,
	ISNULL(TKD_Yyksho.KaktYmd				,' '	)	AS		KaktYmd,
	ISNULL(TKD_Yyksho.NyuKinKbn				,0		)	AS		NyuKinKbn,
	ISNULL(TKD_Yyksho.TokuiSeq				,0		)	AS		TokuiSeq,
	ISNULL(eVPM_Tokisk01.TokuiCd			,0		)	AS		TokuiCd,
	ISNULL(TKD_Yyksho.CanFuYmd				,' ')		AS		CanFuYmd,
	ISNULL(TKD_Yyksho.CanFuRiy				,' ')		AS		CanFuRiy,
	ISNULL(eVPM_Syain03.SyainCd				,0)			AS		CanFutanCd,
	ISNULL(eVPM_Syain03.SyainNm				,' ')		AS		CanFutanNm,
	ISNULL(eVPM_Gyosya02.GyosyaCd			,' '	)	AS		GyosyaCd,
	ISNULL(eVPM_Gyosya02.GyosyaNm			,' '	)	AS		GyosyaNm,
	ISNULL(eVPM_Tokisk01.TokuiNm			,' '	)	AS		TokuiNm,
	ISNULL(eVPM_Tokisk01.RyakuNm			,' '	)	AS		TokuiRyakuNm,
	ISNULL(TKD_Yyksho.SitenCdSeq			,0		)	AS		SitenCdSeq,
	ISNULL(eVPM_TokiSt01.SitenCd			,0		)	AS		SitenCd,
	ISNULL(eVPM_TokiSt01.SitenNm			,' '	)	AS		SitenNm,
	ISNULL(eVPM_TokiSt01.RyakuNm			,' '	)	AS		SitenRyakuNm,
	ISNULL(eVPM_TokiSt02.TesKbn				,0		)	AS		SeiSitTesKbn,
	ISNULL(eVPM_TokiSt02.TesKbnFut			,0		)	AS		SeiSitTesKbnFut,
	ISNULL(eVPM_TokiSt02.TesKbnGui			,0		)	AS		SeiSitTesKbnGui,
	ISNULL(TKD_Yyksho.TokuiTanNm			,' '	)	AS		TokuiTanNm,
	ISNULL(eTKD_Unkobi01.DanTaNm			,' '	)	AS		DanTaNm,
	ISNULL(eTKD_Haisha01.DanTaNm2			,' '	)	AS		DanTaNm2,
	ISNULL(eTKD_Haisha01.IkNm				,' '	)	AS		IkNm,
	ISNULL(eTKD_Haisha01.HaiSYmd			,' '	)	AS		HaiSYmd,
	ISNULL(eTKD_Haisha01.HaiSTime			,' '	)	AS		HaiSTime,
	ISNULL(eTKD_Haisha01.HaiSNm				,' '	)	AS		HaiSNm,
	ISNULL(eTKD_Haisha01.HaiSJyuS1			,' '	)	AS		HaiSJyuS1,
	ISNULL(eTKD_Haisha01.HaiSJyuS2			,' '	)	AS		HaiSJyuS2,
	ISNULL(eTKD_Haisha01.TouYmd				,' '	)	AS		TouYmd,
	ISNULL(eTKD_Haisha01.TouChTime			,' '	)	AS		TouChTime,
	ISNULL(eTKD_Haisha01.TouNm				,' '	)	AS		TouNm,
	ISNULL(eTKD_Haisha01.TouJyusyo1			,' '	)	AS		TouJyusyo1,
	ISNULL(eTKD_Haisha01.TouJyusyo2			,' '	)	AS		TouJyusyo2,
	ISNULL(eTKD_Haisha01.DrvJin				,0		)	AS		DrvJin,
	ISNULL(eTKD_Haisha01.GuiSu				,0		)	AS		GuiSu,
	ISNULL(eTKD_Haisha01.SyuPaTime			,' '	)	AS		SyuPaTime,
	ISNULL(eTKD_Haisha01.HsKsSryCdSeq		,0		)	AS		SyaryoCdSeq,
	ISNULL(eVPM_Eigyos01.EigyoCd			,0		)	AS		SyaryoEigCd,
	ISNULL(eVPM_Eigyos01.EigyoNm			,' '	)	AS		SyaryoEigyoNm,
	ISNULL(eVPM_Eigyos01.RyakuNm			,' '	)	AS		SyaryoEigRyakuNm,
	CASE WHEN eTKD_Haisha01.HaiSKbn = 2 THEN ISNULL(eVPM_SyaRyo01.SyaRyoNm ,' '	)
		WHEN eTKD_Haisha01.HaiSKbn <> 2 AND eTKD_Haisha01.KSKbn =	2 THEN ISNULL(eVPM_SyaRyo01.KariSyaRyoNm, ' ')
		ELSE ' '	
		END	AS SyaRyoNm,
	ISNULL(eVPM_SyaRyo01.SyaRyoCd			,' '	)	AS		SyaRyoCd,
	ISNULL(eTKD_Haisha01.GoSya				,' '	)	AS		GoSya,
	ISNULL(eTKD_Yousha01.YouCdSeq			,0		)	AS		YouCdSeq,
	ISNULL(eVPM_Tokisk02.TokuiCd			,0		)	AS		YouCd,
	ISNULL(eVPM_Gyosya04.GyosyaCd			,0		)	AS		YouGyoCd,
	ISNULL(eVPM_Gyosya04.GyosyaNm			,' '		)	AS		YouGyoNm,
	ISNULL(eVPM_Tokisk02.TokuiNm			,' '		)	AS		YouNm,
	ISNULL(eVPM_Tokisk02.RyakuNm			,' '	)	AS		YouRyakuNm,
	ISNULL(eTKD_Yousha01.YouSitCdSeq		,0		)	AS		YouSitCdSeq,
	ISNULL(eVPM_TokiSt03.SitenCd			,0		)	AS		YouSitCd,
	ISNULL(eVPM_TokiSt03.SitenNm			,' '		)	AS		YouSitNm,
	ISNULL(eVPM_TokiSt03.RyakuNm			,' '	)	AS		YouSitCdRyakuNm,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.SyaRyoUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaUnc
	END AS SyaRyoUnc,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.SyaRyoSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaSyo
	END AS SyaRyoSyo,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.SyaRyoTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaTes
	END AS SyaRyoTes,

	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaUnc
	END AS JiSyaRyoUnc,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaSyo
	END AS JiSyaRyoSyo,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN eTKD_Haisha01.SyaRyoTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN eTKD_Haisha01.YoushaTes
	END AS JiSyaRyoTes,

	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.YoushaUnc
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YoushaUnc,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.YoushaSyo
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YoushaSyo,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN eTKD_Haisha01.YoushaTes
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YoushaTes,
	ISNULL(eTKD_FutTum01.SyaRyoSyo_S		,0		)	AS		Gui_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum01.SyaRyoTes_S		,0		)	AS		Gui_SyaRyoTes_S,
	ISNULL(eTKD_FutTum01.UriGakKin_S		,0		)	AS		Gui_UriGakKin_S,
	ISNULL(eTKD_FutTum02.UriGakKin_S		,0		)	AS		Tum_UriGakKin_S,
	ISNULL(eTKD_FutTum02.SyaRyoSyo_S		,0		)	AS		Tum_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum02.SyaRyoTes_S		,0		)	AS		Tum_SyaRyoTes_S,
	ISNULL(eTKD_FutTum03.UriGakKin_S		,0		)	AS		Fut_UriGakKin_S,
	ISNULL(eTKD_FutTum03.SyaRyoSyo_S		,0		)	AS		Fut_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum03.SyaRyoTes_S		,0		)	AS		Fut_SyaRyoTes_S,
	ISNULL(eTKD_FutTum04.UriGakKin_S		,0		)	AS		Tuk_UriGakKin_S,
	ISNULL(eTKD_FutTum04.SyaRyoSyo_S		,0		)	AS		Tuk_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum04.SyaRyoTes_S		,0		)	AS		Tuk_SyaRyoTes_S,
	ISNULL(eTKD_FutTum05.UriGakKin_S		,0		)	AS		Teh_UriGakKin_S,
	ISNULL(eTKD_FutTum05.SyaRyoSyo_S		,0		)	AS		Teh_SyaRyoSyo_S,
	ISNULL(eTKD_FutTum05.SyaRyoTes_S		,0		)	AS		Teh_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu01.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YGui_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu01.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YGui_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu01.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS YGui_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu02.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytum_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu02.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytum_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu02.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytum_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu04.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yfut_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu04.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yfut_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu04.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yfut_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu05.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytuk_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu05.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytuk_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu05.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Ytuk_SyaRyoTes_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu06.HaseiKin_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yteh_HaseiKin_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu06.SyaRyoSyo_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yteh_SyaRyoSyo_S,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(eTKD_YFutTu06.SyaRyoTes_S, 0)
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0
	END AS Yteh_SyaRyoTes_S,
	ISNULL(eTKD_Haisha01.JyoSyaJin			,0		)	AS		JyoSyaJin,
	ISNULL(eTKD_Haisha01.PlusJin			,0		)	AS		PlusJin,
	ISNULL(eTKD_Haisha01.SyuKoYmd			,' '	)	AS		SyuKoYmd,
	ISNULL(eTKD_Haisha01.SyuKoTime			,' '	)	AS		SyuKoTime,
	ISNULL(eTKD_Haisha01.SyuEigCdSeq		,0		)	AS		SyuEigCdSeq,
	ISNULL(eVPM_Eigyos03.EigyoCd			,0		)	AS		SyuEigCd,
	ISNULL(eVPM_Eigyos03.EigyoNm			,' '		)	AS		SyuEigNm,
	ISNULL(eVPM_Eigyos03.RyakuNm			,' '	)	AS		SyuEigRyakuNm,
	ISNULL(eTKD_Haisha01.KikYmd				,' '	)	AS		KikYmd,
	ISNULL(eTKD_Haisha01.KikTime			,' '	)	AS		KikTime,
	ISNULL(eTKD_Haisha01.KikEigSeq			,0		)	AS		KikEigCdSeq,
	ISNULL(eVPM_Eigyos04.EigyoCd			,0		)	AS		KikEigCd,
	ISNULL(eVPM_Eigyos04.EigyoNm			,' '		)	AS		KikEigNm,
	ISNULL(eVPM_Eigyos04.RyakuNm			,' '	)	AS		KikEigRyakuNm,
	ISNULL(eTKD_Shabni01.JisaIPKm			,0		)	AS		JisaIPKm,
	ISNULL(eTKD_Shabni01.JisaKSKm			,0		)	AS		JisaKSKm,
	ISNULL(eTKD_Shabni01.KisoIPkm			,0		)	AS		KisoIPkm,
	ISNULL(eTKD_Shabni01.KisoKOKm			,0		)	AS		KisoKOKm,
	ISNULL(eTKD_Shabni01.OthKm				,0		)	AS		OthKm,
	ISNULL(eTKD_Shabni01.NipJyoSyaJin		,0		)	AS		NipJyoSyaJin,
	ISNULL(eTKD_Shabni01.NipPlusJin			,0		)	AS		NipPlusJin,
	ISNULL(eVPM_SyaRyo03.NenryoCd1Seq		,0		)	AS		NenryoCd1Seq,
	ISNULL(eVPM_CodeKb01.CodeKbn			,' '	)	AS		NenryoCd1,
	ISNULL(eVPM_CodeKb01.CodeKbnNm			,' '	)	AS		NenryoName1,
	ISNULL(eVPM_CodeKb01.RyakuNm			,' '	)	AS		NenryoRyak1,
	ISNULL(eVPM_SyaRyo03.NenryoCd2Seq		,0		)	AS		NenryoCd2Seq,
	ISNULL(eVPM_CodeKb02.CodeKbn			,' '	)	AS		NenryoCd2,
	ISNULL(eVPM_CodeKb02.CodeKbnNm			,' '	)	AS		NenryoName2,
	ISNULL(eVPM_CodeKb02.RyakuNm			,' '	)	AS		NenryoRyak2,
	ISNULL(eVPM_SyaRyo03.NenryoCd3Seq		,0		)	AS		NenryoCd3Seq,
	ISNULL(eVPM_CodeKb03.CodeKbn			,' '	)	AS		NenryoCd3,
	ISNULL(eVPM_CodeKb03.CodeKbnNm			,' '	)	AS		NenryoName3,
	ISNULL(eVPM_CodeKb03.RyakuNm			,' '	)	AS		NenryoRyak3,
	ISNULL(TKD_Yyksho.UkeEigCdSeq			,0		)	AS		UkeEigCdSeq,
	ISNULL(eVPM_Eigyos05.EigyoCd			,0		)	AS		UkeEigCd,
	ISNULL(eVPM_Eigyos05.EigyoNm			,' '  )		AS		UkeEigNm,
	ISNULL(eVPM_Eigyos05.RyakuNm			,' '	)	AS		UkeEigRyakuNm,
	ISNULL(TKD_Yyksho.EigTanCdSeq			,0		)	AS		EigTanCdSeq,
	ISNULL(eVPM_Syain01.SyainCd				,' '	)	AS		EigTanSyainCd,
	ISNULL(eVPM_Syain01.SyainNm				,' '	)	AS		EigTanSyainNm,
	ISNULL(TKD_Yyksho.InTanCdSeq			,0		)	AS		InTanCdSeq,
	ISNULL(eVPM_Syain02.SyainCd				,' '	)	AS		InputTanSyainCd,
	ISNULL(eVPM_Syain02.SyainNm				,' '	)	AS		InputTanSyainNm,
	ISNULL(eVPM_YoyKbn01.YoyaKbn			,0		)	AS		YoyaKbn,
	ISNULL(eVPM_YoyKbn01.YoyaKbnNm			,' '	)	AS		YoyaKbnNm,
	ISNULL(TKD_Yyksho.UkeYmd				,' '	)	AS		UkeYmd,
	ISNULL(eTKD_Koteik01.CntInS_Kotei		,0		)	AS		CntInS_Kotei,
	ISNULL(eTKD_Tehai01.CntInS_Tehai		,0		)	AS		CntInS_Tehai,
	ISNULL(eTKD_FutTum06.CntInS_FutTum		,0		)	AS		CntInS_Tum,
	ISNULL(eTKD_FutTum07.CntInS_FutTum		,0		)	AS		CntInS_Fut,
	ISNULL(eTKD_Biko01.CntInS_Biko			,0		)	AS		CntInS_Biko,
	ISNULL(TKD_Yyksho.NCouKbn				,0		)	AS		NCouKbn,
	ISNULL(TKD_Yyksho.SihKbn				,0		)	AS		SihKbn,
	ISNULL(TKD_Yyksho.SCouKbn				,0		)	AS		SCouKbn,
	ISNULL(eTKD_Haisha01.KSKbn				,0		)	AS		KSKbn,
	ISNULL(eTKD_Haisha01.KHinKbn			,0		)	AS		KHinKbn,
	ISNULL(eTKD_Haisha01.HaiSKbn			,0		)	AS		HaiSKbn,
	ISNULL(eTKD_Haisha01.HaiIKbn			,0		)	AS		HaiIKbn,
	ISNULL(eTKD_Haisha01.GuiWNin			,0		)	AS		GuiWNin,
	ISNULL(eTKD_Haisha01.NippoKbn			,0		)	AS		NippoKbn,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.SyaSyuCd, 0)
		ELSE ISNULL(eVPM_SyaSyu01.SyaSyuCd, 0)
	END AS SyaSyuCd,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.SyaSyuNm, ' ')
		ELSE ISNULL(eVPM_SyaSyu01.SyaSyuNm, ' ')
	END AS SyaSyuNm,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.KataKbn, 0)
		ELSE ISNULL(eVPM_SyaSyu01.KataKbn, 0)
	END AS KataKbn,
	CASE
		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_CodeKb13.RyakuNm, ' ')
		ELSE ISNULL(eVPM_CodeKb12.RyakuNm, ' ')
	END AS KataNm,
	ISNULL(eTKD_Shabni01.Nenryo1			,0		)	AS		Nenryo1,
	ISNULL(eTKD_Shabni01.Nenryo2			,0		)	AS		Nenryo2,
	ISNULL(eTKD_Shabni01.Nenryo3			,0		)	AS		Nenryo3,
	CASE
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 1
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 2
		WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 1
	END AS YouKbn,
	ISNULL(eTKD_Haisha01.BikoTblSeq			,0		)	AS		BikoTblSeq,
	ISNULL(eTKD_Unkobi01.BikoTblSeq			,0		)	AS		BikoTblSeq_Unk,
	ISNULL(JM_UkeJyKbn.RyakuNm				,' '		)	AS		UkeJyKbn,
	ISNULL(JM_SijJoKbn1.RyakuNm				,' '		)	AS		SijJoKbn1,
	ISNULL(JM_SijJoKbn2.RyakuNm				,' '		)	AS		SijJoKbn2,
	ISNULL(JM_SijJoKbn3.RyakuNm				,' '		)	AS		SijJoKbn3,
	ISNULL(JM_SijJoKbn4.RyakuNm				,' '		)	AS		SijJoKbn4,
	ISNULL(JM_SijJoKbn5.RyakuNm				,' '		)	AS		SijJoKbn5,
	ISNULL(eVPM_CodeKb07.CodeKbn			,' ' 	)	AS		DantaiCd,
	ISNULL(eVPM_CodeKb07.CodeKbnNm			,' '	)   AS		DantaiCdNm,
	ISNULL(eVPM_JyoKya01.JyoKyakuCd			,' ' 	)	AS		JyoKyakuCd,
	ISNULL(eVPM_JyoKya01.JyoKyakuNm			,' '  )		AS		JyoKyakuNm,
	ISNULL(eVPM_HenSya01.TenkoNo			,' '  )		AS		TenkoNo
FROM eTKD_Haisha00 AS eTKD_Haisha01
JOIN eTKD_Haisha20
	ON eTKD_Haisha01.UkeNo = eTKD_Haisha20.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Haisha20.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Haisha20.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Haisha20.BunkRen
LEFT JOIN TKD_Unkobi AS	eTKD_Unkobi01 
	ON eTKD_Unkobi01.UkeNo = eTKD_Haisha01.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_Haisha01.UnkRen
	AND eTKD_Unkobi01.SiyoKbn =	1
LEFT JOIN eTKD_FutTum00	AS eTKD_FutTum01
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum01.BunkRen
	AND eTKD_FutTum01.FutTumKbn = 1
	AND eTKD_FutTum01.FutGuiKbn = 5
LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum02
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum02.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum02.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum02.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum02.BunkRen
	AND eTKD_FutTum02.FutGuiKbn = 0
LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum03
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum03.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum03.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum03.TeiDanNo
	AND eTKD_Haisha01.BunkRen =	eTKD_FutTum03.BunkRen
	AND eTKD_FutTum03.FutTumKbn = 1
	AND eTKD_FutTum03.FutGuiKbn = 2
LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum04
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum04.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum04.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum04.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum04.BunkRen
	AND eTKD_FutTum04.FutTumKbn = 1
	AND eTKD_FutTum04.FutGuiKbn = 3
LEFT JOIN eTKD_FutTum00	AS eTKD_FutTum05
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum05.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum05.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum05.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum05.BunkRen
	AND eTKD_FutTum05.FutTumKbn = 1
	AND eTKD_FutTum05.FutGuiKbn = 4
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu01
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu01.BunkRen
	AND eTKD_YFutTu01.FutTumKbn = 1
	AND eTKD_YFutTu01.SeiFutSyu = 5
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu02
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu02.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu02.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu02.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu02.BunkRen
	AND eTKD_YFutTu02.FutTumKbn = 2
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu03
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu03.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu03.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu03.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu03.BunkRen
	AND eTKD_YFutTu03.FutTumKbn = 1
	AND eTKD_YFutTu03.SeiFutSyu = 1
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu04
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu04.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu04.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu04.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu04.BunkRen
	AND eTKD_YFutTu04.FutTumKbn = 1
	AND eTKD_YFutTu04.SeiFutSyu = 2
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu05
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu05.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu05.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu05.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu05.BunkRen
	AND eTKD_YFutTu05.FutTumKbn = 1
	AND eTKD_YFutTu05.SeiFutSyu = 3
LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu06
	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu06.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu06.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu06.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu06.BunkRen
	AND eTKD_YFutTu06.FutTumKbn = 1
	AND eTKD_YFutTu06.SeiFutSyu = 4
LEFT JOIN
	(
		SELECT UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen,
			COUNT(UkeNo) AS CntInS_Kotei
		FROM TKD_Koteik
		WHERE SiyoKbn = 1
		GROUP BY UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen) AS eTKD_Koteik01
	ON eTKD_Haisha01.UkeNo = eTKD_Koteik01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Koteik01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Koteik01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Koteik01.BunkRen
LEFT JOIN
	(
		SELECT UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen,
			COUNT(UkeNo) AS CntInS_Tehai
		FROM TKD_Tehai
		WHERE SiyoKbn = 1
		GROUP BY UkeNo,
			UnkRen,
			TeiDanNo,
			BunkRen) AS	eTKD_Tehai01
	ON eTKD_Haisha01.UkeNo = eTKD_Tehai01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Tehai01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Tehai01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Tehai01.BunkRen
LEFT JOIN eTKD_FutTumCnt AS	eTKD_FutTum06
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum06.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum06.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum06.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum06.BunkRen
	AND eTKD_FutTum06.FutTumKbn = 2
LEFT JOIN eTKD_FutTumCnt AS	eTKD_FutTum07
	ON eTKD_Haisha01.UkeNo = eTKD_FutTum07.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_FutTum07.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum07.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_FutTum07.BunkRen
	AND eTKD_FutTum07.FutTumKbn = 1
	LEFT JOIN TKD_YykSyu AS	eTKD_YykSyu01
	ON eTKD_Haisha01.UkeNo = eTKD_YykSyu01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YykSyu01.UnkRen
	AND eTKD_Haisha01.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen
	AND eTKD_YykSyu01.SiyoKbn = 1
LEFT JOIN
	(
		SELECT TKD_Shabni.UkeNo,
			TKD_Shabni.UnkRen,
			TKD_Shabni.TeiDanNo,
			TKD_Shabni.BunkRen,
			SUM(TKD_Shabni.JisaIPKm) AS JisaIPKm,
			SUM(TKD_Shabni.JisaKSKm) AS JisaKSKm,
			SUM(TKD_Shabni.KisoIPkm) AS KisoIPkm,
			SUM(TKD_Shabni.KisoKOKm) AS KisoKOKm,
			SUM(TKD_Shabni.OthKm) AS OthKm,
			SUM(TKD_Shabni.Nenryo1) AS Nenryo1,
			SUM(TKD_Shabni.Nenryo2) AS Nenryo2,
			SUM(TKD_Shabni.Nenryo3) AS Nenryo3,
			MAX(TKD_Shabni.JyoSyaJin) AS NipJyoSyaJin,
			MAX(TKD_Shabni.PlusJin) AS NipPlusJin
		FROM TKD_Shabni
		WHERE TKD_Shabni.SiyoKbn = 1
		GROUP BY TKD_Shabni.UkeNo,
			TKD_Shabni.UnkRen,
			TKD_Shabni.TeiDanNo,
			TKD_Shabni.BunkRen) AS eTKD_Shabni01
	ON eTKD_Haisha01.UkeNo = eTKD_Shabni01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Shabni01.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Shabni01.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Shabni01.BunkRen
LEFT JOIN TKD_Yousha AS eTKD_Yousha01
	ON eTKD_Haisha01.UkeNo = eTKD_Yousha01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Yousha01.UnkRen
	AND eTKD_Haisha01.YouTblSeq = eTKD_Yousha01.YouTblSeq
	AND eTKD_Yousha01.SiyoKbn = 1
LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo03
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo03.SyaRyoCdSeq
LEFT JOIN TKD_Yyksho
	ON eTKD_Haisha01.UkeNo = TKD_Yyksho.UkeNo
LEFT JOIN VPM_HenSya AS	eVPM_HenSya01
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_HenSya01.SyaRyoCdSeq
	AND eTKD_Haisha01.SyuKoYmd BETWEEN eVPM_HenSya01.StaYmd AND	eVPM_HenSya01.EndYmd
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos01
	ON eVPM_HenSya01.EigyoCdSeq = eVPM_Eigyos01.EigyoCdSeq
LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo01
	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo01.SyaRyoCdSeq

LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos03
	ON eTKD_Haisha01.SyuEigCdSeq = eVPM_Eigyos03.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny02
	ON eVPM_Eigyos03.CompanyCdSeq = eVPM_Compny02.CompanyCdSeq
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos04
	ON eTKD_Haisha01.KikEigSeq = eVPM_Eigyos04.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny03
	ON eVPM_Eigyos04.CompanyCdSeq = eVPM_Compny03.CompanyCdSeq

LEFT JOIN VPM_Basyo AS eVPM_Basyo03
	ON eTKD_Haisha01.IkMapCdSeq	= eVPM_Basyo03.BasyoMapCdSeq
LEFT JOIN
	(
		SELECT UkeNo,
			BikoTblSeq,
			COUNT(UkeNo) AS CntInS_Biko
		FROM TKD_Biko
		WHERE SiyoKbn = 1
		GROUP BY UkeNo,
			BikoTblSeq ) AS eTKD_Biko01
	ON eTKD_Haisha01.UkeNo = eTKD_Biko01.UkeNo
	AND eTKD_Haisha01.BikoTblSeq = eTKD_Biko01.BikoTblSeq
LEFT JOIN VPM_Haichi AS	eVPM_Haichi01
	ON eTKD_Haisha01.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn1
	ON	eTKD_Unkobi01.SijJoKbn1 = CONVERT(tinyint, JM_SijJoKbn1.CodeKbn)
	AND JM_SijJoKbn1.CodeSyu = 'SIJJOKBN1'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn2
	ON	eTKD_Unkobi01.SijJoKbn2 = CONVERT(tinyint, JM_SijJoKbn2.CodeKbn)
	 AND JM_SijJoKbn2.CodeSyu = 'SIJJOKBN2'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn3
	ON	eTKD_Unkobi01.SijJoKbn4 = CONVERT(tinyint, JM_SijJoKbn3.CodeKbn)
	 AND JM_SijJoKbn3.CodeSyu = 'SIJJOKBN3'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn4
	ON	eTKD_Unkobi01.SijJoKbn4 = CONVERT(tinyint, JM_SijJoKbn4.CodeKbn)
	 AND JM_SijJoKbn4.CodeSyu = 'SIJJOKBN4'
LEFT JOIN eVPM_CodeKb AS JM_SijJoKbn5
	ON	eTKD_Unkobi01.SijJoKbn5 = CONVERT(tinyint, JM_SijJoKbn5.CodeKbn)
	AND JM_SijJoKbn5.CodeSyu = 'SIJJOKBN5'
LEFT JOIN eVPM_CodeKb AS JM_UkeJyKbn
	ON	eTKD_Unkobi01.UkeJyKbnCd = CONVERT(tinyint, JM_UkeJyKbn.CodeKbn)
	   AND JM_UkeJyKbn.CodeSyu = 'UkeJyKbnCd'
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb01
	ON eVPM_SyaRyo03.NenryoCd1Seq = eVPM_CodeKb01.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb02
	ON eVPM_SyaRyo03.NenryoCd2Seq = eVPM_CodeKb02.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb03
	ON eVPM_SyaRyo03.NenryoCd3Seq = eVPM_CodeKb03.CodeKbnSeq
LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu01
	ON eVPM_SyaRyo03.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq
	AND eVPM_SyaSyu01.TenantCdSeq = @TenantCdSeq
LEFT JOIN eVPM_CodeKb AS	eVPM_CodeKb12
	ON eVPM_CodeKb12.CodeSyu = 'KATAKBN'
	AND CONVERT(VARCHAR(10),eVPM_SyaSyu01.KataKbn) = eVPM_CodeKb12.CodeKbn
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
	ON TKD_Yyksho.TokuiSeq = eVPM_Tokisk01.TokuiSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
	AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt01
	ON TKD_Yyksho.TokuiSeq = eVPM_TokiSt01.TokuiSeq	
	AND TKD_Yyksho.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya02
	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt02.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk04
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk04.TokuiSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk04.SiyoStaYmd AND eVPM_Tokisk04.SiyoEndYmd
	AND eVPM_Tokisk04.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt05
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt05.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt05.SitenCdSeq
	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt05.SiyoStaYmd AND eVPM_TokiSt05.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya03
	ON eVPM_Tokisk04.GyosyaCdSeq = eVPM_Gyosya03.GyosyaCdSeq
LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos05
	ON TKD_Yyksho.UkeEigCdSeq = eVPM_Eigyos05.EigyoCdSeq
LEFT JOIN VPM_Compny AS	eVPM_Compny01
	ON eVPM_Eigyos05.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain01
	ON TKD_Yyksho.EigTanCdSeq = eVPM_Syain01.SyainCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain02
	ON TKD_Yyksho.InTanCdSeq = eVPM_Syain02.SyainCdSeq
LEFT JOIN VPM_Syain AS eVPM_Syain03
	ON TKD_Yyksho.CANFUTanSeq = eVPM_Syain03.SyainCdSeq
LEFT JOIN VPM_YoyKbn AS	eVPM_YoyKbn01
	ON TKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb17
	ON TKD_Yyksho.SeiKyuKbnSeq = eVPM_CodeKb17.CodeKbnSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo01
	ON eTKD_Unkobi01.HasMapCdSeq = eVPM_Basyo01.BasyoMapCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb05
	ON eVPM_Basyo01.BasyoKenCdSeq = eVPM_CodeKb05.CodeKbnSeq
LEFT JOIN VPM_Basyo AS eVPM_Basyo02
	ON eTKD_Unkobi01.AreaMapSeq	= eVPM_Basyo02.BasyoMapCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb06
	ON eVPM_Basyo02.BasyoKenCdSeq = eVPM_CodeKb06.CodeKbnSeq
LEFT JOIN VPM_JyoKya AS	eVPM_JyoKya01
	ON eTKD_Unkobi01.JyoKyakuCdSeq = eVPM_JyoKya01.JyoKyakuCdSeq
	AND eVPM_JyoKya01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb07
	ON eVPM_JyoKya01.DantaiCdSeq = eVPM_CodeKb07.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb08
	ON eVPM_Basyo03.BasyoKenCdSeq = eVPM_CodeKb08.CodeKbnSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb09
	ON eVPM_Haichi01.BunruiCdSeq = eVPM_CodeKb09.CodeKbnSeq
LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk02
	ON eTKD_Yousha01.YouCdSeq = eVPM_Tokisk02.TokuiSeq
	AND eTKD_Yousha01.HasYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt03
	ON eTKD_Yousha01.YouCdSeq =	 eVPM_TokiSt03.TokuiSeq
	AND eTKD_Yousha01.YouSitCdSeq = eVPM_TokiSt03.SitenCdSeq
	AND eTKD_Yousha01.HasYmd BETWEEN eVPM_TokiSt03.SiyoStaYmd AND eVPM_TokiSt03.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya04
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya04.GyosyaCdSeq
LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu02
	ON eTKD_YykSyu01.SyaSyuCdSeq = eVPM_SyaSyu02.SyaSyuCdSeq
	AND eVPM_SyaSyu02.TenantCdSeq  = @TenantCdSeq
LEFT JOIN eVPM_CodeKb AS	eVPM_CodeKb13
	ON eVPM_CodeKb13.CodeSyu = 'KATAKBN'
	AND	CONVERT(VARCHAR(10),eVPM_SyaSyu02.KataKbn) = eVPM_CodeKb13.CodeKbn
WHERE
	TKD_Yyksho.SiyoKbn = 1
	AND TKD_Yyksho.YoyaSyu = 1
	AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
	AND (@StartDispatchDate IS NULL OR eTKD_Haisha01.HaiSYmd >= @StartDispatchDate)																								-- 配車日　開始
	AND (@EndDispatchDate IS NULL OR eTKD_Haisha01.HaiSYmd <= @EndDispatchDate)																									-- 配車日　終了
	AND (@StartArrivalDate IS NULL OR eTKD_Haisha01.TouYmd >= @StartArrivalDate)																								-- 到着日　開始
	AND (@EndArrivalDate IS NULL OR eTKD_Haisha01.TouYmd <= @EndArrivalDate)																									-- 到着日　終了
	AND (@StartReservationDate IS NULL OR TKD_Yyksho.UkeYmd >= @StartReservationDate)																							-- 予約日　開始
	AND (@EndReservationDate IS NULL OR TKD_Yyksho.UkeYmd<= @EndReservationDate)																								-- 予約日　終了
	AND (@StartReceiptNumber IS NULL OR TKD_Yyksho.UkeNo >= @StartReceiptNumber)																								-- 予約番号　開始
	AND	(@EndReceiptNumber IS NULL OR TKD_Yyksho.UkeNo <= @EndReceiptNumber)																									-- 予約番号　終了
	AND (@StartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)																	-- 予約区分　開始	
	AND	(@EndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)																		-- 予約区分　終了
	AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson)																							-- 営業担当者　開始	
	AND	(@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson)																								-- 営業担当者　終了
	AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos05.EigyoCd >= @StartRegistrationOffice)																					-- 営業所　開始
	AND	(@EndRegistrationOffice IS NULL OR eVPM_Eigyos05.EigyoCd <= @EndRegistrationOffice)																						-- 営業所　終了
	AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson)																								-- 入力担当者　開始
	AND	(@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson)																									-- 入力担当者　終了
	AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') + FORMAT(eVPM_TokiSt01.SitenCd,'0000') >= @StartCustomer)		-- 得意先コード　開始
	AND	(@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') + FORMAT(eVPM_TokiSt01.SitenCd,'0000') <= @EndCustomer)			-- 得意先コード　終了
	AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') + FORMAT(eVPM_Tokisk04.TokuiCd,'0000') + FORMAT(eVPM_TokiSt05.SitenCd,'0000') >= @StartSupplier)		-- 仕入先コード　開始
	AND	(@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') +	FORMAT(eVPM_Tokisk04.TokuiCd,'0000') + FORMAT(eVPM_TokiSt05.SitenCd,'0000') <= @EndSupplier)			-- 仕入先コード　終了
	AND (@StartGroupClassification IS NULL OR eVPM_CodeKb17.CodeKbn >= @StartGroupClassification)																				-- 団体区分　開始
	AND	(@EndGroupClassification IS NULL OR eVPM_CodeKb17.CodeKbn <= @EndGroupClassification)																					-- 団体区分　終了
	AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification)																-- 乗客コード　開始
	AND	(@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification)																	-- 乗客コード　終了
	AND (@StartDestination IS NULL OR eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartDestination)																		-- 行先　開始
	AND	(@EndDestination IS NULL OR eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndDestination)																			-- 行先　終了
	AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace)																	-- 配車地　開始
	AND	(@EndDispatchPlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace)																		-- 配車地　終了
	AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartOccurrencePlace)																-- 発生地　開始
	AND	(@EndOccurrencePlace IS NULL OR eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndOccurrencePlace)																	-- 発生地　終了
	AND (@StartArea IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartArea)																					-- エリア　開始
	AND	(@EndArea IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndArea)																						-- エリア　終了
	AND (@StartReceiptCondition IS NULL OR eTKD_Haisha01.UkeJyKbnCd >= @StartReceiptCondition)																					-- 受付条件　開始
	AND (@EndReceiptCondition IS NULL OR eTKD_Haisha01.UkeJyKbnCd <= @EndReceiptCondition)																						-- 受付条件　終了
	AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice)																							-- 車種単価 開始
	AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice)																								-- 車種単価 終了
	AND (@DantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @DantaNm, '%'))
ORDER BY UkeNo, UnkRen

OPTION (RECOMPILE)
END
RETURN
