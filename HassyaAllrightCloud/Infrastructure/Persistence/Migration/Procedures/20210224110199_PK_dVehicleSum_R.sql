USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicleSum_R]    Script Date: 2021/02/24 10:48:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dVehicle_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data vehicle total list
-- Date			:   2020/08/11
-- Author		:   N.T.Lan.Anh
-- Description	:   Get data for super menu vehicle list with conditions
------------------------------------------------------------
CREATE OR ALTER   PROCEDURE [dbo].[PK_dVehicleSum_R]
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
	)
AS
	BEGIN
WITH eTKD_FutTum00 AS (
	SELECT TKD_FutTum.UkeNo								AS		UkeNo,
		TKD_FutTum.UnkRen								AS		UnkRen,
		TKD_MFutTu.TeiDanNo								AS		TeiDanNo,
		TKD_MFutTu.BunkRen								AS		BunkRen,
		TKD_FutTum.FutTumKbn							AS		FutTumKbn,
		CASE WHEN TKD_FutTum.FutTumKbn = 2 THEN 0
			ELSE VPM_Futai.FutGuiKbn
			END AS FutGuiKbn,
		SUM(TKD_MFutTu.UriGakKin)						AS		UriGakKin_S,
		SUM(TKD_MFutTu.SyaRyoSyo)						AS		SyaRyoSyo_S,
		SUM(TKD_MFutTu.SyaRyoTes)						AS		SyaRyoTes_S
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
		SUM(eTKD_YFutTu10.HaseiKin	)					AS		HaseiKin_S,
		SUM(eTKD_YFutTu10.SyaRyoSyo	)					AS		SyaRyoSyo_S,
		SUM(eTKD_YFutTu10.SyaRyoTes	)					AS		SyaRyoTes_S
	FROM eTKD_YFutTu10
	WHERE eTKD_YFutTu10.JitaFlg	= 0
	GROUP BY eTKD_YFutTu10.UkeNo,
		eTKD_YFutTu10.UnkRen,
		eTKD_YFutTu10.FutTumKbn,
		eTKD_YFutTu10.SeiFutSyu,
		eTKD_YFutTu10.TeiDanNo,
		eTKD_YFutTu10.BunkRen
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

SELECT 
SUM( CASE                                                                                                              
	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoUnc AS BIGINT) 
    WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.SyaRyoUnc AS BIGINT) 
	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaUnc AS BIGINT) 
	END) AS SyaRyoUnc,  		   	                                                                         
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoSyo AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.SyaRyoSyo AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaSyo AS BIGINT)                                
END) AS SyaRyoSyo,                                                                                                 
(SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoUnc AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.SyaRyoUnc AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaUnc AS BIGINT)                                
END) +                                                                                               
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoSyo AS BIGINT)                                 
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.SyaRyoSyo AS BIGINT)                               
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaSyo AS BIGINT)                                
END)) AS TaxIncluded,                                                                                                 
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoTes AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.SyaRyoTes AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaTes AS BIGINT)                                
END) AS SyaRyoTes,                                                                                                 
                                                                                                                         
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoUnc AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaUnc AS BIGINT)                                
END) AS JiSyaRyoUnc,                                                                                               
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoSyo AS BIGINT)                                
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaSyo AS BIGINT)                                 
END) AS JiSyaRyoSyo,                                                                                               
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN CAST(eTKD_Haisha01.SyaRyoTes AS BIGINT)                                 
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN CAST(eTKD_Haisha01.YoushaTes AS BIGINT)                                
END) AS JiSyaRyoTes,                                                                                               
                                                                                                                         
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.YoushaUnc AS BIGINT)                                 
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) AS YoushaUnc,                                                                                                 
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.YoushaSyo AS BIGINT)                                 
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) AS YoushaSyo,                                                                                                 
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN CAST(eTKD_Haisha01.YoushaTes AS BIGINT)                                 
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) AS YoushaTes,                                                                                                 
(SUM( ISNULL(CAST(eTKD_FutTum03.UriGakKin_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum04.UriGakKin_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum05.UriGakKin_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum02.UriGakKin_S AS BIGINT)		,0		)))	 AS CompanyOtherCharge,                
(SUM( ISNULL(CAST(eTKD_FutTum03.SyaRyoSyo_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum04.SyaRyoSyo_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum05.SyaRyoSyo_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum02.SyaRyoSyo_S AS BIGINT)		,0		)))  AS CompanyOtherChargeTax,             
(SUM( ISNULL(CAST(eTKD_FutTum02.SyaRyoTes_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum03.SyaRyoTes_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum04.SyaRyoTes_S AS BIGINT)		,0		))	+                                      
SUM( ISNULL(CAST(eTKD_FutTum05.SyaRyoTes_S AS BIGINT)		,0		))) AS CompanyOtherChargeCommission,       
(SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu04.HaseiKin_S AS BIGINT), 0)                     
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                           
SUM(CASE WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu05.HaseiKin_S AS BIGINT), 0)                     
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                           
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu06.HaseiKin_S AS BIGINT), 0)                     
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                           
    SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu02.HaseiKin_S AS BIGINT), 0)                     
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END)) AS YousyaOtherCharge,                                                                                          
( SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu04.SyaRyoSyo_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                          
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu05.SyaRyoSyo_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                          
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu06.SyaRyoSyo_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                          
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu02.SyaRyoSyo_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END)) AS YousyaOtherChargeTax,                                                                                          
( SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu02.SyaRyoTes_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                          
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu04.SyaRyoTes_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                          
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu05.SyaRyoTes_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END) +                                                                                          
SUM( CASE                                                                                                              
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受自社' THEN 0                                                       
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '自社受傭車' THEN ISNULL(CAST(eTKD_YFutTu06.SyaRyoTes_S AS BIGINT), 0)                    
   	WHEN eTKD_Haisha01.JiTaAtukaiFlg = '他社受自社' THEN 0                                                       
END)) AS YousyaOtherChargeCommission
FROM eTKD_Haisha00 AS eTKD_Haisha01
LEFT JOIN TKD_Unkobi AS	eTKD_Unkobi01 
	ON eTKD_Unkobi01.UkeNo = eTKD_Haisha01.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_Haisha01.UnkRen
	AND eTKD_Unkobi01.SiyoKbn =	1
