USE [HOC_Kashikiri]
GO
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
CREATE OR ALTER  PROCEDURE [dbo].[PK_dVehicle_R]
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
		,	@OffSet int
		,	@Fetch int
	 	-- Output
		,	@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS
	DECLARE @strSQL VARCHAR(MAX)

	-- Processing
    BEGIN
			SET	@strSQL             =
				-- ＜取得＞付帯積込品情報を集計(eTKD_FutTum00)
			+	'   WITH                                                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   eTKD_FutTum00 AS (                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT TKD_FutTum.UkeNo								AS		UkeNo,                                                '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_FutTum.UnkRen								AS		UnkRen,                                               '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.TeiDanNo								AS		TeiDanNo,                                             '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.BunkRen								AS		BunkRen,                                              '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_FutTum.FutTumKbn							AS		FutTumKbn,                                            '
			+   CHAR(13)+CHAR(10)	+	'   		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   			ELSE VPM_Futai.FutGuiKbn                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   			END AS FutGuiKbn,                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   		SUM(TKD_MFutTu.UriGakKin)						AS		UriGakKin_S,                                          '
			+   CHAR(13)+CHAR(10)	+	'   		SUM(TKD_MFutTu.SyaRyoSyo)						AS		SyaRyoSyo_S,                                          '
			+   CHAR(13)+CHAR(10)	+	'   		SUM(TKD_MFutTu.SyaRyoTes)						AS		SyaRyoTes_S                                           '
			+   CHAR(13)+CHAR(10)	+	'   	FROM TKD_FutTum                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN VPM_Futai                                                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		AND VPM_Futai.SiyoKbn = 1                                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   	JOIN TKD_MFutTu                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_FutTum.UkeNo = TKD_MFutTu.UkeNo                                                                        '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_FutTum.UnkRen = TKD_MFutTu.UnkRen                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_FutTum.FutTumKbn = TKD_MFutTu.FutTumKbn                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_FutTum.FutTumRen = TKD_MFutTu.FutTumRen                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_MFutTu.SiyoKbn = 1                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	GROUP BY                                                                                                          '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_FutTum.UkeNo,                                                                                             '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_FutTum.UnkRen,                                                                                            '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.TeiDanNo,                                                                                          '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.BunkRen,                                                                                           '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_FutTum.FutTumKbn,                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   			ELSE VPM_Futai.FutGuiKbn                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   			END                                                                                                       '
			+   CHAR(13)+CHAR(10)	+	'   ),                                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   eTKD_YFutTu10 AS (                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT TKD_YMFuTu.*,                                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		VPM_Futai.FutGuiKbn								AS		SeiFutSyu,                                            '
			+   CHAR(13)+CHAR(10)	+	'   		ISNULL(TKD_Yousha.JitaFlg,1)					AS		JitaFlg                                               '
			+   CHAR(13)+CHAR(10)	+	'   	FROM TKD_YMFuTu                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN TKD_YFutTu                                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_YMFuTu.UkeNo = TKD_YFutTu.UkeNo                                                                        '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_YMFuTu.UnkRen = TKD_YFutTu.UnkRen                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_YMFuTu.YouTblSeq = TKD_YFutTu.YouTblSeq                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_YMFuTu.FutTumKbn = TKD_YFutTu.FutTumKbn                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_YMFuTu.YouFutTumRen = TKD_YFutTu.YouFutTumRen                                                         '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN VPM_Futai                                                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_YFutTu.FutTumCdSeq = VPM_Futai.FutaiCdSeq                                                              '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN TKD_Yousha                                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_YMFuTu.UkeNo = TKD_Yousha.UkeNo                                                                        '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_YMFuTu.UnkRen = TKD_Yousha.UnkRen                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_YMFuTu.YouTblSeq = TKD_Yousha.YouTblSeq                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_Yousha.SiyoKbn = 1                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	WHERE TKD_YMFuTu.SiyoKbn = 1                                                                                      '
			+   CHAR(13)+CHAR(10)	+	'   ),                                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   eTKD_YFutTu00 AS (                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT eTKD_YFutTu10.UkeNo								AS		UkeNo,                                            '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.UnkRen							AS		UnkRen,                                               '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.FutTumKbn							AS		FutTumKbn,                                            '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.SeiFutSyu							AS		SeiFutSyu,                                            '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.TeiDanNo							AS		TeiDanNo,                                             '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.BunkRen							AS		BunkRen,                                              '
			+   CHAR(13)+CHAR(10)	+	'   		SUM(eTKD_YFutTu10.HaseiKin	)					AS		HaseiKin_S,                                           '
			+   CHAR(13)+CHAR(10)	+	'   		SUM(eTKD_YFutTu10.SyaRyoSyo	)					AS		SyaRyoSyo_S,                                          '
			+   CHAR(13)+CHAR(10)	+	'   		SUM(eTKD_YFutTu10.SyaRyoTes	)					AS		SyaRyoTes_S                                           '
			+   CHAR(13)+CHAR(10)	+	'   	FROM eTKD_YFutTu10                                                                                                '
			+   CHAR(13)+CHAR(10)	+	'   	WHERE eTKD_YFutTu10.JitaFlg	=		0                                                                             '
			+   CHAR(13)+CHAR(10)	+	'   	GROUP BY eTKD_YFutTu10.UkeNo,                                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.UnkRen,                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.FutTumKbn,                                                                                      '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.SeiFutSyu,                                                                                      '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.TeiDanNo,                                                                                       '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_YFutTu10.BunkRen                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   ),                                                                                                                    '

			    SET @strSQL = @strSQL
			+   CHAR(13)+CHAR(10)	+	'   eTKD_FutTumCnt AS (                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT TKD_MFutTu.UkeNo AS UkeNo,                                                                                 '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.UnkRen AS UnkRen,                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.TeiDanNo AS TeiDanNo,                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.BunkRen AS BunkRen,                                                                                '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.FutTumKbn AS FutTumKbn,                                                                            '
			+   CHAR(13)+CHAR(10)	+	'   		COUNT(TKD_MFutTu.UkeNo) AS CntInS_FutTum                                                                      '
			+   CHAR(13)+CHAR(10)	+	'   	FROM TKD_MFutTu                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   	WHERE TKD_MFutTu.SiyoKbn = 1                                                                                      '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_MFutTu.Suryo <> 0                                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   	GROUP BY TKD_MFutTu.UkeNo,                                                                                        '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.UnkRen,                                                                                            '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.TeiDanNo,                                                                                          '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.BunkRen,                                                                                           '
			+   CHAR(13)+CHAR(10)	+	'   		TKD_MFutTu.FutTumKbn                                                                                          '
			+   CHAR(13)+CHAR(10)	+	'   ),                                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   eTKD_Haisha00 AS (                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT TKD_Haisha.*,                                                                                              '
			IF(@CompanyCdSeq IS NOT NULL)
	          BEGIN
		        SET @strSQL = @strSQL
			        +   CHAR(13)+CHAR(10)	+	CONCAT('CASE WHEN ISNULL(VPM_Eigyos.CompanyCdSeq, 0) = ', @CompanyCdSeq)
	          END
			 SET @strSQL = @strSQL

			+   CHAR(13)+CHAR(10)	+	'   				THEN                                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   					CASE WHEN TKD_Haisha.YouTblSeq <> 0 AND	ISNULL(TKD_Yousha.JitaFlg, 1) = 0 THEN ''自社受傭車''      '
			+   CHAR(13)+CHAR(10)	+	'   						ELSE ''自社受自社''                                                                            '
			+   CHAR(13)+CHAR(10)	+	'   						END                                                                                           '
			+   CHAR(13)+CHAR(10)	+	'   			ELSE CASE WHEN TKD_Haisha.YouTblSeq = 0 THEN ''自社受自社''                                                '
			+   CHAR(13)+CHAR(10)	+	'   						ELSE ''他社受自社''                                                                            '
			+   CHAR(13)+CHAR(10)	+	'   						END                                                                                           '
			+   CHAR(13)+CHAR(10)	+	'   			END AS JiTaAtukaiFlg,                                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   		CASE WHEN TKD_Haisha.HaiSKbn =	2 THEN	TKD_Haisha.HaiSSryCdSeq                                               '
			+   CHAR(13)+CHAR(10)	+	'   			WHEN TKD_Haisha.HaiSKbn <> 2 AND TKD_Haisha.KSKbn = 2 THEN TKD_Haisha.KSSyaRSeq                           '
			+   CHAR(13)+CHAR(10)	+	'   			ELSE 0                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   			END AS HsKsSryCdSeq                                                                                       '
			+   CHAR(13)+CHAR(10)	+	'   	FROM TKD_Haisha                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN TKD_Yyksho                                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		ON		TKD_Haisha.UkeNo		=		TKD_Yyksho.UkeNo                                                      '
			+   CHAR(13)+CHAR(10)	+	'   		AND		TKD_Yyksho.SiyoKbn		=		1                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN	VPM_Eigyos ON TKD_Yyksho.UkeEigCdSeq = VPM_Eigyos.EigyoCdSeq                                          '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN	TKD_Yousha ON TKD_Haisha.UkeNo = TKD_Yousha.UkeNo                                                     '
			+   CHAR(13)+CHAR(10)	+	'   				AND		TKD_Haisha.UnkRen		=		TKD_Yousha.UnkRen                                             '
			+   CHAR(13)+CHAR(10)	+	'   				AND		TKD_Haisha.YouTblSeq	=		TKD_Yousha.YouTblSeq                                          '
			+   CHAR(13)+CHAR(10)	+	'   				AND		TKD_Yousha.SiyoKbn		=		1                                                             '
			+   CHAR(13)+CHAR(10)	+	'                                                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   ),                                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   eTKD_Haisha10 AS (                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT TKD_Haisha.*,                                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		ISNULL(eTKD_Haiin10.HaiInRen, 0) AS HaiInRen,                                                                 '
			+   CHAR(13)+CHAR(10)	+	'   		ISNULL(eTKD_Haiin10.SyainCdSeq, 0) AS SyainCdSeq,                                                             '
			+   CHAR(13)+CHAR(10)	+	'   		ISNULL(eVPM_Syain02.SyainCd, 0) AS SyainCd,                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   		CASE WHEN TKD_Haisha.HaiSKbn = 2 THEN ISNULL(eVPM_Syain02.SyainNm, '' '')                                     '
			+   CHAR(13)+CHAR(10)	+	'   			WHEN TKD_Haisha.HaiSKbn <> 2 AND TKD_Haisha.KSKbn = 2 THEN ISNULL(eVPM_Syain02.KariSyainNm, '' '')        '
			+   CHAR(13)+CHAR(10)	+	'   			ELSE '' ''                                                                                                '
			+   CHAR(13)+CHAR(10)	+	'   			END AS SyainNm                                                                                            '
			+   CHAR(13)+CHAR(10)	+	'   	FROM TKD_Haisha                                                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN TKD_Yyksho                                                                                              '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_Haisha.UkeNo = TKD_Yyksho.UkeNo                                                                        '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_Yyksho.SiyoKbn = 1                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN TKD_Haiin AS eTKD_Haiin10                                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		ON TKD_Haisha.UkeNo = eTKD_Haiin10.UkeNo                                                                      '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_Haisha.UnkRen = eTKD_Haiin10.UnkRen                                                                   '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_Haisha.TeiDanNo = eTKD_Haiin10.TeiDanNo                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		AND TKD_Haisha.BunkRen = eTKD_Haiin10.BunkRen                                                                 '
			+   CHAR(13)+CHAR(10)	+	'   		AND eTKD_Haiin10.SiyoKbn = 1                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   	LEFT JOIN VPM_Syain AS eVPM_Syain02                                                                               '
			+   CHAR(13)+CHAR(10)	+	'   		ON eTKD_Haiin10.SyainCdSeq = eVPM_Syain02.SyainCdSeq                                                          '
			+   CHAR(13)+CHAR(10)	+	'   ),                                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   eTKD_Haisha20 AS (                                                                                                    '
			+   CHAR(13)+CHAR(10)	+	'   	SELECT	DISTINCT                                                                                                  '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.UkeNo								AS		UkeNo,                                                '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.UnkRen							AS		UnkRen,                                               '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.TeiDanNo							AS		TeiDanNo,                                             '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.BunkRen							AS		BunkRen                                               '
			+   CHAR(13)+CHAR(10)	+	'   	FROM eTKD_Haisha10                                                                                                '
			+   CHAR(13)+CHAR(10)	+	'   	GROUP BY eTKD_Haisha10.UkeNo,                                                                                     '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.UnkRen,                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.TeiDanNo,                                                                                       '
			+   CHAR(13)+CHAR(10)	+	'   		eTKD_Haisha10.BunkRen                                                                                         '
			+   CHAR(13)+CHAR(10)	+	'   )                                                                                                                     '
			+   CHAR(13)+CHAR(10)	+	'                                                                                                                         '
			IF (@OffSet IS NULL)
				BEGIN
					SET @strSQL = @strSQL
					+   CHAR(13)+CHAR(10)	+	'SELECT COUNT(*) AS count'
				END
			ELSE
				BEGIN
				    SET @strSQL = @strSQL
					+   CHAR(13)+CHAR(10)	+	'   SELECT ISNULL(TKD_Yyksho.UkeNo				,0		)	AS		UkeNo,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Unkobi01.UnkRen				,0		)	AS		UnkRen,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.TeiDanNo			,0		)	AS		TeiDanNo,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.BunkRen			,0		)	AS		BunkRen,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.KaknKais				,0		)	AS		KaknKais,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.KaktYmd				,'' ''	)	AS		KaktYmd,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.NyuKinKbn				,0		)	AS		NyuKinKbn,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.TokuiSeq				,0		)	AS		TokuiSeq,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Tokisk01.TokuiCd			,0		)	AS		TokuiCd,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.CanFuYmd				,'' '')		AS		CanFuYmd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.CanFuRiy				,'' '')		AS		CanFuRiy,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Syain03.SyainCd				,0)			AS		CanFutanCd,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Syain03.SyainNm				,'' '')		AS		CanFutanNm,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Gyosya02.GyosyaCd			,'' ''	)	AS		GyosyaCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Gyosya02.GyosyaNm			,'' ''	)	AS		GyosyaNm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Tokisk01.TokuiNm			,'' ''	)	AS		TokuiNm,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Tokisk01.RyakuNm			,'' ''	)	AS		TokuiRyakuNm,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.SitenCdSeq			,0		)	AS		SitenCdSeq,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt01.SitenCd			,0		)	AS		SitenCd,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt01.SitenNm			,'' ''	)	AS		SitenNm,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt01.RyakuNm			,'' ''	)	AS		SitenRyakuNm,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt02.TesKbn				,0		)	AS		SeiSitTesKbn,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt02.TesKbnFut			,0		)	AS		SeiSitTesKbnFut,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt02.TesKbnGui			,0		)	AS		SeiSitTesKbnGui,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.TokuiTanNm			,'' ''	)	AS		TokuiTanNm,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Unkobi01.DanTaNm			,'' ''	)	AS		DanTaNm,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.DanTaNm2			,'' ''	)	AS		DanTaNm2,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.IkNm				,'' ''	)	AS		IkNm,                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiSYmd			,'' ''	)	AS		HaiSYmd,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiSTime			,'' ''	)	AS		HaiSTime,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiSNm				,'' ''	)	AS		HaiSNm,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiSJyuS1			,'' ''	)	AS		HaiSJyuS1,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiSJyuS2			,'' ''	)	AS		HaiSJyuS2,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.TouYmd				,'' ''	)	AS		TouYmd,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.TouChTime			,'' ''	)	AS		TouChTime,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.TouNm				,'' ''	)	AS		TouNm,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.TouJyusyo1			,'' ''	)	AS		TouJyusyo1,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.TouJyusyo2			,'' ''	)	AS		TouJyusyo2,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.DrvJin				,0		)	AS		DrvJin,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.GuiSu				,0		)	AS		GuiSu,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.SyuPaTime			,'' ''	)	AS		SyuPaTime,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HsKsSryCdSeq		,0		)	AS		SyaryoCdSeq,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos01.EigyoCd			,0		)	AS		SyaryoEigCd,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos01.EigyoNm			,'' ''	)	AS		SyaryoEigyoNm,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos01.RyakuNm			,'' ''	)	AS		SyaryoEigRyakuNm,                                     '
					+   CHAR(13)+CHAR(10)	+	'   	CASE WHEN eTKD_Haisha01.HaiSKbn = 2 THEN ISNULL(eVPM_SyaRyo01.SyaRyoNm ,'' '')                                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.HaiSKbn <> 2 AND eTKD_Haisha01.KSKbn =	2 THEN ISNULL(eVPM_SyaRyo01.KariSyaRyoNm, '' '')  '
					+   CHAR(13)+CHAR(10)	+	'   		ELSE '' ''	                                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   		END	AS SyaRyoNm,                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_SyaRyo01.SyaRyoCd			,'' ''	)	AS		SyaRyoCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.GoSya				,'' ''	)	AS		GoSya,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Yousha01.YouCdSeq			,0		)	AS		YouCdSeq,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Tokisk02.TokuiCd			,0		)	AS		YouCd,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Gyosya04.GyosyaCd			,0		)	AS		YouGyoCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Gyosya04.GyosyaNm			,0		)	AS		YouGyoNm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Tokisk02.TokuiNm			,0		)	AS		YouNm,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Tokisk02.RyakuNm			,'' ''	)	AS		YouRyakuNm,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Yousha01.YouSitCdSeq		,0		)	AS		YouSitCdSeq,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt03.SitenCd			,0		)	AS		YouSitCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt03.SitenNm			,0		)	AS		YouSitNm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_TokiSt03.RyakuNm			,'' ''	)	AS		YouSitCdRyakuNm,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN eTKD_Haisha01.SyaRyoUnc                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN eTKD_Haisha01.SyaRyoUnc                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN eTKD_Haisha01.YoushaUnc                                 '
					+   CHAR(13)+CHAR(10)	+	'   	END AS SyaRyoUnc,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN eTKD_Haisha01.SyaRyoSyo                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN eTKD_Haisha01.SyaRyoSyo                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN eTKD_Haisha01.YoushaSyo                                 '
					+   CHAR(13)+CHAR(10)	+	'   	END AS SyaRyoSyo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN eTKD_Haisha01.SyaRyoTes                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN eTKD_Haisha01.SyaRyoTes                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN eTKD_Haisha01.YoushaTes                                 '
					+   CHAR(13)+CHAR(10)	+	'   	END AS SyaRyoTes,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'                                                                                                                         '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN eTKD_Haisha01.SyaRyoUnc                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN eTKD_Haisha01.YoushaUnc                                 '
					+   CHAR(13)+CHAR(10)	+	'   	END AS JiSyaRyoUnc,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN eTKD_Haisha01.SyaRyoSyo                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN eTKD_Haisha01.YoushaSyo                                 '
					+   CHAR(13)+CHAR(10)	+	'   	END AS JiSyaRyoSyo,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN eTKD_Haisha01.SyaRyoTes                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN eTKD_Haisha01.YoushaTes                                 '
					+   CHAR(13)+CHAR(10)	+	'   	END AS JiSyaRyoTes,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'                                                                                                                         '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN eTKD_Haisha01.YoushaUnc                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YoushaUnc,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN eTKD_Haisha01.YoushaSyo                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YoushaSyo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN eTKD_Haisha01.YoushaTes                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YoushaTes,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum01.UriGakKin_S		,0		)	AS		Gui_UriGakKin_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum01.SyaRyoSyo_S		,0		)	AS		Gui_SyaRyoSyo_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum01.SyaRyoTes_S		,0		)	AS		Gui_SyaRyoTes_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum02.UriGakKin_S		,0		)	AS		Tum_UriGakKin_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum02.SyaRyoSyo_S		,0		)	AS		Tum_SyaRyoSyo_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum02.SyaRyoTes_S		,0		)	AS		Tum_SyaRyoTes_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum03.UriGakKin_S		,0		)	AS		Fut_UriGakKin_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum03.SyaRyoSyo_S		,0		)	AS		Fut_SyaRyoSyo_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum03.SyaRyoTes_S		,0		)	AS		Fut_SyaRyoTes_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum04.UriGakKin_S		,0		)	AS		Tuk_UriGakKin_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum04.SyaRyoSyo_S		,0		)	AS		Tuk_SyaRyoSyo_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum04.SyaRyoTes_S		,0		)	AS		Tuk_SyaRyoTes_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum05.UriGakKin_S		,0		)	AS		Teh_UriGakKin_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum05.SyaRyoSyo_S		,0		)	AS		Teh_SyaRyoSyo_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum05.SyaRyoTes_S		,0		)	AS		Teh_SyaRyoTes_S,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu01.HaseiKin_S, 0)                     '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YGui_HaseiKin_S,                                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu01.SyaRyoSyo_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YGui_SyaRyoSyo_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu01.SyaRyoTes_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YGui_SyaRyoTes_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu02.HaseiKin_S, 0)                     '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Ytum_HaseiKin_S,                                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu02.SyaRyoSyo_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Ytum_SyaRyoSyo_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu02.SyaRyoTes_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Ytum_SyaRyoTes_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu04.HaseiKin_S, 0)                     '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Yfut_HaseiKin_S,                                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu04.SyaRyoSyo_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Yfut_SyaRyoSyo_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu04.SyaRyoTes_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Yfut_SyaRyoTes_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu05.HaseiKin_S, 0)                     '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Ytuk_HaseiKin_S,                                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu05.SyaRyoSyo_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Ytuk_SyaRyoSyo_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu05.SyaRyoTes_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Ytuk_SyaRyoTes_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu06.HaseiKin_S, 0)                     '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Yteh_HaseiKin_S,                                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu06.SyaRyoSyo_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Yteh_SyaRyoSyo_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN ISNULL(eTKD_YFutTu06.SyaRyoTes_S, 0)                    '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 0                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS Yteh_SyaRyoTes_S,                                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.JyoSyaJin			,0		)	AS		JyoSyaJin,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.PlusJin			,0		)	AS		PlusJin,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.SyuKoYmd			,'' ''	)	AS		SyuKoYmd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.SyuKoTime			,'' ''	)	AS		SyuKoTime,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.SyuEigCdSeq		,0		)	AS		SyuEigCdSeq,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos03.EigyoCd			,0		)	AS		SyuEigCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos03.EigyoNm			,0		)	AS		SyuEigNm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos03.RyakuNm			,'' ''	)	AS		SyuEigRyakuNm,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.KikYmd				,'' ''	)	AS		KikYmd,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.KikTime			,'' ''	)	AS		KikTime,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.KikEigSeq			,0		)	AS		KikEigCdSeq,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos04.EigyoCd			,0		)	AS		KikEigCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos04.EigyoNm			,0		)	AS		KikEigNm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos04.RyakuNm			,'' ''	)	AS		KikEigRyakuNm,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.JisaIPKm			,0		)	AS		JisaIPKm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.JisaKSKm			,0		)	AS		JisaKSKm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.KisoIPkm			,0		)	AS		KisoIPkm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.KisoKOKm			,0		)	AS		KisoKOKm,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.OthKm				,0		)	AS		OthKm,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.NipJyoSyaJin		,0		)	AS		NipJyoSyaJin,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.NipPlusJin			,0		)	AS		NipPlusJin,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_SyaRyo03.NenryoCd1Seq		,0		)	AS		NenryoCd1Seq,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb01.CodeKbn			,'' ''	)	AS		NenryoCd1,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb01.CodeKbnNm			,'' ''	)	AS		NenryoName1,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb01.RyakuNm			,'' ''	)	AS		NenryoRyak1,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_SyaRyo03.NenryoCd2Seq		,0		)	AS		NenryoCd2Seq,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb02.CodeKbn			,'' ''	)	AS		NenryoCd2,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb02.CodeKbnNm			,'' ''	)	AS		NenryoName2,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb02.RyakuNm			,'' ''	)	AS		NenryoRyak2,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_SyaRyo03.NenryoCd3Seq		,0		)	AS		NenryoCd3Seq,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb03.CodeKbn			,'' ''	)	AS		NenryoCd3,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb03.CodeKbnNm			,'' ''	)	AS		NenryoName3,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb03.RyakuNm			,'' ''	)	AS		NenryoRyak3,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.UkeEigCdSeq			,0		)	AS		UkeEigCdSeq,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos05.EigyoCd			,0		)	AS		UkeEigCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos05.EigyoNm			,'' ''  )		AS		UkeEigNm,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Eigyos05.RyakuNm			,'' ''	)	AS		UkeEigRyakuNm,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.EigTanCdSeq			,0		)	AS		EigTanCdSeq,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Syain01.SyainCd				,'' ''	)	AS		EigTanSyainCd,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Syain01.SyainNm				,'' ''	)	AS		EigTanSyainNm,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.InTanCdSeq			,0		)	AS		InTanCdSeq,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Syain02.SyainCd				,'' ''	)	AS		InputTanSyainCd,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_Syain02.SyainNm				,'' ''	)	AS		InputTanSyainNm,                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_YoyKbn01.YoyaKbn			,0		)	AS		YoyaKbn,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_YoyKbn01.YoyaKbnNm			,'' ''	)	AS		YoyaKbnNm,                                            '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.UkeYmd				,'' ''	)	AS		UkeYmd,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Koteik01.CntInS_Kotei		,0		)	AS		CntInS_Kotei,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Tehai01.CntInS_Tehai		,0		)	AS		CntInS_Tehai,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum06.CntInS_FutTum		,0		)	AS		CntInS_Tum,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_FutTum07.CntInS_FutTum		,0		)	AS		CntInS_Fut,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Biko01.CntInS_Biko			,0		)	AS		CntInS_Biko,                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.NCouKbn				,0		)	AS		NCouKbn,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.SihKbn				,0		)	AS		SihKbn,                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(TKD_Yyksho.SCouKbn				,0		)	AS		SCouKbn,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.KSKbn				,0		)	AS		KSKbn,                                                '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.KHinKbn			,0		)	AS		KHinKbn,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiSKbn			,0		)	AS		HaiSKbn,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.HaiIKbn			,0		)	AS		HaiIKbn,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.GuiWNin			,0		)	AS		GuiWNin,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.NippoKbn			,0		)	AS		NippoKbn,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.SyaSyuCd, 0)                                      '
					+   CHAR(13)+CHAR(10)	+	'   		ELSE ISNULL(eVPM_SyaSyu01.SyaSyuCd, 0)                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   	END AS SyaSyuCd,                                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.SyaSyuNm, 0)                                      '
					+   CHAR(13)+CHAR(10)	+	'   		ELSE ISNULL(eVPM_SyaSyu01.SyaSyuNm, 0)                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   	END AS SyaSyuNm,                                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_SyaSyu02.KataKbn, 0)                                       '
					+   CHAR(13)+CHAR(10)	+	'   		ELSE ISNULL(eVPM_SyaSyu01.KataKbn, 0)                                                                         '
					+   CHAR(13)+CHAR(10)	+	'   	END AS KataKbn,                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.YouTblSeq <>	0 THEN ISNULL(eVPM_CodeKb13.RyakuNm, 0)                                       '
					+   CHAR(13)+CHAR(10)	+	'   		ELSE ISNULL(eVPM_CodeKb12.RyakuNm, 0)                                                                         '
					+   CHAR(13)+CHAR(10)	+	'   	END AS KataNm,                                                                                                    '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.Nenryo1			,0		)	AS		Nenryo1,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.Nenryo2			,0		)	AS		Nenryo2,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Shabni01.Nenryo3			,0		)	AS		Nenryo3,                                              '
					+   CHAR(13)+CHAR(10)	+	'   	CASE                                                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受自社'' THEN 1                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''自社受傭車'' THEN 2                                                       '
					+   CHAR(13)+CHAR(10)	+	'   		WHEN eTKD_Haisha01.JiTaAtukaiFlg = ''他社受自社'' THEN 1                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	END AS YouKbn,                                                                                                    '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Haisha01.BikoTblSeq			,0		)	AS		BikoTblSeq,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eTKD_Unkobi01.BikoTblSeq			,0		)	AS		BikoTblSeq_Unk,                                       '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(JM_UkeJyKbn.RyakuNm				,''''		)	AS		UkeJyKbn,                                         '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(JM_SijJoKbn1.RyakuNm				,''''		)	AS		SijJoKbn1,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(JM_SijJoKbn2.RyakuNm				,''''		)	AS		SijJoKbn2,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(JM_SijJoKbn3.RyakuNm				,''''		)	AS		SijJoKbn3,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(JM_SijJoKbn4.RyakuNm				,''''		)	AS		SijJoKbn4,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(JM_SijJoKbn5.RyakuNm				,''''		)	AS		SijJoKbn5,                                        '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb07.CodeKbn			,'' '' 	)	AS		DantaiCd,                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_CodeKb07.CodeKbnNm			,'' ''	)   AS		DantaiCdNm,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_JyoKya01.JyoKyakuCd			,'' '' 	)	AS		JyoKyakuCd,                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_JyoKya01.JyoKyakuNm			,'' ''  )		AS		JyoKyakuNm,                                       '
					+   CHAR(13)+CHAR(10)	+	'   	ISNULL(eVPM_HenSya01.TenkoNo			,'' ''  )		AS		TenkoNo                                           '
				END
			            SET @strSQL = @strSQL
					+   CHAR(13)+CHAR(10)	+	'   FROM eTKD_Haisha00 AS eTKD_Haisha01                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN TKD_Unkobi AS	eTKD_Unkobi01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Unkobi01.UkeNo = eTKD_Haisha01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Unkobi01.UnkRen = eTKD_Haisha01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Unkobi01.SiyoKbn =	1                                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN TKD_Yyksho                                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = TKD_Yyksho.UkeNo                                                                         '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.TokuiSeq = eVPM_Tokisk01.TokuiSeq                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+	CONCAT('AND eVPM_Tokisk01.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.TokuiSeq = eVPM_TokiSt01.TokuiSeq	                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq                                                                '
					+   CHAR(13)+CHAR(10)	+	'   	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt02.SitenCdSeq                                                        '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk04                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk04.TokuiSeq                                                                '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk04.SiyoStaYmd AND eVPM_Tokisk04.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+	CONCAT('AND eVPM_Tokisk04.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt05                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt05.TokuiSeq                                                                '
					+   CHAR(13)+CHAR(10)	+	'   	AND eVPM_TokiSt01.SeiSitenCdSeq	= eVPM_TokiSt05.SitenCdSeq                                                        '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt05.SiyoStaYmd AND eVPM_TokiSt05.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Tokisk04.GyosyaCdSeq = eVPM_Gyosya03.GyosyaCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos05                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.UkeEigCdSeq = eVPM_Eigyos05.EigyoCdSeq                                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Compny AS	eVPM_Compny01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Eigyos05.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq                                                        '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Syain AS eVPM_Syain01                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.EigTanCdSeq = eVPM_Syain01.SyainCdSeq                                                               '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Syain AS eVPM_Syain02                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.InTanCdSeq = eVPM_Syain02.SyainCdSeq                                                                '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Syain AS eVPM_Syain03                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.CANFUTanSeq = eVPM_Syain03.SyainCdSeq                                                               '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_YoyKbn AS	eVPM_YoyKbn01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq                                                               '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.SirCdSeq = eVPM_Tokisk03.TokuiSeq                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk03.SiyoStaYmd AND	eVPM_Tokisk03.SiyoEndYmd                          '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+	CONCAT('AND eVPM_Tokisk03.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt04                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.SirCdSeq = eVPM_TokiSt04.TokuiSeq                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SirSitenCdSeq = eVPM_TokiSt04.SitenCdSeq                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.SeiTaiYmd BETWEEN	eVPM_TokiSt04.SiyoStaYmd AND eVPM_TokiSt04.SiyoEndYmd                         '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Tokisk03.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb17                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON TKD_Yyksho.SeiKyuKbnSeq = eVPM_CodeKb17.CodeKbnSeq                                                             '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT CodeKbn,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			CodeKbnNm,                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			RyakuNm                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   		FROM VPM_CodeKb                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE CodeSyu = ''SIJJOKBN1''                                                                                 '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+	        CONCAT('AND TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   			AND	SiyoKbn = 1) AS JM_SijJoKbn1                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ON	eTKD_Unkobi01.SijJoKbn1 = CONVERT(tinyint, JM_SijJoKbn1.CodeKbn)                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT CodeKbn,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			CodeKbnNm,                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			RyakuNm                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   		FROM VPM_CodeKb                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE CodeSyu = ''SIJJOKBN2''                                                                                 '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+     	CONCAT('AND TenantCdSeq = ', @TenantCdSeq)								                                               
					+   CHAR(13)+CHAR(10)	+	'   			AND	SiyoKbn = 1) AS JM_SijJoKbn2                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ON	eTKD_Unkobi01.SijJoKbn2 = CONVERT(tinyint, JM_SijJoKbn2.CodeKbn)                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT CodeKbn,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			CodeKbnNm,                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			RyakuNm                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   		FROM VPM_CodeKb                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE CodeSyu = ''SIJJOKBN3''                                                                                 '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+     	CONCAT('AND TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   			AND	SiyoKbn = 1) AS JM_SijJoKbn3                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ON	eTKD_Unkobi01.SijJoKbn4 = CONVERT(tinyint, JM_SijJoKbn3.CodeKbn)                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT CodeKbn,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			CodeKbnNm,                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			RyakuNm                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   		FROM VPM_CodeKb                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE CodeSyu = ''SIJJOKBN4''                                                                                 '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+     	CONCAT('AND TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   			AND	SiyoKbn = 1) AS JM_SijJoKbn4                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ON	eTKD_Unkobi01.SijJoKbn4 = CONVERT(tinyint, JM_SijJoKbn4.CodeKbn)                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT CodeKbn,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			CodeKbnNm,                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			RyakuNm                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   		FROM VPM_CodeKb                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE CodeSyu = ''SIJJOKBN5''                                                                                 '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+	+     	CONCAT('AND TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   			AND	SiyoKbn = 1) AS JM_SijJoKbn5                                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	ON	eTKD_Unkobi01.SijJoKbn5 = CONVERT(tinyint, JM_SijJoKbn5.CodeKbn)                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT CodeKbn,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			CodeKbnNm,                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			RyakuNm                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   		FROM VPM_CodeKb                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE CodeSyu = ''UkeJyKbnCd''                                                                                '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+	+     	CONCAT('AND TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   			AND	SiyoKbn = 1) AS JM_UkeJyKbn                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   	ON	eTKD_Unkobi01.UkeJyKbnCd = CONVERT(tinyint, JM_UkeJyKbn.CodeKbn)                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Basyo AS eVPM_Basyo01                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Unkobi01.HasMapCdSeq = eVPM_Basyo01.BasyoMapCdSeq                                                         '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS eVPM_CodeKb05                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Basyo01.BasyoKenCdSeq = eVPM_CodeKb05.CodeKbnSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Basyo AS eVPM_Basyo02                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Unkobi01.AreaMapSeq	= eVPM_Basyo02.BasyoMapCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS eVPM_CodeKb06                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Basyo02.BasyoKenCdSeq = eVPM_CodeKb06.CodeKbnSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_JyoKya AS	eVPM_JyoKya01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Unkobi01.JyoKyakuCdSeq = eVPM_JyoKya01.JyoKyakuCdSeq                                                      '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+     	CONCAT('AND eVPM_JyoKya01.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb07                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_JyoKya01.DantaiCdSeq = eVPM_CodeKb07.CodeKbnSeq                                                           '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_HenSya AS	eVPM_HenSya01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_HenSya01.SyaRyoCdSeq                                                         '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.SyuKoYmd BETWEEN eVPM_HenSya01.StaYmd AND	eVPM_HenSya01.EndYmd                                  '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_HenSya01.EigyoCdSeq = eVPM_Eigyos01.EigyoCdSeq                                                            '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo01.SyaRyoCdSeq                                                         '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTum00	AS eTKD_FutTum01                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum01.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_FutTum01.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum01.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum01.FutGuiKbn = 5                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum02                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum02.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum02.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum02.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_FutTum02.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum02.FutGuiKbn = 0                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum03                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum03.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum03.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum03.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen =	eTKD_FutTum03.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum03.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum03.FutGuiKbn = 2                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTum00	AS	eTKD_FutTum04                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum04.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum04.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum04.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_FutTum04.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum04.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum04.FutGuiKbn = 3                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTum00	AS eTKD_FutTum05                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum05.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum05.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum05.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_FutTum05.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum05.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum05.FutGuiKbn = 4                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu01                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu01.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu01.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu01.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu01.SeiFutSyu = 5                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu02                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu02.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu02.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu02.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu02.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu02.FutTumKbn = 2                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu03                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu03.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu03.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu03.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu03.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu03.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu03.SeiFutSyu = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu04                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu04.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu04.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu04.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu04.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu04.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu04.SeiFutSyu = 2                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu05                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu05.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu05.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu05.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu05.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu05.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu05.SeiFutSyu = 3                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_YFutTu00 AS eTKD_YFutTu06                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YFutTu06.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YFutTu06.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_YFutTu06.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_YFutTu06.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu06.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YFutTu06.SeiFutSyu = 4                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.SyuEigCdSeq = eVPM_Eigyos03.EigyoCdSeq                                                           '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Compny AS	eVPM_Compny02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Eigyos03.CompanyCdSeq = eVPM_Compny02.CompanyCdSeq                                                        '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos04                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.KikEigSeq = eVPM_Eigyos04.EigyoCdSeq                                                             '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Compny AS	eVPM_Compny03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Eigyos04.CompanyCdSeq = eVPM_Compny03.CompanyCdSeq                                                        '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_SyaRyo AS	eVPM_SyaRyo03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_SyaRyo03.SyaRyoCdSeq                                                         '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_SyaRyo03.NenryoCd1Seq = eVPM_CodeKb01.CodeKbnSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_SyaRyo03.NenryoCd2Seq = eVPM_CodeKb02.CodeKbnSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_SyaRyo03.NenryoCd3Seq = eVPM_CodeKb03.CodeKbnSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_SyaRyo03.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+     	CONCAT('AND eVPM_SyaSyu01.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb12                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_CodeKb12.CodeSyu = ''KATAKBN''                                                                            '
					+   CHAR(13)+CHAR(10)	+	'   	AND CONVERT(VARCHAR(10),eVPM_SyaSyu01.KataKbn) = eVPM_CodeKb12.CodeKbn                                            '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+   CONCAT('AND eVPM_CodeKb12.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Basyo AS eVPM_Basyo03                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.IkMapCdSeq	= eVPM_Basyo03.BasyoMapCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS eVPM_CodeKb08                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Basyo03.BasyoKenCdSeq = eVPM_CodeKb08.CodeKbnSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT UkeNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			UnkRen,                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   			TeiDanNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			BunkRen,                                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   			COUNT(UkeNo) AS CntInS_Kotei                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		FROM TKD_Koteik                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE SiyoKbn = 1                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   		GROUP BY UkeNo,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			UnkRen,                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   			TeiDanNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			BunkRen) AS eTKD_Koteik01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Koteik01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_Koteik01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_Koteik01.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_Koteik01.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT UkeNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			UnkRen,                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   			TeiDanNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			BunkRen,                                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   			COUNT(UkeNo) AS CntInS_Tehai                                                                              '
					+   CHAR(13)+CHAR(10)	+	'   		FROM TKD_Tehai                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE SiyoKbn = 1                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   		GROUP BY UkeNo,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			UnkRen,                                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   			TeiDanNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			BunkRen) AS	eTKD_Tehai01                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Tehai01.UkeNo                                                                       '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_Tehai01.UnkRen                                                                    '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_Tehai01.TeiDanNo                                                                '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_Tehai01.BunkRen                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTumCnt AS	eTKD_FutTum06                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum06.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum06.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum06.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_FutTum06.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum06.FutTumKbn = 2                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN eTKD_FutTumCnt AS	eTKD_FutTum07                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_FutTum07.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_FutTum07.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_FutTum07.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_FutTum07.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_FutTum07.FutTumKbn = 1                                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT UkeNo,                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   			BikoTblSeq,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			COUNT(UkeNo) AS CntInS_Biko                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		FROM TKD_Biko                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE SiyoKbn = 1                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   		GROUP BY UkeNo,                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   			BikoTblSeq ) AS eTKD_Biko01                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Biko01.UkeNo                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BikoTblSeq = eTKD_Biko01.BikoTblSeq                                                             '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Haichi AS	eVPM_Haichi01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq                                                              '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb09                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Haichi01.BunruiCdSeq = eVPM_CodeKb09.CodeKbnSeq                                                           '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Haichi AS	eVPM_Haichi02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.TouCdSeq = eVPM_Haichi02.HaiSCdSeq                                                               '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb10                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Haichi02.BunruiCdSeq = eVPM_CodeKb10.CodeKbnSeq                                                           '
					+   CHAR(13)+CHAR(10)	+	'   JOIN eTKD_Haisha20                                                                                                    '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Haisha20.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_Haisha20.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_Haisha20.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_Haisha20.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_HenSya AS	eVPM_HenSya03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.HsKsSryCdSeq = eVPM_HenSya03.SyaRyoCdSeq                                                         '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.SyuKoYmd BETWEEN	eVPM_HenSya03.StaYmd AND eVPM_HenSya03.EndYmd                                 '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Eigyos AS	eVPM_Eigyos06 ON eVPM_HenSya03.EigyoCdSeq = eVPM_Eigyos06.EigyoCdSeq                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Compny AS	eVPM_Compny04 ON eVPM_Eigyos06.CompanyCdSeq = eVPM_Compny04.CompanyCdSeq                      '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN                                                                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	(                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT TKD_Shabni.UkeNo,                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.UnkRen,                                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.TeiDanNo,                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.BunkRen,                                                                                       '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.JisaIPKm) AS JisaIPKm,                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.JisaKSKm) AS JisaKSKm,                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.KisoIPkm) AS KisoIPkm,                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.KisoKOKm) AS KisoKOKm,                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.OthKm) AS OthKm,                                                                           '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.Nenryo1) AS Nenryo1,                                                                       '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.Nenryo2) AS Nenryo2,                                                                       '
					+   CHAR(13)+CHAR(10)	+	'   			SUM(TKD_Shabni.Nenryo3) AS Nenryo3,                                                                       '
					+   CHAR(13)+CHAR(10)	+	'   			MAX(TKD_Shabni.JyoSyaJin) AS NipJyoSyaJin,                                                                '
					+   CHAR(13)+CHAR(10)	+	'   			MAX(TKD_Shabni.PlusJin) AS NipPlusJin                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   		FROM TKD_Shabni                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE TKD_Shabni.SiyoKbn = 1                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   		GROUP BY TKD_Shabni.UkeNo,                                                                                    '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.UnkRen,                                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.TeiDanNo,                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.BunkRen) AS eTKD_Shabni01                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Shabni01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_Shabni01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_Shabni01.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_Shabni01.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   JOIN (                                                                                                                '
					+   CHAR(13)+CHAR(10)	+	'   		SELECT TKD_Shabni.UkeNo,                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.UnkRen,                                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.TeiDanNo,                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.BunkRen                                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   		FROM TKD_Shabni                                                                                               '
					+   CHAR(13)+CHAR(10)	+	'   		LEFT JOIN VPM_Calend AS eVPM_Calend04                                                                         '
					+   CHAR(13)+CHAR(10)	-- ログインユーザの会社コードＳＥＱ
					+   CHAR(13)+CHAR(10)	+	        CONCAT('ON eVPM_Calend04.CompanyCdSeq = ', @CompanyCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   			AND eVPM_Calend04.CalenSyu = 1                                                                            '
					+   CHAR(13)+CHAR(10)	+	'   			AND TKD_Shabni.UnkYmd = eVPM_Calend04.CalenYmd                                                            '
					+   CHAR(13)+CHAR(10)	+	'   		WHERE TKD_Shabni.SiyoKbn = 1                                                                                  '
					+   CHAR(13)+CHAR(10)	+	'   		GROUP BY                                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.UkeNo,                                                                                         '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.UnkRen,                                                                                        '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.TeiDanNo,                                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   			TKD_Shabni.BunkRen) AS eTKD_Shabni02                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Shabni02.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_Shabni02.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.TeiDanNo = eTKD_Shabni02.TeiDanNo                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.BunkRen = eTKD_Shabni02.BunkRen                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN TKD_Yousha AS eTKD_Yousha01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_Yousha01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_Yousha01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.YouTblSeq = eTKD_Yousha01.YouTblSeq                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Yousha01.SiyoKbn = 1                                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Tokisk AS	eVPM_Tokisk02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Yousha01.YouCdSeq = eVPM_Tokisk02.TokuiSeq                                                                '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Yousha01.HasYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+   CONCAT('AND eVPM_Tokisk02.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_TokiSt AS	eVPM_TokiSt03                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Yousha01.YouCdSeq =	 eVPM_TokiSt03.TokuiSeq                                                               '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Yousha01.YouSitCdSeq = eVPM_TokiSt03.SitenCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Yousha01.HasYmd BETWEEN eVPM_TokiSt03.SiyoStaYmd AND eVPM_TokiSt03.SiyoEndYmd                            '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_Gyosya AS	eVPM_Gyosya04                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya04.GyosyaCdSeq                                                          '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN TKD_YykSyu AS	eTKD_YykSyu01                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_Haisha01.UkeNo = eTKD_YykSyu01.UkeNo                                                                      '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.UnkRen = eTKD_YykSyu01.UnkRen                                                                   '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_Haisha01.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen                                                             '
					+   CHAR(13)+CHAR(10)	+	'   	AND eTKD_YykSyu01.SiyoKbn = 1                                                                                     '
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_SyaSyu AS	eVPM_SyaSyu02                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eTKD_YykSyu01.SyaSyuCdSeq = eVPM_SyaSyu02.SyaSyuCdSeq                                                          '
					                        -- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+   CONCAT('AND eVPM_SyaSyu02.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb13                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	ON eVPM_CodeKb13.CodeSyu = ''KATAKBN''                                                                            '
					+   CHAR(13)+CHAR(10)	+	'   	AND	CONVERT(VARCHAR(10),eVPM_SyaSyu02.KataKbn) = eVPM_CodeKb13.CodeKbn                                            '
					+   CHAR(13)+CHAR(10)	-- ログインしたユーザーのTenantCdSeq
					+   CHAR(13)+CHAR(10)	+   CONCAT('AND eVPM_CodeKb13.TenantCdSeq = ', @TenantCdSeq)
					+   CHAR(13)+CHAR(10)	+	'   WHERE                                                                                                                 '
					+   CHAR(13)+CHAR(10)	+	'   	TKD_Yyksho.SiyoKbn = 1                                                                                            '
					+   CHAR(13)+CHAR(10)	+	'   	AND TKD_Yyksho.YoyaSyu = 1                                                                                        '
					+   CHAR(13)+CHAR(10)	-- ログインユーザの会社のCompanyCdSeq
					+   CHAR(13)+CHAR(10)	+	CONCAT('AND eVPM_Compny01.CompanyCdSeq = ', @CompanyCdSeq)                                                       
			-- 配車日
			IF (@StartDispatchDate IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					 +   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Haisha01.HaiSYmd >= ''', @StartDispatchDate, '''')
			END
			IF (@EndDispatchDate IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					 +   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Haisha01.HaiSYmd <= ''', @EndDispatchDate, '''')
			END

			-- 到着日
			IF (@StartArrivalDate IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					 +   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Haisha01.TouYmd >= ''', @StartArrivalDate, '''')
			END
			IF (@EndArrivalDate IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					 +   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_Haisha01.TouYmd <= ''', @EndArrivalDate, '''')
			END

			-- 予約日
			IF (@StartReservationDate IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					 +   CHAR(13)+CHAR(10)	+ CONCAT('AND TKD_Yyksho.UkeYmd >= ''', @StartReservationDate, '''')
			END
			IF (@EndReservationDate IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND TKD_Yyksho.UkeYmd <= ''', @EndReservationDate, '''')
			END

			-- 予約番号
			IF (@StartReceiptNumber IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND TKD_Yyksho.UkeNo >= ''', @StartReceiptNumber, '''')
			END
			IF (@EndReceiptNumber IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND TKD_Yyksho.UkeNo <= ''', @EndReceiptNumber, '''')
			END

			-- 予約区分
			IF (@StartReservationClassification IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_YoyKbn01.YoyaKbn >= ', @StartReservationClassification)
			END
			IF (@EndReservationClassification IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_YoyKbn01.YoyaKbn <= ', @EndReservationClassification)
			END

			-- 営業担当者
			IF (@StartServicePerson IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Syain01.SyainCd >= ', @StartServicePerson)
			END
			IF (@EndServicePerson IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Syain01.SyainCd <= ', @EndServicePerson)
			END

			-- 営業所
			IF (@StartRegistrationOffice IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Eigyos05.EigyoCd >= ', @StartRegistrationOffice)
			END
			IF (@EndRegistrationOffice IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Eigyos05.EigyoCd <= ', @EndRegistrationOffice)
			END

			-- 入力担当者
			IF (@StartInputPerson IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Syain02.SyainCd >= ', @StartInputPerson)
			END
			IF (@EndInputPerson IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_Syain02.SyainCd <= ', @EndInputPerson)
			END

			-- 得意先コード
			IF (@StartCustomer IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND FORMAT(eVPM_Gyosya02.GyosyaCd,''000'') + FORMAT(eVPM_Tokisk01.TokuiCd,''0000'') + FORMAT(eVPM_TokiSt01.SitenCd,''0000'') >= ''' + @StartCustomer + ''''
			END
			IF (@EndCustomer IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND FORMAT(eVPM_Gyosya02.GyosyaCd,''000'') + FORMAT(eVPM_Tokisk01.TokuiCd,''0000'') + FORMAT(eVPM_TokiSt01.SitenCd,''0000'') <= ''' + @EndCustomer + ''''
			END

			-- 仕入先コード
			IF (@StartSupplier IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND FORMAT(eVPM_Gyosya03.GyosyaCd,''000'') + FORMAT(eVPM_Tokisk04.TokuiCd,''0000'') + FORMAT(eVPM_TokiSt05.SitenCd,''0000'') >= ''' + @StartSupplier + ''''
			END
			IF (@EndSupplier IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND FORMAT(eVPM_Gyosya03.GyosyaCd,''000'') + FORMAT(eVPM_Tokisk04.TokuiCd,''0000'') + FORMAT(eVPM_TokiSt05.SitenCd,''0000'') <= ''' + @EndSupplier + ''''
			END

			-- 団体区分
			IF (@StartGroupClassification IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb17.CodeKbn >= ''' + @StartGroupClassification + ''''
			END
			IF (@EndGroupClassification IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb17.CodeKbn <= ''' + @EndGroupClassification + ''''
			END

			-- 乗客コード
			IF (@StartCustomerTypeClassification IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_JyoKya01.JyoKyakuCd >= ', @StartCustomerTypeClassification)
			END
			IF (@EndCustomerTypeClassification IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_JyoKya01.JyoKyakuCd <= ', @EndCustomerTypeClassification)
			END

			-- 行先
			IF (@StartDestination IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd >= ''' + @StartDestination + ''''
			END
			IF (@EndDestination IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb08.CodeKbn + eVPM_Basyo03.BasyoMapCd <= ''' + @EndDestination + ''''
			END

			-- 配車地
			IF (@StartDispatchPlace IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd >= ''' + @StartDispatchPlace + ''''
			END
			IF (@EndDispatchPlace IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb09.CodeKbn + eVPM_Haichi01.HaiSCd <= ''' + @EndDispatchPlace + ''''
			END

			-- 発生地
			IF (@StartOccurrencePlace IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd >= ''' + @StartOccurrencePlace + ''''
			END
			IF (@EndOccurrencePlace IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb05.CodeKbn + eVPM_Basyo01.BasyoMapCd <= ''' + @EndOccurrencePlace + ''''
			END

			-- エリア
			IF (@StartArea IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd >= ''' + @StartArea + ''''
			END
			IF (@EndArea IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eVPM_CodeKb06.CodeKbn + eVPM_Basyo02.BasyoMapCd <= ''' + @EndArea + ''''
			END

			-- 受付条件
			IF (@StartReceiptCondition IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eTKD_Haisha01.UkeJyKbnCd >= ''' + @StartReceiptCondition + ''''
			END
			IF (@EndReceiptCondition IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ 'AND eTKD_Haisha01.UkeJyKbnCd <= ''' + @EndReceiptCondition + ''''
			END

			-- 車種
			IF (@StartCarType IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
				    +   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_SyaSyu01.SyaSyuCd >=', @StartCarType)
			END
			IF (@EndCarType IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
				    +   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_SyaSyu01.SyaSyuCd <=', @EndCarType)
			END

			-- 車種単価
			IF (@StartCarTypePrice IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
				    +   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_YykSyu01.SyaSyuTan >=', @StartCarTypePrice)
			END								
	        IF (@EndCarTypePrice IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL +
					+   CHAR(13)+CHAR(10)	+ CONCAT('AND eTKD_YykSyu01.SyaSyuTan <=', @EndCarTypePrice)
			END
			--
			IF (@OffSet IS NOT NULL)
			BEGIN
				SET @strSQL = @strSQL
				    +   CHAR(13)+CHAR(10)	+ ' ORDER BY UkeNo, UnkRen '
				    +   CHAR(13)+CHAR(10)	+ CONCAT(' OFFSET ', @OffSet, ' ROWS ')
				    +   CHAR(13)+CHAR(10)	+ CONCAT(' FETCH NEXT ', @Fetch, ' ROWS ONLY')
			END
		EXEC(@strSQL)
		SET	@ROWCOUNT	=	@@ROWCOUNT
     END
RETURN
