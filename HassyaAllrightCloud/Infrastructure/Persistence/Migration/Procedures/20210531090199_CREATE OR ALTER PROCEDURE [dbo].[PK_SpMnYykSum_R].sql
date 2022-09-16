USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpMnYykSum_R]    Script Date: 2021/02/23 10:38:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	PK_SpMnYykSum_R
-- DB-Name		:	予約書テーブル、他
-- Name			:	SuperMenu予約編運行日合計データ取得処理
-- Date			:	2020/05/01
-- Author		:	tthanhson
-- Description	:	予約書テーブルその他のSelect処理
-- 				:	予約書に紐付けられる他テーブル情報も取得する
---------------------------------------------------
-- Update		: NTLanAnh- 2020/12/07
-- Comment		: Change type of conditions date time
----------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_SpMnYykSum_R]
	@TenantCdSeq int,
    @CompanyCdSeq int,
	@StartDispatchDate nvarchar(8),
	@EndDispatchDate nvarchar(8),
	@StartArrivalDate nvarchar(8),
	@EndArrivalDate nvarchar(8),
	@StartReservationDate nvarchar(8),
	@EndReservationDate nvarchar(8),
	@StartReceiptNumber char(15),
	@EndReceiptNumber char(15),
	@StartReservationClassification int,
	@EndReservationClassification int,
	@StartServicePerson nvarchar(10),
	@EndServicePerson nvarchar(10),
	@StartRegistrationOffice int,
	@EndRegistrationOffice int,
	@StartInputPerson nvarchar(10),
	@EndInputPerson nvarchar(10),
	@StartCustomer nvarchar(11),
	@EndCustomer nvarchar(11),
	@StartSupplier nvarchar(11),
	@EndSupplier nvarchar(11),
	@StartGroupClassification nvarchar(10),
	@EndGroupClassification nvarchar(10),
	@StartCustomerTypeClassification int,
	@EndCustomerTypeClassification int,
	@StartDestination nvarchar(10),
	@EndDestination nvarchar(10),
	@StartDispatchPlace nvarchar(10),
	@EndDispatchPlace nvarchar(10),
	@StartOccurrencePlace nvarchar(10),
	@EndOccurrencePlace nvarchar(10),
	@StartArea nvarchar(10),
	@EndArea nvarchar(10),
	@StartReceiptCondition nvarchar(10),
	@EndReceiptCondition nvarchar(10),
	@StartCarType int,
	@EndCarType int,
	@StartCarTypePrice int,
	@EndCarTypePrice int,
	@DantaNm nvarchar(100),
	@MaxMinSetting int,
	@ReservationStatus int
