USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_HpMnGrp_R]    Script Date: 2020/12/07 13:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	PK_HpMnGrp_R
-- DB-Name		:	予約書テーブル、他
-- Name			:	ハイパーメニューグラフ表示のデータ取得処理
-- Date			:	2020/05/06
-- Author		:	tthanson
-- Descriotion	:	予約書テーブルその他のSelect処理
-- 				:	予約書に紐付けられる他テーブル情報も取得する
---------------------------------------------------
-- Update		:
-- Comment		:
----------------------------------------------------

ALTER PROCEDURE [dbo].[PK_HpMnGrp_R]
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
	@EndCarTypePrice int

AS 
WITH

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


--＜取得＞車種情報(eTKD_YykSyu01)
eTKD_YykSyu01	AS	(
SELECT
		eTKD_YykSyu01.UkeNo
	,	eTKD_YykSyu01.UnkRen
FROM
		TKD_YykSyu				AS	eTKD_YykSyu01
		LEFT JOIN	VPM_SyaSyu	AS	eVPM_SyaSyu01	ON	eTKD_YykSyu01.SyaSyuCdSeq	=		eVPM_SyaSyu01.SyaSyuCdSeq
													AND eVPM_SyaSyu01.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_SyaSyu01.SiyoKbn		=		1
WHERE	eTKD_YykSyu01.SiyoKbn	=	1
	AND (@StartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @StartCarType)            -- 車種　開始
	AND (@EndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @EndCarType)                -- 車種　終了
	AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice) -- 車種単価　開始
	AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice)     -- 車種単価　終了

GROUP BY
		eTKD_YykSyu01.UkeNo
	,	eTKD_YykSyu01.UnkRen
)


/*＜取得＞予約データ取得*/
SELECT 
		eTKD_Yyksho01.UkeNo								AS	UkeNo						-- 受付番号
	,	ISNULL(eTKD_Unkobi01.UnkRen			,0		)	AS	UnkRen						-- 運行日連番
	,	eTKD_Yyksho01.TokuiSeq							AS	TokuiSeq					-- 得意先ＳＥＱ
	,	ISNULL(eVPM_Tokisk01.TokuiCd		,0		)	AS	TokuiCd						-- 得意先コード
	,	ISNULL(eVPM_Tokisk01.RyakuNm		,''		)	AS	TokuiRyakuNm				-- 得意先略名
	,	eTKD_Yyksho01.SitenCdSeq						AS	SitenCdSeq					-- 支店コードＳＥＱ
	,	ISNULL(eVPM_TokiSt01.SitenCd		,0		)	AS	SitenCd						-- 支店コード
	,	ISNULL(eVPM_TokiSt01.SitenNm		,''		)	AS	SitenRyakuNm				-- 支店略名
	,	ISNULL(eTKD_Haisha01.SyaRyoUnc		,0		)	AS	SyaRyoUnc					-- 運賃
	,	eTKD_Yyksho01.EigTanCdSeq						AS	EigTanCdSeq					-- 営業担当者コードＳＥＱ
	,	ISNULL(eVPM_Syain01.SyainCd			,''		)	AS	EigTan_SyainCd				-- 営業担当者社員コード
	,	ISNULL(eVPM_Syain01.SyainNm			,''		)	AS	EigTan_SyainNm				-- 営業担当者社員名
	,	ISNULL(eVPM_CodeKb05.CodeKbn		,''		)	AS	DantaiKbn					-- 団体区分
	,	ISNULL(eVPM_CodeKb05.RyakuNm		,''		)	AS	DantaiKbnRyakuNm			-- 団体区分略名
	,	eTKD_Unkobi01.HaiSYmd							AS	HaiSYmd						-- 配車年月日
	,	eTKD_Unkobi01.TouYmd							AS	TouYmd						-- 到着年月日
	,	eTKD_Yyksho01.UkeYmd							AS	UkeYmd						-- 受付年月日

