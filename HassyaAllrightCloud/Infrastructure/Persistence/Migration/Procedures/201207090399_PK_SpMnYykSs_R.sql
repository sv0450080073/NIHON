USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpMnYykSs_R]    Script Date: 2020/12/07 8:58:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	PK_SpMnYykSs_R
-- DB-Name		:	予約書テーブル、他
-- Name			:	SuperMenu予約編車種データ取得処理
-- Date			:	2020/04/29
-- Author		:	tthanhson
-- Descriotion	:	予約書テーブルその他のSelect処理
-- 				:	予約書に紐付けられる他テーブル情報も取得する
---------------------------------------------------
-- Update		: NTLanAnh- 2020/12/07
-- Comment		: Change type of conditions date time
----------------------------------------------------

ALTER   PROCEDURE [dbo].[PK_SpMnYykSs_R]
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
	@StartFilterNo nvarchar(20),
	@EndFilterNo nvarchar(20),
	@DantaNm nvarchar(100)
AS

SELECT
		eTKD_Yyksho01.UkeNo								AS	UkeNo					-- 受付番号															
	,	ISNULL(eTKD_Unkobi01.UnkRen			,0		)	AS	UnkRen					-- 運行日連番
	,	ISNULL(eTKD_YykSyu01.SyaSyuRen		,0		)	AS	SyaSyuRen				-- 車種連番
	,	ISNULL(eTKD_YykSyu01.SyaSyuCdSeq	,0		)	AS	SyaSyuCdSeq				-- 車種コードＳＥＱ
	,	ISNULL(eTKD_YykSyu01.SyaSyuCd		,0		)	AS	SyaSyuCd				-- 車種コード
	,	ISNULL(eTKD_YykSyu01.SyaSyuNm		,' '	)	AS	SyaSyuNm				-- 車種名
	,	ISNULL(eTKD_YykSyu01.KataKbn		,0		)	AS	KataKbn					-- 型区分
	,	ISNULL(eTKD_YykSyu01.RyakuNm		,' '	)	AS	KataKbnRyakuNm			-- 型区分略名
	,	ISNULL(eTKD_YykSyu01.SyaSyuDai		,0		)	AS	SyaSyuDai				-- 車種台数

	FROM
		TKD_Yyksho	AS	eTKD_Yyksho01 

		-- ＜検索/取得＞（eTKD_Unkobi01）
		LEFT JOIN	TKD_Unkobi	AS	eTKD_Unkobi01	ON	eTKD_Yyksho01.UkeNo			=		eTKD_Unkobi01.UkeNo
													AND	eTKD_Unkobi01.SiyoKbn		=		1     
		-- ＜検索/取得＞予約書テーブル．得意先コードＳＥＱより得意先情報を取得(eVPM_Tokisk01)
		LEFT JOIN	VPM_Tokisk	AS	eVPM_Tokisk01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_Tokisk01.TokuiSeq
													AND eVPM_Tokisk01.TenantCdSeq	=		@TenantCdSeq
													AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk01.SiyoStaYmd
																					AND		eVPM_Tokisk01.SiyoEndYmd
		-- ＜検索＞予約書テーブルの得意先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya01)
		LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya01	ON	eVPM_Tokisk01.GyosyaCdSeq	=		eVPM_Gyosya01.GyosyaCdSeq
														AND	eVPM_Gyosya01.SiyoKbn		=		1
		-- ＜検索/取得＞予約書テーブル．支店コードＳＥＱより得意先情報を取得(eVPM_TokiSt01)
		LEFT JOIN	VPM_TokiSt	AS	eVPM_TokiSt01	ON	eTKD_Yyksho01.TokuiSeq		=		eVPM_TokiSt01.TokuiSeq
													AND	eTKD_Yyksho01.SitenCdSeq	=		eVPM_TokiSt01.SitenCdSeq
													AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt01.SiyoStaYmd
																					AND		eVPM_TokiSt01.SiyoEndYmd
		-- ＜検索/取得＞(eVPM_Eigyos01)
		LEFT JOIN	VPM_Eigyos	AS	eVPM_Eigyos01	ON	eTKD_Yyksho01.UkeEigCdSeq	=		eVPM_Eigyos01.EigyoCdSeq
													AND	eVPM_Eigyos01.SiyoKbn		=		1
		-- ＜検索/取得＞(eVPM_Compny01)
		LEFT JOIN	VPM_Compny AS	eVPM_Compny01	ON	eVPM_Eigyos01.CompanyCdSeq	=		eVPM_Compny01.CompanyCdSeq
													AND	eVPM_Compny01.SiyoKbn		=		1
		-- ＜検索/取得＞(eVPM_Syain01)
		LEFT JOIN	VPM_Syain	AS	eVPM_Syain01	ON	eTKD_Yyksho01.EigTanCdSeq	=		eVPM_Syain01.SyainCdSeq
		-- ＜検索/取得＞(eVPM_Syain02)
		LEFT JOIN	VPM_Syain	AS	eVPM_Syain02	ON	eTKD_Yyksho01.InTanCdSeq	=		eVPM_Syain02.SyainCdSeq
		-- ＜取得＞条件設定の車種コード、車種別単価、型区分に該当する予約車種情報
		JOIN	(	SELECT	
						eTKD_YykSyu01.UkeNo
					,	eTKD_YykSyu01.UnkRen
					,	eTKD_YykSyu01.SyaSyuRen
					,	eTKD_YykSyu01.SyaSyuCdSeq
					,	eVPM_SyaSyu01.SyaSyuCd
					,	eVPM_SyaSyu01.SyaSyuNm
					,	eTKD_YykSyu01.KataKbn
					,	eVPM_CodeKb12.RyakuNm
					,	eTKD_YykSyu01.SyaSyuDai
					FROM
						TKD_YykSyu				AS	eTKD_YykSyu01
					-- ＜検索＞(eVPM_SyaSyu01)
						LEFT JOIN	VPM_SyaSyu	AS	eVPM_SyaSyu01	ON	eTKD_YykSyu01.SyaSyuCdSeq	=	eVPM_SyaSyu01.SyaSyuCdSeq
																	AND eVPM_SyaSyu01.TenantCdSeq	=	@TenantCdSeq
																	AND	eVPM_SyaSyu01.SiyoKbn		=	1
					-- ＜検索＞(eVPM_CodeKb12)
						LEFT JOIN	VPM_CodeKb	AS	eVPM_CodeKb12	ON	eVPM_CodeKb12.CodeSyu						=	'KATAKBN'
																	AND eVPM_CodeKb12.TenantCdSeq	=	@TenantCdSeq
																	AND	CONVERT(VARCHAR(10),eTKD_YykSyu01.KataKbn)	=	eVPM_CodeKb12.CodeKbn
																	AND	eVPM_CodeKb12.SiyoKbn						=	1
					WHERE	
						eTKD_YykSyu01.SiyoKbn	=	1
						AND (@StartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @StartCarType)            -- 車種　開始
						AND (@EndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @EndCarType)                -- 車種　終了
						AND (@StartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @StartCarTypePrice) -- 車種単価　開始
						AND (@EndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @EndCarTypePrice)     -- 車種単価　終了
				)	AS	eTKD_YykSyu01				ON	eTKD_Unkobi01.UkeNo			=		eTKD_YykSyu01.UkeNo
													AND	eTKD_Unkobi01.UnkRen		=		eTKD_YykSyu01.UnkRen
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_Tokisk03)
		LEFT JOIN	VPM_Tokisk	AS	eVPM_Tokisk03	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_Tokisk03.TokuiSeq	
													AND eVPM_Tokisk03.TenantCdSeq	=		@TenantCdSeq
													AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk03.SiyoStaYmd	
																					AND		eVPM_Tokisk03.SiyoEndYmd	
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya03)
		LEFT JOIN	VPM_Gyosya		AS	eVPM_Gyosya03	ON	eVPM_Tokisk03.GyosyaCdSeq	=		eVPM_Gyosya03.GyosyaCdSeq
														AND	eVPM_Gyosya03.SiyoKbn		=		1
		-- ＜検索＞予約書テーブルの仕入先支店コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_TokiSt03
		LEFT JOIN	VPM_TokiSt	AS	eVPM_TokiSt03	ON	eTKD_Yyksho01.SirCdSeq		=		eVPM_TokiSt03.TokuiSeq		
													AND	eTKD_Yyksho01.SirSitenCdSeq	=		eVPM_TokiSt03.SitenCdSeq	
													AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt03.SiyoStaYmd	
																					AND		eVPM_TokiSt03.SiyoEndYmd	
		-- ＜検索＞運行日テーブルの行き先マップコードＳＥＱより取得(eVPM_Basyo01)
		LEFT JOIN	VPM_Basyo	AS	eVPM_Basyo01	ON	eTKD_Unkobi01.IkMapCdSeq	=		eVPM_Basyo01.BasyoMapCdSeq	
													AND	eVPM_Basyo01.SiyoKbn		=		1							
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb06)
		LEFT JOIN	VPM_CodeKb	AS	eVPM_CodeKb06	ON	eVPM_Basyo01.BasyoKenCdSeq	=		eVPM_CodeKb06.CodeKbnSeq
													AND eVPM_CodeKb06.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_CodeKb06.SiyoKbn		=		1							
		-- ＜検索＞運行日テーブルの配車地コードＳＥＱより取得(eVPM_Haichi01)
		LEFT JOIN	VPM_Haichi	AS	eVPM_Haichi01	ON	eTKD_Unkobi01.HaiSCdSeq		=		eVPM_Haichi01.HaiSCdSeq		
													AND	eVPM_Haichi01.SiyoKbn		=		1							
		-- ＜検索＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb07)
		LEFT JOIN	VPM_CodeKb	AS	eVPM_CodeKb07	ON	eVPM_Haichi01.BunruiCdSeq	=		eVPM_CodeKb07.CodeKbnSeq	
													AND eVPM_CodeKb07.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_CodeKb07.SiyoKbn		=		1													
		-- ＜検索＞運行日テーブルの発生地マップコードＳＥＱより取得(eVPM_Basyo02)
		LEFT JOIN	VPM_Basyo	AS	eVPM_Basyo02	ON	eTKD_Unkobi01.HasMapCdSeq	=		eVPM_Basyo02.BasyoMapCdSeq	
													AND	eVPM_Basyo02.SiyoKbn		=		1							
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb09)
		LEFT JOIN	VPM_CodeKb	AS	eVPM_CodeKb09	ON	eVPM_Basyo02.BasyoKenCdSeq	=		eVPM_CodeKb09.CodeKbnSeq
													AND eVPM_CodeKb09.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_CodeKb09.SiyoKbn		=		1							
		-- ＜検索＞運行日テーブルのエリアマップコードＳＥＱより取得(eVPM_Basyo03)
		LEFT JOIN	VPM_Basyo	AS	eVPM_Basyo03	ON	eTKD_Unkobi01.AreaMapSeq	=		eVPM_Basyo03.BasyoMapCdSeq	
													AND	eVPM_Basyo03.SiyoKbn		=		1							
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb10)
		LEFT JOIN	VPM_CodeKb	AS	eVPM_CodeKb10	ON	eVPM_Basyo03.BasyoKenCdSeq	=		eVPM_CodeKb10.CodeKbnSeq
													AND eVPM_CodeKb10.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_CodeKb10.SiyoKbn		=		1							
		-- ＜検索＞運行日テーブルの乗客区分コードＳＥＱより取得(eVPM_JyoKya01)
		LEFT JOIN	VPM_JyoKya	AS	eVPM_JyoKya01	ON	eTKD_Unkobi01.JyoKyakuCdSeq	=		eVPM_JyoKya01.JyoKyakuCdSeq
													AND eVPM_JyoKya01.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_JyoKya01.SiyoKbn		=		1							
		-- ＜検索＞乗客区分マスタの団体区分コードＳＥＱより取得(eVPM_CodeKb11)
		LEFT JOIN	VPM_CodeKb	AS	eVPM_CodeKb11	ON	eVPM_JyoKya01.DantaiCdSeq	=		eVPM_CodeKb11.CodeKbnSeq
													AND eVPM_CodeKb11.TenantCdSeq	=		@TenantCdSeq
													AND	eVPM_CodeKb11.SiyoKbn		=		1											
		-- ＜検索/取得＞(eVPM_YoyKbn01)
		LEFT JOIN	VPM_YoyKbn	AS	eVPM_YoyKbn01	ON	eTKD_Yyksho01.YoyaKbnSeq	=		eVPM_YoyKbn01.YoyaKbnSeq