AS
BEGIN
	DECLARE @LocTenantCdSeq int
	SET @LocTenantCdSeq = @TenantCdSeq

    DECLARE @LocCompanyCdSeq int
	SET @LocCompanyCdSeq = @CompanyCdSeq

	DECLARE @LocStartDispatchDate nvarchar(8)
	SET @LocStartDispatchDate = @StartDispatchDate

	DECLARE @LocEndDispatchDate nvarchar(8)
	SET @LocEndDispatchDate = @EndDispatchDate

	DECLARE @LocStartArrivalDate nvarchar(8)
	SET @LocStartArrivalDate = @StartArrivalDate

	DECLARE @LocEndArrivalDate nvarchar(8)
	SET @LocEndArrivalDate = @EndArrivalDate

	DECLARE @LocStartReservationDate nvarchar(8)
	SET @LocStartReservationDate = @StartReservationDate

	DECLARE @LocEndReservationDate nvarchar(8)
	SET @LocEndReservationDate = @EndReservationDate

	DECLARE @LocStartReceiptNumber char(15)
	SET @LocStartReceiptNumber = @StartReceiptNumber

	DECLARE @LocEndReceiptNumber char(15)
	SET @LocEndReceiptNumber = @EndReceiptNumber

	DECLARE @LocStartReservationClassification int
	SET @LocStartReservationClassification = @StartReservationClassification

	DECLARE @LocEndReservationClassification int
	SET @LocEndReservationClassification = @EndReservationClassification

	DECLARE @LocStartServicePerson nvarchar(10)
	SET @LocStartServicePerson = @StartServicePerson

	DECLARE @LocEndServicePerson nvarchar(10)
	SET @LocEndServicePerson = @EndServicePerson

	DECLARE @LocStartRegistrationOffice int
	SET @LocStartRegistrationOffice = @StartRegistrationOffice

	DECLARE @LocEndRegistrationOffice int
	SET @LocEndRegistrationOffice = @EndRegistrationOffice

	DECLARE @LocStartInputPerson nvarchar(10)
	SET @LocStartInputPerson = @StartInputPerson

	DECLARE @LocEndInputPerson nvarchar(10)
	SET @LocEndInputPerson = @EndInputPerson

	DECLARE @LocStartCustomer nvarchar(11)
	SET @LocStartCustomer = @StartCustomer

	DECLARE @LocEndCustomer nvarchar(11)
	SET @LocEndCustomer = @EndCustomer

	DECLARE @LocStartSupplier nvarchar(11)
	SET @LocStartSupplier = @StartSupplier

	DECLARE @LocEndSupplier nvarchar(11)
	SET @LocEndSupplier = @EndSupplier

	DECLARE @LocStartGroupClassification nvarchar(10)
	SET @LocStartGroupClassification = @StartGroupClassification

	DECLARE @LocEndGroupClassification nvarchar(10)
	SET @LocEndGroupClassification = @EndGroupClassification

	DECLARE @LocStartCustomerTypeClassification int
	SET @LocStartCustomerTypeClassification = @StartCustomerTypeClassification

	DECLARE @LocEndCustomerTypeClassification int
	SET @LocEndCustomerTypeClassification = @EndCustomerTypeClassification

	DECLARE @LocStartDestination nvarchar(10)
	SET @LocStartDestination = @StartDestination

	DECLARE @LocEndDestination nvarchar(10)
	SET @LocEndDestination = @EndDestination

	DECLARE @LocStartDispatchPlace nvarchar(10)
	SET @LocStartDispatchPlace = @StartDispatchPlace

	DECLARE @LocEndDispatchPlace nvarchar(10)
	SET @LocEndDispatchPlace = @EndDispatchPlace

	DECLARE @LocStartOccurrencePlace nvarchar(10)
	SET @LocStartOccurrencePlace = @StartOccurrencePlace

	DECLARE @LocEndOccurrencePlace nvarchar(10)
	SET @LocEndOccurrencePlace = @EndOccurrencePlace

	DECLARE @LocStartArea nvarchar(10)
	SET @LocStartArea = @StartArea

	DECLARE @LocEndArea nvarchar(10)
	SET @LocEndArea = @EndArea

	DECLARE @LocStartReceiptCondition nvarchar(10)
	SET @LocStartReceiptCondition = @StartReceiptCondition

	DECLARE @LocEndReceiptCondition nvarchar(10)
	SET @LocEndReceiptCondition = @EndReceiptCondition

	DECLARE @LocStartCarType int
	SET @LocStartCarType = @StartCarType

	DECLARE @LocEndCarType int
	SET @LocEndCarType = @EndCarType

	DECLARE @LocStartCarTypePrice int
	SET @LocStartCarTypePrice = @StartCarTypePrice

	DECLARE @LocEndCarTypePrice int
	SET @LocEndCarTypePrice = @EndCarTypePrice

	DECLARE @LocDantaNm nvarchar(100)
	SET @LocDantaNm = @DantaNm

	DECLARE @LocMaxMinSetting int
	SET @LocMaxMinSetting = @MaxMinSetting

	DECLARE @LocReservationStatus int
	SET @LocReservationStatus = @ReservationStatus

	IF (@LocTenantCdSeq IS NOT NULL)
	BEGIN
		-- ＜取得＞ コード区分名
		WITH eVPM_CodeKb AS (
			SELECT VPM_CodeKb.CodeKbnSeq,
				VPM_CodeKb.CodeKbn,
				VPM_CodeKb.CodeSyu,
				VPM_CodeKb.CodeKbnNm,
				VPM_CodeKb.RyakuNm
			FROM VPM_CodeKb
			LEFT JOIN VPM_CodeSy
				ON VPM_CodeKb.CodeSyu = VPM_CodeSy.CodeSyu
			WHERE VPM_CodeKb.CodeSyu IN ('ZEIKBN', 'SEIKYUKBN', 'DANTAICD', 'KENCD', 'BUNRUICD')
				AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn <> 1 AND VPM_CodeKb.TenantCdSeq = @LocTenantCdSeq))
				AND VPM_CodeKb.SiyoKbn = 1
		),
		-- ＜取得＞ 予約上限下限運賃料金計算テーブル
		eTKD_BookingMaxMinFareFeeCalc01 AS (
			SELECT SUM(UnitPriceMaxAmount) AS UnitPriceMaxAmount,
				SUM(UnitPriceMinAmount) AS UnitPriceMinAmount,
				TKD_BookingMaxMinFareFeeCalc.UkeNo,
				TKD_BookingMaxMinFareFeeCalc.UnkRen
			FROM TKD_BookingMaxMinFareFeeCalc
			GROUP BY UkeNo, UnkRen
		),
		-- ＜取得＞付帯積込品情報を集計(eTKD_FutTum00)
		eTKD_FutTum00 AS (
			SELECT TKD_FutTum.UkeNo AS UkeNo,
				TKD_FutTum.UnkRen AS UnkRen,
				TKD_FutTum.FutTumKbn AS FutTumKbn, -- 付帯積込品区分
				CASE
					WHEN TKD_FutTum.FutTumKbn = 1 THEN VPM_Futai.FutGuiKbn -- 付帯料金区分
					ELSE 0
				END AS FutGuiKbn,
				SUM(TKD_FutTum.UriGakKin) AS UriGakKin_S, -- 売上額集計値
				SUM(TKD_FutTum.SyaRyoSyo) AS SyaRyoSyo_S, -- 消費税額集計値
				SUM(TKD_FutTum.SyaRyoTes) AS SyaRyoTes_S -- 手数料額集計値
			FROM TKD_FutTum
			LEFT JOIN VPM_Futai
				ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq
				AND VPM_Futai.SiyoKbn = 1
				AND VPM_Futai.TenantCdSeq = @LocTenantCdSeq
			WHERE TKD_FutTum.SiyoKbn = 1
			GROUP BY UkeNo,
				UnkRen,
				FutTumKbn,
				CASE
					WHEN TKD_FutTum.FutTumKbn = 1 THEN VPM_Futai.FutGuiKbn
					ELSE 0
				END
		),
		-- ＜取得＞傭車付帯積込品情報を集計(eTKD_YFutTu10)
		eTKD_YFutTu10 AS (
			SELECT TKD_YFutTu.UkeNo AS UkeNo,
				TKD_YFutTu.UnkRen AS UnkRen,
				TKD_YFutTu.FutTumKbn AS FutTumKbn,
				TKD_YFutTu.FutTumCdSeq AS FutTumCdSeq,
				TKD_YFutTu.HaseiKin AS HaseiKin,
				TKD_YFutTu.SyaRyoSyo AS SyaRyoSyo,
				TKD_YFutTu.SyaRyoTes AS SyaRyoTes,
				ISNULL(TKD_Yousha.JitaFlg,1) AS JitaFlg
			FROM TKD_YFutTu
			LEFT JOIN TKD_Yousha
				ON TKD_YFutTu.UkeNo = TKD_Yousha.UkeNo
				AND TKD_YFutTu.UnkRen = TKD_Yousha.UnkRen
				AND TKD_YFutTu.YouTblSeq = TKD_Yousha.YouTblSeq
				AND	TKD_Yousha.SiyoKbn = 1
			WHERE TKD_YFutTu.SiyoKbn = 1
		),	
		eTKD_YFutTu00 AS (
			SELECT eTKD_YFutTu10.UkeNo AS UkeNo,
				eTKD_YFutTu10.UnkRen AS UnkRen,
				eTKD_YFutTu10.FutTumKbn AS FutTumKbn,
				CASE
					WHEN eTKD_YFutTu10.FutTumKbn = 1 THEN VPM_Futai.FutGuiKbn -- 付帯料金区分
					ELSE 0
				END AS SeiFutSyu,
				SUM(eTKD_YFutTu10.HaseiKin) AS HaseiKin_S,
				SUM(eTKD_YFutTu10.SyaRyoSyo) AS SyaRyoSyo_S,
				SUM(eTKD_YFutTu10.SyaRyoTes) AS SyaRyoTes_S
			FROM eTKD_YFutTu10
			LEFT JOIN VPM_Futai
				ON eTKD_YFutTu10.FutTumCdSeq = VPM_Futai.FutaiCdSeq
			WHERE eTKD_YFutTu10.JitaFlg = 0
			GROUP BY eTKD_YFutTu10.UkeNo,
				eTKD_YFutTu10.UnkRen,
				eTKD_YFutTu10.FutTumKbn,
				CASE
					WHEN eTKD_YFutTu10.FutTumKbn = 1 THEN VPM_Futai.FutGuiKbn -- 付帯料金区分
					ELSE 0
				END
		),
		-- ＜取得＞配車情報(eTKD_Haisha10)
		eTKD_Haisha10 AS (
			SELECT TKD_Haisha.UkeNo AS UkeNo,
				TKD_Haisha.UnkRen AS UnkRen,
				TKD_Haisha.SyaRyoUnc AS SyaRyoUnc,
				TKD_Haisha.SyaRyoSyo AS SyaRyoSyo,
				TKD_Haisha.SyaRyoTes AS SyaRyoTes,
				TKD_Haisha.YoushaUnc AS YoushaUnc,
				TKD_Haisha.YoushaSyo AS YoushaSyo,
				TKD_Haisha.YoushaTes AS YoushaTes,
				CASE
					WHEN TKD_Haisha.YouTblSeq = 0 THEN 1
					WHEN TKD_Haisha.YouTblSeq <> 0 AND ISNULL(TKD_Yousha.JitaFlg, 1) = 1 THEN 1
					ELSE 0
					END AS JitaFlg
			FROM TKD_Haisha
			LEFT JOIN TKD_Yousha
				ON TKD_Haisha.UkeNo = TKD_Yousha.UkeNo
				AND	TKD_Haisha.UnkRen = TKD_Yousha.UnkRen
				AND	TKD_Haisha.YouTblSeq = TKD_Yousha.YouTblSeq
				AND	TKD_Yousha.SiyoKbn = 1
			WHERE TKD_Haisha.SiyoKbn = 1
		),
		eTKD_Haisha20 AS (
			SELECT eTKD_Haisha10.UkeNo AS UkeNo,
				eTKD_Haisha10.UnkRen AS UnkRen,
				eTKD_Haisha10.SyaRyoUnc AS SyaRyoUnc,
				eTKD_Haisha10.SyaRyoSyo AS SyaRyoSyo,
				eTKD_Haisha10.SyaRyoTes AS SyaRyoTes,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 1 THEN eTKD_Haisha10.SyaRyoUnc
					ELSE 0
					END AS JiSyaRyoUnc,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 1 THEN	eTKD_Haisha10.SyaRyoSyo
					ELSE 0
					END AS JiSyaRyoSyo,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 1
					THEN eTKD_Haisha10.SyaRyoTes
					ELSE 0
					END AS JiSyaRyoTes,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 0 THEN	eTKD_Haisha10.YoushaUnc
					ELSE 0
					END AS YoushaUnc,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 0 THEN eTKD_Haisha10.YoushaSyo
					ELSE 0
					END AS YoushaSyo,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 0 THEN eTKD_Haisha10.YoushaTes
					ELSE 0
					END AS YoushaTes,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 0 THEN 1
					ELSE 0
					END AS YoushaDai,
				eTKD_Haisha10.JitaFlg AS JitaFlg,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 0 THEN 1
					ELSE 0
					END AS YouExist,
				CASE
					WHEN eTKD_Haisha10.JitaFlg = 1 THEN 1
					ELSE 0
					END AS JiShaExist
			FROM eTKD_Haisha10
		),
		eTKD_Haisha01 AS (
			SELECT eTKD_Haisha20.UkeNo AS UkeNo,
				eTKD_Haisha20.UnkRen AS UnkRen,
				SUM(eTKD_Haisha20.SyaRyoUnc) AS SyaRyoUnc,
				SUM(eTKD_Haisha20.SyaRyoSyo) AS SyaRyoSyo,
				SUM(eTKD_Haisha20.SyaRyoTes) AS SyaRyoTes,
				SUM(eTKD_Haisha20.JiSyaRyoUnc) AS JiSyaRyoUnc,
				SUM(eTKD_Haisha20.JiSyaRyoSyo) AS JiSyaRyoSyo,
				SUM(eTKD_Haisha20.JiSyaRyoTes) AS JiSyaRyoTes,
				SUM(eTKD_Haisha20.YoushaUnc) AS YoushaUnc,
				SUM(eTKD_Haisha20.YoushaSyo) AS YoushaSyo,
				SUM(eTKD_Haisha20.YoushaTes) AS YoushaTes,
				SUM(eTKD_Haisha20.YoushaDai) AS YoushaDai,
				MAX(eTKD_Haisha20.YouExist) AS YouExist,
				MAX(eTKD_Haisha20.JiShaExist) AS JiShaExist
			FROM eTKD_Haisha20
			GROUP BY eTKD_Haisha20.UkeNo,
				eTKD_Haisha20.UnkRen
		),
		--＜取得＞行程入力状況(eTKD_Koteik01)
		eTKD_Koteik01 AS (
			SELECT UkeNo,
				UnkRen,
				COUNT(UkeNo) AS CntInS_Kotei
			FROM TKD_Koteik
			WHERE SiyoKbn =	1
			GROUP BY UkeNo,
				UnkRen
		),
		--＜取得＞手配入力状況(eTKD_Tehai01)
		eTKD_Tehai01 AS	(
			SELECT UkeNo,
				UnkRen,
				COUNT(UkeNo) AS CntInS_Tehai
			FROM TKD_Tehai
			WHERE SiyoKbn = 1
			GROUP BY UkeNo,
				UnkRen
		),
		-- ＜取得＞運行日/付帯積込品区分単位で付帯積込品情報をカウント(eTKD_FutTumCnt)
		eTKD_FutTumCnt AS (
			SELECT UkeNo,
				UnkRen,
				FutTumKbn,
				COUNT(UkeNo) AS CntInS_FutTum
			FROM TKD_FutTum
			WHERE SiyoKbn = 1
			GROUP BY UkeNo, UnkRen, FutTumKbn
		),
		--＜取得＞備考入力状況(eTKD_Biko01)
		eTKD_Biko01	AS	(
			SELECT UkeNo,
				BikoTblSeq,
				COUNT(UkeNo) AS CntInS_Biko
			FROM TKD_Biko
			WHERE SiyoKbn =	1
				AND	BikTblKbn =	2
			GROUP BY UkeNo, BikoTblSeq
		),
		--＜取得＞請求書出力情報(eTKD_SeiPrS01)
		eTKD_SeiPrS01 AS (
			SELECT UkeNo,
				MAX(SeiHatYmd) AS SeiHatYmd
			FROM TKD_SeiPrS
			LEFT JOIN TKD_SeiMei
				ON TKD_SeiMei.SeiOutSeq	= TKD_SeiPrS.SeiOutSeq
				AND TKD_SeiMei.SiyoKbn = 1
			WHERE TKD_SeiPrS.SiyoKbn = 1
			GROUP BY UkeNo
		),
		--＜取得＞車種情報(eTKD_YykSyu02)
		eTKD_YykSyu02 AS (
			SELECT eTKD_YykSyu01.UkeNo,
				eTKD_YykSyu01.UnkRen
			FROM TKD_YykSyu AS eTKD_YykSyu01
			LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu01
				ON eTKD_YykSyu01.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq
				AND	eVPM_SyaSyu01.SiyoKbn = 1
				AND eVPM_SyaSyu01.TenantCdSeq = @LocTenantCdSeq
			WHERE eTKD_YykSyu01.SiyoKbn = 1
				AND (@LocStartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @LocStartCarType) -- 車種　開始
				AND (@LocEndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @LocEndCarType) -- 車種　開始
				AND (@LocStartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @LocStartCarTypePrice) -- 車種　開始
				AND (@LocEndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @LocEndCarTypePrice) -- 車種　開始
			GROUP BY eTKD_YykSyu01.UkeNo,
				eTKD_YykSyu01.UnkRen
		)
		/*＜取得＞予約データ取得*/
		SELECT SUM(CAST(ISNULL(eTKD_Haisha01.SyaRyoUnc, 0) AS BIGINT)) AS SyaRyoUnc,
			SUM(CAST(ISNULL(eTKD_Haisha01.SyaRyoSyo, 0) AS BIGINT)) AS SyaRyoSyo,
			SUM(CAST(ISNULL(eTKD_Haisha01.SyaRyoTes, 0) AS BIGINT)) AS SyaRyoTes,
			SUM(CAST(ISNULL(eTKD_Yyksho01.GuitKin, 0) AS BIGINT)) AS Gui_UriGakKin_S,
			SUM(CAST(ISNULL(eTKD_Yyksho01.TaxGuider, 0) AS BIGINT)) AS Gui_SyaRyoSyo_S,
			SUM(CAST(ISNULL(eTKD_Yyksho01.FeeGuider, 0) AS BIGINT)) AS Gui_SyaRyoTes_S,
			SUM(CAST(ISNULL(eTKD_FutTum02.UriGakKin_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum03.UriGakKin_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum04.UriGakKin_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum05.UriGakKin_S, 0) AS BIGINT)) AS UriGakKin_S,
			SUM(CAST(ISNULL(eTKD_FutTum02.SyaRyoSyo_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum03.SyaRyoSyo_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum04.SyaRyoSyo_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum05.SyaRyoSyo_S, 0) AS BIGINT)) AS SyaRyoSyo_S,
			SUM(CAST(ISNULL(eTKD_FutTum02.SyaRyoTes_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum03.SyaRyoTes_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum04.SyaRyoTes_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum05.SyaRyoTes_S, 0) AS BIGINT)) AS SyaRyoTes_S,
			SUM(CAST(ISNULL(eTKD_Haisha01.JiSyaRyoUnc, 0) AS BIGINT)) AS JiSyaRyoUnc,
			SUM(CAST(ISNULL(eTKD_Haisha01.JiSyaRyoSyo, 0) AS BIGINT)) AS JiSyaRyoSyo,
			SUM(CAST(ISNULL(eTKD_Haisha01.JiSyaRyoTes, 0) AS BIGINT)) AS JiSyaRyoTes,
			SUM(CAST(ISNULL(eTKD_Haisha01.YoushaUnc, 0) AS BIGINT)) AS YoushaUnc,
			SUM(CAST(ISNULL(eTKD_Haisha01.YoushaSyo, 0) AS BIGINT)) AS YoushaSyo,
			SUM(CAST(ISNULL(eTKD_Haisha01.YoushaTes, 0) AS BIGINT)) AS YoushaTes,
			SUM(CAST(ISNULL(eTKD_YFutTu01.HaseiKin_S, 0) AS BIGINT)) AS YGui_HaseiKin_S,
			SUM(CAST(ISNULL(eTKD_YFutTu01.SyaRyoSyo_S, 0) AS BIGINT)) AS YGui_SyaRyoSyo_S,
			SUM(CAST(ISNULL(eTKD_YFutTu01.SyaRyoTes_S, 0) AS BIGINT)) AS YGui_SyaRyoTes_S,
			SUM(CAST(ISNULL(eTKD_YFutTu02.HaseiKin_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu03.HaseiKin_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu04.HaseiKin_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu05.HaseiKin_S, 0) AS BIGINT)) AS FGui_HaseiKin_S,
			SUM(CAST(ISNULL(eTKD_YFutTu02.SyaRyoSyo_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu03.SyaRyoSyo_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu04.SyaRyoSyo_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu05.SyaRyoSyo_S, 0) AS BIGINT)) AS FGui_SyaRyoSyo_S,
			SUM(CAST(ISNULL(eTKD_YFutTu02.SyaRyoTes_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu03.SyaRyoTes_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu04.SyaRyoTes_S, 0) AS BIGINT) + CAST(ISNULL(eTKD_YFutTu05.SyaRyoTes_S, 0) AS BIGINT)) AS FGui_SyaRyoTes_S
		FROM TKD_Yyksho AS eTKD_Yyksho01
		LEFT JOIN TKD_Unkobi AS eTKD_Unkobi01
			ON eTKD_Yyksho01.UkeNo = eTKD_Unkobi01.UkeNo
			AND eTKD_Unkobi01.SiyoKbn = 1
			-- ＜検索/取得＞予約書テーブル．得意先コードＳＥＱより得意先情報を取得(eVPM_Tokisk01)
		LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01
			ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
			AND eTKD_Yyksho01.TenantCdSeq = eVPM_Tokisk01.TenantCdSeq
			AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
		-- ＜検索＞予約書テーブルの得意先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya01)
		LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01
			ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
			AND eVPM_Gyosya01.TenantCdSeq = eVPM_Tokisk01.TenantCdSeq -- 2021/05/24 ADD
			AND eVPM_Gyosya01.SiyoKbn = 1
		-- ＜検索/取得＞予約書テーブル．支店コードＳＥＱより得意先情報を取得(eVPM_TokiSt01)
		LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01
			ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
			AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
			AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_Tokisk03)
		LEFT JOIN VPM_Tokisk AS eVPM_Tokisk03
			ON eTKD_Yyksho01.SirCdSeq = eVPM_Tokisk03.TokuiSeq
			AND eTKD_Yyksho01.TenantCdSeq = eVPM_Tokisk03.TenantCdSeq
			AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk03.SiyoStaYmd AND eVPM_Tokisk03.SiyoEndYmd
		-- ＜検索＞予約書テーブルの得意先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya01)
		LEFT JOIN VPM_Gyosya AS eVPM_Gyosya03
			ON eVPM_Tokisk03.GyosyaCdSeq = eVPM_Gyosya03.GyosyaCdSeq
			AND eVPM_Gyosya03.TenantCdSeq = eVPM_Tokisk03.TenantCdSeq -- 2021/05/24 ADD
			AND eVPM_Gyosya01.SiyoKbn = 1
		-- ＜検索/取得＞予約書テーブル．支店コードＳＥＱより得意先情報を取得(eVPM_TokiSt01)
		LEFT JOIN VPM_TokiSt AS eVPM_TokiSt03
			ON eTKD_Yyksho01.SirCdSeq = eVPM_TokiSt03.TokuiSeq
			AND eTKD_Yyksho01.SirSitenCdSeq = eVPM_TokiSt03.SitenCdSeq
			AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt03.SiyoStaYmd AND eVPM_TokiSt03.SiyoEndYmd
		LEFT JOIN eTKD_Haisha01	AS eTKD_Haisha01
			ON eTKD_Unkobi01.UkeNo = eTKD_Haisha01.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_Haisha01.UnkRen
		-- ＜検索＞予約上限下限運賃料金計算テーブル (eTKD_BookingMaxMinFareFeeCalc01)
		LEFT JOIN eTKD_BookingMaxMinFareFeeCalc01
			ON eTKD_BookingMaxMinFareFeeCalc01.UkeNo = eTKD_Unkobi01.UkeNo
			AND	eTKD_BookingMaxMinFareFeeCalc01.UnkRen = eTKD_Unkobi01.UnkRen
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：ガイド料”に該当する額を集計(eTKD_FutTum01)
		LEFT JOIN eTKD_FutTum00 AS eTKD_FutTum01
			ON eTKD_Unkobi01.UkeNo = eTKD_FutTum01.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_FutTum01.UnkRen
			AND	eTKD_FutTum01.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_FutTum01.FutGuiKbn = 5 -- 付帯料金区分＝ガイド料
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：付帯”に該当する額を集計(eTKD_FutTum02)
		LEFT JOIN eTKD_FutTum00 AS eTKD_FutTum02
			ON	eTKD_Unkobi01.UkeNo = eTKD_FutTum02.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_FutTum02.UnkRen
			AND	eTKD_FutTum02.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_FutTum02.FutGuiKbn = 2 -- 付帯料金区分＝付帯
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：通行料”に該当する額を集計(eTKD_FutTum03)
		LEFT JOIN eTKD_FutTum00 AS eTKD_FutTum03
			ON eTKD_Unkobi01.UkeNo = eTKD_FutTum03.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_FutTum03.UnkRen
			AND	eTKD_FutTum03.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_FutTum03.FutGuiKbn = 3 -- 付帯料金区分＝通行料
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：手配料”に該当する額を集計(eTKD_FutTum04)
		LEFT JOIN eTKD_FutTum00 AS eTKD_FutTum04
			ON eTKD_Unkobi01.UkeNo = eTKD_FutTum04.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_FutTum04.UnkRen
			AND	eTKD_FutTum04.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_FutTum04.FutGuiKbn = 4 -- 付帯料金区分＝手配料
		-- ＜取得＞“付帯積込品区分：積込品”に該当する額を集計(eTKD_FutTum05)
		LEFT JOIN eTKD_FutTum00 AS eTKD_FutTum05
			ON eTKD_Unkobi01.UkeNo = eTKD_FutTum05.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_FutTum05.UnkRen
			AND	eTKD_FutTum05.FutTumKbn = 2
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：ガイド料”に該当する額を集計(eTKD_YFutTu01)
		LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu01
			ON eTKD_Unkobi01.UkeNo = eTKD_YFutTu01.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_YFutTu01.UnkRen
			AND	eTKD_YFutTu01.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_YFutTu01.SeiFutSyu = 5 -- 請求付帯種別＝ガイド料
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：付帯”に該当する額を集計(eTKD_YFutTu02)
		LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu02
			ON eTKD_Unkobi01.UkeNo = eTKD_YFutTu02.UkeNo
			AND eTKD_Unkobi01.UnkRen = eTKD_YFutTu02.UnkRen
			AND	eTKD_YFutTu02.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_YFutTu02.SeiFutSyu = 2 -- 請求付帯種別＝付帯
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：通行料”に該当する額を集計(eTKD_YFutTu03)
		LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu03
			ON eTKD_Unkobi01.UkeNo = eTKD_YFutTu03.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_YFutTu03.UnkRen
			AND	eTKD_YFutTu03.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_YFutTu03.SeiFutSyu = 3 -- 請求付帯種別＝通行料
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：手配料”に該当する額を集計(eTKD_YFutTu04)
		LEFT JOIN eTKD_YFutTu00	AS eTKD_YFutTu04
			ON eTKD_Unkobi01.UkeNo = eTKD_YFutTu04.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_YFutTu04.UnkRen
			AND	eTKD_YFutTu04.FutTumKbn = 1 -- 付帯積込品区分＝付帯
			AND eTKD_YFutTu04.SeiFutSyu = 4 -- 請求付帯種別＝手配料
		-- ＜取得＞“付帯積込品区分：積込品”に該当する額を集計(eTKD_YFutTu05)
		LEFT JOIN eTKD_YFutTu00	AS	eTKD_YFutTu05
			ON eTKD_Unkobi01.UkeNo = eTKD_YFutTu05.UkeNo
			AND	eTKD_Unkobi01.UnkRen = eTKD_YFutTu05.UnkRen
			AND	eTKD_YFutTu05.FutTumKbn = 2 -- 付帯積込品区分＝積込品
		-- ＜検索/取得＞(eVPM_Eigyos01)
		LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos01
			ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
			AND	eVPM_Eigyos01.SiyoKbn = 1
		-- ＜検索/取得＞(eVPM_Syain01)
		LEFT JOIN VPM_Syain AS eVPM_Syain01
			ON eTKD_Yyksho01.EigTanCdSeq = eVPM_Syain01.SyainCdSeq
		-- ＜検索/取得＞(eVPM_Syain02)
		LEFT JOIN VPM_Syain AS eVPM_Syain02
			ON eTKD_Yyksho01.InTanCdSeq	= eVPM_Syain02.SyainCdSeq
		-- ＜検索/取得＞(eVPM_YoyKbn01)
		LEFT JOIN VPM_YoyKbn AS	eVPM_YoyKbn01
			ON eTKD_Yyksho01.YoyaKbnSeq	= eVPM_YoyKbn01.YoyaKbnSeq
			AND eVPM_YoyKbn01.TenantCdSeq = @LocTenantCdSeq -- 	2021/05/24 ADD
		--- LEFT JOIN VPM_YoyaKbnSort AS eVPM_YoyKbnSort01
		---	ON eVPM_YoyKbn01.YoyaKbnSeq = eVPM_YoyKbnSort01.YoyaKbnSeq
		---	AND	eVPM_YoyKbnSort01.TenantCdSeq =	@LocTenantCdSeq
		-- ＜検索＞運行日テーブルの乗客区分コードＳＥＱより取得(eVPM_JyoKya01)
		LEFT JOIN VPM_JyoKya AS	eVPM_JyoKya01
			ON eTKD_Unkobi01.JyoKyakuCdSeq = eVPM_JyoKya01.JyoKyakuCdSeq
			AND	eVPM_JyoKya01.SiyoKbn = 1
			AND eVPM_JyoKya01.TenantCdSeq = @LocTenantCdSeq
		-- ＜検索＞乗客区分マスタの団体区分コードＳＥＱより取得(eVPM_CodeKb11)
		LEFT JOIN eVPM_CodeKb AS eVPM_CodeKb11
			ON eVPM_JyoKya01.DantaiCdSeq = eVPM_CodeKb11.CodeKbnSeq
		-- ＜検索＞運行日テーブルの行き先マップコードＳＥＱより取得(eVPM_Basyo01)
		LEFT JOIN VPM_Basyo AS eVPM_Basyo01
			ON eTKD_Unkobi01.IkMapCdSeq = eVPM_Basyo01.BasyoMapCdSeq
			AND eVPM_Basyo01.TenantCdSeq = @LocTenantCdSeq
			AND	eVPM_Basyo01.SiyoKbn = 1
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb06)
		LEFT JOIN eVPM_CodeKb AS eVPM_CodeKb06
			ON eVPM_Basyo01.BasyoKenCdSeq = eVPM_CodeKb06.CodeKbnSeq
		-- ＜検索＞運行日テーブルの配車地コードＳＥＱより取得(eVPM_Haichi01)
		LEFT JOIN VPM_Haichi AS	eVPM_Haichi01
			ON eTKD_Unkobi01.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq
			AND eVPM_Haichi01.TenantCdSeq = @LocTenantCdSeq
			AND	eVPM_Haichi01.SiyoKbn = 1
		-- ＜検索＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb07)
		LEFT JOIN eVPM_CodeKb AS eVPM_CodeKb07
			ON eVPM_Haichi01.BunruiCdSeq = eVPM_CodeKb07.CodeKbnSeq
		-- ＜検索＞運行日テーブルの発生地マップコードＳＥＱより取得(eVPM_Basyo02)
		LEFT JOIN VPM_Basyo AS eVPM_Basyo02
			ON eTKD_Unkobi01.HasMapCdSeq = eVPM_Basyo02.BasyoMapCdSeq
			AND eVPM_Basyo02.TenantCdSeq = @LocTenantCdSeq
			AND	eVPM_Basyo02.SiyoKbn = 1
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb09)
		LEFT JOIN eVPM_CodeKb AS eVPM_CodeKb09
			ON eVPM_Basyo02.BasyoKenCdSeq = eVPM_CodeKb09.CodeKbnSeq
		-- ＜検索＞運行日テーブルのエリアマップコードＳＥＱより取得(eVPM_Basyo03)
		LEFT JOIN VPM_Basyo AS eVPM_Basyo03
			ON eTKD_Unkobi01.AreaMapSeq	= eVPM_Basyo03.BasyoMapCdSeq
			AND eVPM_Basyo03.TenantCdSeq = @LocTenantCdSeq
			AND	eVPM_Basyo03.SiyoKbn = 1
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb10)
		LEFT JOIN eVPM_CodeKb AS eVPM_CodeKb10
			ON eVPM_Basyo03.BasyoKenCdSeq = eVPM_CodeKb10.CodeKbnSeq
		/*取得条件*/
		WHERE eTKD_Yyksho01.YoyaSyu IN (1, 3)
            AND eTKD_Yyksho01.TenantCdSeq = @LocTenantCdSeq
            AND (@LocStartDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd >= @LocStartDispatchDate) -- 配車日
            AND (@LocEndDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd <= @LocEndDispatchDate) -- 配車日
            AND (@LocStartArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd >= @LocStartArrivalDate) -- 到着日
            AND (@LocEndArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd <= @LocEndArrivalDate) -- 到着日
            AND (@LocStartReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd >= @LocStartReservationDate) -- 予約日
            AND (@LocEndReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd <= @LocEndReservationDate) -- 予約日
            AND (@LocStartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @LocStartReceiptNumber) -- 予約番号
            AND (@LocEndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @LocEndReceiptNumber) -- 予約番号
            AND (@LocStartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @LocStartReservationClassification) -- 予約区分
            AND (@LocEndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @LocEndReservationClassification) -- 予約区分
            AND (@LocStartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @LocStartServicePerson) -- 営業担当者
            AND (@LocEndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @LocEndServicePerson) -- 営業担当者
            AND (@LocStartRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd >= @LocStartRegistrationOffice) -- 営業所
            AND (@LocEndRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd <= @LocEndRegistrationOffice) -- 営業所
            AND (@LocStartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @LocStartInputPerson) -- 入力担当者
            AND (@LocEndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @LocEndInputPerson) -- 入力担当者
            AND (@LocStartCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') >= @LocStartCustomer) -- 得意先コード
            AND (@LocEndCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') <= @LocEndCustomer) -- 得意先コード
            AND (@LocStartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') >= @LocStartSupplier) -- 仕入先コード
            AND (@LocEndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') <= @LocEndSupplier) -- 仕入先コード
            AND (@LocStartGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn >= @LocStartGroupClassification) -- 団体区分
            AND (@LocEndGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn <= @LocEndGroupClassification) -- 団体区分
            AND (@LocStartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @LocStartCustomerTypeClassification) -- 乗客コード
            AND (@LocEndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @LocEndCustomerTypeClassification) -- 乗客コード
            AND (@LocStartDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @LocStartDestination) -- 行先
            AND (@LocEndDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @LocEndDestination) -- 行先
            AND (@LocStartDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd >= @LocStartDispatchPlace) -- 配車地
            AND (@LocEndDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd <= @LocEndDispatchPlace) -- 配車地
            AND (@LocStartOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @LocStartOccurrencePlace) -- 発生地
            AND (@LocEndOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @LocEndOccurrencePlace) -- 発生地
            AND (@LocStartArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @LocStartArea) -- エリア
            AND (@LocEndArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @LocEndArea) -- エリア
            AND (@LocStartReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd >= @LocStartReceiptCondition) -- 受付条件
            AND (@LocEndReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd <= @LocEndReceiptCondition) -- 受付条件
            AND (@LocDantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @LocDantaNm, '%')) -- 団体名 P.M.Nhat add condition 2020/09/04
			AND (@LocMaxMinSetting IS NULL OR @LocMaxMinSetting <> 0 OR ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMaxAmount, 0) <> 0 OR ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMinAmount, 0) <> 0)
			AND (@LocMaxMinSetting IS NULL OR @LocMaxMinSetting <> 1 OR (ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMaxAmount, 0) = 0 AND ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMinAmount, 0) = 0))
			AND (@LocReservationStatus IS NULL OR @LocReservationStatus <> 0 OR eTKD_Yyksho01.YoyaSyu = 1)
			AND (@LocReservationStatus IS NULL OR @LocReservationStatus <> 1 OR eTKD_Yyksho01.YoyaSyu = 3)
		OPTION (RECOMPILE)
	END
END