FROM
		TKD_Yyksho					AS	eTKD_Yyksho01		
		LEFT JOIN	TKD_Unkobi		AS	eTKD_Unkobi01	ON	eTKD_Yyksho01.UkeNo			=		eTKD_Unkobi01.UkeNo
														AND	eTKD_Unkobi01.SiyoKbn		=		1
		LEFT JOIN	eTKD_Haisha01	AS	eTKD_Haisha01	ON	eTKD_Unkobi01.UkeNo			=		eTKD_Haisha01.UkeNo
														AND	eTKD_Unkobi01.UnkRen		=		eTKD_Haisha01.UnkRen
		-- ＜検索/取得＞予約書テーブル．得意先コードＳＥＱより得意先情報を取得(eVPM_Tokisk01)
		LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_Tokisk01.TokuiSeq
														AND eVPM_Tokisk01.TenantCdSeq	=		@TenantCdSeq
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
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_Tokisk02)
		LEFT JOIN	VPM_Tokisk		AS	eVPM_Tokisk02	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_Tokisk02.TokuiSeq
														AND eVPM_Tokisk02.TenantCdSeq	=		@TenantCdSeq
														AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk02.SiyoStaYmd
																						AND		eVPM_Tokisk02.SiyoEndYmd
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya03)
		LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya02	ON	eVPM_Tokisk02.GyosyaCdSeq	=		eVPM_Gyosya02.GyosyaCdSeq
														AND	eVPM_Gyosya02.SiyoKbn		=		1
		-- ＜検索＞予約書テーブルの仕入先支店コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_TokiSt02)
		LEFT JOIN	VPM_TokiSt		AS	eVPM_TokiSt02	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_TokiSt02.TokuiSeq
														AND	eTKD_Yyksho01.SirSitenCdSeq	=		eVPM_TokiSt02.SitenCdSeq
														AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt02.SiyoStaYmd
																						AND		eVPM_TokiSt02.SiyoEndYmd
		-- ＜検索＞運行日テーブルの行き先マップコードＳＥＱより取得(eVPM_Basyo01)
		LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo01	ON	eTKD_Unkobi01.IkMapCdSeq	=		eVPM_Basyo01.BasyoMapCdSeq
														AND	eVPM_Basyo01.SiyoKbn		=		1
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb01)
		LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb01	ON	eVPM_Basyo01.BasyoKenCdSeq	=		eVPM_CodeKb01.CodeKbnSeq
														AND eVPM_CodeKb01.TenantCdSeq	=		0
														AND	eVPM_CodeKb01.SiyoKbn		=		1
		-- ＜検索＞運行日テーブルの配車地コードＳＥＱより取得(eVPM_Haichi01)
		LEFT JOIN	VPM_Haichi		AS	eVPM_Haichi01	ON	eTKD_Unkobi01.HaiSCdSeq		=		eVPM_Haichi01.HaiSCdSeq
														AND	eVPM_Haichi01.SiyoKbn		=		1
		-- ＜検索＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb02)
		LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb02	ON	eVPM_Haichi01.BunruiCdSeq	=		eVPM_CodeKb02.CodeKbnSeq
														AND eVPM_CodeKb02.TenantCdSeq	=		0
														AND	eVPM_CodeKb02.SiyoKbn		=		1
		-- ＜検索＞運行日テーブルの発生地マップコードＳＥＱより取得(eVPM_Basyo02)
		LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo02	ON	eTKD_Unkobi01.HasMapCdSeq	=		eVPM_Basyo02.BasyoMapCdSeq
														AND	eVPM_Basyo02.SiyoKbn		=		1
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb03)
		LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb03	ON	eVPM_Basyo02.BasyoKenCdSeq	=		eVPM_CodeKb03.CodeKbnSeq
														AND eVPM_CodeKb03.TenantCdSeq	=		0
														AND	eVPM_CodeKb03.SiyoKbn		=		1
		-- ＜検索＞運行日テーブルのエリアマップコードＳＥＱより取得(eVPM_Basyo03)
		LEFT JOIN	VPM_Basyo		AS	eVPM_Basyo03	ON	eTKD_Unkobi01.AreaMapSeq	=		eVPM_Basyo03.BasyoMapCdSeq
														AND	eVPM_Basyo03.SiyoKbn		=		1
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb04)
		LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb04	ON	eVPM_Basyo03.BasyoKenCdSeq	=		eVPM_CodeKb04.CodeKbnSeq
														AND eVPM_CodeKb04.TenantCdSeq	=		0
														AND	eVPM_CodeKb04.SiyoKbn		=		1
		-- ＜検索＞運行日テーブルの乗客区分コードＳＥＱより取得(eVPM_JyoKya01)
		LEFT JOIN	VPM_JyoKya		AS	eVPM_JyoKya01	ON	eTKD_Unkobi01.JyoKyakuCdSeq	=		eVPM_JyoKya01.JyoKyakuCdSeq
														AND eVPM_JyoKya01.TenantCdSeq	=		@TenantCdSeq
														AND	eVPM_JyoKya01.SiyoKbn		=		1
		-- ＜検索＞乗客区分マスタの団体区分コードＳＥＱより取得(eVPM_CodeKb05)
		LEFT JOIN	VPM_CodeKb		AS	eVPM_CodeKb05	ON	eVPM_JyoKya01.DantaiCdSeq	=		eVPM_CodeKb05.CodeKbnSeq
														AND eVPM_CodeKb05.TenantCdSeq	=		0
														AND	eVPM_CodeKb05.SiyoKbn		=		1
		--＜検索＞条件設定の車種コード、車種別単価、型区分に該当する予約車種情報を取得する(eTKD_YykSyu01)
		LEFT JOIN	eTKD_YykSyu01						ON	eTKD_Unkobi01.UkeNo			=		eTKD_YykSyu01.UkeNo
														AND	eTKD_Unkobi01.UnkRen		=		eTKD_YykSyu01.UnkRen

