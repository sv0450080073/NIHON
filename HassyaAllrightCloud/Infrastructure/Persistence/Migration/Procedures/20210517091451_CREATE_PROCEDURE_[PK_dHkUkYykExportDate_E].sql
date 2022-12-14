USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dHkUkYykExportDate_E]    Script Date: 2021/05/14 17:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	[PK_dHkUkYykExportDate_E]
-- DB-Name		:	運行日テーブル、その他
-- Name			:	運送引受書のデータ取得処理
-- Date			:	2021/02/17
-- Author		:	nhhkieuanh
-- Descriotion	:	運行日テーブルその他のSelect処理
-- 				:	運行日紐付けられる他テーブル情報も取得する
---------------------------------------------------
-- Update		:	
-- Comment		:
----------------------------------------------------

CREATE OR ALTER       PROCEDURE [dbo].[PK_dHkUkYykExportDate_E]
	( 
		@TenantCdSeq		INT		   
	,	@StartDispatchDate	VARCHAR(8)					-- 配車日付開始
	,	@EndDispatchDate	VARCHAR(8)					-- 配車日付終了
	,	@StartArrivalDate	VARCHAR(8)					-- 到着日付開始
	,	@EndArrivalDate		VARCHAR(8)					-- 到着日付終了
	,	@StartReservationDate	VARCHAR(8)				-- 受付日付開始
	,	@EndReservationDate		VARCHAR(8)				-- 受付日付終了		
	,	@YoyaKbnList		VARCHAR(50)					-- 予約区分
	,	@UkeEigCd			VARCHAR(5)					-- 受付営業所コード
	,	@EigSyainCd			VARCHAR(10)					-- 営業社員コード
	,	@InpSyainCd			VARCHAR(10)					-- 入力社員コード	
	,	@TokuiCd			VARCHAR(10)					-- 得意先コード
	,	@SitenCd			VARCHAR(10)					-- 得意先支店コード
	,	@UkeNo				VARCHAR(MAX)					-- 受付番号
	,	@UnkRen				VARCHAR(3)					-- 運行連番
	,	@OutSelect			VARCHAR(1)					-- 出力選択(0:すべて 1:未出力のみ)	
	,	@NenKeiyakuOutFlg	VARCHAR(1)					-- 年間契約出力フラグ(0:出力する 1:出力しない)	
	,	@OutputUnit			VARCHAR(1)					-- 出力単位(1:予約毎 2:予約車種毎)	
	)
