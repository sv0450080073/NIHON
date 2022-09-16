USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpMnYyk_R]    Script Date: 2020/12/07 8:49:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	PK_SpMnYyk_R
-- DB-Name		:	予約書テーブル、他
-- Name			:	SuperMenu予約編運行日データ取得処理
-- Date			:	2020/04/29 
-- Author		:	tthanhson
-- Descriotion	:	予約書テーブルその他のSelect処理
-- 				:	予約書に紐付けられる他テーブル情報も取得する
---------------------------------------------------
-- Update		: NTLanAnh_2020/12/07
-- Comment		: Get more field
----------------------------------------------------

CREATE OR ALTER   PROCEDURE [dbo].[PK_SpMnYyk_R]
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
	@StartServicePerson int,
	@EndServicePerson int,
	@StartRegistrationOffice int,
	@EndRegistrationOffice int,
	@StartInputPerson int,
	@EndInputPerson int,
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
	@OffSet int,
	@Fetch int
AS
BEGIN
	/*＜取得＞予約データ取得*/
	IF (@OffSet IS NULL)
		BEGIN
			-- ＜取得＞付帯積込品情報を集計(eTKD_FutTum00)
			WITH eTKD_FutTum00 AS (
			SELECT 
					TKD_FutTum.UkeNo AS UkeNo
				,	TKD_FutTum.UnkRen AS UnkRen
				,	TKD_FutTum.FutTumKbn AS FutTumKbn							-- 付帯積込品区分
				,	VPM_Futai.FutGuiKbn AS FutGuiKbn							-- 付帯料金区分
				,	SUM(TKD_FutTum.UriGakKin) AS UriGakKin_S					-- 売上額集計値
				,	SUM(TKD_FutTum.SyaRyoSyo) AS SyaRyoSyo_S					-- 消費税額集計値
				,	SUM(TKD_FutTum.SyaRyoTes) AS SyaRyoTes_S					-- 手数料額集計値
			FROM TKD_FutTum 
				LEFT JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq 
									AND VPM_Futai.SiyoKbn = 1 
			WHERE TKD_FutTum.SiyoKbn = 1 
			GROUP BY UkeNo, UnkRen, FutTumKbn, FutGuiKbn						
			),

			-- ＜取得＞傭車付帯積込品情報を集計(eTKD_YFutTu10)
				eTKD_YFutTu10 AS (
				SELECT
						TKD_YFutTu.UkeNo					AS		UkeNo
					,	TKD_YFutTu.UnkRen					AS		UnkRen
					,	TKD_YFutTu.FutTumKbn				AS		FutTumKbn
					,	TKD_YFutTu.FutTumCdSeq				AS		FutTumCdSeq
					,	TKD_YFutTu.HaseiKin					AS		HaseiKin
					,	TKD_YFutTu.SyaRyoSyo				AS		SyaRyoSyo
					,	TKD_YFutTu.SyaRyoTes				AS		SyaRyoTes
					,	ISNULL(TKD_Yousha.JitaFlg,1)		AS		JitaFlg
				FROM
						TKD_YFutTu
						LEFT JOIN	TKD_Yousha	ON	TKD_YFutTu.UkeNo			=	TKD_Yousha.UkeNo
												AND	TKD_YFutTu.UnkRen			=	TKD_Yousha.UnkRen
												AND	TKD_YFutTu.YouTblSeq		=	TKD_Yousha.YouTblSeq
												AND	TKD_Yousha.SiyoKbn			=	1
				WHERE
						TKD_YFutTu.SiyoKbn		=	1	
				),	
			
				eTKD_YFutTu00 AS (
				SELECT
						eTKD_YFutTu10.UkeNo					AS		UkeNo
					,	eTKD_YFutTu10.UnkRen				AS		UnkRen
					,	eTKD_YFutTu10.FutTumKbn				AS		FutTumKbn
					,	VPM_Futai.FutGuiKbn					AS		SeiFutSyu
					,	SUM(eTKD_YFutTu10.HaseiKin	)		AS		HaseiKin_S
					,	SUM(eTKD_YFutTu10.SyaRyoSyo	)		AS		SyaRyoSyo_S
					,	SUM(eTKD_YFutTu10.SyaRyoTes	)		AS		SyaRyoTes_S
				FROM
						eTKD_YFutTu10
						LEFT JOIN	VPM_Futai	ON	eTKD_YFutTu10.FutTumCdSeq	=	VPM_Futai.FutaiCdSeq
				WHERE
						eTKD_YFutTu10.JitaFlg	=	0
				GROUP BY
						eTKD_YFutTu10.UkeNo
					,	eTKD_YFutTu10.UnkRen
					,	eTKD_YFutTu10.FutTumKbn
					,	VPM_Futai.FutGuiKbn
				),

			-- ＜取得＞運行日/付帯積込品区分単位で付帯積込品情報をカウント(eTKD_FutTumCnt)
				eTKD_FutTumCnt AS (
				SELECT
						UkeNo
					,	UnkRen
					,	FutTumKbn
					,	COUNT(UkeNo) AS CntInS_FutTum
				FROM TKD_FutTum WHERE SiyoKbn = 1 GROUP BY UkeNo, UnkRen, FutTumKbn
				),

			-- ＜取得＞金額確定状況(eTKD_Kaknin10)
				eTKD_Kaknin10 AS (
				SELECT
						eTKD_Kaknin01.UkeNo					AS		UkeNo
					,	ISNULL(eTKD_Kaknin02.KingFlg,0)		AS		KingFlg
				FROM
					(
						SELECT
								TKD_Kaknin.UkeNo
						FROM
								TKD_Kaknin
						WHERE
								TKD_Kaknin.SiyoKbn		=		1
						GROUP BY
								TKD_Kaknin.UkeNo
					)							AS	eTKD_Kaknin01
						LEFT JOIN	TKD_Kaknin	AS	eTKD_Kaknin02
												ON	eTKD_Kaknin01.UkeNo		=	eTKD_Kaknin02.UkeNo
												AND	eTKD_Kaknin02.SiyoKbn	=	1
												AND	eTKD_Kaknin02.KingFlg	=	1
				GROUP BY
						eTKD_Kaknin01.UkeNo
					,	eTKD_Kaknin02.KingFlg
				),

			-- ＜取得＞配車情報(eTKD_Haisha10)
				eTKD_Haisha10	AS	( 
				SELECT
						TKD_Haisha.UkeNo							AS		UkeNo
					,	TKD_Haisha.UnkRen							AS		UnkRen
					,	TKD_Haisha.SyaRyoUnc						AS		SyaRyoUnc
					,	TKD_Haisha.SyaRyoSyo						AS		SyaRyoSyo
					,	TKD_Haisha.SyaRyoTes						AS		SyaRyoTes
					,	TKD_Haisha.YoushaUnc						AS		YoushaUnc
					,	TKD_Haisha.YoushaSyo						AS		YoushaSyo
					,	TKD_Haisha.YoushaTes						AS		YoushaTes
					,	CASE
							WHEN	TKD_Haisha.YouTblSeq			=		0							--	１：自社
							THEN	1
							WHEN	TKD_Haisha.YouTblSeq			<>		0							--	１：自社
								AND	ISNULL(TKD_Yousha.JitaFlg,1)	=		1
							THEN	1
							ELSE																		--	０：他社
									0
						END											AS		JitaFlg
				FROM
						TKD_Haisha
						LEFT JOIN	TKD_Yousha	ON	TKD_Haisha.UkeNo		=	TKD_Yousha.UkeNo
												AND	TKD_Haisha.UnkRen		=	TKD_Yousha.UnkRen
												AND	TKD_Haisha.YouTblSeq	=	TKD_Yousha.YouTblSeq
												AND	TKD_Yousha.SiyoKbn		=	1
				WHERE
						TKD_Haisha.SiyoKbn		=	1
				),
				
				eTKD_Haisha20	AS	(
				SELECT
						eTKD_Haisha10.UkeNo							AS		UkeNo
					,	eTKD_Haisha10.UnkRen						AS		UnkRen
					,	eTKD_Haisha10.SyaRyoUnc						AS		SyaRyoUnc
					,	eTKD_Haisha10.SyaRyoSyo						AS		SyaRyoSyo
					,	eTKD_Haisha10.SyaRyoTes						AS		SyaRyoTes
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	eTKD_Haisha10.SyaRyoUnc
							ELSE	0
						END											AS		JiSyaRyoUnc
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	eTKD_Haisha10.SyaRyoSyo
							ELSE	0
						END											AS		JiSyaRyoSyo
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	eTKD_Haisha10.SyaRyoTes
							ELSE	0
						END											AS		JiSyaRyoTes
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	eTKD_Haisha10.YoushaUnc
							ELSE	0
						END											AS		YoushaUnc
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	eTKD_Haisha10.YoushaSyo
							ELSE	0
						END											AS		YoushaSyo
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	eTKD_Haisha10.YoushaTes
							ELSE	0
						END											AS		YoushaTes
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	1
							ELSE	0
						END											AS		YoushaDai
					,	eTKD_Haisha10.JitaFlg						AS		JitaFlg
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	1
							ELSE	0
						END											AS		YouExist
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	1
							ELSE	0
						END											AS		JiShaExist
				FROM
						eTKD_Haisha10
				),

				eTKD_Haisha01	AS	(
				SELECT
						eTKD_Haisha20.UkeNo							AS		UkeNo
					,	eTKD_Haisha20.UnkRen						AS		UnkRen
					,	SUM(eTKD_Haisha20.SyaRyoUnc		)			AS		SyaRyoUnc
					,	SUM(eTKD_Haisha20.SyaRyoSyo		)			AS		SyaRyoSyo
					,	SUM(eTKD_Haisha20.SyaRyoTes		)			AS		SyaRyoTes
					,	SUM(eTKD_Haisha20.JiSyaRyoUnc	)			AS		JiSyaRyoUnc
					,	SUM(eTKD_Haisha20.JiSyaRyoSyo	)			AS		JiSyaRyoSyo
					,	SUM(eTKD_Haisha20.JiSyaRyoTes	)			AS		JiSyaRyoTes
					,	SUM(eTKD_Haisha20.YoushaUnc		)			AS		YoushaUnc
					,	SUM(eTKD_Haisha20.YoushaSyo		)			AS		YoushaSyo
					,	SUM(eTKD_Haisha20.YoushaTes		)			AS		YoushaTes
					,	SUM(eTKD_Haisha20.YoushaDai		)			AS		YoushaDai
					,	MAX(eTKD_Haisha20.YouExist		)			AS		YouExist
					,	MAX(eTKD_Haisha20.JiShaExist	)			AS		JiShaExist
				FROM
						eTKD_Haisha20
				GROUP BY
						eTKD_Haisha20.UkeNo
					,	eTKD_Haisha20.UnkRen
				),

			--＜取得＞行程入力状況(eTKD_Koteik01)
				eTKD_Koteik01	AS	(
				SELECT
						UkeNo
					,	UnkRen
					,	COUNT(UkeNo)	AS		CntInS_Kotei
				FROM
						TKD_Koteik
				WHERE
						SiyoKbn			=	1
				GROUP BY
						UkeNo
					,	UnkRen
				),

			--＜取得＞手配入力状況(eTKD_Tehai01)
				eTKD_Tehai01	AS	(
				SELECT
						UkeNo
					,	UnkRen
					,	COUNT(UkeNo)	AS		CntInS_Tehai
				FROM
						TKD_Tehai
				WHERE
						SiyoKbn			=		1
				GROUP BY
						UkeNo
					,	UnkRen
				),

			--＜取得＞備考入力状況(eTKD_Biko01)
				eTKD_Biko01	AS	(
				SELECT 
						UkeNo
					,	BikoTblSeq
					,	COUNT(UkeNo)	AS		CntInS_Biko
				FROM
						TKD_Biko
				WHERE
						SiyoKbn			=	1
					AND	BikTblKbn		=	2
				GROUP BY
						UkeNo
					,	BikoTblSeq
				),

			--＜取得＞請求書出力情報(eTKD_SeiPrS01)
				eTKD_SeiPrS01	AS	(
				SELECT
						UkeNo
					,	MAX(SeiHatYmd)  AS      SeiHatYmd
				FROM
						TKD_SeiPrS
						LEFT JOIN	TKD_SeiMei	ON	TKD_SeiMei.SeiOutSeq	=		TKD_SeiPrS.SeiOutSeq
						AND			TKD_SeiMei.SiyoKbn	=	1
				WHERE
						TKD_SeiPrS.SiyoKbn	=	1
				GROUP BY
						UkeNo
				),

			--＜取得＞車種情報(eTKD_YykSyu02)
				eTKD_YykSyu02	AS	(
				SELECT
						eTKD_YykSyu01.UkeNo
					,	eTKD_YykSyu01.UnkRen
				FROM
						TKD_YykSyu				AS	eTKD_YykSyu01
						LEFT JOIN	VPM_SyaSyu	AS	eVPM_SyaSyu01	ON	eTKD_YykSyu01.SyaSyuCdSeq	=		eVPM_SyaSyu01.SyaSyuCdSeq
																	AND	eVPM_SyaSyu01.SiyoKbn		=		1
																	AND eVPM_SyaSyu01.TenantCdSeq   =       @TenantCdSeq
				WHERE	eTKD_YykSyu01.SiyoKbn	=	1
				AND (@StartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @StartCarType) -- 車種　開始
				AND (@EndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @EndCarType) -- 車種　開始
				AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice) -- 車種　開始
				AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice) -- 車種　開始
				GROUP BY
						eTKD_YykSyu01.UkeNo
					,	eTKD_YykSyu01.UnkRen
				)

            SELECT COUNT(*) AS count
            FROM
				TKD_Yyksho					AS	eTKD_Yyksho01		
				LEFT JOIN	eTKD_Kaknin10						ON	eTKD_Yyksho01.UkeNo			=		eTKD_Kaknin10.UkeNo
				LEFT JOIN	TKD_Unkobi		AS	eTKD_Unkobi01	ON	eTKD_Yyksho01.UkeNo			=		eTKD_Unkobi01.UkeNo
																AND	eTKD_Unkobi01.SiyoKbn		=		1
				LEFT JOIN	eTKD_Haisha01	AS	eTKD_Haisha01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_Haisha01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_Haisha01.UnkRen
		        -- ＜検索＞予約上限下限運賃料金計算テーブル (eTKD_BookingMaxMinFareFeeCalc01)
				LEFT JOIN	TKD_BookingMaxMinFareFeeCalc		AS	eTKD_BookingMaxMinFareFeeCalc01	
																ON	eTKD_BookingMaxMinFareFeeCalc01.UkeNo	=	eTKD_Unkobi01.UkeNo
																AND	eTKD_BookingMaxMinFareFeeCalc01.UnkRen	=	eTKD_Unkobi01.UnkRen
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：ガイド料”に該当する額を集計(eTKD_FutTum01)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum01.UnkRen
																AND	eTKD_FutTum01.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum01.FutGuiKbn		=		5								-- 付帯料金区分＝ガイド料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：付帯”に該当する額を集計(eTKD_FutTum02)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum02	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum02.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum02.UnkRen
																AND	eTKD_FutTum02.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum02.FutGuiKbn		=		2								-- 付帯料金区分＝付帯
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：通行料”に該当する額を集計(eTKD_FutTum03)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum03	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum03.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum03.UnkRen
																AND	eTKD_FutTum03.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum03.FutGuiKbn		=		3								-- 付帯料金区分＝通行料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：手配料”に該当する額を集計(eTKD_FutTum04)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum04	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum04.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum04.UnkRen
																AND	eTKD_FutTum04.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum04.FutGuiKbn		=		4								-- 付帯料金区分＝手配料
		        -- ＜取得＞“付帯積込品区分：積込品”に該当する額を集計(eTKD_FutTum05)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum05	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum05.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum05.UnkRen
																AND	eTKD_FutTum05.FutTumKbn		=		2								-- 付帯積込品区分＝積込品
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：ガイド料”に該当する額を集計(eTKD_YFutTu01)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu01.UnkRen
																AND	eTKD_YFutTu01.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu01.SeiFutSyu		=		5								-- 請求付帯種別＝ガイド料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：付帯”に該当する額を集計(eTKD_YFutTu02)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu02	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu02.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu02.UnkRen
																AND	eTKD_YFutTu02.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu02.SeiFutSyu		=		2								-- 請求付帯種別＝付帯
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：通行料”に該当する額を集計(eTKD_YFutTu03)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu03	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu03.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu03.UnkRen
																AND	eTKD_YFutTu03.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu03.SeiFutSyu		=		3								-- 請求付帯種別＝通行料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：手配料”に該当する額を集計(eTKD_YFutTu04)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu04	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu04.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu04.UnkRen
																AND	eTKD_YFutTu04.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu04.SeiFutSyu		=		4								-- 請求付帯種別＝手配料
		        -- ＜取得＞“付帯積込品区分：積込品”に該当する額を集計(eTKD_YFutTu05)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu05	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu05.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu05.UnkRen
																AND	eTKD_YFutTu05.FutTumKbn		=		2								-- 付帯積込品区分＝積込品
		        -- ＜取得＞(eTKD_Koteik01)
				LEFT JOIN	eTKD_Koteik01						ON	eTKD_Unkobi01.UkeNo			=		eTKD_Koteik01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_Koteik01.UnkRen
		        -- ＜取得＞(eTKD_Tehai01)
				LEFT JOIN	eTKD_Tehai01						ON	eTKD_Unkobi01.UkeNo			=		eTKD_Tehai01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_Tehai01.UnkRen
		        -- ＜取得＞(eTKD_FutTum06)
				LEFT JOIN	eTKD_FutTumCnt	AS	eTKD_FutTum06	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum06.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum06.UnkRen
																AND	eTKD_FutTum06.FutTumKbn		=		2								-- 付帯積込品区分＝積込品
		        -- ＜取得＞(eTKD_FutTum07)
				LEFT JOIN	eTKD_FutTumCnt	AS	eTKD_FutTum07	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum07.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum07.UnkRen
																AND	eTKD_FutTum07.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
		        -- ＜取得＞(eTKD_Biko01)
				LEFT JOIN	eTKD_Biko01							ON	eTKD_Unkobi01.UkeNo			=		eTKD_Biko01.UkeNo
																AND	eTKD_Unkobi01.BikoTblSeq	=		eTKD_Biko01.BikoTblSeq
		        -- ＜検索/取得＞予約書テーブル．得意先コードＳＥＱより得意先情報を取得(eVPM_Tokisk01)
				LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_Tokisk01.TokuiSeq
																AND eVPM_Tokisk01.TenantCdSeq   =       @TenantCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk01.SiyoStaYmd
																								AND		eVPM_Tokisk01.SiyoEndYmd
		        -- ＜検索＞予約書テーブルの得意先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya01)
				LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya01	ON	eVPM_Tokisk01.GyosyaCdSeq	=		eVPM_Gyosya01.GyosyaCdSeq
																AND	eVPM_Gyosya01.SiyoKbn		=		1
		        -- ＜検索/取得＞予約書テーブル．支店コードＳＥＱより得意先情報を取得(eVPM_TokiSt01)
				LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_TokiSt01.TokuiSeq
																AND eTKD_Yyksho01.SitenCdSeq	=		eVPM_TokiSt01.SitenCdSeq
																AND eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt01.SiyoStaYmd
																								AND		eVPM_TokiSt01.SiyoEndYmd
		        -- ＜検索/取得＞得意先支店マスタ．請求先コードＳＥＱより請求先情報を取得(eVPM_Tokisk02)
				LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk02	ON	eVPM_TokiSt01.SeiCdSeq		=		eVPM_Tokisk02.TokuiSeq
																AND eVPM_Tokisk02.TenantCdSeq   =       @TenantCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk02.SiyoStaYmd
																								AND		eVPM_Tokisk02.SiyoEndYmd
		        -- ＜検索＞予約書テーブルの請求先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya02)
				LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya02	ON	eVPM_Tokisk02.GyosyaCdSeq	=		eVPM_Gyosya02.GyosyaCdSeq
																AND	eVPM_Gyosya02.SiyoKbn		=		1
		        -- ＜検索/取得＞得意先支店マスタ．請求先コードＳＥＱ/請求先支店コードＳＥＱより請求先情報を取得(eVPM_TokiSt02)
				LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt02	ON	eVPM_TokiSt01.SeiCdSeq		=		eVPM_TokiSt02.TokuiSeq
																AND	eVPM_TokiSt01.SeiSitenCdSeq	=		eVPM_TokiSt02.SitenCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt02.SiyoStaYmd
																								AND		eVPM_TokiSt02.SiyoEndYmd
		        -- ＜取得＞(eVPM_CodeKb01)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb01	ON	eVPM_CodeKb01.CodeKbn		=		dbo.FP_RpZero( 2, eTKD_Yyksho01.ZeiKbn )
																AND eVPM_CodeKb01.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb01.CodeSyu		=		'ZEIKBN'
																AND	eVPM_CodeKb01.SiyoKbn		=		1
		        -- ＜検索/取得＞(eVPM_Eigyos01)
				LEFT JOIN	VPM_Eigyos		AS	eVPM_Eigyos01	ON	eTKD_Yyksho01.UkeEigCdSeq	=		eVPM_Eigyos01.EigyoCdSeq
																AND	eVPM_Eigyos01.SiyoKbn		=		1
		        -- ＜検索/取得＞(eVPM_Compny01)
				LEFT JOIN	VPM_Compny		AS	eVPM_Compny01	ON	eVPM_Eigyos01.CompanyCdSeq	=		eVPM_Compny01.CompanyCdSeq
																AND	eVPM_Compny01.SiyoKbn		=		1
		        -- ＜検索/取得＞(eVPM_Syain01)
				LEFT JOIN	VPM_Syain		AS	eVPM_Syain01	ON	eTKD_Yyksho01.EigTanCdSeq	=		eVPM_Syain01.SyainCdSeq
		        -- ＜検索/取得＞(eVPM_Syain02)
				LEFT JOIN	VPM_Syain		AS	eVPM_Syain02	ON	eTKD_Yyksho01.InTanCdSeq	=		eVPM_Syain02.SyainCdSeq
		        -- ＜検索/取得＞(eVPM_YoyKbn01)
				LEFT JOIN	VPM_YoyKbn		AS	eVPM_YoyKbn01	ON	eTKD_Yyksho01.YoyaKbnSeq	=		eVPM_YoyKbn01.YoyaKbnSeq
				LEFT JOIN	VPM_YoyaKbnSort	AS	eVPM_YoyKbnSort01	ON 	eVPM_YoyKbn01.YoyaKbnSeq =		eVPM_YoyKbnSort01.YoyaKbnSeq
																	AND	eVPM_YoyKbnSort01.TenantCdSeq =	@TenantCdSeq
		        -- ＜取得＞(eTKD_SeiPrS01)
				LEFT JOIN	eTKD_SeiPrS01						ON	eTKD_Yyksho01.UkeNo			=		eTKD_SeiPrs01.UkeNo
		        -- ＜取得＞（eTKD_UnkobiExp01）
				LEFT JOIN	TKD_UnkobiExp	AS	eTKD_UnkobiExp01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_UnkobiExp01.UkeNo
																	AND	eTKD_Unkobi01.UnkRen		=		eTKD_UnkobiExp01.UnkRen
		        -- ＜検索＞予約書テーブルの仕入先コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_Tokisk03)
				LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk03	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_Tokisk03.TokuiSeq
																AND eVPM_Tokisk03.TenantCdSeq   =       @TenantCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk03.SiyoStaYmd
																								AND		eVPM_Tokisk03.SiyoEndYmd
		        -- ＜検索＞予約書テーブルの仕入先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya03)
				LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya03	ON	eVPM_Tokisk03.GyosyaCdSeq	=		eVPM_Gyosya03.GyosyaCdSeq
																AND	eVPM_Gyosya03.SiyoKbn		=		1
		        -- ＜検索＞予約書テーブルの仕入先支店コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_TokiSt03)
				LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt03	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_TokiSt03.TokuiSeq
																AND	eTKD_Yyksho01.SirSitenCdSeq	=		eVPM_TokiSt03.SitenCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt03.SiyoStaYmd
																								AND		eVPM_TokiSt03.SiyoEndYmd
		        -- ＜検索/取得＞予約書テーブルの請求区分ＳＥＱより取得(eVPM_CodeKb05)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb05	ON	eTKD_Yyksho01.SeiKyuKbnSeq	=		eVPM_CodeKb05.CodeKbnSeq
																AND eVPM_CodeKb05.TenantCdSeq   =       @TenantCdSeq
		        -- ＜検索＞運行日テーブルの行き先マップコードＳＥＱより取得(eVPM_Basyo01)
				LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo01	ON	eTKD_Unkobi01.IkMapCdSeq	=		eVPM_Basyo01.BasyoMapCdSeq
																AND	eVPM_Basyo01.SiyoKbn		=		1
		        -- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb06)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb06	ON	eVPM_Basyo01.BasyoKenCdSeq	=		eVPM_CodeKb06.CodeKbnSeq
																AND eVPM_CodeKb06.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb06.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルの配車地コードＳＥＱより取得(eVPM_Haichi01)
				LEFT JOIN	VPM_Haichi		AS	eVPM_Haichi01	ON	eTKD_Unkobi01.HaiSCdSeq		=		eVPM_Haichi01.HaiSCdSeq
																AND	eVPM_Haichi01.SiyoKbn		=		1
		        -- ＜検索＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb07)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb07	ON	eVPM_Haichi01.BunruiCdSeq	=		eVPM_CodeKb07.CodeKbnSeq
																AND eVPM_CodeKb07.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb07.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルの発生地マップコードＳＥＱより取得(eVPM_Basyo02)
				LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo02	ON	eTKD_Unkobi01.HasMapCdSeq	=		eVPM_Basyo02.BasyoMapCdSeq
																AND	eVPM_Basyo02.SiyoKbn		=		1
		        -- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb09)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb09	ON	eVPM_Basyo02.BasyoKenCdSeq	=		eVPM_CodeKb09.CodeKbnSeq
																AND eVPM_CodeKb09.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb09.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルのエリアマップコードＳＥＱより取得(eVPM_Basyo03)
				LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo03	ON	eTKD_Unkobi01.AreaMapSeq	=		eVPM_Basyo03.BasyoMapCdSeq
																AND	eVPM_Basyo03.SiyoKbn		=		1
		        -- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb10)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb10	ON	eVPM_Basyo03.BasyoKenCdSeq	=		eVPM_CodeKb10.CodeKbnSeq
																AND eVPM_CodeKb10.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb10.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルの乗客区分コードＳＥＱより取得(eVPM_JyoKya01)
				LEFT JOIN	VPM_JyoKya		AS	eVPM_JyoKya01	ON	eTKD_Unkobi01.JyoKyakuCdSeq	=		eVPM_JyoKya01.JyoKyakuCdSeq
																AND	eVPM_JyoKya01.SiyoKbn		=		1
																AND eVPM_JyoKya01.TenantCdSeq   =       @TenantCdSeq
		        -- ＜検索＞乗客区分マスタの団体区分コードＳＥＱより取得(eVPM_CodeKb11)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb11	ON	eVPM_JyoKya01.DantaiCdSeq	=		eVPM_CodeKb11.CodeKbnSeq
																AND eVPM_CodeKb11.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb11.SiyoKbn		=		1
		        --＜検索＞条件設定の車種コード、車種別単価、型区分に該当する予約車種情報を取得する(eTKD_YykSyu01)
				INNER JOIN	eTKD_YykSyu02						ON	eTKD_Unkobi01.UkeNo			=		eTKD_YykSyu02.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YykSyu02.UnkRen
                /*取得条件*/
            WHERE
                eTKD_Yyksho01.YoyaSyu = 1
                AND eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq -- ログインユーザの会社のCompanyCdSeq
                AND (@StartDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd >= @StartDispatchDate) -- 配車日
                AND (@EndDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd <= @EndDispatchDate) -- 配車日
                AND (@StartArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd >= @StartArrivalDate) -- 到着日
                AND (@EndArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd <= @EndArrivalDate) -- 到着日
                AND (@StartReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd >= @StartReservationDate) -- 予約日
                AND (@EndReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd <= @EndReservationDate) -- 予約日
                AND (@StartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber) -- 予約番号
                AND (@EndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber) -- 予約番号
                AND (@StartReservationClassification IS NULL OR eVPM_YoyKbnSort01.PriorityNum >= @StartReservationClassification) -- 予約区分
                AND (@EndReservationClassification IS NULL OR eVPM_YoyKbnSort01.PriorityNum <= @EndReservationClassification) -- 予約区分
                AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson) -- 営業担当者
                AND (@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson) -- 営業担当者
                AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd >= @StartRegistrationOffice) -- 営業所
                AND (@EndRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd <= @EndRegistrationOffice) -- 営業所
                AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson) -- 入力担当者
                AND (@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson) -- 入力担当者
                AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') >= @StartCustomer) -- 得意先コード
                AND (@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') <= @EndCustomer) -- 得意先コード
                AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') >= @StartSupplier) -- 仕入先コード
                AND (@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') <= @EndSupplier) -- 仕入先コード
                AND (@StartGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn >= @StartGroupClassification) -- 団体区分
                AND (@EndGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn <= @EndGroupClassification) -- 団体区分
                AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification) -- 乗客コード
                AND (@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification) -- 乗客コード
                AND (@StartDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartDestination) -- 行先
                AND (@EndDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndDestination) -- 行先
                AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace) -- 配車地
                AND (@EndDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace) -- 配車地
                AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartOccurrencePlace) -- 発生地
                AND (@EndOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndOccurrencePlace) -- 発生地
                AND (@StartArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartArea) -- エリア
                AND (@EndArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndArea) -- エリア
                AND (@StartReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd >= @StartReceiptCondition) -- 受付条件
                AND (@EndReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd <= @EndReceiptCondition) -- 受付条件
                AND (@DantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @DantaNm, '%')) -- 団体名 P.M.Nhat add condition 2020/09/04
		END
	ELSE
		BEGIN
			-- ＜取得＞付帯積込品情報を集計(eTKD_FutTum00)
			WITH eTKD_FutTum00 AS (
			SELECT 
					TKD_FutTum.UkeNo AS UkeNo
				,	TKD_FutTum.UnkRen AS UnkRen
				,	TKD_FutTum.FutTumKbn AS FutTumKbn							-- 付帯積込品区分
				,	VPM_Futai.FutGuiKbn AS FutGuiKbn							-- 付帯料金区分
				,	SUM(TKD_FutTum.UriGakKin) AS UriGakKin_S					-- 売上額集計値
				,	SUM(TKD_FutTum.SyaRyoSyo) AS SyaRyoSyo_S					-- 消費税額集計値
				,	SUM(TKD_FutTum.SyaRyoTes) AS SyaRyoTes_S					-- 手数料額集計値
			FROM TKD_FutTum 
				LEFT JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq 
									AND VPM_Futai.SiyoKbn = 1 
			WHERE TKD_FutTum.SiyoKbn = 1 
			GROUP BY UkeNo, UnkRen, FutTumKbn, FutGuiKbn						
			),

			-- ＜取得＞傭車付帯積込品情報を集計(eTKD_YFutTu10)
				eTKD_YFutTu10 AS (
				SELECT
						TKD_YFutTu.UkeNo					AS		UkeNo
					,	TKD_YFutTu.UnkRen					AS		UnkRen
					,	TKD_YFutTu.FutTumKbn				AS		FutTumKbn
					,	TKD_YFutTu.FutTumCdSeq				AS		FutTumCdSeq
					,	TKD_YFutTu.HaseiKin					AS		HaseiKin
					,	TKD_YFutTu.SyaRyoSyo				AS		SyaRyoSyo
					,	TKD_YFutTu.SyaRyoTes				AS		SyaRyoTes
					,	ISNULL(TKD_Yousha.JitaFlg,1)		AS		JitaFlg
				FROM
						TKD_YFutTu
						LEFT JOIN	TKD_Yousha	ON	TKD_YFutTu.UkeNo			=	TKD_Yousha.UkeNo
												AND	TKD_YFutTu.UnkRen			=	TKD_Yousha.UnkRen
												AND	TKD_YFutTu.YouTblSeq		=	TKD_Yousha.YouTblSeq
												AND	TKD_Yousha.SiyoKbn			=	1
				WHERE
						TKD_YFutTu.SiyoKbn		=	1	
				),	
			
				eTKD_YFutTu00 AS (
				SELECT
						eTKD_YFutTu10.UkeNo					AS		UkeNo
					,	eTKD_YFutTu10.UnkRen				AS		UnkRen
					,	eTKD_YFutTu10.FutTumKbn				AS		FutTumKbn
					,	VPM_Futai.FutGuiKbn					AS		SeiFutSyu
					,	SUM(eTKD_YFutTu10.HaseiKin	)		AS		HaseiKin_S
					,	SUM(eTKD_YFutTu10.SyaRyoSyo	)		AS		SyaRyoSyo_S
					,	SUM(eTKD_YFutTu10.SyaRyoTes	)		AS		SyaRyoTes_S
				FROM
						eTKD_YFutTu10
						LEFT JOIN	VPM_Futai	ON	eTKD_YFutTu10.FutTumCdSeq	=	VPM_Futai.FutaiCdSeq
				WHERE
						eTKD_YFutTu10.JitaFlg	=	0
				GROUP BY
						eTKD_YFutTu10.UkeNo
					,	eTKD_YFutTu10.UnkRen
					,	eTKD_YFutTu10.FutTumKbn
					,	VPM_Futai.FutGuiKbn
				),

			-- ＜取得＞運行日/付帯積込品区分単位で付帯積込品情報をカウント(eTKD_FutTumCnt)
				eTKD_FutTumCnt AS (
				SELECT
						UkeNo
					,	UnkRen
					,	FutTumKbn
					,	COUNT(UkeNo) AS CntInS_FutTum
				FROM TKD_FutTum WHERE SiyoKbn = 1 GROUP BY UkeNo, UnkRen, FutTumKbn
				),

			-- ＜取得＞金額確定状況(eTKD_Kaknin10)
				eTKD_Kaknin10 AS (
				SELECT
						eTKD_Kaknin01.UkeNo					AS		UkeNo
					,	ISNULL(eTKD_Kaknin02.KingFlg,0)		AS		KingFlg
				FROM
					(
						SELECT
								TKD_Kaknin.UkeNo
						FROM
								TKD_Kaknin
						WHERE
								TKD_Kaknin.SiyoKbn		=		1
						GROUP BY
								TKD_Kaknin.UkeNo
					)							AS	eTKD_Kaknin01
						LEFT JOIN	TKD_Kaknin	AS	eTKD_Kaknin02
												ON	eTKD_Kaknin01.UkeNo		=	eTKD_Kaknin02.UkeNo
												AND	eTKD_Kaknin02.SiyoKbn	=	1
												AND	eTKD_Kaknin02.KingFlg	=	1
				GROUP BY
						eTKD_Kaknin01.UkeNo
					,	eTKD_Kaknin02.KingFlg
				),

			-- ＜取得＞配車情報(eTKD_Haisha10)
				eTKD_Haisha10	AS	( 
				SELECT
						TKD_Haisha.UkeNo							AS		UkeNo
					,	TKD_Haisha.UnkRen							AS		UnkRen
					,	TKD_Haisha.SyaRyoUnc						AS		SyaRyoUnc
					,	TKD_Haisha.SyaRyoSyo						AS		SyaRyoSyo
					,	TKD_Haisha.SyaRyoTes						AS		SyaRyoTes
					,	TKD_Haisha.YoushaUnc						AS		YoushaUnc
					,	TKD_Haisha.YoushaSyo						AS		YoushaSyo
					,	TKD_Haisha.YoushaTes						AS		YoushaTes
					,	CASE
							WHEN	TKD_Haisha.YouTblSeq			=		0							--	１：自社
							THEN	1
							WHEN	TKD_Haisha.YouTblSeq			<>		0							--	１：自社
								AND	ISNULL(TKD_Yousha.JitaFlg,1)	=		1
							THEN	1
							ELSE																		--	０：他社
									0
						END											AS		JitaFlg
				FROM
						TKD_Haisha
						LEFT JOIN	TKD_Yousha	ON	TKD_Haisha.UkeNo		=	TKD_Yousha.UkeNo
												AND	TKD_Haisha.UnkRen		=	TKD_Yousha.UnkRen
												AND	TKD_Haisha.YouTblSeq	=	TKD_Yousha.YouTblSeq
												AND	TKD_Yousha.SiyoKbn		=	1
				WHERE
						TKD_Haisha.SiyoKbn		=	1
				),
				
				eTKD_Haisha20	AS	(
				SELECT
						eTKD_Haisha10.UkeNo							AS		UkeNo
					,	eTKD_Haisha10.UnkRen						AS		UnkRen
					,	eTKD_Haisha10.SyaRyoUnc						AS		SyaRyoUnc
					,	eTKD_Haisha10.SyaRyoSyo						AS		SyaRyoSyo
					,	eTKD_Haisha10.SyaRyoTes						AS		SyaRyoTes
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	eTKD_Haisha10.SyaRyoUnc
							ELSE	0
						END											AS		JiSyaRyoUnc
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	eTKD_Haisha10.SyaRyoSyo
							ELSE	0
						END											AS		JiSyaRyoSyo
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	eTKD_Haisha10.SyaRyoTes
							ELSE	0
						END											AS		JiSyaRyoTes
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	eTKD_Haisha10.YoushaUnc
							ELSE	0
						END											AS		YoushaUnc
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	eTKD_Haisha10.YoushaSyo
							ELSE	0
						END											AS		YoushaSyo
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	eTKD_Haisha10.YoushaTes
							ELSE	0
						END											AS		YoushaTes
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	1
							ELSE	0
						END											AS		YoushaDai
					,	eTKD_Haisha10.JitaFlg						AS		JitaFlg
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	0
							THEN	1
							ELSE	0
						END											AS		YouExist
					,	CASE
							WHEN	eTKD_Haisha10.JitaFlg	=	1
							THEN	1
							ELSE	0
						END											AS		JiShaExist
				FROM
						eTKD_Haisha10
				),

				eTKD_Haisha01	AS	(
				SELECT
						eTKD_Haisha20.UkeNo							AS		UkeNo
					,	eTKD_Haisha20.UnkRen						AS		UnkRen
					,	SUM(eTKD_Haisha20.SyaRyoUnc		)			AS		SyaRyoUnc
					,	SUM(eTKD_Haisha20.SyaRyoSyo		)			AS		SyaRyoSyo
					,	SUM(eTKD_Haisha20.SyaRyoTes		)			AS		SyaRyoTes
					,	SUM(eTKD_Haisha20.JiSyaRyoUnc	)			AS		JiSyaRyoUnc
					,	SUM(eTKD_Haisha20.JiSyaRyoSyo	)			AS		JiSyaRyoSyo
					,	SUM(eTKD_Haisha20.JiSyaRyoTes	)			AS		JiSyaRyoTes
					,	SUM(eTKD_Haisha20.YoushaUnc		)			AS		YoushaUnc
					,	SUM(eTKD_Haisha20.YoushaSyo		)			AS		YoushaSyo
					,	SUM(eTKD_Haisha20.YoushaTes		)			AS		YoushaTes
					,	SUM(eTKD_Haisha20.YoushaDai		)			AS		YoushaDai
					,	MAX(eTKD_Haisha20.YouExist		)			AS		YouExist
					,	MAX(eTKD_Haisha20.JiShaExist	)			AS		JiShaExist
				FROM
						eTKD_Haisha20
				GROUP BY
						eTKD_Haisha20.UkeNo
					,	eTKD_Haisha20.UnkRen
				),

			--＜取得＞行程入力状況(eTKD_Koteik01)
				eTKD_Koteik01	AS	(
				SELECT
						UkeNo
					,	UnkRen
					,	COUNT(UkeNo)	AS		CntInS_Kotei
				FROM
						TKD_Koteik
				WHERE
						SiyoKbn			=	1
				GROUP BY
						UkeNo
					,	UnkRen
				),

			--＜取得＞手配入力状況(eTKD_Tehai01)
				eTKD_Tehai01	AS	(
				SELECT
						UkeNo
					,	UnkRen
					,	COUNT(UkeNo)	AS		CntInS_Tehai
				FROM
						TKD_Tehai
				WHERE
						SiyoKbn			=		1
				GROUP BY
						UkeNo
					,	UnkRen
				),

			--＜取得＞備考入力状況(eTKD_Biko01)
				eTKD_Biko01	AS	(
				SELECT 
						UkeNo
					,	BikoTblSeq
					,	COUNT(UkeNo)	AS		CntInS_Biko
				FROM
						TKD_Biko
				WHERE
						SiyoKbn			=	1
					AND	BikTblKbn		=	2
				GROUP BY
						UkeNo
					,	BikoTblSeq
				),

			--＜取得＞請求書出力情報(eTKD_SeiPrS01)
				eTKD_SeiPrS01	AS	(
				SELECT
						UkeNo
					,	MAX(SeiHatYmd)  AS      SeiHatYmd
				FROM
						TKD_SeiPrS
						LEFT JOIN	TKD_SeiMei	ON	TKD_SeiMei.SeiOutSeq	=		TKD_SeiPrS.SeiOutSeq
						AND			TKD_SeiMei.SiyoKbn	=	1
				WHERE
						TKD_SeiPrS.SiyoKbn	=	1
				GROUP BY
						UkeNo
				),

			--＜取得＞車種情報(eTKD_YykSyu02)
				eTKD_YykSyu02	AS	(
				SELECT
						eTKD_YykSyu01.UkeNo
					,	eTKD_YykSyu01.UnkRen
				FROM
						TKD_YykSyu				AS	eTKD_YykSyu01
						LEFT JOIN	VPM_SyaSyu	AS	eVPM_SyaSyu01	ON	eTKD_YykSyu01.SyaSyuCdSeq	=		eVPM_SyaSyu01.SyaSyuCdSeq
																	AND	eVPM_SyaSyu01.SiyoKbn		=		1
																	AND eVPM_SyaSyu01.TenantCdSeq   =       @TenantCdSeq
				WHERE	eTKD_YykSyu01.SiyoKbn	=	1
				AND (@StartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @StartCarType) -- 車種　開始
				AND (@EndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @EndCarType) -- 車種　開始
				AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice) -- 車種　開始
				AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice) -- 車種　開始
				GROUP BY
						eTKD_YykSyu01.UkeNo
					,	eTKD_YykSyu01.UnkRen
				)
				
            SELECT 
                    eTKD_Yyksho01.UkeNo								AS	UkeNo						-- 受付番号
                ,	eTKD_Yyksho01.UkeCd								AS	UkeCd						-- 受付コード
                ,	ISNULL(eTKD_Unkobi01.UnkRen			,0		)	AS	UnkRen						-- 運行日連番
                ,	eTKD_Yyksho01.KaknKais							AS	KaknKais					-- 記号_確認回数
                ,	eTKD_Yyksho01.KaktYmd							AS	KaktYmd						-- 記号_確定年月日
                ,	eTKD_Yyksho01.NyuKinKbn							AS	NyuKinKbn					-- 記号_入金区分
                ,	eTKD_Yyksho01.TokuiSeq							AS	TokuiSeq					-- 得意先ＳＥＱ
                ,	ISNULL(eVPM_Tokisk01.TokuiCd		,0		)	AS	TokuiCd						-- 得意先コード
                ,	ISNULL(eVPM_Tokisk01.RyakuNm		,''		)	AS	TokuiRyakuNm				-- 得意先略名
                ,	eTKD_Yyksho01.SitenCdSeq						AS	SitenCdSeq					-- 支店コードＳＥＱ
                ,	ISNULL(eVPM_TokiSt01.SitenCd		,0		)	AS	SitenCd 					-- 支店コード
                ,	ISNULL(eVPM_TokiSt01.RyakuNm		,''		)	AS	SitenRyakuNm				-- 支店略名
                ,	ISNULL(eVPM_TokiSt02.TesKbn			,0		)	AS	SeiSitTesKbn				-- 請求先支店手数料区分_運賃
                ,	ISNULL(eVPM_TokiSt02.TesKbnFut		,0		)	AS	SeiSitTesKbnFut				-- 請求先支店手数料区分_付帯
                ,	ISNULL(eVPM_TokiSt02.TesKbnGui		,0		)	AS	SeiSitTesKbnGui				-- 請求先支店手数料区分_ガイド料
                ,	eTKD_Yyksho01.TokuiTanNm						AS	TokuiTanNm					-- 得意先担当者名
                ,	ISNULL(eTKD_Unkobi01.DanTaNm		,''		)	AS	DanTaNm						-- 団体名
                ,	ISNULL(eTKD_Unkobi01.KanJNm			,''		)	AS	KanJNm						-- 幹事氏名
                ,	ISNULL(eTKD_Unkobi01.IkNm			,''		)	AS	IkNm						-- 行き先名
                ,	ISNULL(eTKD_Unkobi01.HaiSYmd		,''		)	AS	HaiSYmd						-- 配車年月日
                ,	ISNULL(eTKD_Unkobi01.HaiSTime		,''		)	AS	HaiSTime					-- 配車時間
                ,	ISNULL(eTKD_Unkobi01.HaiSNm			,''		)	AS	HaiSNm						-- 配車地名
                ,	ISNULL(eTKD_Unkobi01.TouYmd			,''		)	AS	TouYmd						-- 到着年月日
                ,	ISNULL(eTKD_Unkobi01.TouChTime		,''		)	AS	TouChTime					-- 到着時間
                ,	ISNULL(eTKD_Unkobi01.TouNm			,''		)	AS	TouNm						-- 到着地名
                ,	ISNULL(eTKD_Unkobi01.DrvJin			,''		)	AS	DrvJin						-- 運転手数
                ,	ISNULL(eTKD_Unkobi01.GuiSu			,0		)	AS	GuiSu						-- ガイド数
                ,	ISNULL(eTKD_Unkobi01.SyuPaTime		,''		)	AS	SyuPaTime					-- 出発時間
				,	ISNULL(eTKD_Unkobi01.UnitPriceIndex ,0		)	AS	UnitPriceIndex
                ,	ISNULL(eTKD_Haisha01.SyaRyoUnc		,0		)	AS	SyaRyoUnc					-- 運賃
                ,	eTKD_Yyksho01.ZeiKbn							AS	ZeiKbn						-- 税区分
                ,	ISNULL(eVPM_CodeKb01.RyakuNm		,'' 	)	AS	ZeiKbnRyakuNm				-- 税区分略名
                ,	eTKD_Yyksho01.Zeiritsu							AS	Zeiritsu					-- 消費税率
                ,	ISNULL(eTKD_Haisha01.SyaRyoSyo		,0		)	AS	SyaRyoSyo					-- 消費税額
                ,	eTKD_Yyksho01.TesuRitu							AS	TesuRitu					-- 手数料率
                ,	ISNULL(eTKD_Haisha01.SyaRyoTes		,0		)	AS	SyaRyoTes					-- 手数料額
                ,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMaxAmount,0)	AS	UnitPriceMaxAmount	-- 単価上限額
                ,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMinAmount,0)	AS	UnitPriceMinAmount	-- 単価下限額
                ,	ISNULL(eTKD_Haisha01.JiSyaRyoUnc	,0		)	AS	JiSyaRyoUnc					-- 自社運賃
                ,	ISNULL(eTKD_Haisha01.JiSyaRyoSyo	,0		)	AS	JiSyaRyoSyo					-- 自社消費税額
                ,	ISNULL(eTKD_Haisha01.JiSyaRyoTes	,0		)	AS	JiSyaRyoTes					-- 自社手数料額
                ,	ISNULL(eTKD_FutTum01.UriGakKin_S	,0		)	AS	Gui_UriGakKin_S				-- ガイド料_売上額集計値
                ,	ISNULL(eTKD_FutTum01.SyaRyoSyo_S	,0		)	AS	Gui_SyaRyoSyo_S				-- ガイド料_消費税額集計値
                ,	ISNULL(eTKD_FutTum01.SyaRyoTes_S	,0		)	AS	Gui_SyaRyoTes_S				-- ガイド料_手数料額集計値
                ,	ISNULL(eTKD_FutTum02.UriGakKin_S	,0		)	AS	Fut_UriGakKin_S				-- 付帯_売上額集計値
                ,	ISNULL(eTKD_FutTum02.SyaRyoSyo_S	,0		)	AS	Fut_SyaRyoSyo_S				-- 付帯_消費税額集計値
                ,	ISNULL(eTKD_FutTum02.SyaRyoTes_S	,0		)	AS	Fut_SyaRyoTes_S				-- 付帯_手数料額集計値
                ,	ISNULL(eTKD_FutTum03.UriGakKin_S	,0		)	AS	Tuk_UriGakKin_S				-- 通行料_売上額集計値
                ,	ISNULL(eTKD_FutTum03.SyaRyoSyo_S	,0		)	AS	Tuk_SyaRyoSyo_S				-- 通行料_消費税額集計値
                ,	ISNULL(eTKD_FutTum03.SyaRyoTes_S	,0		)	AS	Tuk_SyaRyoTes_S				-- 通行料_手数料額集計値
                ,	ISNULL(eTKD_FutTum04.UriGakKin_S	,0		)	AS	Teh_UriGakKin_S			-- 手配料_売上額集計値
                ,	ISNULL(eTKD_FutTum04.SyaRyoSyo_S	,0		)	AS	Teh_SyaRyoSyo_S				-- 手配料_消費税額集計値
                ,	ISNULL(eTKD_FutTum04.SyaRyoTes_S	,0		)	AS	Teh_SyaRyoTes_S				-- 手配料_手数料額集計値
                ,	ISNULL(eTKD_FutTum05.UriGakKin_S	,0		)	AS	Tum_UriGakKin_S				-- 積込品_売上額集計値
                ,	ISNULL(eTKD_FutTum05.SyaRyoSyo_S	,0		)	AS	Tum_SyaRyoSyo_S				-- 積込品_消費税額集計値
                ,	ISNULL(eTKD_FutTum05.SyaRyoTes_S	,0		)	AS	Tum_SyaRyoTes_S				-- 積込品_手数料額集計値
                ,	ISNULL(eTKD_Haisha01.YoushaDai		,0		)	AS	YoushaDai					-- 傭車台数
                ,	ISNULL(eTKD_Haisha01.YoushaUnc		,0		)	AS	YoushaUnc					-- 傭車運賃
                ,	ISNULL(eTKD_Haisha01.YoushaSyo		,0		)	AS	YoushaSyo					-- 傭車消費税額
                ,	ISNULL(eTKD_Haisha01.YoushaTes		,0		)	AS	YoushaTes					-- 傭車手数料額
                ,	ISNULL(eTKD_YFutTu01.HaseiKin_S		,0		)	AS	YGui_HaseiKin_S				-- 傭車ガイド料_売上額集計値
                ,	ISNULL(eTKD_YFutTu01.SyaRyoSyo_S	,0		)	AS	YGui_SyaRyoSyo_S			-- 傭車ガイド料_消費税額集計値
                ,	ISNULL(eTKD_YFutTu01.SyaRyoTes_S	,0		)	AS	YGui_SyaRyoTes_S			-- 傭車ガイド料_手数料額集計値
                ,	ISNULL(eTKD_YFutTu02.HaseiKin_S		,0		)	AS	Yfut_HaseiKin_S				-- 傭車付帯_売上額集計値
                ,	ISNULL(eTKD_YFutTu02.SyaRyoSyo_S	,0		)	AS	Yfut_SyaRyoSyo_S			-- 傭車付帯_消費税額集計値
                ,	ISNULL(eTKD_YFutTu02.SyaRyoTes_S	,0		)	AS	Yfut_SyaRyoTes_S			-- 傭車付帯_手数料額集計値
                ,	ISNULL(eTKD_YFutTu03.HaseiKin_S		,0		)	AS	Ytuk_HaseiKin_S				-- 傭車通行料_売上額集計値
                ,	ISNULL(eTKD_YFutTu03.SyaRyoSyo_S	,0		)	AS	Ytuk_SyaRyoSyo_S			-- 傭車通行料_消費税額集計値
                ,	ISNULL(eTKD_YFutTu03.SyaRyoTes_S	,0		)	AS	Ytuk_SyaRyoTes_S			-- 傭車通行料_手数料額集計値
                ,	ISNULL(eTKD_YFutTu04.HaseiKin_S		,0		)	AS	Yteh_HaseiKin_S				-- 傭車手配料_売上額集計値
                ,	ISNULL(eTKD_YFutTu04.SyaRyoSyo_S	,0		)	AS	Yteh_SyaRyoSyo_S			-- 傭車手配料_消費税額集計値
                ,	ISNULL(eTKD_YFutTu04.SyaRyoTes_S	,0		)	AS	Yteh_SyaRyoTes_S			-- 傭車手配料_手数料額集計値
                ,	ISNULL(eTKD_YFutTu05.HaseiKin_S		,0		)	AS	Ytum_HaseiKin_S				-- 傭車積込品_売上額集計値
                ,	ISNULL(eTKD_YFutTu05.SyaRyoSyo_S	,0		)	AS	Ytum_SyaRyoSyo_S			-- 傭車積込品_消費税額集計値
                ,	ISNULL(eTKD_YFutTu05.SyaRyoTes_S	,0		)	AS	Ytum_SyaRyoTes_S			-- 傭車積込品_手数料額集計値
                ,	ISNULL(eTKD_Unkobi01.JyoSyaJin		,0		)	AS	JyoSyaJin					-- 乗車人員
                ,	ISNULL(eTKD_Unkobi01.PlusJin		,0		)	AS	PlusJin						-- プラス人員
                ,	ISNULL(eVPM_CodeKb05.CodeKbn		,0		)	AS	SeiKyuKbn					-- 請求区分
                ,	ISNULL(eVPM_CodeKb05.RyakuNm		,''		)	AS	SeiKyuKbn_RyakuNm			-- 請求区分略名
                ,	eTKD_Yyksho01.SeiTaiYmd							AS	SeiTaiYmd					-- 請求対象年月日
                ,	eTKD_Yyksho01.UkeEigCdSeq						AS	UkeEigCdSeq					-- 受付営業所コードＳＥＱ
                ,	ISNULL(eVPM_Eigyos01.EigyoCd		,0		)	AS	Uke_EigyoCd					-- 受付営業所コード
                ,	ISNULL(eVPM_Eigyos01.RyakuNm		,''		)	AS	UkeEig_RyakuNm				-- 受付営業所略名
                ,	eTKD_Yyksho01.EigTanCdSeq						AS	EigTanCdSeq					-- 営業担当者コードＳＥＱ
                ,	ISNULL(eVPM_Syain01.SyainCd			,''		)	AS	EigTan_SyainCd				-- 営業担当者社員コード
                ,	ISNULL(eVPM_Syain01.SyainNm			,''		)	AS	EigTan_SyainNm				-- 営業担当者社員名
                ,	eTKD_Yyksho01.InTanCdSeq						AS	InTanCdSeq					-- 入力担当者コードＳＥＱ
                ,	ISNULL(eVPM_Syain02.SyainCd			,''		)	AS	InputTan_SyainCd			-- 入力担当者社員コード
                ,	ISNULL(eVPM_Syain02.SyainNm			,''		)	AS	InputTan_SyainNm			-- 入力担当者社員名
                ,	ISNULL(eVPM_YoyKbn01.YoyaKbn		,0		)	AS	YoyaKbn						-- 予約区分
                ,	ISNULL(eVPM_YoyKbn01.YoyaKbnNm		,''		)	AS	YoyaKbn_YoyaKbnNm			-- 予約区分名
                ,	eTKD_Yyksho01.UkeYmd							AS	UkeYmd						-- 受付年月日
                ,	ISNULL(eTKD_Koteik01.CntInS_Kotei	,0		)	AS	CntInS_Kotei				-- 入力状況_行程
                ,	ISNULL(eTKD_Tehai01.CntInS_Tehai	,0		)	AS	CntInS_Tehai				-- 入力状況_手配
                ,	ISNULL(eTKD_FutTum06.CntInS_FutTum	,0		)	AS	CntInS_Tum					-- 入力状況_積込品
                ,	ISNULL(eTKD_FutTum07.CntInS_FutTum	,0		)	AS	CntInS_Fut					-- 入力状況_付帯
                ,	ISNULL(eTKD_Biko01.CntInS_Biko		,0		)	AS	CntInS_Biko					-- 入力状況_備考
                ,	eTKD_Yyksho01.NCouKbn							AS	NCouKbn						-- 入金クーポン区分
                ,	eTKD_Yyksho01.SihKbn							AS	SihKbn						-- 支払区分
                ,	eTKD_Yyksho01.SCouKbn							AS	SCouKbn						-- 支払クーポン区分
                ,	ISNULL(eTKD_Unkobi01.KSKbn			,0		)	AS	KSKbn						-- 仮車区分
                ,	ISNULL(eTKD_Unkobi01.KHinKbn		,0		)	AS	KHinKbn						-- 仮配員区分
                ,	ISNULL(eTKD_Unkobi01.HaiSKbn		,0		)	AS	HaiSKbn						-- 配車区分
                ,	ISNULL(eTKD_Unkobi01.HaiIKbn		,0		)	AS	HaiIKbn						-- 配員区分
                ,	ISNULL(eTKD_Unkobi01.GuiWNin		,0		)	AS	GuiWNin						-- ガイド割当人数
                ,	ISNULL(eTKD_Unkobi01.NippoKbn		,0		)	AS	NippoKbn					-- 日報区分
                ,	ISNULL(eTKD_Unkobi01.YouKbn			,0		)	AS	YouKbn						-- 傭車区分
                ,	ISNULL(eTKD_Kaknin10.KingFlg		,0		)	AS	KingFlg
                ,	ISNULL(eTKD_Haisha01.YouExist		,0		)	AS	YouExist
                ,	ISNULL(eTKD_Haisha01.JiShaExist		,0		)	AS	JiShaExist
                ,	ISNULL(eTKD_SeiPrS01.SeiHatYmd		,''	)	AS	SeiHatYmd						-- 請求出力年月日
                ,	SUBSTRING(ISNULL(eTKD_UnkobiExp01.ExpItem	,''	)	,1	,8	)	AS	UnsoHikiYmd	-- 運送引受書出力年月日
                ,	SUBSTRING(ISNULL(eTKD_UnkobiExp01.ExpItem	,''	)	,55	,8	)	AS	UnkSijiYmd
                ,	ISNULL(eTKD_Unkobi01.BikoTblSeq		,0		)	AS	BikoTblSeq_Unk				-- 備考テーブルＳＥＱ（運行日）
            FROM
				TKD_Yyksho					AS	eTKD_Yyksho01		
				LEFT JOIN	eTKD_Kaknin10						ON	eTKD_Yyksho01.UkeNo			=		eTKD_Kaknin10.UkeNo
				LEFT JOIN	TKD_Unkobi		AS	eTKD_Unkobi01	ON	eTKD_Yyksho01.UkeNo			=		eTKD_Unkobi01.UkeNo
																AND	eTKD_Unkobi01.SiyoKbn		=		1
				LEFT JOIN	eTKD_Haisha01	AS	eTKD_Haisha01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_Haisha01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_Haisha01.UnkRen
		        -- ＜検索＞予約上限下限運賃料金計算テーブル (eTKD_BookingMaxMinFareFeeCalc01)
				LEFT JOIN	TKD_BookingMaxMinFareFeeCalc		AS	eTKD_BookingMaxMinFareFeeCalc01	
																ON	eTKD_BookingMaxMinFareFeeCalc01.UkeNo	=	eTKD_Unkobi01.UkeNo
																AND	eTKD_BookingMaxMinFareFeeCalc01.UnkRen	=	eTKD_Unkobi01.UnkRen
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：ガイド料”に該当する額を集計(eTKD_FutTum01)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum01.UnkRen
																AND	eTKD_FutTum01.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum01.FutGuiKbn		=		5								-- 付帯料金区分＝ガイド料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：付帯”に該当する額を集計(eTKD_FutTum02)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum02	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum02.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum02.UnkRen
																AND	eTKD_FutTum02.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum02.FutGuiKbn		=		2								-- 付帯料金区分＝付帯
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：通行料”に該当する額を集計(eTKD_FutTum03)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum03	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum03.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum03.UnkRen
																AND	eTKD_FutTum03.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum03.FutGuiKbn		=		3								-- 付帯料金区分＝通行料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“付帯料金区分：手配料”に該当する額を集計(eTKD_FutTum04)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum04	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum04.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum04.UnkRen
																AND	eTKD_FutTum04.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_FutTum04.FutGuiKbn		=		4								-- 付帯料金区分＝手配料
		        -- ＜取得＞“付帯積込品区分：積込品”に該当する額を集計(eTKD_FutTum05)
				LEFT JOIN	eTKD_FutTum00	AS	eTKD_FutTum05	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum05.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum05.UnkRen
																AND	eTKD_FutTum05.FutTumKbn		=		2								-- 付帯積込品区分＝積込品
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：ガイド料”に該当する額を集計(eTKD_YFutTu01)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu01.UnkRen
																AND	eTKD_YFutTu01.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu01.SeiFutSyu		=		5								-- 請求付帯種別＝ガイド料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：付帯”に該当する額を集計(eTKD_YFutTu02)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu02	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu02.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu02.UnkRen
																AND	eTKD_YFutTu02.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu02.SeiFutSyu		=		2								-- 請求付帯種別＝付帯
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：通行料”に該当する額を集計(eTKD_YFutTu03)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu03	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu03.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu03.UnkRen
																AND	eTKD_YFutTu03.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu03.SeiFutSyu		=		3								-- 請求付帯種別＝通行料
		        -- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、運行日単位で“支払付帯種別：手配料”に該当する額を集計(eTKD_YFutTu04)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu04	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu04.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu04.UnkRen
																AND	eTKD_YFutTu04.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
																AND eTKD_YFutTu04.SeiFutSyu		=		4								-- 請求付帯種別＝手配料
		        -- ＜取得＞“付帯積込品区分：積込品”に該当する額を集計(eTKD_YFutTu05)
				LEFT JOIN	eTKD_YFutTu00	AS	eTKD_YFutTu05	ON	eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu05.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YFutTu05.UnkRen
																AND	eTKD_YFutTu05.FutTumKbn		=		2								-- 付帯積込品区分＝積込品
		        -- ＜取得＞(eTKD_Koteik01)
				LEFT JOIN	eTKD_Koteik01						ON	eTKD_Unkobi01.UkeNo			=		eTKD_Koteik01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_Koteik01.UnkRen
		        -- ＜取得＞(eTKD_Tehai01)
				LEFT JOIN	eTKD_Tehai01						ON	eTKD_Unkobi01.UkeNo			=		eTKD_Tehai01.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_Tehai01.UnkRen
		        -- ＜取得＞(eTKD_FutTum06)
				LEFT JOIN	eTKD_FutTumCnt	AS	eTKD_FutTum06	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum06.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum06.UnkRen
																AND	eTKD_FutTum06.FutTumKbn		=		2								-- 付帯積込品区分＝積込品
		        -- ＜取得＞(eTKD_FutTum07)
				LEFT JOIN	eTKD_FutTumCnt	AS	eTKD_FutTum07	ON	eTKD_Unkobi01.UkeNo			=		eTKD_FutTum07.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_FutTum07.UnkRen
																AND	eTKD_FutTum07.FutTumKbn		=		1								-- 付帯積込品区分＝付帯
		        -- ＜取得＞(eTKD_Biko01)
				LEFT JOIN	eTKD_Biko01							ON	eTKD_Unkobi01.UkeNo			=		eTKD_Biko01.UkeNo
																AND	eTKD_Unkobi01.BikoTblSeq	=		eTKD_Biko01.BikoTblSeq
		        -- ＜検索/取得＞予約書テーブル．得意先コードＳＥＱより得意先情報を取得(eVPM_Tokisk01)
				LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_Tokisk01.TokuiSeq
																AND eVPM_Tokisk01.TenantCdSeq   =       @TenantCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk01.SiyoStaYmd
																								AND		eVPM_Tokisk01.SiyoEndYmd
		        -- ＜検索＞予約書テーブルの得意先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya01)
				LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya01	ON	eVPM_Tokisk01.GyosyaCdSeq	=		eVPM_Gyosya01.GyosyaCdSeq
																AND	eVPM_Gyosya01.SiyoKbn		=		1
		        -- ＜検索/取得＞予約書テーブル．支店コードＳＥＱより得意先情報を取得(eVPM_TokiSt01)
				LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_TokiSt01.TokuiSeq
																AND eTKD_Yyksho01.SitenCdSeq	=		eVPM_TokiSt01.SitenCdSeq
																AND eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt01.SiyoStaYmd
																								AND		eVPM_TokiSt01.SiyoEndYmd
		        -- ＜検索/取得＞得意先支店マスタ．請求先コードＳＥＱより請求先情報を取得(eVPM_Tokisk02)
				LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk02	ON	eVPM_TokiSt01.SeiCdSeq		=		eVPM_Tokisk02.TokuiSeq
																AND eVPM_Tokisk02.TenantCdSeq   =       @TenantCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk02.SiyoStaYmd
																								AND		eVPM_Tokisk02.SiyoEndYmd
		        -- ＜検索＞予約書テーブルの請求先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya02)
				LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya02	ON	eVPM_Tokisk02.GyosyaCdSeq	=		eVPM_Gyosya02.GyosyaCdSeq
																AND	eVPM_Gyosya02.SiyoKbn		=		1
		        -- ＜検索/取得＞得意先支店マスタ．請求先コードＳＥＱ/請求先支店コードＳＥＱより請求先情報を取得(eVPM_TokiSt02)
				LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt02	ON	eVPM_TokiSt01.SeiCdSeq		=		eVPM_TokiSt02.TokuiSeq
																AND	eVPM_TokiSt01.SeiSitenCdSeq	=		eVPM_TokiSt02.SitenCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt02.SiyoStaYmd
																								AND		eVPM_TokiSt02.SiyoEndYmd
		        -- ＜取得＞(eVPM_CodeKb01)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb01	ON	eVPM_CodeKb01.CodeKbn		=		dbo.FP_RpZero( 2, eTKD_Yyksho01.ZeiKbn )
																AND eVPM_CodeKb01.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb01.CodeSyu		=		'ZEIKBN'
																AND	eVPM_CodeKb01.SiyoKbn		=		1
		        -- ＜検索/取得＞(eVPM_Eigyos01)
				LEFT JOIN	VPM_Eigyos		AS	eVPM_Eigyos01	ON	eTKD_Yyksho01.UkeEigCdSeq	=		eVPM_Eigyos01.EigyoCdSeq
																AND	eVPM_Eigyos01.SiyoKbn		=		1
		        -- ＜検索/取得＞(eVPM_Compny01)
				LEFT JOIN	VPM_Compny		AS	eVPM_Compny01	ON	eVPM_Eigyos01.CompanyCdSeq	=		eVPM_Compny01.CompanyCdSeq
																AND	eVPM_Compny01.SiyoKbn		=		1
		        -- ＜検索/取得＞(eVPM_Syain01)
				LEFT JOIN	VPM_Syain		AS	eVPM_Syain01	ON	eTKD_Yyksho01.EigTanCdSeq	=		eVPM_Syain01.SyainCdSeq
		        -- ＜検索/取得＞(eVPM_Syain02)
				LEFT JOIN	VPM_Syain		AS	eVPM_Syain02	ON	eTKD_Yyksho01.InTanCdSeq	=		eVPM_Syain02.SyainCdSeq
		        -- ＜検索/取得＞(eVPM_YoyKbn01)
				LEFT JOIN	VPM_YoyKbn		AS	eVPM_YoyKbn01	ON	eTKD_Yyksho01.YoyaKbnSeq	=		eVPM_YoyKbn01.YoyaKbnSeq
				LEFT JOIN	VPM_YoyaKbnSort	AS	eVPM_YoyKbnSort01	ON 	eVPM_YoyKbn01.YoyaKbnSeq =		eVPM_YoyKbnSort01.YoyaKbnSeq
																	AND	eVPM_YoyKbnSort01.TenantCdSeq =	@TenantCdSeq
		        -- ＜取得＞(eTKD_SeiPrS01)
				LEFT JOIN	eTKD_SeiPrS01						ON	eTKD_Yyksho01.UkeNo			=		eTKD_SeiPrs01.UkeNo
		        -- ＜取得＞（eTKD_UnkobiExp01）
				LEFT JOIN	TKD_UnkobiExp	AS	eTKD_UnkobiExp01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_UnkobiExp01.UkeNo
																	AND	eTKD_Unkobi01.UnkRen		=		eTKD_UnkobiExp01.UnkRen
		        -- ＜検索＞予約書テーブルの仕入先コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_Tokisk03)
				LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk03	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_Tokisk03.TokuiSeq
																AND eVPM_Tokisk03.TenantCdSeq   =       @TenantCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk03.SiyoStaYmd
																								AND		eVPM_Tokisk03.SiyoEndYmd
		        -- ＜検索＞予約書テーブルの仕入先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya03)
				LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya03	ON	eVPM_Tokisk03.GyosyaCdSeq	=		eVPM_Gyosya03.GyosyaCdSeq
																AND	eVPM_Gyosya03.SiyoKbn		=		1
		        -- ＜検索＞予約書テーブルの仕入先支店コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_TokiSt03)
				LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt03	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_TokiSt03.TokuiSeq
																AND	eTKD_Yyksho01.SirSitenCdSeq	=		eVPM_TokiSt03.SitenCdSeq
																AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt03.SiyoStaYmd
																								AND		eVPM_TokiSt03.SiyoEndYmd
		        -- ＜検索/取得＞予約書テーブルの請求区分ＳＥＱより取得(eVPM_CodeKb05)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb05	ON	eTKD_Yyksho01.SeiKyuKbnSeq	=		eVPM_CodeKb05.CodeKbnSeq
																AND eVPM_CodeKb05.TenantCdSeq   =       @TenantCdSeq
		        -- ＜検索＞運行日テーブルの行き先マップコードＳＥＱより取得(eVPM_Basyo01)
				LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo01	ON	eTKD_Unkobi01.IkMapCdSeq	=		eVPM_Basyo01.BasyoMapCdSeq
																AND	eVPM_Basyo01.SiyoKbn		=		1
		        -- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb06)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb06	ON	eVPM_Basyo01.BasyoKenCdSeq	=		eVPM_CodeKb06.CodeKbnSeq
																AND eVPM_CodeKb06.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb06.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルの配車地コードＳＥＱより取得(eVPM_Haichi01)
				LEFT JOIN	VPM_Haichi		AS	eVPM_Haichi01	ON	eTKD_Unkobi01.HaiSCdSeq		=		eVPM_Haichi01.HaiSCdSeq
																AND	eVPM_Haichi01.SiyoKbn		=		1
		        -- ＜検索＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb07)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb07	ON	eVPM_Haichi01.BunruiCdSeq	=		eVPM_CodeKb07.CodeKbnSeq
																AND eVPM_CodeKb07.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb07.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルの発生地マップコードＳＥＱより取得(eVPM_Basyo02)
				LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo02	ON	eTKD_Unkobi01.HasMapCdSeq	=		eVPM_Basyo02.BasyoMapCdSeq
																AND	eVPM_Basyo02.SiyoKbn		=		1
		        -- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb09)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb09	ON	eVPM_Basyo02.BasyoKenCdSeq	=		eVPM_CodeKb09.CodeKbnSeq
																AND eVPM_CodeKb09.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb09.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルのエリアマップコードＳＥＱより取得(eVPM_Basyo03)
				LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo03	ON	eTKD_Unkobi01.AreaMapSeq	=		eVPM_Basyo03.BasyoMapCdSeq
																AND	eVPM_Basyo03.SiyoKbn		=		1
		        -- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb10)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb10	ON	eVPM_Basyo03.BasyoKenCdSeq	=		eVPM_CodeKb10.CodeKbnSeq
																AND eVPM_CodeKb10.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb10.SiyoKbn		=		1
		        -- ＜検索＞運行日テーブルの乗客区分コードＳＥＱより取得(eVPM_JyoKya01)
				LEFT JOIN	VPM_JyoKya		AS	eVPM_JyoKya01	ON	eTKD_Unkobi01.JyoKyakuCdSeq	=		eVPM_JyoKya01.JyoKyakuCdSeq
																AND	eVPM_JyoKya01.SiyoKbn		=		1
																AND eVPM_JyoKya01.TenantCdSeq   =       @TenantCdSeq
		        -- ＜検索＞乗客区分マスタの団体区分コードＳＥＱより取得(eVPM_CodeKb11)
				LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb11	ON	eVPM_JyoKya01.DantaiCdSeq	=		eVPM_CodeKb11.CodeKbnSeq
																AND eVPM_CodeKb11.TenantCdSeq   =       @TenantCdSeq
																AND	eVPM_CodeKb11.SiyoKbn		=		1
		        --＜検索＞条件設定の車種コード、車種別単価、型区分に該当する予約車種情報を取得する(eTKD_YykSyu01)
				INNER JOIN	eTKD_YykSyu02						ON	eTKD_Unkobi01.UkeNo			=		eTKD_YykSyu02.UkeNo
																AND	eTKD_Unkobi01.UnkRen		=		eTKD_YykSyu02.UnkRen
                /*取得条件*/
            WHERE
                eTKD_Yyksho01.YoyaSyu = 1
                AND eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq -- ログインユーザの会社のCompanyCdSeq
                AND (@StartDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd >= @StartDispatchDate) -- 配車日
                AND (@EndDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd <= @EndDispatchDate) -- 配車日
                AND (@StartArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd >= @StartArrivalDate) -- 到着日
                AND (@EndArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd <= @EndArrivalDate) -- 到着日
                AND (@StartReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd >= @StartReservationDate) -- 予約日
                AND (@EndReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd <= @EndReservationDate) -- 予約日
                AND (@StartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber) -- 予約番号
                AND (@EndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber) -- 予約番号
                AND (@StartReservationClassification IS NULL OR eVPM_YoyKbnSort01.PriorityNum >= @StartReservationClassification) -- 予約区分
                AND (@EndReservationClassification IS NULL OR eVPM_YoyKbnSort01.PriorityNum <= @EndReservationClassification) -- 予約区分
                AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson) -- 営業担当者
                AND (@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson) -- 営業担当者
                AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd >= @StartRegistrationOffice) -- 営業所
                AND (@EndRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd <= @EndRegistrationOffice) -- 営業所
                AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson) -- 入力担当者
                AND (@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson) -- 入力担当者
                AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') >= @StartCustomer) -- 得意先コード
                AND (@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') <= @EndCustomer) -- 得意先コード
                AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') >= @StartSupplier) -- 仕入先コード
                AND (@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') <= @EndSupplier) -- 仕入先コード
                AND (@StartGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn >= @StartGroupClassification) -- 団体区分
                AND (@EndGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn <= @EndGroupClassification) -- 団体区分
                AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification) -- 乗客コード
                AND (@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification) -- 乗客コード
                AND (@StartDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartDestination) -- 行先
                AND (@EndDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndDestination) -- 行先
                AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace) -- 配車地
                AND (@EndDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace) -- 配車地
                AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartOccurrencePlace) -- 発生地
                AND (@EndOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndOccurrencePlace) -- 発生地
                AND (@StartArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartArea) -- エリア
                AND (@EndArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndArea) -- エリア
                AND (@StartReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd >= @StartReceiptCondition) -- 受付条件
                AND (@EndReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd <= @EndReceiptCondition) -- 受付条件
                AND (@DantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @DantaNm, '%')) -- 団体名 P.M.Nhat add condition 2020/09/04
            ORDER BY UkeNo, UnkRen
            OFFSET @OffSet ROWS
            FETCH NEXT @Fetch ROWS ONLY
		END
END