WHERE
		eTKD_Yyksho01.YoyaSyu = 1
		AND eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq                                                                                                                                 -- ログインユーザの会社のCompanyCdSeq
		AND (@StartDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd >= @StartDispatchDate)                                                                                            -- 配車日　開始
		AND (@EndDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd <= @EndDispatchDate)                                                                                                -- 配車日　終了
		AND (@StartArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd >= @StartArrivalDate)                                                                                               -- 到着日　開始
		AND (@EndArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd <= @EndArrivalDate)                                                                                                   -- 到着日　終了
		AND (@StartReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd >= @StartReservationDate)                                                                                       -- 予約日　開始
		AND (@EndReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd<= @EndReservationDate)                                                                                            -- 予約日　終了
		AND (@StartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @StartReceiptNumber)                                                                                            -- 予約番号　開始
		AND	(@EndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @EndReceiptNumber)                                                                                                -- 予約番号　終了
		AND (@StartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @StartReservationClassification)                                                                  -- 予約区分　開始	
		AND	(@EndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @EndReservationClassification)                                                                      -- 予約区分　終了
		AND (@StartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @StartServicePerson)                                                                                           -- 営業担当者　開始	
		AND	(@EndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @EndServicePerson)                                                                                               -- 営業担当者　終了
		AND (@StartRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd >= @StartRegistrationOffice)                                                                                -- 営業所　開始
		AND	(@EndRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd <= @EndRegistrationOffice)                                                                                    -- 営業所　終了
		AND (@StartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @StartInputPerson)                                                                                               -- 入力担当者　開始
		AND	(@EndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @EndInputPerson)                                                                                                   -- 入力担当者　終了
		AND (@StartCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') + FORMAT(eVPM_TokiSt01.SitenCd,'0000') >= @StartCustomer)       -- 得意先コード　開始
		AND	(@EndCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd,'000') + FORMAT(eVPM_Tokisk01.TokuiCd,'0000') + FORMAT(eVPM_TokiSt01.SitenCd,'0000') <= @EndCustomer)           -- 得意先コード　終了
		AND (@StartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') + FORMAT(eVPM_Tokisk03.TokuiCd,'0000') + FORMAT(eVPM_TokiSt03.SitenCd,'0000') >= @StartSupplier)       -- 仕入先コード　開始
		AND	(@EndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd,'000') +	FORMAT(eVPM_Tokisk03.TokuiCd,'0000') + FORMAT(eVPM_TokiSt03.SitenCd,'0000') <= @EndSupplier)           -- 仕入先コード　終了
		AND (@StartGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn >= @StartGroupClassification)                                                                              -- 団体区分　開始
		AND	(@EndGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn <= @EndGroupClassification)                                                                                  -- 団体区分　終了
		AND (@StartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @StartCustomerTypeClassification)                                                             -- 乗客コード　開始
		AND	(@EndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @EndCustomerTypeClassification)                                                                 -- 乗客コード　終了
		AND (@StartDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @StartDestination)                                                                    -- 行先　開始
		AND	(@EndDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @EndDestination)                                                                        -- 行先　終了
		AND (@StartDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd >= @StartDispatchPlace)                                                                   -- 配車地　開始
		AND	(@EndDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd <= @EndDispatchPlace)                                                                       -- 配車地　終了
		AND (@StartOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @StartOccurrencePlace)                                                            -- 発生地　開始
		AND	(@EndOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @EndOccurrencePlace)                                                                -- 発生地　終了
		AND (@StartArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @StartArea)                                                                                  -- エリア　開始
		AND	(@EndArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @EndArea)                                                                                      -- エリア　終了
		AND (@StartReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd >= @StartReceiptCondition)                                                                                 -- 受付条件　開始
		AND (@EndReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd <= @EndReceiptCondition)                                                                                     -- 受付条件　終了
		AND (@DantaNm IS NULL OR eTKD_Unkobi01.DanTaNm LIKE CONCAT('%', @DantaNm, '%')) 
		AND eTKD_Yyksho01.UkeNo + FORMAT(eTKD_Unkobi01.UnkRen,'00000') >= @StartFilterNo
		AND eTKD_Yyksho01.UkeNo + FORMAT(eTKD_Unkobi01.UnkRen,'00000') <= @EndFilterNo
ORDER BY UkeNo, UnkRen