/*取得条件*/

WHERE 
		eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq                                                                                                                                  -- ログインユーザの会社のCompanyCdSeq
		AND (@StartDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd >= @StartDispatchDate)                                                                                             -- 配車日　開始
		AND (@EndDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd <= @EndDispatchDate)                                                                                                 -- 配車日　終了
		AND (@StartArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd >= @StartArrivalDate)                                                                                                -- 到着日　開始
		AND (@EndArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd <= @EndArrivalDate)                                                                                                    -- 到着日　終了
		AND (@StartReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd >= @StartReservationDate)                                                                                        -- 予約日　開始
		AND (@EndReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd<= @EndReservationDate)                                                                                             -- 予約日　終了
		AND (@StartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber)                                                                                             -- 予約番号　開始
		AND	(@EndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber)                                                                                                 -- 予約番号　終了
		AND (@StartReservationClassification IS NULL OR eVPM_YoyKbnSort01.PriorityNum >= @StartReservationClassification)                                                           -- 予約区分
                AND (@EndReservationClassification IS NULL OR eVPM_YoyKbnSort01.PriorityNum <= @EndReservationClassification)                                                       -- 予約区分
		AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson)                                                                                            -- 営業担当者　開始	
		AND	(@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson)                                                                                                -- 営業担当者　終了
		AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd >= @StartRegistrationOffice)                                                                                 -- 営業所　開始
		AND	(@EndRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd <= @EndRegistrationOffice)                                                                                     -- 営業所　終了
		AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson)                                                                                                -- 入力担当者　開始
		AND	(@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson)                                                                                                    -- 入力担当者　終了
		AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt01.SitenCd,'0000') >= @StartCustomer)     -- 得意先コード　開始
		AND	(@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt01.SitenCd,'0000') <= @EndCustomer)           -- 得意先コード　終了
		AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') +	FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') >= @StartSupplier)     -- 仕入先コード　開始
		AND	(@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya02.GyosyaCd,'000') +	FORMAT(eVPM_Tokisk02.TokuiCd,'0000') +	FORMAT(eVPM_TokiSt02.SitenCd,'0000') <= @EndSupplier)           -- 仕入先コード　終了
		AND (@StartGroupClassification IS NULL OR eVPM_CodeKb05.CodeKbn >= @StartGroupClassification)                                                                               -- 団体区分　開始
		AND	(@EndGroupClassification IS NULL OR eVPM_CodeKb05.CodeKbn <= @EndGroupClassification)                                                                                   -- 団体区分　終了
		AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification)                                                              -- 乗客コード　開始
		AND	(@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification)                                                                  -- 乗客コード　終了
		AND (@StartDestination IS NULL OR eVPM_CodeKb01.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartDestination)                                                                     -- 行先　開始
		AND	(@EndDestination IS NULL OR eVPM_CodeKb01.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndDestination)                                                                         -- 行先　終了
		AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb02.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace)                                                                    -- 配車地　開始
		AND	(@EndDispatchPlace IS NULL OR eVPM_CodeKb02.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace)                                                                        -- 配車地　終了
		AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb03.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartOccurrencePlace)                                                             -- 発生地　開始
		AND	(@EndOccurrencePlace IS NULL OR eVPM_CodeKb03.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndOccurrencePlace)                                                                 -- 発生地　終了
		AND (@StartArea IS NULL OR eVPM_CodeKb04.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartArea)                                                                                   -- エリア　開始
		AND	(@EndArea IS NULL OR eVPM_CodeKb04.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndArea)                                                                                       -- エリア　終了
		AND (@StartReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd >= @StartReceiptCondition)                                                                                  -- 受付条件　開始
		AND (@EndReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd <= @EndReceiptCondition)                                                                                      -- 受付条件　終了