AS 
DECLARE @syshiduke CHAR(08);
BEGIN
	SELECT @syshiduke = FP_tblHiduke FROM FP_DatTim();
	UPDATE TKD_Unkobi SET [UnsoOutputYmd] = @syshiduke
	WHERE UkeNo IN (
		SELECT TKD_Unkobi.UkeNo																																
		FROM TKD_Unkobi																		
		LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Unkobi.UkeNo = JT_Yyksho.UkeNo																		
				AND JT_Yyksho.SiyoKbn = 1																
				AND JT_Yyksho.TenantCdSeq = @TenantCdSeq									  														
		LEFT JOIN VPM_Tokisk AS JM_Tokisk ON JT_Yyksho.TokuiSeq = JM_Tokisk.TokuiSeq																		
				AND JM_Tokisk.TenantCdSeq = @TenantCdSeq								  
				AND JT_Yyksho.SeiTaiYmd >= JM_Tokisk.SiyoStaYmd																		
				AND JT_Yyksho.SeiTaiYmd <= JM_Tokisk.SiyoEndYmd																		
		LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq																		
				AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq																		
				AND JT_Yyksho.SeiTaiYmd >= JM_TokiSt.SiyoStaYmd																		
				AND JT_Yyksho.SeiTaiYmd <= JM_TokiSt.SiyoEndYmd																																																																																	
		LEFT JOIN TKD_UnkobiExp AS JT_UnkobiExp ON TKD_Unkobi.UkeNo = JT_UnkobiExp.UkeNo																		
				AND TKD_Unkobi.UnkRen = JT_UnkobiExp.UnkRen																																		
		LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq																		
				AND JM_UkeEigyos.SiyoKbn = 1																																			
		LEFT JOIN VPM_Syain AS JM_EigTan ON JT_Yyksho.EigTanCdSeq = JM_EigTan.SyainCdSeq																		
		LEFT JOIN VPM_Syain AS JM_InTan ON JT_Yyksho.InTanCdSeq = JM_InTan.SyainCdSeq																		
		LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq																		
				AND JT_Yyksho.SiyoKbn = 1																		
				AND JT_Yyksho.TenantCdSeq = @TenantCdSeq								  	
		WHERE JT_Yyksho.YoyaSyu = 1							
			AND TKD_Unkobi.SiyoKbn = 1 						
			-- IF 日付 = 1-配車日付		||-- IF 日付 = 2-到着日付	|| -- IF 日付 = 3-受付日付		
			AND (@StartDispatchDate IS NULL OR @StartDispatchDate = '' OR TKD_Unkobi.HaiSYmd >= @StartDispatchDate)              -- 配車日　開始
			AND (@EndDispatchDate IS NULL OR @EndDispatchDate = '' OR TKD_Unkobi.HaiSYmd <= @EndDispatchDate)                    -- 配車日　終了
			AND (@StartArrivalDate IS NULL OR @StartArrivalDate = '' OR TKD_Unkobi.TouYmd >= @StartArrivalDate)                  -- 到着日　開始
			AND (@EndArrivalDate IS NULL OR @EndArrivalDate = '' OR TKD_Unkobi.TouYmd <= @EndArrivalDate)                        -- 到着日　終了
			AND (@StartReservationDate IS NULL OR @StartReservationDate = '' OR UkeYmd >= @StartReservationDate)                 -- 予約日　開始
			AND (@EndReservationDate IS NULL OR @EndReservationDate = '' OR UkeYmd<= @EndReservationDate)                        -- 予約日　終了
			-- 受付営業所：						
			AND (@UkeEigCd IS NULL OR @UkeEigCd = '' OR JM_UkeEigyos.EigyoCd = @UkeEigCd)
			-- 営業担当者：						
			AND (@EigSyainCd IS NULL OR @EigSyainCd = '' OR JM_EigTan.SyainCd = @EigSyainCd)					
			-- 入力担当者：						
			AND (@InpSyainCd IS NULL  OR @InpSyainCd = '' OR JM_InTan.SyainCd = @InpSyainCd)		
			-- 受付番号：						
			AND (@UkeNo IS NULL OR @UkeNo = '' OR TKD_Unkobi.UkeNo IN (select value from string_split(@UkeNo, ',')))
			-- 予約区分：						
			AND (@YoyaKbnList IS NULL OR @YoyaKbnList = '' OR JM_YoyKbn.YoyaKbn IN (select value from string_split(@YoyaKbnList, ',')))	
			-- 得意先：						
			AND (@TokuiCd IS NULL OR @TokuiCd = '' OR JM_Tokisk.TokuiCd = @TokuiCd)
			AND (@SitenCd IS NULL OR @SitenCd = '' OR JM_TokiSt.SitenCd = @SitenCd)
			-- IF 出力選択 <> 0  (0:すべて 1:未出力のみ)						
			AND	(@OutSelect = 0 OR @OutSelect = '' OR ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem,0,9),'') = '')
			-- IF 年間契約出力フラグ <> 0 (0:出力する 1:出力しない)						
			AND	(@NenKeiyakuOutFlg = 0 OR @NenKeiyakuOutFlg = '' OR ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem,78,1),0) = 0)
			-- 運行日テーブルの連携						
			AND (@UnkRen IS NULL OR @UnkRen = '' OR TKD_Unkobi.UnkRen = @UnkRen)
	)
END