LEFT JOIN TKD_Yyksho
	ON eTKD_Haisha01.UkeNo = TKD_Yyksho.UkeNo
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

LEFT JOIN VPM_Basyo AS eVPM_Basyo03
	ON eTKD_Haisha01.IkMapCdSeq	= eVPM_Basyo03.BasyoMapCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb08
	ON eVPM_Basyo03.BasyoKenCdSeq = eVPM_CodeKb08.CodeKbnSeq

LEFT JOIN VPM_Haichi AS	eVPM_Haichi01
	ON eTKD_Haisha01.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq
LEFT JOIN VPM_CodeKb AS	eVPM_CodeKb09
	ON eVPM_Haichi01.BunruiCdSeq = eVPM_CodeKb09.CodeKbnSeq
JOIN eTKD_Haisha20
	ON eTKD_Haisha01.UkeNo = eTKD_Haisha20.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_Haisha20.UnkRen
	AND eTKD_Haisha01.TeiDanNo = eTKD_Haisha20.TeiDanNo
	AND eTKD_Haisha01.BunkRen = eTKD_Haisha20.BunkRen

LEFT JOIN TKD_YykSyu AS	eTKD_YykSyu01
	ON eTKD_Haisha01.UkeNo = eTKD_YykSyu01.UkeNo
	AND eTKD_Haisha01.UnkRen = eTKD_YykSyu01.UnkRen
	AND eTKD_Haisha01.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen
	AND eTKD_YykSyu01.SiyoKbn = 1
WHERE
	TKD_Yyksho.SiyoKbn = 1
	AND TKD_Yyksho.YoyaSyu = 1
	AND eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq																																-- ログインユーザの会社のCompanyCdSeq
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
	OPTION (RECOMPILE)
END
RETURN
